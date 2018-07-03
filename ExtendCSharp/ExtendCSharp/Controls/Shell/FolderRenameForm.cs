using ExtendCSharp.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExtendCSharp.Controls.Shell
{
    public partial class FolderRenameForm : Form
    {
        String Path;
        public bool IsRenamed = false;
        public FolderRenameForm(String Path,Point location)
        {
            InitializeComponent();
            Location = location;
            this.Path = Path;
            SystemService ss = ServicesManager.GetOrSet(() => { return new SystemService(); });
            if (!ss.DirectoryExist(Path))
                Close();        //TODO: da testare se è possibile fare la close nel costruttore

            textBox1.Text = ss.GetFileName(Path);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if( e.KeyCode==Keys.Enter)
            {
                //salva
                SystemService ss = ServicesManager.GetOrSet(() => { return new SystemService(); });
                String FinalPath = ss.CombinePaths(ss.GetParent(Path), textBox1.Text);
                ss.Rename(Path,FinalPath, false);
                IsRenamed = true;
                Close();
            }
            else if( e.KeyCode==Keys.Escape)
            {
                //annulla
                Close();
            }
        }
    }
}
