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
    class PureTcpServer
    {
        public delegate void RecieveConnectRequestEventHandler(object sender, Socket e);
        public event RecieveConnectRequestEventHandler Event_ConnectRequest; //接受数据事件

        private bool _isListen = false;
        private int _ip_port; //端口号
        private static Socket serverSocket = null;//监听socket
        private static Thread localThread = null; //监听线程      
        static int LISTNCOUNT = 5; //监听连接数量

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
        public PureTcpServer()
        {
        }

        //Socket监听
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

    public class PureTcpServerTerminal
    {
        public delegate void RecieveMsgEventHandler(object sender, string e);
        public event RecieveMsgEventHandler Event_ReceiveMsg; //接受数据事件

        public int BUFFSIZE = 1024; //数据接收buffer
        private Socket RemoteSocket;
        private Thread LocalThread;

        public void ConnectRemoteSocket(Socket remoteSocket)
        {
            RemoteSocket = remoteSocket; 
            LocalThread = new Thread(MessageListenThread);
            LocalThread.Start(remoteSocket);
        }
        private void MessageListenThread()//接收远端socket数据处理函数
        {
            byte[] result = new byte[BUFFSIZE];
            try
            {
                while (true)
                {
                    if (RemoteSocket.Available <= 0) continue;
                    int receiveNumber = RemoteSocket.Receive(result);//接收数据
                    if (receiveNumber > 0)
                    {
                        string recStr = Encoding.UTF8.GetString(result, 0, receiveNumber);
                        Event_ReceiveMsg?.Invoke(this, recStr);
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DisconnectRemoteSocket()
        {
            RemoteSocket?.Shutdown(SocketShutdown.Both);
            RemoteSocket?.Close();
            RemoteSocket = null;

            LocalThread?.Abort();
            LocalThread = null;
        }

        public void SendMessage(string str)//发送数据
        {
            RemoteSocket?.Send(Encoding.UTF8.GetBytes(str));
        }
    }
}
