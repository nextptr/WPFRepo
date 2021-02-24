using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class Operation
    {
        public double Number1 = 0.0;
        public double Number2 = 0.0;
        public virtual double GetResult()
        {
            return 0.0;
        }
    }
}
