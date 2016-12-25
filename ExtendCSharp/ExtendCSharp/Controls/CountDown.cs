using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;


namespace ExtendCSharp.Controls
{
    public class CountDown : Component
    {
        Timer t = null;
        TimeSpanPlus tsp = null;


        public int Interval { get; set; }

        

        /// <summary>
        /// Impostando il Tempo corrente, il timer verrà stoppato
        /// </summary>
        public TimeSpanPlus Time
        {
            get
            {
                return tsp;
            }
            set
            {
                Stop(StopStatus.Stopped);
                tsp = new TimeSpanPlus(value);
            }
        }
        public bool Running
        {
            get
            {
                return t.Enabled;
            }
        }


        public delegate void CountDownHandler(CountDown sender);
        public event CountDownHandler Tick;
        public event CountDownHandler Started;
        public delegate void CountDownStopHandler(CountDown sender, StopStatus s);
        public event CountDownStopHandler Stopped;


        public CountDown()
        {
            tsp = new TimeSpanPlus();
            this.Interval = 1000;
            InitTimer();
        }

        public CountDown(int Interval=1000)
        {
            tsp = new TimeSpanPlus();
            this.Interval = Interval;
            InitTimer();
        }
        public CountDown(TimeSpanPlus TimeSpan, int Interval = 1000)
        {
            SetTime(TimeSpan);
            this.Interval = Interval;
            InitTimer();
        }
        public CountDown(TimeSpan TimeSpan, int Interval = 1000)
        {
            SetTime(TimeSpan);
            this.Interval = Interval;
            InitTimer();
        }


        private void InitTimer()
        {
            t = new Timer();
            t.Interval = Interval;
            t.Elapsed += (s, e) => {
                if (tsp.SubtractMilliseconds(Interval) && tsp.TotalMilliseconds>0)
                {
                    if (Tick != null)
                        Tick(this);
                }
                else
                {
                    Stop(StopStatus.End);
                }

            };
        }


        public void SetTime(TimeSpanPlus TimeSpan)
        {
            Stop(StopStatus.Stopped);
            tsp = new TimeSpanPlus(TimeSpan);
           
        }
        public void SetTime(TimeSpan TimeSpan)
        {
            Stop(StopStatus.Stopped);
            tsp = new TimeSpanPlus(TimeSpan);
        }


        public void Start()
        {
            if (t != null)
            {
                t.Start();
                if (Started != null)
                    Started(this);
            }
        }
        public void Stop(StopStatus s = StopStatus.Stopped)
        {
            if (t != null)
            {
                t.Stop();
                if (Stopped != null)
                    Stopped(this,s);
            }
        }


        public void RemoveAllHandlers()
        {
            if(Tick!=null)
                foreach(Delegate d in Tick.GetInvocationList())
                    Tick -= (CountDownHandler)d;

            if (Started != null)
                foreach (Delegate d in Started.GetInvocationList())
                    Started -= (CountDownHandler)d;

            if (Stopped != null)
                foreach (Delegate d in Stopped.GetInvocationList())
                    Stopped -= (CountDownStopHandler)d;
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

        protected override void Dispose(bool disposing)
        {
            Stop(StopStatus.Stopped);
            t.Dispose();
            base.Dispose(disposing);

        }


    }
    public enum StopStatus
    {
        End,
        Stopped
    }
}
