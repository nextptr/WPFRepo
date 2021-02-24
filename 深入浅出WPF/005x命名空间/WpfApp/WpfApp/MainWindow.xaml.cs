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
        string oriStr = "";
        public MainWindow()
        {
            InitializeComponent();
            //x:FieldModifier
            txt_input.TextChanged += Txt_input_TextChanged;
            oriStr = attachPanel.txt_box1.Text+"\r\n";

            //x:Key
            btn_find.Click += Btn_find_Click;

        }

        //x:FieldModifier
        private void Txt_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            attachPanel.txt_box1.Text = oriStr + txt_input.Text;
        }


        //x:Key
        private void Btn_find_Click(object sender, RoutedEventArgs e)
        {
            txt_view.Text = "txt_view.Text = FindResource(\""+"myString"+").ToString();";
            txt_view.Text = FindResource("myString").ToString();
        }

        //x:static
        public static string winTitle = "山高月小";
        public static string showText = "水落石出";

    }
}
