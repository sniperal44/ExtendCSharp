using ExtendCSharp.Attributes;
using ExtendCSharp.ExtendedClass;
using System;

namespace ExtendCSharp.README.DemoClasses
{
    //Creazione della classe per la lettura dei dati da una query
    class MySQL_demo_Class
    {
        //ogni campo da far riempire alla query occorre che sia contrassegnato con l'attributo:
        //MySQLField
        //e tramite il valore Name, specificare il nome della colonna corrispondente nella query

        //ATTENZIONE! se si utilizza il comando "as" nella query
        //select ID as NUOVO_NOME from tabella
        //occorrerà inserire nel campo Name -> NUOVO_NOME

        //Conversione dei dati

        // [ MySQL ]    -> [ C# ]
        // int          -> int
        // long         -> long
        // float        -> float
        // double       -> double
        // Date         -> DateTime
        // Time         -> TimeSpan
        // DateTime     -> DateTime
        // timestamp    -> DateTime
        // char         -> CharMySQL
        // bool         -> BoolMySQL

        // CharMySQL e BoolMySQL sono 2 classi customizzate
        // se si vuole creare una classe custom, in modo da parsare il dato,
        // occore crere una classe e basarsi sul modello di CharMySQL
        

        [MySQLField(Name = "ID")]   
         public int ID;


         [MySQLField(Name = "nome")]
         public String nome;
         [MySQLField(Name = "virgolaD")]
         public double virgolaD;
         [MySQLField(Name = "virgolaF")]
         public float virgolaF;

         [MySQLField(Name = "Data")]
         public DateTime Data;
         [MySQLField(Name = "Ora")]
         public TimeSpan Ora;
         [MySQLField(Name = "DataOra")]
         public DateTime DataOra;

         [MySQLField(Name = "Long")]
         public long Long;

         [MySQLField(Name = "timestamp")]
         public long timestamp;


        [MySQLField(Name = "Carattere")]
        public charMySQL carattere;

        [MySQLField(Name = "Bool")]
        public BoolMySQL Bool;




    }
}
