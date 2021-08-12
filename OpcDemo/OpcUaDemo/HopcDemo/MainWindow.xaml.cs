using Hylasoft.Opc.Ua;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HopcDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        UaClient uaClient = null;
        private ViewEntry viewEntry = new ViewEntry();
        public MainWindow()
        {
            InitializeComponent();
            btnConnect.Click += BtnConnect_Click;
            btnDisConnect.Click += BtnDisConnect_Click;
            this.DataContext = viewEntry;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var options = new UaClientOptions
            //{
            //    UserIdentity = new Opc.Ua.UserIdentity("L2550M0256", "070F367")
            //};
            uaClient = new UaClient(new Uri("opc.tcp://192.0.2.1:4840"));
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                uaClient?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uaClient?.Dispose();
                viewEntry.LsData.Add("断开连接");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uaClient.Connect();
                viewEntry.LsData.Add("连接成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;
            string index = btn.Tag.ToString().Trim("tg".ToCharArray());
            int id = int.Parse(index);
            switch (id)
            {
                case 1:
                    read("Setting.CDI_PFPenable");
                    break;
                case 2:
                    //write("Activation.MPI-Slot03.Input.Word00.PILOT_LASER_ON", true);
                    write("Production.MPI-Slot03.Input.Word00.PILOT_LASER_ON", true);
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                default:
                    break;
            }
        }

        private void read(string path)
        {
            try
            {
                var dat = uaClient.Read<bool>("ns=2;s=" + path);
                viewEntry.LsData.Add(dat.Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void write<T>(string path, T val)
        {
            try
            {
                uaClient.Write("ns=2;s=" + path, val);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
    public class ViewEntry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string txtInput;
        public string TxtInput
        {
            get
            {
                return txtInput;
            }
            set
            {
                txtInput = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TxtInput)));
            }
        }

        private ObservableCollection<string> lsData;
        public ObservableCollection<string> LsData
        {
            get
            {
                return lsData;
            }
            set
            {
                lsData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LsData)));
            }
        }

        public ViewEntry()
        {
            LsData = new ObservableCollection<string>();
        }
    }
}
