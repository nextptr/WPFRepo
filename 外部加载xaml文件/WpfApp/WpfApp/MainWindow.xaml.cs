using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadExtElement("Skins/extControl.xaml");//加载外部控件
            //LoadSkin("Skins/skin1.xaml");         //加载外部控件style
        }

        public void loadExtElement(string filePath)
        {
            this.Width = this.Height = 500;
            this.Left = this.Top = 200;
            this.Title = "动态加载XAML";
            //从其他文件中获取xaml内容
            DependencyObject rootElement;
            using (FileStream fileStream =new FileStream(filePath,FileMode.Open))
            {
                rootElement = (DependencyObject)XamlReader.Load(fileStream);
            }
            this.Content = rootElement;
            button1 = (Button)LogicalTreeHelper.FindLogicalNode(rootElement,"button1");
        }

        private void LoadSkin(string filePath)
        {
            XmlReader XmlRead = XmlReader.Create(filePath);
            Application.Current.Resources = (ResourceDictionary)XamlReader.Load(XmlRead);
            XmlRead.Close();
        }
    }
}
