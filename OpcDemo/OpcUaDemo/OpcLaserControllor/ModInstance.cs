using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpcLaserControllor
{
    public class ModInstance
    {
        private ModInstance() { }
        public static ModInstance _instance = null;
        public static ModInstance Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModInstance();
                }
                return _instance;
            }
        }

        public LaserDevice laserDevice = null;

        public delegate void TestEventHadle(EventArgs arg);
        public event TestEventHadle TestEvent;

        public void Init(string ip)
        {
            try
            {
                laserDevice = new LaserDevice(ip);
                laserDevice.UaClient.ConnectComplete += UaClient_ConnectComplete;
                laserDevice.Connect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UaClient_ConnectComplete(object sender, EventArgs e)
        {
            TestEvent?.Invoke(e);
        }

        public void UnInit()
        {
            try
            {
                laserDevice?.DisConnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
