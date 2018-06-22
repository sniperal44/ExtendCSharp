using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class _TestService : IService
    {

        public void Test(object obj)
        {
            
            
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                Log.Log.AddLog(propertyInfo.ToString());
            }
        }
    }
}
