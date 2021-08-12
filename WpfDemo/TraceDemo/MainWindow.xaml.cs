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

namespace TraceDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //博客连接
        //bool someBool = true;
        //Trace.Listeners.Add(new TextWriterTraceListener(@"D:\MyListener.log")); 
        //Trace.AutoFlush = true;//每次写入日志后是否都将其保存到磁盘中
        //Trace.WriteLine(DateTime.Now.ToString() + "--Enter function LogTest");
        //Trace.Indent(); //缩进+1
        //Trace.WriteLine("This is indented once");
        //Trace.Indent();
        //Trace.WriteLineIf(someBool, "Only written if someBool is true");  //条件输出
        //Trace.Unindent(); //缩进-1
        //Trace.Unindent();
        //Trace.WriteLine("Leave function LogTest");
        //Trace.Flush();//立即输出
        //https://blog.csdn.net/aming090/article/details/81540552
        //https://blog.csdn.net/qinyuanpei/article/details/53054002

        public MainWindow()
        {
            InitializeComponent();
            TraceLog.Info("MainWindow","This is a information message");
            TraceLog.Error("MainWindow","This is an error message");
            func();
        }

        private void func()
        {
            try
            {
                int a = 0;
                int d = 5 / a;
            }
            catch (Exception ex)
            {
                TraceLog.Error("func", ex);
            }
        }
    }
}
