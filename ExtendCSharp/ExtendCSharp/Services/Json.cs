using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class Json
    {
        public static T Deserialize<T>(String s)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                return JsonConvert.DeserializeObject<T>(s, settings);
            }
            catch (Exception ex) { return default(T); }
        }
        public static T Deserialize<T>(Stream s)
        {
            try
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd(), settings);
                }
            }
            catch (Exception ex) { return default(T); }
        }

        public static String Serialize(object o)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                return JsonConvert.SerializeObject(o, settings);
            }
            catch (Exception ex) { return default(String); }
        }
    }


   
}


