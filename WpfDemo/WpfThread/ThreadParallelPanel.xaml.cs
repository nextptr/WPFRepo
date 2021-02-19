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
    /// ThreadParallelPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ThreadParallelPanel : UserControl
    {
        public ThreadParallelPanel()
        {
            InitializeComponent();
            btnForeach.Click += BtnForeach_Click;
            btnFor.Click += BtnFor_Click;
            btnInvoke.Click += BtnInvoke_Click;
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

        List<string> datas = new List<string>();
        private void BtnForeach_Click(object sender, RoutedEventArgs e)
        {
            datas.Clear();
            datas.Add("foreach1");
            datas.Add("foreach2");
            datas.Add("foreach3");
            datas.Add("foreach4");
            datas.Add("foreach5");
            datas.Add("foreach6");
            Parallel.ForEach<string>(datas, obj =>
            {
                msg(obj);
            });
        }

        private void BtnFor_Click(object sender, RoutedEventArgs e)
        {
            datas.Clear();
            datas.Add("for1");
            datas.Add("for2");
            datas.Add("for3");
            datas.Add("for4");
            datas.Add("for5");
            datas.Add("for6");
            Parallel.For(0, datas.Count, i =>
            {
                msg(datas[i]);
            });
        }
        private void BtnInvoke_Click(object sender, RoutedEventArgs e)
        {
            Parallel.Invoke(
                () =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        msg($"Ainvoke{i}");
                    }
                },
                () =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        msg($"Binvoke{i}");
                    }
                }
                );
        }
    }
}
