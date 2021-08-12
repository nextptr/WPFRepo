using Opc.Ua;
using Opc.Ua.Client;
using OpcUaHelper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LaserControllor
{
    public class LaserInstanceControllor
    {
        #region 事件
        public delegate void GetOPCHandlerForDataStr(string data, string itemFlag, string ip);
        public static event GetOPCHandlerForDataStr GetOPCValueEventForDataStr;

        public delegate void ValueHandler(string data);
        public static event ValueHandler GetOPCMsgEvent;

        public delegate void GetOPCHandlerForDataBool(bool data, string itemFlag, string ip);
        public static event GetOPCHandlerForDataBool GetOPCValueEventForDataBool;

        public delegate void GetOPCHandlerForDataNo(double data, string itemFlag, string ip);
        public static event GetOPCHandlerForDataNo GetOPCValueEventForDataNo;

        public delegate void LaserIsCon(int data, string ip);
        public static event LaserIsCon LaserIsConnect;

        public delegate void MainLoadFreshEventHandler(int data, string ip);
        public static event MainLoadFreshEventHandler MainLoadFresh;
        #endregion

        private static LaserInstanceControllor _instance = null;
        private LaserChannel laserChannel = null;
        protected bool _isConnect = false;

        public static LaserInstanceControllor Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LaserInstanceControllor();
                return _instance;
            }
        }
        private LaserInstanceControllor()
        { }


        public void Initialize() //最先运行
        {
            string ip = "192.0.2.1:4840";
            OpcUaClient opcUaClient1 = new OpcUaClient();
            laserChannel = new LaserChannel("opc.tcp://" + ip + "", "uv1", opcUaClient1, "", "", ip);
        }
        public string GetLaserPath(string ip, string keyName)
        {
            //var item = opcPathParameter.Where(d => ((d as OpcPathParameterItem).IpAddress
            //                             == ip && (d as OpcPathParameterItem).KeyName == keyName));
            //OpcPathParameterItem paraItem = new OpcPathParameterItem();
            //if (item.Any())
            //{
            //    paraItem = item.FirstOrDefault() as OpcPathParameterItem;
            //}
            //return paraItem.Path;
            return "";
        }

        //public async Task ConnectLaser(string ip)
        //{
        //    await laserChannel.Connect();
        //    if (laserChannel.IsConn)
        //    {
        //        LaserIsConnect?.Invoke(1, ip);
        //        MainLoadFresh?.Invoke(1, ip);
        //    }
        //}
        public void ConnectLaser(string ip)
        {
            laserChannel.Connect();
            if (laserChannel.IsConn)
            {
                LaserIsConnect?.Invoke(1, ip);
                MainLoadFresh?.Invoke(1, ip);
            }
        }
        public void DisConnectLaser(string ip)
        {
            laserChannel.DisConnect();
            LaserIsConnect?.Invoke(0, ip);
        }

        public string Test()
        {
            bool val= laserChannel._uaClient.ReadNode<bool>("ns=2;s=Production.MPI - Slot03.Output.Word00.LASER_IS_ON");
            return val.ToString();
        }

        public async Task<string> GetLaserValue(string ip, string path)
        {
            try
            {
                return await laserChannel.GetLaserValue(path);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void SetLaserValue(bool flag, string ip, string path)
        {
            laserChannel.SetLaserValue(flag, path);
        }
        public bool GetLaserValueOnInner(string path, string ip)
        {
            return laserChannel.GetLaserValueOnInner(path);
        }

        public async Task<bool> ResetLaser(string ip, string prefix, string outPreFix = "")
        {
            try
            {
                laserChannel.SetLaserValue(true, prefix + ".RESET");
                Thread.Sleep(2500);
                laserChannel.SetLaserValue(false, prefix + ".RESET");
                string _errorCode = await LaserInstanceControllor.Instance.GetLaserValue(ip, "ns=2;s=" + outPreFix + ".MsgCode");
                if (_errorCode != "0")//有错误
                {
                    laserChannel.SetLaserValue(false, prefix + ".PSTART_STATICAL");//关闭激活激光
                    laserChannel.SetLaserValue(false, prefix + ".LASER_ON");//关闭
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task StartLaser(string ip)
        {
            string outPath = string.Empty;
            string inPath = string.Empty;

            outPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "outAction");
            inPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "inAction");

            string _laserOn = await laserChannel.GetLaserValue("ns=2;s=" + outPath + ".LASER_IS_ON");
            if (_laserOn.Trim() != "1")
            {
                bool isFront = false;
                if (ip.Contains("100.9") || ip.Contains("100.8"))
                {
                    isFront = true;
                }
                laserChannel.SetLaserValue(true, inPath + ".LASER_ON", isFront);
            }
        }
        public async Task CloseLaser(string ip)
        {
            string outPath = string.Empty;
            string inPath = string.Empty;

            outPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "outAction");
            inPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "inAction");

            string _laserOn = await laserChannel.GetLaserValue("ns=2;s=" + outPath + ".LASER_IS_ON");

            if (_laserOn.Trim() == "1")
            {
                laserChannel.SetLaserValue(false, inPath + ".LASER_ON");
            }
        }

        public async void ActivateLaser(string ip)
        {
            string outPath = string.Empty;
            string inPath = string.Empty;

            outPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "outAction");
            inPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "inAction");

            string PStr = await laserChannel.GetLaserValue("ns=2;s=" + outPath + ".PROG_ACTIVE");
            if (PStr.Trim() != "1")
            {
                bool isFront = false;
                if (ip.Contains("100.9") || ip.Contains("100.8"))
                {
                    isFront = true;
                }
                laserChannel.SetLaserValue(true, inPath + ".PSTART_STATICAL".ToString(), isFront);
                bool isCon = await laserChannel.GetLaserValue("ns=2;s=" + outPath + ".PROG_ACTIVE") == "1" ? true : false;
            }
        }
        public async void ReleaseActivateLaser(string ip)
        {
            string outPath = string.Empty;
            string inPath = string.Empty;
            outPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "outAction");
            inPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "inAction");

            string PStr = await laserChannel.GetLaserValue("ns=2;s=" + outPath + ".PROG_ACTIVE");
            if (PStr.Trim() == "1")
            {
                bool isFront = false;
                if (ip.Contains("100.9") || ip.Contains("100.8"))
                {
                    isFront = true;
                }
                laserChannel.SetLaserValue(false, inPath + ".PSTART_STATICAL".ToString(), isFront);//停止

            }
        }

        public async Task<string> GetLaserMsg(string ip)
        {
            string str = await laserChannel.GetLaserValue("ns=2;s=LastMessage.ShortText");
            string codeStr = await laserChannel.GetLaserValue("ns=2;s=LastMessage.MsgCode");
            return "ErrorCode:" + codeStr + "\n" + "ErrorMessage:" + str;
            // MessageEvent("MessageCode:" + codeStr + ";Message:" + str);
        }
        private async Task GetPorPower(string ip)
        {
            if (!laserChannel.IsConn)
            {
                return;
            }
            string powerPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "Power");
            string energyPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "Energy");

            double powerValue = double.Parse(await laserChannel.GetLaserValue("ns=2;s=" + powerPath + ".LASER_POWER"));
            double energyValue = double.Parse(await laserChannel.GetLaserValue("ns=2;s=" + energyPath + ".Word07.ENERGY"));

        }
        public async Task RemoveListenerLaserAnction(string ip)
        {
            laserChannel._uaClient.RemoveAllSubscription();
        }
        public async Task AddListenerLaserAnction(string path, string Name, string ip)
        {
            string originalIp = laserChannel._originalIp;
            laserChannel._uaClient.AddSubscription(Name + "," + ip + "," + originalIp, path, SubCallbackAsync);
        }

        private void SubCallbackAsync(string Name, MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs args)
        {
            string key = Name.Split(',')[0];
            string ip = Name.Split(',')[1];
            string originalIp = Name.Split(',')[2];
            // 如果有多个的订阅值都关联了当前的方法，可以通过key和monitoredItem来区分
            MonitoredItemNotification notification = args.NotificationValue as MonitoredItemNotification;
            bool value = false;
            string itemValue = string.Empty;
            double fitemValue = 0.0;
            if (notification.Value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Boolean)
            {
                // 读取到的是bool数据，在这里做处理
                value = (bool)notification.Value.WrappedValue.Value;
            }
            else if (notification.Value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.String)
            {
                // 读取到的是string数据，在这里做处理
                itemValue = (string)notification.Value.WrappedValue.Value;
            }
            else if (notification.Value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Double)
            {
                // 读取到的是double数据，在这里做处理
                fitemValue = (double)notification.Value.WrappedValue.Value;
            }
            else if (notification.Value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.UInt16)
            {
                // 读取到的是INT数据，在这里做处理
                value = notification.Value.WrappedValue.Value.ToString() == "0" ? false : true;
            }
            if (key == "btn_Laser_on" || key == "btn_Laser_on1")
            {
                // 如果有多个的订阅值都关联了当前的方法，可以通过key和monitoredItem来区分
                if (notification != null)
                {
                    if (key == "btn_Laser_on1")
                    {
                        GetOPCValueEventForDataBool(value, "arg3itemlist2", ip);
                        ////arg3itemlist[2].Flag = value;
                    }
                    else
                    {
                        GetOPCValueEventForDataBool(value, "arg1itemlist2", ip);
                        // //arg1itemlist[2].Flag = value;
                    }
                }
            }
            else if (key == "btn_Laser_ready" || key == "btn_Laser_ready1")
            {
                if (value)
                {
                    GetOPCValueEventForDataStr("1", key, ip); //待定
                    // /GetHandlerEvent("1");
                }
                else
                {
                    GetOPCValueEventForDataStr("0", key, ip);//待定
                    /// / GetHandlerEvent("0");
                }

            }
            else if (key == "btn_Laser_stop" || key == "btn_Laser_stop1") //激活
            {
                // 如果有多个的订阅值都关联了当前的方法，可以通过key和monitoredItem来区分
                if (notification != null)
                {

                    if (key == "btn_Laser_stop1")
                    {
                        GetOPCValueEventForDataBool(value, "arg3itemlist4", ip);
                        //// arg3itemlist[4].Flag = value;
                    }
                    else
                    {
                        GetOPCValueEventForDataBool(value, "arg1itemlist4", ip);
                        ////arg1itemlist[4].Flag = value;
                    }
                }

            }
            else if (key == "LASER_POWER" || key == "LASER_POWER1")
            {
                if (notification != null && fitemValue >= 0.0)
                {

                    if (key == "LASER_POWER1")
                    {
                        GetOPCValueEventForDataNo(fitemValue, "arg4itemlist0", ip);
                        //// arg4itemlist[0].Arg2 = fitemValue;// fitemValue;
                    }
                    else
                    {
                        GetOPCValueEventForDataNo(fitemValue, "arg2itemlist0", ip);
                        ////arg2itemlist[0].Arg2 = fitemValue;// fitemValue;
                    }
                }
            }

            else if (key == "ENERGY" || key == "ENERGY1")
            {
                if (notification != null && fitemValue > 0.0)
                {

                    if (key == "ENERGY1")
                    {
                        GetOPCValueEventForDataNo(fitemValue, "arg4itemlist1", ip);
                        //// arg4itemlist[1].Arg2 = fitemValue;// fitemValue;
                    }
                    else
                    {
                        GetOPCValueEventForDataNo(fitemValue, "arg2itemlist1", ip);
                        //// arg2itemlist[1].Arg2 = fitemValue;
                    }

                }
            }
            else if (key == "btn_Laser_fault" || key == "btn_Laser_fault1")
            {
                if (notification != null && value)
                {
                    if (key == "btn_Laser_fault1")
                    {
                        GetOPCValueEventForDataBool(value, "arg3itemlist3", ip);
                        ////arg3itemlist[3].Flag = value;
                    }
                    else
                    {
                        GetOPCValueEventForDataBool(value, "arg1itemlist3", ip);
                        ////arg1itemlist[3].Flag = value;
                    }
                    //// ValueHandlerEvent();
                }
            }
        }


        public async void ListenerLaserAnction(string ip, int IsLeft)
        {
            //ip=8/9 Production.MPI-Slot01.Output.Word00.FAULT_LASER
            string powerPath = string.Empty;
            string energyPath = string.Empty;
            string errorPath = string.Empty;
            string path = LaserInstanceControllor.Instance.GetLaserPath( ip.Trim(), "outAction");
            ip = "opc.tcp://" + ip.Trim() + ":4840";
            powerPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "Power");
            energyPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "Energy");
            errorPath = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "FAULT_LASER");
            if (IsLeft == 1)
            {
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + path + ".PROG_ACTIVE", "btn_Laser_stop1", ip);
                //await OPCInstanceControllor.Instance.GetActualTimeValue("ns=2;s=" + powerPath + ".LASER_POWER", "LASER_POWER1", ip);
                //await OPCInstanceControllor.Instance.GetActualTimeValue("ns=2;s=" + energyPath + ".ENERGY", "ENERGY1", ip);
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + path + ".LASER_IS_ON", "btn_Laser_on1", ip);
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + path + ".LASER_READY", "btn_Laser_ready1", ip);
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + errorPath + ".FAULT_LASER", "btn_Laser_fault1", ip);
            }
            else
            {
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + path + ".PROG_ACTIVE", "btn_Laser_stop", ip);
                // await OPCInstanceControllor.Instance.GetActualTimeValue("ns=2;s=" + powerPath + ".LASER_POWER", "LASER_POWER", ip);
                //  await OPCInstanceControllor.Instance.GetActualTimeValue("ns=2;s=" + energyPath + ".ENERGY", "ENERGY", ip);
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + path + ".LASER_IS_ON", "btn_Laser_on", ip);
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + path + ".LASER_READY", "btn_Laser_ready", ip);
                await LaserInstanceControllor.Instance.AddListenerLaserAnction("ns=2;s=" + errorPath + ".FAULT_LASER", "btn_Laser_fault", ip);
            }
        }

        public async Task<LaserStatus> GetLaserStatus(string ip)
        {
            try
            {
                string path = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "outAction");
                if (laserChannel.IsConn)
                {
                    if (await laserChannel.GetLaserValue("ns=2;s=" + path + ".LASER_IS_ON") == "1")
                    {
                        if (await laserChannel.GetLaserValue("ns=2;s=" + path + ".LASER_READY") == "1")
                        {
                            if (await laserChannel.GetLaserValue("ns=2;s=" + path + ".PROG_ACTIVE") == "1")
                            {
                                return LaserStatus.LaserActive;
                            }
                            else
                            {
                                return LaserStatus.LaserOn;
                            }
                        }
                        return LaserStatus.LaserOn;
                    }
                    else
                    {
                        return LaserStatus.None;
                    }
                }
                else
                {
                    return LaserStatus.LaserDisCon;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return LaserStatus.None;

        }
        public async Task<LaserStatus> GetLaserConnect(string ip)
        {
            try
            {
                string path = LaserInstanceControllor.Instance.GetLaserPath(ip.Trim(), "outAction");
                if (laserChannel.IsConn)
                {
                    return LaserStatus.LaserCon;
                }
                else
                {
                    return LaserStatus.LaserDisCon;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public enum LaserStatus
    {
        None,
        LaserCon,
        LaserOn,
        LaserReady,
        LaserActive,
        LaserNotActive,
        LaserStop,
        LaserDisCon
    }
}
