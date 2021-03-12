using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.TCP
{
    ///1.构造函数创建远端IP节点
    ///2.创建socket变量连接远端ip节点，开启异步数据监听和心跳包
    ///3.数据接收、解析、触发相关的接收消息、断开连接、心跳包事件
    public class ClientSocket
    {
        public bool ISConnect { get; set; } = false;           //连接状态
        byte[] _buffer = new byte[SocketCommon.RECV_BUFSIZE];  //接收数据缓冲区
        IPEndPoint _remoteIPE = null;                          //远端IP
        Socket _clientSocekt = null;                           //底层socket
        Thread _heartThread = null;                            //心跳线程
        bool _heartBeatEnable = false;                         //心跳线程控制
        bool _alive = false;                                   //心跳结果

        public event ReceiveMsgHandler ReceiveData;
        public event ReceiveMsgHandler OnServerClose;

        public ClientSocket(string ip, int port)
        {
            _remoteIPE = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public bool Connect(int heartBeatMillSeconds = 5000)
        {
            if (_clientSocekt != null && _clientSocekt.Connected && (_clientSocekt.RemoteEndPoint as IPEndPoint).Equals(_remoteIPE))
            {
                return true;
            }
            //已经连接了Server时，是否可以直接new,是否需要先断开连接？
            _clientSocekt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//1.创建socket
            try
            {
                _clientSocekt.Connect(_remoteIPE);//2.连接服务器socket
            }
            catch (Exception e)
            {
                ///服务未开启:立即返回
                ///网络不可达:立即返回
                ///连接超时,等待一段时间返回
                ISConnect = false;
                return false;
            }
            if (_clientSocekt.Connected)
            {
                ISConnect = true;
                BeginReceive();//3.开始接收数据
                HeartBeat(heartBeatMillSeconds);//4.发送心跳包
                return true;
            }
            ISConnect = false;
            return false;
        }
        public void Disconnect()
        {
            ISConnect = false;
            if (_clientSocekt != null && _clientSocekt.Connected)
            {
                _clientSocekt.Shutdown(SocketShutdown.Both);
                _clientSocekt.Close();
                //不加这句EndReceive会异常
            }
            ExitHeartBeat();
        }
        public void SendMessage(string str)
        {
            if (ISConnect)
            {
                _clientSocekt.Send(Encoding.Default.GetBytes(str));
            }
        }

        private void BeginReceive()
        {
            _clientSocekt.BeginReceive(_buffer, 0, SocketCommon.RECV_BUFSIZE, SocketFlags.None, new AsyncCallback(AsyncReceiveCallBack), _clientSocekt);
        }
        private void AsyncReceiveCallBack(IAsyncResult IA)
        {
            Socket socket = IA.AsyncState as Socket;
            if (socket != null && socket.Connected)
            {
                try
                {
                    int datalength = (IA.AsyncState as Socket).EndReceive(IA);
                    string msg = "";
                    if (datalength == 0)//1.断开连接
                    {
                        Disconnect();
                        OnServerClose?.Invoke(this, "服务器断开连接");
                    }
                    else
                    {
                        msg = Encoding.Default.GetString(_buffer, 0, datalength);
                        if (_heartBeatEnable && msg == "HEARTBEAT")//2.心跳数据解析
                        {
                            _alive = true;
                        }
                        else
                        {
                            ReceiveData?.Invoke(this, msg);//3.数据接收
                        }
                    }
                }
                catch
                { }
            }
        }


        private void HeartBeat(int heartBeatMillSeconds)
        {
            if (_heartBeatEnable == false)
                return;
            _alive = true;

            if (ISConnect)
            {
                _heartThread = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(heartBeatMillSeconds);
                        if (!_alive)
                        {
                            Disconnect();
                            OnServerClose?.Invoke(this, "服务器心跳停止");
                        }
                        _alive = false;
                        SendMessage("HEARTBEAT");
                    }
                });
                _heartThread.Start();
            }
        }
        private void ExitHeartBeat()
        {
            if (_heartThread != null && _heartThread.IsAlive)
            {
                try
                {
                    _heartThread.Abort();
                }
                catch
                { }
            }
            _alive = false;
        }
    }
}
