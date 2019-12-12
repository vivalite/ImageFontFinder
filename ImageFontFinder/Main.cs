using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using Size = OpenCvSharp.Size;

namespace ImageFontFinder
{
    public partial class Main : Form
    {
        Baidu.Aip.Ocr.Ocr _client;
        List<TextSegmentData> _textSegments = new List<TextSegmentData>();
        Size _imageSize = new Size(1, 1);


        public Main()
        {
            InitializeComponent();

            var APP_ID = "17917707";
            var API_KEY = "34cIesqLIgj2advlwktx0D0K";
            var SECRET_KEY = "GUmlGTeFtIfqctLWLRabiV2aWrliPltB";

            _client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY)
            {
                Timeout = 60000
            };

        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff", 
                ShowHelp = true
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                LoadImage(fileDialog.FileName);
            }
        }

        private void LoadImage(string filePath)
        {
            _textSegments.Clear();

            List<Mat> croppedChars = new List<Mat>();
            byte[] image = File.ReadAllBytes(filePath);

            Mat originalMat = Mat.FromImageData(image, ImreadModes.AnyColor);
            Mat displayMat = originalMat.Clone();

            _imageSize = originalMat.Size();

            Dictionary<string, object> options = new Dictionary<string, object>
            {
                {"recognize_granularity", "small"},
                {"detect_direction", "true"},
                {"vertexes_location", "true"},
                {"probability", "true"},
                {"detect_language", "true"}
            };

            JObject resultJson = _client.Accurate(image, options); // OCR accurate
            //Debug.Print(resultJson.ToString());

            dynamic ocrResult = JsonConvert.DeserializeObject<dynamic>(resultJson.ToString());

            int wordCount = ocrResult.words_result_num;

            for (int i = 0; i < wordCount; i++)
            {
                dynamic chars = ocrResult.words_result[i].chars;

                for (int j = 0; j < chars.Count; j++)
                {
                    TextSegmentData segmentData = new TextSegmentData
                    {
                        TextLine = ocrResult.words_result[i].words,
                        TextLineWidth = ocrResult.words_result[i].location.width,
                        TextLineTop = ocrResult.words_result[i].location.top,
                        TextLineLeft = ocrResult.words_result[i].location.left,
                        TextLineHeight = ocrResult.words_result[i].location.height,
                        TextChar = chars[j]["char"],
                        TextCharWidth = chars[j].location.width,
                        TextCharTop = chars[j].location.top,
                        TextCharLeft = chars[j].location.left,
                        TextCharHeight = chars[j].location.height
                    };
                    segmentData.IsCJK = segmentData.TextChar.Any(x => x.IsChinese());

                    Debug.Print($"Text: {segmentData.TextChar}, IsCJK? {segmentData.IsCJK}, W {segmentData.TextCharWidth}, H {segmentData.TextCharHeight}, T {segmentData.TextCharTop}, L {segmentData.TextCharLeft} ");

                    if (segmentData.IsCJK)
                    {
                        Rect cropCharRect = new Rect(segmentData.TextCharLeft, segmentData.TextCharTop, (int)(segmentData.TextCharWidth * 2), (int)(segmentData.TextCharHeight * 1.2));
                        //displayMat.Rectangle(cropTextRect, Scalar.RandomColor(), 2, LineTypes.AntiAlias); // mark every word

                        Mat croppedChar = new Mat(originalMat, cropCharRect);
                        croppedChars.Add(croppedChar);
                        segmentData.TextCharCroppedMat = croppedChar.Clone();

                        Rect cropTextRect = new Rect(segmentData.TextLineLeft,segmentData.TextLineTop, segmentData.TextLineWidth, segmentData.TextLineHeight);
                        Mat croppedLine = new Mat(originalMat, cropTextRect);
                        segmentData.TextLineCroppedMat = croppedLine.Clone();

                        //croppedText.SaveImage("!" + Guid.NewGuid() + ".png");
                    }

                    _textSegments.Add(segmentData);
                }
            }

            int netInputWidth = 80;
            int netInputHeight = 80;

            using (Net net = CvDnn.ReadNetFromTensorflow("all_freezed_vgg19_tf18.pb"))
            {
                foreach (TextSegmentData sgData in _textSegments.Where(x => x.IsCJK).ToArray())
                {
                    // preprocess

                    Mat greyText = sgData.TextCharCroppedMat.CvtColor(ColorConversionCodes.BGR2GRAY);

                    Mat textAfterGradMorph = new Mat();
                    Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(2, 2));
                    Cv2.MorphologyEx(greyText, textAfterGradMorph, MorphTypes.Gradient, kernel);

                    Mat textAfterThreshold = new Mat();
                    Cv2.Threshold(greyText, textAfterThreshold, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                    Mat textAfterMorph = new Mat();
                    kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(2, 2));
                    Cv2.MorphologyEx(textAfterThreshold, textAfterMorph, MorphTypes.Open, kernel);
                    Cv2.MorphologyEx(textAfterMorph, textAfterMorph, MorphTypes.Close, kernel);

                    // resize

                    double scaleW = netInputWidth / (double)sgData.TextCharCroppedMat.Width;
                    double scaleH = netInputHeight / (double)sgData.TextCharCroppedMat.Height;
                    double scale = scaleW < scaleH ? scaleW : scaleH;

                    Mat resizedText = new Mat();
                    Cv2.Resize(textAfterMorph, resizedText, new Size(0, 0), scale, scale, InterpolationFlags.Cubic);

                    resizedText = resizedText.CvtColor(ColorConversionCodes.GRAY2BGR);

                    //Cv2.ImShow("" + Guid.NewGuid(), resizedText);

                    int classId1;
                    double classProb1;

                    var inputBlob = CvDnn.BlobFromImage(resizedText, 1, new Size(netInputWidth, netInputHeight));
                    net.SetInput(inputBlob);
                    var prob = net.Forward();
                    GetMaxClass(prob, out classId1, out classProb1);
                    sgData.ClassLable1 = GetClassText(classId1);
                    sgData.ClassProb1 = classProb1;

                    int classId2;
                    double classProb2;

                    Cv2.BitwiseNot(resizedText, resizedText);

                    inputBlob = CvDnn.BlobFromImage(resizedText, 1, new Size(netInputWidth, netInputHeight));
                    net.SetInput(inputBlob);
                    prob = net.Forward();
                    GetMaxClass(prob, out classId2, out classProb2);
                    sgData.ClassLable2 = GetClassText(classId2);
                    sgData.ClassProb2 = classProb2;

                    Debug.Print($"ClassID:{GetClassText(classId1)}, classProb:{classProb1} ClassID2:{GetClassText(classId2)}, classProb2:{classProb2}");
                }
            }

            // done image processing, calculating

            var groupedTextLines = _textSegments.Where(x => x.IsCJK).GroupBy(
                x => new
                {
                    x.TextLineWidth,
                    x.TextLineTop,
                    x.TextLineLeft,
                    x.TextLineHeight
                }).ToArray();


            foreach (var textLine in groupedTextLines)
            {
                Dictionary<string, int> fontListFreq = new Dictionary<string, int>();

                foreach (TextSegmentData data in textLine)
                {
                    if (!fontListFreq.ContainsKey(data.ClassLable1))
                    {
                        fontListFreq.Add(data.ClassLable1, 1);
                    }
                    else
                    {
                        fontListFreq[data.ClassLable1]++;
                    }

                    if (!fontListFreq.ContainsKey(data.ClassLable2))
                    {
                        fontListFreq.Add(data.ClassLable2, 1);
                    }
                    else
                    {
                        fontListFreq[data.ClassLable2]++;
                    }

                }

                var orderedFontOccurenceList = fontListFreq.OrderByDescending(x => x.Value).ToArray();

                if (orderedFontOccurenceList.Length > 1)
                {
                    if (orderedFontOccurenceList[0].Value > orderedFontOccurenceList[1].Value)
                    {
                        // significant font found
                        Debug.Print($"Text Line: {textLine.FirstOrDefault()?.TextLine}, Font Name: {orderedFontOccurenceList[0].Key}");
                    }
                    else
                    {
                        // no significant font found, take the highest probability
                        Dictionary<string, double> fontListProb = new Dictionary<string, double>();

                        foreach (TextSegmentData data in textLine)
                        {
                            if (!fontListProb.ContainsKey(data.ClassLable1))
                            {
                                fontListProb.Add(data.ClassLable1, data.ClassProb1);
                            }
                            else
                            {
                                fontListProb[data.ClassLable1] += data.ClassProb1;
                            }

                            if (!fontListProb.ContainsKey(data.ClassLable2))
                            {
                                fontListProb.Add(data.ClassLable2, data.ClassProb2);
                            }
                            else
                            {
                                fontListProb[data.ClassLable2] += data.ClassProb2;
                            }
                        }

                        var orderedFontProbList = fontListProb.OrderByDescending(x => x.Value).ToArray();

                        Debug.Print($"Text Line: {textLine.FirstOrDefault()?.TextLine}, Font Name: {orderedFontProbList[0].Key}");

                    }
                }

                Rect textLineRect = new Rect((int)textLine.FirstOrDefault()?.TextLineLeft, (int)textLine.FirstOrDefault()?.TextLineTop, (int)textLine.FirstOrDefault()?.TextLineWidth, (int)textLine.FirstOrDefault()?.TextLineHeight);
                displayMat.Rectangle(textLineRect, Scalar.RandomColor(), 2, LineTypes.AntiAlias);

            }

            pictureBoxOriginal.Image = displayMat.ToBitmap();
        }

        Dictionary<int, string> classLable = new Dictionary<int, string>();
        private string GetClassText(int classId)
        {
            if (classLable.Count == 0)
            {
                foreach (string line in File.ReadAllLines("classes.csv"))
                {
                    string[] lineContents = line.Split(',');

                    int lineNum = -1;
                    int.TryParse(lineContents[0], out lineNum);

                    if (lineNum >= 0)
                    {
                        classLable.Add(lineNum, lineContents[1]);
                    }

                }

            }

            return classLable.Count > 0 ? classLable[classId] : "";
        }

        private static void GetMaxClass(Mat probBlob, out int classId, out double classProb)
        {
            // reshape the blob to 1x1000 matrix
            var probMat = probBlob.Reshape(1, 1);
            Cv2.MinMaxLoc(probMat, out _, out classProb, out _, out var classNumber);
            classId = classNumber.X;
        }

        private void pictureBoxOriginal_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (_textSegments.Count > 0 && _imageSize.Width > 1)
            {
                double wfactor = (double)_imageSize.Width / pictureBoxOriginal.ClientSize.Width;
                double hfactor = (double)_imageSize.Height / pictureBoxOriginal.ClientSize.Height;
                double resizeFactor = Math.Max(wfactor, hfactor);
                Size imageSize = new Size((int)(_imageSize.Width / resizeFactor), (int)(_imageSize.Height / resizeFactor));

                double xOffset = pictureBoxOriginal.ClientSize.Width / 2 - imageSize.Width / 2;
                double yOffset = pictureBoxOriginal.ClientSize.Height / 2 - imageSize.Height / 2;

                double mouseX = me.X - xOffset;
                double mouseY = me.Y - yOffset;

                foreach (var dGroup in _textSegments.Where(x => x.IsCJK).GroupBy(
                    x => new
                    {
                        x.TextLineWidth,
                        x.TextLineTop,
                        x.TextLineLeft,
                        x.TextLineHeight
                    }))
                {
                    var data = dGroup.ToArray().FirstOrDefault();

                    if (data == null)
                    {
                        continue;
                    }

                    if (mouseX >= data.TextLineLeft / resizeFactor && mouseX <= (data.TextLineLeft + data.TextLineWidth) / resizeFactor &&
                        mouseY >= data.TextLineTop / resizeFactor && mouseY <= (data.TextLineTop + data.TextLineHeight) / resizeFactor)
                    {
                        Debug.Print("===========>" + data.TextLine);
                    }
                }

            }

        }
    }

    public class TextSegmentData
    {
        public string TextLine { get; set; }
        public Mat TextLineCroppedMat { get; set; }
        public int TextLineTop { get; set; }
        public int TextLineLeft { get; set; }
        public int TextLineWidth { get; set; }
        public int TextLineHeight { get; set; }
        public string TextChar { get; set; }
        public int TextCharTop { get; set; }
        public int TextCharLeft { get; set; }
        public int TextCharWidth { get; set; }
        public int TextCharHeight { get; set; }
        public bool IsCJK { get; set; }
        public Mat TextCharCroppedMat { get; set; }
        public string ClassLable1 { get; set; }
        public double ClassProb1 { get; set; }
        public string ClassLable2 { get; set; }
        public double ClassProb2 { get; set; }

    }
}
