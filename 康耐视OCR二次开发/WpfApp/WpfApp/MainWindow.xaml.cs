using Cognex.InSight.Controls.Display;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyOfPropertyChange(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //private CvsInSightDisplay cognexView;
        //public CvsInSightDisplay CognexView
        //{
        //    get { return cognexView; }
        //    set
        //    {
        //        cognexView = value;
        //        NotifyOfPropertyChange(nameof(CognexView));
        //    }
        //}

        private ObservableCollection<string> lsData = new ObservableCollection<string>();
        public ObservableCollection<string> LsData
        {
            get { return lsData; }
            set
            {
                lsData = value;
                NotifyOfPropertyChange(nameof(LsData));
            }
        }

        private ViewArgs viewArgs = new ViewArgs();
        public ViewArgs ViewArgs
        {
            get { return viewArgs; }
            set
            {
                viewArgs = value;
                NotifyOfPropertyChange(nameof(ViewArgs));
            }
        }


        //TCPIP Client端
        private TcpClient _client;

        // 也是用于Client用
        private Thread _connectionThread;

        // 接收相机读到的码信息
        private StreamWriter _write;

        public MainWindow()
        {
            InitializeComponent();

            ls_data.DataContext = this;
            txt_ip.DataContext = ViewArgs;
            txt_user.DataContext = ViewArgs;
            txt_password.DataContext = ViewArgs;
            txt_command.DataContext = ViewArgs;

            CognexView.DefaultTextScaleMode = Cognex.InSight.Controls.Display.CvsInSightDisplay.TextScaleModeType.Proportional;
            CognexView.DialogIcon = null;
            CognexView.Name = "cvsInSightDisplay1";
            CognexView.PreferredCropScaleMode = Cognex.InSight.Controls.Display.CvsInSightDisplayCropScaleMode.Default;
            CognexView.Size = new System.Drawing.Size(488, 408);
            CognexView.TabIndex = 3;
            CognexView.ConnectedChanged += new System.EventHandler(this.cvsInSightDisplay1_ConnectedChanged);

        }

        private void msg(string arg)
        {
            this.Dispatcher.Invoke(() =>
            {
                LsData.Insert(0, arg);
                if (lsData.Count > 50)
                {
                    lsData.RemoveAt(50);
                }
            });
        }

        private void cvsInSightDisplay1_ConnectedChanged(object sender, EventArgs e)
        {
            if (CognexView.Connected)
            {
                ViewArgs.HasConnectServer = true;
                msg("设备已经连接");
            }
            else
            {
                ViewArgs.HasConnectServer = false;
                msg("设备已断开连接");
            }
        }

        private void btnConnectServer(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ViewArgs.HasConnectServer)
                {
                    msg("btn 连接Server");
                    ConnectToServer();
                }
                else
                {
                    msg("btn 断开Server");
                    StopClient();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnDisConnectServer(object sender, RoutedEventArgs e)
        {
            try
            {
                CognexView.InSight.Disconnect();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnConnectCamera(object sender, RoutedEventArgs e)
        {
            try
            {
                msg("连接相机");
                CognexView.Connect(ViewArgs.Cog_Ip, ViewArgs.Cog_User, ViewArgs.Cog_Password, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void btnDisConnectCamera(object sender, RoutedEventArgs e)
        {
            try
            {
                msg("断开相机");
                CognexView.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnLine_CheckedChanged(object sender, EventArgs e)
        {
            if (OnLineBox.IsChecked == true)
            {
                //如果勾选上了，表示设置相机线状态
                msg("Online");
                CognexView.SoftOnline = true;
            }
            else
            {
                //若为选中，则表示设置相机为离线状态
                msg("Offline");
                CognexView.SoftOnline = false;
            }
        }

        private void btnSendCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                NetworkStream netStream = null;
                netStream = _client.GetStream();
                byte[] message = Encoding.ASCII.GetBytes(ViewArgs.Cog_Command + "\r\n");
                netStream.Write(message, 0, message.Length);
                netStream.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnReadCode(object sender, RoutedEventArgs e)
        {
            try
            {
                msg("读取晶圆ID");
                NetworkStream netStream = null;
                netStream = _client.GetStream();
                byte[] message = Encoding.ASCII.GetBytes("read" + "\r\n");
                netStream.Write(message, 0, message.Length);
                netStream.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnTestState(object sender, RoutedEventArgs e)
        {
            msg("Test");


            /*
            var tt = CognexView.Connected;
            msg($"连接{tt}");
            var d = CognexView;
            */
        }

        #region func


        private void ConnectToServer()
        {
            //连接server
            try
            {
                // There is only one connection thread that is used to connect clients.
                _connectionThread = new System.Threading.Thread(new ThreadStart(ReceiveDataFromServer));
                _connectionThread.IsBackground = true;
                _connectionThread.Priority = ThreadPriority.AboveNormal;
                _connectionThread.Name = "Handle Server";
                _connectionThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Client Sample");
            }
        }

        private void ReceiveDataFromServer()
        {
            msg("启动连接线程");
            //监听tcp/IP server
            try
            {
                // Create TcpClient to initiate the connection to the server.
                _client = new TcpClient(ViewArgs.Cog_Ip, 2000);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
                msg("连接失败");
                return;
            }

            NetworkStream netStream = null;
            try
            {
                netStream = _client.GetStream();
            }
            catch (Exception ex)
            {
                // a bad connection, couldn't get the NetworkStream
                MessageBox.Show(ex.Message);
                msg("连接失败");
                return;
            }
            // Make sure we can read the data from the network stream
            if (netStream.CanRead)
            {
                msg("连接成功 开始数据监控");
                try
                {
                    // Receive buffer -- should be large enough to reduce overhead.
                    byte[] receiveBuffer = new byte[512];
                    int bytesReceived;                    // Received byte count
                                                          // Read data until server closes the connection.
                    while ((bytesReceived = netStream.Read(receiveBuffer, 0, receiveBuffer.Length)) > 0)
                    {
                        //if (_write != null)
                        //    _write.Write(Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived));

                        msg(Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived));
                        Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    // Ignore if the error is caused during shutdown
                    // since the stream and client will be closed
                    MessageBox.Show(ex.Message);
                    msg(ex.Message);
                }
            }

            msg("exit");
            StopClient();
        }

        private void StopClient()
        {
            //关闭连接
            if (_client != null)
            {
                // Close the connection
                _client.Close();

                // Wait for the thread to terminate.
                _connectionThread.Join();
                msg("断开客户端连接");
            }
        }

        #endregion

    }

    public class ViewArgs : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyOfPropertyChange(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string cog_Ip = "192.168.0.211";
        private string cog_User = "admin";
        private string cog_Password = "";
        private string cog_Command;

        public string Cog_Ip
        {
            get { return cog_Ip; }
            set
            {
                cog_Ip = value;
                NotifyOfPropertyChange(nameof(Cog_Ip));
            }
        }
        public string Cog_User
        {
            get { return cog_User; }
            set
            {
                cog_User = value;
                NotifyOfPropertyChange(nameof(Cog_User));
            }
        }
        public string Cog_Password
        {
            get { return cog_Password; }
            set
            {
                cog_Password = value;
                NotifyOfPropertyChange(nameof(Cog_Password));
            }
        }
        public string Cog_Command
        {
            get { return cog_Command; }
            set
            {
                cog_Command = value;
                NotifyOfPropertyChange(nameof(Cog_Command));
            }
        }

        public bool HasConnectServer = false;
        public bool HasConnectCamera = false;

    }

}
