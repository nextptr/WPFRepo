using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Common
{
    public class RootPath
    {
        public static string Root
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        private static string _G_ParameterPath;
        public static string G_ParameterPath
        {
            set
            {
                _G_ParameterPath = value;
            }
            get
            {
                _G_ParameterPath = Path.Combine(RootPath.Root, "Parameters");
                return _G_ParameterPath;
            }
        }
    }
}
