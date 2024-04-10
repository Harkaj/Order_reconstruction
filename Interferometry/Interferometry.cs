using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Interferometry
{
    public partial class Interferometry : Form
    {
        public Interferometry()
        {
            InitializeComponent();
        }

        #region Variables

        static int M, N, L, mode;
        static double eps, d, lambda, n_o, n_e, beta0, beta1, delix, deliy, deliz;
        static double exRe, exIm, eyRe, eyIm, ezRe, ezIm, cos_fo, sin_fo, dx1, dy1, dz1, dz, fake;

        static double cos_pol0, sin_pol0, cos_pol1, sin_pol1;

        static int[][] rgbb;
        static double[] n;
        static double[,,,] SV;

        #endregion

        private void Start_Click(object sender, EventArgs e)
        {
            #region Initialization

            M = (int)MM_n.Value;
            N = (int)NN_n.Value;
            L = (int)LL_n.Value;

            eps = (double)error_n.Value;
            d = (double)d_n.Value;
            lambda = (double)lambda_n.Value;
            n_o = (double)n_o_n.Value;
            n_e = (double)n_e_n.Value;

            beta0 = (double)beta0_n.Value * Math.PI / 180.0;
            beta1 = (double)beta1_n.Value * Math.PI / 180.0;

            delix = (double)delix_n.Value;
            deliy = (double)deliy_n.Value;
            deliz = (double)deliz_n.Value;

            dz = d / (L * deliz);
            fake = 2.0 * Math.PI * dz / lambda;
            cos_fo = Math.Cos(fake * n_o);
            sin_fo = Math.Sin(fake * n_o);

            cos_pol0 = Math.Cos(beta0);
            sin_pol0 = Math.Sin(beta0);
            cos_pol1 = Math.Cos(beta1);
            sin_pol1 = Math.Sin(beta1);

            if (Input_file.Checked)
            {
                mode = 0;
            }
            if (PB.Checked)
            {
                mode = 1;
            }
            if (PP.Checked)
            {
                mode = 2;
            }
            if (ER.Checked)
            {
                mode = 3;
            }
            
            #endregion

            Create_director_field(mode);

            Color color = Color.FromArgb(0, 0, 0);
            Graphics formGraphics = pictureBox1.CreateGraphics();
            Draw(color, formGraphics);

            MessageBox.Show("Done!");
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
                    color = Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]);
                    formGraphics.FillRectangle(new SolidBrush(color), i * dx, j * dx, dx, dx);
                }
            }
        }

        #region Functions

        static void Create_director_field(int mode)
        {
            #region Input file

            if (mode == 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] datoteka, data;
                    string[] separators = { "\t", " " };

                    int ii, jj, kk, Nx, Ny, Nz;
                    int[] x_i, y_j, z_k;

                    datoteka = File.ReadAllLines(ofd.FileName);

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

                    M = Nx;
                    N = Ny;
                    L = Nz;

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

                        if (SV[ii, jj, kk, 0] < 0)
                        {
                            SV[ii, jj, kk, 0] = -SV[ii, jj, kk, 0];
                            SV[ii, jj, kk, 1] = -SV[ii, jj, kk, 1];
                            SV[ii, jj, kk, 2] = -SV[ii, jj, kk, 2];
                        }
                    }

                    #endregion
                }
            }
            
            #endregion

            #region Demonstrations

            else
            {
                #region Nastavitev vrednosti iz menija

                dx1 = 2.0 / ((double)M - 1.0);
                dy1 = 2.0 / ((double)N - 1.0);
                dz1 = 1.0 / ((double)L - 1.0);

                double ni, g, x, y, r, f;
                double[] vektor = new double[3];

                ni = 10.0;
                g = Math.Sqrt(1.0 + 4.0 / (ni * ni)) - 2.0 / ni;

                SV = new double[M, N, L, 3];

                #endregion

                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        x = i * dx1 - 1.0;
                        y = j * dy1 - 1.0;

                        r = Math.Sqrt(x * x + y * y);

                        if (Math.Abs(x) > eps)
                        {
                            f = Math.Atan2(y, x);
                        }
                        else
                        {
                            if (x > 0.0)
                            {
                                f = Math.PI / 2.0;
                            }
                            else
                            {
                                f = Math.PI * 3.0 / 2.0;
                            }
                        }

                        vektor = Vector(g, r, f, mode);

                        for (int k = 0; k < L; k++)
                        {
                            SV[i, j, k, 0] = vektor[0];
                            SV[i, j, k, 1] = vektor[1];
                            SV[i, j, k, 2] = vektor[2];
                        }
                    }
                }

                #region Writing

                using (StreamWriter writer = new StreamWriter("Projection_xy_k" + (L / 2).ToString() + ".txt", false))
                {
                    for (int i = 0; i < M; i++)
                    {
                        if (i % 3 == 0)
                        {
                            for (int j = 0; j < N; j++)
                            {
                                x = i * dx1 - 1.0;
                                y = j * dy1 - 1.0;

                                r = Math.Sqrt(x * x + y * y);

                                if (j % 3 == 0 && r < 1)
                                {
                                    writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(SV[i, j, (int)(L / 2), 1], SV[i, j, (int)(L / 2), 0]), 1.0);
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            
            #endregion

            #region Writing

            using (StreamWriter writer = new StreamWriter("Director_field.txt", false))
            {
                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        for (int k = 0; k < L; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}", i, j, k, SV[i, j, k, 0], SV[i, j, k, 1], SV[i, j, k, 2]);
                        }
                    }
                }
            }
            
            #endregion
        }

        static double[] Vector(double g, double r, double f, int mode)
        {
            double phi, theta;
            double[] vektor = new double[3];
            
            if (r > 1.0)
            {
                vektor[0] = 0.0;
                vektor[1] = 0.0;
                vektor[2] = 0.0;
            }
            else
            {
                if (mode == 1)
                {
                    phi = Math.Atan((1.0 + g * r * r) / (Math.Tan(f) * (1 - g * r * r)));

                    vektor[0] = Math.Cos(f) * Math.Cos(phi) - Math.Sin(f) * Math.Sin(phi);
                    vektor[1] = Math.Sin(f) * Math.Cos(phi) + Math.Cos(f) * Math.Sin(phi);
                    vektor[2] = 0.0;
                }

                if (mode == 2)
                {
                    phi = Math.PI / 2.0 - Math.Atan((1.0 + g * r * r) * Math.Tan(f) / (1 - g * r * r));

                    vektor[0] = Math.Cos(f) * Math.Cos(phi) - Math.Sin(f) * Math.Sin(phi);
                    vektor[1] = Math.Sin(f) * Math.Cos(phi) + Math.Cos(f) * Math.Sin(phi);
                    vektor[2] = 0.0;
                }

                if (mode == 3)
                {
                    phi = Math.PI / 2.0 - Math.Atan((1.0 + g * r * r) * Math.Tan(f) / (1 - g * r * r));
                    theta = 2.0 * Math.Atan(Math.Tan(Math.Acos(0.7)) * r);

                    vektor[0] = Math.Sin(theta) * Math.Cos(f);
                    vektor[1] = Math.Sin(theta) * Math.Sin(f);
                    vektor[2] = Math.Cos(theta);
                }
            }

            return vektor;
        }
        
        static double[] Determine_n(int ix, int iy, int iz, int iix, int iiy, int iiz)
        {
            double f111, f112, f121, f122, f211, f212, f221, f222, ff11, ff12, ff21, ff22;
            double fff1, fff2, fff, absn;

            double[] n = new double[3];

            #region Interpolation

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

            #region Normalization

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

        static void Convert(double[] n)
        {
            #region Setup

            double cos_fe, sin_fe, root, ni, kvoc, P11Re, P11Im, P12Re, P12Im, P22Re, P22Im;
            double exRe_new, exIm_new, eyRe_new, eyIm_new;

            root = n_o * n_o * (1.0 - n[2] * n[2]) + n_e * n_e * n[2] * n[2];
            ni = n_o * n_e / Math.Sqrt(root);
            cos_fe = Math.Cos(fake * ni);
            sin_fe = Math.Sin(fake * ni);

            #endregion

            #region Re and Im components of the polarization matrix

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
                P11Re = (cos_fo * n[1] * n[1] + cos_fe * n[0] * n[0]) / kvoc;
                P11Im = (sin_fo * n[1] * n[1] + sin_fe * n[0] * n[0]) / kvoc;

                P22Re = (cos_fo * n[0] * n[0] + cos_fe * n[1] * n[1]) / kvoc;
                P22Im = (sin_fo * n[0] * n[0] + sin_fe * n[1] * n[1]) / kvoc;

                P12Re = (cos_fe - cos_fo) * n[0] * n[1] / kvoc;
                P12Im = (sin_fe - sin_fo) * n[0] * n[1] / kvoc;
            }

            #endregion

            #region Re and Im components of the polarization vector

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

        static void Draw(Color color, Graphics formGraphics)
        {
            #region Initialization
            
            int iixzac, iiyzac, iizzac, ttx, tty, rgb, dx;
            double inter1;

            n = new double[3];

            rgbb = new int[(M - 1) * (int)delix + 1][];
            for (int i = 0; i < rgbb.Length; i++)
            {
                rgbb[i] = new int[(N - 1) * (int)deliy + 1];
            }
            
            if (delix <= 4)
            {
                dx = 2;
            }
            else
            {
                dx = 1;
            }

            #endregion

            #region Calculation

            using (StreamWriter writer = new StreamWriter("Output.txt", false))
            {
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
                                        n = Determine_n(ix, iy, iz, iix, iiy, iiz);
                                        Convert(n);
                                    }
                                }

                                inter1 = (cos_pol1 * exRe + sin_pol1 * eyRe) * (cos_pol1 * exRe + sin_pol1 * eyRe) + (cos_pol1 * exIm + sin_pol1 * eyIm) * (cos_pol1 * exIm + sin_pol1 * eyIm);
                                writer.WriteLine("{0,4}  {1,4}  {2,8:F6}  ", count_x, count_y, inter1);

                                #region Drawing

                                rgbb[count_x][count_y] = (int)(inter1 * 255);
                                rgb = (int)(inter1 * 255);
                                if (rgb < 0)
                                {
                                    rgb = 0;
                                    rgbb[count_x][count_y] = 0;
                                }
                                //color = Color.FromArgb(rgb, rgb, rgb);
                                //formGraphics.FillRectangle(new SolidBrush(color), count_x, count_y, 1, 1);

                                #endregion

                                count_y++;
                            }
                        }

                        count_x++;
                    }
                }
            }

            #region Drawing

            for (int i = 0; i < rgbb.Length; i++)
            {
                for (int j = 0; j < rgbb[i].Length; j++)
                {
                    color = Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]);
                    formGraphics.FillRectangle(new SolidBrush(color), i * dx, j * dx, dx, dx);
                }
            }

            #endregion

            #endregion

            #region Images and parameters

            using (Bitmap bmp = new Bitmap(rgbb.Length, rgbb[0].Length))
            {
                for (int i = 0; i < rgbb.Length; i++)
                {
                    for (int j = 0; j < rgbb[i].Length; j++)
                    {
                        bmp.SetPixel(rgbb.Length - i - 1, rgbb[i].Length - j - 1, Color.FromArgb(rgbb[i][j], rgbb[i][j], rgbb[i][j]));
                    }
                }

                bmp.Save("Interferometry.tif", System.Drawing.Imaging.ImageFormat.Tiff);
            }

            using (StreamWriter writer = new StreamWriter("dimension.txt", false))
            {
                ttx = (M - 1) * (int)delix + 1;
                tty = (N - 1) * (int)deliy + 1;

                writer.WriteLine("{0}  {1}", ttx, tty);
            }

            #endregion
        }

        #endregion
    }
}
