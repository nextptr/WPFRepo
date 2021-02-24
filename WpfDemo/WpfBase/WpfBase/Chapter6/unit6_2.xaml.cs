using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfBase.Chapter6
{
    /// <summary>
    /// unit6_2.xaml 的交互逻辑
    /// </summary>
    public partial class unit6_2 : UserControl
    {
        public unit6_2()
        {
            InitializeComponent();
            this.Tag = "内容控件";
            btn_Repeat.Click += Btn_Repeat_Click;
            btn_Hover.Click += Btn_Hover_Click;

            btnUpDown.PreviewMouseLeftButtonDown += BtnUpDown_PreviewMouseLeftButtonDown;
            btnUpDown.PreviewMouseLeftButtonUp += BtnUpDown_PreviewMouseLeftButtonUp;

        }

       

        private void BtnUpDown_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ls_event.AddMsg("btn_Down");
        }
        private void BtnUpDown_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ls_event.AddMsg("btn_Up");
        }

        private void Btn_Hover_Click(object sender, RoutedEventArgs e)
        {
            ls_event.AddMsg("悬停触发按钮");
        }
   

        private void Btn_Repeat_Click(object sender, RoutedEventArgs e)
        {
            ls_event.AddMsg("RepeatBtn按住一直触发");
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            ls_event.AddMsg("group:"+btn.GroupName);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox btn = sender as CheckBox;
            ls_event.AddMsg("IsChecked:" + btn.IsChecked);
        }

        private void Run_MouseEnter(object sender, MouseEventArgs e)
        {
            //pop控件不会自动显示必须，主动设置为True才能显示
            //pop控件默认StaysOpen="False"当点击其他地方时，pop控件会自动消失
            //pop控件StaysOpen="True"时，只有显式的赋值为false，pop控件才会消失
            popLink.IsOpen = true;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            popWin.IsOpen = true;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            popWin.IsOpen = false;
        }
    }
}
