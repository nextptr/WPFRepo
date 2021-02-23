using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class4
{
    #region codeStart
    public class OtherClass
    {
        public delegate void SendMsgEventHandle(string str);
        public SendMsgEventHandle SendMsgEvent;
        public void addMsg(string str)
        {
            if (SendMsgEvent != null)
            {
                SendMsgEvent(str);
            }
        }
    }
    #endregion codeEnd

    public class Class4_1:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            OtherClass other = new OtherClass();
            other.SendMsgEvent += this.GetMsg;  //连接other类的SendMsgEvent事件和本类的GetMsg()槽函数
            other.addMsg("other类.addMsg()发出消息,触发SendMsgEvent事件");
            other.addMsg("class类在槽函数GetMsg()收到数据并显示到控件上");
            other.SendMsgEvent -= this.GetMsg;  //断开other类的SendMsgEvent事件和本类的GetMsg()槽函数
            other.addMsg("other类.addMsg()发出消息，无法接收");
            //非匿名函数方法
            ddr($"内联函数add20(5){add20(5)}");
            ddr($"内联函数add20(6){add20(6)}");
            HideFunc hideFunc = delegate (int x)
              {
                  return x + 20;
              };
            ddr($"委托匿名函数hideFunc(5){hideFunc(5)}");
            ddr($"委托匿名函数hideFunc(6){hideFunc(6)}");
            HideFunc lambdaFunc1 = (int x) =>
              {
                  return x + 10;
              };
            HideFunc lambdaFunc2 = (x) =>
              {
                  return x + 10;
              };
            HideFunc lambdaFunc3 = x =>
            {
                return x + 10;
            };
            HideFunc lambdaFunc4 = x => x + 10;
            ddr($"lambda匿名函数lambdaFunc1(5){lambdaFunc1(5)}");
            ddr($"lambda匿名函数lambdaFunc2(6){lambdaFunc2(6)}");
            ddr($"lambda匿名函数lambdaFunc3(7){lambdaFunc2(7)}");
            ddr($"lambda匿名函数lambdaFunc4(8){lambdaFunc2(8)}");

        }
        //使用委托创建匿名方法HideFunc实现add20()函数效果
        public delegate int HideFunc(int x);
        public static int add20(int x)
        {
            return x + 20;
        }
        //委托事件对应的槽函数
        public void GetMsg(string str)
        {
            ddr(str);
        }
        #endregion codeEnd
    }
}
