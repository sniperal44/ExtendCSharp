using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp
{
    public class MD5Plus:IDisposable
    {

        public event MD5BlockTransformEventHandler OnMD5BlockTransformEventHandler;
        public event MD5BlockTransformEventHandler2 OnMD5BlockTransformEventHandler2;
        public event MD5ComputeHashFinishEventHandler OnMD5ComputeHashFinishEventHandler;


        MD5 md5 = null;
        public MD5Plus()
        {
            md5 = MD5.Create();
        }
        public MD5Plus(String algName)
        {
            md5 = MD5.Create(algName);
        }


    
        public Thread ComputeHashMultiBlockAsync(Stream s)
        {
            Thread t = new Thread(() =>
            {
                
                int BufferSize = 1024 * 1024;
                byte[] buffer = new byte[BufferSize];
                int readCount;

                double PercentPerRead = BufferSize * 100.0 / s.Length;
                while ((readCount = s.Read(buffer, 0, BufferSize)) > 0)
                {
                    md5.TransformBlock(buffer, 0, readCount, buffer, 0);
                    OnMD5BlockTransformEventHandler?.Invoke(PercentPerRead);
                }
                md5.TransformFinalBlock(buffer, 0, readCount);

                OnMD5ComputeHashFinishEventHandler?.Invoke(md5.Hash);
                
            });
            t.Start();
            return t;
        }

        public Thread ComputeHashMultiBlockAsync( byte[] input, int size)
        {
            Thread t = new Thread(() =>
            {
                int offset = 0;

                while (input.Length - offset >= size)
                {
                    offset += md5.TransformBlock(input, offset, size, input, offset);
                    OnMD5BlockTransformEventHandler2?.Invoke((offset * 100L) / input.Length, size, offset);
                }
                md5.TransformFinalBlock(input, offset, input.Length - offset);
                OnMD5ComputeHashFinishEventHandler?.Invoke(md5.Hash);
            });
            t.Start();
            return t;
        }

        public void Dispose()
        {
            md5.Dispose();
        }
    }
    public delegate void MD5BlockTransformEventHandler(double ReadPercent);
    public delegate void MD5BlockTransformEventHandler2(double Percent, int Size, int Offset);
    public delegate void MD5ComputeHashFinishEventHandler(byte[] Hash);
}
