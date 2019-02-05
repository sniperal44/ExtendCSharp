using System.Drawing;

namespace ExtendCSharp.Wrapper
{
    public class PointReadOnly
    {
        Point _internal;

        public PointReadOnly(Point p)
        {
            _internal = p;
        }
        public int X
        {
            get { return _internal.X; }
        }
        public int Y
        {
            get { return _internal.Y; }
        }

        public static implicit operator PointReadOnly(Point d)
        {
            return new PointReadOnly(d);
        }
        public static implicit operator Point(PointReadOnly d)
        {
            return new Point(d._internal.X, d._internal.Y);
        }

    }

}
