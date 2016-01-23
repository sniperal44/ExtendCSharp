using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class ObjectPlus : ICloneablePlus
    {

        public object Clone()
        {
            return this;
        }
    }
}
