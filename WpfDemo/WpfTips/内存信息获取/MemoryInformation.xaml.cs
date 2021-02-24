using Common.EventAggregator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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


namespace WpfTips.MemoryInfo
{
    /// <summary>
    /// MemoryInformation.xaml 的交互逻辑
    /// </summary>
    public partial class MemoryInformation : UserControl
    {
        public MemoryInformation()
        {
            InitializeComponent();
        }

        List<IntPtr> lsptr = new List<IntPtr>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            int count = int.Parse(tag);
            for (int i = 0; i < count; i++)
            {
                IntPtr ptr = Marshal.AllocHGlobal(1000);
                lsptr.Add(ptr);
            }
            CommonEventAggregator.Instance.Publish(new MessageEventArgs(tag + "  " + GetRam("WpfTips")));
        }

        private void Button_Click_free(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lsptr.Count; i++)
            {
                Marshal.FreeHGlobal(lsptr[i]);
            }
        }

        internal static string GetRam(string rmu_exe)
        {
            try
            {
                Process CurrentProcess = Process.GetProcessesByName(rmu_exe)[0];
                PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", CurrentProcess.ProcessName);
                string workingSe = String.Format("{0:F}", pf1.NextValue() / 1024 / 1024);
                return $"{ workingSe }MB";
            }
            catch
            {
                string rmu_mbRam = "0 KB";
                return rmu_mbRam;
            }
        }
    }
}
