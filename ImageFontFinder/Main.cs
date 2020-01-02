using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    public sealed partial class Main : Form
    {
        Baidu.Aip.Ocr.Ocr _client;
        List<TextSegmentData> _textSegments = new List<TextSegmentData>();
        Size _imageSize = new Size(1, 1);
        Dictionary<int, string> _classLabel = new Dictionary<int, string>();
        private string[] _classDefination = File.ReadAllLines("classes.csv");
        private string _fontBasePath = @"D:\FontImageGenerator\TextImageGenerator\TextImageGenerator\bin\Release\Fonts\";

        private FontCollections _fontCollections;

        private TextSegmentData _currentTextSegmentData;
        private int _currentFontProbIndex = 0;

        public Main()
        {
            InitializeComponent();

            // these are 
            var APP_ID = "17917707";
            var API_KEY = "34cIesqLIgj2advlwktx0D0K";
            var SECRET_KEY = "GUmlGTeFtIfqctLWLRabiV2aWrliPltB";

            _client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY)
            {
                Timeout = 60000
            };

            // load fonts
            _fontCollections = new FontCollections();

            foreach (string file in Directory.EnumerateFiles(_fontBasePath, "*.*", SearchOption.AllDirectories))
            {
                _fontCollections.AddFont(file, Path.GetFileNameWithoutExtension(file));
            }

            // load class label

            foreach (string line in _classDefination)
            {
                string[] lineContents = line.Split(',');

                int lineNum = -1;
                int.TryParse(lineContents[0], out lineNum);

                if (lineNum >= 0)
                {
                    _classLabel.Add(lineNum, lineContents[1]);
                }

            }

            Text = "智能图片文字字体鉴别 V"+ Application.ProductVersion + "（科研专用）";
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
                        Rect cropCharRect = new Rect(
                            segmentData.TextCharLeft,
                            segmentData.TextCharTop,
                            GetSizeSafe((int)(segmentData.TextCharWidth * 1.5), segmentData.TextCharLeft, originalMat.Width),
                            GetSizeSafe((int)(segmentData.TextCharHeight * 1.2), segmentData.TextCharTop, originalMat.Height)
                            );

                        //displayMat.Rectangle(cropCharRect, Scalar.RandomColor(), 2, LineTypes.AntiAlias); // mark every word

                        Mat croppedChar = new Mat(originalMat, cropCharRect);
                        croppedChars.Add(croppedChar);
                        segmentData.TextCharCroppedMat = croppedChar.Clone();

                        Rect cropTextRect = new Rect(segmentData.TextLineLeft, segmentData.TextLineTop, segmentData.TextLineWidth, segmentData.TextLineHeight);
                        Mat croppedLine = new Mat(originalMat, cropTextRect);
                        segmentData.TextLineCroppedMat = croppedLine.Clone();

                        //croppedChar.SaveImage("!" + DateTime.Now.Ticks + ".png");
                    }

                    _textSegments.Add(segmentData);
                }
            }

            int netInputWidth = 80;
            int netInputHeight = 80;

            using (Net net = CvDnn.ReadNetFromTensorflow(AppDomain.CurrentDomain.BaseDirectory + "all_freezed_vgg19_tf115.pb"))
            {
                foreach (TextSegmentData sgData in _textSegments.Where(x => x.IsCJK).ToArray())
                {
                    // preprocess

                    //sgData.TextCharCroppedMat.SaveImage("!" + DateTime.Now.Ticks + ".png");

                    Mat greyText = sgData.TextCharCroppedMat.CvtColor(ColorConversionCodes.BGR2GRAY);

                    //Mat textAfterThreshold = new Mat();
                    //Cv2.Threshold(greyText, textAfterThreshold, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                    //Mat textAfterMorph = new Mat();
                    //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));
                    //Cv2.MorphologyEx(textAfterThreshold, textAfterMorph, MorphTypes.Open, kernel);
                    //Cv2.MorphologyEx(textAfterMorph, textAfterMorph, MorphTypes.Close, kernel);

                    // resize

                    double scaleW = netInputWidth / (double)sgData.TextCharCroppedMat.Width;
                    double scaleH = netInputHeight / (double)sgData.TextCharCroppedMat.Height;
                    double scale = scaleW < scaleH ? scaleW : scaleH;

                    Mat resizedText = new Mat();
                    Cv2.Resize(greyText, resizedText, new Size(0, 0), scale, scale, InterpolationFlags.Cubic);

                    int padTop = 0;
                    int padBottom = 0;
                    int padLeft = 0;
                    int padRight = 0;
                    if (resizedText.Width < netInputWidth)
                    {
                        padLeft = (netInputWidth - resizedText.Width) / 2;

                        if ((netInputWidth - resizedText.Width) % 2 > 0)
                        {
                            padRight = padLeft + 1;
                        }
                        else
                        {
                            padRight = padLeft;
                        }

                    }
                    else if (resizedText.Height < netInputHeight)
                    {
                        padTop = (netInputHeight - resizedText.Height) / 2;

                        if ((netInputHeight - resizedText.Height) % 2 > 0)
                        {
                            padBottom = padTop + 1;
                        }
                        else
                        {
                            padBottom = padTop;
                        }

                    }

                    resizedText = resizedText.CopyMakeBorder(padTop, padBottom, padLeft, padRight, BorderTypes.Constant, Scalar.White);

                    resizedText = resizedText.CvtColor(ColorConversionCodes.GRAY2BGR); // inferring needs BGR input instead of gray

                    //Cv2.ImShow("" + Guid.NewGuid(), resizedText);
                    //resizedText.SaveImage("!" + DateTime.Now.Ticks + ".png");

                    int classId1;
                    double classProb1;
                    List<CharProbClass> probList;

                    var inputBlob = CvDnn.BlobFromImage(resizedText, 1, new Size(netInputWidth, netInputHeight), new Scalar(104, 117, 123));
                    net.SetInput(inputBlob);
                    var prob = net.Forward();
                    GetMaxClass(prob, out classId1, out classProb1, out probList);
                    sgData.ClassLable = GetClassText(classId1);
                    sgData.ClassProb = classProb1;
                    sgData.ProbClassList = probList;

                    Debug.Print($"Char:{sgData.TextChar},  ClassID:{GetClassText(classId1)}, classProb:{classProb1}");
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
                Dictionary<string, double> fontProbDict = new Dictionary<string, double>();

                foreach (TextSegmentData segmentData in textLine)
                {
                    if (!fontProbDict.ContainsKey(segmentData.ClassLable))
                    {
                        fontProbDict.Add(segmentData.ClassLable, segmentData.ClassProb);
                    }
                    else if (segmentData.ClassProb > fontProbDict[segmentData.ClassLable])
                    {
                        fontProbDict[segmentData.ClassLable] += segmentData.ClassProb;
                    }
                }

                var orderedFontProb = fontProbDict.OrderByDescending(x => x.Value).ToArray();
                Debug.Print($"Text Line: {textLine.FirstOrDefault()?.TextLine}, Font Name: {orderedFontProb[0].Key}");

                foreach (TextSegmentData data in textLine)
                {
                    data.TextLineFont = orderedFontProb[0].Key;
                    data.ProbClassList = textLine.ToList().FirstOrDefault()?.ProbClassList;
                }

                Rect textLineRect = new Rect((int)textLine.FirstOrDefault()?.TextLineLeft, (int)textLine.FirstOrDefault()?.TextLineTop, (int)textLine.FirstOrDefault()?.TextLineWidth, (int)textLine.FirstOrDefault()?.TextLineHeight);
                displayMat.Rectangle(textLineRect, Scalar.RandomColor(), 2, LineTypes.AntiAlias);

            }

            pictureBoxOriginal.Image = displayMat.ToBitmap();
        }

        private int GetSizeSafe(int inputSize, int offset, int totalSize)
        {
            var rtn = inputSize + offset <= totalSize ? inputSize : totalSize - offset;
            return rtn;
        }

        private string GetClassText(int classId)
        {
            return _classLabel.Count > 0 ? _classLabel[classId] : "";
        }

        private void GetMaxClass(Mat probBlob, out int classId, out double classProb, out List<CharProbClass> probList)
        {
            float[] probData = new float[probBlob.Width * probBlob.Height];
            Marshal.Copy(probBlob.Data, probData, 0, probBlob.Width * probBlob.Height);

            probList = new List<CharProbClass>();

            for (int i = 0; i < probData.Length; i++)
            {
                probList.Add(new CharProbClass()
                {
                    ID = i,
                    ClassName = GetClassText(i),
                    Probability = probData[i]
                });
            }

            probList = probList.OrderByDescending(x => x.Probability).ToList();

            // reshape the blob to 1x1000 matrix
            //var probMat = probBlob.Reshape(1, 1);
            Cv2.MinMaxLoc(probBlob, out _, out classProb, out _, out var classNumber);
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
                    TextSegmentData data = dGroup.ToArray().FirstOrDefault();

                    if (data == null)
                    {
                        continue;
                    }

                    if (mouseX >= data.TextLineLeft / resizeFactor && mouseX <= (data.TextLineLeft + data.TextLineWidth) / resizeFactor &&
                        mouseY >= data.TextLineTop / resizeFactor && mouseY <= (data.TextLineTop + data.TextLineHeight) / resizeFactor)
                    {
                        _currentTextSegmentData = data;

                        pictureBoxOriginalCrop.Image = data.TextLineCroppedMat.ToBitmap();

                        _currentFontProbIndex = 0;

                        DisplayFontImageInfo();
                    }
                }

            }

        }

        private void buttonTest_Click(object sender, EventArgs e)
        {

            Mat orgMat = new Mat(AppDomain.CurrentDomain.BaseDirectory + @"TestImage\z7.png", ImreadModes.AnyColor);

            orgMat = orgMat.CvtColor(ColorConversionCodes.BGR2GRAY);
            orgMat = orgMat.CvtColor(ColorConversionCodes.GRAY2BGR);

            Cv2.ImShow("img", orgMat);

            using (Net net = CvDnn.ReadNetFromTensorflow(AppDomain.CurrentDomain.BaseDirectory + "all_freezed_vgg19_tf115.pb"))
            {
                int classId1;
                double classProb1;
                List<CharProbClass> probList;

                var inputBlob = CvDnn.BlobFromImage(orgMat, 1, new Size(80, 80), new Scalar(104, 117, 123));
                net.SetInput(inputBlob);
                var prob = net.Forward();
                GetMaxClass(prob, out classId1, out classProb1, out probList);

                Debug.Print($"ClassID:{GetClassText(classId1)}, classProb:{classProb1}");
            }

        }

        private void buttonUp_Click(object sender, EventArgs e)
        {

            if (_currentFontProbIndex-1<0)
            {
                return;
            }

            _currentFontProbIndex--;
            
            DisplayFontImageInfo();

        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (_currentFontProbIndex+1 > 346 || _currentTextSegmentData== null)
            {
                return;
            }

            _currentFontProbIndex++;
            
            DisplayFontImageInfo();
        }

        private void DisplayFontImageInfo()
        {
            string fontPath = $@"{_fontBasePath}{_currentTextSegmentData.ProbClassList[_currentFontProbIndex].ClassName}\";
            fontPath = Directory.GetFiles(fontPath).FirstOrDefault();

            if (fontPath != null)
            {
                var fontFamily = _fontCollections
                    .FontCollection[_currentTextSegmentData.ProbClassList[_currentFontProbIndex].ClassName].FirstOrDefault()
                    ?.Families.FirstOrDefault();

                //Debug.Print(data.TextLine + "  " + fontFamily);

                if (fontFamily != null)
                {
                    Font font = new Font(fontFamily, 200);

                    pictureBoxGenerated.Image = Utility.DrawText(_currentTextSegmentData.TextLine, font);
                }
            }

            labelFontInfo.Text =
                $@"字体公司：{_currentTextSegmentData.ProbClassList[_currentFontProbIndex].ClassName.Substring(0, _currentTextSegmentData.ProbClassList[_currentFontProbIndex].ClassName.IndexOf("_", StringComparison.Ordinal))}    字体文件名：{_currentTextSegmentData.ProbClassList[_currentFontProbIndex].ClassName}     相似度:{_currentTextSegmentData.ProbClassList[_currentFontProbIndex].Probability:P2}";
            labelCurrentFontNum.Text = _currentFontProbIndex.ToString();
        }
    }

    public class FontCollections : IDisposable
    {
        private List<KeyValuePair<string, PrivateFontCollection>> _privateFontCollection = new List<KeyValuePair<string, PrivateFontCollection>>();

        public ILookup<string, PrivateFontCollection> FontCollection => _privateFontCollection.ToLookup((i) => i.Key, (i) => i.Value);

        public void AddFont(string fullFileName, string fontGroupName)
        {
            AddFont(File.ReadAllBytes(fullFileName), fontGroupName);
        }

        public void AddFont(byte[] fontBytes, string fontGroupName)
        {
            var handle = GCHandle.Alloc(fontBytes, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();
            try
            {
                PrivateFontCollection pfc = new PrivateFontCollection();
                pfc.AddMemoryFont(pointer, fontBytes.Length);

                _privateFontCollection.Add(new KeyValuePair<string, PrivateFontCollection>(fontGroupName, pfc));
            }
            finally
            {
                handle.Free();
            }

        }

        public void Clear()
        {
            _privateFontCollection.Clear();
        }

        public void Dispose()
        {
            foreach (KeyValuePair<string, PrivateFontCollection> pair in _privateFontCollection)
            {
                pair.Value.Dispose();
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
        public string ClassLable { get; set; }
        public double ClassProb { get; set; }
        public string TextLineFont { get; set; }
        public List<CharProbClass> ProbClassList { get; set; }

    }

    public class CharProbClass
    {
        public int ID { get; set; }
        public string ClassName { get; set; }
        public float Probability { get; set; }
    }
}
