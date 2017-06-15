using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using ExtendCSharp.Struct;
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
        public SerialComunicationPlus  StartCommunication(SerialComunicationSetting setting)
        {
            return new SerialComunicationPlus(setting);
        }

        public bool IsValidPort(String port)
        {
            var match = Regex.Match(port, "^COM[\\d]+$", RegexOptions.None);

            return match.Success;
        }
    }
}
