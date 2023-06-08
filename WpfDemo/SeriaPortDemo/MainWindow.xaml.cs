using Common;
using Common.ComPort;
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

namespace SeriaPortDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ComSeriaPort comSeriaPort = new ComSeriaPort();
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            btnConnect.Click += BtnConnect_Click;
            btn_clean.Click += Btn_clean_Click;

            connectStatus.DataContext = comSeriaPort;
            comSeriaPort.ReceiveDataEvent += ComSeriaPort_ReceiveDataEvent;
        }
        private void msg(object obj)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                list_text.Items.Insert(0, obj);
            }));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                ComPort.Items.Clear();
                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {
                    ComPort.Items.Add(s);
                }
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (comSeriaPort.ComConnected == true)
            {
                comSeriaPort.Close();
            }
        }
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (btnConnect.IsChecked == true)
            {
                btnConnect.Content = "断开";
                if (comSeriaPort.ComConnected == true)
                {
                    comSeriaPort.Close();
                }
                comSeriaPort.ComBaud = ComSeriaPort.GetBaud(ComBaud.SelectedIndex);
                comSeriaPort.ComName = ComPort.SelectedItem.ToString();
                comSeriaPort.OpenNoneParity();
            }
            else
            {
                btnConnect.Content = "连接";
                if (comSeriaPort.ComConnected == true)
                {
                    comSeriaPort.Close();
                }
            }
        }
        private void ComSeriaPort_ReceiveDataEvent(object sender, byte[] data)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                string txt = "";
                string tmp = "";

                if (CheckHEX.IsChecked == true)
                {
                    txt = ComMath.ByteArryToHex16String(data);
                  
                }
                else
                {
                    txt = ASCIIEncoding.ASCII.GetString(data);
                }

                for (int i = 0; i < txt.Length; i++)
                {
                    tmp += txt[i] + " ";
                }
                msg(tmp);
            }));
        }


        public void Btn_Send_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            string cmd = txt_Command.Text;
            if (CheckCL.IsChecked == true)
            {
                cmd += "\r\n";
            }
            switch (btn.Tag.ToString())
            {
                case "TestCom":
                    TestConnect();
                    break;
                case "AscSend":
                    comSeriaPort.SendASCIICommand(cmd);
                    break;
                case "DeftSend":
                    comSeriaPort.SendDefaultCommand(cmd);
                    break;
                case "StringSend":
                    comSeriaPort.SendStringCommand(cmd);
                    break;
                case "HexSend":
                    comSeriaPort.SendHexCommand(cmd);
                    break;
                default:
                    break;
            }
            if (cmd == "")
                return;

        }

        private void TestConnect()
        {
            List<string> lsTmp = new List<string>();
            for (int i = 0; i < ComPort.Items.Count; i++)
            {
                if (comSeriaPort.TestComConnect(ComPort.Items[i].ToString()))
                {
                    lsTmp.Add(ComPort.Items[i].ToString());
                }
            }
            ComPort.Items.Clear();
            foreach (var item in lsTmp)
            {
                ComPort.Items.Add(item);
            }
            msg("测试com连接完成");
        }

        private void Btn_clean_Click(object sender, RoutedEventArgs e)
        {
            list_text.Items.Clear();
        }
    }
}
