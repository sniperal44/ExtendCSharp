using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp
{
    public class ExcelCoordinates : ICloneable
    {

        public int Row { get; set; }
        public int Column { get; set; }



        public ExcelCoordinates(int column, int row)
        {
            Row = row;
            Column = column;
        }

        public ExcelCoordinates(string column, int row)
        {
            Row = row;
            Column = ConvertColumntStringIntoNumber(column);
        }


        public ExcelCoordinates(string ColumnRow)
        {
            ColumnRow = ColumnRow.ToUpper();
            int Status = 0; //0 = controllo lettere | 1 = controllo numeri
            String ColumnS = "";
            String RowS = "";

            for (int i = 0; i < ColumnRow.Length; i++)
            {
                if (Status == 0)      //se mi aspetto un carattere
                {
                    if (ColumnRow[i].IsUpperChar())     //e trovo un carattere
                    {
                        ColumnS += ColumnRow[i];     //lo aggiungo alle colonne
                    }
                    else if (ColumnRow[i].IsNumber())    //se invece trovo un numero 
                    {
                        if (ColumnS.Length == 0)         //se non ho ancora trovato una lettera
                        {
                            throw new ArgumentException("Attesa lettera");         //sintassi errata
                        }
                        else   //avendo invece già trovato una lettera, vuol dire che posso passare alla parte numeri
                        {
                            Status = 1;
                            RowS += ColumnRow[i];
                        }
                    }
                    else //se trovo altro
                    {
                        throw new ArgumentException("Attesa lettera o numero");
                    }

                }
                else if (Status == 1)
                {
                    if (ColumnRow[i].IsNumber())
                    {
                        RowS += ColumnRow[i];
                    }
                    else   //qualsiasi altro carattere diverso da un numero ( in questa fase ) è un errore
                    {
                        throw new ArgumentException("Atteso numero");
                    }

                }
            }
            Row = RowS.ParseInt();
            Column = ConvertColumntStringIntoNumber(ColumnS);
        }


        private static int ConvertColumntStringIntoNumber(string ColumnsString)
        {
            ColumnsString = ColumnsString.ToUpper();
            int N = 0;
            ColumnsString = ColumnsString.ReverseString();

            int NumeroLettereAlfabeto = 26;
            for (int i = 0; i < ColumnsString.Length; i++)
            {
                //i corrisponde alla potenza 
                char c = ColumnsString[i];
                if (!(c >= 'A' && c <= 'Z'))
                    throw new ArgumentException("Lettera non valida: " + c + "\r\nStringa di riferimento: " + ColumnsString);

                int pos = (c - 'A') + 1;
                N += pos * (int)Math.Pow(NumeroLettereAlfabeto, i);
            }
            return N;
        }

        public static String ConvertColumnRowToString(int Col,int Row)
        {
            String Lettere="";
            Col--;
            int NumeroLettereAlfabeto = 26;
            do
            {
                Lettere += (char)((Col % NumeroLettereAlfabeto)+(int)'A');
                Col /= NumeroLettereAlfabeto;
            } while (Col>0);

            return Lettere + Row;
        }
        public String ToStringRef()
        {
            return ConvertColumnRowToString(Column, Row);
        }
        public object Clone()
        {
            return new ExcelCoordinates(Column, Row);
        }

        public ExcelCoordinates CastClone()
        {
            return (ExcelCoordinates)Clone();
        }
    }
}
