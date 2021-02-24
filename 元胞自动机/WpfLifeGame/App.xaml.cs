using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfLifeGame.CellBase;

namespace WpfLifeGame
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            _init();
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _uninit();
            base.OnExit(e);
            System.Environment.Exit(0);
        }

        protected void _init()
        {
            CellsParameter.Instance.Read("CellPatterns.xml");
        }
        protected void _uninit()
        {
            CellsParameter.Instance.Write("CellPatterns.xml");
        }
    }
}
