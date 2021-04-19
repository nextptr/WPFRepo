using PowerMeterDevice.Interf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerMeterDevice.Event;

namespace PowerMeterDevice.Common
{
    public class LaserParamSet : IPMParamSet
    {
        private double param = 0.0;
        public double Param
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
                ParamChangedEvent?.Invoke(param);
            }
        }
        public bool IsReadyWork { get; set; }

        public event PMParamChangedEvent ParamChangedEvent;

    }
}
