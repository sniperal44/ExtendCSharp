using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Interfaces
{
    public delegate void Updated(object self);
    public interface IUpdater
    {

        event Updated OnUpdated;
    }
}
