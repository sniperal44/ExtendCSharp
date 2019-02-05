namespace ExtendCSharp.Interfaces
{
    public interface IBinarySerializer
    {
        byte[] Serialize();
        void Deserialize(byte[] data);
    }
}
