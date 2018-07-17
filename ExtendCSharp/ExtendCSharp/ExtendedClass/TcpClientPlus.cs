using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ExtendCSharp;
using System.Threading;

namespace ExtendCSharp.ExtendedClass
{
    public class TcpClientPlus
    {

        private TcpClient inter;
        private ThreadPlus t;
        private int CheckStatusTime = 1000;
        private bool CheckTreadActive = true;


        public event ClosedDelegate Closed;



        #region Costruttori
        public TcpClientPlus(System.Net.IPEndPoint localEP)
        {
            inter = new System.Net.Sockets.TcpClient(localEP);
            EventAssotiation();
        }

        public TcpClientPlus()
        {
            inter = new System.Net.Sockets.TcpClient();
            EventAssotiation();
        }

        public TcpClientPlus(System.Net.Sockets.AddressFamily family)
        {
            inter = new System.Net.Sockets.TcpClient(family);
            EventAssotiation();
        }

        public TcpClientPlus(String hostname, Int32 port)
        {
            inter = new System.Net.Sockets.TcpClient(hostname, port);
            EventAssotiation();
        }
        public TcpClientPlus(TcpClient t)
        {
            inter = t;
            EventAssotiation();
        }
        #endregion

        #region Wrapper
        public void Connect(String hostname, Int32 port)
        {
            inter.Connect(hostname, port);
        }

        public void Connect(System.Net.IPAddress address, Int32 port)
        {
            inter.Connect(address, port);
        }

        public void Connect(System.Net.IPEndPoint remoteEP)
        {
            inter.Connect(remoteEP);
        }

        public void Connect(System.Net.IPAddress[] ipAddresses, Int32 port)
        {
            inter.Connect(ipAddresses, port);
        }

        public void Connect(String hostname,Int32 port,int timeoutMillisecond)
        {
            var result = inter.BeginConnect(hostname, port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMillisecond));

            if (!success)
            {
                throw new  SocketException((int)SocketError.TimedOut);
            }

        }

        public IAsyncResult BeginConnect(String host, Int32 port, System.AsyncCallback requestCallback, object state)
        {
            return inter.BeginConnect(host, port, requestCallback, state);
        }

        public IAsyncResult BeginConnect(System.Net.IPAddress address, Int32 port, System.AsyncCallback requestCallback, object state)
        {
            return inter.BeginConnect(address, port, requestCallback, state);
        }

        public IAsyncResult BeginConnect(System.Net.IPAddress[] addresses, Int32 port, System.AsyncCallback requestCallback, object state)
        {
            return inter.BeginConnect(addresses, port, requestCallback, state);
        }

        public void EndConnect(System.IAsyncResult asyncResult)
        {
            inter.EndConnect(asyncResult);
        }

        public Task ConnectAsync(System.Net.IPAddress address, Int32 port)
        {
            return inter.ConnectAsync(address, port);
        }

        public Task ConnectAsync(String host, Int32 port)
        {
            return inter.ConnectAsync(host, port);
        }

        public Task ConnectAsync(System.Net.IPAddress[] addresses, Int32 port)
        {
            return inter.ConnectAsync(addresses, port);
        }

        public NetworkStream GetStream()
        {
            return inter.GetStream();
        }

        public void Close()
        {
            StopCheckClose();
            inter.Close(); 
        }
        public void TryClose()
        {
            
        }

        public void Dispose()
        {
            inter.Dispose();
        }

        public String ToString()
        {
            return inter.ToString();
        }

        public Boolean Equals(object obj)
        {
            return inter.Equals(obj);
        }

        public Int32 GetHashCode()
        {
            return inter.GetHashCode();
        }

        public Type GetType()
        {
            return inter.GetType();
        }

        public System.Net.Sockets.Socket Client
        {
            get
            {
                return inter.Client;
            }
            set
            {
                inter.Client = value;
            }
        }
        public Int32 Available
        {
            get
            {
                return inter.Available;
            }
        }
        public bool Connected
        {
            get
            {
                return inter.Connected;
            }
        }
        public bool ExclusiveAddressUse
        {
            get
            {
                return inter.ExclusiveAddressUse;
            }
            set
            {
                inter.ExclusiveAddressUse = value;
            }
        }
        public Int32 ReceiveBufferSize
        {
            get
            {
                return inter.ReceiveBufferSize;
            }
            set
            {
                inter.ReceiveBufferSize = value;
            }
        }
        public Int32 SendBufferSize
        {
            get
            {
                return inter.SendBufferSize;
            }
            set
            {
                inter.SendBufferSize = value;
            }
        }
        public Int32 ReceiveTimeout
        {
            get
            {
                return inter.ReceiveTimeout;
            }
            set
            {
                inter.ReceiveTimeout = value;
            }
        }
        public Int32 SendTimeout
        {
            get
            {
                return inter.SendTimeout;
            }
            set
            {
                inter.SendTimeout = value;
            }
        }
        public System.Net.Sockets.LingerOption LingerState
        {
            get
            {
                return inter.LingerState;
            }
            set
            {
                inter.LingerState = value;
            }
        }
        public bool NoDelay
        {
            get
            {
                return inter.NoDelay;
            }
            set
            {
                inter.NoDelay = value;
            }
        }



        private void EventAssotiation()
        {
        }
        #endregion


        public void StartCheckClose()
        {
            if( t!=null)
            {
                StopCheckClose();
            }
            t = new ThreadPlus(ThreadCheckClose);
            CheckTreadActive = true;
            t.Start();

        }
        public void StopCheckClose()
        {
            try
            {
                CheckTreadActive = false;
                t.Join();
            }
            catch(Exception ex)
            {

            }
        }
        private void ThreadCheckClose()
        {
            do
            {
                Thread.Sleep(CheckStatusTime);
                if (!inter.IsConnected())
                {
                    bool ToDispose = false;
                    Closed?.Invoke(this,out ToDispose);
                    if(ToDispose)
                    {
                        CheckTreadActive = false;
                        Dispose();
                    }
                }
                    
            }
            while (CheckTreadActive);
        }



    }
    public delegate void ClosedDelegate(TcpClientPlus client,out bool ToDispose);
}
