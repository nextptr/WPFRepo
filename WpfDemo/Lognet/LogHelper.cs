using log4net;
using log4net.Appender;
using System.Linq;

namespace Lognet
{
    public class LogHelper
    {
        /*
           %m(message):输出的日志消息，如ILog.Debug(…)输出的一条消息
           %n(new line)：換行
           %d(datetime):输出当前语句运行的时刻 
           %r(run time):输出程序从运行到执行到当前语句时消耗的毫秒数 
           %t(thread id):当前语句所在的线程ID
           %p(priority): 日志的当前优先级别，即DEBUG、INFO、WARN…等
           %c(class):当前日志对象的名称
           %L：输出语句所在的行号
           %F：输出语句所在的文件名
           %-数字：表示该项的最小长度，如果不够，则用空格填充
         */

        public static readonly log4net.ILog leftLog = log4net.LogManager.GetLogger("LeftLogger");
        public static readonly log4net.ILog righLog = log4net.LogManager.GetLogger("RighLogger");
        public static readonly log4net.ILog testLog = log4net.LogManager.GetLogger("TestLogger");
        public static void wrLt(string msg)
        {
            leftLog.Info(msg);
        }
        public static void wrRt(string msg)
        {
            righLog.Info(msg);
        }
        public static void wrTest(string msg)
        {
            testLog.Info(msg);
        }

        /// <summary>
        /// folder=日志文件存储路径
        /// </summary>
        /// <param name="folder"></param>
        public static void ChangeFolder(string folder)
        {
            var repository = LogManager.GetRepository();
            var appenders = repository.GetAppenders();
            if (appenders == null)
                return;

            foreach (var app in appenders)
            {
                if (app.Name.Equals("ErrorAppender") || app.Name.Equals("InfoAppender"))
                {
                    var ra = app as RollingFileAppender;
                    ra.File = folder;
                    ra.ActivateOptions();
                }
            }
        }

        /// <summary>
        /// file=日志文件名
        /// </summary>
        /// <param name="file"></param>
        public static void ChangeFile(string fileName)
        {
            var repository = LogManager.GetRepository();
            var appenders = repository.GetAppenders();
            var targetApder = appenders.First(p => p.Name == "RunLog") as log4net.Appender.RollingFileAppender;
            targetApder.File = fileName;
            targetApder.Writer = new System.IO.StreamWriter(targetApder.File, targetApder.AppendToFile, targetApder.Encoding);
            //targetApder.ActivateOptions();
            //ILog logger = LogManager.GetLogger(GetType());
            //logger.Error(ex);
        }
    }
}