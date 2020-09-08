using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace ExtendCSharp.Serializers
{
    
    public class ZeroFormatterService : ISerializer
    {

        public T Deserialize<T>(byte[] data)
        {
            return ZeroFormatterSerializer.Deserialize<T>(data);
        }
        public T Deserialize<T>(Stream stream)
        {
            return ZeroFormatterSerializer.Deserialize<T>(stream);
        }

    
        public byte[] Serialize(object o)
        {
            return ZeroFormatterSerializer.Serialize(o);
        }
        public void Serialize(Stream stream,object o)
        {
            ZeroFormatterSerializer.Serialize(stream,o);
        }
    }
}
