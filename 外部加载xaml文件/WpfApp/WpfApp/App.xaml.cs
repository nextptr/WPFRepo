using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace WpfApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //LoadSkin("Skins/skin1.xaml");
        }

        private void LoadSkin(string filePath)
        {
            XmlReader XmlRead = XmlReader.Create(filePath);
            Application.Current.Resources = (ResourceDictionary)XamlReader.Load(XmlRead);
            XmlRead.Close();
        }
    }
}
