using ExtendCSharp.Interfaces;
using System;

namespace ExtendCSharp.ExtendedClass
{
    //charMySQL -> ESEMPIO COMMENTATO PER UNA CUSTOM CLASS MySQL ( usabile con l'attributo MySQLField )  
    //La costruzione di una classe su questo modello permetterà la Property_Field_Info.SetValue di andare a cercare un CAST adeguato all'object ricevuto dal MySQL


    //Implemento la ICastable per definire un Cast specifico e "proprietario"
    public class charMySQL: ICastable
    {
        //il valore da memorizzare
        char c;

        //costruttori
        //ATTENZIONE! IMPLEMENTARE SEMPRE UN COSTRUTTORE VUOTO!
        public charMySQL()
        {
        }
        public charMySQL(char c)
        {
            this.c = c;
        }
        public charMySQL(string s)
        {
            if( s.Length>0)
                this.c = s[0];
        }


        //implementazione dell'interfaccia
        public object Cast(object o)
        {
            charMySQL tmp = null;
            //controllo se il tipo dell'oggetto passato corrisponde al tipo di dato che posso accettare ( in genere, i tipi di dato specificati dagli "implicit operator" )
            // e, richiamando gli implicit operator, vado a creare un oggetto del tipo della classe corrente (charMySQL)
            if (o is string)
                tmp = (string)o;
            else if (o is char)
                tmp = (char)o;
            return tmp;
        }
        public void SelfCast(object o)
        {
            charMySQL tmp = (charMySQL) Cast(o);
            if( tmp!=null)
            {
                this.c = tmp.c;
            }
        }


        //operatori ( cast ) impliciti 
        public static implicit operator charMySQL(char c)
        {
            return new charMySQL(c);
        }
        public static implicit operator charMySQL(String s)
        {
            return new charMySQL(s);
        }
        public static implicit operator char(charMySQL cms)
        {
            return cms.c;
        }
        public static implicit operator string(charMySQL cms)
        {
            return cms.c.ToString();
        }
    }

    public class BoolMySQL: ICastable
    {
        bool b;

        public BoolMySQL()
        {
        }
        public BoolMySQL(bool b)
        {
            this.b = b;
        }
        public BoolMySQL(sbyte s)
        {
            this.b = s != 0;    //se è diverso da 0 allora è TRUE
        }


        public object Cast(object o)
        {
            BoolMySQL tmp = null;
            if (o is sbyte)
                tmp = (sbyte)o;
            else if (o is bool)
                tmp = (bool)o;
            return tmp;
        }
        public void SelfCast(object o)
        {
            BoolMySQL tmp = (BoolMySQL)Cast(o);
            if (tmp != null)
            {
                this.b = tmp.b;
            }
        }


        public static implicit operator BoolMySQL(bool b)
        {
            return new BoolMySQL(b);
        }
        public static implicit operator BoolMySQL(sbyte s)
        {
            return new BoolMySQL(s);
        }
        public static implicit operator bool(BoolMySQL cms)
        {
            return cms.b;
        }
        public static implicit operator sbyte(BoolMySQL cms)
        {
            return cms.b ? (sbyte)1 : (sbyte)0;   //ritorno 1 se è TRUE e 0 se è FALSE
        }
    }


}
