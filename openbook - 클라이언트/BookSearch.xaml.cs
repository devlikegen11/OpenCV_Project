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
    public partial class BookSearch : Page
    {
        public BookSearch()
        {
            InitializeComponent();
            VideoRecog.video_respon = "";
        }
        string respondata = VideoRecog.video_respon;
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            JObject title_set;
            string title_search = Bookname_Search.Text;
            title_set = DJBOOKLIB.DJLIB.title_search(title_search);
            DJBOOKLIB.DJLIB.DataWrite(MainWindow.stream, title_set);

            string response = DJBOOKLIB.DJLIB.Read(MainWindow.stream);
            VideoRecog.video_respon = response;
            NavigationService.Navigate(new Uri("/SearchResult.xaml", UriKind.Relative));
        }

        private void btn_Csharp_Click(object sender, RoutedEventArgs e)
        {
            string c_sharp_search = "9788967356743";
            image_search(c_sharp_search);
        }

        private void btn_TCP_IP_Click(object sender, RoutedEventArgs e)
        {
            string TCP_IP_search = "9788936477196";
            image_search(TCP_IP_search);
        }

        private void btn_C_Click(object sender, RoutedEventArgs e)
        {
            string today_search = "9791190116107";
            image_search(today_search);
        }

        private void btn_MySQL_Click(object sender, RoutedEventArgs e)
        {
            string MySQL_search = "9791157802623";
            image_search(MySQL_search);
        }

        private void btn_learn_MySQL_Click(object sender, RoutedEventArgs e)
        {
            string CODE_search = "9788965135722";
            image_search(CODE_search);
        }

        private void btn_CPP_Click(object sender, RoutedEventArgs e)
        {
            string CS_search = "9791185585819";
            image_search(CS_search);
        }
        private void image_search(string number)
        {
            JObject books;
            string book_ISBN = number;
            books = DJBOOKLIB.DJLIB.title_search(book_ISBN);
            DJBOOKLIB.DJLIB.DataWrite(MainWindow.stream, books);

            string response = DJBOOKLIB.DJLIB.Read(MainWindow.stream);
            VideoRecog.video_respon = response;
            NavigationService.Navigate(new Uri("/SearchResult.xaml", UriKind.Relative));
        }
    }
}
