using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WpfApp
{
    [Flags]
    public enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000
    }

    public class MouseHelper
    {
        /// <summary>
        /// 移动鼠标位置
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// </summary>
        /// <param name="flags">事件类型</param>
        /// <param name="dx">鼠标位置</param>
        /// <param name="dy">鼠标位置</param>
        /// <param name="data">当flags为Wheel鼠标滚轮时 此参数表示滚轮的滑动方向 正值向前滚动 负值向后滚动 其余事件默认为0</param>
        /// <param name="extraInfo">附加参数值 应用程序中使用GetMessageExtraInfo可获取此附加值</param>
        /// <returns></returns>
        //[DllImport("user32.dll",CharSet =CharSet.Auto,CallingConvention =CallingConvention.StdCall)]
        //public static extern bool mouse_event(uint flags, int dx, int dy,uint data ,UIntPtr extraInfo);
        [DllImport("user32.dll",CharSet =CharSet.Auto,CallingConvention =CallingConvention.StdCall)]
        private static extern bool mouse_event(uint flags, uint dx, uint dy, uint data , UIntPtr extraInfo);

        /// <summary>
        /// </summary>
        /// <param name="key">虚拟按键 可用System.Windows.Forms.Keys</param>
        /// <param name="bScan">扫描码 默认为0</param>
        /// <param name="dwFlags">扫描标志 KeyDown置为0 KeyUp置为2</param>
        /// <param name="dwExtraInfo">默认置为0</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool keybd_event(byte key, byte bScan, int dwFlags, int dwExtraInfo);

        public static bool SemiMouseEventAbsPix(MouseEventFlag eventFlag,int pixX,int pixY)
        {
            double dpiX = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            double dpiY = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度
            uint realX = (uint)(65536.0 / dpiX * pixX - 1);
            uint realY = (uint)(65536.0 / dpiY * pixY - 1);
            return mouse_event((uint)(eventFlag | MouseEventFlag.Absolute | MouseEventFlag.Move), realX, realY, 0, UIntPtr.Zero);
        }
        public static bool SemiMouseEventRelPix(MouseEventFlag eventFlag, int pixX, int pixY)
        {
            return mouse_event((uint)(eventFlag| MouseEventFlag.Move), (uint)pixX, (uint)pixY, 0, UIntPtr.Zero);
        }

        public static bool SemiKeyBoardEvent(Keys key)
        {
            bool flg = keybd_event((byte)key, 0, 0, 0); //按下
            flg &= keybd_event((byte)key, 0, 2, 0); //弹起
            return flg;
        }
    }
}
