using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class StringCollectionManager
    {

        String[] data;

        int index=0;

        public int Index
        {
            get { return index; }
            set
            {
                if (value < data.Length)
                    index = value;
                else
                {
                    //TODO: pensare se dare errore | impostare a -1 | non fare niente
                    index = -1;
                }
            }
        }
        public int Length
        {
            get { return data.Length; }
        }
        public String Current
        {
            get
            {
                if (index == -1)
                    return null;
                return data[index];
            }
        }
        public IEnumerable<String> Data
        {
            get
            {
                return data.Select((s)=> { return s; });
            }
        }
        public object this[int i]
        {
            get
            {
                return data[i];
            }
        }




        public StringCollectionManager(String[] data)
        {
            this.data = data;
        }




        public void Reset()
        {
            index = 0;
        }
        public bool Next(int Step=1)
        {
            if (IsInLength(index + Step))
            {
                index+= Step;
                return true;
            }
            else
            {
                index = -1;
                return false;
            }
        }

        public bool GoTo(Func<String,bool> Comparator)
        {  
            for( ; index<data.Length; index++)
            {
                if (Comparator(data[index]))
                {
                    return true;
                }
            }
            index = -1;
            return false;
        }
        public bool GoToRegex(String pattern)
        {
            Regex r = new Regex(pattern);
            return GoTo((s) => { return r.IsMatch(s); });
        }
        public bool GoToContains(String str)
        {
            return GoTo((s) => { return s.Contains(str); });
        }
        

        private bool IsInLength(int index)
        {
            return index < data.Length && index > -1;
        }

        public String[] GetRange(int Start)
        {
            if (IsInLength(Start))
            {
                return data.SubArray(Start);
            }
            throw new ArgumentException("Controllare se Start (" + Start + ")  sia all'interno del range di Index validi");

        }
        public String[] GetRange(int Start,int End)
        {
            return GetRangeCount(Start, End - Start);
        }
        public String[] GetRangeCount(int Start, int Count)
        {
            if (IsInLength(Start) && IsInLength(Start + Count) && Count>0)
            {
                return data.SubArray(Start, Count);
            }
            throw new ArgumentException("Controllare se Start (" + Start + ") ed End o Start+Count (" + (Start + Count) + ") siano all'interno del range di Index validi e che       End sia più grande di Start / Count sia maggiore di 0 ");

        }



    }

}
