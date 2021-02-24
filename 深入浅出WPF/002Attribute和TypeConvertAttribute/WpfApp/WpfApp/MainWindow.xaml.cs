using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
            btn_clear.Click += Btn_clear_Click;
            btn_find.Click += Btn_find_Click;
            btn_cvt.Click += Btn_cvt_Click;
        }


        private void Link_ori_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
            //Process.Start(link.NavigateUri.AbsoluteUri);
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ls_view.Items.Clear();
        }

        private void Btn_find_Click(object sender, RoutedEventArgs e)
        {
            object[] attributes = typeof(Human).GetCustomAttributes(true);
            HelpAttribute attribute = attributes[0] as HelpAttribute;
            ShowMsg(attribute.Info);
        }
        private void Btn_cvt_Click(object sender, RoutedEventArgs e)
        {
            ShowMsg($"human={"string"}:字符串和human类不能隐式直接转换");

            string str = "字符串和human类通过StringToHumanConverter转换";
            StringToHumanConverter convert = new StringToHumanConverter();
            Human human = (Human)convert.ConvertFrom(str);
            ShowMsg(human.Name);

            Human hum = (Human)this.FindResource("human");
            MessageBox.Show(hum.Child.Name);
        }

        private void ShowMsg(string str)
        {
            ls_view.Items.Add(str);
        }
    }

    [Help("这是一个自定义attribute测试")]
    [TypeConverterAttribute(typeof(StringToHumanConverter))]
    public class Human
    {
        public string Name { get; set; }
        public Human Child { get; set; }
    }

    public class HelpAttribute : Attribute
    {
        public HelpAttribute(string info)
        {
            this.Info = info;
        }
        public string Info { get; private set; }
    }

    public class StringToHumanConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                Human h = new Human();
                h.Name = value as string;
                return h;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}
