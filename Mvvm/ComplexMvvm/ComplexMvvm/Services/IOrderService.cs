using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexMvvm.Services
{
    public interface IOrderService
    {
        void PleaseOrder(List<string> dishes);
    }
}
