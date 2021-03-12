using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Common.TCP
{
    public class tcpSocketServer
    {
        public delegate void RecieveConnectRequestEventHandler(object sender, Socket e);
        public event RecieveConnectRequestEventHandler Event_ConnectRequest; //接受数据事件

        private bool _isListen = false;
        private int _ip_port; //端口号
        private static Socket serverSocket = null;//监听socket
        private static Thread localThread = null; //监听线程      
        static int LISTNCOUNT = 30; //监听连接数量

        public bool IsListen
        {
            get
            {
                return _isListen;
            }
        }
        public string IP_Address
        {
            get
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        string ipaddress = IpEntry.AddressList[i].ToString();
                        string[] items = ipaddress.Split('.');
                        if (items[0] == "192")
                            return ipaddress;
                    }
                }
                return "";
            }
        }
        public tcpSocketServer()
        {
        }

        public void BeginListenConnectRequest(int port = 10010) //绑定端口
        {
            try
            {
                if (!_isListen)
                {
                    _ip_port = port;
                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, _ip_port));//监听所有可用ip
                    serverSocket.Listen(LISTNCOUNT);
                    //监听处理线程
                    _isListen = true;
                    localThread = new Thread(StartAcceptLoopThread);
                    localThread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void StartAcceptLoopThread()//监听连接
        {
            while (_isListen)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept();//接收到远端socket连接请求
                    Event_ConnectRequest?.Invoke(this, clientSocket);
                }
                catch (Exception ex)
                {
                    //TCP退出时Accept阻塞会触发异常，忽略即可 https://blog.csdn.net/yijun494610095/article/details/55257787
                    //MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        public void StopListenConnectRequest()
        {
            _isListen = false;
            localThread.Abort();
            serverSocket.Close();
            serverSocket.Dispose();
        }
    }

    public class tcpSocketServerProtocol
    {
        public delegate void RecieveMsgEventHandler(object sender, ReceiveArgs e);
        public event RecieveMsgEventHandler Event_ReceiveMsg; //接受数据事件

        static int BUFFSIZE = 1024; //数据接收buffer

        private static Dictionary<IPEndPoint, Socket> RemotClientSockets = new Dictionary<IPEndPoint, Socket>();//远端socket
        private static Dictionary<IPEndPoint, Stopwatch> RemoteSocketLiveTime = new Dictionary<IPEndPoint, Stopwatch>();//远端socket连接计时器（心跳）
        private static Dictionary<IPEndPoint, Thread> threadBuf = new Dictionary<IPEndPoint, Thread>();//线程表

        private static Dictionary<IPEndPoint, string> RemotClientUser = new Dictionary<IPEndPoint, string>();   //远端用户表

        public void AddRemoteSocket(Socket remoteSocket)
        {
            IPEndPoint ip = (IPEndPoint)remoteSocket.RemoteEndPoint; //远端socket的ip
            RemotClientSockets[ip] = remoteSocket; //添加到远端socket表

            Stopwatch sw = new Stopwatch();
            RemoteSocketLiveTime.Add(ip, sw);  //添加RemoteSocket的计时器
            RemoteSocketLiveTime[ip].Restart();//启动RemoteSocket的计时器

            Thread receiveThread = new Thread(MessageListenThread);
            threadBuf[ip] = receiveThread;    //添加远端数据接收线程
            receiveThread.Start(remoteSocket);//启动远端数据接收线程
        }
        private void MessageListenThread(object reoteSocket)//接收远端socket数据处理函数
        {
            Socket remoteSocket = (Socket)reoteSocket;
            IPEndPoint ip = (IPEndPoint)remoteSocket.RemoteEndPoint;
            byte[] result = new byte[BUFFSIZE];
            TcpPocket pocket = new TcpPocket();
            try
            {
                while (true)
                {
                    if (remoteSocket.Available <= 0) continue;
                    int receiveNumber = remoteSocket.Receive(result);//接收数据
                    if (receiveNumber > 0)
                    {
                        string recStr = Encoding.UTF8.GetString(result, 0, receiveNumber);
                        if (!pocket.AnalyseCommand(recStr))
                        {
                            continue;
                        }

                        if (pocket.Command == SocketCommand.ActConnected)//连接
                        {
                            //回复客户端状态
                            //发送外部消息
                            remoteSocket.Send(Encoding.UTF8.GetBytes(pocket.ConStructCommand(SocketCommand.ActConnected, SocketCommand.True, SocketCommand.IdfServer, "连接成功,欢迎!")));
                            Event_ReceiveMsg?.Invoke(this, new ReceiveArgs(SocketCommand.ActConnected, SocketCommand.True, pocket.Identify, pocket.ConStructShowMsg()));
                        }
                        else if (pocket.Command == SocketCommand.ActAbortSocket)//退出
                        {
                            //发送外部消息
                            if (RemotClientUser.ContainsValue(pocket.Identify))
                            { //已注册
                                Event_ReceiveMsg?.Invoke(this, new ReceiveArgs(SocketCommand.ActAbortSocket, SocketCommand.True, pocket.Identify, pocket.ConStructShowMsg()));
                            }
                            else
                            {//未注册
                                Event_ReceiveMsg?.Invoke(this, new ReceiveArgs(SocketCommand.ActAbortSocket, SocketCommand.False, pocket.Identify, pocket.ConStructShowMsg()));
                            }
                            break;
                        }
                        else if (pocket.Command == SocketCommand.ActALive)//心跳
                        {
                            RemoteSocketLiveTime[ip].Restart();
                        }
                        else if (pocket.Command == SocketCommand.ActRegister)//注册
                        {
                            //查询是否重名
                            //回复客户端状态
                            if (RemotClientUser.ContainsValue(pocket.Identify)) //已经存在
                            {
                                remoteSocket.Send(Encoding.UTF8.GetBytes(pocket.ConStructCommand(SocketCommand.ActRegister, SocketCommand.False, SocketCommand.IdfServer, "注册失败,此用户已存在!")));
                            }
                            else
                            {
                                RemotClientUser[ip] = pocket.Identify;
                                remoteSocket.Send(Encoding.UTF8.GetBytes(pocket.ConStructCommand(SocketCommand.ActRegister, SocketCommand.True, SocketCommand.IdfServer, "注册成功!")));
                                Event_ReceiveMsg?.Invoke(this, new ReceiveArgs(SocketCommand.ActRegister, SocketCommand.True, pocket.Identify, pocket.ConStructShowMsg()));
                            }
                        }
                        else if (pocket.Command == SocketCommand.ActMsg)//消息
                        {
                            //查询是否注册
                            //转发消息
                            //发送外部消息
                            if (RemotClientUser.ContainsValue(pocket.Identify))
                            {
                                SendAnotherSocketMessage(ip, pocket.ConStructCommand());
                                Event_ReceiveMsg?.Invoke(this, new ReceiveArgs(SocketCommand.ActMsg, pocket.Result, pocket.Identify, pocket.ConStructShowMsg()));
                            }
                        }
                    }
                    if (RemoteSocketLiveTime[ip].ElapsedMilliseconds > SocketCommand.ServerHeartbeatClock)
                    {//心跳机制
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //退出
            threadBuf[ip] = null;
            if (RemotClientSockets.Count != 0 && RemotClientSockets[ip] != null)
            {
                RemotClientSockets[ip].Shutdown(SocketShutdown.Both);
                RemotClientSockets[ip].Close();
                RemotClientSockets[ip] = null;
            }
            if (RemoteSocketLiveTime.Count != 0 && RemoteSocketLiveTime[ip] != null)
            {
                RemoteSocketLiveTime[ip] = null;
            }
            if (RemotClientUser.Count != 0 && RemotClientUser.ContainsKey(ip))
            {
                RemotClientUser[ip] = null;
            }
        }

        public void DeleteRemoteSocket()
        {
            foreach (var ip in RemotClientSockets.Keys)
            {
                RemoteSocketLiveTime[ip] = null;

                RemotClientSockets[ip].Shutdown(SocketShutdown.Both);
                RemotClientSockets[ip].Close();
                RemotClientSockets[ip] = null;

                RemotClientUser[ip] = null;

                threadBuf[ip].Abort();
                threadBuf[ip] = null;
            }
        }

        private void SendAnotherSocketMessage(IPEndPoint localip, string str)//发送数据
        {
            TcpPocket pocket = new TcpPocket();
            pocket.Identify = RemotClientUser[localip]; 
            foreach (IPEndPoint ip in RemotClientSockets.Keys)
            {
                try
                {
                    if (localip != ip)
                    {
                        if (RemotClientSockets[ip] != null && RemotClientSockets[ip].Connected)
                        {
                            RemotClientSockets[ip].Send(Encoding.UTF8.GetBytes(pocket.ConStructCommand(SocketCommand.ActMsg, SocketCommand.True, pocket.Identify, str)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void SendEverSocketMessage(string str)//发送数据
        {
            TcpPocket pocket = new TcpPocket();
            foreach (IPEndPoint ip in RemotClientSockets.Keys)
            {
                try
                {
                    if (RemotClientSockets[ip] != null && RemotClientSockets[ip].Connected)
                    {
                        RemotClientSockets[ip].Send(Encoding.UTF8.GetBytes(pocket.ConStructCommand(SocketCommand.ActMsg, SocketCommand.True, SocketCommand.IdfServer, str)));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }

}
