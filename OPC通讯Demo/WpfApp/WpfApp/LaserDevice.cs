using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class LaserDevice
    {
        private OpcUaClient _uaClient = null;
        public OpcUaClient UaClient
        {
            get
            {
                return _uaClient;
            }
        }

        private string _opcIp = "";
        public string OpcIp
        {
            get
            {
                return _opcIp;
            }
        }

        public string _pwd = string.Empty;
        public string _usrName = string.Empty;

        public LaserDevice(string opcIp)
        {
            _uaClient = new OpcUaClient();
            _opcIp = "opc.tcp://" + opcIp;
        }
        public LaserDevice(string opcIp,string name,string pwd)
        {
            _uaClient = new OpcUaClient();
            _opcIp = "opc.tcp://" + opcIp;
            _usrName = name;
            _pwd = pwd;
        }


        public void Connect()
        {
            try
            {
                //this._uaClient.UserIdentity = new Opc.Ua.UserIdentity(_usrName, _pwd);
                //this._uaClient.ConnectServer(this._opcIp);

                this._uaClient.UserIdentity = new Opc.Ua.UserIdentity();
                UserIdentityToken fp = this._uaClient.UserIdentity.GetIdentityToken();
                this._uaClient.ConnectServer(this._opcIp);
                this._uaClient.UseSecurity = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DisConnect()
        {
            try
            {
                this._uaClient.Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ReadNode(string path)
        {
            if (UaClient == null)
            {
                return false;
            }
            return UaClient.ReadNode<bool>(path);
        }
        public object ReadIntNode(string path)
        {
            if (UaClient == null)
            {
                return -1;
            }
            var val = UaClient.ReadNode<UInt16>(path);
            return val;
        }
        public bool ReadBoolNode(string path)
        {
            if (UaClient == null)
            {
                return false;
            }
            var dat = UaClient.ReadNode<Boolean>(path);
            return dat;
        }
        public bool WriteNode(string path, UInt16 value)
        {
            if (UaClient == null)
            {
                return false;
            }
            UaClient.WriteNode<UInt16>(path, (UInt16)value);
            return true;
        }
        public bool WriteBoolNode(string path, bool value)
        {
            if (UaClient == null)
            {
                return false;
            }
            UaClient.WriteNode<bool>(path, value);
            return true;
        }
        public bool WriteIntNode(string path, UInt16 value)
        {
            if (UaClient == null)
            {
                return false;
            }
            UaClient.WriteNode<UInt16>(path, value);
            return true;
        }

        public bool WriteStringNode(string path, string value)
        {
            if (UaClient == null)
            {
                return false;
            }
            UaClient.WriteNode<string>(path, value);
            return true;
        }


        public async Task<bool> WriteOPCValue(object value, string opcPath)
        {
            try
            {
                return await this._uaClient.WriteNodeAsync(opcPath, value);
            }
            catch (Exception ex)
            {
                // 使用了opc ua的错误处理机制来处理错误，网络不通或是读取拒绝
                //Opc.Ua.Client.Controls.ClientUtils.HandleException(ex.Message, ex);
                return false;
            }
        }
    }
}
