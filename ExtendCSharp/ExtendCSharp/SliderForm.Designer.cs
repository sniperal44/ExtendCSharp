namespace ExtendCSharp
{
    partial class SliderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_right = new System.Windows.Forms.Button();
            this.button_left = new System.Windows.Forms.Button();
            this.button_top = new System.Windows.Forms.Button();
            this.button_bottom = new System.Windows.Forms.Button();
            this.Panel_Container = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button_right
            // 
            this.button_right.Location = new System.Drawing.Point(321, 316);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(75, 23);
            this.button_right.TabIndex = 0;
            this.button_right.Text = "button1";
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.Visible = false;
            // 
            // button_left
            // 
            this.button_left.Location = new System.Drawing.Point(159, 316);
            this.button_left.Name = "button_left";
            this.button_left.Size = new System.Drawing.Size(75, 23);
            this.button_left.TabIndex = 0;
            this.button_left.Text = "button1";
            this.button_left.UseVisualStyleBackColor = true;
            this.button_left.Visible = false;
            // 
            // button_top
            // 
            this.button_top.Location = new System.Drawing.Point(240, 304);
            this.button_top.Name = "button_top";
            this.button_top.Size = new System.Drawing.Size(75, 23);
            this.button_top.TabIndex = 0;
            this.button_top.Text = "button1";
            this.button_top.UseVisualStyleBackColor = true;
            this.button_top.Visible = false;
            // 
            // button_bottom
            // 
            this.button_bottom.Location = new System.Drawing.Point(240, 333);
            this.button_bottom.Name = "button_bottom";
            this.button_bottom.Size = new System.Drawing.Size(75, 23);
            this.button_bottom.TabIndex = 0;
            this.button_bottom.Text = "button1";
            this.button_bottom.UseVisualStyleBackColor = true;
            this.button_bottom.Visible = false;
            // 
            // Panel_Container
            // 
            this.Panel_Container.BackColor = System.Drawing.Color.White;
            this.Panel_Container.Location = new System.Drawing.Point(0, 0);
            this.Panel_Container.Name = "Panel_Container";
            this.Panel_Container.Size = new System.Drawing.Size(567, 251);
            this.Panel_Container.TabIndex = 1;
            // 
            // SliderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 368);
            this.Controls.Add(this.Panel_Container);
            this.Controls.Add(this.button_bottom);
            this.Controls.Add(this.button_top);
            this.Controls.Add(this.button_left);
            this.Controls.Add(this.button_right);
            this.DoubleBuffered = true;
            this.Name = "SliderForm";
            this.Text = "SliderForm";
            this.Load += new System.EventHandler(this.SliderForm_Load);
            this.SizeChanged += new System.EventHandler(this.SliderPanel_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Panel_Container;
        public System.Windows.Forms.Button button_right;
        public System.Windows.Forms.Button button_left;
        public System.Windows.Forms.Button button_top;
        public System.Windows.Forms.Button button_bottom;
    }
}