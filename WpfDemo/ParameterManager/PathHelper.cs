using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterManager
{
    public class PathHelper
    {
        public static string RootPath
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
                _G_ParameterPath = Path.Combine(PathHelper.RootPath, "Parameters");
                return _G_ParameterPath;
            }
        }
    }
}
