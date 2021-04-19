using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Event
{
    public class PowerDataReceiveEventArgs : EventArgs
    {
        public int index;
        public double ke;
        public double val;
    }
}
