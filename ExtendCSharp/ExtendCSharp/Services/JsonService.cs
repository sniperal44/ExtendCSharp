
using ExtendCSharp.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;

namespace ExtendCSharp.Services
{
    public class JsonService :IService
    {
        /// <summary>
        /// Deserializza una stringa JSON in un oggetto 
        /// </summary>
        /// <typeparam name="T">Il tipo di oggetto da restituire</typeparam>
        /// <param name="jsonData">La stringa JSON da parsare</param>
        /// <returns></returns>
        public T Deserialize<T>(String jsonData)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                return JsonConvert.DeserializeObject<T>(jsonData, settings);
            }
            catch (Exception ex) { return default(T); }
        }
        public T Deserialize<T>(Stream jsonDataStream)
        {
            try
            {
                using (StreamReader sr = new StreamReader(jsonDataStream))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd(), settings);
                }
            }
            catch (Exception ) { return default(T); }
        }

        public XmlDocument DeserializeXmlNode(string jsonData)
        {
            return JsonConvert.DeserializeXmlNode(jsonData);
        }

        public String Serialize(object o)
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


