using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Serializers.Xml
{
    /// <summary>
    /// Legge e bufferizza dal NetworkStream andando a rilasciare una quantità di byte durante la read fino a che non trova il simbolo '<' nel buffer
    /// </summary>
    class NetworkStreamWrap : Stream
    {
        NetworkStream inter;
        MemoryStream internalBuffer;

        public NetworkStreamWrap(NetworkStream s)
        {
            inter = s;
            internalBuffer = new MemoryStream();
        }
        public override bool CanRead => inter.CanRead;

        public override bool CanSeek => inter.CanSeek;

        public override bool CanWrite => inter.CanWrite;

        public override long Length => inter.Length;

        public override long Position { get => inter.Position; set => inter.Position = value; }

        public override void Flush()
        {
            inter.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count <= 0)
                return 0;

            //se non ho dati in buffer, devo per forza aspettare qualcosa dallo stream 
            //oppure se c'è qualcosa sul buffer
            bool b1 = internalBuffer.Position == internalBuffer.Length;
            //bool b2 =  inter.DataAvailable;

            if (b1 /*|| b2*/)
            {
                byte[] tmp = new byte[count];

                int n = inter.Read(tmp, 0, tmp.Length);

                long OldPos = internalBuffer.Position;
                internalBuffer.Seek(0, SeekOrigin.End);
                internalBuffer.Write(tmp, 0, n);
                internalBuffer.Position = OldPos;
            }

            //prendi tutto quello che c'è nello stream fino a raggiungere un '<' e lo metto nel buffer
            if (internalBuffer.Length == internalBuffer.Position)
                return 0;

            //TODO: recupero il buffer da internalBuffer 
            //faccio un for da internalBuffer.Position a internalBuffer.Length
            //se trovo un '<' mi fermo
            //alla fine copio i dati nel buffer con Buffer.BlockCopy
            //( mi sa che devo controllare di non sforare dal buffer destinazione )

            byte[] tmpBuffer = new byte[internalBuffer.Length - internalBuffer.Position];
            int tmpBufferSize = tmpBuffer.Length;

            //MemoryStream tmp2 = new MemoryStream();
            int i = 0;
            for (;i< tmpBufferSize;i++ )
            {
                tmpBuffer[i] = (byte)internalBuffer.ReadByte();
                if (tmpBuffer[i] == '<')
                {
                    i++;
                    break;
                }
            }
            Buffer.BlockCopy(tmpBuffer, 0, buffer, offset, i);
            return i;


            /*while( internalBuffer.Position < internalBuffer.Length)
            {
                byte b = (byte)internalBuffer.ReadByte();


                //tmp2.WriteByte(b);
                if (b == '<')
                    break;
            }
            tmp2.ToArray().CopyTo(buffer, offset);
            return (int)tmp2.Length;*/
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return inter.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            inter.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            inter.Write(buffer, offset, count);
        }
    }

}
