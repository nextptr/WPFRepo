using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private LaserDevice laserDevice = new LaserDevice("192.0.2.1:4840", "192.0.2.1", "000431D");
        private ViewEntry viewEntry = new ViewEntry();
        public MainWindow()
        {
            InitializeComponent();
            btnConnect.Click += BtnConnect_Click;
            btnDisConnect.Click += BtnDisConnect_Click;
            btnIsLaserON.Click += BtnIsLaserOn_Click;
            btnLaserON.Click += BtnLaserOn_Click;
            btnLaserOff.Click += BtnLaserOff_Click;
            btnPilotON.Click += BtnPilotON_Click;
            btnPilotOff.Click += BtnPilotOff_Click;
            btnExternalON.Click += BtnExternalON_Click;
            btnExternalOff.Click += BtnExternalOff_Click;
            btnTest.Click += BtnTest_Click;
            btnReadInt.Click += BtnReadInt_Click;
            this.DataContext = viewEntry;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < 8; i++)
            //{
            //    UInt16 dd = 0;
            //    viewEntry.LsData.Add(MathHelper.SetBit(dd,i).ToString());
            //}
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                laserDevice?.DisConnect();
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
                laserDevice.DisConnect();
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
                laserDevice.Connect();
                viewEntry.LsData.Add("连接成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var tt = laserDevice.UaClient.BrowseNodeReference("ns=2;s=Production.MPI-Slot03.Output");
                var tt = laserDevice.UaClient.BrowseNodeReference("ns=2;s=Production.MPI-Slot03.Input");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool write(string path, UInt16 val)
        {
            try
            {
                var dat = laserDevice.WriteIntNode(path, val);
                return dat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private void BtnIsLaserOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dat = laserDevice.ReadNode("ns=2;s=Production.MPI-Slot03.Output.Word00.LASER_IS_ON");
                viewEntry.LsData.Add(dat.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnLaserOn_Click(object sender, RoutedEventArgs e)
        {
            //if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.PILOT_LASER_ON", true))
            //{
            //    viewEntry.LsData.Add("CMD LaserOn OK");
            //}
            //else
            //{
            //    viewEntry.LsData.Add("CMD LaserOn Fault");
            //}
        }
        private void BtnLaserOff_Click(object sender, RoutedEventArgs e)
        {
            //if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.PILOT_LASER_ON", false))
            //{
            //    viewEntry.LsData.Add("CMD LaserOff OK");
            //}
            //else
            //{
            //    viewEntry.LsData.Add("CMD LaserOff Fault");
            //}
        }


        private void BtnPilotON_Click(object sender, RoutedEventArgs e)
        {
            //bool ret = write("ns=2;s=Production.MPI-Slot03.Input.Word00", MathHelper.SetBit((UInt16)0, 8));
            bool ret = write("ns=2;s=Production.MPI-Slot03.Input.Word00.PILOT_LASER_ON", 1);
            if (ret)
            {
                viewEntry.LsData.Add("CMD PilotLaserOn OK");
            }
            else
            {
                viewEntry.LsData.Add("CMD PilotLaserOn Fault");
            }
        }

        private void BtnPilotOff_Click(object sender, RoutedEventArgs e)
        {
            //bool ret = write("ns=2;s=Production.MPI-Slot03.Input.Word00", (UInt16)0);
            bool ret = write("ns=2;s=Production.MPI-Slot03.Input.Word00.PILOT_LASER_ON", 0 );
            if (ret)
            {
                viewEntry.LsData.Add("CMD PilotLaserOn OK");
            }
            else
            {
                viewEntry.LsData.Add("CMD PilotLaserOn Fault");
            }
        }

        private void BtnExternalON_Click(object sender, RoutedEventArgs e)
        {
            //if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.EXT_ACTIVATION", true))
            //{
            //    viewEntry.LsData.Add("CMD EXT_ACTIVATION OK");
            //}
            //else
            //{
            //    viewEntry.LsData.Add("CMD EXT_ACTIVATION Fault");
            //}
        }

        private void BtnExternalOff_Click(object sender, RoutedEventArgs e)
        {
            //if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.EXT_ACTIVATION", false))
            //{
            //    viewEntry.LsData.Add("CMD EXT_ACTIVATIOff OK");
            //}
            //else
            //{
            //    viewEntry.LsData.Add("CMD EXT_ACTIVATIOff Fault");
            //}
        }

        private void BtnReadInt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dat = laserDevice.ReadIntNode("ns=2;s=Production.MPI-Slot03.Input.Word00");
                viewEntry.LsData.Add(dat.ToString());
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
