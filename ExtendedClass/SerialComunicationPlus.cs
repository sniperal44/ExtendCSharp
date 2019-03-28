using ExtendCSharp.Struct;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    //TODO: wrappare completamente la SerialPort
    public class SerialComunicationPlus : IDisposable
    {
        private SerialPort serialPort;
        public SerialPort BaseSerialPort
        {
            get { return serialPort; }
        }

        private Thread t = null;


        public delegate void LineReceivedEventArgs(String Line);
        public event LineReceivedEventArgs OnLineReceived;

        public bool RtsEnable {
            get
            {
                return serialPort.RtsEnable;
            }
            set
            {
                serialPort.RtsEnable= value;
            }
        }


        public SerialComunicationPlus(String port)
        {
            serialPort = new SerialPort(port);
            serialPort.Open();
           
        }

        public SerialComunicationPlus(String port,int baudRate)
        {
            serialPort = new SerialPort(port,baudRate);
            serialPort.Open();

        }

        public SerialComunicationPlus(SerialComunicationSetting setting)
        {
            serialPort = new SerialPort(setting.Port,setting.Speed,setting.Parity,setting.DataBits,setting.StopBits);
            serialPort.Open();
            
        }


        public void StartListenLineThread()
        {
            t = new Thread(ListenLine);
            t.Start();
        }



        void ListenLine()
        {
            try
            {
                while (true)
                {
                    String t = serialPort.ReadLine();
                    OnLineReceived?.Invoke(t);
                }
            }
            catch (Exception )
            {

            }
        }
        public void WriteLine(String Line)
        {
            serialPort.Write(Line);
            serialPort.Write("\r");
        }
        public char? ReadChar(int TimeOutMillis)
        {
            var token = new CancellationTokenSource(TimeOutMillis);
            Task<int?> t=Task<int?>.Factory.StartNew(() =>
            {
                while(!token.IsCancellationRequested)
                {
                    if(serialPort.BytesToRead>0)
                    {
                        return serialPort.ReadByte();        
                    }
                    Task.Delay(1);
                }
                return null;
            });
            t.Wait();
            return (char?)t.Result;
        }




        public void Dispose()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }

            if (t != null && t.IsAlive)
                t.Abort();
        }

    }
}
