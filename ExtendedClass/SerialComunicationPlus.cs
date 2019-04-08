using ExtendCSharp.Struct;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{

    public class SerialComunicationPlus:IDisposable
    {

        
        System.IO.Ports.SerialPort inter;
        MemoryStreamMutex serialStream = new MemoryStreamMutex();

        private String endLineChars = "\r\n";
        private byte[] endLineCharByte = "\r\n".ToByteArrayASCII();

        public String EndLineChars
        {
            get
            {
                return endLineChars;
            }
            set
            {
                endLineChars = value;
                endLineCharByte = endLineChars.ToByteArrayASCII();
            }
        }



        #region Constructor

        public SerialComunicationPlus(System.ComponentModel.IContainer container)
        {
            inter = new System.IO.Ports.SerialPort(container);
            EventAssotiation();
        }

        public SerialComunicationPlus()
        {
            inter = new System.IO.Ports.SerialPort();
            EventAssotiation();
        }

        public SerialComunicationPlus(String portName)
        {
            inter = new System.IO.Ports.SerialPort(portName);
            EventAssotiation();
        }

        public SerialComunicationPlus(String portName, Int32 baudRate)
        {
            inter = new System.IO.Ports.SerialPort(portName, baudRate);
            EventAssotiation();
        }

        public SerialComunicationPlus(String portName, Int32 baudRate, System.IO.Ports.Parity parity)
        {
            inter = new System.IO.Ports.SerialPort(portName, baudRate, parity);
            EventAssotiation();
        }

        public SerialComunicationPlus(String portName, Int32 baudRate, System.IO.Ports.Parity parity, Int32 dataBits)
        {
            inter = new System.IO.Ports.SerialPort(portName, baudRate, parity, dataBits);
            EventAssotiation();
        }

        public SerialComunicationPlus(String portName, Int32 baudRate, System.IO.Ports.Parity parity, Int32 dataBits, System.IO.Ports.StopBits stopBits)
        {
            inter = new System.IO.Ports.SerialPort(portName, baudRate, parity, dataBits, stopBits);
            EventAssotiation();
        }

        public SerialComunicationPlus(System.IO.Ports.SerialPort inter)
        {
            this.inter = inter;
        }

        #endregion

        #region Fields

        static public Int32 InfiniteTimeout;
        #endregion

        #region Properties

        public System.IO.Stream BaseStream
        {
            get
            {
                return inter.BaseStream;
            }
        }
        public Int32 BaudRate
        {
            get
            {
                return inter.BaudRate;
            }
            set
            {
                inter.BaudRate = value;
            }
        }
        public bool BreakState
        {
            get
            {
                return inter.BreakState;
            }
            set
            {
                inter.BreakState = value;
            }
        }
        public Int32 BytesToWrite
        {
            get
            {
                return inter.BytesToWrite;
            }
        }
        public Int32 BytesToRead
        {
            get
            {
                return inter.BytesToRead;
            }
        }
        public bool CDHolding
        {
            get
            {
                return inter.CDHolding;
            }
        }
        public bool CtsHolding
        {
            get
            {
                return inter.CtsHolding;
            }
        }
        public Int32 DataBits
        {
            get
            {
                return inter.DataBits;
            }
            set
            {
                inter.DataBits = value;
            }
        }
        public bool DiscardNull
        {
            get
            {
                return inter.DiscardNull;
            }
            set
            {
                inter.DiscardNull = value;
            }
        }
        public bool DsrHolding
        {
            get
            {
                return inter.DsrHolding;
            }
        }
        public bool DtrEnable
        {
            get
            {
                return inter.DtrEnable;
            }
            set
            {
                inter.DtrEnable = value;
            }
        }
        public System.Text.Encoding Encoding
        {
            get
            {
                return inter.Encoding;
            }
            set
            {
                inter.Encoding = value;
            }
        }
        public System.IO.Ports.Handshake Handshake
        {
            get
            {
                return inter.Handshake;
            }
            set
            {
                inter.Handshake = value;
            }
        }
        public bool IsOpen
        {
            get
            {
                return inter.IsOpen;
            }
        }
        public String NewLine
        {
            get
            {
                return inter.NewLine;
            }
            set
            {
                inter.NewLine = value;
            }
        }
        public System.IO.Ports.Parity Parity
        {
            get
            {
                return inter.Parity;
            }
            set
            {
                inter.Parity = value;
            }
        }
        public byte ParityReplace
        {
            get
            {
                return inter.ParityReplace;
            }
            set
            {
                inter.ParityReplace = value;
            }
        }
        public String PortName
        {
            get
            {
                return inter.PortName;
            }
            set
            {
                inter.PortName = value;
            }
        }
        public Int32 ReadBufferSize
        {
            get
            {
                return inter.ReadBufferSize;
            }
            set
            {
                inter.ReadBufferSize = value;
            }
        }
        public Int32 ReadTimeout
        {
            get
            {
                return inter.ReadTimeout;
            }
            set
            {
                inter.ReadTimeout = value;
            }
        }
        public Int32 ReceivedBytesThreshold
        {
            get
            {
                return inter.ReceivedBytesThreshold;
            }
            set
            {
                inter.ReceivedBytesThreshold = value;
            }
        }
        public bool RtsEnable
        {
            get
            {
                return inter.RtsEnable;
            }
            set
            {
                inter.RtsEnable = value;
            }
        }
        public System.IO.Ports.StopBits StopBits
        {
            get
            {
                return inter.StopBits;
            }
            set
            {
                inter.StopBits = value;
            }
        }
        public Int32 WriteBufferSize
        {
            get
            {
                return inter.WriteBufferSize;
            }
            set
            {
                inter.WriteBufferSize = value;
            }
        }
        public Int32 WriteTimeout
        {
            get
            {
                return inter.WriteTimeout;
            }
            set
            {
                inter.WriteTimeout = value;
            }
        }
        public ISite Site
        {
            get
            {
                return inter.Site;
            }
            set
            {
                inter.Site = value;
            }
        }
        public System.ComponentModel.IContainer Container
        {
            get
            {
                return inter.Container;
            }
        }
        #endregion

        #region Methods

        public void Close()
        {
            inter.Close();
        }

        public void DiscardInBuffer()
        {
            inter.DiscardInBuffer();
        }

        public void DiscardOutBuffer()
        {
            inter.DiscardOutBuffer();
        }

        public static System.String[] GetPortNames()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }

        public void Open()
        {
            inter.Open();
        }

        public Int32 Read(System.Byte[] buffer, Int32 offset, Int32 count)
        {
            serialStream.Position = 0;
            Int32 ByteLetti = serialStream.Read(buffer, offset, count);
            serialStream.Remove(ByteLetti);
            serialStream.Seek(0, SeekOrigin.End);
            return ByteLetti;
        }
        public char ReadChar()
        {
          
            serialStream.Position = 0;
            char car = (char)serialStream.ReadByte();         //TODO: testare!
            serialStream.Remove(sizeof(char));        //TODO: testare!
            serialStream.Seek(0, SeekOrigin.End);
            return car;
        }
        public char? ReadChar(int TimeOutMillis)
        {
            var token = new CancellationTokenSource(TimeOutMillis);
            Task<int?> t = Task<int?>.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (serialStream.Position > 0)
                    {
                        return ReadChar();
                    }
                    Task.Delay(1);
                }
                return null;
            });
            t.Wait();
            return (char?)t.Result;
        }


        public byte ReadByte()
        {

            serialStream.Position = 0;
            byte car = (byte)serialStream.ReadByte();         //TODO: testare!
            serialStream.Remove(sizeof(byte));        //TODO: testare!
            serialStream.Seek(0, SeekOrigin.End);
            return car;
        }
        public byte? ReadByte(int TimeOutMillis)
        {
            var token = new CancellationTokenSource(TimeOutMillis);
            Task<int?> t = Task<int?>.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (serialStream.Position > 0)
                    {
                        return ReadByte();
                    }
                    Task.Delay(1);
                }
                return null;
            });
            t.Wait();
            return (byte?)t.Result;
        }

        public String ReadExisting()
        {
            int len =  (int)serialStream.Position;
            serialStream.Position = 0;
            byte[] tmp = new byte[len];
            Read(tmp, 0, len);
            serialStream.Remove(len);
            serialStream.Seek(0, SeekOrigin.End);
            return tmp.ToASCIIString();
        }

        public String ReadLine()
        {
            return ReadTo(endLineChars);
        }

        //TODO: da testare
        public String ReadTo(String value)
        {   
            int indice = IndexOf(value);
            byte[] data = null;
            if ( indice!=-1)
            {
                //prendi i dati prima dell'indice
                serialStream.Position = 0;
                data = serialStream.Read(indice+ value.Length);
                serialStream.Remove(data.Length);
                serialStream.Seek(0, SeekOrigin.End);
            }
         



            if (data != null)
                return data.ToASCIIString();
            else
                return null;
            
        }


        public int IndexOf(String text)
        {
            return IndexOf(text.ToByteArrayASCII());
        }
        public int IndexOf(byte[] data)
        {
            byte[] buffer = serialStream.GetSizedBuffer();

            return buffer.IndexOf(data); 
        
        }

        public void Write(String text)
        {
            inter.Write(text);
        }

        public void Write(System.Char[] buffer, Int32 offset, Int32 count)
        {
            inter.Write(buffer, offset, count);
        }

        public void Write(System.Byte[] buffer, Int32 offset, Int32 count)
        {
            inter.Write(buffer, offset, count);
        }

        public void Write(byte b)
        {
            inter.Write(new byte[] { b }, 0, 1);
        }

        public void WriteLine(String text)
        {
            inter.WriteLine(text);
        }

        public void Dispose()
        {
            inter.Dispose();
            serialStream.Dispose();

           /* if (t != null && t.IsAlive)
                t.Abort();*/
        }

        public String ToString()
        {
            return inter.ToString();
        }

        public object GetLifetimeService()
        {
            return inter.GetLifetimeService();
        }

        public object InitializeLifetimeService()
        {
            return inter.InitializeLifetimeService();
        }

        public System.Runtime.Remoting.ObjRef CreateObjRef(System.Type requestedType)
        {
            return inter.CreateObjRef(requestedType);
        }

        public bool Equals(object obj)
        {
            return inter.Equals(obj);
        }

        public Int32 GetHashCode()
        {
            return inter.GetHashCode();
        }
        public byte[] Read(int count)
        {
                //Dichiaro un array lungo quanto passato ( count ) 
                byte[] temp = new byte[count];
                inter.Read(temp, 0, temp.Length);
                return temp;       
        }
        public void Write(byte[] buffer)
        {
            inter.Write(buffer, 0, buffer.Length);          
        }


        #endregion

        #region Events

        public event System.IO.Ports.SerialErrorReceivedEventHandler ErrorReceived;
        public event System.IO.Ports.SerialPinChangedEventHandler PinChanged;
        public event System.IO.Ports.SerialDataReceivedEventHandler DataReceived;
        public event System.IO.Ports.SerialDataReceivedEventHandler LineReceived;
        public event EventHandler Disposed;
        private void EventAssotiation()
        {
            inter.ErrorReceived += ErrorReceived;
            inter.PinChanged += PinChanged;
            inter.DataReceived += DataReceived;
            inter.DataReceived += Inter_DataReceived;
            inter.Disposed += Disposed;
        }


        #endregion

        MemoryStreamMutex LineBuffer = new MemoryStreamMutex();
        private void Inter_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            MemoryStreamMutex msTemp = new MemoryStreamMutex();
            if(BytesToRead>0)
            {
                msTemp.Write(Read(BytesToRead));
            }
                //msTemp.WriteByte((byte)inter.ReadByte());

            
            byte[] buff = msTemp.GetSizedBuffer();
            serialStream.Write(buff);
            LineBuffer.Write(buff);

            buff = LineBuffer.GetSizedBuffer();
             
            while (true)
            {
                buff = LineBuffer.GetSizedBuffer();
                int index = buff.IndexOf(endLineCharByte);
                if (index == -1)
                    break;

                LineReceived?.Invoke(this, e);
                LineBuffer.Remove(index + endLineCharByte.Length);   
            }
            

            
        }

    }
}