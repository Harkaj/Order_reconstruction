namespace LC_order_reconstruction_3D
{
    partial class Input_selection
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
            this.Tensor = new System.Windows.Forms.RadioButton();
            this.Director = new System.Windows.Forms.RadioButton();
            this.Confirm = new System.Windows.Forms.Button();
            this.Use_different_boundary = new System.Windows.Forms.CheckBox();
            this.Reset_tensor = new System.Windows.Forms.CheckBox();
            this.Interpolation = new System.Windows.Forms.CheckBox();
            this.Factor_n = new System.Windows.Forms.NumericUpDown();
            this.Factor_label = new System.Windows.Forms.Label();
            this.Input_file_group = new System.Windows.Forms.GroupBox();
            this.Changing_parameter_group = new System.Windows.Forms.GroupBox();
            this.Change_boundary = new System.Windows.Forms.RadioButton();
            this.E_field = new System.Windows.Forms.RadioButton();
            this.a = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.Factor_n)).BeginInit();
            this.Input_file_group.SuspendLayout();
            this.Changing_parameter_group.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tensor
            // 
            this.Tensor.AutoSize = true;
            this.Tensor.Checked = true;
            this.Tensor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tensor.Location = new System.Drawing.Point(6, 19);
            this.Tensor.Name = "Tensor";
            this.Tensor.Size = new System.Drawing.Size(109, 24);
            this.Tensor.TabIndex = 0;
            this.Tensor.TabStop = true;
            this.Tensor.Text = "Tensor field";
            this.Tensor.UseVisualStyleBackColor = true;
            // 
            // Director
            // 
            this.Director.AutoSize = true;
            this.Director.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Director.Location = new System.Drawing.Point(6, 49);
            this.Director.Name = "Director";
            this.Director.Size = new System.Drawing.Size(116, 24);
            this.Director.TabIndex = 1;
            this.Director.Text = "Director field";
            this.Director.UseVisualStyleBackColor = true;
            // 
            // Confirm
            // 
            this.Confirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Confirm.Location = new System.Drawing.Point(12, 226);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(245, 35);
            this.Confirm.TabIndex = 2;
            this.Confirm.Text = "Confirm";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Use_different_boundary
            // 
            this.Use_different_boundary.AutoSize = true;
            this.Use_different_boundary.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Use_different_boundary.Location = new System.Drawing.Point(12, 136);
            this.Use_different_boundary.Name = "Use_different_boundary";
            this.Use_different_boundary.Size = new System.Drawing.Size(190, 24);
            this.Use_different_boundary.TabIndex = 3;
            this.Use_different_boundary.Text = "Use different boundary";
            this.Use_different_boundary.UseVisualStyleBackColor = true;
            // 
            // Reset_tensor
            // 
            this.Reset_tensor.AutoSize = true;
            this.Reset_tensor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Reset_tensor.Location = new System.Drawing.Point(12, 196);
            this.Reset_tensor.Name = "Reset_tensor";
            this.Reset_tensor.Size = new System.Drawing.Size(136, 24);
            this.Reset_tensor.TabIndex = 4;
            this.Reset_tensor.Text = "Reset Q tensor";
            this.Reset_tensor.UseVisualStyleBackColor = true;
            // 
            // Interpolation
            // 
            this.Interpolation.AutoSize = true;
            this.Interpolation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Interpolation.Location = new System.Drawing.Point(12, 166);
            this.Interpolation.Name = "Interpolation";
            this.Interpolation.Size = new System.Drawing.Size(117, 24);
            this.Interpolation.TabIndex = 5;
            this.Interpolation.Text = "Interpolation";
            this.Interpolation.UseVisualStyleBackColor = true;
            this.Interpolation.CheckedChanged += new System.EventHandler(this.Interpolation_CheckedChanged);
            // 
            // Factor_n
            // 
            this.Factor_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Factor_n.Location = new System.Drawing.Point(217, 165);
            this.Factor_n.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Factor_n.Name = "Factor_n";
            this.Factor_n.Size = new System.Drawing.Size(40, 26);
            this.Factor_n.TabIndex = 53;
            this.Factor_n.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.Factor_n.Visible = false;
            // 
            // Factor_label
            // 
            this.Factor_label.AutoSize = true;
            this.Factor_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Factor_label.Location = new System.Drawing.Point(156, 167);
            this.Factor_label.Name = "Factor_label";
            this.Factor_label.Size = new System.Drawing.Size(55, 20);
            this.Factor_label.TabIndex = 63;
            this.Factor_label.Text = "Factor";
            this.Factor_label.Visible = false;
            // 
            // Input_file_group
            // 
            this.Input_file_group.Controls.Add(this.Tensor);
            this.Input_file_group.Controls.Add(this.Director);
            this.Input_file_group.Location = new System.Drawing.Point(12, 12);
            this.Input_file_group.Name = "Input_file_group";
            this.Input_file_group.Size = new System.Drawing.Size(128, 78);
            this.Input_file_group.TabIndex = 64;
            this.Input_file_group.TabStop = false;
            this.Input_file_group.Text = "Input file";
            // 
            // Changing_parameter_group
            // 
            this.Changing_parameter_group.Controls.Add(this.Change_boundary);
            this.Changing_parameter_group.Controls.Add(this.E_field);
            this.Changing_parameter_group.Controls.Add(this.a);
            this.Changing_parameter_group.Location = new System.Drawing.Point(140, 12);
            this.Changing_parameter_group.Name = "Changing_parameter_group";
            this.Changing_parameter_group.Size = new System.Drawing.Size(122, 110);
            this.Changing_parameter_group.TabIndex = 65;
            this.Changing_parameter_group.TabStop = false;
            this.Changing_parameter_group.Text = "Changing parameter";
            // 
            // Change_boundary
            // 
            this.Change_boundary.AutoSize = true;
            this.Change_boundary.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Change_boundary.Location = new System.Drawing.Point(6, 79);
            this.Change_boundary.Name = "Change_boundary";
            this.Change_boundary.Size = new System.Drawing.Size(95, 24);
            this.Change_boundary.TabIndex = 2;
            this.Change_boundary.Text = "Boundary";
            this.Change_boundary.UseVisualStyleBackColor = true;
            // 
            // E_field
            // 
            this.E_field.AutoSize = true;
            this.E_field.Checked = true;
            this.E_field.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.E_field.Location = new System.Drawing.Point(6, 19);
            this.E_field.Name = "E_field";
            this.E_field.Size = new System.Drawing.Size(112, 24);
            this.E_field.TabIndex = 0;
            this.E_field.TabStop = true;
            this.E_field.Text = "Electric field";
            this.E_field.UseVisualStyleBackColor = true;
            // 
            // a
            // 
            this.a.AutoSize = true;
            this.a.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.a.Location = new System.Drawing.Point(4, 49);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(112, 24);
            this.a.TabIndex = 1;
            this.a.Text = "System size";
            this.a.UseVisualStyleBackColor = true;
            // 
            // Input_selection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 273);
            this.Controls.Add(this.Changing_parameter_group);
            this.Controls.Add(this.Input_file_group);
            this.Controls.Add(this.Factor_label);
            this.Controls.Add(this.Factor_n);
            this.Controls.Add(this.Interpolation);
            this.Controls.Add(this.Reset_tensor);
            this.Controls.Add(this.Use_different_boundary);
            this.Controls.Add(this.Confirm);
            this.Name = "Input_selection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input selection";
            ((System.ComponentModel.ISupportInitialize)(this.Factor_n)).EndInit();
            this.Input_file_group.ResumeLayout(false);
            this.Input_file_group.PerformLayout();
            this.Changing_parameter_group.ResumeLayout(false);
            this.Changing_parameter_group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.RadioButton Tensor;
        private System.Windows.Forms.RadioButton Director;
        private System.Windows.Forms.CheckBox Use_different_boundary;
        private System.Windows.Forms.CheckBox Reset_tensor;
        private System.Windows.Forms.CheckBox Interpolation;
        private System.Windows.Forms.NumericUpDown Factor_n;
        private System.Windows.Forms.Label Factor_label;
        private System.Windows.Forms.GroupBox Input_file_group;
        private System.Windows.Forms.GroupBox Changing_parameter_group;
        private System.Windows.Forms.RadioButton E_field;
        private System.Windows.Forms.RadioButton a;
        private System.Windows.Forms.RadioButton Change_boundary;
    }
}