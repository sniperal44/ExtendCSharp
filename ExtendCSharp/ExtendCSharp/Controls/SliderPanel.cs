using ExtendCSharp.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<SliderPanel, UserControl>))]
    public abstract class SliderPanel:UserControl
    {
        public abstract CanGoReturn CanGo(SlideFormButton dir);

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SliderPanel
            // 
            this.Name = "SliderPanel";
            this.Size = new System.Drawing.Size(331, 151);
            this.ResumeLayout(false);

        }
    }
    public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }

    public class CanGoReturn
    {
        public bool CanGo;
        public String Message;

        public CanGoReturn(bool CanGo,String Message="")
        {
            this.CanGo = CanGo;
            this.Message = Message;
        }


        public static bool operator ==(CanGoReturn a, CanGoReturn b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return false;
        }
        public static bool operator !=(CanGoReturn a, CanGoReturn b)
        {
            return !(a == b);
        }

        public static bool operator ==(CanGoReturn a, bool b)
        {
            if (a == null)
                return false;
            return a.CanGo == b;
        }
        public static bool operator !=(CanGoReturn a, bool b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if( obj is CanGoReturn)
                return this == obj as CanGoReturn;
            else if (obj is bool)
                return this ==(bool)obj;

            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
