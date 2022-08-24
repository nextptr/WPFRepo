using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVbox.Common
{
    public interface IRouter
    {
        void Push(string name);
        void GoBack();
        void BackToTop();
        IPage GetPageInstance(string name);
    }
}
