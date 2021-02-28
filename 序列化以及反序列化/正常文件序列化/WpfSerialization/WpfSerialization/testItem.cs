using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSerialization
{
    public class testItem
    {
        public int age;
        public int weight;
        public string name;
        public string school;

        public List<int> intarr;
        public testItem()
        {
            age = 0;
            weight = 10;
            name = "小赵";
            school = "南邮";
            intarr = new List<int>();
            intarr.Add(1);
            intarr.Add(2);
            intarr.Add(3);
            intarr.Add(4);
            intarr.Add(5);
        }
    }
}
