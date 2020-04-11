using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.CustomClasses
{
    class Errors
    {
        public int errCode { get; set; }
        public int lineNo { get; set; }
        public string Message { get; set; }
    }
}
