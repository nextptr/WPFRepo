using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.UDP
{
    public class CommonUdpServer
    {
        public delegate void ReceiceMsgEventHandle(string e);
        public event ReceiceMsgEventHandle ReceiveMsgEvent;

        byte[] data = new byte[1024];
      
        private bool _isMonitor = false;
        public bool IsMonitor
        {
            get
            {
                return _isMonitor;
            }
        }


        Thread localThread = null;
        IPEndPoint serverIp = null;
        Socket serverSocket = null;
        EndPoint remote = null;

        public CommonUdpServer(int bindPort)
        {
            serverIp = new IPEndPoint(IPAddress.Any, bindPort);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(serverIp);
            remote = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
        }
        public void Start()
        {
            if (_isMonitor == false)
            {
                _isMonitor = true;
                localThread = new Thread(MonitorThread);
                localThread.Start();
            }
        }
        public void Stop()
        {
            _isMonitor = false;
            if (localThread != null)
            {
                localThread.Abort();
                localThread = null;
            }
        }
        public void Exit()
        {
            Stop();
            serverSocket.Close();
        }

        public void MonitorThread()
        {
            int recv = 0;
            while (_isMonitor)
            {
                data = new byte[1024];
                recv = serverSocket.ReceiveFrom(data, ref remote);
                if (ReceiveMsgEvent != null)
                {
                    ReceiveMsgEvent(Encoding.UTF8.GetString(data, 0, recv));
                }
            }
        }
        public void SendMsg(string str)
        {
            byte[] arr = new byte[1024];
            arr = Encoding.UTF8.GetBytes(str);
            serverSocket.SendTo(arr, arr.Length, SocketFlags.None, remote);
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
    }
}
