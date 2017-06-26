namespace ExtendCSharp.Controls
{
    partial class ButtonMenu
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
            this.button_int = new System.Windows.Forms.Button();
            this.button_arrow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_int
            // 
            this.button_int.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_int.Location = new System.Drawing.Point(0, 0);
            this.button_int.Name = "button_int";
            this.button_int.Size = new System.Drawing.Size(72, 48);
            this.button_int.TabIndex = 0;
            this.button_int.Text = "button";
            this.button_int.UseVisualStyleBackColor = true;
            // 
            // button_arrow
            // 
            this.button_arrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_arrow.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_arrow.Location = new System.Drawing.Point(71, 0);
            this.button_arrow.Name = "button_arrow";
            this.button_arrow.Size = new System.Drawing.Size(21, 48);
            this.button_arrow.TabIndex = 1;
            this.button_arrow.Text = "▼";
            this.button_arrow.UseVisualStyleBackColor = true;
            this.button_arrow.Click += new System.EventHandler(this.button2_Click);
            // 
            // ButtonMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_arrow);
            this.Controls.Add(this.button_int);
            this.Name = "ButtonMenu";
            this.Size = new System.Drawing.Size(92, 48);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_arrow;
        private System.Windows.Forms.Button button_int;
    }
}
