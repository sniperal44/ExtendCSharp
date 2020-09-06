using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class TcpListenerPlus:TcpListener
    {
        public event ClientConnectedDelegate ClientConnected;
        private ThreadPlus tp;

        /// <summary>
        /// Ritorna lo stato del listener
        /// </summary>
        public new bool Active
        {
            get { return base.Active; }
        }

        public TcpListenerPlus(int port):base(port)
        {

        } 
        public TcpListenerPlus(IPEndPoint localEP) : base(localEP)
        {

        }
        public TcpListenerPlus(IPAddress localaddr,int port) : base(localaddr, port)
        {
            
        }
        public TcpListenerPlus(String hostname, int port) : base(HostToIPAddr(hostname), port)
        {

            
        }

        static private IPAddress HostToIPAddr(string hostname)
        {
            IPAddress addr;
            if(IPAddress.TryParse(hostname,out addr))
            {
                return addr;
            }
            else
            {
                addr= Dns.GetHostEntry(hostname).AddressList.FirstOrDefault(); ;
                if( addr!=null)
                    return addr;
                throw new Exception("IP or Hostname not recognized");
            }
            
            
        }




        /*private void ThreadListener()
        {
            try
            {
                while (true)
                {
                    TcpClient client = AcceptTcpClient();
                    ClientConnected?.Invoke(client.ToPlus());
                }
            }
            catch(Exception ex) { }
        }
        public new void Start()
        {
            base.Start();
            tp = new ThreadPlus(ThreadListener);
            tp?.Start();
        }
        public new void Stop()
        {
            base.Stop();
            tp?.Abort();
        }*/
       
        CancellationTokenSource cs_listener;
        public new void Start()
        {
            Stop();
            cs_listener = new CancellationTokenSource();
            CancellationToken ct = cs_listener.Token;

            base.Start();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (!ct.IsCancellationRequested)
                    {
                        TcpClient client = AcceptTcpClient();
                        ClientConnected?.Invoke(client.ToPlus());
                    }
                }
                catch(Exception ex)
                {

                }
                
            }, ct);
        }
        public new void Stop()
        {
            if (cs_listener != null)
            {
                cs_listener.Cancel();
                base.Stop();
            }
        }




    }
    public delegate void ClientConnectedDelegate(TcpClientPlus client);
}
