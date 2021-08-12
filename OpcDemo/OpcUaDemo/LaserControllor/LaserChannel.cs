using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using OpcUaHelper;

namespace LaserControllor
{
    public enum OPCType
    {
        INPUT,
        OUTPUT
    }

    public enum OPCName
    {
        INPUT,
        OUTPUT
    }
    public class LaserChannel
    {
        protected OPCName _opcName = OPCName.INPUT;
        protected OPCType _opcType = OPCType.INPUT;
        public int _opcValue;
        public string _opcPath = null;
        public string _opcIp = null;
        public string _opcN = string.Empty;
        private bool _isConn = false;
        private bool _isStart = false;
        private bool _isActive = false;
        private bool _isInterActivation = false;
        public string _pwd = string.Empty;
        public string _usrName = string.Empty;
        public string _originalIp = string.Empty;
        private int _activeCount = 0;
        private int _laserOnCount = 0;

        private static LaserChannel _instance = null;
        protected LaserChannel _opcChannel = null;
        public OpcUaClient _uaClient = new OpcUaClient();

        public bool IsConn
        {
            get { return _isConn; }
        }
        public bool IsStart //是否打开
        {
            get { return _isStart; }
        }
        public bool IsActive //是否激活
        {
            get { return _isActive; }
        }
        public bool IsInterActivation //是否激活
        {
            get { return _isInterActivation; }
        }

        public int activeCount
        {
            get { return _activeCount; }
            set { _activeCount = value; }
        }
        public int laserOnCount
        {
            get { return _laserOnCount; }
            set { _laserOnCount = value; }
        }

        //public OPCChannel(OPCType opctype, OPCName opcName, int opcValue, string opcPath)
        //{
        //    _opcType = opctype;
        //    _opcName = opcName;
        //    _opcValue = opcValue;
        //    _opcPath = opcPath;

        //}

        //public OPCChannel(string opcIp)
        //{
        //    _opcIp = opcIp;
        //}
        public LaserChannel(string opcIp, string opcName, OpcUaClient opcClient, string usrName = "", string pwd = "", string originalIp = "")
        {
            _opcIp = opcIp;
            _opcN = opcName;
            _uaClient = opcClient;
            _pwd = pwd;
            _usrName = usrName;
            _originalIp = originalIp;
        }
        public void SetLaserValue(object value, string opcPath, bool isFront = false)
        {
            if (isFront)
            {
                //OmronPlcGateway.OmronGatewayFront.WriteVariable(opcPath.ToString(), value);
            }
            else
            {
                //OmronPlcGateway.OmronGatewayRear.WriteVariable(opcPath.ToString(), value);
            }
            if (opcPath.Contains("LASER_ON"))//LASER_ON START_STATICAL
            {
                this._isStart = (bool)value;
            }
            if (opcPath.Contains("START_STATICAL"))
            {
                this._isActive = (bool)value;
                if ((bool)value)
                {
                    this._activeCount = 1;
                }
            }

        }
        public bool GetLaserValueOnInner(string opcPath, bool isFront = false)
        {
            if (isFront)
            {
                return false; //OmronPlcGateway.OmronGatewayFront.ReadVariable(opcPath.ToString());
            }
            else
            {
                return false;//OmronPlcGateway.OmronGatewayRear.ReadVariable(opcPath.ToString());
            }
        }
        public async Task<string> GetLaserValue(string nodePath)
        {
            string objValue = "";
            try
            {

                // Opc.Ua.DataValue dataValue=this._uaClient.ReadNode(nodePath);
                // if (dataValue.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Double)
                //{
                //    // 读取到的是string字符串，在这里做处理
                //    objValue = ((double)dataValue.WrappedValue.Value).ToString();
                //    return objValue;
                //}
                //nodePath = "ns=2;s=Production.MPI-Slot01.Output.Word00.LASER_WARNINGLAMP_IS_ON";
                //nodePath = "ns=2;s=Production.MPI-Slot01.Output.Word00.LASER_IS_ON";
                //nodePath = "ns=2;s=Production.MPI-Slot01.Output.Word07.ENERGY";
                if (nodePath.ToLower().Contains("energy") || nodePath.ToLower().Contains("power"))
                {
                    double pathValue = await this._uaClient.ReadNodeAsync<Double>(nodePath);

                    objValue = pathValue.ToString();

                }
                else if (nodePath.Contains("PROG_ACTIVE") || nodePath.Contains("LASER_IS_ON"))
                {
                    bool status = ((bool)await this._uaClient.ReadNodeAsync<Boolean>(nodePath));
                    objValue = status == true ? "1" : "0";
                    if (nodePath.Contains("PROG_ACTIVE")) //PROG_ACTIVE LASER_IS_ON
                    {
                        this._isActive = status;
                    }
                    if (nodePath.Contains("LASER_IS_ON")) //PROG_ACTIVE LASER_IS_ON
                    {
                        this._isStart = status;
                    }

                }
                else if (nodePath.Contains("ShortText"))
                {
                    LocalizedText pathValue1 = this._uaClient.ReadNode<LocalizedText>(nodePath);
                    objValue = pathValue1.Locale + "," + pathValue1.Text.ToString();
                }
                else if (nodePath.Contains("MsgCode"))
                {
                    int pathValue = this._uaClient.ReadNode<UInt16>(nodePath);
                    objValue = pathValue.ToString();
                }
                else if ((nodePath.Contains("EXT_ACTIVATION")))
                {
                    bool status = ((bool)await this._uaClient.ReadNodeAsync<Boolean>(nodePath));
                    this._isInterActivation = status;
                }
                //Opc.Ua.DataValue dataValue = await this._uaClient.ReadNodeAsync<DataValue>(nodePath);
                //Opc.Ua.DataValue dataValue = await this._uaClient.ReadNodeAsync<DataValue>(nodePath);
                //    if (dataValue.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Boolean)
                //    {
                //        // 读取到的是bool数据，在这里做处理
                //        if ((bool)dataValue.WrappedValue.Value)
                //        {

                //            objValue = "1";
                //        }


                //    }
                //    else if (dataValue.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.String)
                //    {
                //        // 读取到的是string字符串，在这里做处理
                //        objValue = dataValue.WrappedValue.Value.ToString();
                //    }
                //    else if (dataValue.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Double)
                //    {
                //    // 读取到的是string字符串，在这里做处理
                //        objValue =((double)dataValue.WrappedValue.Value).ToString();
                //    }
                // MessageBox.Show(objValue); // 显示测试数据

            }
            catch (Exception ex)
            {
                string pa = nodePath;
                // 使用了opc ua的错误处理机制来处理错误，网络不通或是读取拒绝
                //Opc.Ua.Client.Controls.ClientUtils.HandleException(ex.Message, ex);
            }

            // }
            return objValue;
        }

        public async Task<bool> WriteOPCValue(object value, string opcPath)
        {
            try
            {
                string nodePath = opcPath;// "ns=2;s=Setting.MPI_LaserNumber";
                return await this._uaClient.WriteNodeAsync(nodePath, 1);
            }
            catch (Exception ex)
            {
                // 使用了opc ua的错误处理机制来处理错误，网络不通或是读取拒绝
                //Opc.Ua.Client.Controls.ClientUtils.HandleException(ex.Message, ex);
                return false;
            }
        }
        public async Task<bool> Connect()
        {
            try
            {
                if (this._usrName != "" && this._pwd != "")
                {
                    this._uaClient.UserIdentity = new Opc.Ua.UserIdentity(_usrName, _pwd);
                    await this._uaClient.ConnectServer(this._opcIp);
                    this._isConn = true;
                    this._laserOnCount = 1;
                    return true;
                }
                else
                {
                    this._uaClient.UserIdentity = new Opc.Ua.UserIdentity();
                    UserIdentityToken fp = this._uaClient.UserIdentity.GetIdentityToken();
                    await this._uaClient.ConnectServer(this._opcIp);
                    this._laserOnCount = 1;
                    this._uaClient.UseSecurity = false;
                    this._isConn = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Device could not be found", "错误");
                return false;
            }
        }
        public bool DisConnect()
        {
            if (!string.IsNullOrEmpty(this._opcIp))
            {
                this._uaClient.Disconnect();
                this._isConn = false;
                this._laserOnCount = 0;
            }
            return false;
        }
    }
}
