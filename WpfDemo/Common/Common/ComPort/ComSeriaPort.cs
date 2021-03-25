using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;


namespace Common.ComPort
{
    public class ComSeriaPort : NotifyPropertyChanged
    {
        public delegate void ReceiveDataEventHandle(object sender, object data);
        public event ReceiveDataEventHandle ReceiveDataEvent; //接受数据事件

        private SerialPort ComDevice = null;
        private string[] Buffer = null;
        private string SourceData = null;
        private ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();

        private bool _comConnected = false;
        private string _comName = "";
        private int _comBaud = 0;

        public bool ComConnected
        {
            get
            {
                if (ComDevice.IsOpen)
                {
                    _comConnected = true;
                }
                else
                {
                    _comConnected = false;
                }
                return _comConnected;
            }
            set
            {
                _comConnected = value;
                OnPropertyChanged("ComConnected");
            }
        }
        public string ComName
        {
            get
            {
                return _comName;
            }
            set
            {
                _comName = value;
                OnPropertyChanged("ComName");
            }
        }
        public int ComBaud
        {
            get
            {
                return _comBaud;
            }
            set
            {
                _comBaud = value;
                OnPropertyChanged("ComBaud");
            }
        }

        public ComSeriaPort()
        {
            ComDevice = new SerialPort();
        }

        ///初始化设备
        public bool OpenEvenParity()
        {
            try
            {
                if (_comBaud == 0)
                {
                    MessageBox.Show("打开串口失败,波特率为0!");
                    return false;
                }
                if (_comName == "")
                {
                    MessageBox.Show("打开串口失败,串口号为空!");
                    return false;
                }
                ComDevice.PortName = _comName;
                ComDevice.BaudRate = _comBaud;
                ComDevice.DataBits = 8;
                ComDevice.Parity = Parity.Even;
                ComDevice.StopBits = StopBits.One;
                ComDevice.DataReceived -= ComDevice_DataReceived;
                ComDevice.DataReceived += ComDevice_DataReceived;
                ComDevice.Open();
                ComConnected = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("打开串口失败:" + e.Message);
                return false;
            }
        }
        public bool OpenNoneParity()
        {
            try
            {
                if (_comBaud == 0)
                {
                    MessageBox.Show("打开串口失败,波特率为0!");
                    return false;
                }
                if (_comName == "")
                {
                    MessageBox.Show("打开串口失败,串口号为空!");
                    return false;
                }
                ComDevice.PortName = _comName;
                ComDevice.BaudRate = _comBaud;
                ComDevice.DataBits = 8;
                ComDevice.Parity = Parity.None;
                ComDevice.StopBits = StopBits.One;
                ComDevice.DataReceived -= ComDevice_DataReceived;
                ComDevice.DataReceived += ComDevice_DataReceived;
                ComDevice.Open();
                ComConnected = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("打开串口失败:" + e.Message);
                return false;
            }
        }
        public void Close()
        {
            try
            {
                if (ComDevice.IsOpen)
                {
                    ComDevice.DataReceived -= ComDevice_DataReceived;
                    ComDevice.Close();
                    ComConnected = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("关闭串口失败:" + e.Message);
            }
        }

        ///发送数据
        public void SendHexCommand(string command)
        {
            byte[] sendData = ComMath.Hex16StringToByteArry(command);
            if (ComDevice.IsOpen)
            {
                try
                {
                    ComDevice.Write(sendData, 0, sendData.Length);//发送数据
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送失败：" + ex.Message);
                }
            }
        }
        public void SendByteCommand(byte[] sendData)
        {
            if (ComDevice.IsOpen)
            {
                try
                {
                    ComDevice.Write(sendData, 0, sendData.Length);//发送数据
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送失败：" + ex.Message);
                }
            }
        }

        ///发送数据
        public void SendASCIICommand(string command)
        {
            try
            {
                byte[] sendData = null;
                sendData = Encoding.ASCII.GetBytes(command);
                SendByteCommand(sendData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送失败：" + ex.Message);
            }
        }
        public void SendDefaultCommand(string command)
        {
            try
            {
                byte[] sendData = null;
                sendData = Encoding.Default.GetBytes(command);
                SendByteCommand(sendData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送失败：" + ex.Message);
            }
        }

        ///读取数据事件
        private void ComDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] ReDatas = new byte[ComDevice.BytesToRead];
                ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
                SourceData = aSCIIEncoding.GetString(ReDatas);
                ReceiveDataEvent?.Invoke(this, SourceData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("接受数据出错:" + ex.Message);
            }
        }

        public static int GetBaud(int index)
        {
            int Baud = 9600;
            switch (index)
            {
                case 0:
                    {
                        Baud = 2400;
                    }
                    break;
                case 1:
                    {
                        Baud = 4800;
                    }
                    break;
                case 2:
                    {
                        Baud = 9600;
                    }
                    break;
                case 3:
                    {
                        Baud = 14400;
                    }
                    break;
                case 4:
                    {
                        Baud = 19200;
                    }
                    break;
                case 5:
                    {
                        Baud = 38400;
                    }
                    break;
                case 6:
                    {
                        Baud = 56000;
                    }
                    break;
                case 7:
                    {
                        Baud = 57600;
                    }
                    break;
                case 8:
                    {
                        Baud = 115200;
                    }
                    break;
                default:
                    Baud = 9600;
                    break;
            }
            return Baud;
        }
    }
}