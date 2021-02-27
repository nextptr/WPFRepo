using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WpfDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();

            //进程名称被修改
            Process processes = Process.GetCurrentProcess();
            if (processes.ProcessName != assembly.Name)
            {
                MessageBox.Show("错误:进程名称和程序集名称不统一!");
                System.Environment.Exit(0);
            }

            //进程重复启动
            Process[] myProcesses = Process.GetProcessesByName(assembly.Name);//获取指定的进程名
            if (myProcesses.Length > 1)
            {
                MessageBox.Show("错误:程序已经启动!");
                System.Environment.Exit(0);
            }

            //程序运行中
            bool createNew = false;
            Semaphore singleInstanceWatcher = new Semaphore(1, 1, assembly.Name, out createNew);
            if (!createNew)
            {
                MessageBox.Show("错误:程序运行中!");
                System.Environment.Exit(0);
            }

            //parameter.Init();//parameter数据加载
            bool checkUser = true;  //用户登陆和密码验证
            if (checkUser)
            {
                if (File.Exists(@"C:\Runtime"))
                {
                    //_initHardware(); //初始化硬件
                    //
                }

                //显示主界面
                //VM_WindowMain vm = new VM_WindowMain();
                //vm.CloseCallBack += (s, el) =>
                //{
                //    _uninit();
                //    System.Environment.Exit(0);
                //};
                //Application.Current.MainWindow = vm.UIElement as Window;
                //vm.Show();
            }
            else
            {
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// 程序加载完成
        /// </summary>
        protected override void OnLoadCompleted(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 程序退出
        /// </summary>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Process.GetProcessById(Process.GetCurrentProcess().Id).Kill();
            //System.Environment.Exit(0);
        }
    }
}
