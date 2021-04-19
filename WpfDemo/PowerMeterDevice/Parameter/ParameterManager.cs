using PowerMeterDevice.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Parameter
{
    public class ParameterManager
    {
        private CsvHelper csvHelper = null;
        private ParameterManager()
        {
            string dir = Directory.GetCurrentDirectory() + @"\PowerAdjustHistoryData";
            csvHelper = new CsvHelper(dir);
        }
        private static ParameterManager _instance;
        public static ParameterManager Instance
        {
            get
            {
                return localObj.instance;
            }
        }

        private class localObj
        {
            static localObj() { }
            internal static readonly ParameterManager instance = new ParameterManager();
        }


        public PowerAdjustParameter powerAdjustParameter = new PowerAdjustParameter();
        public HistoryDataParameter historyDataParameter = new HistoryDataParameter();

        public void AddPowerAdjustParameter(PowerAdjustParameter dat)
        {
            historyDataParameter.AddParam(dat);
            historyDataParameter.Write();
            writeAdjustData(dat);
        }
        private void writeAdjustData(PowerAdjustParameter dat)
        {
            if (csvHelper == null)
                return;
            List<KeyValuePair<double, double>> ls = new List<KeyValuePair<double, double>>();

            foreach (var item in dat.AdjustDatas.LineDatas)
            {
                ls.Add(new KeyValuePair<double, double>(item.TestKey, item.TestValue));
            }
            csvHelper.Write(dat.TestDateTime, ls);
        }

        public void WriteFile()
        {
            powerAdjustParameter.Write();
            historyDataParameter.Write();
        }
        public void ReadFile()
        {
            powerAdjustParameter.Read();
            historyDataParameter.Read();
        }
    }
}
