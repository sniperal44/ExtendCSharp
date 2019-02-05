using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendCSharp.Controls.DataGrid
{
    public class DataGridViewImageCheckColumn: DataGridViewImageColumn
    {
        public DataGridViewImageCheckColumn()
        {
            this.CellTemplate = new DataGridViewImageCheckCell();
        }

        public Image TrueImage { get; set; }
        public Image FalseImage { get; set; }

       


    }
}
