using PowerMeterDevice.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerMeterDevice.Driver
{
    public abstract class SerialHelper
    {

        #region 需要重写的函数，也是不同设备串口协议可能不同的地方

        protected abstract string ResponseEOF { get; }

        /// <summary>
        /// 用正则判断接收的数据是否正常
        /// </summary>
        /// <param name="response">响应</param>
        /// <returns>是否正常</returns>
        protected abstract bool IsResponseNormal(string response);

        /// <summary>
        /// 检查响应，找到响应的数据并显示
        /// </summary>
        /// <param name="response">待匹配的字符</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="groupindex">取匹配字符的位置</param>
        /// <param name="Amplify">匹配成功，传出数字</param>
        /// <returns>是否匹配</returns>
        public abstract bool CheckResponse(string response, string pattern, int groupindex, out int Amplify);

        /// <summary>
        /// 检查响应，找到响应的数据并显示
        /// </summary>
        /// <param name="response">待匹配的字符</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="groupindex">取匹配字符的位置</param>
        /// <param name="RetStr">匹配成功，传出字符</param>
        /// <returns>是否匹配</returns>
        public abstract bool CheckResponse(string response, string pattern, int groupindex, out string retStr);

        /// <summary>
        /// 校验位
        /// </summary>
        /// <param name="precedingByte"></param>
        /// <returns></returns>
        public virtual byte[] CheckSum(byte[] precedingByte)
        {
            int sum = 0;
            byte[] checksum = new byte[2];

            foreach (byte item in precedingByte)
            {
                sum += item;
            }

            checksum[0] = (byte)((sum >> 8) & 0xFF);        //高8位
            checksum[1] = (byte)(sum & 0xFF);               //低位

            return checksum;
        }

        /// <summary>
        /// 定义与激光器交互的编码格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual byte[] BuildCmd(string message)
        {
            string strcmd = message;
            return WriteToByte(strcmd);
        }
        /// <summary>
        /// 定义与激光器交互的编码格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual string ReadFromByte(byte[] data)
        {
            return Encoding.Default.GetString(data);
        }

        protected virtual byte[] WriteToByte(string str)
        {
            return Encoding.Default.GetBytes(str);
        }
        #endregion


        public SerialHelper()
        {
            //AVIA page 84
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            port.Parity = Parity.None;
            port.BaudRate = baudRate_;
            port.ErrorReceived += Port_ErrorReceived;
            port.DataReceived += Port_DataReceived;
            portName = "COM3";
            timeout = 300;
            port.DtrEnable = true;//设置DTR为高电平
            port.RtsEnable = true;//设置RTS位高电平
        }

        public virtual bool Enable
        {
            set;
            get;
        }

        private int baudRate_ = 115200;
        /// <summary>
        /// 一般情况下串口波特率比较多，其他串口参数不怎么变
        /// </summary>
        /// <param name="baudRate">波特率</param>
        public void SetBaudRate(int baudRate)
        {
            baudRate_ = baudRate;
            port.BaudRate = baudRate_;
        }
        public void SetPortName(string portName_)
        {
            portName = portName_;
        }

        public void InvokeError(string errorMessage)
        {
            if (!Enable)
                return;
            DeviceErrorEventArgs args = new DeviceErrorEventArgs()
            {
                CurrentDateTime = DateTime.Now,
                CurrentError = errorMessage,
            };
            ErrorEvent?.Invoke(args);
        }

        //未读完的响应
        private string unresolveResponse = "";
        //串口响应，只保存5条，采用了线程安全队列
        private ConcurrentQueue<string> serialResponse = new ConcurrentQueue<string>();

        /// <summary>
        /// 如果在流的尾字节上出现奇偶校验错误，将向输入缓冲区添加一个值为 126 的额外字节。将在辅助线程上引发 ErrorReceived 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int bytesToRead = port.BytesToRead;
                byte[] ReDatas = new byte[bytesToRead];
                lock (this)
                {
                    //读串口数据
                    port.Read(ReDatas, 0, bytesToRead);
                }
                string mid = ReadFromByte(ReDatas);
                unresolveResponse += mid;

                //查看是否以结束符结尾
                if (unresolveResponse.EndsWith(ResponseEOF))
                {
                    //using (StreamWriter sw = new StreamWriter("ResponsList.txt", true))
                    //{
                    //    sw.Write(unresolveResponse);
                    //    sw.WriteLine();
                    //}
                    foreach (var str in Regex.Split(unresolveResponse, ResponseEOF, RegexOptions.IgnoreCase))
                    {
                        if (str.Length < 1)
                            continue;
                        string rp = str + ResponseEOF;

                        //这里应先判断接收数据是否正常
                        if (!IsResponseNormal(rp))
                        {
                            DeviceErrorEventArgs _error = new DeviceErrorEventArgs()
                            {
                                CurrentDateTime = DateTime.Now,
                                CurrentProcess = "ReceiveBuffer",
                                CurrentError = "Response AbNormal: " + rp
                            };
                            ErrorEvent?.Invoke(_error);
                            continue;
                        }

                        //发出接收信息事件
                        DeviceMessageEventArgs _eventArgs = new DeviceMessageEventArgs
                        {
                            CurrentDateTime = DateTime.Now,
                            Message = rp,
                            MessageType = rp.GetType()
                        };

                        //如果没有事件能处理该信息，则把信息保存到缓存队列里
                        bool flag = false;
                        if (!(MessageEvent is null))
                        {
                            foreach (DeviceMessageEvent dele in MessageEvent.GetInvocationList())
                            {
                                if (dele.Invoke(_eventArgs))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (!flag)
                        {
                            //将信息保存到缓存队列里，缓存队列由本类对象自己产生
                            if (serialResponse.Count > 5)
                            {
                                serialResponse.TryDequeue(out _);
                            }
                            serialResponse.Enqueue(rp);
                        }
                    }
                    //处理完成，设置响应缓存为""
                    unresolveResponse = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        /// <summary>
        // 摘要:
        //     输入的缓冲区溢出。 输入缓冲区中没有空间不足或之后接收到的文件尾 (EOF) 字符。
        // RXOver = 1,
        //
        // 摘要:
        //     发生字符缓冲区溢出。 下一个字符会丢失。
        // Overrun = 2,
        //
        // 摘要:
        //     硬件中检测到奇偶校验错误。
        // RXParity = 4,
        //
        // 摘要:
        //     硬件检测到了分帧错误。
        // Frame = 8,
        //
        // 摘要:
        //     应用程序尝试传输一个字符，但输出缓冲区已满。
        // TXFull = 256
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            SerialError error = e.EventType;
            DeviceErrorEventArgs ee = new DeviceErrorEventArgs();
            //由error构建ee
            //待完善...
            ErrorEvent?.Invoke(ee);
            port.DiscardInBuffer();
            //重启串口
            if (port.IsOpen)
                port.Close();
            port.Open();
        }

        /// <summary>
        /// 下面的三个委托将作为SerialHelper使用者的事件，
        /// 只能在本类中调用Invoke，不可new\+=\-=
        /// </summary>
        public DeviceErrorEvent ErrorEvent;
        public DeviceTimeoutEvent TimeoutEvent;
        public DeviceMessageEvent MessageEvent;

        /// <summary>
        /// 同步方式发命令，等待直到响应返回或超时
        /// </summary>
        /// <param name="cmdMessage"></param>
        /// <returns>是否正常发送命令</returns>
        public bool SendCommand(string cmdMessage, out string response, bool hasResponse = true, string pattern = "")
        {
            response = "";
            if (!Enable)
                return false;
            if (port.IsOpen)
            {
                ////清串口
                //port.DiscardInBuffer();
                //port.DiscardOutBuffer();

                //构建命令
                byte[] sendData = BuildCmd(cmdMessage);

                lock (this)
                {
                    try
                    {
                        //port.RtsEnable = true;
                        port.Write(sendData, 0, sendData.Length);//发送数据
                        //if (port.BytesToWrite == 0)
                        //{
                        //    port.RtsEnable = false;
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                //假如没有返回请求
                if (!hasResponse)
                    return true;

                //超时机制
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    //联合原命令与响应，判断该响应是否正常
                    if (serialResponse.Count <= 0)
                        continue;
                    //尝试取出当前的队列头
                    if (serialResponse.TryPeek(out string res) == false)
                        continue;
                    //未设置匹配字符，或匹配成功，尝试取出当前的Response并返回
                    if (pattern == "" || Regex.IsMatch(res, pattern))
                    {
                        if (serialResponse.TryDequeue(out response) == false)
                            continue;
                        return true;
                    }
                }

                stopwatch.Stop();

                //本次未匹配成功，为免影响后续匹配，将所有响应清空
                while (serialResponse.Count > 0)
                    serialResponse.TryDequeue(out string nop);

                DeviceTimeoutEventArgs args = new DeviceTimeoutEventArgs()
                {
                    CurrentDateTime = DateTime.Now,
                    CurrentProcess = "SendMessage",
                };
                TimeoutEvent?.Invoke(args);
            }
            else
            {
                InvokeError("串口未打开！");
            }
            return false;
        }

        public string GetResponse()
        {
            if (serialResponse.Count > 0)
            {
                if (serialResponse.TryDequeue(out string mid))
                    return mid;
            }
            return null;
        }

        protected int timeout;
        public bool SetTimeOut(int MillSeconds)
        {
            timeout = MillSeconds;
            lock (this)
            {
                if (port == null)
                    return false;
                port.ReadTimeout = MillSeconds;
                port.WriteTimeout = MillSeconds;
                if (port.IsOpen)
                {
                    port.Close();
                    port.Open();
                }
                return true;
            }
        }

        public bool Connect()
        {
            lock (this)
            {
                if (portName is null)
                    return false;
                if (port.IsOpen && port.PortName == portName)
                    return true;
                else
                {
                    port.Close();
                    port.PortName = portName;
                }
                try
                {
                    port.Open();
                }
                catch (Exception e)
                {
                    this.ErrorEvent?.Invoke(new DeviceErrorEventArgs()
                    {
                        CurrentDateTime = DateTime.Now,
                        CurrentDevice = this.portName,
                        CurrentError = e.ToString(),
                        CurrentProcess = "PortOpen"
                    });
                    return false;
                }
                if (port.IsOpen)
                    return true;
                return false;
            }
        }
        public bool DisConnect()
        {
            lock (this)
            {
                if (port.IsOpen)
                    port.Close();
                if (port.IsOpen)
                    return false;
                return true;
            }
        }
        protected SerialPort port = new SerialPort();
        protected string portName;

        public bool IsOpen
        {
            get
            {
                return port.IsOpen;
            }
        }
    }

    public class PowerMeterSPHelper : SerialHelper
    {
        protected override string ResponseEOF => "\r\n";

        public override bool CheckResponse(string response, string pattern, int groupindex, out int Amplify)
        {
            //throw new NotImplementedException();
            if (Regex.IsMatch(response, pattern))
            {
                var matchcase = Regex.Match(response, pattern);
                string str = matchcase.Groups[groupindex].Value;
                if (int.TryParse(str, out Amplify))
                    return true;
            }
            Amplify = -int.MaxValue;
            return false;
        }

        public override bool CheckResponse(string response, string pattern, int groupindex, out string retStr)
        {
            //throw new NotImplementedException();
            if (Regex.IsMatch(response, pattern))
            {
                var matchcase = Regex.Match(response, pattern);
                retStr = matchcase.Groups[groupindex].Value;
                return true;
            }
            retStr = "";
            return false;
        }

        protected override bool IsResponseNormal(string str)
        {
            //throw new NotImplementedException();
            return true;
        }

        protected override string ReadFromByte(byte[] data)
        {
            return Encoding.ASCII.GetString(data);
        }

        protected override byte[] WriteToByte(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
    }
}
