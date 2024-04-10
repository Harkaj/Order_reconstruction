namespace LC_order_reconstruction_3D
{
    partial class Mode_selection
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
            this.Standard = new System.Windows.Forms.Button();
            this.Continue = new System.Windows.Forms.Button();
            this.Interferometry = new System.Windows.Forms.Button();
            this.Determine_director_field = new System.Windows.Forms.Button();
            this.Field_known = new System.Windows.Forms.Button();
            this.POV_output = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Standard
            // 
            this.Standard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Standard.Location = new System.Drawing.Point(12, 12);
            this.Standard.Name = "Standard";
            this.Standard.Size = new System.Drawing.Size(243, 30);
            this.Standard.TabIndex = 48;
            this.Standard.Text = "Start from setup parameters";
            this.Standard.UseVisualStyleBackColor = true;
            this.Standard.Click += new System.EventHandler(this.Standard_Click);
            // 
            // Continue
            // 
            this.Continue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Continue.Location = new System.Drawing.Point(12, 48);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(243, 30);
            this.Continue.TabIndex = 49;
            this.Continue.Text = "Continue from input file";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // Interferometry
            // 
            this.Interferometry.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Interferometry.Location = new System.Drawing.Point(12, 131);
            this.Interferometry.Name = "Interferometry";
            this.Interferometry.Size = new System.Drawing.Size(243, 30);
            this.Interferometry.TabIndex = 50;
            this.Interferometry.Text = "Interferometry";
            this.Interferometry.UseVisualStyleBackColor = true;
            this.Interferometry.Click += new System.EventHandler(this.Interferometry_Click);
            // 
            // Determine_director_field
            // 
            this.Determine_director_field.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Determine_director_field.Location = new System.Drawing.Point(12, 95);
            this.Determine_director_field.Name = "Determine_director_field";
            this.Determine_director_field.Size = new System.Drawing.Size(243, 30);
            this.Determine_director_field.TabIndex = 51;
            this.Determine_director_field.Text = "Determine n field";
            this.Determine_director_field.UseVisualStyleBackColor = true;
            this.Determine_director_field.Click += new System.EventHandler(this.Determine_director_field_Click);
            // 
            // Field_known
            // 
            this.Field_known.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Field_known.Location = new System.Drawing.Point(12, 179);
            this.Field_known.Name = "Field_known";
            this.Field_known.Size = new System.Drawing.Size(242, 30);
            this.Field_known.TabIndex = 52;
            this.Field_known.Text = "Field known";
            this.Field_known.UseVisualStyleBackColor = true;
            this.Field_known.Click += new System.EventHandler(this.Field_known_Click);
            // 
            // POV_output
            // 
            this.POV_output.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.POV_output.Location = new System.Drawing.Point(12, 215);
            this.POV_output.Name = "POV_output";
            this.POV_output.Size = new System.Drawing.Size(242, 30);
            this.POV_output.TabIndex = 53;
            this.POV_output.Text = "POV output";
            this.POV_output.UseVisualStyleBackColor = true;
            this.POV_output.Click += new System.EventHandler(this.POV_output_Click);
            // 
            // Mode_selection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 265);
            this.Controls.Add(this.POV_output);
            this.Controls.Add(this.Field_known);
            this.Controls.Add(this.Determine_director_field);
            this.Controls.Add(this.Interferometry);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Standard);
            this.Name = "Mode_selection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mode selection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Standard;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Button Interferometry;
        private System.Windows.Forms.Button Determine_director_field;
        private System.Windows.Forms.Button Field_known;
        private System.Windows.Forms.Button POV_output;
    }
}