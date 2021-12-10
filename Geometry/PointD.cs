using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtendCSharp.Geometry
{
    /// <summary>
    /// Rappresenta un punto con cordinate double
    /// </summary>
    public class PointD
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointD()
        {
            X = 0;
            Y = 0;
        }
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static PointD operator +(PointD p1,PointD p2)
        {
            return new PointD(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static PointD operator -(PointD p1, PointD p2)
        {
            return new PointD(p1.X -p2.X, p1.Y - p2.Y);
        }

        public static PointD operator +(PointD p1, Vector p2)
        {
            return new PointD(p1.X + p2.X, p1.Y + p2.Y);
        }



        public static implicit operator System.Drawing.PointF(PointD d) => new System.Drawing.PointF((float)d.X, (float)d.Y);
        public static implicit operator PointD(System.Drawing.PointF d) => new PointD(d.X, d.Y);


        public static implicit operator System.Drawing.Point(PointD d) => new System.Drawing.Point((int)d.X, (int)d.Y);
        public static implicit operator PointD(System.Drawing.Point d) => new PointD(d.X, d.Y);

        

    }
}
