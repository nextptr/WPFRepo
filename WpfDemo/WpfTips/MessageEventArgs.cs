using Common.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTips
{
    public class MessageEventArgs: IEventArgs
    {
        private object msg;
        public object MSG
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }
        public MessageEventArgs(object val)
        {
            MSG = val;
        }
    }
}
