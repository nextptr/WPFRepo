using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Event
{
    /// <summary>
    /// 设备发送命令超时事件
    /// </summary>
    /// <param name="args"></param>
    public delegate void DeviceTimeoutEvent(DeviceTimeoutEventArgs args);
    public class DeviceTimeoutEventArgs : EventArgs
    {
        public string CurrentDevice { get; set; }
        public string CurrentProcess { get; set; }
        public string CurrentState { get; set; }
        public DateTime CurrentDateTime { get; set; }
    }
}
