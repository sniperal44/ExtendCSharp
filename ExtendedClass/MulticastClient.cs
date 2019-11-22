using ExtendCSharp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class MulticastClient:IDisposable
    {
        private int MaxDatagramLenght = 1000;

        IPAddress ipAddress;
        int Port;
        public Socket Socket;
         

        public MulticastClient(string Address, int port, bool initializeNow = true) : this(IPAddress.Parse(Address), port, initializeNow)
        {

        }
        public MulticastClient(IPAddress ipAddress, int port, bool initializeNow = true) : this(new IPEndPoint(ipAddress, port), initializeNow)
        {

        }
        public MulticastClient(IPEndPoint ipEndPoint, bool initializeNow = true)
        {
            ipAddress = ipEndPoint.Address;
            Port = ipEndPoint.Port;
            if(initializeNow)
            {
                JoinMulticast();
            }
        }

        /// <summary>
        /// Si unisce alla rete multicast
        /// </summary>
        public void JoinMulticast(bool RandomPort=false)
        {
            try
            {
                // Create a multicast socket.
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


                IPAddress localIPAddr = IPAddress.Parse("192.168.2.8");

                // Create an IPEndPoint object. 
                int TmpPort = Port;
                if (RandomPort)
                    TmpPort = 0;
                IPEndPoint IPlocal = new IPEndPoint(localIPAddr, TmpPort);       //TODO: cambio in ip dell'interfaccia di out?

                // Bind this endpoint to the multicast socket.
                Socket.Bind(IPlocal);

                // Define a MulticastOption object specifying the multicast group 
                // address and the local IP address.
                // The multicast group address is the same as the address used by the listener.
                MulticastOption mcastOption;
                mcastOption = new MulticastOption(ipAddress, localIPAddr);

                Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
        }

        public void Close()
        {
            StopListener();
            Socket.Close();
        }

        /// <summary>
        /// Invia un messaggio via connessione multicast
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            IPEndPoint endPoint;

            try
            {
                //Send multicast packets to the listener.
                endPoint = new IPEndPoint(ipAddress, Port);
                Socket.SendTo(ASCIIEncoding.ASCII.GetBytes(message), endPoint);


            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }

            //Socket.Close();
        }

        /// <summary>
        /// Invia un messaggio via connessione multicast
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage(byte[] data)
        {
            IPEndPoint endPoint;

            try
            {
                //Send multicast packets to the listener.
                endPoint = new IPEndPoint(ipAddress, Port);

                if(data.Length< MaxDatagramLenght)
                {
                    Socket.SendTo(data, endPoint);
                }
                else
                {
                    byte[][] chunks = data.Chunkize(MaxDatagramLenght);
                    for(int i=0;i<chunks.Length;i++)
                    {
                        //Socket.SendTo(chunks[i], endPoint);
                        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                        e.RemoteEndPoint = endPoint;
                        e.SetBuffer(chunks[i],0, chunks[i].Length);

                        Socket.SendToAsync(e);
                    }

                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }

            //Socket.Close();
        }




        public bool ListenerStatus { get; set; } = false;

        /// <summary>
        /// Permette di far partire il listener ( Task )che ascolta se ci sono byte in arrivo e lancia un evento all'arrivo di un pacchetto
        /// </summary>
        public void StartListen()
        {
            ListenerStatus = true;

            new Task(() =>
            {
                byte[] bytes = new Byte[MaxDatagramLenght];
                IPEndPoint groupEP = new IPEndPoint(ipAddress, Port);
                EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

                while (ListenerStatus)
                {
                    Socket.ReceiveFrom(bytes, ref remoteEP);
                    onReceivedByte?.Invoke(bytes, remoteEP);
                }

            }).Start();
        }
        public void StopListener()
        {
            ListenerStatus = false;
        }

        public void Dispose()
        {
            StopListener();
        }

        public delegate void ReceivedByteDelegate(byte[] data, EndPoint remoteEP);
        public event ReceivedByteDelegate onReceivedByte;
        
    }
}
