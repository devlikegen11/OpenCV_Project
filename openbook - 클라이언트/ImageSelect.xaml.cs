using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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
namespace openbook
{
    public partial class ImageSelect : Page
    {
        public static int general;
        public static int Philosophy;
        public static int Literature;
        public static int Art;
        public static int social_science;
        public static int technological_science;
        public SeriesCollection SeriesCollection1 { get; set; }
        public ChartValues<ObservableValue> Values { get; set; }
        public string[] XLabel { get; set; }
        public Func<double, string> Formatter { get; set; }
        public ImageSelect()
        {
            InitializeComponent();
            set_views();
            SeriesCollection1 = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "총류",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(general) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "철학",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Philosophy) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "문학",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Literature) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "예술",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Art) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "사회과학",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(social_science) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "기술과학",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(technological_science) },
                    DataLabels = true
                }
            };

            XLabel = new[] { "슈뢰딩거", "방관시대", "아프다", "차별주의자", "약국", "음악들", "TCP/IP" };
            DataContext = this;
        }

        private void set_views()
        {
            JObject view_set;
            view_set = DJBOOKLIB.DJLIB.bookviewwws();
            DJBOOKLIB.DJLIB.DataWrite(MainWindow.stream, view_set); // 프로토콜 30 전송

            string view_return = DJBOOKLIB.DJLIB.Read(MainWindow.stream);
            JObject view_response = JObject.Parse(view_return);
            view_grap(view_response["VIEWS"]);
        }

        private void view_grap(JToken view_info)
        {
            Values = new ChartValues<ObservableValue>();
            int count;
            string category;
            string img;
            string book_name;
            int num = 0;
            if (view_info != null)
            {
                foreach (var view_count in view_info)
                {
                    ++num;
                    book_name = view_count["title"].ToString();
                    category = view_count["cat"].ToString();
                    img = "http://cover.nl.go.kr/" + view_count["image"].ToString();
                    count = (int)view_count["count"];

                    if (category == "총류")
                    {
                        general += count;
                    }
                    else if (category == "철학")
                    {
                        Philosophy += count;
                    }
                    else if (category == "문학")
                    {
                        Literature += count;
                    }
                    else if (category == "예술")
                    {
                        Art += count;
                    }
                    else if (category == "사회과학")
                    {
                        social_science += count;
                    }
                    else if (category == "기술과학")
                    {
                        technological_science += count;
                    }
                    Values.Add(new ObservableValue(count));
                    if (num == 1)
                    {
                        if (!string.IsNullOrEmpty(img))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(img, UriKind.Absolute);
                            bitmap.EndInit();
                            first.Source = bitmap;
                        }
                    }
                    if (num == 2)
                    {
                        if (!string.IsNullOrEmpty(img))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(img, UriKind.Absolute);
                            bitmap.EndInit();
                            second.Source = bitmap;
                        }
                    }
                    if (num == 3)
                    {
                        if (!string.IsNullOrEmpty(img))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(img, UriKind.Absolute);
                            bitmap.EndInit();
                            third.Source = bitmap;
                        }
                    }
                }
            } 
   
            else
            {
                MessageBox.Show("책 정보를 찾을 수 없습니다.");
            }

        }

            private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/STARTPAGE.xaml", UriKind.Relative));
        }
    }
}
