using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceDemo
{
    public enum MessageType
    {
        Info = 0,
        Warn = 1,
        Erro = 2
    }
    public class TraceLog
    {
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

        private TraceLog()
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            var logFileName = logPath +
                string.Format("Log-{0}.txt", DateTime.Now.ToString("yyyyMMdd"));

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TextWriterTraceListener(logFileName));
            Trace.AutoFlush = true;
        }

        private static TraceLog _instance = null;
        private static TraceLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TraceLog();
                }
                return _instance;
            }
        }

        public static void Error(string source,object message)
        {
            Exception ex = message as Exception;
            if (ex == null)
            {
                Instance.Log(message, MessageType.Erro, source);
            }
            else
            {
                Instance.Log(ex.Message + ex.StackTrace, MessageType.Erro, source);
            }
        }

        public static void Warning(string source,object message)
        {
            Instance.Log(message, MessageType.Warn, source);
        }

        public static void Info(string source, object message)
        {
            Instance.Log(message, MessageType.Info, source);
        }

        private void Log(object message, MessageType type, object source)
        {
            Trace.WriteLine(
                string.Format("{0},{1},{2},{3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                type.ToString(),
                source,
                message));
        }
    }

}
