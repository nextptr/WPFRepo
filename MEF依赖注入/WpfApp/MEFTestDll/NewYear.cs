using MEFCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFTestDll
{
    [Export(typeof(IMefBase)), ExportMetadata("MefType", "NewYear")]
    public class NewYear : IMefBase
    {
        public string MefType { get; set; }

        public string GetResult()
        {
            return "big money";
        }
    }
}
