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

        public T Test<T>() where T : new()
        {

            Type tipo = typeof(T);
           
            T newObj = new T();

          
            //Recupero tutti i campi PUBBLICI con l'attributo MySQLFieldAttribute
            FieldInfo[] Campi = tipo.GetFields().Where(prop => Attribute.IsDefined(prop, typeof(MySQLFieldAttribute))).ToArray();
            //Se si vogliono trovare tutte le Proprietà: sostituire GetFields con GetProperties
           


            //Recupero un campo dato il Name specificato nel MySQLFieldAttribute -> ritorna NULL se non lo trova
            FieldInfo CampoSpecifico = Campi.FirstOrDefault(field=> Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "NOME");




            FieldInfo Intero = Campi.FirstOrDefault(field => Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "Intero");
            FieldInfo Decimale = Campi.FirstOrDefault(field => Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "Decimale");
            FieldInfo Stringa = Campi.FirstOrDefault(field => Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "Stringa");


            Intero.SetValue(newObj, 1);
            Decimale.SetValue(newObj, (float)5.3);
            Stringa.SetValue(newObj, "Hello World!");

            return newObj;
        }
    }
}
