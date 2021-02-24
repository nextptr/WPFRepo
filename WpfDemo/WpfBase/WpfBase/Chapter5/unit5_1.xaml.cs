using System.Windows;
using System.Windows.Controls;

namespace WpfBase.Chapter5
{
    /// <summary>
    /// unit5_1.xaml 的交互逻辑
    /// </summary>
    public partial class unit5_1 : UserControl
    {
        protected int eventCounter = 0;
        public unit5_1()
        {
            InitializeComponent();
            this.Tag = "第五章路由事件#事件路由";
            btn_clear.Click += Btn_clear_Click;
            radio_bubble.Click += Radio_btn_Click;
            radio_preview.Click += Radio_btn_Click;
            //附加事件
            stack_panel.AddHandler(Button.ClickEvent, new RoutedEventHandler(DoSomeThing));
        }


        //冒泡路由事件
        private void Btn_clear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            lst_message.Items.Clear();
            eventCounter = 0;
        }
        private void Radio_btn_Click(object sender, RoutedEventArgs e)
        {
            Btn_clear_Click(sender, e);
        }

        //冒泡路由从内层到Root根元素传递
        private void BubbleMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (radio_bubble.IsChecked == true)
            {
                eventCounter++;
                string msg = $"#{eventCounter.ToString()}:\r\n"
                    + $"Sender: {sender.ToString()}\r\n"
                    + $"Source: {e.Source}\r\n"
                    + $"Original Source: {e.OriginalSource}\r\n";
                lst_message.Items.Add(msg);
                e.Handled = (bool)ch_handle.IsChecked; //e.Handled可以终止事件的路由传递
            }
        }
        //隧道路由从最外层元素向内层元素传递，事件由Preview加以区别，如PreviewMouseUp
        private void previewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (radio_bubble.IsChecked == false)
            {
                eventCounter++;
                string msg = $"#{eventCounter.ToString()}:\r\n"
                    + $"Sender: {sender.ToString()}\r\n"
                    + $"Source: {e.Source}\r\n"
                    + $"Original Source: {e.OriginalSource}\r\n";
                lst_message.Items.Add(msg);
                e.Handled = (bool)ch_handle.IsChecked; //e.Handled可以终止事件的路由传递
            }
        }


        //附加事件
        private void DoSomeThing(object sender, System.Windows.RoutedEventArgs e)
        {
            //if (sender == cmd1)
            //{
            //    MessageBox.Show("1111");
            //}
            //Button btn = sender as Button;
            //MessageBox.Show(btn.Name);
            MessageBox.Show("触发按钮事件");

        }
    }
}
