using OpcLaserControllor;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace OpcUaDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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

            this.DataContext = viewEntry;
            //LaserInstanceControllor.Instance.Initialize();

        }


        private void BtnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //LaserInstanceControllor.Instance.DisConnectLaser("123");
                ModInstance.Instance.UnInit();
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
                //op.Init("192.0.2.1:4840", "service");

                //ModInstance.Instance.Init("192.0.2.1:4840");
                viewEntry.LsData.Add("连接成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Instance_TestEvent(EventArgs arg)
        {
            viewEntry.LsData.Add(arg?.ToString());
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tt= ModInstance.Instance.laserDevice.UaClient.BrowseNodeReference("ns=2;s=Production.MPI-Slot03.Output");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool write(string path,object val)
        {
            try
            {
                var dat = ModInstance.Instance.laserDevice.WriteNode(path, val);
                return dat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private bool Bwrite(string path, bool val)
        {
            try
            {
                var dat = ModInstance.Instance.laserDevice.WriteBoolNode(path, val);
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
                var dat = ModInstance.Instance.laserDevice.ReadNode("ns=2;s=Production.MPI-Slot03.Output.Word00.LASER_IS_ON");
                viewEntry.LsData.Add(dat.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnLaserOn_Click(object sender, RoutedEventArgs e)
        {
            if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.PILOT_LASER_ON", true))
            {
                viewEntry.LsData.Add("CMD LaserOn OK");
            }
            else
            {
                viewEntry.LsData.Add("CMD LaserOn Fault");
            }
        }
        private void BtnLaserOff_Click(object sender, RoutedEventArgs e)
        {
            if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.PILOT_LASER_ON", false))
            {
                viewEntry.LsData.Add("CMD LaserOff OK");
            }
            else
            {
                viewEntry.LsData.Add("CMD LaserOff Fault");
            }
        }


        private void BtnPilotON_Click(object sender, RoutedEventArgs e)
        {
            if (write("ns=2;s=Production.MPI-Slot03.Input.Word00", 2))
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
            if (write("ns=2;s=Production.MPI-Slot03.Input.Word00", 0))
            {
                viewEntry.LsData.Add("CMD PilotLaserOff OK");
            }
            else
            {
                viewEntry.LsData.Add("CMD PilotLaserOff Fault");
            }
        }

        private void BtnExternalON_Click(object sender, RoutedEventArgs e)
        {
            if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.EXT_ACTIVATION", true))
            {
                viewEntry.LsData.Add("CMD EXT_ACTIVATION OK");
            }
            else
            {
                viewEntry.LsData.Add("CMD EXT_ACTIVATION Fault");
            }
        }

        private void BtnExternalOff_Click(object sender, RoutedEventArgs e)
        {
            if (write("ns=2;s=Production.MPI-Slot03.Input.Word00.EXT_ACTIVATION", false))
            {
                viewEntry.LsData.Add("CMD EXT_ACTIVATIOff OK");
            }
            else
            {
                viewEntry.LsData.Add("CMD EXT_ACTIVATIOff Fault");
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
