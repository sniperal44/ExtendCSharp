using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class SerialComunicationService : IDisposable,IService
    {
        private SerialPort serialPort;
        public SerialPort BaseSerialPort
        {
            get { return serialPort; }
        }

        private Thread t = null;


        public delegate void LineReceivedEventArgs(String Line);
        public event LineReceivedEventArgs OnLineReceived;




        public SerialComunicationService(String port)
        {
            serialPort = new SerialPort(port);
            serialPort.Open();

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
