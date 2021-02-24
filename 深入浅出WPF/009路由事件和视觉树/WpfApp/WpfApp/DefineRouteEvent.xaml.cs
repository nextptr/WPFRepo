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
    /// DefineRouteEvent.xaml 的交互逻辑
    /// </summary>
    public partial class DefineRouteEvent : UserControl
    {
        public DefineRouteEvent()
        {
            InitializeComponent();
            btn_time.Click += Btn_time_Click;
        }

        private void Btn_time_Click(object sender, RoutedEventArgs e)
        {
            btn_time.RaiseReportTimeEvent();
        }

        private void ReportTimeEventHandle(object sender, ReportTimeEventArgs e)
        {
            if (chk_handle.IsChecked==true)
            {
                if ((sender as FrameworkElement).Name == "grid_3")
                {
                    e.Handled = true;
                }
            }

            FrameworkElement element = sender as FrameworkElement;
            string timeStr = e.ClickTime.ToLongTimeString();
            string content = string.Format("{0}到达{1}", timeStr, element.Name);
            ls_box.Items.Add(content);
        }
    }


    class ReportTimeEventArgs : RoutedEventArgs
    {
        public ReportTimeEventArgs(RoutedEvent routeEvent, object source)
            : base(routeEvent, source)
        { }

        public DateTime ClickTime { get; set; }
    }


    class TimeButton : Button
    {
        public static readonly RoutedEvent ReportTimeEvent = EventManager.RegisterRoutedEvent("ReportTime",
            RoutingStrategy.Tunnel, typeof(EventHandler<ReportTimeEventArgs>), typeof(TimeButton));

        public event RoutedEventHandler ReportTime
        {
            add
            {
                this.AddHandler(ReportTimeEvent, value);
            }
            remove
            {
                this.RemoveHandler(ReportTimeEvent, value);
            }
        }

        public void RaiseReportTimeEvent()
        {
            ReportTimeEventArgs args = new ReportTimeEventArgs(ReportTimeEvent, this);
            args.ClickTime = DateTime.Now;
            this.RaiseEvent(args);
        }


    }
}
