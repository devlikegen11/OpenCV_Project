using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace openbook
{

    public partial class STARTPAGE : Page
    {
        public STARTPAGE()
        {
            InitializeComponent();
        }

        private void btn_BookSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BookSearch.xaml", UriKind.Relative));
        }

        private void btn_VideoRecog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/VideoRecog.xaml", UriKind.Relative));
        }

        private void btn_ImageSelect_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ImageSelect.xaml", UriKind.Relative));
        }
    }
}
