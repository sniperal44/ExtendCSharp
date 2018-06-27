using ExtendCSharp.Attributes;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Services
{
    public class _TestService : IService
    {

        public IEnumerable<T> Test<T>() where T : new()
        {

            Type tipo = typeof(T);
            List<T> OutList = new List<T>();
            Dictionary<String, FieldInfo> fieldDictionary = new Dictionary<string, FieldInfo>();
            

          
            //Recupero tutti i campi PUBBLICI con l'attributo MySQLFieldAttribute
            FieldInfo[] Campi = tipo.GetFields().Where(prop => Attribute.IsDefined(prop, typeof(MySQLFieldAttribute))).ToArray();
            //Se si vogliono trovare tutte le Proprietà: sostituire GetFields con GetProperties
            
            


            //Recupero un campo dato il Name specificato nel MySQLFieldAttribute -> ritorna NULL se non lo trova
            FieldInfo CampoSpecifico = Campi.FirstOrDefault(field=> Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "NOME");



            //recupero campo per campo in base al nome e li inserisco in un dizionario per una ricerca più rapida
            FieldInfo Intero = Campi.FirstOrDefault(field => Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "Intero");
            fieldDictionary.Add("Intero", Intero);

            FieldInfo Decimale = Campi.FirstOrDefault(field => Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "Decimale");
            fieldDictionary.Add("Decimale", Decimale);

            FieldInfo Stringa = Campi.FirstOrDefault(field => Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "Stringa");
            fieldDictionary.Add("Stringa", Stringa);



            //ciclo di tutti gli elementi
            for( int i=0;i<10;i++)
            {
                T tmp = new T();
                fieldDictionary["Intero"]?.SetValue(tmp,null);
                fieldDictionary["Decimale"]?.SetValue(tmp, null);
                fieldDictionary["Stringa"]?.SetValue(tmp, "Hello World!");
                OutList.Add(tmp);
            }

          

            return OutList;
        }
    }
}
