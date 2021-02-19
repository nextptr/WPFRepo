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
    /// ThreadParameter.xaml 的交互逻辑
    /// </summary>
    public partial class ThreadParameterPanel : UserControl
    {
        public ThreadParameterPanel()
        {
            InitializeComponent();
            btn_add.Click += Btn_add_Click;
        }
        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            int a = int.Parse(txt_num1.Text);
            int b = int.Parse(txt_num2.Text);

            //在子线程中切换界面线程，join()等不到，程序卡死
            {
                //Thread localThread = null;
                //localThread = new Thread(new ParameterizedThreadStart(mathPlus));
                //localThread.Start(new mathArg(a, b));

                //localThread.Join();         //在mathPlus调用函数中 调用this.Dispatcher.Invoke(new Action(() =>、、之后，Join()等不到
                //int dsd = 10;               //Join()一直等不到，程序不能进入此语句
                //localThread = null;         

                //第40行ui线程开始运行子线程
                //      子线程中Dispatcher调用ui线程显示信息
                //第42行ui线程join()等待子线程结束，ui卡死
                //      由于ui线程卡死,子线程中Dispatcher无法调用ui线程无法执行，子线程卡死
                //  死锁
            }

            //如果子线程中需要切换界面线程显示数据，需要将子线程再次封装一个线程
            {
                Thread tt = new Thread(() =>
                {
                    Thread localThread = null;
                    localThread = new Thread(new ParameterizedThreadStart(mathPlus));
                    localThread.Start(new mathArg(a, b)); //线程传参

                    localThread.Join();
                    int dsd = 10;
                    localThread = null;
                });
                tt.Start();

                //第59行tt子线程开始运行local子线程
                //      local子线程中Dispatcher调用ui线程显示信息
                //第62行tt子线程join()等待local子线程结束，tt卡死
                //      ui线程没有join()等待tt,local子线程中可以Dispatcher调用ui线程显示消息
            }

            lab_main.Content = $"主线程：{a + b}";
        }

        private void mathPlus(object obj)
        {
            mathArg arg = obj as mathArg;

            this.Dispatcher.Invoke(new Action(() =>
            {
                lab_sub.Content = "子线程：" + (arg.num1 + arg.num2).ToString();
            }));
        }
    }
    public class mathArg
    {
        public int num1 = 0;
        public int num2 = 0;
        public mathArg(int a, int b)
        {
            num1 = a;
            num2 = b;
        }
    }
}
