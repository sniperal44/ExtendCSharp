using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public class TimerPlus: Component
    {
        const int DefaultMillisec = 1000;

        int _Interval;
        public int Interval
        {
            get
            {
                return _Interval;
            }

            set
            {
                _Interval = value;
            }
        }

        bool _Pause;
        public bool Pause
        {
            get
            {
                return _Pause;
            }

            set
            {
                _Pause = value;
            }
        }

        
        TimerPlusStatus _Status=TimerPlusStatus.Stopped;
        public TimerPlusStatus Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        public TimerPlus()
        {
            _Interval = DefaultMillisec;
        }

        /// <summary>
        /// Crea un oggetto TimerPlus
        /// </summary>
        /// <param name="Millisec">Intervallo di tempo tra un Tick e l'altro espresso in millisecondi</param>
        public TimerPlus(int Millisec)
        {
            _Interval = Millisec;
        }

        

        
        public event OnTickEventHandler OnTick;

        Thread t = null;

        public void Start()
        {
            if(_Status==TimerPlusStatus.Running)
                Stop();
            

            _Status = TimerPlusStatus.Running;
            t = new Thread(Ciclo);
            t.Start();
        }
        public void Stop()
        {
            if(t!=null)
            {
                try
                {
                    t.Abort();
                    t.Join();
                }
                catch(Exception ){ }
            }
        }

        public void Ciclo()
        {

            while(_Status==TimerPlusStatus.Running || _Status==TimerPlusStatus.Paused)
            {
                while (_Pause)
                    Thread.Sleep(1);
                if (OnTick != null)
                {
                    if (OnTick.Target is Control)
                    {
                        Control c = (OnTick.Target as Control);
                        if(!c.IsDisposed && !c.Disposing)
                           (OnTick.Target as Control).Invoke(OnTick);
                    }
                    else
                        OnTick();
                }
                    

                
                Thread.Sleep(_Interval);
            }
        }


    }
    public enum TimerPlusStatus
    {
        Running,
        Paused,
        Stopped
    }

    public delegate void OnTickEventHandler();
}
