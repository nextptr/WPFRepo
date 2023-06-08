using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class stock360 : StockBase
    {
        public void Buy(int count, double price)
        {
            //16块钱买500股
        }

        public void Sell(int count, double price)
        {
            //32块钱卖出
        }
    }

    public class stockyunda : StockBase
    {
        public void Buy(int count, double price)
        {
            //18块钱买1300股
        }

        public void Sell(int count, double price)
        {
            //20块钱卖出
        }
    }
}
