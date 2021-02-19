using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace WpfThread
{
    /// <summary>
    /// ThreadSemaphorePanel.xaml 的交互逻辑
    /// </summary>
    public partial class ThreadSemaphorePanel : UserControl
    {
        static Semaphore sema = new Semaphore(1, 1);
        List<Thread> ls_th = null;
        public ThreadSemaphorePanel()
        {
            InitializeComponent();
            btn_start.Click += Btn_start_Click;
            btn_stop.Click += Btn_stop_Click;
            txt_signal_init.TextChanged += Txt_signal_init_TextChanged;
            txt_signal_max.TextChanged += Txt_signal_max_TextChanged;
        }

        private void Txt_signal_init_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int init = 0;
                int max = 0;
                int.TryParse(txt_signal_init.Text, out init);
                int.TryParse(txt_signal_max.Text, out max);
                sema = new Semaphore(init, max);
            }
            catch (Exception ex)
            {
                Msg(ex.Message);
            }
        }

        private void Txt_signal_max_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int init = 0;
                int max = 0;
                int.TryParse(txt_signal_init.Text, out init);
                int.TryParse(txt_signal_max.Text, out max);
                sema = new Semaphore(init, max);
            }
            catch (Exception ex)
            {
                Msg(ex.Message);
            }
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            ls_box.Items.Clear();
            if (ls_th != null)
            {
                foreach (Thread th in ls_th)
                {
                    th.Abort();
                }
            }

            ls_th = new List<Thread>();
            for (int i = 0; i < 3; i++)
            {
                Thread th = new Thread(test) { Name = $"Thread{i}" };
                ls_th.Add(th);
                th.Start();
            }
        }

        private void Btn_stop_Click(object sender, RoutedEventArgs e)
        {
            foreach (Thread th in ls_th)
            {
                th.Abort();
            }
            ls_th = null;
        }

        private void test()
        {
            sema.WaitOne();
            for (int i = 0; i < 3; i++)
            {
                Msg($"ThreadName:{Thread.CurrentThread.Name} i:{i}");
                Thread.Sleep(500);
            }
            sema.Release();
        }

        private void Msg(object obj)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ls_box.Items.Add(obj);
            }));
        }
    }
}
