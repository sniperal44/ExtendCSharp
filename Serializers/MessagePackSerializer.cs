using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Serializers
{
    public class MessagePackSerializer:ISerializer
    {
        public T Deserialize<T>(byte[] data)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(data);
        }
        public T Deserialize<T>(Stream stream)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(stream);
        }


        public byte[] Serialize(object o)
        {
            return MessagePack.MessagePackSerializer.Serialize(o);
        }
        public void Serialize(Stream stream, object o)
        {
            MessagePack.MessagePackSerializer.Serialize(stream, o);
        }
    }
}
