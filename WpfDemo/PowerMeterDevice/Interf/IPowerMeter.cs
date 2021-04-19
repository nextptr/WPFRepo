using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Interf
{
    /// <summary>
    /// 功率计数据刷新事件定义
    /// </summary>
    /// <param name="args"></param>
    public delegate void PowerMeterOnTimeEvent(PowerMeterOnTimeEventArgs args);
    public class PowerMeterOnTimeEventArgs : EventArgs
    {
        /// <summary>
        /// 当前功率值
        /// </summary>
        public double OntimeValue { get; set; }
    }


    /// <summary>
    /// 功率计设置归零状态事件
    /// </summary>
    /// <param name="args"></param>
    public delegate void PowerMeterZeroEvent(PowerMeterZeroEventArgs args);
    public class PowerMeterZeroEventArgs : EventArgs
    {
        /// <summary>
        /// 是否置零成功
        /// </summary>
        public bool IsZeroSetted { get; set; } = false;
    }


    /// <summary>
    /// 当前设置的波长
    /// </summary>
    /// <param name="args"></param>
    public delegate void PowerMeterWaveLengthEvent(PowerMeterWaveLengthEventArgs args);
    public class PowerMeterWaveLengthEventArgs : EventArgs
    {
        /// <summary>
        /// 波长更新值
        /// </summary>
        public int WaveLength { get; set; } = 300;
    }


    /// <summary>
    /// 功率计采集状态改变事件
    /// </summary>
    /// <param name="args"></param>
    public delegate void PowerMeterSamplingEvent(PowerMeterSamplingEventArgs args);
    public class PowerMeterSamplingEventArgs : EventArgs
    {
        /// <summary>
        /// 采集状态改变
        /// </summary>
        public bool IsSampling { get; set; } = false;
    }


    public interface IPowerMeter
    {
        /// <summary>
        /// 功率计的数据刷新事件
        /// </summary>
        event PowerMeterOnTimeEvent PowerMeterOnTimeEvent;

        /// <summary>
        /// 功率计的波长更新事件
        /// </summary>
        event PowerMeterWaveLengthEvent PowerMeterWaveLengthEvent;

        /// <summary>
        /// 功率计置零事件
        /// </summary>
        event PowerMeterZeroEvent PowerMeterZeroEvent;

        /// <summary>
        /// 功率计采集状态改变事件
        /// </summary>
        /// <param name="args"></param>
        event PowerMeterSamplingEvent PowerMeterSamplingEvent;

        /// <summary>
        /// 执行功率计连接
        /// </summary>
        /// <returns></returns>
        bool Connect();

        /// <summary>
        /// 断开功率计连接
        /// </summary>
        /// <returns></returns>
        bool DisConnect();

        string SPortName { set; get; }
        bool IsConnected { get; set; }
        bool IsSampling { get; set; }
        int WaveLength { set; get; }

        /// <summary>
        /// 开始功率计数据采集
        /// </summary>
        /// <returns></returns>
        bool BeginSampling();

        /// <summary>
        /// 停止采集
        /// </summary>
        /// <returns></returns>
        bool StopSampling();

        /// <summary>
        /// 设置功率计数据查询的延时
        /// </summary>
        /// <returns></returns>
        bool SetTimeOut(int MillSeconds);

        /// <summary>
        /// 获取当前功率
        /// </summary>
        /// <returns></returns>
        bool GetCurrentPowerValue(out double CurrentPower);

        /// <summary>
        /// 设置当前功率计波长
        /// </summary>
        /// <param name="WaveLength"></param>
        /// <returns></returns>
        bool SetWaveLength(int WaveLength);

        /// <summary>
        /// 获取当前功率计波长
        /// </summary>
        /// <param name="WaveLength"></param>
        /// <returns></returns>
        bool GetWaveLength(out int WaveLength);

        /// <summary>
        /// 设置归零值
        /// </summary>
        /// <returns></returns>
        bool SetZero();

        /// <summary>
        /// 清除归零值
        /// </summary>
        /// <returns></returns>
        bool CleanZero();

    }
}
