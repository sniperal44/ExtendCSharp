using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;


namespace ExtendCSharp
{
    public class CountDown :IDisposable
    {
        Timer t = null;
        TimeSpanPlus tsp = null;

        public TimeSpanPlus Time
        {
            get
            {
                return tsp;
            }
        }
        public bool Running
        {
            get
            {
                return t.Enabled;
            }
        }

        public delegate void TickHandler(CountDown sender);
        public event TickHandler Tick;

        public CountDown()
        {
            tsp = new TimeSpanPlus();
            InitTimer();
        }
        public CountDown(TimeSpanPlus TimeSpan)
        {
            SetTime(TimeSpan);
            InitTimer();
        }
        public CountDown(TimeSpan TimeSpan)
        {
            SetTime(TimeSpan);
            InitTimer();
        }

        public void SetTime(TimeSpanPlus TimeSpan)
        {
            Stop();
            tsp = new TimeSpanPlus(TimeSpan);
        }
        public void SetTime(TimeSpan TimeSpan)
        {
            Stop();
            tsp = new TimeSpanPlus(TimeSpan);
        }

        private void InitTimer()
        {
            t = new Timer();
            t.Interval = 1000;
            t.Elapsed += (s, e) => {
                if (tsp.SubtractSeconds(1))
                {
                    if (Tick != null)
                        Tick(this);
                }
                else
                    t.Stop();
            };
        }


        public void Start()
        {
            if (t != null)
                t.Start();
        }
        public void Stop()
        {
            if(t!=null)
                t.Stop();
        }


        public void RemoveAllHandlers()
        {
            if(Tick!=null)
                foreach(Delegate d in Tick.GetInvocationList())
                {
                    Tick -= (TickHandler)d;
                }      
        }


        public String ToString(String Format)
        {
            return tsp.ToString(Format);
        }
        public override String ToString()
        {
            if( tsp.Time.Days==0)

                return tsp.ToString("h':'mm':'ss");
            else
                return tsp.ToString("d'g 'h':'mm':'ss");

        }

        public void Dispose()
        {
            Stop();
            t.Dispose();
        }



    }
}
