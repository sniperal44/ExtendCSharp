using ExtendCSharp.ExtendedClass;
using ExtendCSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Wrapper
{
    public class Property_Field_Info
    {
        PropertyInfo pi = null;
        FieldInfo fi = null;

        Type type = null;
        public Property_Field_Info(PropertyInfo pi)
        {
            this.pi = pi;
            type = pi.PropertyType;
        }
        public Property_Field_Info(FieldInfo fi)
        {
            this.fi = fi;
            type = fi.FieldType;
        }

        public void SetValue(object obj,object value)
        {
            
            if(type.GetInterfaces().Contains(typeof(ICastable)))
            {
                ICastable instance = (ICastable)Activator.CreateInstance(type); //Creo una nuova istanza dell'oggetto che implementa la ICastable
                value = instance.Cast(value);   //richiamo la funzione CAST e sostituisco l'oggetto corrente con quello castato
            }


            //in base se è una property o un field, richiamo la SetValue
            if ( pi!=null)
            { 
                pi.SetValue(obj, value);               
            }
            else if(fi != null)
            {
                fi.SetValue(obj, value);
            }
        }

        public static implicit operator Property_Field_Info(PropertyInfo pi)
        {
            return new Property_Field_Info(pi);
        }
        public static implicit operator Property_Field_Info(FieldInfo fi)
        {
            return new Property_Field_Info(fi);
        }
        public static implicit operator PropertyInfo(Property_Field_Info pfi)
        {
            return pfi.pi;
        }
        public static implicit operator FieldInfo(Property_Field_Info pfi)
        {
            return pfi.fi;
        }

    }
}
