using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.TCP
{
    public class TcpServerManager: NotifyPropertyChanged
    {
        public delegate void RecieveMsgEventHandler(object sender, object e);
        public event RecieveMsgEventHandler Event_ReceiveMsg; //接受数据事件

        private TcpServerManager()
        {
            TcpServer = new tcpSocketServer();
            tcpSocketServerProtocol = new tcpSocketServerProtocol();
        }
        public static TcpServerManager Instance
        {
            get
            {
                return internalClass.instance;
            }
        }
        private class internalClass
        {
            static internalClass()
            { }
            internal static readonly TcpServerManager instance = new TcpServerManager();
        }

        private int userCount;
        public int UserCount
        {
            get
            {
                return userCount;
            }
            set
            {
                userCount = value;
                OnPropertyChanged(nameof(UserCount));
            }
        }

        private string serverIp;
        public string ServerIp
        {
            get
            {
                return serverIp;
            }
            set
            {
                serverIp = value;
                OnPropertyChanged(nameof(ServerIp));
            }
        }

        private tcpSocketServer tcpServer;
        public tcpSocketServer TcpServer
        {
            get
            {
                return tcpServer;
            }
            set
            {
                tcpServer = value;
                OnPropertyChanged(nameof(TcpServer));
            }
        }

        private tcpSocketServerProtocol tcpSocketServerProtocol;
        public tcpSocketServerProtocol TcpSocketServerProtocol
        {
            get
            {
                return tcpSocketServerProtocol;
            }
        }

        public bool StartListen(int port)
        {
            try
            {
                TcpServer.Event_ConnectRequest -= TcpServer_Event_ConnectRequest;
                TcpServer.Event_ConnectRequest += TcpServer_Event_ConnectRequest;
                TcpServer.BeginListenConnectRequest(port);

                TcpSocketServerProtocol.Event_ReceiveMsg -= TcpSocketServerProtocol_Event_ReceiveMsg;
                TcpSocketServerProtocol.Event_ReceiveMsg += TcpSocketServerProtocol_Event_ReceiveMsg;

                ServerIp = TcpServer.IP_Address;
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private void TcpServer_Event_ConnectRequest(object sender, System.Net.Sockets.Socket e)
        {
            tcpSocketServerProtocol.AddRemoteSocket(e);
        }
        private void TcpSocketServerProtocol_Event_ReceiveMsg(object sender, ReceiveArgs e)
        {
            if (e.Action == SocketCommand.ActConnected)
            {
            }
            else if (e.Action == SocketCommand.ActAbortSocket)
            {
                if (e.Result == SocketCommand.True)
                {
                    UserCount--;
                    Event_ReceiveMsg?.Invoke(this, e.Id + ":" + "退出");
                }
            }
            else if (e.Action == SocketCommand.ActRegister)
            {
                UserCount++;
                Event_ReceiveMsg?.Invoke(this, e.Id + ":" + "注册成功");
            }
            else if (e.Action == SocketCommand.ActMsg)
            {
                Event_ReceiveMsg?.Invoke(this, e.Message);
            }
        }

        public void StopListen()
        {
            try
            {
                TcpServer.Event_ConnectRequest -= TcpServer_Event_ConnectRequest;
                TcpServer.StopListenConnectRequest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
