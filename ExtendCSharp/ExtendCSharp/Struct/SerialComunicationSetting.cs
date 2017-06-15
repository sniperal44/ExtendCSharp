using ExtendCSharp.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Struct
{
    public class SerialComunicationSetting : ICloneable
    {
        public int Speed;
        public int DataBits;
        public StopBits StopBits;
        public Parity Parity;
        public String Port;
        //public SerialProtocol serialProtocol;

       public object Clone()
        {
            return new SerialComunicationSetting
            {
                Speed = this.Speed,
                DataBits = this.DataBits,
                StopBits = this.StopBits,
                Parity = this.Parity,
                Port = this.Port,
            };
        }
    }
}
