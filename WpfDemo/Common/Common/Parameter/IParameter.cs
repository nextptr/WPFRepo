using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Parameter
{
    public interface IParameter
    {
        void Read();

        void Write();

        void Read(string fileName);

        void Write(string fileName);

        void Copy(IParameter source);
    }
}
