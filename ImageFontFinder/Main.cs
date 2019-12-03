using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            var image = File.ReadAllBytes(@".\test.jpg");
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.Accurate(image);
            Debug.Print(result.ToString());
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"recognize_granularity", "small"},
                {"detect_direction", "true"},
                {"vertexes_location", "true"},
                {"probability", "true"}
            };
            // 带参数调用通用文字识别, 图片参数为本地图片
            result = client.Accurate(image, options);
            Debug.Print(result.ToString());
        }
    }
}
