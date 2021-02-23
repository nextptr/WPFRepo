using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBase.Class2
{
    #region codeStart
    public class cla1
    {
        public string first;
        public string second;
        public string last;
        public string this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0:
                        first = value;
                        break;
                    case 1:
                        second = value;
                        break;
                    case 2:
                        last = value;
                        break;
                }
            }
            get
            {
                switch (index)
                {
                    case 0:
                        return first;
                    case 1:
                        return second;
                    case 2:
                        return last;
                    default:
                        return "";
                }
            }
        }
        public string this[string id]  //索引器重载
        {
            get
            {
                switch (id)
                {
                    case "f":
                        return first;
                    case "s":
                        return second;
                    case "l":
                        return last;
                    default:
                        return "";
                }
            }
        }
        public string this[int id, bool flag]  //多索引重载
        {
            get
            {
                string tmp = "";
                switch (id)
                {
                    case 0:
                        tmp= first;
                        break;
                    case 1:
                        tmp= second;
                        break;
                    case 2:
                        tmp= last;
                        break;
                    default:
                        return "";
                }
                if (flag)
                {
                    return tmp;
                }
                return "";
            }
        }
    }
    #endregion codeEnd
    public class Class2_2:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            cla1 cla = new cla1();
            cla[0] = "first";
            cla[1] = "2222";
            cla[2] = "第三";
            ddr("int index下标 cal[0]  " + cla[0]);
            ddr("int index下标 cal[1]  " + cla[1]);
            ddr("int index下标 cal[2]  " + cla[2]);
            ddr("\n");
            ddr("string index 下标重载索引器");
            ddr("string index下标 cal[f]  " + cla[@"f"]);
            ddr("string index下标 cal[s]  " + cla[@"s"]);
            ddr("string index下标 cal[l]  " + cla[@"l"]);
            ddr("\n");
            ddr("多下标索引器");
            ddr("ind id,bool falg cla[0,true]  " + cla[0,true]);
            ddr("ind id,bool falg cla[1,false] " + cla[1,false]);
            ddr("ind id,bool falg cla[2,true]  " + cla[2,true]);
        }
        #endregion codeEnd
    }
}