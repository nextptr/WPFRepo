using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ComplexMvvm.Services
{
    public class MockOrderService : IOrderService
    {
        public void PleaseOrder(List<string> dishes)
        {
            string pth = Environment.CurrentDirectory + @"\order.txt";
            if (!File.Exists(pth))
            {
                File.Create(pth);
            }
            File.WriteAllLines(pth, dishes.ToArray());
        }
    }
}
