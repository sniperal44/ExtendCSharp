using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public class ProgessBarPlus: ProgressBar
    {
        long _MaximumLong;
        long _ValueLong;
        int BitShift;

        public bool _Animation = true;
        public bool Animation
        {
            get
            {
                return _Animation;
            }
            set
            {
                _Animation = value;
            }
        }
        

        public long MaximumLong
        {
            get
            {
                return _MaximumLong;
            }
            set
            {
                _MaximumLong = value;
               
                long temp = value;

                int i = 0;
                while (temp > int.MaxValue)
                {
                    temp=temp >> 1;
                    i++;
                }
                BitShift = i;

                this.SetMaximumInvoke((int)temp);
            }
        }


        public long ValueLong
        {
            get
            {
                return _ValueLong;
            }
            set
            {
                _ValueLong = value;
                int v = (int)(_ValueLong >> BitShift);
                if (Animation)
                {
                    this.SetValueInvoke(v);
                }
                else
                {
                    this.SetValueNoAnimationInvoke(v);
                }
            }
        }


    }
}
