using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    class TcpClientPlus:TcpClient
    {



        public Stream SendObject<T>(T Obj)
        {
            // Initialize a storage medium to hold the serialized object
            Stream stream = this.GetStream();

            // Serialize an object into the storage medium referenced by 'stream' object.
            BinaryFormatter formatter = new BinaryFormatter();

            // Serialize multiple objects into the stream
            formatter.Serialize(stream, Obj);

            // Return a stream with multiple objects
            return stream;
        }

        public T ReceiveObject<T>()
        {
            // Construct a binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            Stream stream = this.GetStream();


            // Deserialize the stream into object
            T obj = (T)formatter.Deserialize(stream);

            return obj;
        }

    }
}
