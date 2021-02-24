using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfBase.Chapter5
{
    /// <summary>
    /// unit5_2.xaml 的交互逻辑
    /// </summary>
    public partial class unit5_2 : UserControl
    {
        private int step = 0;
        public unit5_2()
        {
            InitializeComponent();
            this.Tag = "生命周期事件";
            btn_pop.Click += Btn_pop_Click;
            btn_clear.Click += Btn_clear_Click;
            btn_popInstance.Click += Btn_popInstance_Click;
            this.Loaded += Unit5_2_Loaded;
        }

        private void Unit5_2_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                InstanceWindow.Instance.Loaded += InstanceWindow_Loaded;
                this.Loaded -= Unit5_2_Loaded;
            }
        }

        private void InstanceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ddmsg("InstanceWindow_Loaded");
        }


        /// <summary>
        /// 窗口已经关闭时发生，任可访问元素，此时可执行一些清理工作，向永久存储写入信息等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Closed(object sender, EventArgs e)
        {
            ddmsg("Closed");
        }

        /// <summary>
        /// 关闭窗口时发生，一般使用Window.Close()，Application.SessionEnding()关闭窗口，
        /// 在closing事件中CancelEventArgs e.Cancel = true;可以终止关闭操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult ret = MessageBox.Show("是否关闭窗口?", "提示", MessageBoxButton.OKCancel);
            if (ret == MessageBoxResult.OK)
            {
                ddmsg("Closing");
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 当切换至其他窗口时发生，类似控件的LostFocus事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Deactivated(object sender, EventArgs e)
        {
            ddmsg("Deactivated");
        }

        /// <summary>
        /// 当从其他窗口切换至本窗口，以及第一次加载窗口时发生，类似控件的GetFocus事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Activated(object sender, EventArgs e)
        {
            ddmsg("Activated");
        }

        /// <summary>
        /// 窗口第一次呈现后立即发生，窗口已经完全可见了，并且已经准备好接受输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_ContentRendered(object sender, EventArgs e)
        {
            ddmsg("ContentRendered");
        }

        /// <summary>
        /// 在窗口可见之前取得窗口的HwndSource属性时发生，HwndSource是窗口句柄，用于调用WIn32 API中的遗留函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_SourceInitialized(object sender, EventArgs e)
        {
            ddmsg("SourceInitialized");
        }

        /// <summary>
        /// 当元素被释放时发生，例如包含元素的窗口被关闭，或特定的元素从窗口中删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Unloaded(object sender, RoutedEventArgs e)
        {
            ddmsg("Unloaded");
            step = 0;
        }

        /// <summary>
        /// 当窗口已经初始化并应用了样式和数据绑定时发生，这是元素被呈现之前的最后一站，此时的IsLoaded==true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible == true) //在异常情况下会多次触发loaded事件，最好加以判断
            {
                ;//
            }
            ddmsg("Loaded");
        }

        /// <summary>
        /// 当元素被实例化，并已经根据Xaml标记设置了元素的属性之后发生，这时元素已经初始化，但窗口其他部分可能尚未初始化
        /// 此时尚未应用样式和数据绑定，IsInitialized==true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Initialized(object sender, EventArgs e)
        {
            ddmsg("Initialized");
        }

        private void Btn_popInstance_Click(object sender, RoutedEventArgs e)
        {
            InstanceWindow.Instance.Owner = Application.Current.MainWindow;
            InstanceWindow.Instance.WindowState = WindowState.Normal;
            InstanceWindow.Instance.Show();
            InstanceWindow.Instance.Show();
        }
        private void Btn_pop_Click(object sender, RoutedEventArgs e)
        {
            PopWindow win = new PopWindow();
            win.Initialized += Win_Initialized;
            win.Loaded += Win_Loaded;
            win.Unloaded += Win_Unloaded;
            win.SourceInitialized += Win_SourceInitialized;
            win.ContentRendered += Win_ContentRendered;
            win.Activated += Win_Activated;
            win.Deactivated += Win_Deactivated;
            win.Closing += Win_Closing;
            win.Closed += Win_Closed;

            win.Show();
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            list_view.Items.Clear();
        }
        private void ddmsg(string msg)
        {
            list_view.Items.Add($"初始化顺序:({++step}){msg}");
        }
    }
}
