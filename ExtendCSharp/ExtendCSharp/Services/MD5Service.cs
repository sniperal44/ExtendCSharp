using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class MD5Service:IDisposable,IService
    {

        public event MD5BlockTransformEventHandler OnMD5BlockTransformEventHandler;
        public event MD5BlockTransformEventHandler2 OnMD5BlockTransformEventHandler2;
        public event MD5ComputeHashFinishEventHandler OnMD5ComputeHashFinishEventHandler;


        MD5 md5 = null;
        public MD5Service()
        {
            md5 = MD5.Create();
        }
        public MD5Service(String algName)
        {
            md5 = MD5.Create(algName);
        }

        /// <summary>
        /// Permette di calcolare l'Hash MD5 di una stringa
        /// </summary>
        /// <param name="s">stringa su cui calcolare l'Hash</param>
        /// <returns>i byte dell'Hash calcolato ( usare ToHexString per convertirla in stringa ) </returns>
        public byte[] ComputeHashString(String s)
        {           
            byte[] temp = s.ToByteArrayASCII();
            return ComputeHashMultiBlock(temp, temp.Length);           
        }
        public byte[] ComputeHashMultiBlock(Stream s)
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
            return md5.Hash;
        }
        public  Thread ComputeHashMultiBlockThread(Stream s)
        {
            Thread t = new Thread(() =>
            {
                ComputeHashMultiBlock(s);
            });
            t.Start();
            return t;
        }


        public byte[] ComputeHashMultiBlock(byte[] input, int size)
        {
            int offset = 0;

            while (input.Length - offset >= size)
            {
                offset += md5.TransformBlock(input, offset, size, input, offset);
                OnMD5BlockTransformEventHandler2?.Invoke((offset * 100L) / input.Length, size, offset);
            }
            md5.TransformFinalBlock(input, offset, input.Length - offset);
            OnMD5ComputeHashFinishEventHandler?.Invoke(md5.Hash);
            return md5.Hash;
        }


        public Thread ComputeHashMultiBlockThread( byte[] input, int size)
        {
            Thread t = new Thread(() =>
            {
                ComputeHashMultiBlock(input,size);
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
