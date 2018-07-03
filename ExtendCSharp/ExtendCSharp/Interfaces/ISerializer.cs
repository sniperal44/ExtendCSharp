using System;

namespace ExtendCSharp.Interfaces
{
    public interface ISerializer
    {
        String Serialize();
        void Deserialize(String data);
    }
}
