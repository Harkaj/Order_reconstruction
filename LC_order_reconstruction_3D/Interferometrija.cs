using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LC_order_reconstruction_3D
{
    public partial class Interferometrija : Form
    {
        public Interferometrija()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        #region Variables

        static int Nx, Ny, Nz, draw_size;
        static double eps, d, lambda, n_o, n_e, beta0, beta1, delix, deliy, deliz;
        static double exRe, exIm, eyRe, eyIm, cos_fo, sin_fo, dz, fake;
        static double cos_pol0, sin_pol0, cos_pol1, sin_pol1;
        
        static int[][] rgbb;
        static int[][][] rgbd;
        static double[][][] rgbc, rgbm;
        static double[] n;
        static double[,,,] SV;
        
        #endregion

        #region Actions

        private void Start_Click(object sender, EventArgs e)
        {
            #region Initialization

            draw_size = pictureBox1.Height;

            Nx = (int)MM_n.Value;
            Ny = (int)NN_n.Value;
            Nz = (int)LL_n.Value;

            eps = (double)error_n.Value;
            d = (double)d_n.Value;
            lambda = (double)lambda_n.Value;
            n_o = (double)n_o_n.Value;
            n_e = (double)n_e_n.Value;

            beta0 = (double)beta0_n.Value;
            beta1 = (double)beta1_n.Value;
            beta0 = beta0 * Math.PI / 180.0;
            beta1 = beta1 * Math.PI / 180.0;

            delix = (double)delix_n.Value;
            deliy = (double)deliy_n.Value;
            deliz = (double)deliz_n.Value;

            dz = d / (Nz * deliz);
            fake = 2.0 * Math.PI * dz / lambda;
            cos_fo = Math.Cos(fake * n_o);
            sin_fo = Math.Sin(fake * n_o);

            cos_pol0 = Math.Cos(beta0);
            sin_pol0 = Math.Sin(beta0);
            cos_pol1 = Math.Cos(beta1);
            sin_pol1 = Math.Sin(beta1);

            progressBar1.Visible = true;
            progressBar1.Maximum = Nx;
            progressBar1.Value = 0;

            #endregion

            #region Process

            if (BeriSV.Checked)
            {
                Beri_SV(Nx, Ny, Nz);
            }

            if (BeriDirektorskopolje.Checked)
            {
                Beri_Direktorsko_polje();
            }
            
            MessageBox.Show("Done!");
            progressBar1.Visible = false;

            #endregion
        }

        private void Redraw_Click(object sender, EventArgs e)
        {
            int dx;

            if ((int)delix_n.Value <= 4)
            {
                dx = 2;
            }
            else
            {
                dx = 1;
            }

            Color color = Color.FromArgb(0, 0, 0);
            Graphics formGraphics = pictureBox1.CreateGraphics();

            for (int i = 0; i < rgbb.Length; i++)
            {
                for (int j = 0; j < rgbb[i].Length; j++)
                {
                    //color = Color.FromArgb(rgbd[i][j][0], rgbd[i][j][1], rgbd[i][j][2]);
                    color = Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]);
                    formGraphics.FillRectangle(new SolidBrush(color), i * dx, draw_size - ((j + 1) * dx + 1), dx, dx);
                }
            }
        }

        private void Done_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        #endregion
        
        #region Functions

        static void Beri_SV(int M, int N, int L)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] datoteka = File.ReadAllLines(ofd.FileName);
                string[] data;
                string[] separators = { "\t", " " };

                int count = 0;
                SV = new double[M, N, L, 3];

                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        for (int k = 0; k < L; k++)
                        {
                            data = datoteka[count].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            SV[i, j, k, 0] = double.Parse(data[1]);
                            SV[i, j, k, 1] = double.Parse(data[2]);
                            SV[i, j, k, 2] = double.Parse(data[3]);

                            count++;
                        }
                    }
                }
            }
        }

        void Beri_Direktorsko_polje()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] datoteka, data;
                string[] separators = { "\t", " " };
                
                int ii, jj, kk, Nx, Ny, Nz, file_n;
                int[] x_i, y_j, z_k;

                file_n = 0;

                foreach (string file in ofd.FileNames)
                {
                    datoteka = File.ReadAllLines(file);

                    #region Postavitev lokacij

                    x_i = new int[datoteka.Length];
                    y_j = new int[datoteka.Length];
                    z_k = new int[datoteka.Length];

                    Nx = 0;
                    Ny = 0;
                    Nz = 0;
                    for (int i = 0; i < datoteka.Length; i++)
                    {
                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        x_i[i] = int.Parse(data[0]);
                        y_j[i] = int.Parse(data[1]);
                        z_k[i] = int.Parse(data[2]);

                        if (x_i[i] > Nx)
                        {
                            Nx = x_i[i];
                        }
                        if (y_j[i] > Ny)
                        {
                            Ny = y_j[i];
                        }
                        if (z_k[i] > Nz)
                        {
                            Nz = z_k[i];
                        }
                    }
                    Nx++;
                    Ny++;
                    Nz++;

                    #endregion

                    #region Določitev vrednosti

                    Interferometrija.Nx = Nx;
                    Interferometrija.Ny = Ny;
                    Interferometrija.Nz = Nz;

                    SV = new double[Nx, Ny, Nz, 3];

                    for (int count = 0; count < datoteka.Length; count++)
                    {
                        data = datoteka[count].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        ii = int.Parse(data[0]);
                        jj = int.Parse(data[1]);
                        kk = int.Parse(data[2]);

                        SV[ii, jj, kk, 0] = double.Parse(data[3]);
                        SV[ii, jj, kk, 1] = double.Parse(data[4]);
                        SV[ii, jj, kk, 2] = double.Parse(data[5]);
                    }

                    #endregion

                    if (Multiple_lambdas.Checked)
                    {
                        Risi_v2(file_n);
                    }
                    
                    else
                    {
                        Risi(file_n);
                    }
                    
                    file_n++;
                }

                #region Izris

                Color color = Color.FromArgb(0, 0, 0);
                Graphics formGraphics = pictureBox1.CreateGraphics();

                for (int i = 0; i < rgbb.Length; i++)
                {
                    for (int j = 0; j < rgbb[i].Length; j++)
                    {
                        //color = Color.FromArgb(rgbd[i][j][0], rgbd[i][j][1], rgbd[i][j][2]);
                        color = Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]);
                        formGraphics.FillRectangle(new SolidBrush(color), i, draw_size - (j + 1), 1, 1);
                    }
                }

                #endregion
            }
        }
        
        private void Risi(int file_n)
        {
            #region Original
            
            #region Initialization

            string dir;
            int iixzac, iiyzac, iizzac, ttx, tty;
            double inter1;

            n = new double[3];

            rgbb = new int[(Nx - 1) * (int)delix + 1][];
            rgbd = new int[(Nx - 1) * (int)delix + 1][][];
            for (int i = 0; i < rgbb.Length; i++)
            {
                rgbb[i] = new int[(Ny - 1) * (int)deliy + 1];
                rgbd[i] = new int[(Ny - 1) * (int)deliy + 1][];
                for (int j = 0; j < rgbd[i].Length; j++)
                {
                    rgbd[i][j] = new int[3];
                }
            }

            dir = "Interferometrija";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #endregion

            #region Izračun in izpis
            
            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "optizhod" + file_n.ToString() + ".txt"), false))
            {
                int count_x, count_y, counter;

                rgbm = new double[(Nx - 1) * (int)delix + 1][][];
                for (int i = 0; i < rgbm.Length; i++)
                {
                    rgbm[i] = new double[(Ny - 1) * (int)deliy + 1][];
                    for (int j = 0; j < rgbm[i].Length; j++)
                    {
                        rgbm[i][j] = new double[25];
                    }
                }

                counter = 0;
                lambda = 500.0;

                while (lambda <= 501.0)
                {
                    count_x = 0;

                    for (int ix = 0; ix < Nx - 1; ix++)
                    {
                        if (ix == 0) { iixzac = 0; }
                        else { iixzac = 1; }

                        for (int iix = iixzac; iix <= delix; iix++)
                        {
                            count_y = 0;

                            for (int iy = 0; iy < Ny - 1; iy++)
                            {
                                if (iy == 0) { iiyzac = 0; }
                                else { iiyzac = 1; }

                                for (int iiy = iiyzac; iiy <= deliy; iiy++)
                                {
                                    exRe = cos_pol0;
                                    eyRe = sin_pol0;
                                    exIm = 0.0;
                                    eyIm = 0.0;

                                    for (int iz = 0; iz < Nz - 1; iz++)
                                    {
                                        if (iz == 0) { iizzac = 0; }
                                        else { iizzac = 1; }

                                        for (int iiz = iizzac; iiz <= deliz; iiz++)
                                        {
                                            n = Interpolacija(ix, iy, iz, iix, iiy, iiz);
                                            Pretvori(n);
                                        }
                                    }

                                    inter1 = (cos_pol1 * exRe + sin_pol1 * eyRe) * (cos_pol1 * exRe + sin_pol1 * eyRe) + (cos_pol1 * exIm + sin_pol1 * eyIm) * (cos_pol1 * exIm + sin_pol1 * eyIm);
                                    writer.WriteLine("{0,4}  {1,4}  {2,8:F6}  ", count_x, count_y, inter1);

                                    if (inter1 > 0.0)
                                    {
                                        rgbm[count_x][count_y][counter] = inter1;
                                    }

                                    //rgbb[count_x][count_y] = (int)(inter1 * 255);
                                    /*if (rgbb[count_x][count_y] < 0)
                                    {
                                        rgbb[count_x][count_y] = 0;
                                    }*/

                                    count_y++;
                                }
                            }

                            count_x++;
                        }
                    }
                    lambda += 5.0;
                    fake = 2.0 * Math.PI * dz / lambda;
                    cos_fo = Math.Cos(fake * n_o);
                    sin_fo = Math.Sin(fake * n_o);

                    counter++;
                    progressBar1.Increment(1);
                }

                double temp_I;
                for (int i = 0; i < rgbm.Length; i++)
                {
                    for (int j = 0; j < rgbm[i].Length; j++)
                    {
                        temp_I = 0.0;
                        for (int k = 0; k < rgbm[i][j].Length; k++)
                        {
                            if (temp_I < rgbm[i][j][k])
                            {
                                temp_I = rgbm[i][j][k];
                            }
                        }

                        //writer.WriteLine("{0,4}  {1,4}  {2,8:F6}  ", i, j, temp_I);
                        rgbb[i][j] = (int)(temp_I * 255.0);
                        if (rgbb[i][j] > 255)
                        {
                            rgbb[i][j] = 255;
                        }
                    }
                }
            }
            
            #endregion

            #region RGB Izračun in izpis
            /*
            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "optizhod" + file_n.ToString() + ".txt"), false))
            {
                int count_x, count_y;
                
                for (lambda = 400.0; lambda <= 700.0; lambda += 20)
                {
                    count_x = 0;
                    
                    for (int ix = 0; ix < M - 1; ix++)
                    {
                        if (ix == 0) { iixzac = 0; }
                        else { iixzac = 1; }

                        for (int iix = iixzac; iix <= delix; iix++)
                        {
                            count_y = 0;

                            for (int iy = 0; iy < N - 1; iy++)
                            {
                                if (iy == 0) { iiyzac = 0; }
                                else { iiyzac = 1; }

                                for (int iiy = iiyzac; iiy <= deliy; iiy++)
                                {
                                    exRe = cos_pol0;
                                    eyRe = sin_pol0;
                                    exIm = 0.0;
                                    eyIm = 0.0;

                                    for (int iz = 0; iz < L - 1; iz++)
                                    {
                                        if (iz == 0) { iizzac = 0; }
                                        else { iizzac = 1; }

                                        for (int iiz = iizzac; iiz <= deliz; iiz++)
                                        {
                                            n = Interpolacija(ix, iy, iz, iix, iiy, iiz);
                                            Pretvori(n);
                                        }
                                    }

                                    inter1 = (cos_pol1 * exRe + sin_pol1 * eyRe) * (cos_pol1 * exRe + sin_pol1 * eyRe) + (cos_pol1 * exIm + sin_pol1 * eyIm) * (cos_pol1 * exIm + sin_pol1 * eyIm);
                                    //writer.WriteLine("{0,4}  {1,4}  {2,8:F6}  ", count_x, count_y, inter1);

                                    if (inter1 < 0)
                                    {
                                        inter1 = 0;
                                    }

                                    rgbb[count_x][count_y] += (int)(inter1 * 255);
                                    count_y++;
                                }
                            }

                            count_x++;
                        }
                    }
                }
            }
            
            for (int i = 0; i < rgbb.Length; i++)
            {
                for (int j = 0; j < rgbb[i].Length; j++)
                {
                    rgbb[i][j] = (int)(rgbb[i][j] / 15);
                    if (rgbb[i][j] > 255)
                    {
                        rgbb[i][j] = 255;
                    }
                }
            }
            */
            #endregion

            #region Slika in dimenzije

            using (Bitmap bmp = new Bitmap(rgbd.Length, rgbd[0].Length))
            {
                for (int i = 0; i < rgbd.Length; i++)
                {
                    for (int j = 0; j < rgbd[i].Length; j++)
                    {
                        //bmp.SetPixel(rgbd.Length - i - 1, rgbd[i].Length - j - 1, Color.FromArgb(rgbd[i][j][0], rgbd[i][j][1], rgbd[i][j][2]));
                        bmp.SetPixel(i, rgbb[i].Length - j - 1, Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]));
                    }
                }

                bmp.Save(Path.Combine(dir, "Interferometrija" + file_n.ToString() + ".png"), System.Drawing.Imaging.ImageFormat.Png);
            }
            
            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "dimenz.txt"), false))
            {
                ttx = (Nx - 1) * (int)delix + 1;
                tty = (Ny - 1) * (int)deliy + 1;

                writer.WriteLine("{0}  {1}", ttx, tty);
            }

            #endregion

            #endregion

            #region Repeated calculation
            /*
            #region Initialization

            string dir;
            int iixzac, iiyzac, iizzac, ttx, tty;
            double inter1;

            n = new double[3];

            rgbb = new int[(M - 1) * (int)delix + 1][];
            for (int i = 0; i < rgbb.Length; i++)
            {
                rgbb[i] = new int[(N - 1) * (int)deliy + 1];
            }

            dir = "D:\\Saša - MR\\Paper\\New results\\Ey0,15\\Secondary_interferometrija\\";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            int dx;

            if (delix <= 4)
            {
                dx = 2;
            }
            else
            {
                dx = 1;
            }

            #endregion

            #region Izračun in izpis

            for (int counter = 100; counter <= 20000; counter+=100)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "optizhod" + counter.ToString() + ".txt"), false))
                {
                    #region Postavitev lokacij

                    string path = "D:\\Saša - MR\\Paper\\New results\\Ey0,15\\Secondary_director_fields\\direktorsko_polje" + counter.ToString() + ".txt";
                    string[] datoteka = File.ReadAllLines(path);
                    string[] data;
                    string[] separators = { "\t", " " };

                    int Nx, Ny, Nz;
                    int[] x_i, y_j, z_k;

                    x_i = new int[datoteka.Length];
                    y_j = new int[datoteka.Length];
                    z_k = new int[datoteka.Length];

                    Nx = 0;
                    Ny = 0;
                    Nz = 0;
                    for (int i = 0; i < datoteka.Length; i++)
                    {
                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        x_i[i] = int.Parse(data[0]);
                        y_j[i] = int.Parse(data[1]);
                        z_k[i] = int.Parse(data[2]);

                        if (x_i[i] > Nx)
                        {
                            Nx = x_i[i];
                        }
                        if (y_j[i] > Ny)
                        {
                            Ny = y_j[i];
                        }
                        if (z_k[i] > Nz)
                        {
                            Nz = z_k[i];
                        }
                    }
                    Nx++;
                    Ny++;
                    Nz++;

                    #endregion

                    #region Importing data

                    M = Nx;
                    N = Ny;
                    L = Nz;

                    SV = new double[Nx, Ny, Nz, 3];
                    int ii, jj, kk;

                    for (int count = 0; count < datoteka.Length; count++)
                    {
                        data = datoteka[count].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        ii = int.Parse(data[0]);
                        jj = int.Parse(data[1]);
                        kk = int.Parse(data[2]);

                        SV[ii, jj, kk, 0] = double.Parse(data[3]);
                        SV[ii, jj, kk, 1] = double.Parse(data[4]);
                        SV[ii, jj, kk, 2] = double.Parse(data[5]);
                    }

                    #endregion

                    #region Izračun

                    int count_x, count_y;
                    count_x = 0;

                    for (int ix = 0; ix < M - 1; ix++)
                    {
                        if (ix == 0) { iixzac = 0; }
                        else { iixzac = 1; }

                        for (int iix = iixzac; iix <= delix; iix++)
                        {
                            count_y = 0;

                            for (int iy = 0; iy < N - 1; iy++)
                            {
                                if (iy == 0) { iiyzac = 0; }
                                else { iiyzac = 1; }

                                for (int iiy = iiyzac; iiy <= deliy; iiy++)
                                {
                                    exRe = cos_pol0;
                                    eyRe = sin_pol0;
                                    exIm = 0.0;
                                    eyIm = 0.0;

                                    for (int iz = 0; iz < L - 1; iz++)
                                    {
                                        if (iz == 0) { iizzac = 0; }
                                        else { iizzac = 1; }

                                        for (int iiz = iizzac; iiz <= deliz; iiz++)
                                        {
                                            n = Interpolacija(ix, iy, iz, iix, iiy, iiz);
                                            Pretvori(n);
                                        }
                                    }

                                    inter1 = (cos_pol1 * exRe + sin_pol1 * eyRe) * (cos_pol1 * exRe + sin_pol1 * eyRe) + (cos_pol1 * exIm + sin_pol1 * eyIm) * (cos_pol1 * exIm + sin_pol1 * eyIm);
                                    writer.WriteLine("{0,4}  {1,4}  {2,8:F6}  ", count_x, count_y, inter1);

                                    #region Risanje

                                    rgbb[count_x][count_y] = (int)(inter1 * 255);

                                    if (rgbb[count_x][count_y] < 0)
                                    {
                                        rgbb[count_x][count_y] = 0;
                                    }

                                    #endregion

                                    count_y++;
                                }
                            }

                            count_x++;
                        }
                    }

                    #endregion
                }
            }
            
            #region Risanje

            for (int i = 0; i < rgbb.Length; i++)
            {
                for (int j = 0; j < rgbb[i].Length; j++)
                {
                    color = Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]);
                    formGraphics.FillRectangle(new SolidBrush(color), i * dx, draw_size - ((j + 1) * dx + 1), dx, dx);
                }
            }
            
            using (Bitmap bmp = new Bitmap(rgbb.Length, rgbb[0].Length))
            {
                for (int i = 0; i < rgbb.Length; i++)
                {
                    for (int j = 0; j < rgbb[i].Length; j++)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]));
                    }
                }

                bmp.Save(Path.Combine(dir, "Interferometrija" + counter.ToString() + ".tif"), System.Drawing.Imaging.ImageFormat.Tiff);
            }

            #endregion

            #endregion

            #region Parametri

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "dimenz.txt"), false))
            {
                ttx = (M - 1) * (int)delix + 1;
                tty = (N - 1) * (int)deliy + 1;

                writer.WriteLine("{0}  {1}", ttx, tty);
            }

            #endregion
            */
            #endregion
        }
        
        private void Risi_v2(int file_n)
        {
            #region Initialization

            string dir;
            int iixzac, iiyzac, iizzac, ttx, tty, count_x, count_y, l_count;
            double inter1, l_red, l_green, l_blue, inter2;
            double[][] rgba;

            l_count = 0;

            l_red = 580.0;
            l_green = 540.0;
            l_blue = 440.0;

            n = new double[3];

            rgba = new double[(Nx - 1) * (int)delix + 1][];
            rgbb = new int[(Nx - 1) * (int)delix + 1][];
            rgbc = new double[(Nx - 1) * (int)delix + 1][][];
            rgbd = new int[(Nx - 1) * (int)delix + 1][][];
            for (int i = 0; i < rgbb.Length; i++)
            {
                rgba[i] = new double[(Ny - 1) * (int)deliy + 1];
                rgbb[i] = new int[(Ny - 1) * (int)deliy + 1];
                rgbc[i] = new double[(Ny - 1) * (int)deliy + 1][];
                rgbd[i] = new int[(Ny - 1) * (int)deliy + 1][];
                for (int j = 0; j < rgbd[i].Length; j++)
                {
                    rgba[i][j] = 0.0;
                    rgbc[i][j] = new double[3];
                    rgbd[i][j] = new int[3];

                    rgbc[i][j][0] = 0.0;
                    rgbc[i][j][1] = 0.0;
                    rgbc[i][j][2] = 0.0;
                }
            }

            dir = "Interferometrija";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #endregion

            #region Izračun in izpis

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "optizhod" + file_n.ToString() + ".txt"), false))
            {
                for (lambda = 380; lambda < 740; lambda += 20)
                {
                    fake = 2.0 * Math.PI * dz / lambda;
                    cos_fo = Math.Cos(fake * n_o);
                    sin_fo = Math.Sin(fake * n_o);

                    count_x = 0;

                    for (int ix = 0; ix < Nx - 1; ix++)
                    {
                        if (ix == 0) { iixzac = 0; }
                        else { iixzac = 1; }

                        for (int iix = iixzac; iix <= delix; iix++)
                        {
                            count_y = 0;

                            for (int iy = 0; iy < Ny - 1; iy++)
                            {
                                if (iy == 0) { iiyzac = 0; }
                                else { iiyzac = 1; }

                                for (int iiy = iiyzac; iiy <= deliy; iiy++)
                                {
                                    exRe = cos_pol0;
                                    eyRe = sin_pol0;
                                    exIm = 0.0;
                                    eyIm = 0.0;

                                    for (int iz = 0; iz < Nz - 1; iz++)
                                    {
                                        if (iz == 0) { iizzac = 0; }
                                        else { iizzac = 1; }

                                        for (int iiz = iizzac; iiz <= deliz; iiz++)
                                        {
                                            n = Interpolacija(ix, iy, iz, iix, iiy, iiz);
                                            Pretvori(n);
                                        }
                                    }

                                    inter1 = (cos_pol1*exRe + sin_pol1*eyRe)*(cos_pol1*exRe + sin_pol1*eyRe) + (cos_pol1*exIm + sin_pol1*eyIm)*(cos_pol1*exIm + sin_pol1*eyIm);
                                    
                                    rgba[count_x][count_y] += inter1;

                                    if (rgba[count_x][count_y] < 0)
                                    {
                                        rgba[count_x][count_y] = 0;
                                    }

                                    inter2 = Intensity_conversion((double)lambda, l_red);
                                    rgbc[count_x][count_y][0] += inter1 * inter2;

                                    inter2 = Intensity_conversion((double)lambda, l_green);
                                    rgbc[count_x][count_y][1] += inter1 * inter2;

                                    inter2 = Intensity_conversion((double)lambda, l_blue);
                                    rgbc[count_x][count_y][2] += inter1 * inter2;

                                    count_y++;
                                }
                            }

                            count_x++;
                        }

                        progressBar1.Increment(1);
                    }

                    l_count++;
                }

                for (int i = 0; i < rgba.Length; i++)
                {
                    for (int j = 0; j < rgba[i].Length; j++)
                    {
                        rgba[i][j] = rgba[i][j] / (double)l_count;
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F6}  ", i, j, rgba[i][j]);

                        rgbb[i][j] = (int)(rgba[i][j] * 255);
                        
                        rgbd[i][j][0] = (int)(rgbc[i][j][0] * 255 / l_count);
                        rgbd[i][j][1] = (int)(rgbc[i][j][1] * 255 / l_count);
                        rgbd[i][j][2] = (int)(rgbc[i][j][2] * 255 / l_count);
                    }
                }
            }

            #endregion

            #region Slika in dimenzije

            using (Bitmap bmp = new Bitmap(rgbd.Length, rgbd[0].Length))
            {
                for (int i = 0; i < rgbd.Length; i++)
                {
                    for (int j = 0; j < rgbd[i].Length; j++)
                    {
                        bmp.SetPixel(i, rgbd[i].Length - j - 1, Color.FromArgb(rgbd[i][j][0], rgbd[i][j][1], rgbd[i][j][2]));
                        //bmp.SetPixel(i, rgbb[i].Length - j - 1, Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]));
                    }
                }

                bmp.Save(Path.Combine(dir, "Interferometrija" + file_n.ToString() + ".png"), System.Drawing.Imaging.ImageFormat.Png);
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "dimenz.txt"), false))
            {
                ttx = (Nx - 1) * (int)delix + 1;
                tty = (Ny - 1) * (int)deliy + 1;

                writer.WriteLine("{0}  {1}", ttx, tty);
            }

            #endregion
        }

        static double[] Interpolacija(int ix, int iy, int iz, int iix, int iiy, int iiz)
        {
            double f111, f112, f121, f122, f211, f212, f221, f222, ff11, ff12, ff21, ff22;
            double fff1, fff2, fff, absn;

            double[] n = new double[3];

            #region Izračun vrednosti

            #region Nastavitev direktorjev
            
            if (SV[ix, iy, iz, 0] * SV[ix + 1, iy, iz, 0] + SV[ix, iy, iz, 1] * SV[ix + 1, iy, iz, 1] + SV[ix, iy, iz, 2] * SV[ix + 1, iy, iz, 2] < -0.5)
            {
                SV[ix + 1, iy, iz, 0] = -SV[ix + 1, iy, iz, 0];
                SV[ix + 1, iy, iz, 1] = -SV[ix + 1, iy, iz, 1];
                SV[ix + 1, iy, iz, 2] = -SV[ix + 1, iy, iz, 2];
            }
            if (SV[ix, iy, iz, 0] * SV[ix, iy + 1, iz, 0] + SV[ix, iy, iz, 1] * SV[ix, iy + 1, iz, 1] + SV[ix, iy, iz, 2] * SV[ix, iy + 1, iz, 2] < -0.5)
            {
                SV[ix, iy + 1, iz, 0] = -SV[ix, iy + 1, iz, 0];
                SV[ix, iy + 1, iz, 1] = -SV[ix, iy + 1, iz, 1];
                SV[ix, iy + 1, iz, 2] = -SV[ix, iy + 1, iz, 2];
            }
            if (SV[ix, iy, iz, 0] * SV[ix, iy, iz + 1, 0] + SV[ix, iy, iz, 1] * SV[ix, iy, iz + 1, 1] + SV[ix, iy, iz, 2] * SV[ix, iy, iz + 1, 2] < -0.5)
            {
                SV[ix, iy, iz + 1, 0] = -SV[ix, iy, iz + 1, 0];
                SV[ix, iy, iz + 1, 1] = -SV[ix, iy, iz + 1, 1];
                SV[ix, iy, iz + 1, 2] = -SV[ix, iy, iz + 1, 2];
            }
            if (SV[ix, iy, iz, 0] * SV[ix + 1, iy + 1, iz, 0] + SV[ix, iy, iz, 1] * SV[ix + 1, iy + 1, iz, 1] + SV[ix, iy, iz, 2] * SV[ix + 1, iy + 1, iz, 2] < -0.5)
            {
                SV[ix + 1, iy + 1, iz, 0] = -SV[ix + 1, iy + 1, iz, 0];
                SV[ix + 1, iy + 1, iz, 1] = -SV[ix + 1, iy + 1, iz, 1];
                SV[ix + 1, iy + 1, iz, 2] = -SV[ix + 1, iy + 1, iz, 2];
            }
            if (SV[ix, iy, iz, 0] * SV[ix + 1, iy, iz + 1, 0] + SV[ix, iy, iz, 1] * SV[ix + 1, iy, iz + 1, 1] + SV[ix, iy, iz, 2] * SV[ix + 1, iy, iz + 1, 2] < -0.5)
            {
                SV[ix + 1, iy, iz + 1, 0] = -SV[ix + 1, iy, iz + 1, 0];
                SV[ix + 1, iy, iz + 1, 1] = -SV[ix + 1, iy, iz + 1, 1];
                SV[ix + 1, iy, iz + 1, 2] = -SV[ix + 1, iy, iz + 1, 2];
            }
            if (SV[ix, iy, iz, 0] * SV[ix, iy + 1, iz + 1, 0] + SV[ix, iy, iz, 1] * SV[ix, iy + 1, iz + 1, 1] + SV[ix, iy, iz, 2] * SV[ix, iy + 1, iz + 1, 2] < -0.5)
            {
                SV[ix, iy + 1, iz + 1, 0] = -SV[ix, iy + 1, iz + 1, 0];
                SV[ix, iy + 1, iz + 1, 1] = -SV[ix, iy + 1, iz + 1, 1];
                SV[ix, iy + 1, iz + 1, 2] = -SV[ix, iy + 1, iz + 1, 2];
            }
            if (SV[ix, iy, iz, 0] * SV[ix + 1, iy + 1, iz + 1, 0] + SV[ix, iy, iz, 1] * SV[ix + 1, iy + 1, iz + 1, 1] + SV[ix, iy, iz, 2] * SV[ix + 1, iy + 1, iz + 1, 2] < -0.5)
            {
                SV[ix + 1, iy + 1, iz + 1, 0] = -SV[ix + 1, iy + 1, iz + 1, 0];
                SV[ix + 1, iy + 1, iz + 1, 1] = -SV[ix + 1, iy + 1, iz + 1, 1];
                SV[ix + 1, iy + 1, iz + 1, 2] = -SV[ix + 1, iy + 1, iz + 1, 2];
            }
            
            #endregion

            for (int i = 0; i < n.Length; i++)
            {
                f111 = SV[ix, iy, iz, i];
                f222 = SV[ix + 1, iy + 1, iz + 1, i];

                f112 = SV[ix, iy, iz + 1, i];
                f121 = SV[ix, iy + 1, iz, i];
                f211 = SV[ix + 1, iy, iz, i];

                f122 = SV[ix, iy + 1, iz + 1, i];
                f221 = SV[ix + 1, iy + 1, iz, i];
                f212 = SV[ix + 1, iy, iz + 1, i];

                ff11 = f111 + iix * (f211 - f111) / delix;
                ff21 = f121 + iix * (f221 - f121) / delix;
                ff12 = f112 + iix * (f212 - f112) / delix;
                ff22 = f122 + iix * (f222 - f122) / delix;

                fff1 = ff11 + iiy * (ff21 - ff11) / deliy;
                fff2 = ff12 + iiy * (ff22 - ff12) / deliy;

                fff = fff1 + iiz * (fff2 - fff1) / deliz;

                n[i] = fff;
            }

            #endregion

            #region Preverjanje velikosti

            absn = Math.Sqrt(n[0] * n[0] + n[1] * n[1] + n[2] * n[2]);

            if (absn < eps)
            {
                n[0] = 0.0;
                n[1] = 0.0;
                n[2] = 1.0;
            }

            else
            {
                n[0] = n[0] / absn;
                n[1] = n[1] / absn;
                n[2] = n[2] / absn;
            }

            #endregion

            return n;
        }

        static void Pretvori(double[] n)
        {
            #region Nastavitev parametrov

            double cos_fe, sin_fe, koren, ni, kvoc, P11Re, P11Im, P12Re, P12Im, P22Re, P22Im;
            double exRe_new, exIm_new, eyRe_new, eyIm_new;

            koren = n_o * n_o * (1.0 - n[2] * n[2]) + (n_e * n[2]) * (n_e * n[2]);
            ni = n_o * n_e / Math.Sqrt(koren);
            cos_fe = Math.Cos(fake * ni);
            sin_fe = Math.Sin(fake * ni);

            #endregion

            #region Re in Im komponente matrike

            if (Math.Abs(n[2]) > 1.0 - eps)
            {
                P11Re = cos_fo;
                P11Im = sin_fo;
                P22Re = cos_fo;
                P22Im = sin_fo;
                P12Re = 0.0;
                P12Im = 0.0;
            }

            else
            {
                kvoc = 1 - n[2] * n[2];
                P11Re = (cos_fo * n[0] * n[0] + cos_fe * n[1] * n[1]) / kvoc;
                P11Im = (sin_fo * n[0] * n[0] + sin_fe * n[1] * n[1]) / kvoc;

                P22Re = (cos_fo * n[1] * n[1] + cos_fe * n[0] * n[0]) / kvoc;
                P22Im = (sin_fo * n[1] * n[1] + sin_fe * n[0] * n[0]) / kvoc;

                P12Re = (cos_fo - cos_fe) * n[0] * n[1] / kvoc;
                P12Im = (sin_fo - sin_fe) * n[0] * n[1] / kvoc;
            }

            #endregion

            #region Re in Im komponente polarizacijskega vektorja

            exRe_new = P11Re * exRe - P11Im * exIm + P12Re * eyRe - P12Im * eyIm;
            exIm_new = P11Re * exIm + P11Im * exRe + P12Re * eyIm + P12Im * eyRe;

            eyRe_new = P12Re * exRe - P12Im * exIm + P22Re * eyRe - P22Im * eyIm;
            eyIm_new = P12Re * exIm + P12Im * exRe + P22Re * eyIm + P22Im * eyRe;

            exRe = exRe_new;
            exIm = exIm_new;
            eyRe = eyRe_new;
            eyIm = eyIm_new;

            #endregion
        }

        static double Intensity_conversion(double lambda, double lambda0)
        {
            double result, sigma, exponent;

            sigma = 50.0;
            exponent = (lambda - lambda0) * (lambda - lambda0) / (2 * sigma * sigma);

            result = Math.Exp(-exponent);

            return result;
        }
        
        #endregion
    }
}
