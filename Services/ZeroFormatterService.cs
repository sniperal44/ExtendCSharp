using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace ExtendCSharp.Services
{
    
    public class ZeroFormatterService : IService
    {

        public T Deserialize<T>(byte[] data)
        {
            try
            {
                return ZeroFormatterSerializer.Deserialize<T>(data);
            }
            catch (Exception ex) { return default(T); }
        }
        public T Deserialize<T>(Stream stream)
        {
            try
            {
                return ZeroFormatterSerializer.Deserialize<T>(stream);
            }
            catch (Exception ex) { return default(T); }
        }

    
        public byte[] Serialize(object o)
        {
            try
            {
                return ZeroFormatterSerializer.Serialize(o);
            }
            catch (Exception ex) { return default(byte[]); }

        }
    }
}
