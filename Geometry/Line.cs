using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Geometry
{
    /// <summary>
    /// Rappresenta una retta sul piano cartesiano rappresentata dalla formula y=mx+q
    /// </summary>
    class Line
    {
        double m, q;//y = mx+q

        bool isVertical = false;//uso la q per memorizzare la x     x=q


        public Line(double m, double q)
        {
            this.m = m;
            this.q = q;

        }
        //crea una retta x = N
        public Line(double x)
        {
            isVertical = true;
            this.q = x;
        }
        public Line(PointD first, PointD second)
        {
            if (second.X == first.X)
            {
                isVertical = true;
                this.q = first.X;
            }
            else
            {
                m = (second.Y - first.Y) / (second.X - first.X);
                q = first.Y - (m * first.X);
            }
        }


        public PointD Intersect(Line rect2)
        {
            PointD temp = new PointD();

            if (isVertical && rect2.isVertical)
                return null;
            else if (isVertical)
            {
                temp.X = (float)q;
                temp.Y = (float)((rect2.m * temp.X) + rect2.q);
            }
            else if (rect2.isVertical)
            {
                temp.X = (float)rect2.q;
                temp.Y = (float)((m * temp.X) + q);
            }
            else if (m == rect2.m)
                return null;
            else
            {
                temp.X = (float)((rect2.q - q) / (m - rect2.m));
                temp.Y = (float)((m * temp.X) + q);
            }
            return temp;
        }

    }
}
