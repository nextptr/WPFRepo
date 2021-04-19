using PowerMeterDevice.Common;
using PowerMeterDevice.Parameter;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PowerMeterDevice
{
    /// <summary>
    /// HistoryDataView.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryDataView : Window
    {
        private CsvHelper csvHelper = null;
        public delegate void ChoiceDataItemEventHandle(PowerAdjustParameter obj);
        public event ChoiceDataItemEventHandle ChoiceDataItemEvent;
        private HistoryDataModel mode = new HistoryDataModel();

        public HistoryDataView()
        {
            InitializeComponent();
            mode.Param = ParameterManager.Instance.historyDataParameter;
            mode.SelectData = new PowerAdjustParameter();
            mode.IsSelected = false;
            this.DataContext = mode;
            string dir = Directory.GetCurrentDirectory() + @"\PowerAdjustHistoryData";
            csvHelper = new CsvHelper(dir);
        }

        private void btnApplyItem(object sender, RoutedEventArgs e)
        {
            ChoiceDataItemEvent?.Invoke(mode.SelectData);
        }
        private void btnDeleteItem(object sender, RoutedEventArgs e)
        {
            bool isDelete = false;
            int index = 0;
            for (index = 0; index < mode.Param.Datas.Count; index++)
            {
                if (mode.Param.Datas[index].TestDateTime == mode.SelectData.TestDateTime)
                {
                    isDelete = true;
                    break;
                }
            }
            if (isDelete)
            {
                mode.Param.Datas.RemoveAt(index);
                mode.SelectData.AdjustDatas.LineDatas.Clear();
                mode.Param.Write();
                if (mode.Param.Datas.Count <= 0)
                {
                    mode.IsSelected = false;
                }
            }
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs arg)
        {
            if (arg.AddedItems.Count <= 0)
                return;

            HistoryDataParameterItem item = arg.AddedItems[0] as HistoryDataParameterItem;
            if (item == null)
                return;

            //copy data 
            mode.SelectData.AdjustPosition.AxisXPos = item.AxisXPos;
            mode.SelectData.AdjustPosition.AxisYPos = item.AxisYPos;
            mode.SelectData.AdjustPosition.AxisZPos = item.AxisZPos;
           
            mode.SelectData.AdjustParamSet.RangeSta = item.RangeSta;
            mode.SelectData.AdjustParamSet.RangeEnd = item.RangeEnd;
            mode.SelectData.AdjustParamSet.TestInterval = item.TestInterval;
            mode.SelectData.AdjustParamSet.TimeDelay = item.TimeDelay;
            mode.SelectData.AdjustParamSet.TimeAcq = item.TimeAcq;
            mode.SelectData.AdjustParamSet.TimeCalc = item.TimeCalc;
            
            mode.SelectData.TestDateTime = item.TestDateTime;
            mode.SelectData.FittingType = item.FittingType;
            
            mode.IsSelected = true;
            List<KeyValuePair<double, double>> ls = csvHelper.Read(item.TestDateTime);
            if (ls == null)
                return;
            mode.SelectData.AdjustDatas.LineDatas.Clear();
            int id = 0;
            foreach (var dat in ls)
            {
                mode.SelectData.AdjustDatas.LineDatas.Add(new TestDataItem(id, dat.Key, dat.Value));
                id++;
            }
        }
    }

    public class HistoryDataModel: NotifyPropertyChanged
    {
        private HistoryDataParameter param;
        public HistoryDataParameter Param
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
                RaisePropertyChanged(nameof(Param));
            }
        }

        private PowerAdjustParameter selectData;
        public PowerAdjustParameter SelectData
        {
            get
            {
                return selectData;
            }
            set
            {
                selectData = value;
                RaisePropertyChanged(nameof(SelectData));
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

    }
}
