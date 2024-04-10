namespace LC_order_reconstruction_3D
{
    partial class Defect_properties
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
            this.m_l = new System.Windows.Forms.Label();
            this.m_value = new System.Windows.Forms.NumericUpDown();
            this.lega_y_l = new System.Windows.Forms.Label();
            this.lega_x_l = new System.Windows.Forms.Label();
            this.defekt_y = new System.Windows.Forms.NumericUpDown();
            this.defekt_x = new System.Windows.Forms.NumericUpDown();
            this.Potrdi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defekt_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defekt_x)).BeginInit();
            this.SuspendLayout();
            // 
            // m_l
            // 
            this.m_l.AutoSize = true;
            this.m_l.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.m_l.Location = new System.Drawing.Point(12, 46);
            this.m_l.Name = "m_l";
            this.m_l.Size = new System.Drawing.Size(22, 20);
            this.m_l.TabIndex = 102;
            this.m_l.Text = "m";
            // 
            // m_value
            // 
            this.m_value.DecimalPlaces = 1;
            this.m_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.m_value.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.m_value.Location = new System.Drawing.Point(68, 44);
            this.m_value.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.m_value.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.m_value.Name = "m_value";
            this.m_value.Size = new System.Drawing.Size(47, 26);
            this.m_value.TabIndex = 101;
            this.m_value.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lega_y_l
            // 
            this.lega_y_l.AutoSize = true;
            this.lega_y_l.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lega_y_l.Location = new System.Drawing.Point(131, 14);
            this.lega_y_l.Name = "lega_y_l";
            this.lega_y_l.Size = new System.Drawing.Size(50, 20);
            this.lega_y_l.TabIndex = 100;
            this.lega_y_l.Text = "lega y";
            // 
            // lega_x_l
            // 
            this.lega_x_l.AutoSize = true;
            this.lega_x_l.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lega_x_l.Location = new System.Drawing.Point(12, 14);
            this.lega_x_l.Name = "lega_x_l";
            this.lega_x_l.Size = new System.Drawing.Size(50, 20);
            this.lega_x_l.TabIndex = 99;
            this.lega_x_l.Text = "lega x";
            // 
            // defekt_y
            // 
            this.defekt_y.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.defekt_y.Location = new System.Drawing.Point(187, 12);
            this.defekt_y.Maximum = new decimal(new int[] {
            199,
            0,
            0,
            0});
            this.defekt_y.Name = "defekt_y";
            this.defekt_y.Size = new System.Drawing.Size(47, 26);
            this.defekt_y.TabIndex = 98;
            this.defekt_y.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // defekt_x
            // 
            this.defekt_x.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.defekt_x.Location = new System.Drawing.Point(68, 12);
            this.defekt_x.Maximum = new decimal(new int[] {
            199,
            0,
            0,
            0});
            this.defekt_x.Name = "defekt_x";
            this.defekt_x.Size = new System.Drawing.Size(47, 26);
            this.defekt_x.TabIndex = 97;
            this.defekt_x.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // Potrdi
            // 
            this.Potrdi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Potrdi.Location = new System.Drawing.Point(152, 42);
            this.Potrdi.Name = "Potrdi";
            this.Potrdi.Size = new System.Drawing.Size(82, 28);
            this.Potrdi.TabIndex = 103;
            this.Potrdi.Text = "Potrdi";
            this.Potrdi.UseVisualStyleBackColor = true;
            this.Potrdi.Click += new System.EventHandler(this.Potrdi_Click);
            // 
            // Defect_properties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 90);
            this.Controls.Add(this.Potrdi);
            this.Controls.Add(this.m_l);
            this.Controls.Add(this.m_value);
            this.Controls.Add(this.lega_y_l);
            this.Controls.Add(this.lega_x_l);
            this.Controls.Add(this.defekt_y);
            this.Controls.Add(this.defekt_x);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Defect_properties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Defect properties";
            ((System.ComponentModel.ISupportInitialize)(this.m_value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defekt_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defekt_x)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_l;
        private System.Windows.Forms.Label lega_y_l;
        private System.Windows.Forms.Label lega_x_l;
        private System.Windows.Forms.Button Potrdi;
        public System.Windows.Forms.NumericUpDown m_value;
        public System.Windows.Forms.NumericUpDown defekt_y;
        public System.Windows.Forms.NumericUpDown defekt_x;
    }
}