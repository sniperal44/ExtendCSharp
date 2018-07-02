using ExtendCSharp.Attributes;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace ExtendCSharp.Services
{
    public class _TestService : IService
    {

        public void Test()
        {


#if (NETFX4_5)
            Log.Log.AddLog("v4_5 was set");
#endif

#if (NETFX4_0)
        Log.Log.AddLog("v4_0 was set");
#endif
#if (NETFX3_5)

        Log.Log.AddLog("v3_5 was set");

#endif


        }
    }
}
