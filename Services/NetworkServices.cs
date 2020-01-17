using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class NetworkService : IService
    {
        public string GetIPv4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
        public List<ComboIp> GetAllIPv4Addresses()
        {
            List<ComboIp> ipList = new List<ComboIp>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {

                foreach (var ua in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipList.Add(new ComboIp( ua.Address.ToString(), ni.Name));
                    }
                }
            }
            return ipList;
        }
    }

    public class ComboIp
    {
        public String Address { get; set; }
        public String Description { get; set; }

        public ComboIp(string address, string description)
        {
            Address = address;
            Description = description;
        }

        public override string ToString()
        {
            return Address + " - " + Description;
        }
    }
}
