using Common;
using Common.UDP;
using System;
using System.Windows;
using System.Windows.Controls;

namespace UDPSocketDemo
{
    /// <summary>
    /// Server.xaml 的交互逻辑
    /// </summary>
    public partial class Server : UserControl
    {
        CommonUdpServer serv = null;
        public Server()
        {
            InitializeComponent();           
            btn_Connect.Click += Btn_Connect_Click;
            btn_Send.Click += Btn_Send_Click;
        }
        private void Btn_Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serv.SendMsg(txt_box.Text);
                txt_box.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Serv_ReceiveMsgEvent(string e)
        {
            ls.Dispatcher.Invoke(new Action(() =>
            {
                ListViewItem item = new ListViewItem();
                item.Content = e;
                ls.Items.Insert(0, item);
            }));
        }
        private void Btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (serv == null)
                {
                    int port = -1;
                    int.TryParse(txt_port.Text.ToLower(), out port);
                    if (port == -1)
                    {
                        MessageBox.Show("请输入port");
                        return;
                    }
                    serv = new CommonUdpServer(port);
                }

                string str = btn_Connect.Content.ToString();
                if (str == "开始监听")
                {
                    btn_Connect.Content = "停止监听";
                    lab_ip.Content = "IP:" + serv.IP_Address;//"IP:127.0.1";
                    serv.Start();
                    serv.ReceiveMsgEvent += Serv_ReceiveMsgEvent;
                }
                else
                {
                    btn_Connect.Content = "开始监听";
                    serv.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Exit()
        {
            if (serv != null)
            {
                serv.Exit();
            }
        }
    }
}
