using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Event
{
    /// <summary>
    /// 设备发出信息事件
    /// </summary>
    /// <param name="args"></param>
    public delegate bool DeviceMessageEvent(DeviceMessageEventArgs args);
    public class DeviceMessageEventArgs
    {
        public object Message { get; set; }
        public Type MessageType { get; set; }
        public DateTime CurrentDateTime { get; set; }
    }
}
