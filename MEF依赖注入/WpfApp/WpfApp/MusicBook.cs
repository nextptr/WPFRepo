using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    [Export(typeof(IBookService))]
    public class MusicBook : IBookService
    {
        public string BookName { get; set; }

        public string GetBookName()
        {
            return "MusicBook";
        }
    }
}
