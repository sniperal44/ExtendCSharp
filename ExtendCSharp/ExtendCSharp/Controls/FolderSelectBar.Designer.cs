namespace ExtendCSharp.Controls
{
    partial class FolderSelectBar
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_select = new System.Windows.Forms.Button();
            this.textBoxPlus1 = new ExtendCSharp.Controls.TextBoxPlus();
            this.SuspendLayout();
            // 
            // button_select
            // 
            this.button_select.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_select.Location = new System.Drawing.Point(359, 0);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(49, 20);
            this.button_select.TabIndex = 0;
            this.button_select.Text = "Select";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // textBoxPlus1
            // 
            this.textBoxPlus1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPlus1.AutoScroll = false;
            this.textBoxPlus1.CaptionColor = System.Drawing.Color.Gray;
            this.textBoxPlus1.ForeColor = System.Drawing.Color.Gray;
            this.textBoxPlus1.Location = new System.Drawing.Point(0, 0);
            this.textBoxPlus1.Name = "textBoxPlus1";
            this.textBoxPlus1.Size = new System.Drawing.Size(353, 20);
            this.textBoxPlus1.TabIndex = 1;
            this.textBoxPlus1.TextCaption = "";
            this.textBoxPlus1.TextObject = "";
            // 
            // FolderSelectBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxPlus1);
            this.Controls.Add(this.button_select);
            this.Name = "FolderSelectBar";
            this.Size = new System.Drawing.Size(408, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_select;
        private TextBoxPlus textBoxPlus1;
    }
}
