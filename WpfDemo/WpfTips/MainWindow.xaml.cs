using Common.EventAggregator;
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

namespace WpfTips
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IEventHandler<MessageEventArgs>
    {
        public MainWindow()
        {
            InitializeComponent();
            CommonEventAggregator.Instance.Subscribe(this);
        }

        public void Handler(MessageEventArgs @event)
        {
            this.Dispatcher.Invoke(() =>
            {
                ls_box.Items.Add(@event.MSG);
            });
        }
    }
}
