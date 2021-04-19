using PowerMeterDevice.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Interf
{
    public interface IPMParamSet
    {
        double Param
        {
            set; get;
        }

        bool IsReadyWork
        {
            get; set;
        }

        event PMParamChangedEvent ParamChangedEvent;
        
    }
}
