using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{

    public class SerialServices : IDisposable
    {
        private SerialPort serialPort;
        private Thread t=null;
        public SerialServices(String port)
        {
            serialPort = new SerialPort(port);
            serialPort.Open();

            t = new Thread(AscoltaLine);
            t.Start();  
        }

        void AscoltaLine()
        {
            try
            {


                while (true)
                {
                    String t = serialPort.ReadLine();
                    OnLineReceived?.Invoke(t);
                }
            }
            catch(Exception ex)
            {

            }
        }

      
        public delegate void LineReceivedEventArgs(String Line);
        public event LineReceivedEventArgs OnLineReceived;

        public void Dispose()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }

            if (t != null && t.IsAlive)
                t.Abort();
        }


        public void WriteLine(String Line)
        {
            serialPort.Write(Line);
            serialPort.Write("\r");
        }
       
        public SerialPort GetBaseSerialPort()
        {
            return serialPort;
        }


    }
}
