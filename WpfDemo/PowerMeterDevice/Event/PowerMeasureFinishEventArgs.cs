using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Event
{
    public class PowerMeasureFinishEventArgs : EventArgs
    {
        public bool StepStatus;
        public PowerMeasureFinishEventArgs(bool flg)
        {
            StepStatus = flg;
        }
    }
}
