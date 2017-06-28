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


    }
}
