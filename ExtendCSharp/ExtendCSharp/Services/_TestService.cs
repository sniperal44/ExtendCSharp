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
            //PropertyInfo[] pi = tipo.GetProperties();
            //FieldInfo[] fi=tipo.GetFields();
            T newObj = new T();



            FieldInfo[] Campi = tipo.GetFields().Where(prop => Attribute.IsDefined(prop, typeof(MySQLFieldAttribute))).ToArray();
            FieldInfo CampoSpecifico= Campi.First(field=> Attribute.GetCustomAttribute(field, typeof(MySQLFieldAttribute))._Cast<MySQLFieldAttribute>().Name == "NOME");




            /*foreach (PropertyInfo propertyInfo in pi)
            {
                Log.Log.AddLog(propertyInfo.ToString());
            }

            foreach (FieldInfo fieldInfo in fi)
            {
                fieldInfo.SetValue(newObj, 1);
            }*/


            return newObj;
        }
    }
}
