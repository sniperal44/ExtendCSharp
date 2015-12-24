using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp
{
    public class TimeSpanPlus
    {
        private int _millisec, _sec, _min, _hour, _day;
        public TimeSpan Time
        {
            get
            {
                return new TimeSpan(_day,_hour,_min,_sec,_millisec);
            }
            set
            {
                _millisec = value.Milliseconds;
                _sec = value.Seconds;
                _min = value.Minutes;
                _hour = value.Hours;
                _day = value.Days;
            }
        }

        public int Milliseconds
        {
            get
            {
                return _millisec;
            }
        }
        public int Seconds
        {
            get
            {
                return _sec;
            }
        }
        public int Minutes
        {
            get
            {
                return _min;
            }
        }
        public int Hours
        {
            get
            {
                return _hour;
            }
        }
        public int Days
        {
            get
            {
                return _day;
            }
        }


        public long TotalMilliseconds
        {
            get
            {
                return _millisec+ TotalSeconds*1000;
            }
        }
        public long TotalSeconds
        {
            get
            {
                return _sec + TotalMinutes * 60;
            }
        }
        public long TotalMinutes
        {
            get
            {
                return _min+ TotalHours*60;
            }
        }
        public long TotalHours
        {
            get
            {
                return _hour+TotalDays*24;
            }
        }
        public long TotalDays
        {
            get
            {
                return _day;
            }
        }


        

        public TimeSpanPlus(int milliseconds = 0, int seconds = 0, int minutes = 0, int hours = 0, int days = 0)
        {
            SetTime(milliseconds, seconds, minutes, hours, days);
        }
        public TimeSpanPlus(TimeSpanPlus Time)
        {
            SetTime(Time);
        }
        public TimeSpanPlus(TimeSpan Time)
        {
            SetTime(Time);
        }


        public void SetTime(int milliseconds = 0, int seconds = 0, int minutes = 0, int hours = 0, int days = 0)
        {
            _millisec = milliseconds;
            _sec = seconds;
            _min = minutes;
            _hour = hours;
            _day = days;
        }
        public void SetTime(TimeSpanPlus Time)
        {
            _millisec = Time._millisec;
            _sec = Time._sec;
            _min = Time._min;
            _hour = Time._hour;
            _day = Time._day;
        }
        public void SetTime(TimeSpan Time)
        {
            _millisec = Time.Milliseconds;
            _sec = Time.Seconds;
            _min = Time.Minutes;
            _hour = Time.Hours;
            _day = Time.Days;
        }



      
        public void AddMilliseconds(int milliseconds)
        {
            int t = _millisec + milliseconds;
            if (t < 1000)
                _millisec = t;
            else
            {
                _millisec = t % 1000;
                AddSeconds(t / 1000);
            }
        }
        public void AddSeconds(int seconds)
        {
            int t = _sec + seconds;
            if (t < 60)
                _sec = t;
            else
            {
                _sec=t % 60;
                AddMinuts(t / 60);
            }
        }
        public void AddMinuts(int minutes)
        {
            int t = _min + minutes;
            if (t < 60)
                _min = t;
            else
            {
                _min = t % 60;
                AddHours(t / 60);
            }
        }
        public void AddHours(int hours)
        {
            int t = _hour + hours;
            if (t < 24)
                _hour = t;
            else
            {
                _hour = t % 24;
                AddDays(t / 24);
            }
        }
        public void AddDays(int days)
        {
            _day += days;
        }

        public void AddTime(int milliseconds = 0, int seconds = 0, int minutes = 0, int hours = 0, int days = 0)
        {
            AddMilliseconds(milliseconds);
            AddSeconds(seconds);
            AddMinuts(minutes);
            AddHours(hours);
            AddDays(days);
        }
        public void AddTime(TimeSpanPlus Time)
        {
            AddMilliseconds(Time.Milliseconds);
            AddSeconds(Time.Seconds);
            AddMinuts(Time.Minutes);
            AddHours(Time.Hours);
            AddDays(Time.Days);
        }
        public void AddTime(TimeSpan Time)
        {
            AddMilliseconds(Time.Milliseconds);
            AddSeconds(Time.Seconds);
            AddMinuts(Time.Minutes);
            AddHours(Time.Hours);
            AddDays(Time.Days);
        }



        public bool SubtractMilliseconds(int milliseconds)
        {
            if (milliseconds >= 1000)
            {
                if (!SubtractSeconds(milliseconds / 1000))
                    return false;
                milliseconds %= 1000;
            }

            if (_millisec >= milliseconds)
            {
                _millisec -= milliseconds;
                return true;
            }
            else
            {
                if (SubtractSeconds(1))
                {
                    _millisec = 1000 - milliseconds+ _millisec;
                    return true;
                }
                return false;

            }
        }
        public bool SubtractSeconds(int seconds)
        {
            if (seconds >= 60)
            {
                if (!SubtractMinuts(seconds / 60))
                    return false;
                seconds %= 60;
            }

            if (_sec >= seconds)
            {
                _sec -= seconds;
                return true;
            }
            else
            {
                if (SubtractMinuts(1))
                {
                    _sec = 60 - seconds+ _sec;
                    return true;
                }
                return false;

            }
        }
        public bool SubtractMinuts(int minutes)
        {
            if (minutes >= 60)
            {
                if (!SubtractHours(minutes / 60))
                    return false;
                minutes %= 60;
            }

            if (_min >= minutes)
            {
                _min -= minutes;
                return true;
            }
            else
            {
                if (SubtractHours(1))
                {
                    _min = 60 - minutes+ _min;
                    return true;
                }
                return false;

            }
        }
        public bool SubtractHours(int hours)
        {
            if(hours>=24)
            {
                if (!SubtractDays(hours / 24))
                    return false;
                hours %= 24;
            }

            if (_hour >= hours)
            {
                _hour -= hours;
                return true;
            }
            else
            {
                if(SubtractDays(1))
                {
                    _hour = 24 - hours+ _hour;
                    return true;
                }
                return false;
                
            }
        }
        public bool SubtractDays(int days)
        {
            if (_day >= days)
            {
                _day -= days;
                return true;
            }
            else
            {
                _day = 0;
                _hour = 0;
                _min = 0;
                _sec = 0;
                _millisec = 0;
                return false;
            }
        }

        public bool SubtractTime(int milliseconds = 0, int seconds = 0, int minutes = 0, int hours = 0, int days = 0)
        {
            return SubtractDays(days) && SubtractHours(hours) && SubtractMinuts(minutes) && SubtractSeconds(seconds) && SubtractMilliseconds(milliseconds);
        }
        public bool SubtractTime(TimeSpanPlus Time)
        {
            return SubtractDays(Time.Days) && SubtractHours(Time.Hours) && SubtractMinuts(Time.Minutes) && SubtractSeconds(Time.Seconds) && SubtractMilliseconds(Time.Milliseconds);
        }
        public bool SubtractTime(TimeSpan Time)
        {
            return SubtractDays(Time.Days) && SubtractHours(Time.Hours) && SubtractMinuts(Time.Minutes) && SubtractSeconds(Time.Seconds) && SubtractMilliseconds(Time.Milliseconds);
        }


        override public string ToString()
        {
            return Time.ToString();
        }
        public string ToString(string format)
        {
            return Time.ToString(format);
        }


        public static TimeSpanPlus operator +(TimeSpanPlus c1, TimeSpanPlus c2)
        {
            TimeSpanPlus t=new TimeSpanPlus(c1);
            t.AddTime(c2);
            return t;
        }

        public static TimeSpanPlus operator -(TimeSpanPlus c1, TimeSpanPlus c2)
        {
            TimeSpanPlus t = new TimeSpanPlus(c1);
            t.SubtractTime(c2);
            return t;
        }

    }
}
