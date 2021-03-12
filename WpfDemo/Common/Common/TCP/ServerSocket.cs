using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Common.TCP
{
    public class ServerSocket
    {
        const int MAX_LISTEN_CLINET = 10;
        byte[] buffer = new byte[SocketCommon.RECV_BUFSIZE];

        public delegate void ClientSocketHandler(object sender, string IP);
        public event ClientSocketHandler AsyncNewClient;
        public event ClientSocketHandler AsyncClientClosed;
        public event ReceiveMsgHandler AsyncDataReceive;

        public int IP_Port
        {
            get;
            private set;
        }

        Socket listenSocket = null;
        Dictionary<string, Socket> clientList = new Dictionary<string, Socket>();
        private static object lockobj = new object();

        public ServerSocket(int port = 10010)
        {
            this.IP_Port = port;
        }

        public string IP_Address   //只监听250网段
        {
            get
            {
                string HostName = Dns.GetHostName();
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
                        if (items[2] == "250")
                            return ipaddress;
                    }
                }
                return "";
            }
        }

        public void Connect()
        {
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.SendBufferSize = SocketCommon.SEND_BUFSIZE;
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, IP_Port));
            listenSocket.Listen(MAX_LISTEN_CLINET);
            BeginAccept();
        }
        public void Disconnect()
        {
            List<Socket> temp = new List<Socket>();
            foreach (Socket client in clientList.Values)
            {
                temp.Add(client);
            }
            foreach (Socket client in temp)
            {
                if (client != null && client.Connected == true)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
            clientList.Clear();
            if (listenSocket != null)
            {
                listenSocket.Close();
            }
        }


        private void BeginAccept()
        {
            listenSocket.BeginAccept(new AsyncCallback(AsyncAcceptCallBack), listenSocket);
        }

        private void AsyncAcceptCallBack(IAsyncResult IA)
        {
            try
            {
                Socket communiSocket = (IA.AsyncState as Socket).EndAccept(IA);
                OnClientConnect(communiSocket);
                BeginAccept();
                BeginReceive(communiSocket);
            }
            catch
            {
                //服务器停止运行
            }
        }

        private void OnClientConnect(Socket communiSocket)
        {
            IPEndPoint ipClient = communiSocket.RemoteEndPoint as IPEndPoint;
            string IP = ipClient.Address.ToString();
            if (clientList.ContainsKey(IP))
            {
                clientList[IP].Shutdown(SocketShutdown.Both);
                clientList[IP].Close();
                clientList[IP] = communiSocket;
            }
            else
            {
                clientList.Add(IP, communiSocket);
                AsyncNewClient?.Invoke(this, IP);
            }
        }

        private void BeginReceive(Socket communiSocket)
        {
            lock (lockobj)
            {
                communiSocket.BeginReceive(buffer, 0, SocketCommon.RECV_BUFSIZE, SocketFlags.None, new AsyncCallback(AsyncReceiveCallBack), communiSocket);
            }
        }

        private void AsyncReceiveCallBack(IAsyncResult IA)
        {
            Socket communiSocket = IA.AsyncState as Socket;
            try
            {
                int dataLength = communiSocket.EndReceive(IA);
                string msg = "";
                //客户端调用shutdown关闭连接
                if (dataLength == 0)
                {
                    OnClientClose(communiSocket);
                }
                else
                {
                    msg = Encoding.Default.GetString(buffer, 0, dataLength);
                    if (msg == "HEARTBEAT")
                    {
                        SendMessage((communiSocket.RemoteEndPoint as IPEndPoint).Address.ToString(), "HEARTBEAT");
                    }
                    else
                    {
                        AsyncDataReceive?.Invoke(this, msg);
                    }
                }
                BeginReceive(communiSocket);
            }
            catch (Exception ex)
            {
                OnClientClose(communiSocket);
            }
        }

        private void OnClientClose(Socket communiSocket)
        {
            try
            {
                IPEndPoint ipClient = communiSocket.RemoteEndPoint as IPEndPoint;
                string IP = ipClient.Address.ToString();
                if (clientList.ContainsKey(IP))
                {
                    clientList.Remove(IP);
                }
                communiSocket.Dispose();
                AsyncClientClosed?.Invoke(this, IP);
            }
            catch
            {
                //服务器主动断开连接
            }
        }
        public void SendMessage(string ip, string msg)
        {
            if (clientList.ContainsKey(ip))
            {
                clientList[ip].Send(Encoding.Default.GetBytes(msg));
            }
        }
        public void SendMessage(string str)
        {
            foreach (string ip in clientList.Keys)
            {
                SendMessage(ip, str);
            }
        }



    }
}
