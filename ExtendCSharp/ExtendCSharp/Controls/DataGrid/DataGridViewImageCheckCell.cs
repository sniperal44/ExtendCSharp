using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls.DataGrid
{
    public class DataGridViewImageCheckCell: DataGridViewImageCell
    {
        bool status;
        public bool Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                if (base.OwningColumn is DataGridViewImageCheckColumn)
                {
                    DataGridViewImageCheckColumn column = (DataGridViewImageCheckColumn)base.OwningColumn;
                    if (status)
                        this.Value = column.TrueImage;
                    else
                        this.Value = column.FalseImage;
                }
                
            }
        }

        public DataGridViewImageCheckCell()
        {
            status = false;
        }
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Status = !Status;        
            base.OnMouseClick(e);
            StatusChanged?.Invoke(this, status);
        }


        public delegate void StatusChangedEventArgs(DataGridViewImageCheckCell sender,bool NewStatus);
        public event StatusChangedEventArgs StatusChanged;

    }
}
