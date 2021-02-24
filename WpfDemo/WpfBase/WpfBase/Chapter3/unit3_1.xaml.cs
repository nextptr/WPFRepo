using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBase.Chapter3
{
    /// <summary>
    /// unit3_1.xaml 的交互逻辑
    /// </summary>
    public partial class unit3_1 : UserControl
    {
        public unit3_1()
        {
            InitializeComponent();
            this.Tag = "第三章布局器";
        }

        private void HiddenButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            if (btn.IsChecked == true)
            {
                btn_hid.Visibility = Visibility.Hidden;
            }
            else
            {
                btn_hid.Visibility = Visibility.Visible;
            }
        }

        private void CollapsedButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            if (btn.IsChecked == true)
            {
                btn_coll.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_coll.Visibility = Visibility.Visible;
            }
        }
    }
}
