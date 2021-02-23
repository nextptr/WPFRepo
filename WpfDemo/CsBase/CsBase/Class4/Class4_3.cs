using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class4
{
    #region codeStart
    public class MyStack<C>  //泛型类
    {
        private List<C> _ls;
        public MyStack()
        {
            _ls = new List<C>();
        }

        public void Push(C d)
        {
            _ls.Add(d);
        }
        public C Pop()
        {
            if (_ls.Count <= 0)
            {
                return default(C);
            }
            C ret = _ls[_ls.Count - 1];
            _ls.RemoveAt(_ls.Count - 1);
            return ret;
        }
    }

    public struct PieceOfData<T> //泛型结构
    {
        private T _data;
        public PieceOfData(T value)
        {
            _data = value;
        }
        public T Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
    }

    public interface Iifc<T>
    {
        T ReturnIt(T val);
    }
    public class Simple<S>:Iifc<S>
    {
        public S ReturnIt(S val)
        {
            return val;
        }
    }

    #endregion codeEnd
    public class Class4_3:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            ddr("泛型Stack");
            MyStack<int> lsInt = new MyStack<int>();
            lsInt.Push(1);
            lsInt.Push(3);
            lsInt.Push(5);
            lsInt.Push(7);
            ddr($"MyStack<int> lsInt.Pop():{lsInt.Pop()}");
            MyStack<string> lsStr = new MyStack<string>();
            lsStr.Push("str2");
            lsStr.Push("str4");
            lsStr.Push("str6");
            lsStr.Push("str8");
            ddr($"MyStack<string> lsStr.Pop():{lsStr.Pop()}");

            PieceOfData<int> IntData=new PieceOfData<int>(125);
            var strData = new PieceOfData<string>("this is string");
            ddr("泛型结构");
            ddr($"PieceOfData<int>    IntData:{IntData.Data}");
            ddr($"PieceOfData<string> strData:{strData.Data}");

            Simple<int> intIfc = new Simple<int>();
            Simple<string> strIfc = new Simple<string>();
            ddr("泛型接口");
            ddr($"intIfc {intIfc.ReturnIt(567)}");
            ddr($"strIfc {strIfc.ReturnIt("stringInterface")}");


        }
        #endregion codeEnd
    }
}
