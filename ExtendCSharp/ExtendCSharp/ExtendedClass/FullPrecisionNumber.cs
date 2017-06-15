using ExtendCSharp.ExtendedClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.ExtendedClass
{
    public class FullPrecisionNumber
    {
        FullPrecisionNumberList Number;
        public FullPrecisionNumber(uint i)
        {
            Number = new FullPrecisionNumberList(i);
        }
        public FullPrecisionNumber(int i)
        {
            if(i<0)
            {
                Number = new FullPrecisionNumberList((uint)(i*-1));
                Positive = false;
            }
            else
                Number = new FullPrecisionNumberList((uint)i);
        }
        public FullPrecisionNumber(long i)
        {
            if (i < 0)
            {
                Number = new FullPrecisionNumberList((ulong)(i * -1));
                Positive = false;
            }
            else
                Number = new FullPrecisionNumberList((ulong)i);
        }
        public FullPrecisionNumber(ulong i)
        {
            Number = new FullPrecisionNumberList(i);
        }
        public FullPrecisionNumber(FullPrecisionNumber i)
        {
            Number = new FullPrecisionNumberList(i.Number);
        }

        public bool Positive = true;

        public uint myAdd(uint a, uint b)
        {
            uint carry = a & b;
            uint result = a ^ b;
            while (carry != 0)
            {
                uint shiftedcarry = carry << 1;
                //if shiftedcarry<carry -> c'è un bit che va shiftato
                carry = result & shiftedcarry;
                result ^= shiftedcarry;
            }
            return result;
        }


        public static FullPrecisionNumber operator +(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            FullPrecisionNumber a = new FullPrecisionNumber(c1.Positive?c1:~c1+1);
            FullPrecisionNumber b = new FullPrecisionNumber(c2.Positive ? c2 : ~c2 + 1);

            FullPrecisionNumber carry = a & b;
            FullPrecisionNumber result = a ^ b;
            while (carry != 0)
            {
                FullPrecisionNumber shiftedcarry = carry << 1;
                carry = result & shiftedcarry;
                result ^= shiftedcarry;
            }
            return result;
        }



        public static FullPrecisionNumber operator &(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            int indexC1 = c1.Number.Position;
            int indexC2 = c2.Number.Position;


            int C1Count = c1.Number.Count;
            int C2Count = c2.Number.Count;
            C1Count = C1Count > C2Count ? C1Count : C2Count;

            FullPrecisionNumber temp = new FullPrecisionNumber(0);
            for(int i=0;i<C1Count;i++)
            {
                c1.Number.Position = i;
                c2.Number.Position = i;
                temp.Number.Position = i;
                temp.Number.Current= c1.Number.Current & c2.Number.Current;
            }

            c1.Number.Position = indexC1;
            c2.Number.Position = indexC2;
            return temp;
        }
        public static FullPrecisionNumber operator ^(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            int indexC1 = c1.Number.Position;
            int indexC2 = c2.Number.Position;


            int C1Count = c1.Number.Count;
            int C2Count = c2.Number.Count;
            C1Count = C1Count > C2Count ? C1Count : C2Count;

            FullPrecisionNumber temp = new FullPrecisionNumber(0);
            for (int i = 0; i < C1Count; i++)
            {
                c1.Number.Position = i;
                c2.Number.Position = i;
                temp.Number.Position = i;
                temp.Number.Current = c1.Number.Current ^ c2.Number.Current;
            }

            c1.Number.Position = indexC1;
            c2.Number.Position = indexC2;
            return temp;
        }
        public static FullPrecisionNumber operator |(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            int indexC1 = c1.Number.Position;
            int indexC2 = c2.Number.Position;


            int C1Count = c1.Number.Count;
            int C2Count = c2.Number.Count;
            C1Count = C1Count > C2Count ? C1Count : C2Count;

            FullPrecisionNumber temp = new FullPrecisionNumber(0);
            for (int i = 0; i < C1Count; i++)
            {
                c1.Number.Position = i;
                c2.Number.Position = i;
                temp.Number.Position = i;
                temp.Number.Current = c1.Number.Current | c2.Number.Current;
            }

            c1.Number.Position = indexC1;
            c2.Number.Position = indexC2;
            return temp;
        }
        public static FullPrecisionNumber operator ~(FullPrecisionNumber c1)
        {
            int indexC1 = c1.Number.Position;

            FullPrecisionNumber temp = new FullPrecisionNumber(0);
            for (int i = 0; i < c1.Number.Count; i++)
            {
                c1.Number.Position = i;
                temp.Number.Position = i;
                temp.Number.Current = ~c1.Number.Current ;
            }
            c1.Number.Position = indexC1;
            return temp;
        }

        public static FullPrecisionNumber operator <<(FullPrecisionNumber c1, int n)
        {
            FullPrecisionNumber temp = new FullPrecisionNumber(c1);
            int NumUInt = n / (sizeof(uint) * 8);
            int ScartoUInt = n % (sizeof(uint) * 8);

            if(ScartoUInt==0)
            {
                temp.Number.InsertRange(0, new uint[NumUInt]);
            }
            else
            {
                temp.Number.InsertRange(0, new uint[NumUInt+1]);
                temp >>= (sizeof(uint) * 8) - ScartoUInt;
            }
            temp.Number.Trim();
            return temp;


        }
        public static FullPrecisionNumber operator >>(FullPrecisionNumber c1, int c2)
        {
            FullPrecisionNumber temp = new FullPrecisionNumber(c1);
            temp.Number.RemoveRange(0, c2 / (sizeof(uint) * 8)); //rimuovo i primi bit ( la mia dimensione è uint ) che verrebbero rimossi con lo shift
            c2 %= sizeof(uint) * 8;
            if(c2>0)
            {
                //uint t = (uint)Math.Pow(2, c2);
                uint t = 1;
                for (int i = 0; i < c2 - 1; i++)
                    t = (t << 1) + 1;


                uint OldPersi = 0;
                uint NewPersi = 0;
                for (int i = temp.Number.Count-1; i >=0; i--)
                {
                    temp.Number.Position = i;
                    NewPersi = temp.Number.Current & t;
                    temp.Number.Current = (temp.Number.Current >> c2) + (OldPersi << ((sizeof(uint) * 8) - c2));
                    OldPersi = NewPersi;
                }
            }

            return temp;
        }

        public static implicit operator FullPrecisionNumber(uint fpn)
        {
            return new FullPrecisionNumber(fpn);
        }
        public static implicit operator FullPrecisionNumber(long fpn)
        {
            return new FullPrecisionNumber(fpn);
        }

        public static bool operator ==(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            int indexC1 = c1.Number.Position;
            int indexC2 = c2.Number.Position;


            int C1Count = c1.Number.Count;
            int C2Count = c2.Number.Count;
            C1Count = C1Count > C2Count ? C1Count : C2Count;

            bool equal = true;
            for (int i = C1Count - 1; i >= 0 && equal; i--)
            {
                c1.Number.Position = i;
                c2.Number.Position = i;
                if (c1.Number.Current != c2.Number.Current)
                    equal = false;
            }

            c1.Number.Position = indexC1;
            c2.Number.Position = indexC2;

            return equal;
        }
        public static bool operator !=(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            return !(c1 == c2);
        }

        public static bool operator >=(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            int indexC1 = c1.Number.Position;
            int indexC2 = c2.Number.Position;


            int C1Count = c1.Number.Count;
            int C2Count = c2.Number.Count;
            C1Count = C1Count > C2Count ? C1Count : C2Count;

            int found = 0;
            for (int i = C1Count - 1; i >= 0 && found == 0; i--)
            {
                c1.Number.Position = i;
                c2.Number.Position = i;
                if (c1.Number.Current > c2.Number.Current)
                    found = 1;
                else if (c1.Number.Current < c2.Number.Current)
                    found = 2;
            }

            c1.Number.Position = indexC1;
            c2.Number.Position = indexC2;

            return found == 1 || found==0? true : false;
        }
        public static bool operator <=(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            return !(c1 > c2);
        }


        public static bool operator >(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            int indexC1 = c1.Number.Position;
            int indexC2 = c2.Number.Position;


            int C1Count = c1.Number.Count;
            int C2Count = c2.Number.Count;
            C1Count = C1Count > C2Count ? C1Count : C2Count;

            int found = 0;
            for (int i = C1Count-1; i >=0 && found==0; i--)
            {   
                c1.Number.Position = i;
                c2.Number.Position = i;
                if (c1.Number.Current > c2.Number.Current)
                    found = 1;
                else if (c1.Number.Current < c2.Number.Current)
                    found = 2;
            }
              
            c1.Number.Position = indexC1;
            c2.Number.Position = indexC2;

            return found==1?true:false;
        }
        public static bool operator <(FullPrecisionNumber c1, FullPrecisionNumber c2)
        {
            return !(c1 >= c2);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is FullPrecisionNumber))
                return false;
            else
                return this == ((FullPrecisionNumber)obj);
        }

        public override int GetHashCode()
        {
            int index = Number.Position;
            int c = Number.Count;

            uint t = 0;
            for (int i = 0; i <c; i--)
            {
                Number.Position = i;
                t += Number.Current;
            }
            Number.Position = index;
            return (int)t;
        }


        
    }

    class FullPrecisionNumberList : ListPlusEnumerator<uint>
    {

        public FullPrecisionNumberList(uint i):base(i)
        {
            
        }
        public FullPrecisionNumberList(ulong i):base()
        {
            Set(i);
        }
        public FullPrecisionNumberList(FullPrecisionNumberList List): base(List)
        {

        }

        public void Set(uint i)
        {
            Clear();
            Current = i;
        }
        public void Set(ulong i)
        {
            Clear();
            Current = (uint)i;
            MoveNext();
            Current= (uint)(i >> sizeof(uint) *8);
        }

        public void Trim()
        {
            while(Count>0)
            {
                if (Last() == 0)
                    RemoveLast();
                else
                    return;
            }
            Position = 0;
        }

    }




}
