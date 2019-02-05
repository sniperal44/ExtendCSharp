using ExtendCSharp.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace ExtendCSharp.Services
{
    public class MutexObjectDispatcherServices<T>:IService where T : class
    {
        private List<T> list;
        MutexObjectDispatcherListType ListType;
        Mutex m;

        bool _End;
        public bool End
        {
            get {
                return _End;              
            }
            set
            {

                _End = value;

            }
        }


        public ICatcher<bool> pause;



        public MutexObjectDispatcherServices(MutexObjectDispatcherListType ListType, ICatcher<bool> pause )
        {
            this.ListType = ListType;
            this.pause = pause;
            list = new List<T>();
            m = new Mutex();
        }


        public void Add(T obj)
        {
            m.WaitOne();
            if (ListType==MutexObjectDispatcherListType.fifo)
            {
                list.Add(obj);                
            }
            else if(ListType == MutexObjectDispatcherListType.lifo)
            {
                list.Insert(0, obj);
            }
            m.ReleaseMutex();
        }


        public int Count()
        {
            return list.Count;
        }



        public T Get()
        {
            m.WaitOne();
            T temp = null;
            if (list.Count != 0)
                temp= list.RemoveAndGet(0, null);
            m.ReleaseMutex();

            return temp;
        }



    }
    public enum MutexObjectDispatcherListType
    {
        lifo,
        fifo
    }
}
