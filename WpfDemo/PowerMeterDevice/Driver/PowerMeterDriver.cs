using PowerMeterDevice.Event;
using PowerMeterDevice.Interf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PowerMeterDevice.Driver
{
    public class PowerMeterDriver : IPowerMeter
    {
        public PowerMeterDriver()
        {
            DeviceStatus = DeviceStatus.UnConnected;
            SerialHelper = new PowerMeterSPHelper();
            SerialHelper.MessageEvent += ResponseCheck;
            SerialHelper.Enable = true;
        }

        private SerialHelper SerialHelper;
        public DeviceStatus DeviceStatus { set; get; }

        private double currentPower = 0;
        public string DeviceName => "Integra";
        public string DeviceShowName { set; get; }
        public string SPortName { get; set; }

        private bool isSampling = false;
        public bool IsSampling
        {
            get
            {
                return isSampling;
            }
            set
            {
                isSampling = value;
            }
        }
        public bool IsConnected { get; set; }
        public bool IsOpen
        {
            get
            {
                return SerialHelper.IsOpen;
            }
        }
        public int WaveLength { get; set; }


        public event PowerMeterOnTimeEvent PowerMeterOnTimeEvent;
        public event PowerMeterWaveLengthEvent PowerMeterWaveLengthEvent;
        public event PowerMeterZeroEvent PowerMeterZeroEvent;
        public event PowerMeterSamplingEvent PowerMeterSamplingEvent;
        public event DeviceMessageEvent DeviceMessageEvent;
        public event DeviceTimeoutEvent DeviceTimeoutEvent
        {
            add { SerialHelper.TimeoutEvent += value; }
            remove { SerialHelper.TimeoutEvent -= value; }
        }
        public event DeviceErrorEvent DeviceErrorEvent
        {
            add { SerialHelper.ErrorEvent += value; }
            remove { SerialHelper.ErrorEvent -= value; }
        }

        private int _clock = 0;
        private bool ResponseCheck(DeviceMessageEventArgs args)
        {
            if (args.MessageType != typeof(String))
                return false;
            string str = args.Message as string;
            if (double.TryParse(str, out currentPower))
            {
                _clock++;
                if (_clock % 2 == 1)
                    return true;
                PowerMeterOnTimeEvent?.Invoke(new PowerMeterOnTimeEventArgs()
                {
                    OntimeValue = currentPower,
                });
                return true;
            }
            if ("Please Wait..." == str)
            {
                return true;
            }
            return false;
        }

        public bool Connect()
        {
            if (SPortName == "")
                return false;
            SerialHelper.SetPortName(SPortName);
            if (SerialHelper.Connect())
            {
                IsConnected = true;
                DeviceStatus = DeviceStatus.Connected;
                return true;
            }
            DeviceStatus = DeviceStatus.UnConnected;
            return false;
        }
        public bool DisConnect()
        {
            if (SerialHelper.DisConnect())
            {
                IsConnected = false;
                DeviceStatus = DeviceStatus.UnConnected;
                return true;
            }
            DeviceStatus = DeviceStatus.Error;
            return false;
        }

        public bool SetTimeOut(int MillSeconds)
        {
            return SerialHelper.SetTimeOut(MillSeconds);
        }
        public bool GetCurrentPowerValue(out double CurrentPower)
        {
            CurrentPower = this.currentPower;
            if (SerialHelper.Enable == false || isSampling == false)
                return false;
            return true;
        }
        public bool GetWaveLength(out int WaveLength)
        {
            WaveLength = int.MinValue;
            if (!SerialHelper.SendCommand("*GWL", out string response, hasResponse: true, pattern: "PWC:([^\r\n]+)\r\n"))
                return false;
            if (!Regex.IsMatch(response, @"PWC:([^\r\n]+)\r\n", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
            {
                SerialHelper.ErrorEvent?.Invoke(new DeviceErrorEventArgs()
                {
                    CurrentDateTime = DateTime.Now,
                    CurrentDevice = "PowerMeter",
                    CurrentError = "读取波长出错"
                });
                return false;
            }
            var matchCase = Regex.Match(response, @"PWC:([^\r\n]+)\r\n", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            if (int.TryParse(matchCase.Groups[1].Value, out WaveLength))
            {
                PowerMeterWaveLengthEvent?.Invoke(new PowerMeterWaveLengthEventArgs()
                {
                    WaveLength = WaveLength
                });
            }
            else
            {
                SerialHelper.ErrorEvent?.Invoke(new DeviceErrorEventArgs()
                {
                    CurrentDateTime = DateTime.Now,
                    CurrentDevice = "PowerMeter",
                    CurrentError = "读取波长出错"
                });
                return false;
            }
            return true;
        }
        public bool SetWaveLength(int WaveLength)
        {
            //构造发送数据
            string tmpStr = WaveLength.ToString();
            char[] sendArry = new char[5];
            string sendStr = "";
            if (tmpStr.Length > 5)
            {
                tmpStr.CopyTo(0, sendArry, 0, 5);
            }
            else
            {
                int i = 0;
                for (; i < (5 - tmpStr.Length); i++)
                {
                    sendArry[i] = '0';
                }
                tmpStr.CopyTo(0, sendArry, i, tmpStr.Length);
            }
            sendStr = string.Join("", sendArry);

            if (!SerialHelper.SendCommand("*PWC" + sendStr, out string nop, hasResponse: false))
                return false;
            Thread.Sleep(100);
            if (!SerialHelper.SendCommand("*GWL", out string response, hasResponse: true, pattern: "PWC:([^\r\n]+)\r\n"))
                return false;
            var matchCase = Regex.Match(response, "PWC:([^\r\n]+)\r\n", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            if (int.TryParse(matchCase.Groups[1].Value, out int waveLength) && waveLength == WaveLength)
            {
                PowerMeterWaveLengthEvent?.Invoke(new PowerMeterWaveLengthEventArgs()
                {
                    WaveLength = waveLength
                });
            }
            else
            {
                string str = "";
                str = "设置波长出错";
                if (waveLength != WaveLength)
                {
                    str = "设置波长未成功";
                }
                SerialHelper.ErrorEvent?.Invoke(new DeviceErrorEventArgs()
                {
                    CurrentDateTime = DateTime.Now,
                    CurrentDevice = "PowerMeter",
                    CurrentError = str
                });
            }
            return true;
        }
        public bool SetZero()
        {
            if (!SerialHelper.SendCommand("*SOU", out string response, hasResponse: true))
                return false;
            if (Regex.IsMatch(response, "Done!\r\n"))
                return true;
            else
                return false;
        }
        public bool CleanZero()
        {
            if (!SerialHelper.SendCommand("*COU", out string response, hasResponse: false))
                return false;

            Thread.Sleep(100);
            if (!SerialHelper.SendCommand("*GZO", out string response2, hasResponse: false))
                return false;
            var matchCase = Regex.Match(response, "Zero:([^\r\n]+)\r\n", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            if (int.TryParse(matchCase.Groups[2].Value, out int ZeroValueState))
            {
                PowerMeterZeroEvent?.Invoke(new PowerMeterZeroEventArgs()
                {
                    IsZeroSetted = ZeroValueState == 1 ? true : false
                });
                if (ZeroValueState == 1)
                    return true;
                else
                    return false;
            }
            else
            {
                SerialHelper.ErrorEvent?.Invoke(new DeviceErrorEventArgs()
                {
                    CurrentDateTime = DateTime.Now,
                    CurrentDevice = "PowerMeter",
                    CurrentError = "读取置零状态出错"
                });
                return false;
            }
        }
        public bool BeginSampling()
        {
            if (!SerialHelper.SendCommand("*CAU", out string response, hasResponse: false))
                return false;
            isSampling = true;
            PowerMeterSamplingEvent?.Invoke(new PowerMeterSamplingEventArgs() { IsSampling = true });
            return true;
        }
        public bool StopSampling()
        {
            if (!SerialHelper.SendCommand("*CSU", out string response, hasResponse: false))
                return false;
            isSampling = false;
            PowerMeterSamplingEvent?.Invoke(new PowerMeterSamplingEventArgs() { IsSampling = false });
            return true;
        }

    }
    public enum DeviceStatus
    {
        Connected,
        UnConnected,
        Idle,
        Busy,
        Error
    }
}
