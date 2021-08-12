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
            btn_AscSend.Click += Btn_AscSend_Click;
            btn_DeftSend.Click += Btn_DeftSend_Click;
            btn_StringSend.Click += Btn_StringSend_Click;
            btn_HexSend2.Click += Btn_HexSend2_Click;
            btn_clean.Click += Btn_clean_Click;

            connectStatus.DataContext = comSeriaPort;
            comSeriaPort.ReceiveDataEvent += ComSeriaPort_ReceiveDataEvent;
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
        private void ComSeriaPort_ReceiveDataEvent(object sender, object data)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                list_text.Items.Insert(0, data);
            }));
        }


        private void Btn_AscSend_Click(object sender, RoutedEventArgs e)
        {
            string cmd = txt_AscCommand1.Text + "\r\n";
            comSeriaPort.SendASCIICommand(cmd);
        }
        private void Btn_DeftSend_Click(object sender, RoutedEventArgs e)
        {
            string cmd = txt_AscCommand2.Text + "\r\n";
            comSeriaPort.SendDefaultCommand(cmd);
        }
        private void Btn_StringSend_Click(object sender, RoutedEventArgs e)
        {
            string cmd = txt_StringCommand.Text;
            comSeriaPort.SendStringCommand(cmd);
        }
        private void Btn_HexSend2_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Btn_clean_Click(object sender, RoutedEventArgs e)
        {
            list_text.Items.Clear();
        }
    }
}
