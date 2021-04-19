using PowerMeterDevice.Interf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PowerMeterDevice
{
    /// <summary>
    /// PowerMeterControl.xaml 的交互逻辑
    /// </summary>
    public partial class PowerMeterControl : UserControl
    {
        public PowerMeterControl()
        {
            InitializeComponent();
            btnSetLength.Click += BtnSetLength_Click;
            btnZero.Click += BtnZero_Click;
            btnAcqDevice.Click += BtnAcqDevice_Click;
        }

        bool isAcq = false;

        public void DeviceExit()
        {
            if (pm != null)
            {
                pm.PowerMeterOnTimeEvent -= Pm_PowerMeterOnTimeEvent;
                pm.PowerMeterWaveLengthEvent -= Pm_PowerMeterWaveLengthEvent;
                pm.PowerMeterZeroEvent -= Pm_PowerMeterZeroEvent;
            }
        }
        protected void SetDevice()
        {
            if (PowerMeterDevice != null)
            {
                PowerMeterDevice.PowerMeterOnTimeEvent -= Pm_PowerMeterOnTimeEvent;
                PowerMeterDevice.PowerMeterOnTimeEvent += Pm_PowerMeterOnTimeEvent;
                PowerMeterDevice.PowerMeterWaveLengthEvent -= Pm_PowerMeterWaveLengthEvent;
                PowerMeterDevice.PowerMeterWaveLengthEvent += Pm_PowerMeterWaveLengthEvent;
                PowerMeterDevice.PowerMeterZeroEvent -= Pm_PowerMeterZeroEvent;
                PowerMeterDevice.PowerMeterZeroEvent += Pm_PowerMeterZeroEvent;
                PowerMeterDevice.PowerMeterSamplingEvent -= PowerMeterDevice_PowerMeterSamplingEvent;
                PowerMeterDevice.PowerMeterSamplingEvent += PowerMeterDevice_PowerMeterSamplingEvent;

                lab_com.Content = PowerMeterDevice.SPortName;
                PowerMeterDevice.GetWaveLength(out int length);
                txt_length.Text = length.ToString();
            }
        }

        private void PowerMeterDevice_PowerMeterSamplingEvent(PowerMeterSamplingEventArgs args)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                isAcq = args.IsSampling;
                btnAcqDevice.IsChecked = args.IsSampling;
            }));
        }

        private void Pm_PowerMeterZeroEvent(PowerMeterZeroEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                panel_OnTimeView.SetZeroState(args.IsZeroSetted);
                panel_OnTimeView.labZero.Text = args.IsZeroSetted == true ? "置零" : "未置零";
            }, DispatcherPriority.Normal);
        }
        private void Pm_PowerMeterWaveLengthEvent(PowerMeterWaveLengthEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                txt_length.Text = args.WaveLength.ToString();
                panel_OnTimeView.SetWaveLength(args.WaveLength);
                panel_OnTimeView.labLength.Text = args.WaveLength.ToString();
            }, DispatcherPriority.Normal);
        }
        private void Pm_PowerMeterOnTimeEvent(PowerMeterOnTimeEventArgs args)
        {
            if (isAcq)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    panel_OnTimeView.Add(args.OntimeValue);
                }));
            }
        }

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag = true;
                int length = 0;
                await Task.Run(() =>
                {
                    flag = pm?.Connect() == true;
                });
                //EnablePanel(flag);       //控件使能
                if (flag == true)
                {
                    Thread.Sleep(100);
                    pm?.CleanZero();    //清除归零状态

                    Thread.Sleep(100);
                    pm?.StopSampling(); //开始采集数据

                    Thread.Sleep(100);
                    if (pm?.GetWaveLength(out length) == true)
                    {
                        panel_OnTimeView.SetWaveLength(length);
                    }

                    //entry.Zero = false;
                    btnZero.Content = "归零";
                    panel_OnTimeView.SetZeroState(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void BtnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pm?.StopSampling();
                btnAcqDevice.Content = "开始统计";
                btnAcqDevice.IsChecked = false;
                isAcq = false;

                bool flag = true;
                await Task.Run(() =>
                {
                    flag = pm?.DisConnect() == true;
                });
                //EnablePanel(!flag);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnSetLength_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pm == null)
                {
                    MessageBox.Show("功率计设备为空");
                    return;
                }
                pm?.SetWaveLength(Convert.ToInt32(txt_length));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnAcqDevice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tb = sender as ToggleButton;
                if (pm == null)
                {
                    MessageBox.Show("功率计设备为空");
                    if (tb != null)
                        tb.IsChecked = false;
                    return;
                }
                if (pm.IsConnected == false)
                {
                    MessageBox.Show("功率计设备未连接");
                    if (tb != null)
                        tb.IsChecked = false;
                    return;
                }
                if (tb.IsChecked == true)
                {
                    pm?.BeginSampling();
                    isAcq = true;
                }
                else if (tb.IsChecked == false)
                {
                    pm?.StopSampling();
                    isAcq = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnZero_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleButton btn = sender as ToggleButton;
                if (pm == null)
                {
                    MessageBox.Show("功率计设备为空");
                    if (btn != null)
                        btn.IsChecked = false;
                    return;
                }
                if (pm.IsConnected == false)
                {
                    if (btn != null)
                        btn.IsChecked = false;
                    return;
                }
                if (btn.IsChecked == true)
                {
                    pm?.SetZero();
                }
                else
                {
                    pm?.CleanZero();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EnablePanel(bool flag)
        {
            if (flag == true)
            {
                btnZero.IsEnabled = true;
                txt_length.IsEnabled = true;
                btnSetLength.IsEnabled = true;
                btnAcqDevice.IsEnabled = true;

                btnZero.Opacity = 1;
                txt_length.Opacity = 1;
                btnSetLength.Opacity = 1;
                btnAcqDevice.Opacity = 1;
            }
            else
            {
                btnZero.IsEnabled = false;
                txt_length.IsEnabled = false;
                btnSetLength.IsEnabled = false;
                btnAcqDevice.IsEnabled = false;

                btnZero.Opacity = 0.5;
                txt_length.Opacity = 0.5;
                btnSetLength.Opacity = 0.5;
                btnAcqDevice.Opacity = 0.5;
            }
        }

        private IPowerMeter pm;
        public IPowerMeter PowerMeterDevice
        {
            get => pm;
            set { DeviceExit(); pm = value; SetDevice(); }
        }

        public IPowerMeter PMDeviceProp
        {
            get { return (IPowerMeter)GetValue(PMDevicePropProperty); }
            set
            {
                SetValue(PMDevicePropProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for PMDeviceProp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PMDevicePropProperty =
            DependencyProperty.Register("PMDeviceProp", typeof(IPowerMeter), typeof(PowerMeterControl), new PropertyMetadata(null, new PropertyChangedCallback(OnPMDevicePropChanged)));

        private static void OnPMDevicePropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as PowerMeterControl;
            sender.PowerMeterDevice = e.NewValue as IPowerMeter;
        }
    }
}
