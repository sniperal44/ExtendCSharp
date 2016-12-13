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
        public ListPlus(T item)
        {
            base.Add(item);
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
        public T RemoveAt(int index)
        {
            if (index < base.Count && index>=0)
            {
                int cprima = base.Count;
                T temp = base[index];
                base.RemoveAt(index);
                if (cprima != Count)
                    if (null != OnRemove)
                        OnRemove(this, null);

                return temp;
            }
            return default(T);
        }
        public T RemoveLast()
        {
            if(base.Count==0)
            {
                return default(T);
            }

            T temp = base[base.Count - 1];
            RemoveAt(base.Count - 1);
            return temp;
        }
        public T RemoveFirst()
        {
            if (base.Count == 0)
            {
                return default(T);
            }
            T temp = base[0];
            RemoveAt(0);
            return temp;
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




    public class ListPlusEnumerator<T>
    {
        #region Eventi

        public event EventHandler OnAdd { add { Lista.OnAdd += value; } remove { Lista.OnAdd -= value; } }
        public event EventHandler OnRemove { add { Lista.OnRemove += value; } remove { Lista.OnRemove -= value; } }

        #endregion

        #region Variabili

        private ListPlus<T> Lista;
        private int _Position;
        public int Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (value >= 0)
                {
                    for (int i = Lista.Count; i <= value; i++)
                        Lista.Add(default(T));
                    _Position = value;
                }
            }
        }

        public T Current
        {
            get
            {
                return Lista[_Position];
            }
            set
            {
                Lista[_Position] = value;
            }
        }

        public int Count
        {
            get
            {
                return Lista.Count;
            }
        }
        #endregion



        #region Costruttori

        public ListPlusEnumerator()
        {
            Lista = new ListPlus<T>();
            Position = 0;
        }
        public ListPlusEnumerator(T Item)
        {
            Lista = new ListPlus<T>(Item);
            Position = 0;
        }
        private ListPlusEnumerator(List<T> source)
        {
            Lista = new ListPlus<T>(source);
            Position = 0;
        }
        private ListPlusEnumerator(ListPlus<T> source)
        {
            Lista = new ListPlus<T>(source);
            Position = 0;
        }
        private ListPlusEnumerator(IEnumerable<T> source)
        {
            Lista = new ListPlus<T>(source);
            Position = 0;
        }
        public ListPlusEnumerator(ListPlusEnumerator<T> source)
        {
            Lista = new ListPlus<T>(source.Lista);
            Position = 0;
        }

        #endregion
        #region Metodi ListPlus

        private void Add(T item)
        {
            Lista.Add(item);
        }
        private void AddUnique(T item)
        {
            Lista.AddUnique(item);
        }
        private void AddUnique(ListPlus<T> items)
        {
            Lista.AddUnique(items);
        }
        public void Insert(int index, T item)
        {
            Lista.Insert(index,item);
        }
        public void InsertRange(int index, params T[] items)
        {
            Lista.InsertRange(index, items);
        }
        public void InsertRange(int index, IEnumerable<T> items)
        {
            Lista.InsertRange(index, items);
        }
        private void Remove(T item)
        {
            Lista.Remove(item);
            Position = 0;
        }
        private void RemoveAll(Predicate<T> match)
        {
            Lista.RemoveAll(match);
            Position = 0;
        }
        public void RemoveAt(int index)
        {
            Lista.RemoveAt(index);
            Position = 0;
        }
        public void RemoveRange(int index, int count)
        {
            Lista.RemoveRange(index, count);
            Position = 0;
        }
        private void RemoveRange(ListPlus<T> list)
        {
            Lista.RemoveRange(list);
            Position = 0;
        }
        private void RemoveNotInRange(ListPlus<T> list)
        {
            Lista.RemoveNotInRange(list);
            Position = 0;
        }
        private ListPlus<T> GetNotInList(ListPlus<T> NotInThisList)
        {
            return Lista.GetNotInList(NotInThisList);
        }
        private ListPlus<T> GetInList(ListPlus<T> InThisList)
        {
            return Lista.GetInList(InThisList);
        }
        public void Clear()
        {
            Lista.Clear();
            Position = 0;
        }
        #endregion



        /// <summary>
        /// Consente di spostare la posizione in avanti
        /// </summary>
        /// <returns>TRUE se l'elemento esisteva gia, FALSE se è stato creato un nuovo elemento</returns>
        public bool MoveNext()
        {
            bool b = true;
            if (Position >= Lista.Count)
                b = false;
            Position += 1;
            return b;
        }

        /// <summary>
        /// Consente di spostare la posizione all'indietro
        /// </summary>
        /// <returns>TRUE se l'elemento esiste, FALSE se la position corrente è 0</returns>
        public bool MovePrev()
        {
            bool b = true;
            if (Position < 0)
                b = false;
            else
                Position -= 1;

            return b;
        }

        public T RemoveLast(T DefaultValue=default(T))
        {
            if (Lista.Count > 0)
            {
                T temp = Lista.Last();
                Lista.RemoveLast();
                return temp;
            }
            return DefaultValue;
        }
        public T RemoveFirst(T DefaultValue = default(T))
        {
            if (Lista.Count > 0)
            {
                T temp = Lista.First();
                Lista.RemoveFirst();
                return temp;
            }
            return DefaultValue;
        }

        public T Last()
        {
            return Lista.Last();
        }
        public T First()
        {
            return Lista.First();
        }

        public ListPlusEnumerator<T> Invert()
        {
            ListPlusEnumerator<T> temp = new ListPlusEnumerator<T>();
            foreach(T t in Lista)
                temp.Insert(0, t);
            return temp;
        }
    }
}
