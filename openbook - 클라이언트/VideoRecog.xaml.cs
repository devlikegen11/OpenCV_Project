using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using openbook.cam;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tesseract;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using OpenCvSharp.Extensions;
using System.Windows.Threading;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Web.Caching;

namespace openbook
{
    public partial class VideoRecog : System.Windows.Controls.Page
    {
        VideoCapture cam;
        Mat frame;
        DispatcherTimer timer;
        bool is_initCam, is_initTimer;
        static int a = 0;
        public VideoRecog()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            is_initCam = init_camera();
            is_initTimer = init_Timer(10);

            if (is_initTimer && is_initCam) timer.Start();
        }

        private bool init_Timer(double interval_ms)
        {
            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(interval_ms);
                timer.Tick += new EventHandler(timer_tick);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Timer 초기화 실패: " + ex.Message);
                return false;
            }
        }

        private bool init_camera()
        {
            try
            {
                cam = new VideoCapture(0);
                cam.FrameHeight = (int)cam_1.Height;
                cam.FrameWidth = (int)cam_1.Width;

                frame = new Mat();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("카메라 초기화 실패: " + ex.Message);
                return false;
            }
        }

        public async void timer_tick(object sender, EventArgs e)
        {
            if (cam != null && cam.IsOpened())
            {
                cam.Read(frame);
                if (!frame.Empty())
                {
                    //Cv2.CvtColor(frame, frame, ColorConversionCodes.BGR2GRAY);

                    if (ExVideo(frame))
                    {
                        string isbn = SaveImg(frame);
                        string isbnCode = ISBN_CODE(isbn);
                        //string isbnCode = "9788996094036";      // 테스트용

                        if (!string.IsNullOrEmpty(isbnCode) && isbnCode.Length == 13)
                        {
                            isbn_tx.Text = isbnCode;
                        }

                        await Task.Delay(10);
                    }
                    cam_1.Source = WriteableBitmapConverter.ToWriteableBitmap(frame);
                }
            }
        }

        private bool ExVideo(Mat frame)
        {
            var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.TesseractAndLstm);
            Bitmap bitmap;
            using (MemoryStream ms = frame.ToMemoryStream())
            {
                bitmap = new Bitmap(ms);
            }

            var image = PixConverter.ToPix(bitmap);
            var page = engine.Process(image);
            string code = page.GetText();

            if (code.Contains("ISBN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string SaveImg(Mat frame)
        {
            string save_name = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");
            string file_path = "C:\\Users\\edj94\\source\\repos\\openbook\\images\\" + save_name + ".jpg";
            frame.SaveImage(file_path);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(file_path, UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            ISBN_NUM.Source = bitmap;

            var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.TesseractAndLstm);
            var image = Pix.LoadFromFile(file_path);
            var page = engine.Process(image);
            string code = page.GetText();

            return code;
        }

        public string ISBN_CODE(string code)
        {
            string[] str;

            string isbn = null;
            if (code.Contains("ISBN") || code.Contains("SBN"))
            {
                str = code.Split(new string[] { "SBN" }, StringSplitOptions.RemoveEmptyEntries);
                isbn = str[0];
                isbn = Regex.Replace(isbn, @"[^0-9]", "");
                return isbn;
            }
            return isbn;
        }

        public static string video_respon;
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            JObject video_set;
            string ISBN_num = isbn_tx.Text;
            video_set = DJBOOKLIB.DJLIB.videobject(ISBN_num);
            DJBOOKLIB.DJLIB.DataWrite(MainWindow.stream, video_set);

            string response = DJBOOKLIB.DJLIB.Read(MainWindow.stream);
            video_respon = response;
            if (response != null)
            {
                MessageBox.Show("전송 완료");
            }
            else
            {
                MessageBox.Show("전송 오류");
            }
            NavigationService.Navigate(new Uri("/SearchResult.xaml", UriKind.Relative));
        }
    }
}
