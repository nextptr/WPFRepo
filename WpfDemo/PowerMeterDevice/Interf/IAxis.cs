using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice.Interf
{
    public interface IAxis
    {
        /// <summary>
        /// 回零完成事件
        /// </summary>
        event Action<bool> HomeDone;

        /// <summary>
        /// 状态改变事件，传入改变的参数名称
        /// </summary>
        event Action<string> StateChanged;


        /// <summary>
        /// 轴号
        /// </summary>
        short Id { get; }

        string Name { get; set; }

        void WaitPosition(int timeout = 10000 * 1000);

        ///// <summary>
        ///// 指令位置
        ///// </summary>
        //double Command { get; }

        ///// <summary>
        ///// 当前位置
        ///// </summary>
        //double Position { get; }

        ///// <summary>
        ///// 指令速度
        ///// </summary>
        //double CommandVel { get; }

        ///// <summary>
        ///// 实际反馈速度
        ///// </summary>
        //double FeedBakVel { get; }

        ///// <summary>
        ///// type
        ///// </summary>
        //string TypeName { get; }
        double GetCmdPosition();

        /// <summary>
        /// 下发参数
        /// </summary>
        /// <returns></returns>
        int IssueParam();

        /// <summary>
        /// 伺服激磁
        /// </summary>
        /// <param name="on"></param>
        /// <returns></returns>
        int Servo(bool on);

        /// <summary>
        /// 回零运动
        /// </summary>
        /// <returns></returns>
        int Home();

        /// <summary>
        /// 运动完成
        /// </summary>
        /// <returns></returns>
        bool HomeFinished();

        /// <summary>
        /// 设置当前位置为0
        /// </summary>
        /// <returns></returns>
        int SetZero();

        /// <summary>
        /// Jog运动停止，废弃
        /// </summary>
        /// <returns></returns>
        int JogStop();

        /// <summary>
        /// 点位相对运动
        /// </summary>
        /// <returns></returns>
        int RelativeMove(double distance);

        /// <summary>
        /// 点位绝对运动
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        int AbsoluteMove(double destination);

        /// <summary>
        /// 停止运动
        /// </summary>
        /// <param name="emg"></param>
        /// <returns></returns>
        int StopMove(bool emg = true);

        /// <summary>
        /// 设置正向软限位
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        int SetSoftPel(double pos);

        /// <summary>
        /// 获取正向软限位
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        int GetSoftPel(ref double pos);

        /// <summary>
        /// 设置负向软限位
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        int SetSoftMel(double pos);

        /// <summary>
        /// 获取负向软限位
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        int GetSoftMel(ref double pos);

        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int SetVelocity(double value);

        /// <summary>
        /// 获取速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetVelocity(ref double value);

        /// <summary>
        /// 设置加速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int SetAcceleration(double value);

        /// <summary>
        /// 获取加速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetAcceleration(ref double value);

        /// <summary>
        /// 设置减速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int SetDeceleration(double value);

        /// <summary>
        /// 获取减速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetDeceleration(ref double value);

        /// <summary>
        /// 设置加加速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int SetJerk(double value);

        /// <summary>
        /// 获取加加速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetJerk(ref double value);

        /// <summary>
        /// 设置Jog速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int SetJogVelocity(double value);

        /// <summary>
        /// 获取Jog速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetJogVelocity(ref double value);

        /// <summary>
        /// 设置Jog加速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int SetJogAccelerate(double value);

        /// <summary>
        /// 获取Jog加速度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetJogAccelerate(ref double value);


        bool IsMotionDone();

    }
}
