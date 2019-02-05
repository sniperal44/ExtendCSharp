using System;
using System.Threading;

namespace ExtendCSharp.ExtendedClass
{
    public class ThreadPlus 
    {
        public delegate void EndThreadDelegate(ThreadPlus t);
        public event EndThreadDelegate OnEnd;

        
        private Thread t;
        private Thread WaitEnd;

        private bool SuspendOnEnd = false;


        public ThreadPlus(ParameterizedThreadStart start)
        {
            t = new Thread(start);
        }
        public ThreadPlus(ThreadStart start)
        {
            t = new Thread(start);
        }
        public ThreadPlus(ParameterizedThreadStart start,int maxStackSize)
        {
            t = new Thread(start,maxStackSize);
        }
        public ThreadPlus(ThreadStart start, int maxStackSize)
        {
            t = new Thread(start, maxStackSize);
        }

        public void Start()
        {
            SuspendOnEnd = false;
            t.Start();
            Start_WhaitEndThread();
        }
        public void Start(object parameter)
        {
            SuspendOnEnd = false;
            t.Start(parameter);
            Start_WhaitEndThread();
        }




        private void Start_WhaitEndThread()
        {
            WaitEnd = new Thread(WaitEndThread);
            WaitEnd.Start();
        }
        private void WaitEndThread()
        {
            t.Join();
            if (!SuspendOnEnd)
                OnEnd?.Invoke(this);
            WaitEnd = null;

        }
        private void Interrupt_WhaitEndThread()
        {
            try
            {
                WaitEnd.Abort();
            }
            catch (Exception ) { }
        }







        public void Abort()
        {
            SuspendOnEnd = true;
            t.Abort();
        }
        public void Abort(object parameter)
        {
            t.Abort(parameter);
        }



        public void Join()
        {
            t.Join();
        }
        public void Join(int millisecondsTimeout)
        {
            t.Join(millisecondsTimeout);
        }
        public void Join(TimeSpan timeout)
        {
            t.Join(timeout);
        }


        public void Interrupt()
        {
            t.Interrupt();
        }



       /* public void Resume()
        {
            t.Resume();
        }
        public void Suspend()
        {
            t.Suspend();
            
        }
        */


        public ThreadState ThreadState
        {
            get
            {
                return t.ThreadState;
            }
        }
        public bool IsAlive
        {
            get
            {
                return t.IsAlive;
            }
        }


        public void SetApartmentState(ApartmentState state)
        {
            t.SetApartmentState(state);
        }
    }
}
