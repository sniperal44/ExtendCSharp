using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ExtendCSharp.Wrapper
{
    
    public class MultiKeyDictionary<TKey1,Tkey2,TValue>
    {
        class MultyKeyDictionaryComparer : IEqualityComparer<Tuple<TKey1, Tkey2>>
        {
            public bool Equals(Tuple<TKey1, Tkey2> x, Tuple<TKey1, Tkey2> y)
            {
                return x.Item1.Equals( y.Item1) && x.Item2.Equals(y.Item2);
            }

            public int GetHashCode(Tuple<TKey1, Tkey2> obj)
            {
                return obj.Item1.GetHashCode() ^ obj.Item2.GetHashCode();
            }
        }


        System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue> inter;

        #region Constructor

   
        public MultiKeyDictionary() 
        {
            inter = new System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue>(new MultyKeyDictionaryComparer());
            EventAssotiation();
        }

        public MultiKeyDictionary(Int32 capacity)
        {
            inter = new System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue>(capacity, new MultyKeyDictionaryComparer());
            EventAssotiation();
        }

        public MultiKeyDictionary(IEqualityComparer<Tuple<TKey1, Tkey2>> comparer)
        {
            inter = new System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue>(comparer);
            EventAssotiation();
        }

        public MultiKeyDictionary(Int32 capacity, IEqualityComparer<Tuple<TKey1, Tkey2>> comparer)
        {
            inter = new System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue>(capacity, comparer);
            EventAssotiation();
        }

        public MultiKeyDictionary(IDictionary<Tuple<TKey1, Tkey2>, TValue> dictionary)
        {
            inter = new System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue>(dictionary, new MultyKeyDictionaryComparer());
            EventAssotiation();
        }

        public MultiKeyDictionary(IDictionary<Tuple<TKey1, Tkey2>, TValue> dictionary, IEqualityComparer<Tuple<TKey1, Tkey2>> comparer)
        {
            inter = new System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue>(dictionary, comparer);
            EventAssotiation();
        }

        public MultiKeyDictionary(System.Collections.Generic.Dictionary<Tuple<TKey1, Tkey2>, TValue> inter)
        {
            this.inter = inter;
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public IEqualityComparer<Tuple<TKey1, Tkey2>> Comparer
        {
            get
            {
                return inter.Comparer;
            }
        }
        public Int32 Count
        {
            get
            {
                return inter.Count;
            }
        }
        public Dictionary<Tuple<TKey1, Tkey2>, TValue>.KeyCollection Keys
        {
            get
            {
                return inter.Keys;
            }
        }
        public Dictionary<Tuple<TKey1, Tkey2>, TValue>.ValueCollection Values
        {
            get
            {
                return inter.Values;
            }
        }

        public TValue this[TKey1 key1, Tkey2 key2]
        {
            get
            {
                return inter[new Tuple<TKey1, Tkey2>(key1, key2)];
            }
            set
            {
                inter[new Tuple<TKey1, Tkey2>(key1, key2)] = value;
            }
        }
        #endregion

        #region Methods

        public void Add(TKey1 key1, Tkey2 key2 , TValue value)
        {
            inter.Add(new Tuple<TKey1, Tkey2>(key1, key2), value);
        }

        public void Clear()
        {
            inter.Clear();
        }

        public bool ContainsKey(TKey1 key1, Tkey2 key2)
        {
            return inter.ContainsKey(new Tuple<TKey1, Tkey2>(key1, key2));
        }

        public bool ContainsValue(TValue value)
        {
            return inter.ContainsValue(value);
        }

        public Dictionary<Tuple<TKey1, Tkey2>, TValue>.Enumerator GetEnumerator()
        {
            return inter.GetEnumerator();
        }

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            inter.GetObjectData(info, context);
        }

        public void OnDeserialization(object sender)
        {
            inter.OnDeserialization(sender);
        }

        public bool Remove(TKey1 key1 , Tkey2 key2)
        {
            return inter.Remove(new Tuple<TKey1, Tkey2>(key1, key2));
        }

        public bool TryGetValue(TKey1 key1, Tkey2 key2, out TValue value)
        {
            return inter.TryGetValue(new Tuple<TKey1, Tkey2>(key1, key2), out value);
        }

        public String ToString()
        {
            return inter.ToString();
        }

        public bool Equals(object obj)
        {
            return inter.Equals(obj);
        }

        public Int32 GetHashCode()
        {
            return inter.GetHashCode();
        }

        public System.Type GetType()
        {
            return inter.GetType();
        }

        #endregion

        #region Events

        private void EventAssotiation()
        {
        }
        #endregion


    }
}
