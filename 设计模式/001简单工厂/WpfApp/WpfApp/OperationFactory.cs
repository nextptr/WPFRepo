using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class OperationFactory
    {
        public static Operation CreateInstance(string str)
        {
            Operation res = null;
            switch (str)
            {
                case "+":
                    res= new OperationAdd();
                    break;
                case "-":
                    res = new OperationSub();
                    break;
                case "*":
                    res = new OperationMul();
                    break;
                case "/":
                    res = new OperationDiv();
                    break;
            }
            return res;
        }
    }

    class OperationAdd : Operation
    {
        public override double GetResult()
        {
            return Number1 + Number2;
        }
    }
    class OperationSub : Operation
    {
        public override double GetResult()
        {
            return Number1 - Number2;
        }
    }
    class OperationMul : Operation
    {
        public override double GetResult()
        {
            return Number1 * Number2;
        }
    }
    class OperationDiv : Operation
    {
        public override double GetResult()
        {
            return Number1 / Number2;
        }
    }
}
