using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public interface StockBase
    {
        void Sell(int count,double price);
        void Buy();
    }


}
