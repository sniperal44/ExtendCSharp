using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtendCSharp.Forms;

namespace ExtendCSharp.Controls
{
    public partial class FolderSelectBar : UserControl
    {
        /// <summary>
        /// Imposta se la casella di testo è solo leggibile
        /// </summary>
        public bool ReadOnly
        {
            get => textBoxPlus1.ReadOnly;
            set
            {
                textBoxPlus1.ReadOnly = value;
            }
        }
 

        private String textCaption = "";
        public String TextCaption {
            get => textCaption;
            set
            {
                textCaption = value;
                textBoxPlus1.TextCaption = value;
            }
        }


        public String Folder
        {
            get
            {
                return textBoxPlus1.Text;
            }
            set
            {
                textBoxPlus1.Text = value;
                textBoxPlus1.StartTextValidation();
            }
        }

        public event SelectFolderOpeningDelegate SelectFolderOpening;
        public event SelectFolderClosedDelegate SelectFolderClosed;
        public event ValidationTextDelegate ValidationText;

        public FolderSelectBar()
        {
            InitializeComponent();
            textBoxPlus1.TextCaption = TextCaption;
            textBoxPlus1.ValidationText += TextBoxPlus1_ValidationText;
        }

        private bool? TextBoxPlus1_ValidationText(string textToValidate)
        {
            return ValidationText?.Invoke(textToValidate);
        }

        private void button_select_Click(object sender, EventArgs e)
        {
            SelectFolderOpening?.Invoke();
            FolderSelectDialog fbd = new FolderSelectDialog();
            fbd.ShowDialog();

            textBoxPlus1.Text = fbd.FolderPath;
            SelectFolderClosed?.Invoke();

            textBoxPlus1.StartTextValidation();
        }


        

        
    }
    public delegate void SelectFolderOpeningDelegate();
    public delegate void SelectFolderClosedDelegate();
   


}
