using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Interfaces
{
    public abstract class ICatcher<T>
    {
        public virtual T value { get; }
        public static implicit operator T(ICatcher<T> input)
        {
            return input.value;
        }
    }

    
}
