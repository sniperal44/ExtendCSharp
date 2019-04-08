using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using ExtendCSharp.Struct;
using System;
using System.Text.RegularExpressions;

namespace ExtendCSharp.Services
{

    public class SerialServices : IService
    {
        public SerialComunicationPlus  StartCommunication(SerialComunicationSetting setting)
        {
            return new SerialComunicationPlus(setting.Port,setting.Speed);  //TODO: fix
        }

        public bool IsValidPort(String port)
        {
            var match = Regex.Match(port, "^COM[\\d]+$", RegexOptions.None);

            return match.Success;
        }
    }
}
