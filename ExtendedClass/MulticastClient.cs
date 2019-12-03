﻿using ExtendCSharp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    [Serializable]
    public class MulticastPacket
    {
        public static int MaxDatagramLenght { get; private set; } = 1000;
        public static int SerializedLenght { get; private set; } = MaxDatagramLenght + 226; // 8= long-> Start address ( credo che ne servino molti di più)

        public int index { get; private set; }
        public bool Last { get; private set; } = false;
        public byte[] Data { get; private set; }

        public static MulticastPacket[] CreatePackets(byte[] Data)
        {
            byte[][] chunks = Data.Chunkize(MaxDatagramLenght);
            MulticastPacket[] packets = new MulticastPacket[chunks.Length];
            for (int i = 0; i < chunks.Length; i++)
            {
                packets[i] = new MulticastPacket();
                packets[i].Data = chunks[i];
                packets[i].index = i;
            }
            packets.Last().Last = true;
            return packets;
        }

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
        public static MulticastPacket Deserialize(byte[] data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(data, 0, data.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter formatter = new BinaryFormatter(); // the formatter that will serialize my object on my stream 
                    MulticastPacket m = (MulticastPacket)formatter.Deserialize(ms);
                    return m;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


    public class MulticastPacketGroup
    {
        List<MulticastPacket> list = new List<MulticastPacket>();

        public void AddPacket(MulticastPacket mp)
        {
            list[mp.index] = mp;
        }
        public bool Completed()
        {
            //TODO:
            return false;
        }

        public void Clear()
        {
            list.Clear();
        }
        public byte[] GetData()    
        {
            TODO! DA TESTARE
            int totalLen = 0;
            for(int i=0;i<list.Count;i++)
            {
                totalLen += list[i].Data.Length;
            }
            byte[] data = new byte[totalLen];
            long ByteWritten = 0;
            for (int i = 0; i < list.Count; i++)
            {
                Array.Copy(list[i].Data, 0, data,ByteWritten, list[i].Data.Length);
                ByteWritten += list[i].Data.Length;
            }

            return data
        }
    }

    public class MulticastClient:IDisposable
    { 

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


                IPAddress localIPAddr = IPAddress.Parse("172.22.195.29"); 

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

                MulticastPacket[] packets= MulticastPacket.CreatePackets(data);
                for (int i = 0; i < packets.Length; i++)
                {
                    SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                    e.RemoteEndPoint = endPoint;
                    byte[] d = packets[i].Serialize();
                    e.SetBuffer(d, 0, d.Length);

                    Socket.SendToAsync(e);
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
                try
                {
                    byte[] bytes = new Byte[MulticastPacket.SerializedLenght];
                    IPEndPoint groupEP = new IPEndPoint(ipAddress, Port);
                    EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

                    MulticastPacketGroup mpr = new MulticastPacketGroup();
                   
                    while (ListenerStatus)
                    {
                        
                        int ByteRead=Socket.ReceiveFrom(bytes, ref remoteEP);
                        MulticastPacket mp=MulticastPacket.Deserialize(bytes);
                        mpr.AddPacket(mp);
                        if( mpr.Completed())
                        {
                            onReceivedByte?.Invoke(mpr.GetStream(), remoteEP);
                        }
                        
                    }
                }
                catch(Exception ex)
                {

                }

            }).Start();
        }
        public void StopListener()
        {
            ListenerStatus = false;
        }

        public void Dispose()
        {
            Socket.Dispose();       //TODO: ci mette troppo a chiudersi... forse sta ancora aspettando di inviare i dati? 
            StopListener();
        }

        public delegate void ReceivedByteDelegate(MemoryStream s, EndPoint remoteEP);
        public event ReceivedByteDelegate onReceivedByte;


        
    }
}
