using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternDemo.StatusMachine
{
    public class StatusProgress
    {
        //参数
        public IMachineStatus Status;
        public int Seed;
        public int DelyTim;
        public int TimeAcq;

        private Timer tim;
        private Random rd;
        //事件
        public delegate void DataReceiveEventHandle(object arg);
        public delegate void MeasureFinishEventHandle(bool arg);
        public delegate void RandomDataEventHandle(int arg);

        public event DataReceiveEventHandle DataReceiveEvent;
        public event MeasureFinishEventHandle MeasureFinishEvent;
        public event RandomDataEventHandle RandomDataEvent;

        //控制
        public StatusProgress()
        {
            tim = new Timer(TimerCallBack);
            rd = new Random(Seed);
            tim.Change(10, 500);
            Status = new SetStatus(this);
        }
        private void TimerCallBack(object state)
        {
            RandomDataEvent?.Invoke(rd.Next());
        }
     
        bool stopFlag = false;
        bool continueFlag = false;
        bool suspendFlag = false;
        public bool ReStartProgress()
        {
            while (!stopFlag)
            {
                if (!Step())
                    return false;
            }
            MeasureFinishEvent?.Invoke(stopFlag);
            return !stopFlag;
        }
        public void Stop()
        {
            stopFlag = true;
            MeasureFinishEvent?.Invoke(false);
        }
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
                Status = new SetStatus(this);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Continue()
        {
            Step();
            return true;
        }
        public bool Suspend()
        {
            return true;
        }

    }
    public interface IMachineStatus
    {
        bool Step();
        bool Continue();
        bool Suspend();
    }

    /// <summary>
    /// 当前状态： 设置参数X
    /// </summary>
    internal class SetStatus : IMachineStatus
    {
        public SetStatus(StatusProgress progeress)
        {
            context = progeress;
            localProc = progeress;
        }

        private StatusProgress context;
        public StatusProgress Context => context;
        private StatusProgress localProc = null;

        public bool Step()
        {
            //保存上下文
            localProc.Status = new SetStatus(Context);

            //设置参数变量
            if (CheckDevice())
            {
                if (!SetParam())
                {
                    //设置失败，进入失败状态
                    Context.Status = new ErrorStatus(Context);
                    return false;
                }
                else
                {
                    //进入等待稳定状态
                    Context.Status = new DelayStatus(Context);
                    return true;
                }
            }
            else
            {
                Context.Status = new ErrorStatus(Context);
                return false;
            }
        }

        private bool CheckDevice()
        {
            return true;
        }
        private bool SetParam()
        {
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
    /// 当前状态：等待功率稳定
    /// </summary>
    internal class DelayStatus : IMachineStatus
    {
        public DelayStatus(StatusProgress progeress)
        {
            context = progeress;
        }

        private StatusProgress context;
        public StatusProgress Context => context;

        public bool Step()
        {
            Thread.Sleep(Context.DelyTim);
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
    internal class MeasureStatus : IMachineStatus
    {
        public MeasureStatus(StatusProgress progeress)
        {
            context = progeress;
            Context.RandomDataEvent += Context_RandomDataEvent;
        }

        private StatusProgress context;
        public StatusProgress Context => context;

        bool isAcq = false;
        List<int> powerDatas = new List<int>();
        private void Context_RandomDataEvent(int arg)
        {
            if (isAcq)
            {
                powerDatas.Add(arg);
            }
        }

        public bool Step()
        {
            powerDatas.Clear();
            isAcq = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < Context.TimeAcq) { }
            isAcq = false;
            stopwatch.Reset();

            double realPower = 0;
            if (powerDatas.Count() > 0)
            {
                realPower = powerDatas.Aggregate((a, s) => { s += a; return s; }) / powerDatas.Count();
            }

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
    internal class FinishStatus : IMachineStatus
    {
        public FinishStatus(StatusProgress progeress)
        {
            context = progeress;
        }

        private StatusProgress context;
        public StatusProgress Context => context;

        public bool Continue()
        {
            return true;
        }

        public bool Step()
        {
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
    internal class ErrorStatus : IMachineStatus
    {
        public ErrorStatus(StatusProgress progeress)
        {
            context = progeress;
        }

        private StatusProgress context;
        public StatusProgress Context => context;

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