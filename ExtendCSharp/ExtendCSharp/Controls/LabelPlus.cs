using ExtendCSharp.Interfaces;
using System.Windows.Forms;

namespace ExtendCSharp.Controls
{
    public class LabelPlus : LabelPlus<object>
    {
        private object _TextObject = default(object);


        public new object TextObject
        {
            get
            {
                return _TextObject;
            }

            set
            {
                if (_TextObject != null && _TextObject is IUpdater)
                    (value as IUpdater).OnUpdated -= LabelPlus_OnUpdated;


                _TextObject = value;
                if (value == null)
                    base.Text = null;
                else
                    base.Text = value.ToString();

                if (value is IUpdater)
                {
                    (value as IUpdater).OnUpdated += LabelPlus_OnUpdated;
                }

            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
        }
    }


    public class LabelPlus<T>:Label
    {
        private T _TextObject = default(T);
        public T TextObject
        {
            get
            {
                return _TextObject;
            }
            set
            {
                if(_TextObject!= null && _TextObject is IUpdater)
                    (value as IUpdater).OnUpdated -= LabelPlus_OnUpdated;
                

                _TextObject = value;
                if (value == null)
                    base.Text = null;
                else
                    base.Text = value.ToString();

                if(value is IUpdater)
                {
                    (value as IUpdater).OnUpdated += LabelPlus_OnUpdated;
                }
            }
        }

        protected void LabelPlus_OnUpdated(object self)
        {
            base.Text = self.ToString();
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
        }
  
    }
}
