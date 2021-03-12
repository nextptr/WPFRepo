using Common.TCP;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TCPSocketClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private tcpSocketClient ClientSocket;
        public MainWindow()
        {
            InitializeComponent();
            btnConnect.Click += BtnConnect_Click;
            btnRegister.Click += BtnRegister_Click;
            btnClean.Click += BtnClean_Click;
            btnSend.Click += BtnSend_Click;

            ClientSocket = new tcpSocketClient();
            ClientSocket.Event_RecieveMsg += ClientSocket_Event_RecieveMsg1;
            this.DataContext = ClientSocket;
        }
        private void ClientSocket_Event_RecieveMsg1(object sender, object e)
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientSocket.Disconnect();
        }

        //连接
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (btnConnect.Content.ToString() == "连接服务器")
            {
                string ip = labIpAdd.Text;
                int port = int.Parse(labPort.Text);
                ClientSocket.Connect(ip, port);

                btnConnect.Content = "断开连接";
                btnSend.IsEnabled = true;
            }
            else
            {
                btnConnect.Content = "连接服务器";
                btnSend.IsEnabled = false;
                ClientSocket.Disconnect();
            }
        }
        //注册
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            ClientSocket.RegistUserName();
        }
        //清理
        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            ListViwe.Items.Clear();
        }
        //发送
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            ClientSocket.SendMessage(txtSendMessage.Text);
            txtSendMessage.Clear();
        }
    }
}
