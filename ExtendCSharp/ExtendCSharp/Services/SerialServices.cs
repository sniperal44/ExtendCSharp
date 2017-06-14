using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{

    public class SerialServices : IService
    {
        public SerialComunicationService StartCommunication(String port)
        {
            return new SerialComunicationService(port);
        }

        public bool IsValidPort(String port)
        {
            var match = Regex.Match(port, "^COM[\\d]+$", RegexOptions.None);

            return match.Success;
        }
    }
}
