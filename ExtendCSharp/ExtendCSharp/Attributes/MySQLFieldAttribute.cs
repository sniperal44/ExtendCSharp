using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Attributes
{
    class MySQLFieldAttribute : Attribute
    {
        public string Name { get; set; }
    }

}
