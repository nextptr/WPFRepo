using Common.TCP;
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

namespace TCPSocketServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int port;
        static bool islisten = false;
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += Window_Closing;
            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;
            btnClean.Click += BtnClean_Click;

            this.DataContext = TcpServerManager.Instance;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TcpServerManager.Instance.StopListen();
        }

        //监听
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (islisten == false)
            {
                islisten = true;
                port = int.Parse(labPort.Text);

                TcpServerManager.Instance.StartListen(port);
                TcpServerManager.Instance.Event_ReceiveMsg += Instance_Event_ReceiveMsg;
                btnConnect.Content = "停止服务";
            }
            else
            {
                btnConnect.Content = "开启服务";
                TcpServerManager.Instance.StopListen();
                islisten = false;
            }
        }

        private void Instance_Event_ReceiveMsg(object sender, object e)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                if (ListViwe.Items.Count > 200)
                {
                    ListViwe.Items.Clear();
                }
                ListViewItem item = new ListViewItem();
                item.Content = e;
                item.Background = Brushes.LawnGreen;
                ListViwe.Items.Add(item);
            }));
        }

        //群发
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string dataTime = DateTime.Now.ToString("yyyy/MM/dd/HH:mm ");
            string sendstr = txtSendMessage.Text;
            TcpServerManager.Instance.TcpSocketServerProtocol.SendEverSocketMessage(sendstr);
            ListViewItem item = new ListViewItem();
            item.Content = sendstr;
            item.Background = Brushes.LawnGreen;
            ListViwe.Items.Add(item);
            txtSendMessage.Clear();
        }
        //清屏
        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            ListViwe.Items.Clear();
        }
    }
}
