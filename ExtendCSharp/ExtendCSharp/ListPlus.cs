using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp
{
    public class ListPlus<T> : List<T>
    {

        public event EventHandler OnAdd;
        public event EventHandler OnRemove;

        public ListPlus()
        {
        }
        public ListPlus(List<T> source)
        {
            base.AddRange(source);
        }
        public ListPlus(ListPlus<T> source)
        {
            base.AddRange(source);
        }
        public ListPlus(IEnumerable<T> source)
        {
            base.AddRange(source);
        }

        public void Add(T item)
        {
            if (null != OnAdd)
            {
                OnAdd(this, null);
            }
            base.Add(item);
        }
        public void AddUnique(T item)
        {
            if (!base.Contains(item))
            {
                base.Add(item);
                if (null != OnAdd)
                    OnAdd(this, null);
            }
        }
        public void AddUnique(ListPlus<T> items)
        {
            int cprima = base.Count;
            foreach (T t in items)
                if (!base.Contains(t))
                    base.Add(t);
            if (cprima != Count)
                if (null != OnAdd)
                    OnAdd(this, null);
        }

        public void Remove(T item)
        {
            int cprima = base.Count;
            base.Remove(item);
            if (cprima != Count)
                if (null != OnRemove)
                    OnRemove(this, null);  
        }
        public void RemoveAll(Predicate<T> match)
        {
            int cprima = base.Count;
            base.RemoveAll(match);
            if (cprima != Count)
                if (null != OnRemove)
                    OnRemove(this, null);    
        }
        public void RemoveAt(int index)
        {
            if (index < base.Count)
            {
                int cprima = base.Count;
                base.RemoveAt(index);
                if (cprima != Count)
                    if (null != OnRemove)
                        OnRemove(this, null);
            } 
        }
        public void RemoveRange(int index,int count)
        {
            int cprima = base.Count;
            base.RemoveRange(index, count);
            if(cprima!= Count)
                if (null != OnRemove)
                    OnRemove(this, null);
        }
        public void RemoveRange(ListPlus<T> list)
        {
            int cprima = base.Count;
            foreach(T t in list)
                base.Remove(t);
            if (cprima != Count)
                if (null != OnRemove)
                    OnRemove(this, null);
        }
        public void RemoveNotInRange(ListPlus<T> list)
        {
            RemoveRange(GetNotInList(list));
        }

        public ListPlus<T> GetNotInList(ListPlus<T> NotInThisList)
        {
            ListPlus<T> t = new ListPlus<T>();
            foreach (T tt in this)
                if (!NotInThisList.Contains(tt))
                    t.Add(tt);
            return t;
        }
        public ListPlus<T> GetInList(ListPlus<T> InThisList)
        {
            ListPlus<T> t = new ListPlus<T>();
            foreach (T tt in this)
                if (InThisList.Contains(tt))
                    t.Add(tt);
            return t;
        }

    }

}
