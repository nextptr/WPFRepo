using Common;
using Common.TCP;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TCPWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private TCPParameter param;
        private PureTcpClient ClientSocket;
        public MainWindow()
        {
            InitializeComponent();
            btnConnect.Click += BtnConnect_Click;
            btnClean.Click += BtnClean_Click;
            btnSend.Click += BtnSend_Click;

            ClientSocket = new PureTcpClient();
            ClientSocket.Event_RecieveMsg += ClientSocket_Event_RecieveMsg1;

            param = new TCPParameter();
            this.DataContext = param;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            param.Read();
        }

        private void msg(object arg)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                if (ListViwe.Items.Count > 200)
                {
                    ListViwe.Items.Clear();
                }
                ListViewItem item = new ListViewItem();
                item.Content = arg;
                item.Background = Brushes.LawnGreen;
                ListViwe.Items.Add(item);
            }));
        }
        private void ClientSocket_Event_RecieveMsg1(object sender, byte[] arr)
        {
            msg(ASCIIEncoding.ASCII.GetString(arr));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientSocket.Disconnect();
        }

        //连接
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (param == null || param.IP == "" || param.Port <= 0) 
                return;
            if (btnConnect.Content.ToString() == "连接服务器")
            {
                ClientSocket.Connect(param.IP, param.Port);
                btnConnect.Content = "断开连接";
                btnSend.IsEnabled = true;
                param.Write();
            }
            else
            {
                btnConnect.Content = "连接服务器";
                btnSend.IsEnabled = false;
                ClientSocket.Disconnect();
            }
            ClientSocket.RefrashStatus();
            msg(ClientSocket.IsConnected ? "已连接" : "已断开");
        }
   
        //清理
        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            ListViwe.Items.Clear();
        }
        //发送
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string cmd = txtSendMessage.Text;
            if (param.IsCheckCR)
            {
                cmd += '\r';
            }
            if (param.IsCheckLF)
            {
                cmd += '\n';
            }
            ClientSocket.SendCommand(cmd);
        }
    }
}
