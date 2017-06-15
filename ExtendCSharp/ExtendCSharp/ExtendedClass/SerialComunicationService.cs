using ExtendCSharp.Interfaces;
using ExtendCSharp.Struct;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
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




        public SerialComunicationPlus(String port)
        {
            serialPort = new SerialPort(port);
            serialPort.Open();

            t = new Thread(ListenLine);
            t.Start();
        }
        public SerialComunicationPlus(SerialComunicationSetting setting)
        {
            serialPort = new SerialPort(setting.Port,setting.Speed,setting.Parity,setting.DataBits,setting.StopBits);
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
