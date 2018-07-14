using ExtendCSharp.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    //TODO:
    public class RegistryService:IService
    {
        public RegistryService()
        {

        }

        

        public object GetValue(RegistryKey rkey, String ValueName)
        {
            return rkey.GetValue(ValueName);
        }
        public object GetValue(String rkey,String ValueName,object _default=null)
        {
            return Registry.GetValue(rkey, ValueName, _default);
        }

    }
}
