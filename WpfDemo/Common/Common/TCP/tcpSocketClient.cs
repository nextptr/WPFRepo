using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Common.TCP
{
    public class tcpSocketClient: NotifyPropertyChanged
    {
        public delegate void RecieveMsgEventHandler(object sender, object e);//接收数据事件
        public event RecieveMsgEventHandler Event_RecieveMsg;

        protected int Port;     //端口号
        protected IPAddress Ip; //ip
        protected Socket clientSocket = null; //本机socket
        protected Thread localThread = null;//监听线程
        private static int BUFFSIZE = 1024;   //接受数据buffer长度
        private DispatcherTimer _time = new DispatcherTimer();

        private string userName = "zjw";//用户名
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private bool registerFlag = false;
        public bool RegisterFlag
        {
            get
            {
                return registerFlag;
            }
            set
            {
                registerFlag = value;
                OnPropertyChanged(nameof(registerFlag));
            }
        }

        public tcpSocketClient()
        {
            _time.Tick += _time_Tick;
            _time.Interval = new TimeSpan(0, 0, 0, 0, SocketCommand.ClientHeartbeatClock / 2);
        }
        private void _time_Tick(object sender, EventArgs e)
        {
            TcpPocket pocket = new TcpPocket();
            sendMessage(pocket.ConStructCommand(SocketCommand.ActALive, SocketCommand.True, UserName, "心跳"));
        }
        ~tcpSocketClient()
        {
            Disconnect();
        }

        public bool Connect(string ip, int port) //连接服务器
        {
            Ip = IPAddress.Parse(ip);
            Port = port;

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpPocket pocket = new TcpPocket();
            try
            {
                clientSocket.Connect(new IPEndPoint(Ip, Port));
                if (clientSocket.Connected)
                {
                    _time.Start();
                    localThread = new Thread(MessageListenThread);
                    localThread.Start(clientSocket);
                    clientSocket.Send(Encoding.UTF8.GetBytes(pocket.ConStructCommand(SocketCommand.ActConnected, SocketCommand.True, SocketCommand.IdfRemoter, "testConnect")));
                    return true;
                }
                else
                {
                    Event_RecieveMsg?.Invoke(this, "服务器连接失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public void Disconnect()//断开连接
        {
            TcpPocket pocket = new TcpPocket();
            Event_RecieveMsg?.Invoke(this, "已经断开服务器连接");
            sendMessage(pocket.ConStructCommand(SocketCommand.ActAbortSocket, SocketCommand.True, UserName, "断开"));
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSocket = null;
            }
            if (localThread != null)
            {
                localThread.Abort();
                localThread = null;
            }
        }
        private void MessageListenThread(object socketObj) //接收数据
        {
            Socket clientSocket = (Socket)socketObj;
            byte[] recBufff = new byte[BUFFSIZE];
            TcpPocket pocket = new TcpPocket();
            while (clientSocket.Connected)
            {
                try
                {
                    int receiveNumber = clientSocket.Receive(recBufff);
                    string recStr = Encoding.UTF8.GetString(recBufff, 0, receiveNumber);
                    if (!pocket.AnalyseCommand(recStr))
                    {
                        continue;
                    }

                    if (pocket.Command == SocketCommand.ActConnected)
                    {
                        if (pocket.Result == SocketCommand.True)
                        {
                            Event_RecieveMsg?.Invoke(this, "服务器连接成功");
                        }
                    }
                    else if (pocket.Command == SocketCommand.ActRegister)//注册
                    {
                        if (pocket.Result == SocketCommand.True)
                        {
                            RegisterFlag = true;
                            Event_RecieveMsg?.Invoke(this, pocket.Identify + ":注册成功");
                        }
                        else
                        {
                            RegisterFlag = false;
                            Event_RecieveMsg?.Invoke(this, pocket.Identify + ":" + pocket.Contain);
                        }
                    }
                    else if (RegisterFlag == true && pocket.Command == SocketCommand.ActMsg)//消息
                    {
                        Event_RecieveMsg?.Invoke(this, pocket.Identify + ":" + pocket.Contain);
                    }
                }
                catch (Exception e)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            //退出
            if (this.clientSocket != null && this.clientSocket.Connected)
            {
                this.clientSocket.Shutdown(SocketShutdown.Both);
                this.clientSocket.Close();
            }
            this.clientSocket = null;
            localThread = null;
        }
        private void sendMessage(string str)//发送数据
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(str));
            }
        }
        public void SendMessage(string str)//发送数据
        {
            if (RegisterFlag)
            {
                TcpPocket pocket = new TcpPocket();
                sendMessage(pocket.ConStructCommand(SocketCommand.ActMsg, SocketCommand.True, UserName, str));
            }
        }

        public void RegistUserName()
        {
            TcpPocket pocket = new TcpPocket();
            sendMessage(pocket.ConStructCommand(SocketCommand.ActRegister, SocketCommand.True, UserName, "注册"));
        }
    }
}
