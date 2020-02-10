using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.TOFIX
{

    //TODO: da pulire e vedere se effittivamente è utile...
    [Serializable]
    public class DataPacket
    {
        public byte[] Data;


        /// <summary>
        /// Serializza l'oggetto in un array di byte
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter(); // the formatter that will serialize my object on my stream 
                formatter.Serialize(ms, this); // the serialization process 
                byte[] tmp = ms.ToArray();
                return tmp;
            }
        }
        /// <summary>
        /// Permette di serializzare il dataPacket corrente su uno stream 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        async public Task<OperationData> SerializeToStream(Stream s)
        {          
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter(); // the formatter that will serialize my object on my stream 
                formatter.Serialize(ms, this); // the serialization process 
                byte[] tmp = ms.ToArray();

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                await s.WriteAsync(tmp, 0, tmp.Length);  //NON AWAIT! non devo aspettare che termini!    
                stopWatch.Stop();
                return new OperationData(tmp.Length,stopWatch.Elapsed.TotalMilliseconds);
                

                /* var waitedTask = await Task.WhenAny(task, Task.Delay(1000));

                 await waitedTask; //Wait on the returned task to observe any exceptions.
                 //task.Dispose();
                 int a = 3;*/
                //TODO: Controllo se è stato terminato a causa del wait, allora chiudo lo stream -> errore di scrittura
            }      
        }


        static public DataPacket DeserializeFromStream(Stream s)
        {
            try
            {            
                BinaryFormatter formatter = new BinaryFormatter(); // the formatter that will serialize my object on my stream 
                DataPacket dp = (DataPacket)formatter.Deserialize(s);
                return dp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static public DataPacket Deserialize(byte[] data)
        {
            try
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    ms.Write(data, 0, data.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter formatter = new BinaryFormatter(); // the formatter that will serialize my object on my stream 
                    DataPacket dp = (DataPacket)formatter.Deserialize(ms);
                    return dp;
                }
              
            }
            catch (Exception ex)
            {
                return null;
            }
        }






        static private MemoryStream DeserializeServiceMemory = new MemoryStream();

        /// <summary>
        /// Permette di deserializzare un oggetto dati dei byte NON provenienti da uno stream ( viene lanciato un evento quando il pacchetto è completato )
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        static public DataPacket DeserializeAddPacket(byte[] data) {
            long pos = DeserializeServiceMemory.Position;
            DeserializeServiceMemory.Write(data,0,data.Length);
            pos = DeserializeServiceMemory.Position;
            BinaryFormatter formatter = new BinaryFormatter(); 
            
            try
            {
               
                DeserializeServiceMemory.Seek(0, SeekOrigin.Begin);
                DataPacket dp = (DataPacket)formatter.Deserialize(DeserializeServiceMemory);
                onDeserializationComplete?.Invoke(dp);
                byte[] buf = DeserializeServiceMemory.GetBuffer();

                DeserializeServiceMemory.Dispose();
                DeserializeServiceMemory = new MemoryStream();

                return dp;
            }
            catch (Exception e)
            {
                if(e.Message.StartsWith("Fine"))
                {
                    
                }
                else
                {

                }
                DeserializeServiceMemory.Seek(pos, SeekOrigin.Begin);

            }
            finally
            {
            }
            return null;
        }


        public delegate void DeserializationCompleteDelegate(DataPacket dp);
        public static event DeserializationCompleteDelegate onDeserializationComplete;

    }

    public class OperationData
    {
        public OperationData(int byteSend, double millisecond)
        {
            ByteSend = byteSend;
            Millisecond = millisecond;
        }

        public int ByteSend { get; set; }
        public double Millisecond { get; set; }
        public double Speed { get
            {
                return ByteSend / Millisecond;
            } 
        }
        public String SpeedText { get
            {
                double s = Speed;
                if( s<1000)
                {
                    return s+" B/s";
                }
                else if (s < 1000000)
                {
                    return (s/1000)+" KB/s";
                }
                if (s < 1000000000)
                {
                    return (s / 1000) + " MB/s";
                }
                if (s < 1000000000000)
                {
                    return (s / 1000) + " GB/s";
                }
                return s + " B/s";
            } 
        }

    }
   
}
