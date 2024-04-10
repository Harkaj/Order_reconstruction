namespace LC_order_reconstruction
{
    partial class Form1
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
            this.Start = new System.Windows.Forms.Button();
            this.ipisi_n = new System.Windows.Forms.NumericUpDown();
            this.eps_n = new System.Windows.Forms.NumericUpDown();
            this.itmax_n = new System.Windows.Forms.NumericUpDown();
            this.kor_n = new System.Windows.Forms.NumericUpDown();
            this.Nr_n = new System.Windows.Forms.NumericUpDown();
            this.Nz_n = new System.Windows.Forms.NumericUpDown();
            this.Rmi_n = new System.Windows.Forms.NumericUpDown();
            this.ipisi_label = new System.Windows.Forms.Label();
            this.eps_label = new System.Windows.Forms.Label();
            this.itmax_label = new System.Windows.Forms.Label();
            this.kor_label = new System.Windows.Forms.Label();
            this.Nr_label = new System.Windows.Forms.Label();
            this.Nz_label = new System.Windows.Forms.Label();
            this.Rmi_label = new System.Windows.Forms.Label();
            this.Rma_label = new System.Windows.Forms.Label();
            this.LineDef = new System.Windows.Forms.RadioButton();
            this.fi_z_konst = new System.Windows.Forms.RadioButton();
            this.beri = new System.Windows.Forms.RadioButton();
            this.melt = new System.Windows.Forms.RadioButton();
            this.ApalaD1 = new System.Windows.Forms.RadioButton();
            this.Mode = new System.Windows.Forms.GroupBox();
            this.parametrizacija = new System.Windows.Forms.RadioButton();
            this.Rma_n = new System.Windows.Forms.NumericUpDown();
            this.Parametrizacija_group = new System.Windows.Forms.GroupBox();
            this.fizika = new System.Windows.Forms.RadioButton();
            this.BB_n = new System.Windows.Forms.NumericUpDown();
            this.w_n = new System.Windows.Forms.NumericUpDown();
            this.t_n = new System.Windows.Forms.NumericUpDown();
            this.H_ksi_n = new System.Windows.Forms.NumericUpDown();
            this.BB_label = new System.Windows.Forms.Label();
            this.w_label = new System.Windows.Forms.Label();
            this.lt_label = new System.Windows.Forms.Label();
            this.H_ksi_label = new System.Windows.Forms.Label();
            this.R0_rob_iteriras = new System.Windows.Forms.CheckBox();
            this.RN_rob_iteriras = new System.Windows.Forms.CheckBox();
            this.Zrob = new System.Windows.Forms.CheckBox();
            this.homogeni_x_robovi = new System.Windows.Forms.CheckBox();
            this.izpis_energij = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ipisi_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eps_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itmax_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kor_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nr_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nz_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rmi_n)).BeginInit();
            this.Mode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rma_n)).BeginInit();
            this.Parametrizacija_group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BB_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.t_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.H_ksi_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Start.Location = new System.Drawing.Point(195, 527);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(82, 42);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // ipisi_n
            // 
            this.ipisi_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ipisi_n.Location = new System.Drawing.Point(12, 12);
            this.ipisi_n.Name = "ipisi_n";
            this.ipisi_n.Size = new System.Drawing.Size(72, 26);
            this.ipisi_n.TabIndex = 1;
            // 
            // eps_n
            // 
            this.eps_n.DecimalPlaces = 6;
            this.eps_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.eps_n.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.eps_n.Location = new System.Drawing.Point(151, 12);
            this.eps_n.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.eps_n.Name = "eps_n";
            this.eps_n.Size = new System.Drawing.Size(92, 26);
            this.eps_n.TabIndex = 2;
            this.eps_n.Value = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            // 
            // itmax_n
            // 
            this.itmax_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.itmax_n.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.itmax_n.Location = new System.Drawing.Point(296, 12);
            this.itmax_n.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.itmax_n.Name = "itmax_n";
            this.itmax_n.Size = new System.Drawing.Size(72, 26);
            this.itmax_n.TabIndex = 3;
            this.itmax_n.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // kor_n
            // 
            this.kor_n.DecimalPlaces = 1;
            this.kor_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.kor_n.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.kor_n.Location = new System.Drawing.Point(456, 12);
            this.kor_n.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.kor_n.Name = "kor_n";
            this.kor_n.Size = new System.Drawing.Size(72, 26);
            this.kor_n.TabIndex = 4;
            this.kor_n.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // Nr_n
            // 
            this.Nr_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Nr_n.Location = new System.Drawing.Point(12, 44);
            this.Nr_n.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.Nr_n.Name = "Nr_n";
            this.Nr_n.Size = new System.Drawing.Size(72, 26);
            this.Nr_n.TabIndex = 5;
            this.Nr_n.Value = new decimal(new int[] {
            201,
            0,
            0,
            0});
            // 
            // Nz_n
            // 
            this.Nz_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Nz_n.Location = new System.Drawing.Point(151, 44);
            this.Nz_n.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.Nz_n.Name = "Nz_n";
            this.Nz_n.Size = new System.Drawing.Size(72, 26);
            this.Nz_n.TabIndex = 6;
            this.Nz_n.Value = new decimal(new int[] {
            201,
            0,
            0,
            0});
            // 
            // Rmi_n
            // 
            this.Rmi_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Rmi_n.Location = new System.Drawing.Point(296, 44);
            this.Rmi_n.Name = "Rmi_n";
            this.Rmi_n.Size = new System.Drawing.Size(72, 26);
            this.Rmi_n.TabIndex = 7;
            // 
            // ipisi_label
            // 
            this.ipisi_label.AutoSize = true;
            this.ipisi_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ipisi_label.Location = new System.Drawing.Point(90, 14);
            this.ipisi_label.Name = "ipisi_label";
            this.ipisi_label.Size = new System.Drawing.Size(35, 20);
            this.ipisi_label.TabIndex = 8;
            this.ipisi_label.Text = "ipisi";
            // 
            // eps_label
            // 
            this.eps_label.AutoSize = true;
            this.eps_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.eps_label.Location = new System.Drawing.Point(249, 14);
            this.eps_label.Name = "eps_label";
            this.eps_label.Size = new System.Drawing.Size(35, 20);
            this.eps_label.TabIndex = 9;
            this.eps_label.Text = "eps";
            // 
            // itmax_label
            // 
            this.itmax_label.AutoSize = true;
            this.itmax_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.itmax_label.Location = new System.Drawing.Point(374, 14);
            this.itmax_label.Name = "itmax_label";
            this.itmax_label.Size = new System.Drawing.Size(46, 20);
            this.itmax_label.TabIndex = 10;
            this.itmax_label.Text = "itmax";
            // 
            // kor_label
            // 
            this.kor_label.AutoSize = true;
            this.kor_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.kor_label.Location = new System.Drawing.Point(534, 14);
            this.kor_label.Name = "kor_label";
            this.kor_label.Size = new System.Drawing.Size(31, 20);
            this.kor_label.TabIndex = 11;
            this.kor_label.Text = "kor";
            // 
            // Nr_label
            // 
            this.Nr_label.AutoSize = true;
            this.Nr_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Nr_label.Location = new System.Drawing.Point(90, 46);
            this.Nr_label.Name = "Nr_label";
            this.Nr_label.Size = new System.Drawing.Size(25, 20);
            this.Nr_label.TabIndex = 12;
            this.Nr_label.Text = "Nr";
            // 
            // Nz_label
            // 
            this.Nz_label.AutoSize = true;
            this.Nz_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Nz_label.Location = new System.Drawing.Point(229, 46);
            this.Nz_label.Name = "Nz_label";
            this.Nz_label.Size = new System.Drawing.Size(28, 20);
            this.Nz_label.TabIndex = 13;
            this.Nz_label.Text = "Nz";
            // 
            // Rmi_label
            // 
            this.Rmi_label.AutoSize = true;
            this.Rmi_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Rmi_label.Location = new System.Drawing.Point(374, 48);
            this.Rmi_label.Name = "Rmi_label";
            this.Rmi_label.Size = new System.Drawing.Size(37, 20);
            this.Rmi_label.TabIndex = 14;
            this.Rmi_label.Text = "Rmi";
            // 
            // Rma_label
            // 
            this.Rma_label.AutoSize = true;
            this.Rma_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Rma_label.Location = new System.Drawing.Point(534, 48);
            this.Rma_label.Name = "Rma_label";
            this.Rma_label.Size = new System.Drawing.Size(43, 20);
            this.Rma_label.TabIndex = 15;
            this.Rma_label.Text = "Rma";
            // 
            // LineDef
            // 
            this.LineDef.AutoSize = true;
            this.LineDef.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LineDef.Location = new System.Drawing.Point(3, 19);
            this.LineDef.Name = "LineDef";
            this.LineDef.Size = new System.Drawing.Size(83, 24);
            this.LineDef.TabIndex = 16;
            this.LineDef.Text = "LineDef";
            this.LineDef.UseVisualStyleBackColor = true;
            // 
            // fi_z_konst
            // 
            this.fi_z_konst.AutoSize = true;
            this.fi_z_konst.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fi_z_konst.Location = new System.Drawing.Point(92, 19);
            this.fi_z_konst.Name = "fi_z_konst";
            this.fi_z_konst.Size = new System.Drawing.Size(97, 24);
            this.fi_z_konst.TabIndex = 17;
            this.fi_z_konst.Text = "fi=z*konst";
            this.fi_z_konst.UseVisualStyleBackColor = true;
            // 
            // beri
            // 
            this.beri.AutoSize = true;
            this.beri.Checked = true;
            this.beri.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.beri.Location = new System.Drawing.Point(290, 19);
            this.beri.Name = "beri";
            this.beri.Size = new System.Drawing.Size(53, 24);
            this.beri.TabIndex = 18;
            this.beri.TabStop = true;
            this.beri.Text = "beri";
            this.beri.UseVisualStyleBackColor = true;
            // 
            // melt
            // 
            this.melt.AutoSize = true;
            this.melt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.melt.Location = new System.Drawing.Point(349, 19);
            this.melt.Name = "melt";
            this.melt.Size = new System.Drawing.Size(57, 24);
            this.melt.TabIndex = 19;
            this.melt.Text = "melt";
            this.melt.UseVisualStyleBackColor = true;
            // 
            // ApalaD1
            // 
            this.ApalaD1.AutoSize = true;
            this.ApalaD1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ApalaD1.Location = new System.Drawing.Point(195, 19);
            this.ApalaD1.Name = "ApalaD1";
            this.ApalaD1.Size = new System.Drawing.Size(89, 24);
            this.ApalaD1.TabIndex = 20;
            this.ApalaD1.Text = "ApalaD1";
            this.ApalaD1.UseVisualStyleBackColor = true;
            // 
            // Mode
            // 
            this.Mode.Controls.Add(this.fi_z_konst);
            this.Mode.Controls.Add(this.ApalaD1);
            this.Mode.Controls.Add(this.LineDef);
            this.Mode.Controls.Add(this.melt);
            this.Mode.Controls.Add(this.beri);
            this.Mode.Location = new System.Drawing.Point(12, 76);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(408, 54);
            this.Mode.TabIndex = 21;
            this.Mode.TabStop = false;
            this.Mode.Text = "Mode";
            // 
            // parametrizacija
            // 
            this.parametrizacija.AutoSize = true;
            this.parametrizacija.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.parametrizacija.Location = new System.Drawing.Point(6, 19);
            this.parametrizacija.Name = "parametrizacija";
            this.parametrizacija.Size = new System.Drawing.Size(134, 24);
            this.parametrizacija.TabIndex = 21;
            this.parametrizacija.Text = "parametrizacija";
            this.parametrizacija.UseVisualStyleBackColor = true;
            // 
            // Rma_n
            // 
            this.Rma_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Rma_n.Location = new System.Drawing.Point(456, 46);
            this.Rma_n.Name = "Rma_n";
            this.Rma_n.Size = new System.Drawing.Size(72, 26);
            this.Rma_n.TabIndex = 22;
            this.Rma_n.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Parametrizacija_group
            // 
            this.Parametrizacija_group.Controls.Add(this.fizika);
            this.Parametrizacija_group.Controls.Add(this.parametrizacija);
            this.Parametrizacija_group.Location = new System.Drawing.Point(12, 168);
            this.Parametrizacija_group.Name = "Parametrizacija_group";
            this.Parametrizacija_group.Size = new System.Drawing.Size(211, 48);
            this.Parametrizacija_group.TabIndex = 27;
            this.Parametrizacija_group.TabStop = false;
            this.Parametrizacija_group.Text = "Parametrizacija";
            // 
            // fizika
            // 
            this.fizika.AutoSize = true;
            this.fizika.Checked = true;
            this.fizika.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fizika.Location = new System.Drawing.Point(146, 19);
            this.fizika.Name = "fizika";
            this.fizika.Size = new System.Drawing.Size(63, 24);
            this.fizika.TabIndex = 22;
            this.fizika.TabStop = true;
            this.fizika.Text = "fizika";
            this.fizika.UseVisualStyleBackColor = true;
            // 
            // BB_n
            // 
            this.BB_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BB_n.Location = new System.Drawing.Point(456, 136);
            this.BB_n.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BB_n.Name = "BB_n";
            this.BB_n.Size = new System.Drawing.Size(72, 26);
            this.BB_n.TabIndex = 31;
            // 
            // w_n
            // 
            this.w_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.w_n.Location = new System.Drawing.Point(296, 136);
            this.w_n.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.w_n.Name = "w_n";
            this.w_n.Size = new System.Drawing.Size(72, 26);
            this.w_n.TabIndex = 30;
            this.w_n.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // t_n
            // 
            this.t_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.t_n.Location = new System.Drawing.Point(151, 136);
            this.t_n.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.t_n.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.t_n.Name = "t_n";
            this.t_n.Size = new System.Drawing.Size(72, 26);
            this.t_n.TabIndex = 29;
            this.t_n.Value = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            // 
            // H_ksi_n
            // 
            this.H_ksi_n.DecimalPlaces = 1;
            this.H_ksi_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.H_ksi_n.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.H_ksi_n.Location = new System.Drawing.Point(12, 136);
            this.H_ksi_n.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.H_ksi_n.Name = "H_ksi_n";
            this.H_ksi_n.Size = new System.Drawing.Size(72, 26);
            this.H_ksi_n.TabIndex = 28;
            this.H_ksi_n.Value = new decimal(new int[] {
            45,
            0,
            0,
            65536});
            // 
            // BB_label
            // 
            this.BB_label.AutoSize = true;
            this.BB_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BB_label.Location = new System.Drawing.Point(534, 138);
            this.BB_label.Name = "BB_label";
            this.BB_label.Size = new System.Drawing.Size(31, 20);
            this.BB_label.TabIndex = 39;
            this.BB_label.Text = "BB";
            // 
            // w_label
            // 
            this.w_label.AutoSize = true;
            this.w_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.w_label.Location = new System.Drawing.Point(374, 138);
            this.w_label.Name = "w_label";
            this.w_label.Size = new System.Drawing.Size(20, 20);
            this.w_label.TabIndex = 38;
            this.w_label.Text = "w";
            // 
            // lt_label
            // 
            this.lt_label.AutoSize = true;
            this.lt_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lt_label.Location = new System.Drawing.Point(229, 138);
            this.lt_label.Name = "lt_label";
            this.lt_label.Size = new System.Drawing.Size(14, 20);
            this.lt_label.TabIndex = 37;
            this.lt_label.Text = "t";
            // 
            // H_ksi_label
            // 
            this.H_ksi_label.AutoSize = true;
            this.H_ksi_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.H_ksi_label.Location = new System.Drawing.Point(90, 138);
            this.H_ksi_label.Name = "H_ksi_label";
            this.H_ksi_label.Size = new System.Drawing.Size(44, 20);
            this.H_ksi_label.TabIndex = 36;
            this.H_ksi_label.Text = "H/ksi";
            // 
            // R0_rob_iteriras
            // 
            this.R0_rob_iteriras.AutoSize = true;
            this.R0_rob_iteriras.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.R0_rob_iteriras.Location = new System.Drawing.Point(15, 225);
            this.R0_rob_iteriras.Name = "R0_rob_iteriras";
            this.R0_rob_iteriras.Size = new System.Drawing.Size(127, 24);
            this.R0_rob_iteriras.TabIndex = 40;
            this.R0_rob_iteriras.Text = "R0 rob iteriras";
            this.R0_rob_iteriras.UseVisualStyleBackColor = true;
            // 
            // RN_rob_iteriras
            // 
            this.RN_rob_iteriras.AutoSize = true;
            this.RN_rob_iteriras.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.RN_rob_iteriras.Location = new System.Drawing.Point(148, 225);
            this.RN_rob_iteriras.Name = "RN_rob_iteriras";
            this.RN_rob_iteriras.Size = new System.Drawing.Size(129, 24);
            this.RN_rob_iteriras.TabIndex = 41;
            this.RN_rob_iteriras.Text = "RN rob iteriras";
            this.RN_rob_iteriras.UseVisualStyleBackColor = true;
            // 
            // Zrob
            // 
            this.Zrob.AutoSize = true;
            this.Zrob.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Zrob.Location = new System.Drawing.Point(15, 255);
            this.Zrob.Name = "Zrob";
            this.Zrob.Size = new System.Drawing.Size(65, 24);
            this.Zrob.TabIndex = 42;
            this.Zrob.Text = "Z rob";
            this.Zrob.UseVisualStyleBackColor = true;
            // 
            // homogeni_x_robovi
            // 
            this.homogeni_x_robovi.AutoSize = true;
            this.homogeni_x_robovi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.homogeni_x_robovi.Location = new System.Drawing.Point(148, 255);
            this.homogeni_x_robovi.Name = "homogeni_x_robovi";
            this.homogeni_x_robovi.Size = new System.Drawing.Size(156, 24);
            this.homogeni_x_robovi.TabIndex = 43;
            this.homogeni_x_robovi.Text = "homogeni x-robovi";
            this.homogeni_x_robovi.UseVisualStyleBackColor = true;
            // 
            // izpis_energij
            // 
            this.izpis_energij.AutoSize = true;
            this.izpis_energij.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.izpis_energij.Location = new System.Drawing.Point(12, 311);
            this.izpis_energij.Name = "izpis_energij";
            this.izpis_energij.Size = new System.Drawing.Size(110, 24);
            this.izpis_energij.TabIndex = 44;
            this.izpis_energij.Text = "izpis energij";
            this.izpis_energij.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 501);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(244, 20);
            this.textBox1.TabIndex = 45;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(310, 168);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(402, 402);
            this.pictureBox1.TabIndex = 46;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 581);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.izpis_energij);
            this.Controls.Add(this.homogeni_x_robovi);
            this.Controls.Add(this.Zrob);
            this.Controls.Add(this.RN_rob_iteriras);
            this.Controls.Add(this.R0_rob_iteriras);
            this.Controls.Add(this.BB_label);
            this.Controls.Add(this.w_label);
            this.Controls.Add(this.lt_label);
            this.Controls.Add(this.H_ksi_label);
            this.Controls.Add(this.BB_n);
            this.Controls.Add(this.w_n);
            this.Controls.Add(this.t_n);
            this.Controls.Add(this.H_ksi_n);
            this.Controls.Add(this.Parametrizacija_group);
            this.Controls.Add(this.Rma_n);
            this.Controls.Add(this.Mode);
            this.Controls.Add(this.Rma_label);
            this.Controls.Add(this.Rmi_label);
            this.Controls.Add(this.Nz_label);
            this.Controls.Add(this.Nr_label);
            this.Controls.Add(this.kor_label);
            this.Controls.Add(this.itmax_label);
            this.Controls.Add(this.eps_label);
            this.Controls.Add(this.ipisi_label);
            this.Controls.Add(this.Rmi_n);
            this.Controls.Add(this.Nz_n);
            this.Controls.Add(this.Nr_n);
            this.Controls.Add(this.kor_n);
            this.Controls.Add(this.itmax_n);
            this.Controls.Add(this.eps_n);
            this.Controls.Add(this.ipisi_n);
            this.Controls.Add(this.Start);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Liquid crystals";
            ((System.ComponentModel.ISupportInitialize)(this.ipisi_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eps_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itmax_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kor_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nr_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nz_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rmi_n)).EndInit();
            this.Mode.ResumeLayout(false);
            this.Mode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rma_n)).EndInit();
            this.Parametrizacija_group.ResumeLayout(false);
            this.Parametrizacija_group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BB_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.w_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.t_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.H_ksi_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.NumericUpDown ipisi_n;
        private System.Windows.Forms.NumericUpDown eps_n;
        private System.Windows.Forms.NumericUpDown itmax_n;
        private System.Windows.Forms.NumericUpDown kor_n;
        private System.Windows.Forms.NumericUpDown Nr_n;
        private System.Windows.Forms.NumericUpDown Nz_n;
        private System.Windows.Forms.NumericUpDown Rmi_n;
        private System.Windows.Forms.Label ipisi_label;
        private System.Windows.Forms.Label eps_label;
        private System.Windows.Forms.Label itmax_label;
        private System.Windows.Forms.Label kor_label;
        private System.Windows.Forms.Label Nr_label;
        private System.Windows.Forms.Label Nz_label;
        private System.Windows.Forms.Label Rmi_label;
        private System.Windows.Forms.Label Rma_label;
        private System.Windows.Forms.RadioButton LineDef;
        private System.Windows.Forms.RadioButton fi_z_konst;
        private System.Windows.Forms.RadioButton beri;
        private System.Windows.Forms.RadioButton melt;
        private System.Windows.Forms.RadioButton ApalaD1;
        private System.Windows.Forms.GroupBox Mode;
        private System.Windows.Forms.RadioButton parametrizacija;
        private System.Windows.Forms.NumericUpDown Rma_n;
        private System.Windows.Forms.GroupBox Parametrizacija_group;
        private System.Windows.Forms.RadioButton fizika;
        private System.Windows.Forms.NumericUpDown BB_n;
        private System.Windows.Forms.NumericUpDown w_n;
        private System.Windows.Forms.NumericUpDown t_n;
        private System.Windows.Forms.NumericUpDown H_ksi_n;
        private System.Windows.Forms.Label BB_label;
        private System.Windows.Forms.Label w_label;
        private System.Windows.Forms.Label lt_label;
        private System.Windows.Forms.Label H_ksi_label;
        private System.Windows.Forms.CheckBox R0_rob_iteriras;
        private System.Windows.Forms.CheckBox RN_rob_iteriras;
        private System.Windows.Forms.CheckBox Zrob;
        private System.Windows.Forms.CheckBox homogeni_x_robovi;
        private System.Windows.Forms.CheckBox izpis_energij;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

