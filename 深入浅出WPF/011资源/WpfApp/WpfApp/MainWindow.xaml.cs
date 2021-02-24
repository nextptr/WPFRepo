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

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btn_findResource.Click += Btn_findResource_Click;
            btn_updata.Click += Btn_updata_Click;

            //使用二进制的字符串资源
            lab_hexStr.Content = Properties.Resources.Password;
        }

        private void Btn_findResource_Click(object sender, RoutedEventArgs e)
        {
            lab_find.Content = FindResource("strResource");
        }

        private void Btn_updata_Click(object sender, RoutedEventArgs e)
        {
            this.Resources["res1"] = new TextBlock() { Text = "天涯共此时" };
            this.Resources["res2"] = new TextBlock() { Text = "天涯共此时" };
        }
    }
}
