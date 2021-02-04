using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DXFAnalyse
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ParameterInstance.Instance.dxfParameter.Read();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ParameterInstance.Instance.dxfParameter.Write();
        }
    }
}
