using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Event
{
    public delegate void DeviceErrorEvent(DeviceErrorEventArgs args);
    public class DeviceErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 当前设备ID
        /// </summary>
        public string CurrentDevice { get; set; }

        /// <summary>
        /// 当前出错的过程描述
        /// </summary>
        public string CurrentProcess { get; set; }

        /// <summary>
        /// 当前错误
        /// </summary>
        public string CurrentError { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime CurrentDateTime { set; get; }
    }
}
