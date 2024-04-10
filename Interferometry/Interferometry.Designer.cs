namespace Interferometry
{
    partial class Interferometry
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
            this.Redraw = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ER = new System.Windows.Forms.RadioButton();
            this.PP = new System.Windows.Forms.RadioButton();
            this.PB = new System.Windows.Forms.RadioButton();
            this.Delitev = new System.Windows.Forms.GroupBox();
            this.delix_label = new System.Windows.Forms.Label();
            this.deliz_label = new System.Windows.Forms.Label();
            this.deliy_label = new System.Windows.Forms.Label();
            this.deliz_n = new System.Windows.Forms.NumericUpDown();
            this.delix_n = new System.Windows.Forms.NumericUpDown();
            this.deliy_n = new System.Windows.Forms.NumericUpDown();
            this.Koti_polarizatorjev = new System.Windows.Forms.GroupBox();
            this.first_label = new System.Windows.Forms.Label();
            this.second_label = new System.Windows.Forms.Label();
            this.beta1_n = new System.Windows.Forms.NumericUpDown();
            this.beta0_n = new System.Windows.Forms.NumericUpDown();
            this.n_e_label = new System.Windows.Forms.Label();
            this.n_o_label = new System.Windows.Forms.Label();
            this.n_e_n = new System.Windows.Forms.NumericUpDown();
            this.n_o_n = new System.Windows.Forms.NumericUpDown();
            this.d_label = new System.Windows.Forms.Label();
            this.lambda_label = new System.Windows.Forms.Label();
            this.d_n = new System.Windows.Forms.NumericUpDown();
            this.lambda_n = new System.Windows.Forms.NumericUpDown();
            this.Error_label = new System.Windows.Forms.Label();
            this.error_n = new System.Windows.Forms.NumericUpDown();
            this.LL_label = new System.Windows.Forms.Label();
            this.NN_label = new System.Windows.Forms.Label();
            this.MM_label = new System.Windows.Forms.Label();
            this.LL_n = new System.Windows.Forms.NumericUpDown();
            this.NN_n = new System.Windows.Forms.NumericUpDown();
            this.MM_n = new System.Windows.Forms.NumericUpDown();
            this.Start = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Input_file = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.Delitev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deliz_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delix_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deliy_n)).BeginInit();
            this.Koti_polarizatorjev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beta1_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beta0_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.n_e_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.n_o_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.d_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lambda_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LL_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NN_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MM_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Redraw
            // 
            this.Redraw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Redraw.Location = new System.Drawing.Point(12, 671);
            this.Redraw.Name = "Redraw";
            this.Redraw.Size = new System.Drawing.Size(100, 35);
            this.Redraw.TabIndex = 45;
            this.Redraw.Text = "Redraw";
            this.Redraw.UseVisualStyleBackColor = true;
            this.Redraw.Click += new System.EventHandler(this.Redraw_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Input_file);
            this.groupBox1.Controls.Add(this.ER);
            this.groupBox1.Controls.Add(this.PP);
            this.groupBox1.Controls.Add(this.PB);
            this.groupBox1.Location = new System.Drawing.Point(12, 483);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 141);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reading";
            // 
            // ER
            // 
            this.ER.AutoSize = true;
            this.ER.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ER.Location = new System.Drawing.Point(6, 111);
            this.ER.Name = "ER";
            this.ER.Size = new System.Drawing.Size(132, 24);
            this.ER.TabIndex = 2;
            this.ER.Text = "Escaped radial";
            this.ER.UseVisualStyleBackColor = true;
            // 
            // PP
            // 
            this.PP.AutoSize = true;
            this.PP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PP.Location = new System.Drawing.Point(6, 81);
            this.PP.Name = "PP";
            this.PP.Size = new System.Drawing.Size(111, 24);
            this.PP.TabIndex = 1;
            this.PP.Text = "Planar polar";
            this.PP.UseVisualStyleBackColor = true;
            // 
            // PB
            // 
            this.PB.AutoSize = true;
            this.PB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PB.Location = new System.Drawing.Point(6, 51);
            this.PB.Name = "PB";
            this.PB.Size = new System.Drawing.Size(123, 24);
            this.PB.TabIndex = 0;
            this.PB.Text = "Planar bipolar";
            this.PB.UseVisualStyleBackColor = true;
            // 
            // Delitev
            // 
            this.Delitev.Controls.Add(this.delix_label);
            this.Delitev.Controls.Add(this.deliz_label);
            this.Delitev.Controls.Add(this.deliy_label);
            this.Delitev.Controls.Add(this.deliz_n);
            this.Delitev.Controls.Add(this.delix_n);
            this.Delitev.Controls.Add(this.deliy_n);
            this.Delitev.Location = new System.Drawing.Point(12, 360);
            this.Delitev.Name = "Delitev";
            this.Delitev.Size = new System.Drawing.Size(111, 117);
            this.Delitev.TabIndex = 43;
            this.Delitev.TabStop = false;
            this.Delitev.Text = "Interpolation points";
            // 
            // delix_label
            // 
            this.delix_label.AutoSize = true;
            this.delix_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.delix_label.Location = new System.Drawing.Point(73, 21);
            this.delix_label.Name = "delix_label";
            this.delix_label.Size = new System.Drawing.Size(16, 20);
            this.delix_label.TabIndex = 22;
            this.delix_label.Text = "x";
            // 
            // deliz_label
            // 
            this.deliz_label.AutoSize = true;
            this.deliz_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deliz_label.Location = new System.Drawing.Point(73, 85);
            this.deliz_label.Name = "deliz_label";
            this.deliz_label.Size = new System.Drawing.Size(17, 20);
            this.deliz_label.TabIndex = 24;
            this.deliz_label.Text = "z";
            // 
            // deliy_label
            // 
            this.deliy_label.AutoSize = true;
            this.deliy_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deliy_label.Location = new System.Drawing.Point(73, 53);
            this.deliy_label.Name = "deliy_label";
            this.deliy_label.Size = new System.Drawing.Size(16, 20);
            this.deliy_label.TabIndex = 26;
            this.deliy_label.Text = "y";
            // 
            // deliz_n
            // 
            this.deliz_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deliz_n.Location = new System.Drawing.Point(6, 83);
            this.deliz_n.Name = "deliz_n";
            this.deliz_n.Size = new System.Drawing.Size(61, 26);
            this.deliz_n.TabIndex = 23;
            this.deliz_n.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // delix_n
            // 
            this.delix_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.delix_n.Location = new System.Drawing.Point(6, 19);
            this.delix_n.Name = "delix_n";
            this.delix_n.Size = new System.Drawing.Size(61, 26);
            this.delix_n.TabIndex = 21;
            this.delix_n.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // deliy_n
            // 
            this.deliy_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deliy_n.Location = new System.Drawing.Point(6, 51);
            this.deliy_n.Name = "deliy_n";
            this.deliy_n.Size = new System.Drawing.Size(61, 26);
            this.deliy_n.TabIndex = 25;
            this.deliy_n.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // Koti_polarizatorjev
            // 
            this.Koti_polarizatorjev.Controls.Add(this.first_label);
            this.Koti_polarizatorjev.Controls.Add(this.second_label);
            this.Koti_polarizatorjev.Controls.Add(this.beta1_n);
            this.Koti_polarizatorjev.Controls.Add(this.beta0_n);
            this.Koti_polarizatorjev.Location = new System.Drawing.Point(12, 268);
            this.Koti_polarizatorjev.Name = "Koti_polarizatorjev";
            this.Koti_polarizatorjev.Size = new System.Drawing.Size(111, 86);
            this.Koti_polarizatorjev.TabIndex = 42;
            this.Koti_polarizatorjev.TabStop = false;
            this.Koti_polarizatorjev.Text = "Polarizer angles";
            // 
            // first_label
            // 
            this.first_label.AutoSize = true;
            this.first_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.first_label.Location = new System.Drawing.Point(73, 21);
            this.first_label.Name = "first_label";
            this.first_label.Size = new System.Drawing.Size(18, 20);
            this.first_label.TabIndex = 19;
            this.first_label.Text = "1";
            // 
            // second_label
            // 
            this.second_label.AutoSize = true;
            this.second_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.second_label.Location = new System.Drawing.Point(73, 53);
            this.second_label.Name = "second_label";
            this.second_label.Size = new System.Drawing.Size(18, 20);
            this.second_label.TabIndex = 20;
            this.second_label.Text = "2";
            // 
            // beta1_n
            // 
            this.beta1_n.DecimalPlaces = 1;
            this.beta1_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.beta1_n.Location = new System.Drawing.Point(6, 51);
            this.beta1_n.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.beta1_n.Name = "beta1_n";
            this.beta1_n.Size = new System.Drawing.Size(61, 26);
            this.beta1_n.TabIndex = 14;
            this.beta1_n.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // beta0_n
            // 
            this.beta0_n.DecimalPlaces = 1;
            this.beta0_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.beta0_n.Location = new System.Drawing.Point(6, 19);
            this.beta0_n.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.beta0_n.Name = "beta0_n";
            this.beta0_n.Size = new System.Drawing.Size(61, 26);
            this.beta0_n.TabIndex = 13;
            // 
            // n_e_label
            // 
            this.n_e_label.AutoSize = true;
            this.n_e_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.n_e_label.Location = new System.Drawing.Point(79, 174);
            this.n_e_label.Name = "n_e_label";
            this.n_e_label.Size = new System.Drawing.Size(36, 20);
            this.n_e_label.TabIndex = 41;
            this.n_e_label.Text = "n_e";
            // 
            // n_o_label
            // 
            this.n_o_label.AutoSize = true;
            this.n_o_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.n_o_label.Location = new System.Drawing.Point(79, 142);
            this.n_o_label.Name = "n_o_label";
            this.n_o_label.Size = new System.Drawing.Size(36, 20);
            this.n_o_label.TabIndex = 40;
            this.n_o_label.Text = "n_o";
            // 
            // n_e_n
            // 
            this.n_e_n.DecimalPlaces = 1;
            this.n_e_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.n_e_n.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.n_e_n.Location = new System.Drawing.Point(12, 172);
            this.n_e_n.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.n_e_n.Name = "n_e_n";
            this.n_e_n.Size = new System.Drawing.Size(61, 26);
            this.n_e_n.TabIndex = 39;
            this.n_e_n.Value = new decimal(new int[] {
            18,
            0,
            0,
            65536});
            // 
            // n_o_n
            // 
            this.n_o_n.DecimalPlaces = 1;
            this.n_o_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.n_o_n.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.n_o_n.Location = new System.Drawing.Point(12, 140);
            this.n_o_n.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.n_o_n.Name = "n_o_n";
            this.n_o_n.Size = new System.Drawing.Size(61, 26);
            this.n_o_n.TabIndex = 38;
            this.n_o_n.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // d_label
            // 
            this.d_label.AutoSize = true;
            this.d_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.d_label.Location = new System.Drawing.Point(85, 238);
            this.d_label.Name = "d_label";
            this.d_label.Size = new System.Drawing.Size(18, 20);
            this.d_label.TabIndex = 37;
            this.d_label.Text = "d";
            // 
            // lambda_label
            // 
            this.lambda_label.AutoSize = true;
            this.lambda_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lambda_label.Location = new System.Drawing.Point(79, 206);
            this.lambda_label.Name = "lambda_label";
            this.lambda_label.Size = new System.Drawing.Size(61, 20);
            this.lambda_label.TabIndex = 36;
            this.lambda_label.Text = "lambda";
            // 
            // d_n
            // 
            this.d_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.d_n.Location = new System.Drawing.Point(12, 236);
            this.d_n.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.d_n.Name = "d_n";
            this.d_n.Size = new System.Drawing.Size(67, 26);
            this.d_n.TabIndex = 35;
            this.d_n.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // lambda_n
            // 
            this.lambda_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lambda_n.Location = new System.Drawing.Point(12, 204);
            this.lambda_n.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.lambda_n.Name = "lambda_n";
            this.lambda_n.Size = new System.Drawing.Size(61, 26);
            this.lambda_n.TabIndex = 34;
            this.lambda_n.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // Error_label
            // 
            this.Error_label.AutoSize = true;
            this.Error_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Error_label.Location = new System.Drawing.Point(98, 110);
            this.Error_label.Name = "Error_label";
            this.Error_label.Size = new System.Drawing.Size(61, 20);
            this.Error_label.TabIndex = 33;
            this.Error_label.Text = "Epsilon";
            // 
            // error_n
            // 
            this.error_n.DecimalPlaces = 5;
            this.error_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.error_n.Increment = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.error_n.Location = new System.Drawing.Point(12, 108);
            this.error_n.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.error_n.Name = "error_n";
            this.error_n.Size = new System.Drawing.Size(80, 26);
            this.error_n.TabIndex = 32;
            this.error_n.Value = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            // 
            // LL_label
            // 
            this.LL_label.AutoSize = true;
            this.LL_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LL_label.Location = new System.Drawing.Point(79, 78);
            this.LL_label.Name = "LL_label";
            this.LL_label.Size = new System.Drawing.Size(27, 20);
            this.LL_label.TabIndex = 31;
            this.LL_label.Text = "LL";
            // 
            // NN_label
            // 
            this.NN_label.AutoSize = true;
            this.NN_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.NN_label.Location = new System.Drawing.Point(79, 46);
            this.NN_label.Name = "NN_label";
            this.NN_label.Size = new System.Drawing.Size(31, 20);
            this.NN_label.TabIndex = 30;
            this.NN_label.Text = "NN";
            // 
            // MM_label
            // 
            this.MM_label.AutoSize = true;
            this.MM_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MM_label.Location = new System.Drawing.Point(79, 14);
            this.MM_label.Name = "MM_label";
            this.MM_label.Size = new System.Drawing.Size(35, 20);
            this.MM_label.TabIndex = 29;
            this.MM_label.Text = "MM";
            // 
            // LL_n
            // 
            this.LL_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LL_n.Location = new System.Drawing.Point(12, 76);
            this.LL_n.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.LL_n.Name = "LL_n";
            this.LL_n.Size = new System.Drawing.Size(61, 26);
            this.LL_n.TabIndex = 28;
            this.LL_n.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // NN_n
            // 
            this.NN_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.NN_n.Location = new System.Drawing.Point(12, 44);
            this.NN_n.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NN_n.Name = "NN_n";
            this.NN_n.Size = new System.Drawing.Size(61, 26);
            this.NN_n.TabIndex = 27;
            this.NN_n.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // MM_n
            // 
            this.MM_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MM_n.Location = new System.Drawing.Point(12, 12);
            this.MM_n.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.MM_n.Name = "MM_n";
            this.MM_n.Size = new System.Drawing.Size(61, 26);
            this.MM_n.TabIndex = 26;
            this.MM_n.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // Start
            // 
            this.Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Start.Location = new System.Drawing.Point(12, 630);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(100, 35);
            this.Start.TabIndex = 25;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(178, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 800);
            this.pictureBox1.TabIndex = 46;
            this.pictureBox1.TabStop = false;
            // 
            // Input_file
            // 
            this.Input_file.AutoSize = true;
            this.Input_file.Checked = true;
            this.Input_file.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Input_file.Location = new System.Drawing.Point(6, 19);
            this.Input_file.Name = "Input_file";
            this.Input_file.Size = new System.Drawing.Size(88, 24);
            this.Input_file.TabIndex = 3;
            this.Input_file.TabStop = true;
            this.Input_file.Text = "Input file";
            this.Input_file.UseVisualStyleBackColor = true;
            // 
            // Interferometry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 821);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Redraw);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Delitev);
            this.Controls.Add(this.Koti_polarizatorjev);
            this.Controls.Add(this.n_e_label);
            this.Controls.Add(this.n_o_label);
            this.Controls.Add(this.n_e_n);
            this.Controls.Add(this.n_o_n);
            this.Controls.Add(this.d_label);
            this.Controls.Add(this.lambda_label);
            this.Controls.Add(this.d_n);
            this.Controls.Add(this.lambda_n);
            this.Controls.Add(this.Error_label);
            this.Controls.Add(this.error_n);
            this.Controls.Add(this.LL_label);
            this.Controls.Add(this.NN_label);
            this.Controls.Add(this.MM_label);
            this.Controls.Add(this.LL_n);
            this.Controls.Add(this.NN_n);
            this.Controls.Add(this.MM_n);
            this.Controls.Add(this.Start);
            this.Name = "Interferometry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Interferometry";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Delitev.ResumeLayout(false);
            this.Delitev.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deliz_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delix_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deliy_n)).EndInit();
            this.Koti_polarizatorjev.ResumeLayout(false);
            this.Koti_polarizatorjev.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beta1_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beta0_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.n_e_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.n_o_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.d_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lambda_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LL_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NN_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MM_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Redraw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton PP;
        private System.Windows.Forms.RadioButton PB;
        private System.Windows.Forms.GroupBox Delitev;
        private System.Windows.Forms.Label delix_label;
        private System.Windows.Forms.Label deliz_label;
        private System.Windows.Forms.Label deliy_label;
        private System.Windows.Forms.NumericUpDown deliz_n;
        private System.Windows.Forms.NumericUpDown delix_n;
        private System.Windows.Forms.NumericUpDown deliy_n;
        private System.Windows.Forms.GroupBox Koti_polarizatorjev;
        private System.Windows.Forms.Label first_label;
        private System.Windows.Forms.Label second_label;
        private System.Windows.Forms.NumericUpDown beta1_n;
        private System.Windows.Forms.NumericUpDown beta0_n;
        private System.Windows.Forms.Label n_e_label;
        private System.Windows.Forms.Label n_o_label;
        private System.Windows.Forms.NumericUpDown n_e_n;
        private System.Windows.Forms.NumericUpDown n_o_n;
        private System.Windows.Forms.Label d_label;
        private System.Windows.Forms.Label lambda_label;
        private System.Windows.Forms.NumericUpDown d_n;
        private System.Windows.Forms.NumericUpDown lambda_n;
        private System.Windows.Forms.Label Error_label;
        private System.Windows.Forms.NumericUpDown error_n;
        private System.Windows.Forms.Label LL_label;
        private System.Windows.Forms.Label NN_label;
        private System.Windows.Forms.Label MM_label;
        private System.Windows.Forms.NumericUpDown LL_n;
        private System.Windows.Forms.NumericUpDown NN_n;
        private System.Windows.Forms.NumericUpDown MM_n;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton ER;
        private System.Windows.Forms.RadioButton Input_file;
    }
}

