using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpcLaserControllor
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

        public LaserDevice(string opcIp)
        {
            _uaClient = new OpcUaClient();
            _opcIp = "opc.tcp://"+opcIp;
        }


        public void Connect()
        {
            try
            {
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

        public DataValue ReadNode(string path)
        {
            if (UaClient == null)
            {
                return null;
            }
            return UaClient.ReadNode(path);
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
        public bool WriteNode(string path,object value)
        {
            if (UaClient == null)
            {
                return false;
            }
            UaClient.WriteNode(path, value);
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

    }
}
