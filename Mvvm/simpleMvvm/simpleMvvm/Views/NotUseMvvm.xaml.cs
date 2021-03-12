using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace simpleMvvm.Views
{
    /// <summary>
    /// NotUseMvvm.xaml 的交互逻辑
    /// </summary>
    public partial class NotUseMvvm : UserControl
    {
        public NotUseMvvm()
        {
            InitializeComponent();
            g1_btn_add.Click += G1_btn_add_Click;
            g1_btn_save.Click += G1_btn_save_Click;

            g2_btn_add.Click += G2_btn_add_Click;
            g2_menu_save.Click += G2_menu_save_Click;
        }
        //文本框输入
        private void G1_btn_add_Click(object sender, RoutedEventArgs e)
        {
            double d1 = 0.0, d2 = 0.0;
            double.TryParse(g1_tb1.Text, out d1);
            double.TryParse(g1_tb2.Text, out d2);
            d1 += d2;
            g1_tb3.Text = d1.ToString();
        }
        private void G1_btn_save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.ShowDialog();
        }


        //滑块输入
        private void G2_btn_add_Click(object sender, RoutedEventArgs e)
        {
            g2_sld3.Value = g2_sld1.Value + g2_sld2.Value;
            g2_lab_sun.Content = "sum:(" + g2_sld3.Value.ToString("F3") + ")";
        }
        private void G2_menu_save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.ShowDialog();
        }
    }
}
