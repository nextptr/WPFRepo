using PowerMeterDevice.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerMeterDevice.Interf;

namespace PowerMeterDevice.Parameter
{
    public class HistoryDataParameter : ParameterBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<HistoryDataParameterItem> datas;
        public ObservableCollection<HistoryDataParameterItem> Datas
        {
            get
            {
                return datas;
            }
            set
            {
                datas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Datas)));
            }
        }

        public override void Copy(IParameter source)
        {
            HistoryDataParameter sp = source as HistoryDataParameter;
            if (sp != null)
            {
                this.Datas.Clear();
                foreach (var item in sp.Datas)
                {
                    this.Datas.Add(new HistoryDataParameterItem(item));
                }
            }
        }

        public void AddParam(PowerAdjustParameter dat)
        {
            HistoryDataParameterItem item = new HistoryDataParameterItem();
            item.AxisXPos = dat.AdjustPosition.AxisXPos;
            item.AxisYPos = dat.AdjustPosition.AxisYPos;
            item.AxisZPos = dat.AdjustPosition.AxisZPos;

            item.RangeSta = dat.AdjustParamSet.RangeSta;
            item.RangeEnd = dat.AdjustParamSet.RangeEnd;
            item.TestInterval = dat.AdjustParamSet.TestInterval;
            item.TimeAcq = dat.AdjustParamSet.TimeAcq;
            item.TimeCalc = dat.AdjustParamSet.TimeCalc;
            item.TimeDelay = dat.AdjustParamSet.TimeDelay;

            item.FittingType = dat.FittingType;
            item.TestDateTime = dat.TestDateTime;

            this.Datas.Add(item);
        }

        public HistoryDataParameter()
        {
            Datas = new ObservableCollection<HistoryDataParameterItem>();
        }
    }

    public class HistoryDataParameterItem : NotifyPropertyChanged
    {
        //position
        private double axisXPos;
        private double axisYPos;
        private double axisZPos;
        public double AxisXPos
        {
            get
            {
                return axisXPos;
            }
            set
            {
                axisXPos = value;
                RaisePropertyChanged(nameof(AxisXPos));
            }
        }
        public double AxisYPos
        {
            get
            {
                return axisYPos;
            }
            set
            {
                axisYPos = value;
                RaisePropertyChanged(nameof(AxisYPos));
            }
        }
        public double AxisZPos
        {
            get
            {
                return axisZPos;
            }
            set
            {
                axisZPos = value;
                RaisePropertyChanged(nameof(AxisZPos));
            }
        }

        //setting
        private double rangeSta;
        private double rangeEnd;
        private double testInterval;
        private int timeDelay;
        private int timeAcq;
        private int timeCalc;
        public double RangeSta
        {
            get
            {
                return rangeSta;
            }
            set
            {
                rangeSta = value;
                RaisePropertyChanged(nameof(RangeSta));
            }
        }
        public double RangeEnd
        {
            get
            {
                return rangeEnd;
            }
            set
            {
                rangeEnd = value;
                RaisePropertyChanged(nameof(RangeEnd));
            }
        }
        public double TestInterval
        {
            get
            {
                return testInterval;
            }
            set
            {
                testInterval = value;
                RaisePropertyChanged(nameof(TestInterval));
            }
        }
        public int TimeDelay
        {
            get
            {
                return timeDelay;
            }
            set
            {
                timeDelay = value;
                RaisePropertyChanged(nameof(TimeDelay));
            }
        }
        public int TimeAcq
        {
            get
            {
                return timeAcq;
            }
            set
            {
                timeAcq = value;
                RaisePropertyChanged(nameof(TimeAcq));
            }
        }
        public int TimeCalc
        {
            get
            {
                return timeCalc;
            }
            set
            {
                timeCalc = value;
                RaisePropertyChanged(nameof(TimeCalc));
            }
        }

        //other
        private string fittingType = "";
        private string testDateTime;
        public string FittingType
        {
            get
            {
                return fittingType;
            }
            set
            {
                fittingType = value;
                RaisePropertyChanged(nameof(FittingType));
            }
        }
        public string TestDateTime
        {
            get
            {
                return testDateTime;
            }
            set
            {
                testDateTime = value;
                RaisePropertyChanged(nameof(TestDateTime));
            }
        }


        public void Copy(HistoryDataParameterItem dat)
        {
            this.AxisXPos = dat.AxisXPos;
            this.AxisYPos = dat.AxisYPos;
            this.AxisZPos = dat.AxisZPos;
            this.RangeSta = dat.RangeSta;
            this.RangeEnd = dat.RangeEnd;
            this.TestInterval = dat.TestInterval;
            this.TimeDelay = dat.TimeDelay;
            this.TimeAcq = dat.TimeAcq;
            this.TimeCalc = dat.TimeCalc;
            this.FittingType = dat.FittingType;
            this.TestDateTime = dat.TestDateTime;
        }
        public HistoryDataParameterItem()
        { }
        public HistoryDataParameterItem(HistoryDataParameterItem dat)
        {
            this.AxisXPos = dat.AxisXPos;
            this.AxisYPos = dat.AxisYPos;
            this.AxisZPos = dat.AxisZPos;
            this.RangeSta = dat.RangeSta;
            this.RangeEnd = dat.RangeEnd;
            this.TestInterval = dat.TestInterval;
            this.TimeDelay = dat.TimeDelay;
            this.TimeAcq = dat.TimeAcq;
            this.TimeCalc = dat.TimeCalc;
            this.FittingType = dat.FittingType;
            this.TestDateTime = dat.TestDateTime;
        }
    }
}
