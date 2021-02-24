using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApp
{
    /// <summary>
    /// RouteEventUseDemo.xaml 的交互逻辑
    /// </summary>
    public partial class RouteEventUseDemo : UserControl
    {
        public RouteEventUseDemo()
        {
            InitializeComponent();
            gridRoot.AddHandler(Button.ClickEvent, new RoutedEventHandler(routeClickEvent));
        }

        private void routeClickEvent(object sender,RoutedEventArgs arg)
        {
            MessageBox.Show($"sender:{(sender as FrameworkElement).Name} source:{(arg.Source as FrameworkElement).Name} original_source:{(arg.OriginalSource as FrameworkElement).Name} ");
        }
    }
}
