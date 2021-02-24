using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class cashContex
    {
        cashBase bas;
        public cashContex(cashEnum tp, double rebate = 1, double condition = 1, double Return = 0)
        {
            switch (tp)
            {
                case cashEnum.Rebate:
                    bas = new cashRebate(rebate);
                    break;
                case cashEnum.Return:
                    bas = new cashRetun(condition, Return);
                    break;
                default:
                    break;
            }
        }

        public double Cash(double money)
        {
            return bas.AcceptMoney(money);
        }
    }
}
