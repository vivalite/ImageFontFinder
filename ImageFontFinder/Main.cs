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
        Baidu.Aip.Ocr.Ocr client;

        public Main()
        {
            InitializeComponent();

            var APP_ID = "17917707";
            var API_KEY = "34cIesqLIgj2advlwktx0D0K";
            var SECRET_KEY = "GUmlGTeFtIfqctLWLRabiV2aWrliPltB";

            client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;

        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            LoadImage();
        }

        private void LoadImage()
        {
            List<Mat> croppedTexts = new List<Mat>();
            byte[] image = File.ReadAllBytes(@".\test.jpg");

            Mat originalMat = Mat.FromImageData(image, ImreadModes.AnyColor);
            Mat displayMat = originalMat.Clone();

            Dictionary<string, object> options = new Dictionary<string, object>
            {
                {"recognize_granularity", "small"},
                {"detect_direction", "true"},
                {"vertexes_location", "true"},
                {"probability", "true"},
                {"detect_language", "true"}
            };

            JObject resultJson = client.Accurate(image, options);
            //Debug.Print(resultJson.ToString());

            dynamic ocrResult = JsonConvert.DeserializeObject<dynamic>(resultJson.ToString());

            int wordCount = ocrResult.words_result_num;

            for (int i = 0; i < wordCount; i++)
            {
                string blobText = ocrResult.words_result[i].words;
                dynamic chars = ocrResult.words_result[i].chars;

                for (int j = 0; j < chars.Count; j++)
                {
                    string theChar = chars[j]["char"];
                    int width = chars[j].location.width;
                    int top = chars[j].location.top;
                    int left = chars[j].location.left;
                    int height = chars[j].location.height;

                    Debug.Print($"Text: {theChar}, IsCJK? {theChar.Any(x => x.IsChinese())}, W {width}, H {height}, T {top}, L {left} ");

                    if (theChar.Any(x => x.IsChinese()))
                    {
                        Rect cropTextRect = new Rect(left, top, (int)(width * 1.8), (int)(height * 1.2));
                        displayMat.Rectangle(cropTextRect, Scalar.RandomColor(), 2, LineTypes.AntiAlias);

                        Mat croppedText = new Mat(originalMat, cropTextRect);
                        croppedTexts.Add(croppedText);

                        //croppedText.SaveImage("!" + Guid.NewGuid() + ".png");
                    }
                }
            }

            int widthTarget = 80;
            int heightTarget = 80;


            using (Net net = CvDnn.ReadNetFromTensorflow("all_freezed_vgg19_tf18.pb"))
            {

                foreach (Mat croppedText in croppedTexts)
                {
                    // preprocess

                    Mat greyText = croppedText.CvtColor(ColorConversionCodes.BGR2GRAY);

                    Mat textAfterGradMorph = new Mat();
                    Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(2, 2));
                    Cv2.MorphologyEx(greyText, textAfterGradMorph, MorphTypes.Gradient, kernel);

                    Mat textAfterThreshold = new Mat();
                    Cv2.Threshold(greyText, textAfterThreshold, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                    kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(2, 2));
                    Cv2.MorphologyEx(textAfterThreshold, textAfterThreshold, MorphTypes.Open, kernel);
                    Cv2.MorphologyEx(textAfterThreshold, textAfterThreshold, MorphTypes.Close, kernel);




                    //Mat textDistance = new Mat(croppedText.Size(), MatType.CV_32F);
                    //Cv2.DistanceTransform(textAfterThreshold, textDistance, DistanceTypes.L2, DistanceMaskSize.Precise);

                    //Cv2.Normalize(textDistance,textDistance);

                    //Mat textDistanceBw32F = new Mat(croppedText.Size(), MatType.CV_32F);
                    //double stthreshold = 2.0;
                    //Cv2.Threshold(textDistance, textDistanceBw32F, 0.1, 1.0, ThresholdTypes.Binary);

                    //Mat textDistanceBw8U = new Mat(croppedText.Size(), MatType.CV_8UC1);
                    //textDistanceBw32F.ConvertTo(textDistanceBw8U, MatType.CV_8UC1);


                    // resize
                    double scaleW = widthTarget / (double)croppedText.Width;
                    double scaleH = heightTarget / (double)croppedText.Height;
                    double scale = scaleW < scaleH ? scaleW : scaleH;

                    Mat resizedText = new Mat();
                    Cv2.Resize(textAfterThreshold, resizedText, new OpenCvSharp.Size(0, 0), scale, scale,
                        InterpolationFlags.Cubic);

                    resizedText = resizedText.CvtColor(ColorConversionCodes.GRAY2BGR);

                    //Cv2.ImShow("" + Guid.NewGuid(), resizedText);

                    //continue;

                    int classId1;
                    double classProb1;



                    var inputBlob = CvDnn.BlobFromImage(resizedText, 1, new Size(widthTarget, heightTarget));
                    net.SetInput(inputBlob);
                    var prob = net.Forward();
                    GetMaxClass(prob, out classId1, out classProb1);

                    int classId2;
                    double classProb2;

                    Cv2.BitwiseNot(resizedText, resizedText);

                    inputBlob = CvDnn.BlobFromImage(resizedText, 1, new Size(widthTarget, heightTarget));
                    net.SetInput(inputBlob);
                    prob = net.Forward();
                    GetMaxClass(prob, out classId2, out classProb2);

                    Debug.Print(
                        $"ClassID:{GetClassText(classId1)}, classProb:{classProb1} ClassID2:{GetClassText(classId2)}, classProb2:{classProb2}");
                }
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
    }
}
