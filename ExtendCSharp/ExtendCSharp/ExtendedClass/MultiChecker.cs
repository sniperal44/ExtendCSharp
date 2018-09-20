using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class MultiChecker<T>
    {
        
        Dictionary<T, bool> checkerValue;
        public MultiChecker()
        {
            checkerValue = new Dictionary<T, bool>();
        }


        public void SetValue(T key,bool value)
        {
            checkerValue[key]= value;
        }
        public bool? GetValue(T key)
        {
            if( checkerValue.ContainsKey(key))
            {
                return checkerValue[key];
            }
            return null;
            
        }
        public void RemoveValue(T key)
        {
            if (checkerValue.ContainsKey(key))
            {
                checkerValue.Remove(key);
            }
        }

        public bool AllChecked()
        {
            foreach(KeyValuePair<T,bool> kv in checkerValue)
            {
                if (!kv.Value)
                    return false;
            }
            return true;
        }
    }
}
