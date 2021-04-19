using PowerMeterDevice.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Interf
{
    public interface IDataFitting
    {
        string FittingType { get; set; }

        void ImportKeyValueDatas(List<TestDataItem> ls);

        double GetKeyFromValue(double inPutValue);
        double GetValueFromKey(double inPutKey);
    }
}
