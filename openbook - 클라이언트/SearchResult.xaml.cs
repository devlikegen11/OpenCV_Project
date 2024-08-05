using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace openbook
{
    public partial class SearchResult : Page
    {
        public SearchResult()
        {
            InitializeComponent();
            string respondata = VideoRecog.video_respon;
            try
            {
                JObject jsonresponse = JObject.Parse(respondata);
                Displaybook(jsonresponse["Info"]);
            }
            catch (JsonReaderException ex)
            {
                MessageBox.Show($"JSON parsing error: {ex.Message}");
            }
        }

        private void Displaybook(JToken book_info)
        {

            var firstBook = book_info.First;
            if (firstBook != null)
            {
                title_tx.Text = firstBook["Title"].ToString();
                author_tx.Text = firstBook["Author"].ToString();
                publication_tx.Text = firstBook["Publication"].ToString();
                ISBN_tx.Text = firstBook["ISBN"].ToString();
                place_tx.Text = firstBook["Place"].ToString();
                string imageUrl = "http://cover.nl.go.kr/" + firstBook["Image"].ToString();
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                    bitmap.EndInit();
                    testimg.Source = bitmap;
                }
            }
            else
            {
                MessageBox.Show("책 정보를 찾을 수 없습니다.");
            }

        }
        private void end_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/STARTPAGE.xaml", UriKind.Relative));
        }
    }
}
