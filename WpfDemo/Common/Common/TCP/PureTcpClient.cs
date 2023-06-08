using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Common.TCP
{
    public class PureTcpClient : NotifyPropertyChanged
    {
        public delegate void RecieveMsgEventHandler(object sender, params byte[] arr);//接收数据事件
        public event RecieveMsgEventHandler Event_RecieveMsg;

        protected int Port = 0;     //端口号
        protected IPAddress Ip = null; //ip
        protected Socket clientSocket = null; //本机socket
        protected Thread localThread = null;//监听线程
        private static int BUFFSIZE = 1024;   //接受数据buffer长度

        private bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        public void RefrashStatus()
        {
            if (clientSocket == null)
            {
                IsConnected = false;
                return;
            }
            IsConnected = clientSocket.Connected;
        }

        public PureTcpClient()
        {

        }
        ~PureTcpClient()
        {
            Disconnect();
        }

        public bool Connect(string ip, int port) //连接服务器
        {
            Ip = IPAddress.Parse(ip);
            Port = port;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(new IPEndPoint(Ip, Port));
                if (clientSocket.Connected)
                {
                    localThread = new Thread(MessageListenThread);
                    localThread.Start(clientSocket);
                    return true;
                }
                else
                {
                    MessageBox.Show("服务器连接失败！");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器连接失败:" + ex.Message);
                return false;
            }
            finally
            {
                RefrashStatus();
            }
        }
        public void Disconnect()//断开连接
        {
            TcpPocket pocket = new TcpPocket();
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
            RefrashStatus();
        }
        private void MessageListenThread(object socketObj) //接收数据
        {
            Socket clientSocket = (Socket)socketObj;
            byte[] recBufff = new byte[BUFFSIZE];
            while (clientSocket.Connected)
            {
                try
                {
                    int receiveNumber = clientSocket.Receive(recBufff);
                    byte[] msgArr = new byte[receiveNumber];
                    for (int i = 0; i < receiveNumber; i++)
                    {
                        msgArr[i] = recBufff[i];
                    }
                    Event_RecieveMsg?.Invoke(this, msgArr);
                }
                catch (Exception e)
                {
                    MessageBox.Show("接收数据失败:" + e.Message);
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
        public void SendCommand(string str)//发送数据
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(str));
            }
        }
        public void SendCommand(byte[]byteArr)
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Send(byteArr);
            }
        }
    }

}
