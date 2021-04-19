using PowerMeterDevice.Common;
using PowerMeterDevice.Event;
using PowerMeterDevice.Interf;
using PowerMeterDevice.Parameter;
using PowerMeterDevice.Process;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PowerMeterDevice
{
    public class PowerAdjustModel : NotifyPropertyChanged
    {
        //do
        public PowerAdjustModel()
        {
            btnSetPositionCmd = new DelegateCommand(btnSetPosition);
            btnGoToPositionCmd = new DelegateCommand(btnGoToPosition);
            btnStartCmd = new DelegateCommand(btnStart);
            btnStopCmd = new DelegateCommand(btnStop);
            btnHistoryCmd = new DelegateCommand(btnHistory);
            btnChangeUnitCmd = new DelegateCommand(btnChangeUnit);
            btnCalcCmd = new DelegateCommand(btnCalc);
        }

        #region Command
        public DelegateCommand btnSetPositionCmd { get; set; }
        public DelegateCommand btnGoToPositionCmd { get; set; }
        public DelegateCommand btnStartCmd { get; set; }
        public DelegateCommand btnStopCmd { get; set; }
        public DelegateCommand btnHistoryCmd { get; set; }
        public DelegateCommand btnChangeUnitCmd { get; set; }
        public DelegateCommand btnCalcCmd { get; set; }
        #endregion

        #region 注入属性
        //测试参数修改接口
        private IPMParamSet pmParamSet;
        public IPMParamSet PMParamSet
        {
            get => pmParamSet;
            set
            {
                pmParamSet = value;
                Init();
                RaisePropertyChanged("PMParamSet");
            }
        }

        //功率计修改
        private IPowerMeter pm;
        public IPowerMeter PowerMeterDevice
        {
            get => pm;
            set
            {
                pm = value;
                Init();
                RaisePropertyChanged("PowerMeterDevice");
            }
        }

        protected bool Init()
        {
            if (pmParamSet != null && pm != null)
            {
                process = new PMAdjustProgress(pm, pmParamSet);
                process.DataReceiveEvent += Process_DataReceiveEvent;
                process.MeasureFinishEvent += Process_MeasureFinishEvent;
                return true;
            }
            return false;
        }


        //流程控制
        private PMAdjustProgress process;

        //数据
        private PowerAdjustParameter param;
        public PowerAdjustParameter Param
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
                if (value is null)
                    return;
                foreach (var item in DataFit.DataFittings)
                {
                    if (item.FittingType == value.FittingType)
                    {
                        DataFit.SelectFittingType = item;
                        break;
                    }
                }
                RaisePropertyChanged(nameof(Param));
            }
        }

        //数据拟合
        private KeyValueDataFitting dataFit = new KeyValueDataFitting();
        public KeyValueDataFitting DataFit
        {
            get
            {
                return dataFit;
            }
            set
            {
                dataFit = value;
                RaisePropertyChanged(nameof(DataFit));
            }
        }

        #endregion

        #region 位置校正
        public IAxis AxisX;
        public IAxis AxisY;
        public IAxis AxisZ;
        public void btnSetPosition(object obj)
        {
            try
            {
                Param.AdjustPosition.AxisXPos = Math.Round(AxisX.GetCmdPosition(), 3);
                Param.AdjustPosition.AxisYPos = Math.Round(AxisY.GetCmdPosition(), 3);
                Param.AdjustPosition.AxisZPos = Math.Round(AxisZ.GetCmdPosition(), 3);
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置位置异常:"+ex.Message);
            }
        }
        public void btnGoToPosition(object obj)
        {
            try
            {
                if (AxisX == null || AxisY == null || AxisZ == null)
                    return;

                Parallel.Invoke(() =>
                {
                    AxisX.AbsoluteMove(Param.AdjustPosition.AxisXPos);
                },
                () =>
                {
                    AxisY.AbsoluteMove(Param.AdjustPosition.AxisYPos);
                },
                () =>
                {
                    AxisZ.AbsoluteMove(Param.AdjustPosition.AxisZPos);
                }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("到位异常:"+ ex.Message);
            }
        }
        #endregion

        #region 测量流程控制
        //do
        private void Process_MeasureFinishEvent(PowerMeasureFinishEventArgs arg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                isMeasured = false;
                MeasureComplete = true;
                if (arg.StepStatus)
                {
                    MeasureResult = arg.StepStatus;
                    if (MeasureResult)
                    {
                        Param.TestDateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        Param.Write();
                        param.FittingType = DataFit.SelectFittingType.FittingType.ToString();
                        ParameterManager.Instance.AddPowerAdjustParameter(Param);
                    }
                }
                else
                {
                    if (MeasureProcess == 100)
                    {
                        MeasureResult = true;
                    }
                    else
                    {
                        MeasureResult = false;
                    }
                }
            }));
        }

        private void HistoryModel_ChoiceDataItemEvent(PowerAdjustParameter obj)
        {
            Param.Copy(obj);
        }

        private bool measureResult = false;
        public bool MeasureResult
        {
            get
            {
                return measureResult;
            }
            set
            {
                measureResult = value;
                RaisePropertyChanged(nameof(MeasureResult));
            }
        }

        private bool isMeasured;
        public bool IsMeasured
        {
            get
            {
                return isMeasured;
            }
            set
            {
                isMeasured = value;
                RaisePropertyChanged(nameof(IsMeasured));
            }
        }

        private bool measureComplete;
        public bool MeasureComplete
        {
            get
            {
                return measureComplete;
            }
            set
            {
                measureComplete = value;
                RaisePropertyChanged(nameof(MeasureComplete));
            }
        }

        private int measureProcess;
        public int MeasureProcess
        {
            get
            {
                return measureProcess;
            }
            set
            {
                measureProcess = value;
                RaisePropertyChanged(nameof(MeasureProcess));
            }
        }

        private bool checkDevice()
        {
            if (PowerMeterDevice == null)
            {
                MessageBox.Show("硬件检测:功率计设备为空");
                return false;
            }
            if (PowerMeterDevice.IsConnected == false)
            {
                MessageBox.Show("硬件检测:功率计设备未连接");
                return false;
            }
            if (PowerMeterDevice.IsSampling == false)
            {
                if (!PowerMeterDevice.BeginSampling())
                {
                    MessageBox.Show("硬件检测:功率计设备开始采集失败");
                    return false;
                }
            }

            if (PMParamSet == null)
            {
                MessageBox.Show("硬件检测:测量硬件为空");
                return false;
            }
            if (PMParamSet.IsReadyWork == false)
            {
                MessageBox.Show("硬件检测:测量硬件异常");
                return false;
            }
            return true;
        }
        private bool checkParam()
        {
            if (Param.AdjustParamSet.RangeEnd < Param.AdjustParamSet.RangeSta)
            {
                return false;
            }

            if (Param.AdjustParamSet.RangeEnd == 0 || Param.AdjustParamSet.TestInterval == 0)
            {
                return false;
            }

            return true;
        }
        public async void btnStart(object obj)
        {
            try
            {
                Param.AdjustDatas.LineDatas.Clear();

                if (!checkParam())
                {
                    MessageBox.Show("参数校验:参数设置错误");
                    return;
                }
                if (!checkDevice())
                {
                    return;
                }

                process.AdjustParam = new TestParam();
                process.AdjustParam.TimeAcq = this.Param.AdjustParamSet.TimeAcq * 1000;
                process.AdjustParam.TimeCalc = this.Param.AdjustParamSet.TimeCalc * 1000;
                process.AdjustParam.TimeDelay = this.Param.AdjustParamSet.TimeDelay * 1000;
                process.AdjustParam.RangeSta = this.Param.AdjustParamSet.RangeSta;
                process.AdjustParam.RangeEnd = this.Param.AdjustParamSet.RangeEnd;
                process.AdjustParam.TestInterval = this.Param.AdjustParamSet.TestInterval;

                IsMeasured = true;
                bool retFlag = false;
                await Task.Run(() =>
                {
                    retFlag = process.ReStartProgress(0);
                });
                IsMeasured = false;
                if (retFlag)
                {
                    MeasureProcess = 100;
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:"+ex.Message);
            }
        }
        public void btnStop(object obj)
        {
            if (process == null || IsMeasured == false)
                return;
            process.Stop();
            if (MeasureProcess != 100)
            {
                IsMeasured = false;
            }
        }


        HistoryDataView historyDataView = null;
        public void btnHistory(object obj)
        {
            if (historyDataView == null)
            {
                historyDataView = new HistoryDataView();
                historyDataView.Closed -= historyDataView_Closed;
                historyDataView.Closed += historyDataView_Closed;
                historyDataView.ChoiceDataItemEvent -= HistoryModel_ChoiceDataItemEvent;
                historyDataView.ChoiceDataItemEvent += HistoryModel_ChoiceDataItemEvent;
                historyDataView.Owner = Application.Current.MainWindow;
                historyDataView.ShowDialog();
            }
            else if (!historyDataView.IsActive)
            {
                historyDataView.WindowState = WindowState.Normal;
                historyDataView.Activate();
            }
        }
        private void historyDataView_Closed(object sender, EventArgs e)
        {
            historyDataView = null;
        }
        #endregion

        #region 数据计算
        private void Process_DataReceiveEvent(PowerDataReceiveEventArgs arg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (!IsMeasured)
                    return;

                //记录测试数据
                double ke = Math.Round(arg.ke, 3);
                double va = Math.Round(arg.val, 3);
                Param.AdjustDatas.LineDatas.Add(new TestDataItem(arg.index, ke, va));

                //进度
                int index = -1;
                double tmp = 0.0;
                double exp = 0.0001;
                while (true)
                {
                    index++;
                    double sum = index * Param.AdjustParamSet.TestInterval + Param.AdjustParamSet.RangeSta;
                    tmp = sum - Param.AdjustParamSet.RangeEnd;
                    if (tmp > 0.0001)
                    {
                        break;
                    }
                }
                double percen = (double)(arg.index + 1) / index;
                MeasureProcess = (int)(percen * 100);
            }));
        }

        public void FittingTypeChanged(object sender, SelectionChangedEventArgs arg)
        {
            try
            {
                if (arg.AddedItems.Count <= 0)
                    return;

                IDataFitting set = arg.AddedItems[0] as IDataFitting;
                if (set == null)
                    return;

                param.FittingType = set.FittingType;
            }
            catch (Exception ex)
            {
                MessageBox.Show("拟合错误:"+ex.Message);
            }
        }

        private double inPut;
        public double InPut
        {
            get
            {
                return inPut;
            }
            set
            {
                inPut = value;
                RaisePropertyChanged(nameof(InPut));
            }
        }

        private double outPut;
        public double OutPut
        {
            get
            {
                return outPut;
            }
            set
            {
                outPut = value;
                RaisePropertyChanged(nameof(OutPut));
            }
        }

        private bool isParamToPower = true;
        public bool IsParamToPower
        {
            get
            {
                return isParamToPower;
            }
            set
            {
                isParamToPower = value;
                RaisePropertyChanged(nameof(IsParamToPower));
            }
        }

        private string powerUnit = "输出功率:";
        public string PowerUnit
        {
            get
            {
                return powerUnit;
            }
            set
            {
                powerUnit = value;
                RaisePropertyChanged(nameof(PowerUnit));
            }
        }

        private string paramUnit = "测试参数:";
        public string ParamUnit
        {
            get
            {
                return paramUnit;
            }
            set
            {
                paramUnit = value;
                RaisePropertyChanged(nameof(ParamUnit));
            }
        }

        public void btnCalc(object obj)
        {
            if (param == null || param.AdjustDatas.LineDatas.Count <= 0 || DataFit == null || DataFit.SelectFittingType == null)
            {
                return;
            }
            else
            {
                List<TestDataItem> ls = new List<TestDataItem>();
                foreach (var item in Param.AdjustDatas.LineDatas)
                {
                    ls.Add(item);
                }
                DataFit.SelectFittingType.ImportKeyValueDatas(ls);
            }
            if (IsParamToPower)
            {
                OutPut = DataFit.SelectFittingType.GetValueFromKey(InPut);
            }
            else
            {
                OutPut = DataFit.SelectFittingType.GetKeyFromValue(InPut);
            }
        }
        public void btnChangeUnit(object obj)
        {
            IsParamToPower = !IsParamToPower;
            string tmp1 = PowerUnit;
            string tmp2 = ParamUnit;
            PowerUnit = tmp2;
            ParamUnit = tmp1;
        }
        #endregion 
    }
}
