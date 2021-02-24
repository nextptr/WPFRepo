using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public enum cashEnum
    {
        None,
        Rebate,
        Return
    }
    public class cashFactory
    {
        public static cashBase Create(cashEnum tp,double rebate=1,double condition=1,double Return=0)
        {
            switch (tp)
            {
                case cashEnum.Rebate:
                    return  new cashRebate(rebate);
                case cashEnum.Return:
                    return new cashRetun(condition, Return);
                default:
                    return null;
            }
        }
    }

    public abstract class cashBase
    {
        public abstract double AcceptMoney(double money);
    }

    public class cashRebate : cashBase
    {
        public double _rebate = 0.0;
        public cashRebate(double moneyRebate)
        {
            if (moneyRebate < 0 || moneyRebate > 1)
            {
                moneyRebate = 1;
            }
            _rebate = moneyRebate;
        }
        public override double AcceptMoney(double money)
        {
            return _rebate * money;
        }
    }

    public class cashRetun : cashBase
    {
        public double moneyCondition = 0.0;
        public double moneyReturn = 0.0;
        public cashRetun(double MoneyCondition ,double MoneyReturn)
        {
            moneyCondition = MoneyCondition;
            moneyReturn = MoneyReturn;
        }

        public override double AcceptMoney(double money)
        {
            if (money >= moneyCondition)
            {
                return money - Math.Floor(money / moneyCondition) * moneyReturn;
            }
            return money;
        }
    }
}
