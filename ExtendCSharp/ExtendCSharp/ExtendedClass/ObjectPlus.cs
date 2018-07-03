using ExtendCSharp.Interfaces;

namespace ExtendCSharp.ExtendedClass
{
    public class ObjectPlus : ICloneablePlus
    {

        public object Clone()
        {
            return this;
        }
    }
}
