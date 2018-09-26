using ExtendCSharp.Interfaces;
using Ionic.Zip;
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

        public void ZipFolder(String startPath,String zipPath)
        {
            using (ZipFile zip = new ZipFile())
            {
                //add directory, give it a name
                zip.AddDirectory(startPath);
                zip.Save(zipPath);
            }


        }
 
        public void UnzipFile(String zipPath, String extractPath, ExtractProgressEventHandler extractProgressEventHandler)
        {
            
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                //Calcoli per conteggio percentuali
                int totalFileCount = 0,partialFileCount=0;
                long totalSize = 0, partialSizeTotal = 0, lastVal = 0;

                //Calcolo il numero totale di file da estrarre e la loro dimensione totale
                foreach (var entry in zip)
                {
                    if (!entry.IsDirectory)
                    {
                        totalFileCount++;
                        totalSize += entry.UncompressedSize;
                    }
                }
                

                
                zip.ExtractProgress += new EventHandler<Ionic.Zip.ExtractProgressEventArgs>(
                    (sender, e) =>
                    {
                        //Conto file processati
                        if(e.EventType==ZipProgressEventType.Extracting_AfterExtractEntry && e.CurrentEntry!=null && !e.CurrentEntry.IsDirectory)
                        {
                            partialFileCount++;
                        }
                        else if(e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                        {
                            //Prima di ogni estrazione, resetto l'ultimo valore corrente
                            lastVal = 0;
                        }


                        //Calcolo Percentuale Byte
                        System.Windows.Forms.Application.DoEvents();
                        if (e.TotalBytesToTransfer != 0)    //Escludo cartelle o casi speciali
                        {
                            long partialByteFromLastCheck = e.BytesTransferred - lastVal;       //calcolo quando byte ha elaborato dall'ultimo check
                            partialSizeTotal += partialByteFromLastCheck;                       //li sommo al totale
                            lastVal = e.BytesTransferred;                                       //imposto l'ultimo valore al valore corrente 
                        }
                       
                        //Creo un EventArgs più dettagliato 
                        ExtendCSharp.Event.EventArgs.ExtractProgressEventArgs tmp = new ExtendCSharp.Event.EventArgs.ExtractProgressEventArgs(e);
                        tmp.EntriesTotal = totalFileCount;
                        tmp.EntriesExtracted = partialFileCount;

                        tmp.TotalByteToTransfer = totalSize;
                        tmp.TotalByteTransferred = partialSizeTotal;

                        extractProgressEventHandler?.Invoke(tmp);           //invoco l'evento 
                    });
                zip.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);
            }
        }
        public delegate void ExtractProgressEventHandler(ExtendCSharp.Event.EventArgs.ExtractProgressEventArgs EventArgs);


    }
    



}
