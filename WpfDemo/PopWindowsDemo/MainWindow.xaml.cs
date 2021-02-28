using System;
using System.Windows;

namespace PopWindowsDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private PopWindow popWindow = null;
        public MainWindow()
        {
            InitializeComponent();
            btnFindPop.Click += (s, v) =>
            {
                IntPtr ptr = WpfWindow.FindWindow("FindWindow");
                if (ptr != IntPtr.Zero)
                {
                    Window w = WpfWindow.FromHwnd(ptr);
                    w.WindowState = WindowState.Normal;
                }
                else
                {
                    FindWindow w = new FindWindow();
                    w.Owner = Application.Current.MainWindow;
                    w.Show();
                }
            };
        }
        private void btnPop_Click(object sender, RoutedEventArgs e)
        {
            if (popWindow == null)
            {
                popWindow = new PopWindow();
                popWindow.Closed -= PopWindow_Closed;
                popWindow.Closed += PopWindow_Closed;
                popWindow.Owner = Application.Current.MainWindow;
                popWindow.ShowDialog();
            }
            else if (!popWindow.IsActive)
            {
                popWindow.WindowState = WindowState.Normal;
                popWindow.Activate();
            }
        }

        private void PopWindow_Closed(object sender, EventArgs e)
        {
            popWindow = null;
        }
    }
}
