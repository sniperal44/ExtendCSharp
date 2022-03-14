using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtendCSharp.Serializers.Xml
{
    public class XmlSerializerTcp<T>: ISerializer
    {
        XmlSerializer inter;
        public XmlSerializerTcp()
        {
            inter = new XmlSerializer(typeof(T));
        }




        public void Serialize(Stream stream, object objectToSerialize)
        {
            inter.Serialize(stream, objectToSerialize);
            stream.WriteByte(10);
            stream.WriteByte((byte)'<');
        }

        public byte[] Serialize(object objectToSerialize)
        {
            throw new NotImplementedException();
        }



        public T Deserialize<T>(byte[] data) 
        {
            throw new NotImplementedException();
        }

        public TT Deserialize<TT>(Stream data)
        {
            if (!(data is NetworkStream))
                throw new InvalidCastException("Lo stream deve essere un NetworkStream");

            using (XmlReaderTcp reader = new XmlReaderTcp(data._Cast<NetworkStream>()))
            {
                return inter.Deserialize(reader)._Cast<TT>();
            }
        }
        public T Deserialize(Stream data)
        {
            return Deserialize<T>(data);
        }


    }
}
