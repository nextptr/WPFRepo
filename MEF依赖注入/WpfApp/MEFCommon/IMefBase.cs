using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFCommon
{
    public interface IMefBase
    {
        string MefType { get; set; }

        string GetResult();
    }
}
