using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfThread
{
    /// <summary>
    /// ThreadPoolPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ThreadPoolPanel : UserControl
    {
        public ThreadPoolPanel()
        {
            InitializeComponent();
            test5();
        }
        private void showMsg(string arg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ListViewItem item = new ListViewItem();
                item.Content = arg;
                ls_view.Items.Add(item);
                ls_view.ScrollIntoView(item);
            }));
        }
        private void printThread(string str)
        {
            int workThreadNumber;
            int ioThreadNumber;
            ThreadPool.GetAvailableThreads(out workThreadNumber, out ioThreadNumber);
            showMsg($"{str}\n CurrentThreadId is {Thread.CurrentThread.ManagedThreadId}\n CurrentThread is background :{Thread.CurrentThread.IsBackground}\n workThreadNumber is:{workThreadNumber}\n IOThreadNumbers is: {ioThreadNumber}\n ");
        }


        //线程池传参
        private void test1()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(callBackFunc), new argClass(MathOption.Add));
            ThreadPool.QueueUserWorkItem(new WaitCallback(callBackFunc), new argClass(MathOption.Neg));
            ThreadPool.QueueUserWorkItem(new WaitCallback(callBackFunc), new argClass(MathOption.Mult));
            showMsg("主线程结束");
        }
        private void callBackFunc(object option)
        {
            argClass arg = option as argClass;

            switch (arg.MathOption)
            {
                case MathOption.Add:
                    showMsg("Add");
                    break;
                case MathOption.Mult:
                    showMsg("Mult");
                    break;
                case MathOption.Neg:
                    showMsg("Neg");
                    break;
                case MathOption.Sub:
                    showMsg("Sub");
                    break;
                default:
                    showMsg("Default");
                    break;
            }
        }


        //一般用法
        private void test2()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CountProcess));
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetEnvironmentVariables));
        }
        /// <summary>  
        /// 统计当前正在运行的系统进程信息  
        /// </summary>  
        /// <param name="state"></param>  
        private void CountProcess(object state)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                try
                {
                    showMsg($"进程信息:Id:{p.Id},ProcessName:{ p.ProcessName},StartTime:{p.StartTime}");
                }
                catch (Win32Exception e)
                {
                    showMsg($"ProcessName:{ p.ProcessName}");
                }
                finally
                {
                }
            }
            showMsg("获取进程信息完毕。");
        }
        /// <summary>  
        /// 获取当前机器系统变量设置  
        /// </summary>  
        /// <param name="state"></param>  
        public void GetEnvironmentVariables(object state)
        {
            IDictionary list = System.Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry item in list)
            {
                showMsg($"系统变量信息:key={item.Key},value={item.Value}");
            }
            showMsg("获取系统变量信息完毕。");
        }


        //调用工作线程和io线程的范例以及异步操作
        private void test3()
        {

            //调用工作线程和IO线程的范例
            //设置线程池中处于活动的线程的最大数目
            //设置线程中工作者线程数量为1000，I/O线程数量为1000
            ThreadPool.SetMaxThreads(1000, 1000);
            showMsg("Main Thread:queue an asynchronous method");
            printThread("Main Thread Start");

            //把工作项添加到队列中，此时线程池会用工作者线程去执行回调函数
            ThreadPool.QueueUserWorkItem(asyncMethod);
            asyncWriteFile();
        }
        private void asyncMethod(object state)
        {
            Thread.Sleep(1000);
            printThread("Asynchoronous Method");
            showMsg("Asynchoronous thread has worked ");
        }
        #region 异步读取文件模块
        private void asyncReadFile()
        {
            byte[] byteData = new byte[1024];
            FileStream stream = new FileStream(@"D:\123.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
            //把FileStream对象，byte[]对象，长度等有关数据绑定到FileDate对象中，以附带属性方式送到回调函数
            Hashtable ht = new Hashtable();
            ht.Add("Length", (int)stream.Length);
            ht.Add("Stream", stream);
            ht.Add("ByteData", byteData);

            //启动异步读取,倒数第二个参数是指定回调函数，倒数第一个参数是传入回调函数中的参数
            stream.BeginRead(byteData, 0, (int)ht["Length"], new AsyncCallback(Completed), ht);
            printThread("asyncReadFile Method");
        }

        //实际参数就是回调函数
        private void Completed(IAsyncResult result)
        {
            Thread.Sleep(2000);
            printThread("asyncReadFile Completed Method");
            //参数result实际上就是Hashtable对象，以FileStream.EndRead完成异步读取
            Hashtable ht = (Hashtable)result.AsyncState;
            FileStream stream = (FileStream)ht["Stream"];
            int length = stream.EndRead(result);
            stream.Close();
            string str = Encoding.UTF8.GetString(ht["ByteData"] as byte[]);
            showMsg(str);
            stream.Close();
        }
        #endregion
        #region 异步写入文件模块
        //异步写入模块
        private void asyncWriteFile()
        {
            //文件名 文件创建方式 文件权限 文件进程共享 缓冲区大小为1024 是否启动异步I/O线程为true
            FileStream stream = new FileStream(@"D:\123.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
            //这里要注意，如果写入的字符串很小，则.Net会使用辅助线程写，因为这样比较快
            byte[] bytes = Encoding.UTF8.GetBytes("你在他乡还好吗？");
            //异步写入开始，倒数第二个参数指定回调函数，最后一个参数将自身传到回调函数里，用于结束异步线程
            stream.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(Callback), stream);
            printThread("AsyncWriteFile Method");
        }

        private void Callback(IAsyncResult result)
        {
            //显示线程池现状
            Thread.Sleep(2000);
            printThread("AsyncWriteFile Callback Method");
            //通过result.AsyncState再强制转换为FileStream就能够获取FileStream对象，用于结束异步写入
            FileStream stream = (FileStream)result.AsyncState;
            stream.EndWrite(result);
            stream.Flush();
            stream.Close();
            asyncReadFile();
        }
        #endregion


        //线程池同步
        private void test4()
        {
            ManualResetEvent[] events = new ManualResetEvent[thread_count];
            Stopwatch sw = new Stopwatch();
            Thread tt = new Thread(() =>
            {
                showMsg($"当前主线程id:{Thread.CurrentThread.ManagedThreadId}");
                sw.Start();
                NoThreadPool(thread_count);
                sw.Stop();
                showMsg($"Execution time using threads:{sw.ElapsedMilliseconds}");

                sw.Reset();
                sw.Start();
                for (int i = 0; i < thread_count; i++)
                {
                    //实例化同步工具
                    events[i] = new ManualResetEvent(false);
                    //tst在这里就是同步工具，将同步工具的引用传入能保证共享区内每次只有一个线程进入
                    ThreadPoolDemo tst = new ThreadPoolDemo(events[i]);  //将锁也放入回调函数，执行一次开一把锁
                    tst.ShowMessageEvent += Tst_ShowMessageEvent;
                    //Thread.Sleep(200);
                    //将任务放入线程池中，让线程池中的线程执行改该任务
                    ThreadPool.QueueUserWorkItem(tst.DisplayNumber, i);
                }
                //注意这里，设定WaitAll是为了阻塞调用线程（主线程），让其余线程先执行完毕，
                //其中每个任务完成后调用其set()方法(收到信号),当所有
                //的任务都收到信号后，执行完毕，将控制权再次交回调用线程（这里的主线程）
                ManualResetEvent.WaitAll(events);
                sw.Stop();
                showMsg($"Execution time using threads: {sw.ElapsedMilliseconds}");
                //Console.WriteLine("所有任务做完！");
            });
            tt.Start();
        }
        private void Tst_ShowMessageEvent(string msg)
        {
            showMsg(msg);
        }
        int thread_count = 10;
        private void NoThreadPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(1000);
                showMsg($"当前运算结果:{i},count={i + 1},当前子线程id:{Thread.CurrentThread.ManagedThreadId}的状态:{Thread.CurrentThread.ThreadState}");
            }
        }


        //线程池取消执行线程
        CancellationTokenSource cts = new CancellationTokenSource();
        private void test5()
        {
            btn_cancel.Click += Btn_cancel_Click;

            ThreadPool.SetMaxThreads(1000, 1000);
            showMsg("Main thread run");
            printThread("Start");

            // 这里用Lambda表达式的方式和使用委托的效果一样的，只是用了Lambda后可以少定义一个方法。
            // 这在这里就是让大家明白怎么lambda表达式如何由委托转变的
            // ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));
            ThreadPool.QueueUserWorkItem(callBack, cts.Token);
            showMsg("waiting to cancel the operation");
        }
        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
        private void callBack(object state)
        {
            Thread.Sleep(1000);
            printThread("Asunchoronous Method Start");
            CancellationToken token = (CancellationToken)state;
            _count(token, 1000);
        }
        private void _count(CancellationToken token, int countto)
        {
            for (int i = 0; i < countto; i++)
            {
                if (token.IsCancellationRequested)
                {
                    showMsg("Count is canceled");
                    break;
                }
                showMsg(i.ToString());
                Thread.Sleep(300);
            }
        }
    }
    public enum MathOption
    {
        Add,
        Neg,
        Mult,
        Sub,
    }

    public class argClass
    {
        public MathOption MathOption;

        public argClass(MathOption op)
        {
            MathOption = op;
        }
    }

    //线程池同步
    public class ThreadPoolDemo
    {
        public delegate void ShowMessageEventHandle(string msg);
        public event ShowMessageEventHandle ShowMessageEvent;


        static object lockobj = new object();
        static int count = 0;
        ManualResetEvent manualResetEvent;
        public ThreadPoolDemo(ManualResetEvent manualEvent)
        {
            manualResetEvent = manualEvent;
        }

        public void DisplayNumber(object a)
        {
            lock (lockobj)
            {
                count++;
                ShowMessageEvent?.Invoke($"当前运算结果:{a},Count={count},当前子线程ID:{Thread.CurrentThread.ManagedThreadId}的状态{Thread.CurrentThread.ThreadState}");
            }
            //Console.WriteLine("当前运算结果:{0}", a);
            //Console.WriteLine("当前运算结果:{0},当前子线程id:{1} 的状态:{2}", a,Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.ThreadState);
            //这里是方法执行时间的模拟，如果注释该行代码，就能看出线程池的功能了
            Thread.Sleep(2000);
            //Console.WriteLine("当前运算结果:{0},Count={1},当前子线程id:{2} 的状态:{3}", a, Count, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.ThreadState);
            //这里是释放共享锁，让其他线程进入
            manualResetEvent.Set();
        }
    }
}
