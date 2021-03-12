using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TCP
{
    public delegate void ReceiveMsgHandler(object sender, string msg);
    public class SocketCommon
    {
        public const int SEND_BUFSIZE = 1024 * 20;
        public const int RECV_BUFSIZE = 1024 * 20;
    }
    public class SocketCommand
    {
        public static readonly string ActAbortSocket = "AbortSocket";  //退出
        public static readonly string ActRegister = "Register";        //注册
        public static readonly string ActALive = "ALive";              //心跳机制
        public static readonly string ActMsg = "MSG";                  //消息
        public static readonly string ActConnected = "Connect";        //连接状态

        public static readonly string True  = "T";      //true
        public static readonly string False = "F";      //false

        public static readonly string IdfServer =  "serv";       //server
        public static readonly string IdfRemoter = "rmter";      //remoter

        public static readonly int ServerHeartbeatClock = 3000;  //服务端心跳计时
        public static readonly int ClientHeartbeatClock = 1000;  //客户端心跳计时
    }

    public class ReceiveArgs
    {
        private string _message;
        private string _action;
        private string _result;
        private string _id;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
        public string Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public ReceiveArgs(string act, string ret,string id, string Msg)
        {
            _action = act;
            _result = ret;
            _id = id;
            _message = Msg;
        }
    }

    public class TcpPocket
    {
        public string Command = "";
        public string Result = "";
        public string Identify = "";
        public string Contain = "";
        public bool AnalyseCommand(string str)
        {
            string[] buffer = str.Split('_');
            if (buffer.Length >= 4)
            {
                Command = buffer[0];
                Result = buffer[1];
                Identify = buffer[2];
                Contain = buffer[3];
                return true;
            }
            return false;
        }
        public string ConStructCommand()
        {
            string msg = Command + "_" + Result + "_" + Identify + "_" + Contain;
            return msg;
        }
        public string ConStructCommand(string cmd, string result, string id, string cont)
        {
            string msg = cmd + "_" + result + "_" + id + "_" + cont;
            return msg;
        }
        public string ConStructShowMsg()
        {
            string msg = Identify + ":" + Contain;
            return msg;
        }
    }
}
