using PowerMeterDevice.Event;
using PowerMeterDevice.Interf;
using PowerMeterDevice.Parameter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace PowerMeterDevice.Process
{
    /// <summary>
    /// 功率计测量流程类
    /// </summary>
    internal class PMAdjustProgress
    {
        //X设置接口
        public IPMParamSet PMParamSet;

        //当前状态对象
        public IPMAdjustStatus Status;

        //功率计对象
        public IPowerMeter PM;

        public TestParam AdjustParam;

        private int currentIndex;

        //设置ParamX时的等待
        public int TimeOut = 5000;

        //当前计算功率结果
        public delegate void DataReceiveEventHandle(PowerDataReceiveEventArgs arg);
        public event DataReceiveEventHandle DataReceiveEvent;
        private double retValue;
        public double RetValue
        {
            get
            {
                return retValue;
            }
            set
            {
                retValue = value;
                if (ParamX != null)
                    DataReceiveEvent?.Invoke(new PowerDataReceiveEventArgs() { index = currentIndex, ke = ParamX.Value, val = value });
            }
        }

        public delegate void MeasureFinishEventHandle(PowerMeasureFinishEventArgs arg);
        public event MeasureFinishEventHandle MeasureFinishEvent;

        //当前的输入参数X
        public double? ParamX
        {
            get
            {
                double x = currentIndex * AdjustParam.TestInterval + AdjustParam.RangeSta;
                if (x - AdjustParam.RangeEnd > 0.0001)
                    return null;
                else
                    return x;
            }
        }

        //构造函数
        public PMAdjustProgress(IPowerMeter pm, IPMParamSet paramSet)
        {
            PMParamSet = paramSet;
            PM = pm;
            Status = new SetStatus(this);
            currentIndex = 0;
        }

        //循环执行
        public bool ReStartProgress(int index)
        {
            stopFlag = false;
            currentIndex = index;
            while (!(ParamX is null) && !stopFlag)
            {
                if (!CheckHardDeviceConnectStatus())
                {
                    stopFlag = true;
                    break;
                }

                if (!Step())
                    return false;
            }
            MeasureFinishEvent?.Invoke(new PowerMeasureFinishEventArgs(!stopFlag));
            return !stopFlag;
        }

        private bool CheckHardDeviceConnectStatus()
        {
            try
            {
                if (PM == null)
                {
                    MessageBox.Show("功率计设备为空");
                    return false;
                }
                if (PM.IsConnected == false)
                {
                    MessageBox.Show("功率计设备未连接");
                    return false;
                }

                if (PMParamSet == null)
                {
                    MessageBox.Show("测量硬件为空");
                    return false;
                }
                if (PMParamSet.IsReadyWork == false)
                {
                    MessageBox.Show("测量硬件异常");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("检查硬件状态报错");
                return false;
            }
        }

        bool stopFlag = false;
        public void Stop()
        {
            stopFlag = true;
            MeasureFinishEvent?.Invoke(new PowerMeasureFinishEventArgs(false));
            currentIndex = 0;
        }

        //单步执行操作
        /// <summary>
        /// 返回值为True表示单步运行完成
        /// False表示单步运行未完成
        /// </summary>
        /// <returns></returns>
        public bool Step()
        {
            if (continueFlag == true)
            {
                //继续当前流程
                continueFlag = false;
                suspendFlag = false;
                Status.Continue();
            }

            //依次运行所有状态
            while (Status.Step())
            {
                if (suspendFlag == true)
                {
                    //暂停当前流程
                    continueFlag = false;
                    suspendFlag = false;
                    Status.Suspend();
                    return false;
                }
            }

            if (Status is FinishStatus)
            {
                currentIndex++;
                Status = new SetStatus(this);
                return true;
            }
            else
            { return false; }
        }

        private bool continueFlag = false;
        public bool Continue()
        {
            continueFlag = true;
            suspendFlag = false;
            Step();
            return true;
        }

        private int localIndex = 0;
        private bool suspendFlag = false;
        public bool Suspend()
        {
            continueFlag = false;
            suspendFlag = true;
            localIndex = currentIndex;
            return true;
        }

    }

    interface IPMAdjustStatus
    {
        //PMAdjustProgress Context { get; }

        bool Step();
        bool Continue();
        bool Suspend();
    }

    /// <summary>
    /// 当前状态： 设置参数X
    /// </summary>
    internal class SetStatus : IPMAdjustStatus
    {
        public SetStatus(PMAdjustProgress progeress)
        {
            context = progeress;
            localProc = progeress;
            progeress.PMParamSet.ParamChangedEvent += PMParamSet_ParamChangedEvent;
        }

        private PMAdjustProgress context;
        public PMAdjustProgress Context => context;

        private PMAdjustProgress localProc = null;

        public bool tmot = false;
        private ManualResetEvent _mre = new ManualResetEvent(false);
        private void PMParamSet_ParamChangedEvent(double param)
        {
            _mre.Set();
            tmot = false;
        }

        public bool Step()
        {
            //保存上下文
            localProc.Status = new SetStatus(Context);

            //设置参数变量
            tmot = true;
            _mre.Reset();
            if (Context.ParamX is double x)
            {
                Context.PMParamSet.Param = x;
                _mre.WaitOne(Context.TimeOut);
                if (tmot)
                {
                    _mre.Set();
                    //等待失败，进入失败状态
                    Context.Status = new ErrorStatus(Context);
                    return false;
                }

                //进入等待稳定状态
                Context.Status = new DelayStatus(Context);
                return true;
            }
            else
            {
                Context.Status = new ErrorStatus(Context);
                return false;
            }
        }

        public bool Continue()
        {
            return true;
        }

        public bool Suspend()
        {
            return true;
        }
    }

    /// <summary>
    /// 当前状态：等待功率稳定
    /// </summary>
    internal class DelayStatus : IPMAdjustStatus
    {
        public DelayStatus(PMAdjustProgress progeress)
        {
            context = progeress;
        }

        private PMAdjustProgress context;
        public PMAdjustProgress Context => context;

        public bool Step()
        {
            Thread.Sleep(Context.AdjustParam.TimeDelay);
            //进入测量状态
            Context.Status = new MeasureStatus(Context);
            return true;
        }

        public bool Continue()
        {
            return true;
        }

        public bool Suspend()
        {
            return true;
        }
    }

    /// <summary>
    /// 当前状态：测量并计算
    /// </summary>
    internal class MeasureStatus : IPMAdjustStatus
    {
        public MeasureStatus(PMAdjustProgress progeress)
        {
            context = progeress;
            Context.PM.PowerMeterOnTimeEvent += PM_PowerMeterOnTimeEvent;
        }

        private PMAdjustProgress context;
        public PMAdjustProgress Context => context;

        List<double> powerDatas = new List<double>();

        bool isAcq = false;
        private void PM_PowerMeterOnTimeEvent(PowerMeterOnTimeEventArgs args)
        {
            if (isAcq)
            {
                powerDatas.Add(args.OntimeValue);
            }
        }
        public bool Step()
        {
            powerDatas.Clear();
            isAcq = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < Context.AdjustParam.TimeAcq) { }
            isAcq = false;
            stopwatch.Reset();

            double realPower = 0;
            if (powerDatas.Count() > 0)
            {
                realPower = powerDatas.Aggregate((a, s) => { s += a; return s; }) / powerDatas.Count();
            }
            //更新UI
            Context.RetValue = realPower;

            Context.Status = new FinishStatus(Context);
            return true;
        }
        public bool Continue()
        {
            return true;
        }
        public bool Suspend()
        {
            return true;
        }
    }

    /// <summary>
    /// 当前状态：结束
    /// </summary>
    internal class FinishStatus : IPMAdjustStatus
    {
        public FinishStatus(PMAdjustProgress progeress)
        {
            context = progeress;
        }

        private PMAdjustProgress context;
        public PMAdjustProgress Context => context;

        public bool Continue()
        {
            return true;
        }

        public bool Step()
        {
            //Context.Status = new SetStatus(Context);
            return false;
        }

        public bool Suspend()
        {
            return true;
        }
    }

    /// <summary>
    /// 当前状态：错误
    /// </summary>
    internal class ErrorStatus : IPMAdjustStatus
    {
        public ErrorStatus(PMAdjustProgress progeress)
        {
            context = progeress;
        }

        private PMAdjustProgress context;
        public PMAdjustProgress Context => context;

        public bool Continue()
        {
            return true;
        }

        public bool Step()
        {
            Context.Status = new SetStatus(Context);
            return false;
        }

        public bool Suspend()
        {
            return true;
        }
    }
}
