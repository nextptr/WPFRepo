using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parameter
{
    public class ModuleInstance
    {
        public static void Init()
        {
            ParameterIns.Instance.Read();
        }

        public static void Uninit()
        {
            ParameterIns.Instance.Write();
        }
    }
}
