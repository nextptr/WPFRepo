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

namespace WpfBase.Chapter5
{
    /// <summary>
    /// unit5_3.xaml 的交互逻辑
    /// </summary>
    public partial class unit5_3 : UserControl
    {
        public unit5_3()
        {
            InitializeComponent();
            this.Tag = "键盘事件";
            txt_input.PreviewKeyDown += Txt_KeyEvent;
            txt_input.KeyDown += Txt_KeyEvent;
            txt_input.TextInput += Txt_input_TextInput;
            txt_input.PreviewTextInput += Txt_input_PreviewTextInput;
            txt_input.TextChanged += Txt_input_TextChanged;
            txt_input.KeyUp += Txt_KeyEvent;
            btn_clearList.Click += Btn_clearList_Click;
        }
        private void Btn_clearList_Click(object sender, RoutedEventArgs e)
        {
            txt_input.Text = "";
            list_msg.Items.Clear();
            list_stat.Items.Clear();
        }

        //对不可显示数据，如空格等也可以加以判断
        private void Txt_KeyEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.RoutedEvent.Name != "PreviewKeyDown")
            {
                string stat = $"Key:{e.Key}" + "\r\n";
                stat += "IsKeyDown()==" + Keyboard.IsKeyDown(e.Key) + "\r\n";
                stat += "IsKeyUp()==" + Keyboard.IsKeyUp(e.Key) + "\r\n";
                stat += "IsKeyToggled()==" + Keyboard.IsKeyToggled(e.Key) + "\r\n";
                stat += "GetKeyStates()==" + Keyboard.GetKeyStates(e.Key).ToString() + "\r\n";
                list_stat.Items.Insert(0, stat);
            }



            if (chk_igo.IsChecked == true)
            {
                if (e.Key == Key.Space) //屏蔽不可显示的space空格输入
                {
                    e.Handled = true;
                }
                if (e.IsRepeat) //屏蔽由于一直按压按键产生的连续输入
                {
                    e.Handled = true;
                }
            }
            string msg = $"Event:{e.RoutedEvent} Key:{e.Key}";
            list_msg.Items.Insert(0, msg);
        }

        private void Txt_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            string msg = $"Event:{e.RoutedEvent}";
            list_msg.Items.Insert(0, msg);
        }

        //在TextInput的参数e中包含经过处理的，控件即将接收到的文本
        private void Txt_input_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string msg = $"Event:{e.RoutedEvent} Text:{e.Text}";
            list_msg.Items.Insert(0, msg);
        }

        //由可显示按键输入触发，对输入数据进行判断，不现实非数字值
        private void Txt_input_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (chk_igo.IsChecked == true)
            {
                short val;
                if (!Int16.TryParse(e.Text, out val))//屏蔽不能转换成数字的输入
                {
                    e.Handled = true;
                    return;
                }
            }

            string msg = $"Event:{e.RoutedEvent} Text:{e.Text}";
            list_msg.Items.Insert(0, msg);
        }
    }
}
