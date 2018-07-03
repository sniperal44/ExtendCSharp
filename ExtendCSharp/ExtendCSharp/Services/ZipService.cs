using ExtendCSharp.Interfaces;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ExtendCSharp.Services
{
    public class ZipService : IService
    {
        
        /*public static String Zip(String Soruce)
        {
            try
            {
                byte[] byteArray = new byte[Soruce.Length];
                int indexBA = 0;
                foreach (char item in Soruce.ToCharArray())
                {
                    byteArray[indexBA++] = (byte)item;
                }

                MemoryStream ms = new MemoryStream();
                GZipStream sw = new GZipStream(ms, CompressionMode.Compress);

                sw.Write(byteArray, 0, byteArray.Length);
                sw.Close();

                byteArray = ms.ToArray();
                System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
                foreach (byte item in byteArray)
                {
                    sB.Append((char)item);
                }
                ms.Close();
                sw.Dispose();
                ms.Dispose();
                return sB.ToString();
            }
            catch (Exception)
            {

                return "";
            }
        }
        public static String UnZip(String Soruce)
        {

            try
            {
                byte[] byteArray = new byte[Soruce.Length];
                int indexBA = 0;
                foreach (char item in Soruce.ToCharArray())
                {
                    byteArray[indexBA++] = (byte)item;
                }

                MemoryStream ms = new MemoryStream(byteArray);
                GZipStream sr = new GZipStream(ms, CompressionMode.Decompress);

                System.IO.MemoryStream msOut = new System.IO.MemoryStream();

                sr.CopyTo(msOut);
                var rbyte = msOut.ToArray();


                System.Text.StringBuilder sB = new System.Text.StringBuilder(rbyte.Length);
                for (int i = 0; i < rbyte.Length; i++)
                {
                    sB.Append((char)rbyte[i]);
                }
                sr.Close();
                ms.Close();
                sr.Dispose();
                ms.Dispose();
                return sB.ToString();
            }
            catch (Exception)
            {

                return "";
            }
        }
        */

        public String Zip(String Soruce)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(Soruce);
                using (var msi = new MemoryStream(bytes))
                {
                    using (var mso = new MemoryStream())
                    {
                        using (var gs = new GZipStream(mso, CompressionMode.Compress))
                        {
                            CopyTo(msi, gs);
                        }
                        return Convert.ToBase64String(mso.ToArray());
                    }
                }
            }
            catch (Exception)
            {

                return "";
            }
        }
        public String UnZip(String Soruce)
        {

            try
            {
                byte[] bytes = Convert.FromBase64String(Soruce);
                using (var msi = new MemoryStream(bytes))
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        CopyTo(gs, mso);
                    }
                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
            catch (Exception )
            {

                return "";
            }
        }
        private void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }

}
