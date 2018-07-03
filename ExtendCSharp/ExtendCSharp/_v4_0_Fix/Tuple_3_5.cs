

#if (NETFX3_5)
namespace ExtendCSharp._v3_5_Fix
{

    public class Tuple<T1, T2>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        internal Tuple(T1 Item1, T2 Item2)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
        }
    }
    public static class Tuple
    {
        public static Tuple<T1, T2> New<T1, T2>(T1 Item1, T2 Item2)
        {
            var tuple = new Tuple<T1, T2>(Item1, Item2);
            return tuple;
        }
    }
}
#endif



