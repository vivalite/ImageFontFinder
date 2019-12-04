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
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;


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
				Cv2.Resize(croppedText, resizedText, new OpenCvSharp.Size(0, 0), scale, scale);

				Cv2.ImShow("" + Guid.NewGuid().ToString(), resizedText);

				//using (Mat net = CvDnn.ReadTensorFromONNX("vgg19.onnx"))
				//{

				//}

			}



			pictureBoxOriginal.Image = displayMat.ToBitmap();
		}
	}
}
