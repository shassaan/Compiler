using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.CustomClasses
{
    class Functions
    {
        public string name  { get; set; }
        public string returnType { get; set; }
        public string scope { get; set; }
        public string[] body { get; set; }
        public string returnedValue { get; set; }
    }
}
