using System.Windows.Forms;

namespace ExtendCSharp.Controls
{

    public class NumericUpDownPlus : NumericUpDown
    {
      
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
                hme.Handled = true;

            if (e.Delta > 0)
                this.Value += this.Increment;
            else if (e.Delta < 0)
                this.Value -= this.Increment;
        }
    }

}
