using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public static class AdditionalData
    {
        //objects
        //1) l'oggetto a cui legare altri oggetti
        //2) key 
        //3) oggetto da aggiungere
        static Dictionary<object, Dictionary<object,object>> objs = new Dictionary<object, Dictionary<object, object>>();
        public static void SetAttribute(this object o, object Key,object attribute)
        {
            if(!objs.ContainsKey(o))
                objs[o] = new Dictionary<object, object>();

            objs[o][Key] = attribute;
        }

        public static object GetAttribute(this object o, object Key)
        {
            if (!objs.ContainsKey(o))
                return null;

            if (!objs[o].ContainsKey(Key))
                return null;

            return objs[o][Key];

        }
        public static object RemoveAttribute(this object o, object Key)
        {
            if (!objs.ContainsKey(o))
                return null;

            return objs[o].RemoveAndGet(Key);   //controlla già se è presente la chiave
        }


        public static T GetAttribute<T>(this object o, object Key)
        {
            if (!objs.ContainsKey(o))
                return default(T);

            if (!objs[o].ContainsKey(Key))
                return default(T);

            return (T)objs[o][Key];
        }
        public static T RemoveAttribute<T>(this object o, object Key)
        {
            if (!objs.ContainsKey(o))
                return default(T);

            return (T)objs[o].RemoveAndGet(Key,default(T));   //controlla già se è presente la chiave
        }
        public static void ClearAllAttribute(this object o)
        {
            if (objs.ContainsKey(o))
                objs.Remove(o);

        }
    }
}
