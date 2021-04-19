using PowerMeterDevice;
using PowerMeterDevice.Common;
using PowerMeterDevice.Driver;
using PowerMeterDevice.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace PowerMeterDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        PowerAdjustModel mode = new PowerAdjustModel();
        PowerMeterDriver pm = new PowerMeterDriver();
        public MainWindow()
        {
            InitializeComponent();
            pm.SPortName = "COM7";
            pm.Connect();
            mode.PowerMeterDevice = pm;
            mode.PMParamSet = new LaserParamSet() { IsReadyWork = true };
            mode.Param = ParameterManager.Instance.powerAdjustParameter;
            mode.DataFit.DataFittings.Add(new LinearFit());
            pmd.DataContext = mode;
        }
    }
}
