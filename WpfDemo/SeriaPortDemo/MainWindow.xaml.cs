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
            this.btn_AscSend1.Click += Btn_AscSend1_Click;
            btn_AscSend2.Click += Btn_AscSend2_Click;

            connectStatus.DataContext = comSeriaPort;
            btnConnect.Click += BtnConnect_Click;
            comSeriaPort.ReceiveDataEvent += ComSeriaPort_ReceiveDataEvent;
        }

        private void Btn_AscSend2_Click(object sender, RoutedEventArgs e)
        {
            string cmd = txt_AscCommand2.Text + " \r \n";
            comSeriaPort.SendCommand3(cmd);
        }

        private void Btn_AscSend1_Click(object sender, RoutedEventArgs e)
        {
            string cmd = txt_AscCommand1.Text + " \r \n";
            comSeriaPort.SendCommand1(cmd);
        }

        private void ComSeriaPort_ReceiveDataEvent(object sender,object data)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                list_text.Items.Add(data);
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
                comSeriaPort.Open();
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
    }
}
