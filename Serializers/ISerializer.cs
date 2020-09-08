using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Serializers
{
    interface ISerializer
    {
        T Deserialize<T>(byte[] data);
        T Deserialize<T>(Stream data);

        void Serialize(Stream stream, object objectToSerialize);
        byte[] Serialize(object objectToSerialize);
    }
}
