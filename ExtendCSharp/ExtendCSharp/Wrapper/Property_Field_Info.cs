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
        public Property_Field_Info(PropertyInfo pi)
        {
            this.pi = pi;
        }
        public Property_Field_Info(FieldInfo fi)
        {
            this.fi = fi;
        }

        public void SetValue(object obj,object value)
        {
            if( pi!=null)
            {
                pi.SetMethod.Invoke(obj, value);
            }
            if (fi != null)
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
