using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CsBase.Common
{

    public enum StringType
    {
        Normal,
        Title,
    }
    public class StringItem:NotifyPropertyChanged, IParameterItem
    {
        private string _str;
        public string Str
        {
            get
            {
                return _str;
            }
            set
            {
                if (_str != value)
                {
                    _str = value;
                    OnPropertyChanged("Str");
                }
            }
        }

        public StringType Type;

        public IParameterItem Clone()
        {
            StringItem item = new StringItem();
            item.Str = this.Str;
            return item;
        }
        public void Copy(IParameterItem other)
        {
            StringItem ot = other as StringItem;
            this.Str = ot.Str;
        }
        public StringItem()
        {
        }
        public StringItem(string str)
        {
            _str = str;
            Type = StringType.Normal;
        }
        public StringItem(string str , StringType tp)
        {
            _str = str;
            Type = tp;
        }
    }

    public class classBase
    {
        public delegate void ddMsgEventHandle(string str);
        public ddMsgEventHandle ddMsgEvent;

        public List<StringItem> runList; //执行结果
        public classBase()
        {
            runList = new List<StringItem>();
            
        }
        public virtual void RunTest() //源代码
        {
        }
        protected void ddr(string str)
        {
            runList.Add(new StringItem(str));
            if (ddMsgEvent != null)
            {
                ddMsgEvent(str);
            }
        }
        protected void ddh(string str)
        {
            runList.Add(new StringItem("\n" + str, StringType.Title));
            if (ddMsgEvent != null)
            {
                ddMsgEvent("\n"+str);
            }
        }
    }
}
