using System;
using System.Collections;
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
    /// LockMonitorResetEventPanel.xaml 的交互逻辑
    /// </summary>
    public partial class LockMonitorResetEventPanel : UserControl
    {
        public LockMonitorResetEventPanel()
        {
            InitializeComponent();
        }
        
        private object lockTest = new object();
        private void btnLockTest(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                lock (lockTest)
                {
                    Thread.Sleep(2000);
                    msg("Lock Test");
                }
            });
        }
        private void btnMonitorTest(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Monitor.Enter(this);
                Thread.Sleep(2000);
                msg("Monitor Test");
                Monitor.Exit(this);
            });
        }
        private object monitorLok = new object();
        private void btnTryMonitorTest(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (Monitor.TryEnter(monitorLok, 100))
                {
                    Thread.Sleep(3000);
                    msg("Try Monitor Test");
                    Monitor.Exit(monitorLok);
                }
            });
        }

        const int MAX_LOOP_TIME = 100;
        Queue m_smplQueue = new Queue();
        public void FirstThread()
        {
            int counter = 0;
            lock (m_smplQueue)
            {
                while (counter < MAX_LOOP_TIME)
                {
                    //Wait, if the queue is busy.
                    Monitor.Wait(m_smplQueue);
                    //Push one element.
                    m_smplQueue.Enqueue(counter);
                    //Release the waiting thread.
                    Monitor.Pulse(m_smplQueue);

                    counter++;
                }
                int i = 0;
            }
        }
        public void SecondThread()
        {
            lock (m_smplQueue)
            {
                //Release the waiting thread.
                Monitor.Pulse(m_smplQueue);
                //Wait in the loop, while the queue is busy.
                //Exit on the time-out when the first thread stops.
                while (Monitor.Wait(m_smplQueue, 50000))
                {
                    //Pop the first element.
                    int counter = (int)m_smplQueue.Dequeue();
                    //Print the first element.
                    msg(counter.ToString());
                    //Release the waiting thread.
                    Monitor.Pulse(m_smplQueue);
                }
            }
        }
        //Return the number of queue elements.
        public int GetQueueCount()
        {
            return m_smplQueue.Count;
        }
        private void btnBlogTest(object sender, RoutedEventArgs e)
        {
            //Thread tFirst = new Thread(new ThreadStart(FirstThread));
            ////Create the second thread.
            //Thread tSecond = new Thread(new ThreadStart(SecondThread));
            ////Start threads.
            //tFirst.Start();
            //tSecond.Start();
            ////wait to the end of the two threads
            //tFirst.Join();
            //tSecond.Join();
            ////Print the number of queue elements.
            //msg("Queue Count = " + GetQueueCount().ToString());
        }

        

        private void msg(object obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                ls_box.Items.Add(obj);
            });
        }
    }
}
