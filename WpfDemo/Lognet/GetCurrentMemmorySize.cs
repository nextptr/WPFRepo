using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lognet
{
    public class GetCurrentMemmorySize
    {
        public static string GetMemmorySize(string rmu_exe = "")
        {
            try
            {
                Process CurrentProcess = Process.GetCurrentProcess();
                //Process CurrentProcess = Process.GetProcessesByName(rmu_exe)[0];
                PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", CurrentProcess.ProcessName);
                string workingSe = String.Format("{0:F}", pf1.NextValue() / 1024 / 1024);
                return workingSe;
                //return $"{ workingSe }MB";
            }
            catch
            {
                string rmu_mbRam = "0 KB";
                return rmu_mbRam;
            }
        }
    }
}
