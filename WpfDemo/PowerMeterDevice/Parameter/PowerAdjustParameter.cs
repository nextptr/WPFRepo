using PowerMeterDevice.Common;
using PowerMeterDevice.Interf;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PowerMeterDevice.Parameter
{
    public class PowerAdjustParameter : ParameterBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //位置
        private TestPosition adjustPosition;
        public TestPosition AdjustPosition
        {
            get
            {
                return adjustPosition;
            }
            set
            {
                adjustPosition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdjustPosition)));
            }
        }

        //流程参数设定
        private TestParam adjustParamSet;
        public TestParam AdjustParamSet
        {
            get
            {
                return adjustParamSet;
            }
            set
            {
                adjustParamSet = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdjustParamSet)));
            }
        }

        //校正数据
        private TestDatas adjustDatas;
        public TestDatas AdjustDatas
        {
            get
            {
                return adjustDatas;
            }
            set
            {
                adjustDatas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdjustDatas)));
            }
        }

        //拟合算法类型
        private string fittingType = "";
        public string FittingType
        {
            get
            {
                return fittingType;
            }
            set
            {
                fittingType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FittingType)));
            }
        }

        private string testDateTime;
        public string TestDateTime
        {
            get
            {
                return testDateTime;
            }
            set
            {
                testDateTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TestDateTime)));
            }
        }

        public PowerAdjustParameter()
        {
            AdjustPosition = new TestPosition();
            AdjustParamSet = new TestParam();
            AdjustDatas = new TestDatas();
        }

        public override void Copy(IParameter source)
        {
            PowerAdjustParameter sp = source as PowerAdjustParameter;
            if (sp != null)
            {
                this.AdjustPosition.Copy(sp.AdjustPosition);
                this.AdjustParamSet.Copy(sp.AdjustParamSet);
                this.AdjustDatas.Copy(sp.AdjustDatas);
                this.FittingType = sp.FittingType;
                this.TestDateTime = sp.TestDateTime;
            }
        }
    }

    public class TestPosition : NotifyPropertyChanged
    {
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

        public void Copy(TestPosition sp)
        {
            this.AxisXPos = sp.AxisXPos;
            this.AxisYPos = sp.AxisYPos;
            this.AxisZPos = sp.AxisZPos;
        }
        public TestPosition()
        { }
    }

    public class TestParam : NotifyPropertyChanged
    {
        private double rangeSta = 0;
        private double rangeEnd = 2;
        private double testInterval = 1;
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

        private int timeDelay = 1;
        private int timeAcq = 1;
        private int timeCalc = 1;
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

        public void Copy(TestParam sp)
        {
            this.RangeSta = sp.RangeSta;
            this.RangeEnd = sp.RangeEnd;
            this.TestInterval = sp.TestInterval;
            this.TimeDelay = sp.TimeDelay;
            this.TimeAcq = sp.TimeAcq;
            this.TimeCalc = sp.TimeCalc;
        }
        public TestParam()
        { }
    }

    public class TestDatas : NotifyPropertyChanged
    {
        private ObservableCollection<TestDataItem> lineDatas;
        public ObservableCollection<TestDataItem> LineDatas
        {
            get
            {
                return lineDatas;
            }
            set
            {
                lineDatas = value;
                RaisePropertyChanged(nameof(LineDatas));
            }
        }
        public TestDatas()
        {
            LineDatas = new ObservableCollection<TestDataItem>();
        }

        public void Copy(TestDatas dat)
        {
            this.LineDatas.Clear();
            foreach (var item in dat.LineDatas)
            {
                this.LineDatas.Add(item);
            }
        }
    }

    public class TestDataItem : NotifyPropertyChanged
    {
        private double testIndex;
        private double testKey;
        private double testValue;

        public double TestIndex
        {
            get
            {
                return testIndex;
            }
            set
            {
                testIndex = value;
                RaisePropertyChanged(nameof(TestIndex));
            }
        }
        public double TestKey
        {
            get
            {
                return testKey;
            }
            set
            {
                testKey = value;
                RaisePropertyChanged(nameof(TestKey));
            }
        }
        public double TestValue
        {
            get
            {
                return testValue;
            }
            set
            {
                testValue = value;
                RaisePropertyChanged(nameof(TestValue));
            }
        }

        public TestDataItem()
        {
        }
        public TestDataItem(int id, double ke, double va)
        {
            TestIndex = id;
            TestKey = ke;
            TestValue = va;
        }
    }

    public class KeyValueDataFitting : NotifyPropertyChanged
    {
        private ObservableCollection<IDataFitting> dataFittings;
        public ObservableCollection<IDataFitting> DataFittings
        {
            get
            {
                return dataFittings;
            }
            set
            {
                dataFittings = value;
                RaisePropertyChanged(nameof(DataFittings));
            }
        }

        private IDataFitting selectFittingType;
        public IDataFitting SelectFittingType
        {
            get
            {
                return selectFittingType;
            }
            set
            {
                selectFittingType = value;
                RaisePropertyChanged(nameof(SelectFittingType));
            }
        }

        public KeyValueDataFitting()
        {
            dataFittings = new ObservableCollection<IDataFitting>();
        }
    }
}
