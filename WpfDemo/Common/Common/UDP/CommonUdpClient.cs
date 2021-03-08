using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.UDP
{
    public class CommonUdpClient
    {
        public delegate void ReceiceMsgEventHandle(string e);
        public event ReceiceMsgEventHandle ReceiveMsgEvent;

        byte[] data = new byte[1024];
        bool flag = false;
        string _ip;
        int _port;

        Thread localThread = null;
        IPEndPoint serverIp = null;
        Socket serverSocket = null;
        EndPoint remote = null;

        public CommonUdpClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
            serverIp = new IPEndPoint(IPAddress.Parse(_ip), _port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            remote = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
        }
        public void Start()
        {
            if (flag == false)
            {
                flag = true;
                localThread = new Thread(MonitorThread);
                localThread.Start();
            }
        }
        public void Stop()
        {
            flag = false;
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
            while (flag)
            {
                data = new byte[1024];
                recv = serverSocket.ReceiveFrom(data, ref remote);
                if (ReceiveMsgEvent != null)
                {
                    ReceiveMsgEvent(Encoding.UTF8.GetString(data, 0, recv));
                }
            }
            serverSocket.Close();
        }
        public void SendMsg(string str)
        {
            byte[] arr = new byte[1024];
            arr = Encoding.UTF8.GetBytes(str);
            serverSocket.SendTo(arr, arr.Length, SocketFlags.None, serverIp);
        }
    }
}
