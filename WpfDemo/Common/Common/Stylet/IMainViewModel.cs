using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Stylet
{
    public interface IMainViewModel
    {
        void Top();
        void Push(string page);
        IPage GetPageInstance(string page);
    }
}
