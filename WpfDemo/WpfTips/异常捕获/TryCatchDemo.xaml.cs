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

namespace WpfTips.ExceptionDemo
{
    /// <summary>
    /// TryCatchDemo.xaml 的交互逻辑
    /// </summary>
    public partial class TryCatchDemo : UserControl
    {
        public TryCatchDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonEventAggregator.Instance.Publish(new MessageEventArgs("try"));
                return;
            }
            catch (Exception ex)
            {
                CommonEventAggregator.Instance.Publish(new MessageEventArgs("catch:" + ex.Message));
            }
            finally
            {
                CommonEventAggregator.Instance.Publish(new MessageEventArgs("finally"));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonEventAggregator.Instance.Publish(new MessageEventArgs("try"));
                throw (new Exception("test"));
            }
            catch (Exception ex)
            {
                CommonEventAggregator.Instance.Publish(new MessageEventArgs("catch:"+ex.Message));
            }
            finally
            {
                CommonEventAggregator.Instance.Publish(new MessageEventArgs("finally"));
            }
        }
    }
}
