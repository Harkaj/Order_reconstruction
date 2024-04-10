using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace LC_order_reconstruction
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Variables

        const int NN = 500;
        static int ipisi, ibulk, Nr, Nz, itmax, nap, it, ini, ifile, irobN, irob0, irobZ, ffile;
        static double dr, dz, eps, Rmi, Rma, prev, t, tt, a, AA, kor, w, sb, sila, bba, bR, kR;
        static double zz0, zz1, zz2, zz3, zz4, Wt, Wb, We, BB, Fa0, Fa1, Ft0, Ft1, Dframe, dt, gama;

        static double[] xv, yv;
        static double[,] Sm, Dm, Fm, Sm_nov, Dm_nov, Fm_nov;

        static Random r = new Random();
        static Thread th;

        #endregion

        #region Stare funkcije

        private void Start_Click(object sender, EventArgs e)
        {
            #region Določitev vrednosti

            #region Nastavitev vrednosti iz menija

            xv = new double[NN];
            yv = new double[NN];

            Sm = new double[NN, NN];
            Dm = new double[NN, NN];
            Fm = new double[NN, NN];

            Sm_nov = new double[NN, NN];
            Dm_nov = new double[NN, NN];
            Fm_nov = new double[NN, NN];

            ipisi = (int)ipisi_n.Value;
            eps = (double)eps_n.Value;
            itmax = (int)itmax_n.Value;
            kor = (double)kor_n.Value;

            Nr = (int)Nr_n.Value;
            Nz = (int)Nz_n.Value;

            Rmi = (double)Rmi_n.Value;
            Rma = (double)Rma_n.Value;

            a = (double)H_ksi_n.Value;
            t = (double)t_n.Value;
            w = (double)w_n.Value;
            BB = (double)BB_n.Value;

            #endregion

            #region Izbira začetnega stanja

            if (LineDef.Checked)
            {
                ini = 0;
            }

            if (fi_z_konst.Checked)
            {
                ini = 1;
            }

            if (ApalaD1.Checked)
            {
                ini = 2;
            }

            if (beri.Checked)
            {
                ini = 3;
            }

            if (melt.Checked)
            {
                ini = 5;
            }
            
            #endregion
            
            #region Parametrizacija

            if (parametrizacija.Checked)
            {
                ifile = 1;
            }

            if (fizika.Checked)
            {
                ifile = 2;
            }

            #endregion

            #region Robni pogoji

            if (R0_rob_iteriras.Checked)
            {
                irob0 = 1;
            }

            if (RN_rob_iteriras.Checked)
            {
                irobN = 1;
            }

            if (Zrob.Checked)
            {
                irobZ = 1;
            }

            if (homogeni_x_robovi.Checked)
            {
                ibulk = 1;
            }

            #endregion

            // Do not forget the extra files!!!

            if (izpis_energij.Checked)
            {
                ffile = 1;
            }

            dr = (Rma - Rmi) / ((double)Nr - 1.0);
            dz = 1.0 / ((double)Nz - 1.0);
            tt = 1.0 + Math.Sqrt(1.0 - t);
            AA = a * a;
            sb = tt;

            dt = Math.Pow(10, -5);
            gama = 1.0;

            #endregion

            #region Izpis

            using (StreamWriter writer = new StreamWriter("Parameters.txt", false))
            {
                writer.WriteLine("ipisi: {0}  eps: {1}  itmax: {2}  kor: {3}", ipisi, eps, itmax, kor);
                writer.WriteLine("Nr: {0}  Nz: {1}", Nr, Nz);
                writer.WriteLine("ini: {0}", ini);
                writer.WriteLine("Rmi: {0}  Rma: {1}", Rmi, Rma);
                writer.WriteLine("a: {0}  t: {1}  w: {2}  BB: {3}", a, t, w, BB);
                writer.WriteLine("ifile: {0}", ifile);
                writer.WriteLine("irob0: {0}  irobN: {1}  irobZ: {2}  ibulk: {3}", irob0, irobN, irobZ, ibulk);
                writer.WriteLine("ffile: {0}", ffile);
            }

            #endregion

            th = new Thread(calculation);
            th.IsBackground = true;
            th.Start();
        }
        
        private void calculation()
        {
            #region Računanje

            init();

            if (itmax > 0)
            {
                iter();
            }

            pisi();
            if (ifile == 1)
            {
                pisiM();
            }
            else
            {
                pisiM2();
            }
            if (ffile == 1)
            {

            }

            aver(bba, Dframe, zz0, zz1, zz2, zz3, bR, kR);

            using (StreamWriter writer = new StreamWriter("Averages.txt", false))
            {
                writer.WriteLine("{0,10:F6}  {1,8:F4}  {2,8:F4}  {3,10:F6}  {4,8:F4}  {5,8:F4}  {6,8:F4}  {7,8:F4}", a, BB, bba, Dframe, AA, Rma, bR, kR);
            }

            #endregion

            Color color = Color.FromArgb(0, 0, 0);
            Graphics formGraphics = pictureBox1.CreateGraphics();

            narisi(Fm, Sm, Dm, color, formGraphics);

            SystemSounds.Asterisk.Play();
            MessageBox.Show("Calculation complete");
        }

        /// <summary>
        /// Inicializacija spremenljivk in konstant
        /// </summary>
        static void init()
        {
            #region Assigning values

            double qq0, dd, qqm, kot;
            
            prev = 180.0 / Math.PI;

            for (int i = 1; i <= Nr; i++)
            {
                xv[i] = dr * (i - 1) + Rmi;
            }
            for (int j = 1; j <= Nz; j++)
            {
                yv[j] = dz * (j - 1);
            }

            qq0 = tt / 6.0;
            qqm = 0.0;
            dd = - 3.0 * qq0;

            for (int i = 1; i <= Nr; i++)
            {
                for (int j = 1; j <= Nz; j++)
                {
                    Sm[i, j] = qq0;
                    Dm[i, j] = dd;
                    Fm[i, j] = qqm;
                }
            }

            #endregion

            #region Modes

            #region Enoosna z - smer

            if (ini == 0)
            {
                for (int i = 2; i <= Nr - 1; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        kot = Math.PI * yv[j] / 2.0;

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = - tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                Sm[1, Nz] = 0.0;
                Dm[1, Nz] = 0.0;
                Fm[1, Nz] = 0.0;

                Sm[Nr, Nz] = 0.0;
                Dm[Nr, Nz] = 0.0;
                Fm[Nr, Nz] = 0.0;
            }

            #endregion

            #region Linearni profil

            if (ini == 1)
            {
                for (int i = 2; i <= Nr; i++)
                {
                    for (int j = 2; j <= Nz; j++)
                    {
                        kot = Math.PI * yv[j] / 2.0;

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                Sm[1, Nz] = 0.0;
                Dm[1, Nz] = 0.0;
                Fm[1, Nz] = 0.0;
            }

            #endregion

            #region Apala, L1

            if (ini == 2)
            {
                kot = Math.PI / 4.0;

                for (int i = 2; i <= Nr - 1; i++)
                {
                    for (int j = 2; j <= Nz - 1; j++)
                    {
                        
                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                kot = Math.PI / 2.0;

                for (int i = 1; i <= Nr; i++)
                {
                    Sm[i, 1] = tt / 6.0;
                    Dm[i, 1] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[i, 1] = tt * Math.Sin(2.0 * kot) / 2.0;
                    Sm[i, Nz] = tt / 6.0;
                    Dm[i, Nz] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[i, Nz] = tt * Math.Sin(2.0 * kot) / 2.0;
                }

                kot = 0.0;

                for (int j = 1; j <= Nz; j++)
                {
                    Sm[1, j] = tt / 6.0;
                    Dm[1, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[1, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    Sm[Nr, j] = tt / 6.0;
                    Dm[Nr, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[Nr, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                }
            }

            #endregion

            #region Beri

            if (ini == 3)
            {
                string[] datoteka = File.ReadAllLines("D:\\Saša - MR\\LC2D Q tenzor\\m2.txt");
                string[] data;
                string[] separators = { "\t", " " };
                double[][] vrednosti = new double[datoteka.Length][];

                for (int i = 0; i < datoteka.Length; i++)
                {
                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    vrednosti[i] = new double[data.Length];

                    for (int j = 0; j < data.Length; j++)
                    {
                        vrednosti[i][j] = double.Parse(data[j]);
                    }
                }
                
                for (int i = 0; i < Nr; i++)
                {
                    for (int j = 0; j < Nz; j++)
                    {
                        Fm[i + 1, j + 1] = vrednosti[i * Nz + j][2];
                        Sm[i + 1, j + 1] = vrednosti[i * Nz + j][3];
                        Dm[i + 1, j + 1] = vrednosti[i * Nz + j][4];
                    }
                }

                using (StreamWriter writer = new StreamWriter("test_print.txt", false))
                {
                    for (int i = 1; i <= Nr; i++)
                    {
                        for (int j = 1; j <= Nz; j++)
                        {
                            writer.WriteLine("{0,8:F6}  {1,8:F6}  {2,8:F6}", Fm[i, j], Sm[i, j], Dm[i, j]);
                        }
                    }
                }
            }

            #endregion

            #region PR struktura

            if (ini == 4)
            {
                kot = Math.PI / 2.0;

                for (int i = 2; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        kot = Math.PI / 2.0 * yv[j];

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }
            }

            #endregion

            #region Melted

            if (ini == 5)
            {
                for (int i = 2; i <= Nr; i++)
                {
                    for (int j = 2; j <= Nz; j++)
                    {
                        kot = Math.PI / 2.0 * yv[j];

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                Sm[1, Nz] = 0.0;
                Dm[1, Nz] = 0.0;
                Fm[1, Nz] = 0.0;

                for (int i = 2; i <= Nr; i++)
                {
                    for (int j = 2; j <= Nz - 1; j++)
                    {
                        Sm[i, j] = 0.0;
                        Dm[i, j] = 0.0;
                        Fm[i, j] = 0.0;
                    }
                }
            }

            #endregion

            #endregion

            for (int i = 0; i <= Nr; i++)
            {
                for (int j = 0; j <= Nz; j++)
                {
                    Fm_nov[i, j] = Fm[i, j];
                    Sm_nov[i, j] = Sm[i, j];
                    Dm_nov[i, j] = Dm[i, j];
                }
            }
        }

        #region Original

        /// <summary>
        /// Enačba za F
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        static double funF(int i, int j)
        {
            double fnew, fx, f2x, f2y, ele, dele, r, qm, d, q0, pom, dpom;

            r = xv[i];
            q0 = Sm[i, j];
            qm = Fm[i, j];
            d = Dm[i, j];

            fx = (Fm[i + 1, j] - Fm[i - 1, j]) / (2.0 * dr);
            f2x = (Fm[i + 1, j] + Fm[i - 1, j] - 2.0 * Fm[i, j]) / (dr * dr);
            f2y = (Fm[i, j + 1] + Fm[i, j - 1] - 2.0 * Fm[i, j]) / (dz * dz);

            pom = -(t / 6.0) * qm + 2.0*q0*qm - qm * (d*d + qm*qm + 3.0*q0*q0) / 2.0;
            dpom = -t / 6.0 + 2.0 * q0 - (d*d + 3.0*qm*qm + 3.0*q0*q0) / 2.0;

            ele = f2x + f2y + pom * AA;
            dele = -2.0 / (dr * dr) - 2.0 / (dz * dz) + dpom * AA;

            fnew = Fm[i, j] - kor * ele / dele;
            if (Math.Abs(Fm[i, j] - fnew) > eps)
            {
                nap++;
            }
            return fnew;
        }

        /// <summary>
        /// Enačba za S
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        static double funS(int i, int j)  
        {
            double Snew, Sx, S2x, S2y, ele, dele, r, qm, d, q0, pom, dpom;

            r = xv[i];
            q0 = Sm[i, j];
            qm = Fm[i, j];
            d = Dm[i, j];

            Sx = (Sm[i + 1, j] - Sm[i - 1, j]) / (2.0 * dr);
            S2x = (Sm[i + 1, j] + Sm[i - 1, j] - 2.0 * Sm[i, j]) / (dr * dr);
            S2y = (Sm[i, j + 1] + Sm[i, j - 1] - 2.0 * Sm[i, j]) / (dz * dz);

            pom = -(t / 6.0) * q0 + (d*d + qm*qm - 3.0*q0*q0) / 3.0 - q0 *(3.0*q0*q0 + d*d + qm*qm) / 2.0;
            dpom = -t / 6.0 - 2.0 * q0 - (9.0*q0*q0 + d*d + qm*qm) / 2.0;

            ele = S2x + S2y + pom * AA + BB / 12.0;
            dele = -2.0 / (dr * dr) - 2.0 / (dz * dz) + dpom * AA;

            Snew = Sm[i, j] - kor * ele / dele;
            if (Math.Abs(Sm[i, j] - Snew) > eps)
            {
                nap++;
            }
            return Snew;
        }

        /// <summary>
        /// Enačba za D
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        static double funD(int i, int j)
        {
            double Dnew, Dx, D2x, D2y, ele, dele, r, qm, d, q0, pom, dpom;

            r = xv[i];
            q0 = Sm[i, j];
            qm = Fm[i, j];
            d = Dm[i, j];

            Dx = (Dm[i + 1, j] - Dm[i - 1, j]) / (2.0 * dr);
            D2x = (Dm[i + 1, j] + Dm[i - 1, j] - 2.0 * Dm[i, j]) / (dr * dr);
            D2y = (Dm[i, j + 1] + Dm[i, j - 1] - 2.0 * Dm[i, j]) / (dz * dz);
            
            pom = - (t / 6.0) * d + 2.0*q0*d - d * (d*d + qm*qm + 3.0*q0*q0) / 2.0;
            dpom = - t / 6.0 + 2.0 * q0 - (3.0*d*d + qm*qm + 3.0*q0*q0) / 2.0;

            ele = D2x + D2y + pom * AA - BB / 4.0;
            dele = -2.0 / (dr * dr) - 2.0 / (dz * dz) + dpom * AA;

            Dnew = Dm[i, j] - kor * ele / dele;
            if (Math.Abs(Dm[i, j] - Dnew) > eps)
            {
                nap++;
            }
            return Dnew;
        }

        #endregion

        #region Time dependent

        /// <summary>
        /// Enačba za F
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        static double funF2(int i, int j)
        {
            double fnew, fx, f2x, f2y, ele, r, qm, d, q0, pom;

            r = xv[i];
            q0 = Sm[i, j];
            qm = Fm[i, j];
            d = Dm[i, j];

            fx = (Fm[i + 1, j] - Fm[i - 1, j]) / (2.0 * dr);
            f2x = (Fm[i + 1, j] + Fm[i - 1, j] - 2.0 * Fm[i, j]) / (dr * dr);
            f2y = (Fm[i, j + 1] + Fm[i, j - 1] - 2.0 * Fm[i, j]) / (dz * dz);

            pom = -(t / 6.0) * qm + 2.0*q0*qm - qm * (d*d + qm*qm + 3.0*q0*q0) / 2.0;
            ele = 2.0 * ((f2x + f2y) / AA + pom) / gama;
            
            fnew = Fm[i, j] + dt * ele;
            if (Math.Abs(Fm[i, j] - fnew) > eps)
            {
                nap++;
            }
            return fnew;
        }

        /// <summary>
        /// Enačba za S
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        static double funS2(int i, int j)
        {
            double Snew, Sx, S2x, S2y, ele, r, qm, d, q0, pom;

            r = xv[i];
            q0 = Sm[i, j];
            qm = Fm[i, j];
            d = Dm[i, j];

            Sx = (Sm[i + 1, j] - Sm[i - 1, j]) / (2.0 * dr);
            S2x = (Sm[i + 1, j] + Sm[i - 1, j] - 2.0 * Sm[i, j]) / (dr * dr);
            S2y = (Sm[i, j + 1] + Sm[i, j - 1] - 2.0 * Sm[i, j]) / (dz * dz);

            pom = -(t / 6.0) * q0 + (d*d + qm*qm - 3.0*q0*q0) / 3.0 - q0 * (3.0*q0*q0 + d*d + qm*qm) / 2.0;
            ele = 2.0 * ((S2x + S2y) / AA + pom + BB / 12.0) / gama;

            Snew = Sm[i, j] + dt * ele;
            if (Math.Abs(Sm[i, j] - Snew) > eps)
            {
                nap++;
            }
            return Snew;
        }

        /// <summary>
        /// Enačba za D
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        static double funD2(int i, int j)
        {
            double Dnew, Dx, D2x, D2y, ele, r, qm, d, q0, pom;

            r = xv[i];
            q0 = Sm[i, j];
            qm = Fm[i, j];
            d = Dm[i, j];

            Dx = (Dm[i + 1, j] - Dm[i - 1, j]) / (2.0 * dr);
            D2x = (Dm[i + 1, j] + Dm[i - 1, j] - 2.0 * Dm[i, j]) / (dr * dr);
            D2y = (Dm[i, j + 1] + Dm[i, j - 1] - 2.0 * Dm[i, j]) / (dz * dz);

            pom = -(t / 6.0) * d + 2.0*q0*d - d * (d*d + qm*qm + 3.0*q0*q0) / 2.0;
            ele = 2.0 * ((D2x + D2y) / AA + pom - BB / 4.0) / gama;

            Dnew = Dm[i, j] + dt * ele;
            if (Math.Abs(Dm[i, j] - Dnew) > eps)
            {
                nap++;
            }
            return Dnew;
        }

        #endregion

        /// <summary>
        /// Iterativna zanka
        /// </summary>
        /// <param name="it_f">Pogoj za izvajanje zanke za F</param>
        /// <param name="it_s">Pogoj za izvajanje zanke za S</param>
        /// <param name="it_d">Pogoj za izvajanje zanke za D</param>
        static void iter()
        {
            nap = 1;
            it = 0;

            while (nap > 0 && it < itmax)
            {
                nap = 0;
                it++;

                for (int i = 2; i < Nr; i++)
                {
                    for (int j = 2; j < Nz; j++)
                    {
                        Fm_nov[i, j] = funF2(i, j);
                        Sm_nov[i, j] = funS2(i, j);
                        Dm_nov[i, j] = funD2(i, j);
                    }
                }

                for (int i = 2; i < Nr; i++)
                {
                    for (int j = 2; j < Nz; j++)
                    {
                        Fm[i, j] = Fm_nov[i, j];
                        Sm[i, j] = Sm_nov[i, j];
                        Dm[i, j] = Dm_nov[i, j];
                    }
                }

                #region old
                /*
                #region Pogoj it_f = 1

                if (it_f == 1)
                {
                    for (int i = 2; i <= Nr - 1; i++)
                    {
                        for (int j = 2; j <= Nz - 1; j++)
                        {
                            funF(i, j);
                        }
                    }

                    #region Robni pogoji

                    if (irobN == 1)
                    {
                        for (int j = 1; j <= Nz; j++)
                        {
                            Fm[Nr, j] = Fm[Nr - 1, j];
                        }
                    }

                    if (irobZ == 1)
                    {
                        for (int i = 2; i <= Nr; i++)
                        {
                            Fm[i, Nz] = Fm[i, Nz - 1] / (1 + w * dz);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Pogoj it_d = 1

                if (it_d == 1)
                {
                    for (int i = 2; i <= Nr - 1; i++)
                    {
                        for (int j = 2; j <= Nz - 1; j++)
                        {
                            funD(i, j);
                        }
                    }

                    #region Robni pogoji

                    if (irob0 == 1)
                    {
                        for (int j = 2; j <= Nz - 1; j++)
                        {
                            Dm[1, j] = Dm[2, j];
                        }
                    }

                    if (irobN == 1)
                    {
                        for (int j = 1; j <= Nz; j++)
                        {
                            Dm[Nr, j] = Dm[Nr - 1, j];
                        }
                    }

                    if (irobZ == 1)
                    {
                        for (int i = 1; i <= Nr; i++)
                        {
                            Dm[i, Nz] = (Dm[i, Nz - 1] + w * dz * tt / 2.0) / (1 + w * dz);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Pogoj it_ s = 1

                if (it_f == 1)
                {
                    for (int i = 2; i <= Nr - 1; i++)
                    {
                        for (int j = 2; j <= Nz - 1; j++)
                        {
                            funS(i, j);
                        }
                    }

                    #region Robni pogoji

                    if (irob0 == 1)
                    {
                        for (int j = 1; j <= Nz; j++)
                        {
                            Sm[1, j] = Sm[2, j];
                        }
                    }

                    if (irobN == 1)
                    {
                        for (int j = 2; j <= Nz; j++)
                        {
                            Sm[Nr, j] = Sm[Nr - 1, j];
                        }
                    }

                    if (irobZ == 1)
                    {
                        for (int i = 1; i <= Nr; i++)
                        {
                            Sm[i, Nz] = (Sm[i, Nz - 1] + w * dz * tt / 6.0) / (1 + w * dz);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Pogoj ibulk = 1

                if (ibulk == 1)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        Fm[Nr, j] = Fm[Nr - 1, j];
                        Fm[1, j] = Fm[2, j];

                        Sm[Nr, j] = Sm[Nr - 1, j];
                        Sm[1, j] = Sm[2, j];

                        Dm[Nr, j] = Dm[Nr - 1, j];
                        Dm[1, j] = Dm[2, j];
                    }
                }

                #endregion

                for (int i = 1; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        Fm[i, j] = Fm_nov[i, j];
                        Sm[i, j] = Sm_nov[i, j];
                        Dm[i, j] = Dm_nov[i, j];
                    }
                }
                */
                #endregion

                if (ipisi == 1)
                {
                    //output
                }

                //more output
            }
        }

        /// <summary>
        /// Enačba za določitev S
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        /// <param name="beta"></param>
        /// <param name="kot"></param>
        /// <param name="rEX"></param>
        /// <param name="OP"></param>
        static void structure(int i, int j, double beta, double kot, double rEX, double OP)
        {
            #region Določitev vrednosti

            double qm, d, q0, e1, e2, e3, pom2, pom3, OPref, bb2, kk, rr, ma, trace;

            qm = Fm[i, j];
            d = Dm[i, j];
            q0 = Sm[i, j];
            OPref = tt;

            rr = Math.Sqrt(d * d + qm * qm);
            e1 = q0 - rr;
            e2 = q0 + rr;
            e3 = - 2 * q0;

            #endregion

            #region Izračun

            pom2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
            pom3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);

            #region Pogoji za pom2 in d

            if (pom2 > 0.0)
            {
                bb2 = 1.0 - 6.0 * pom3 * pom3 / (pom2 * pom2 * pom2);
            }
            else
            {
                bb2 = 0.0;
            }

            if (Math.Abs(d) > 0.001)
            {
                kk = Math.Abs(Math.Atan(qm / d) / 2.0);
            }
            else
            {
                kk = Math.PI / 4.0;
            }

            if (d * qm > 0.0)
            {
                kk = Math.PI / 2.0 - kk;
            }

            #endregion

            ma = e1;
            if (e2 > ma)
            {
                ma = e2;
            }
            if (e3 > ma)
            {
                ma = e3;
            }

            beta = Math.Abs(bb2);
            kot = kk;
            rEX = rr;
            trace = 2.0 * (qm * qm + d * d + 3 * q0 * q0 * q0);
            OP = Math.Sqrt(3.0 * trace / 2.0) / OPref;

            #endregion
        }

        /// <summary>
        /// Enačba za določitev proste energije
        /// </summary>
        /// <param name="i">Lega i</param>
        /// <param name="j">Lega j</param>
        /// <param name="ffb"></param>
        /// <param name="ffe"></param>
        /// <param name="fft"></param>
        static void free(int i, int j, double ffb, double ffe, double fft)
        {
            #region Določitev vrednosti

            double pomB, tr2, tr3, tr4, r, qm, d, q0, pomE, pom1, pom2, dq0r, ddr, dqmr, dq0z, ddz, dqmz;

            qm = Fm[i, j];
            d = Dm[i, j];
            q0 = Sm[i, j];
            r = xv[i];

            dq0r = 0.0;
            ddr = 0.0;
            dqmr = 0.0;

            dq0z = 0.0;
            ddz = 0.0;
            dqmz = 0.0;

            #region Smer x

            if (i > 1 && i < Nr)
            {
                dqmr = (Fm[i + 1, j] - Fm[i - 1, j]) / (2.0 * dr);
                ddr = (Dm[i + 1, j] - Dm[i - 1, j]) / (2.0 * dr);
                dq0r = (Sm[i + 1, j] - Sm[i - 1, j]) / (2.0 * dr);
            }

            if (i == 1)
            {
                dqmr = (Fm[2, j] - Fm[1, j]) / dr;
                ddr = (Dm[2, j] - Dm[1, j]) / dr;
                dq0r = (Sm[2, j] - Sm[1, j]) / dr;
            }

            if (i == Nr)
            {
                dqmr = (Fm[Nr, j] - Fm[Nr - 1, j]) / dr;
                ddr = (Dm[Nr, j] - Dm[ Nr - 1, j]) / dr;
                dq0r = (Sm[Nr, j] - Sm[Nr - 1, j]) / dr;
            }

            #endregion

            #region Smer z

            if (j > 1 && j < Nz)
            {
                dqmz = (Fm[i, j + 1] - Fm[i, j - 1]) / (2.0 * dz);
                ddz = (Dm[i, j + 1] - Dm[i, j - 1]) / (2.0 * dz);
                dq0z = (Sm[i, j + 1] - Sm[i, j - 1]) / (2.0 * dz);
            }

            if (j == 1)
            {
                dqmz = (Fm[i, 2] - Fm[i, 1]) / dz;
                ddz = (Dm[i, 2] - Dm[i, 1]) / dz;
                dq0z = (Sm[i, 2] - Sm[i, 1]) / dz;
            }

            if (j == Nz)
            {
                dqmz = (Fm[i, Nz] - Fm[i, Nz - 1]) / dz;
                ddz = (Dm[i, Nz] - Dm[i, Nz - 1]) / dz;
                dq0z = (Sm[i, Nz] - Sm[i, Nz - 1]) / dz;
            }

            #endregion

            #endregion

            #region Izračun

            tr2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
            tr3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);
            tr4 = tr2 * tr2;
            pomB = (t * tr2 / 6.0 - 2.0 * tr3 / 3.0 + tr4 / 8.0) / 2.0 - BB * (q0 - d);

            pom1 = 3.0 * (dq0r * dq0r + dq0z * dq0z) + ddr * ddr + ddz * ddz + dqmr * dqmr + dqmz * dqmz;
            pom2 = 0.0;
            pomE = (pom1 + pom2) / AA;

            ffb = pomB;
            ffe = pomE;
            fft = 1;

            #endregion
        }

        /// <summary>
        /// Integral v smeri x
        /// </summary>
        /// <param name="en1"></param>
        /// <param name="en2"></param>
        /// <param name="norma"></param>
        /// <param name="j"></param>
        static void intx(double en1, double en2, double norma, int j)
        {
            #region Določitev vrednosti

            int ii, kn;
            double e1, e2, e3, s1, s2, s3;

            e1 = 0.0;
            e2 = 0.0;
            e3 = 0.0;

            kn = (int)((Nr - 1) / 2);
            ii = 1;
            free(ii, j, e1, e2, e3);
            s1 = e1;
            s2 = e2;
            s3 = e3;

            #endregion

            #region Izračun

            for (ii = 2; ii <= kn * 2; ii++)
            {
                free(ii, j, e1, e2, e3);

                if (ii - 1 % 2 == 1)
                {
                    s1 = s1 + 4.0 * e1;
                    s2 = s2 + 4.0 * e2;
                    s3 = s3 + 4.0 * e3;
                }
                else
                {
                    s1 = s1 + 2.0 * e1;
                    s2 = s2 + 2.0 * e2;
                    s3 = s3 + 2.0 * e3;
                }
            }

            free(ii, j, e1, e2, e3);
            s1 = s1 + e1;
            s2 = s2 + e2;
            s3 = s3 + e3;

            en1 = s1 * dr / 3.0;
            en2 = s2 * dr / 3.0;
            norma = s3 * dr / 3.0;

            #endregion
        }

        /// <summary>
        /// Dvojni integra v x in y
        /// </summary>
        /// <param name="en1"></param>
        /// <param name="en2"></param>
        /// <param name="norm"></param>
        static void intxy(double en1, double en2, double norm)
        {
            #region Določitev vrednosti

            int k;
            double e1, e2, e3, s1, s2, s3;

            e1 = 0.0;
            e2 = 0.0;
            e3 = 0.0;

            k = 1;
            intx(e1, e2, e3, k);
            s1 = e1;
            s2 = e2;
            s3 = e3;

            k = Nz;
            intx(e1, e2, e3, k);
            s1 = s1 + e1;
            s2 = s2 + e2;
            s3 = s3 + e3;

            #endregion

            #region Izračun

            for (k = 2; k <= Nz  - 1; k++)
            {
                intx(e1, e2, e3, k);

                if (k % 2 == 1)
                {
                    s1 = s1 + 2.0 * e1;
                    s2 = s2 + 2.0 * e2;
                    s3 = s3 + 2.0 * e3;
                }
                else
                {
                    s1 = s1 + 4.0 * e1;
                    s2 = s2 + 4.0 * e2;
                    s3 = s3 + 4.0 * e3;
                }
            }

            en1 = s1 * dz / 3.0;
            en2 = s2 * dz / 3.0;
            norm = s3 * dz / 3.0;

            #endregion
        }

        /// <summary>
        /// Izračun skupne energije
        /// </summary>
        /// <param name="Ener_b"></param>
        /// <param name="Ener_e"></param>
        /// <param name="Ener_t"></param>
        static void energy(double Ener_b, double Ener_e, double Ener_t)
        {
            double volum, pomE1, pomE2;

            volum = 0.0;
            pomE1 = 0.0;
            pomE2 = 0.0;
            intxy(pomE1, pomE2, volum);

            Ener_b = pomE1 / volum;
            Ener_e = pomE2 / volum;
            Ener_t = Ener_b + Ener_e;
        } //same

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        static double sil(int i, int j)
        {
            #region Določitev vrednosti

            double result = 0.0;
            double qm, d, q0, r, pom, pom1, Fpom1, Fpom2, dq0r, dq0z, ddr, ddz, dqmr, dqmz;

            qm = Fm[i, j];
            d = Dm[i, j];
            q0 = Sm[i, j];
            r = xv[i];

            dq0r = 0.0;
            dq0z = 0.0;
            ddr = 0.0;
            ddz = 0.0;
            dqmr = 0.0;
            dqmz = 0.0;

            #endregion

            #region Odvodi ob robovih

            if (i == 1)
            {
                dqmr = (Fm[2, j] - Fm[1, j]) / dr;
                ddr = (Dm[2, j] - Dm[1, j]) / dr;
                dq0r = (Sm[2, j] - Sm[1, j]) / dr;
            }
            if (i == Nr)
            {
                dqmr = (Fm[Nr, j] - Fm[Nr - 1, j]) / dr;
                ddr = (Dm[Nr, j] - Dm[Nr - 1, j]) / dr;
                dq0r = (Sm[Nr, j] - Sm[Nr - 1, j]) / dr;
            }
            if (j == 1)
            {
                dqmz = (Fm[i, 2] - Fm[i, 1]) / dz;
                ddz = (Dm[i, 2] - Dm[i, 1]) / dz;
                dq0z = (Sm[i, 2] - Sm[i, 1]) / dz;
            }

            if (j == Nz)
            {
                dqmz = (Fm[i, Nz] - Fm[i, Nz - 1]) / dz;
                ddz = (Dm[i, Nz] - Dm[i, Nz - 1]) / dz;
                dq0z = (Sm[i, Nz] - Sm[i, Nz - 1]) / dz;
            }

            #endregion

            #region Izračun

            pom = qm * qm + d * d + 3 * q0 * q0;
            pom1 = q0 * (d * d + qm * qm - q0 * q0);
            Fpom1 = 3.0 * (dq0r * dq0r - dq0z * dq0z) + (ddr * ddr - ddz * ddz) + (dqmr * dqmr - dqmz * dqmz);
            Fpom2 = t * pom / 6.0 - 2.0 * pom1 + pom * pom / 4.0;
            result = Fpom1 / AA + Fpom2;

            #endregion

            return result;
        }

        static void Force(int j, double Fav, double Ftot)
        {
            double e0, s0, r, e1, s1, ss;
            int kn;

            kn = (int)((Nr - 1) / 2);
            r = xv[1];
            s1 = sil(1, j) * r;
            s0 = r;

            #region Izračun

            for (int ii = 2; ii <= kn * 2; ii++)
            {
                r = xv[ii];
                e1 = sil(ii, j) * r;
                e0 = r;

                if (ii - 1 % 2 == 1)
                {
                    s1 = s1 + 4.0 * e1;
                    s0 = s0 + 4.0 * e0;
                }
                else
                {
                    s1 = s1 + 2.0 * e1;
                    s0 = s0 + 2.0 * e0;
                }
            }

            r = xv[Nr];
            e1 = sil(Nr, j) * r;
            e0 = r;
            s1 = s1 + e1;
            s0 = s0 + e0;

            #endregion

            ss = s0 * dr / 3.0;
            Ftot = s1 * dr / 3.0;
            Fav = Ftot / ss;
        }

        /// <summary>
        /// Function that determines the biaxiality
        /// </summary>
        /// <param name="q0"></param>
        /// <param name="d"></param>
        /// <param name="qm"></param>
        /// <returns></returns>
        static double beta(double q0, double d, double qm)
        {
            double pom, pom2, pom3;

            pom2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
            pom3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);

            if (pom2 > 0.0)
            {
                pom = 1.0 - 6.0 * pom3 * pom3 / (pom2 * pom2 * pom2);
            }
            else
            {
                pom = 0.0;
            }

            return pom;
        }  //same

        /// <summary>
        /// Izračun povprečne vrednosti
        /// </summary>
        /// <param name="bb"></param>
        /// <param name="koti"></param>
        /// <param name="z0"></param>
        /// <param name="z1"></param>
        /// <param name="z2"></param>
        /// <param name="z3"></param>
        /// <param name="bbR"></param>
        /// <param name="kotR"></param>
        static void aver(double bb, double koti, double z0, double z1, double z2, double z3, double bbR, double kotR)
        {
            #region Prireditev vrednosti

            int ii, jj;
            double r, b0, b1, b2, s0, s1, s2, pom2, pom3, qm, d, q0;

            s0 = 0.0;
            s1 = 0.0;
            s2 = 0.0;

            #endregion

            #region Izračun povprečja

            for (ii = 1; ii <= Nr; ii++)
            {
                r = xv[ii];
                for (jj = 1; jj <= Nz; jj++)
                {
                    qm = Fm[ii, jj];
                    d = Dm[ii, jj];
                    q0 = Sm[ii, jj];

                    pom2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
                    pom3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);

                    if (pom2 > 0.0)
                    {
                        b1 = 1.0 - 6.0 * pom3 * pom3 / (pom2 * pom2 * pom2);
                    }
                    else
                    {
                        b1 = 0.0;
                    }

                    b0 = 1.0;
                    b2 = Math.Abs(d);

                    s0 = s0 + b0;
                    s0 = s1 + b1;
                    s0 = s2 + b2;
                }
            }

            bb = s1 / s0;
            koti = s2 / s0;

            #endregion

            #region Povprečna biaksialnost pri r = R/2

            s0 = 0.0;
            s1 = 0.0;
            s2 = 0.0;

            for (jj = 1; jj <= Nz; jj++)
            {
                s0 = s0 + 1.0;

                qm = Fm[ii, jj];
                d = Dm[ii, jj];
                q0 = Sm[ii, jj];

                pom2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
                pom3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);

                if (pom2 > 0.0)
                {
                    b1 = 1.0 - 6.0 * pom3 * pom3 / (pom2 * pom2 * pom2);
                }
                else
                {
                    b1 = 0.0;
                }

                s1 = s1 + b1;
                s2 = s2 + Math.Abs(d);
            }

            bbR = s1 / s0;
            kotR = s2 / s0;

            #endregion
        }

        static void pisi()
        {
            double bb2, kot, rr, OP0;

            bb2 = 0.0;
            kot = 0.0;
            rr = 0.0;
            OP0 = 0.0;

            using (StreamWriter writer = new StreamWriter("m0.txt", false))
            {
                for (int i = 1; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        structure(i, j, bb2, kot, rr, OP0);

                        writer.WriteLine("{0,10:F6}  {1,10:F6}  {2,10:F6}  {3,10:F6}  {4,10:F6}  {5,10:F6}  {6,10:F6}  {7,10:F6}  {8,10:F6}", xv[i], yv[j], Fm[i, j], Sm[i, j] ,Dm[i, j], bb2, kot * prev, rr, OP0);
                    }
                }
            }
        }

        static void pisiM()
        {
            double q0, qm, d, trace, OP;

            #region Izpis Fm

            using (StreamWriter writer = new StreamWriter("m0a.txt", false))
            {
                for (int i = 1; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        writer.Write("{0,10:F6}  ", Fm[i, j]);
                    }
                    writer.WriteLine();
                }
            }

            #endregion

            #region Izpis Sm

            using (StreamWriter writer = new StreamWriter("m0b.txt", false))
            {
                for (int i = 1; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        writer.Write("{0,10:F6}  ", Sm[i, j]);
                    }
                    writer.WriteLine();
                }
            }

            #endregion

            #region Izpis Dm

            using (StreamWriter writer = new StreamWriter("m0c.txt", false))
            {
                for (int i = 1; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        writer.Write("{0,10:F6}  ", Dm[i, j]);
                    }
                    writer.WriteLine();
                }
            }

            #endregion

            #region Izpis sled

            using (StreamWriter writer = new StreamWriter("m0d.txt", false))
            {
                for (int i = 1; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        q0 = Sm[i, j];
                        qm = Fm[i, j];
                        d = Dm[i, j];

                        trace = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
                        OP = Math.Sqrt(trace * 3.0 / 2.0);

                        writer.Write("{0,10:F6}  ", OP);
                    }
                    writer.WriteLine();
                }
            }

            #endregion
        }

        static void pisiM2()
        {
            
        }

        #endregion

        static void narisi(double[,] Fm, double[,] Sm, double[,] Dm, Color color, Graphics formGraphics)
        {
            int rgb;
            double b2,  qm, d, q0;

            using (StreamWriter writer = new StreamWriter("beta.txt", false))
            {
                for (int i = 1; i <= Nr; i++)
                {
                    for (int j = 1; j <= Nz; j++)
                    {
                        qm = Fm[i, j];
                        d = Dm[i, j];
                        q0 = Sm[i, j];
                        
                        b2 = beta(q0, d, qm);

                        rgb = (int)(b2 * 255);
                        color = Color.FromArgb(rgb, rgb, rgb);
                        formGraphics.FillRectangle(new SolidBrush(color), 2 * (i - 1), 2 * (j - 1), 2, 2);

                        writer.Write("{0,8:F4}  ", b2);
                    }
                    writer.WriteLine();
                }
            }
        }
        
        #region Novejše funkcije

        private void Start_Click2(object sender, EventArgs e)
        {
            #region Določitev vrednosti

            #region Nastavitev vrednosti iz menija

            ipisi = (int)ipisi_n.Value;
            eps = (double)eps_n.Value;
            itmax = (int)itmax_n.Value;
            kor = (double)kor_n.Value;

            Nr = (int)Nr_n.Value;
            Nz = (int)Nz_n.Value;

            Rmi = (double)Rmi_n.Value;
            Rma = (double)Rma_n.Value;

            a = (double)H_ksi_n.Value;
            t = (double)t_n.Value;
            w = (double)w_n.Value;
            BB = (double)BB_n.Value;

            xv = new double[Nr];
            yv = new double[Nz];

            Sm = new double[Nr, Nz];
            Dm = new double[Nr, Nz];
            Fm = new double[Nr, Nz];

            #endregion

            #region Izbira začetnega stanja

            if (LineDef.Checked)
            {
                ini = 0;
            }

            if (fi_z_konst.Checked)
            {
                ini = 1;
            }

            if (ApalaD1.Checked)
            {
                ini = 2;
            }

            if (beri.Checked)
            {
                ini = 3;
            }

            if (melt.Checked)
            {
                ini = 5;
            }

            #endregion

            #region Parametrizacija

            if (parametrizacija.Checked)
            {
                ifile = 1;
            }

            if (fizika.Checked)
            {
                ifile = 2;
            }

            #endregion

            #region Robni pogoji

            if (R0_rob_iteriras.Checked)
            {
                irob0 = 1;
            }

            if (RN_rob_iteriras.Checked)
            {
                irobN = 1;
            }

            if (Zrob.Checked)
            {
                irobZ = 1;
            }

            if (homogeni_x_robovi.Checked)
            {
                ibulk = 1;
            }

            #endregion

            // Do not forget the extra files!!!

            if (izpis_energij.Checked)
            {
                ffile = 1;
            }

            dr = (Rma - Rmi) / ((double)Nr - 1.0);
            dz = 1 / ((double)Nz - 1.0);
            tt = Math.Sqrt(1 - t);
            AA = a * a;
            sb = tt;

            #endregion

            th = new Thread(calculation);
            th.IsBackground = true;
            th.Start();
        }

        static void initialization()
        {
            #region Assigning values

            double qq0, dd, qqm, kot;

            prev = 180.0 / Math.PI;

            for (int i = 0; i < Nr; i++)
            {
                xv[i] = dr * i + Rmi;
            }
            for (int j = 0; j < Nz; j++)
            {
                yv[j] = dz * j;
            }

            qq0 = tt / 6.0;
            qqm = 0.0;
            dd = -3.0 * qq0;

            for (int i = 0; i < Nr; i++)
            {
                for (int j = 0; j < Nz; j++)
                {
                    Sm[i, j] = qq0;
                    Dm[i, j] = dd;
                    Fm[i, j] = qqm;
                }
            }

            #endregion

            #region Modes

            #region Enoosna z - smer

            if (ini == 0)
            {
                for (int i = 1; i < Nr - 1; i++)
                {
                    for (int j = 0; j < Nz; j++)
                    {
                        kot = Math.PI * yv[j] / 2.0;

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                Sm[0, Nz - 1] = 0.0;
                Dm[0, Nz - 1] = 0.0;
                Fm[0, Nz - 1] = 0.0;

                Sm[Nr - 1, Nz - 1] = 0.0;
                Dm[Nr - 1, Nz - 1] = 0.0;
                Fm[Nr - 1, Nz - 1] = 0.0;
            }

            #endregion

            #region Linearni profil

            if (ini == 1)
            {
                for (int i = 1; i < Nr; i++)
                {
                    for (int j = 1; j < Nz; j++)
                    {
                        kot = Math.PI * yv[j] / 2.0;

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                Sm[0, Nz - 1] = 0.0;
                Dm[0, Nz - 1] = 0.0;
                Fm[0, Nz - 1] = 0.0;
            }

            #endregion

            #region Apala, L1

            if (ini == 2)
            {
                kot = Math.PI / 4.0;

                for (int i = 1; i < Nr - 1; i++)
                {
                    for (int j = 1; j < Nz - 1; j++)
                    {

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                kot = Math.PI / 2.0;

                for (int i = 0; i < Nr; i++)
                {
                    Sm[i, 0] = tt / 6.0;
                    Dm[i, 0] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[i, 0] = tt * Math.Sin(2.0 * kot) / 2.0;
                    Sm[i, Nz - 1] = tt / 6.0;
                    Dm[i, Nz - 1] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[i, Nz - 1] = tt * Math.Sin(2.0 * kot) / 2.0;
                }

                kot = 0.0;

                for (int j = 0; j < Nz; j++)
                {
                    Sm[0, j] = tt / 6.0;
                    Dm[0, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[0, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    Sm[Nr - 1, j] = tt / 6.0;
                    Dm[Nr - 1, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                    Fm[Nr - 1, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                }
            }

            #endregion

            #region Beri

            if (ini == 3)
            {
                string[] datoteka = File.ReadAllLines("D:\\Documents\\MR\\program_txt_files\\m2.txt");
                string[] data;
                string[] separators = { "\t", " " };
                double[][] vrednosti = new double[datoteka.Length][];

                for (int i = 0; i < datoteka.Length; i++)
                {
                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    vrednosti[i] = new double[data.Length];

                    for (int j = 0; j < data.Length; j++)
                    {
                        vrednosti[i][j] = double.Parse(data[j]);
                    }
                }

                for (int i = 0; i < Nr; i++)
                {
                    for (int j = 0; j < Nz; j++)
                    {
                        Fm[i, j] = vrednosti[i * Nz + j][2];
                        Sm[i, j] = vrednosti[i * Nz + j][3];
                        Dm[i, j] = vrednosti[i * Nz + j][4];
                    }
                }

                using (StreamWriter writer = new StreamWriter("test_print.txt", false))
                {
                    for (int i = 0; i < Nr; i++)
                    {
                        for (int j = 0; j < Nz; j++)
                        {
                            writer.WriteLine("{0,8:F6}  {1,8:F6}  {2,8:F6}", Fm[i, j], Sm[i, j], Dm[i, j]);
                        }
                    }
                }
            }

            #endregion

            #region PR struktura

            if (ini == 4)
            {
                kot = Math.PI / 2.0;

                for (int i = 1; i < Nr; i++)
                {
                    for (int j = 0; j < Nz; j++)
                    {
                        kot = Math.PI / 2.0 * yv[j];

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }
            }

            #endregion

            #region Melted

            if (ini == 5)
            {
                for (int i = 1; i < Nr; i++)
                {
                    for (int j = 1; j < Nz; j++)
                    {
                        kot = Math.PI / 2.0 * yv[j];

                        Sm[i, j] = tt / 6.0;
                        Dm[i, j] = -tt * Math.Cos(2.0 * kot) / 2.0;
                        Fm[i, j] = tt * Math.Sin(2.0 * kot) / 2.0;
                    }
                }

                Sm[0, Nz - 1] = 0.0;
                Dm[0, Nz - 1] = 0.0;
                Fm[0, Nz - 1] = 0.0;

                for (int i = 1; i < Nr; i++)
                {
                    for (int j = 1; j < Nz - 1; j++)
                    {
                        Sm[i, j] = 0.0;
                        Dm[i, j] = 0.0;
                        Fm[i, j] = 0.0;
                    }
                }
            }

            #endregion

            #endregion
        }

        static double funkcije(double[,] M, int i, int j, string name)
        {
            #region Inicializacija

            double result, fx, f2x, f2y, ele, dele, r, qm, d, q0, pom, dpom;

            ele = 0.0;
            dele = 0.0;
            result = 0.0;

            r = xv[i];
            q0 = M[i, j];
            qm = M[i, j];
            d = M[i, j];

            #endregion

            fx = (M[i + 1, j] - M[i - 1, j]) / (2.0 * dr);
            f2x = (M[i + 1, j] + M[i - 1, j] - 2.0 * M[i, j]) / Math.Sqrt(dr);
            f2y = (M[i, j + 1] + M[i, j - 1] - 2.0 * M[i, j]) / Math.Sqrt(dz);

            #region Fm

            if (name == "Fm")
            {
                pom = (t / 6.0) * qm - 2.0 * q0 * qm + qm * (d * d + qm * qm + 3.0 * q0 * q0) / 2.0;
                dpom = t / 6.0 - 2.0 * q0 + (d * d + 3.0 * qm * qm + 3.0 * q0 * q0) / 2.0;

                ele = -(f2x + f2y) + pom * AA;
                dele = -(-2.0 / Math.Sqrt(dr) - 2.0 / Math.Sqrt(dz)) + dpom * AA;
            }

            #endregion

            #region Sm

            if (name == "Sm")
            {
                pom = (t / 6.0) * q0 + (d * d + qm * qm - 3.0 * q0 * q0) / 3.0 - (3.0 * q0 * q0 * q0 + q0 * d * d + q0 * qm * qm) / 2.0;
                dpom = t / 6.0 - 2.0 * q0 - (9.0 * q0 * q0 + d * d + qm * qm) / 2.0;

                ele = f2x + f2y + pom * AA + BB / 12.0;
                dele = -2.0 / Math.Sqrt(dr) - 2.0 / Math.Sqrt(dz) + dpom * AA;
            }

            #endregion

            #region Dm

            if (name == "Dm")
            {
                pom = (t / 6.0) * d + 2.0 * q0 * d - d * (d * d + qm * qm + 3.0 * q0 * q0) / 2.0;
                dpom = t / 6.0 - 2.0 * q0 + (3.0 * d * d + qm * qm + 3.0 * q0 * q0) / 2.0;

                ele = f2x + f2y + pom * AA - BB / 4.0;
                dele = -2.0 / Math.Sqrt(dr) - 2.0 / Math.Sqrt(dz) + dpom * AA;
            }

            #endregion

            result = M[i, j] - kor * ele / dele;
            if (Math.Abs(M[i, j] - result) > eps)
            {
                nap++;
            }

            return result;
        }

        static void iteracija(int it_f, int it_s, int it_d)
        {
            nap = 1;
            it = 0;

            while (nap > 0 && it < itmax)
            {
                nap = 0;
                it++;

                #region Pogoj it_f = 1

                if (it_f == 1)
                {
                    for (int i = 1; i < Nr - 1; i++)
                    {
                        for (int j = 1; j < Nz - 1; j++)
                        {
                            funF(i, j);
                        }
                    }

                    #region Robni pogoji

                    if (irobN == 1)
                    {
                        for (int j = 0; j < Nz; j++)
                        {
                            Fm[Nr - 1, j] = Fm[Nr - 2, j];
                        }
                    }

                    if (irobZ == 1)
                    {
                        for (int i = 1; i < Nr; i++)
                        {
                            Fm[i, Nz - 1] = Fm[i, Nz - 2] / (1 + w * dz);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Pogoj it_d = 1

                if (it_d == 1)
                {
                    for (int i = 1; i < Nr - 1; i++)
                    {
                        for (int j = 1; j < Nz - 1; j++)
                        {
                            funD(i, j);
                        }
                    }

                    #region Robni pogoji

                    if (irob0 == 1)
                    {
                        for (int j = 1; j < Nz - 1; j++)
                        {
                            Dm[0, j] = Dm[1, j];
                        }
                    }

                    if (irobN == 1)
                    {
                        for (int j = 0; j < Nz; j++)
                        {
                            Dm[Nr - 1, j] = Dm[Nr - 2, j];
                        }
                    }

                    if (irobZ == 1)
                    {
                        for (int i = 0; i < Nr; i++)
                        {
                            Dm[i, Nz - 1] = (Dm[i, Nz - 2] + w * dz * tt / 2.0) / (1 + w * dz);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Pogoj it_ s = 1

                if (it_f == 1)
                {
                    for (int i = 1; i < Nr - 1; i++)
                    {
                        for (int j = 1; j < Nz - 1; j++)
                        {
                            funS(i, j);
                        }
                    }

                    #region Robni pogoji

                    if (irob0 == 1)
                    {
                        for (int j = 0; j < Nz; j++)
                        {
                            Sm[0, j] = Sm[1, j];
                        }
                    }

                    if (irobN == 1)
                    {
                        for (int j = 1; j < Nz; j++)
                        {
                            Sm[Nr - 1, j] = Sm[Nr - 2, j];
                        }
                    }

                    if (irobZ == 1)
                    {
                        for (int i = 0; i < Nr; i++)
                        {
                            Sm[i, Nz - 1] = (Sm[i, Nz - 2] + w * dz * tt / 6.0) / (1 + w * dz);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Pogoj ibulk = 1

                if (ibulk == 1)
                {
                    for (int j = 0; j < Nz; j++)
                    {
                        Fm[Nr - 1, j] = Fm[Nr - 2, j];
                        Fm[0, j] = Fm[1, j];

                        Sm[Nr - 1, j] = Sm[Nr - 2, j];
                        Sm[0, j] = Sm[1, j];

                        Dm[Nr - 1, j] = Dm[Nr - 2, j];
                        Dm[0, j] = Dm[1, j];
                    }
                }

                #endregion

                if (ipisi == 1)
                {
                    //output
                }

                //more output
            }
        }

        static void strukt(int i, int j, double beta, double kot, double rEX, double OP)
        {
            #region Določitev vrednosti

            double qm, d, q0, e1, e2, e3, pom2, pom3, OPref, bb2, kk, rr, ma, trace;

            qm = Fm[i, j];
            d = Dm[i, j];
            q0 = Sm[i, j];
            OPref = tt;

            rr = Math.Sqrt(d * d + qm * qm);
            e1 = q0 - rr;
            e2 = q0 + rr;
            e3 = -2 * q0;

            #endregion

            #region Izračun

            pom2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
            pom3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);

            #region Pogoji za pom2 in d

            if (pom2 > 0.0)
            {
                bb2 = 1.0 - 6.0 * pom3 * pom3 / (pom2 * pom2 * pom2);
            }
            else
            {
                bb2 = 0.0;
            }

            if (Math.Abs(d) > 0.001)
            {
                kk = Math.Abs(Math.Atan(qm / d) / 2.0);
            }
            else
            {
                kk = Math.PI / 4.0;
            }

            if (d * qm > 0.0)
            {
                kk = Math.PI / 2.0 - kk;
            }

            #endregion

            ma = e1;
            if (e2 > ma)
            {
                ma = e2;
            }
            if (e3 > ma)
            {
                ma = e3;
            }

            beta = Math.Abs(bb2);
            kot = kk;
            rEX = rr;
            trace = 2.0 * (qm * qm + d * d + 3 * q0 * q0 * q0);
            OP = Math.Sqrt(3.0 * trace / 2.0) / OPref;

            #endregion
        }

        static void free_en(int i, int j, double ffb, double ffe, double fft)
        {
            #region Določitev vrednosti

            double pomB, tr2, tr3, tr4, r, qm, d, q0, pomE, pom1, pom2, dq0r, ddr, dqmr, dq0z, ddz, dqmz;

            qm = Fm[i, j];
            d = Dm[i, j];
            q0 = Sm[i, j];
            r = xv[i];

            dq0r = 0.0;
            ddr = 0.0;
            dqmr = 0.0;

            dq0z = 0.0;
            ddz = 0.0;
            dqmz = 0.0;

            #region Smer x

            if (i > 0 && i < Nr - 1)
            {
                dqmr = (Fm[i + 1, j] - Fm[i - 1, j]) / (2.0 * dr);
                ddr = (Dm[i + 1, j] - Dm[i - 1, j]) / (2.0 * dr);
                dq0r = (Sm[i + 1, j] - Sm[i - 1, j]) / (2.0 * dr);
            }

            if (i == 0)
            {
                dqmr = (Fm[i + 1, j] - Fm[i, j]) / dr;
                ddr = (Dm[i + 1, j] - Dm[i, j]) / dr;
                dq0r = (Sm[i + 1, j] - Sm[i, j]) / dr;
            }

            if (i == Nr - 1)
            {
                dqmr = (Fm[i, j] - Fm[i - 1, j]) / dr;
                ddr = (Dm[i, j] - Dm[i - 1, j]) / dr;
                dq0r = (Sm[i, j] - Sm[i - 1, j]) / dr;
            }

            #endregion

            #region Smer z

            if (j > 0 && j < Nz - 1)
            {
                dqmz = (Fm[i, j + 1] - Fm[i, j - 1]) / (2.0 * dz);
                ddz = (Dm[i, j + 1] - Dm[i, j - 1]) / (2.0 * dz);
                dq0z = (Sm[i, j + 1] - Sm[i, j - 1]) / (2.0 * dz);
            }

            if (j == 0)
            {
                dqmz = (Fm[i, j + 1] - Fm[i, j]) / dz;
                ddz = (Dm[i, j + 1] - Dm[i, j]) / dz;
                dq0z = (Sm[i, j + 1] - Sm[i, j]) / dz;
            }

            if (j == Nz - 1)
            {
                dqmz = (Fm[i, j] - Fm[i, j - 1]) / dz;
                ddz = (Dm[i, j] - Dm[i, j - 1]) / dz;
                dq0z = (Sm[i, j] - Sm[i, j - 1]) / dz;
            }

            #endregion

            #endregion

            #region Izračun

            tr2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
            tr3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);
            tr4 = tr2 * tr2;
            pomB = (t * tr2 / 6.0 - 2.0 * tr3 / 3.0 + tr4 / 8.0) / 2.0 - BB * (q0 - d);

            pom1 = 3.0 * (Math.Sqrt(dq0r) + Math.Sqrt(dq0z)) + Math.Sqrt(ddr) + Math.Sqrt(ddz) + Math.Sqrt(dqmr) + Math.Sqrt(dqmz);
            pom2 = 0.0;
            pomE = (pom1 + pom2) / AA;

            ffb = pomB;
            ffe = pomE;
            fft = 1;

            #endregion
        }
        
        static void int_x(double en1, double en2, double norma, int j)
        {
            #region Določitev vrednosti
            
            double e1, e2, e3, s1, s2, s3;

            e1 = 0.0;
            e2 = 0.0;
            e3 = 0.0;
            
            free_en(0, j, e1, e2, e3);
            s1 = e1;
            s2 = e2;
            s3 = e3;
            
            #endregion

            #region Izračun

            for (int i = 1; i < Nr - 1; i++)
            {
                free_en(i, j, e1, e2, e3);

                if (i % 2 == 1)
                {
                    s1 += 4.0 * e1;
                    s2 += 4.0 * e2;
                    s3 += 4.0 * e3;
                }
                else
                {
                    s1 += 2.0 * e1;
                    s2 += 2.0 * e2;
                    s3 += 2.0 * e3;
                }
            }

            free_en(Nr - 1, j, e1, e2, e3);
            s1 += e1;
            s2 += e2;
            s3 += e3;

            en1 = s1 * dr / 3.0;
            en2 = s2 * dr / 3.0;
            norma = s3 * dr / 3.0;

            #endregion
        }

        static void int_xy(double en1, double en2, double norm)
        {
            #region Določitev vrednosti
            
            double e1, e2, e3, s1, s2, s3;

            e1 = 0.0;
            e2 = 0.0;
            e3 = 0.0;
            
            intx(e1, e2, e3, 0);
            s1 = e1;
            s2 = e2;
            s3 = e3;
            
            intx(e1, e2, e3, Nz - 1);
            s1 += e1;
            s2 += e2;
            s3 += e3;

            #endregion

            #region Izračun

            for (int i = 1; i < Nz - 1; i++)
            {
                intx(e1, e2, e3, i);

                if (i % 2 == 0)
                {
                    s1 += 2.0 * e1;
                    s2 += 2.0 * e2;
                    s3 += 2.0 * e3;
                }
                else
                {
                    s1 += 4.0 * e1;
                    s2 += 4.0 * e2;
                    s3 += 4.0 * e3;
                }
            }

            en1 = s1 * dz / 3.0;
            en2 = s2 * dz / 3.0;
            norm = s3 * dz / 3.0;

            #endregion
        }
        
        static void average(double bb, double koti, double z0, double z1, double z2, double z3, double bbR, double kotR)
        {
            #region Prireditev vrednosti

            double r, b0, b1, b2, s0, s1, s2, pom2, pom3, qm, d, q0;

            s0 = 0.0;
            s1 = 0.0;
            s2 = 0.0;

            #endregion

            #region Izračun povprečja

            for (int i = 0; i < Nr; i++)
            {
                r = xv[i];
                for (int j = 0; j < Nz; j++)
                {
                    qm = Fm[i, j];
                    d = Dm[i, j];
                    q0 = Sm[i, j];

                    pom2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
                    pom3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);

                    if (pom2 > 0.0)
                    {
                        b1 = 1.0 - 6.0 * pom3 * pom3 / (pom2 * pom2 * pom2);
                    }
                    else
                    {
                        b1 = 0.0;
                    }

                    b0 = 1.0;
                    b2 = Math.Abs(d);

                    s0 = s0 + b0;
                    s0 = s1 + b1;
                    s0 = s2 + b2;
                }
            }

            bb = s1 / s0;
            koti = s2 / s0;

            #endregion

            #region Povprečna biaksialnost pri r = R/2

            s0 = 0.0;
            s1 = 0.0;
            s2 = 0.0;

            for (int j = 0; j < Nz; j++)
            {
                s0 = s0 + 1.0;

                qm = Fm[(int)(Nr / 2), j];
                d = Dm[(int)(Nr / 2), j];
                q0 = Sm[(int)(Nr / 2), j];

                pom2 = 2.0 * (qm * qm + d * d + 3 * q0 * q0);
                pom3 = 6.0 * q0 * (d * d + qm * qm - q0 * q0);

                if (pom2 > 0.0)
                {
                    b1 = 1.0 - 6.0 * pom3 * pom3 / (pom2 * pom2 * pom2);
                }
                else
                {
                    b1 = 0.0;
                }

                s1 = s1 + b1;
                s2 = s2 + Math.Abs(d);
            }

            bbR = s1 / s0;
            kotR = s2 / s0;

            #endregion
        }

        static void pisi_2()
        {
            double bb2, kot, rr, OP0;

            bb2 = 0.0;
            kot = 0.0;
            rr = 0.0;
            OP0 = 0.0;

            using (StreamWriter writer = new StreamWriter("m0.txt", false))
            {
                for (int i = 0; i < Nr; i++)
                {
                    for (int j = 0; j < Nz; j++)
                    {
                        structure(i, j, bb2, kot, rr, OP0);

                        writer.WriteLine("{0,10:F6}  {1,10:F6}  {2,10:F6}  {3,10:F6}  {4,10:F6}  {5,10:F6}  {6,10:F6}  {7,10:F6}  {8,10:F6}", xv[i], yv[j], Fm[i, j], Sm[i, j], Dm[i, j], bb2, kot * prev, rr, OP0);
                    }
                }
            }
        }

        #endregion
    }
}
