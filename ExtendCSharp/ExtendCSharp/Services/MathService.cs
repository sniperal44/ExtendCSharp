using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class MathService:IService
    {
        public double RadToGrad(double Rad)
        {
            return Rad * (180 / Math.PI);
        }
        public double GradToRad(double Grad)
        {
            return (Math.PI / 180) * Grad;
        }

        /// <summary>
        /// calcola la differenza di angoli in senso orario
        /// </summary>
        /// <param name="FirstAngle"></param>
        /// <param name="SecondAngle"></param>
        /// <returns></returns>
        public double AngleDif(double FirstAngle, double SecondAngle)
        {
            double dif = SecondAngle - FirstAngle;
            while (dif < 0)
                dif += 360;
            return dif;
        }
        public float AngleDif(float FirstAngle, float SecondAngle)
        {
            return (float)AngleDif((double)FirstAngle, (double)SecondAngle);
        }


        public decimal Max(params decimal[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException("Passare almeno un elemento");
            else if (values.Length == 1)
                return values[0];
            else
            {
                decimal? max = null;

                foreach (decimal v in values)
                {
                    if (max == null || v > max)
                        max = v;
                }

                return max.Value;

            }
        }
        public decimal Min(params decimal[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException("Passare almeno un elemento");
            else if (values.Length == 1)
                return values[0];
            else
            {
                decimal? min = null;

                foreach (decimal v in values)
                {
                    if (min == null || v < min)
                        min = v;
                }

                return min.Value;

            }
        }


        public float Max(params float[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException("Passare almeno un elemento");
            else if (values.Length == 1)
                return values[0];
            else
            {
                float? max = null;

                foreach (float v in values)
                {
                    if (max == null || v > max)
                        max = v;
                }

                return max.Value;

            }
        }
        public float Min(params float[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException("Passare almeno un elemento");
            else if (values.Length == 1)
                return values[0];
            else
            {
                float? min = null;

                foreach (float v in values)
                {
                    if (min == null || v < min)
                        min = v;
                }

                return min.Value;

            }
        }
    }
}
