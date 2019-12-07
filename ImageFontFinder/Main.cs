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

			double widthTarget = 80;
			double heightTarget = 80;

			foreach (Mat croppedText in croppedTexts)
			{
				double scaleW = widthTarget / croppedText.Width;
				double scaleH = heightTarget / croppedText.Height;
				double scale = scaleW < scaleH ? scaleW : scaleH;

				Mat resizedText = new Mat();
				Cv2.Resize(croppedText, resizedText, new OpenCvSharp.Size(0, 0), scale, scale, InterpolationFlags.Lanczos4);

				//Cv2.ImShow("" + Guid.NewGuid().ToString(), resizedText);

				using (Net net = CvDnn.ReadNetFromTensorflow("all_freezed_vgg19_tf18.pb"))
				{
					var inputBlob = CvDnn.BlobFromImage(croppedText, 1, new Size(widthTarget, heightTarget), new Scalar(104, 117, 123));
					net.SetInput(inputBlob);
					var prob = net.Forward();
					GetMaxClass(prob, out int classId, out double classProb);



					Debug.Print($"ClassID:{GetClassText(classId)}, classProb:{classProb}");


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
