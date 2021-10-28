using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfThread
{
    /// <summary>
    /// ThreadAsyncPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ThreadAsyncPanel : UserControl
    {
        public ThreadAsyncPanel()
        {
            InitializeComponent();
            btnAsync.Click += BtnAsync_Click;
            btnDefAsync.Click += BtnDefAsync_Click;
        }

        private void msg(object obj)
        {
            Thread tt = new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ls_box.Items.Insert(0, obj);
                }));
            });
            tt.Start();
        }

        private async void BtnAsync_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                msg("task run");
                Thread.Sleep(2000);
            });
            msg("task finish");

            Thread tt = new Thread(() =>
            {
                msg("thread run");
            });
            tt.Start();
        }

        private async void BtnDefAsync_Click(object sender, RoutedEventArgs e)
        {
            msg("test start");
            await AsyncMethod();
            msg("test finish");
        }

        private Task<bool> AsyncMethod()
        {
            var tsk = Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(3000);
                    msg("Task ok");
                    if (1 > 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            });
            return tsk;
        }
    }
}
