using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LC_order_reconstruction_3D
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        #region Variables

        static bool defekt, reset, interpolation, new_boundary, time_dependent, inserts, unequal_L, chiral, zoom_in;
        static int i_lower, i_upper, j_lower, j_upper, k_lower, k_upper, bulk, upper_boundary, lower_boundary, sides, fname, plane, skip1, skip2;
        static int Nx, Ny, Nz, itmax, nap, it, run_type, N_defektov, E_changing, N_r, changemode, factor;
        static int moire_r1, moire_r2, moire_r3, zoom_x, zoom_y;
        static double dx, dy, dz, dxy, dxz, dyz, dxyz, eps, Rmi, Rma, t, tt, a, AA, kor, w, sb, BB, B_f, Ex, Ey, Ez, E_max;
        static double gamma, dt, k1, k2, k3, L1, L2, L3, L_chiral, phi0_upper, phi0_lower, phi0_bulk, deps, dmu;
        static int draw_mode = 0;
        static int draw_size = 0;

        static int[][][] matter;
        static double[] xv, yv, zv, E_system;
        static double[][] defekti_up, defekti_down;
        static double[][][] Q1, Q2, Q3, Q4, Q5, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n, b2, S;
        static double[][][] Q1_plate, Q2_plate, Q3_plate, Q4_plate, Q5_plate;
        static double[][][][] direktor, E, B;

        static List<int[]> Inserts = new List<int[]>();
        static List<double[][][]> Parameters1 = new List<double[][][]>();
        static List<double[][][]> Parameters2 = new List<double[][][]>();
        static Random r = new Random();

        #region Interferometry variables

        static double d, lambda, n_o, n_e, beta0, beta1, delix, deliy, deliz;
        static double exRe, exIm, eyRe, eyIm, cos_fo, sin_fo, fake;
        static double cos_pol0, sin_pol0, cos_pol1, sin_pol1;

        static int[][] rgbb;
        static int[][][] rgbd;
        static double[][][] rgbc;
        static double[] n;
        static double[,,,] SV;

        #endregion

        #endregion


        #region Setup

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private double[][] Defect_setup(NumericUpDown N_defects, string up_down)
        {
            #region Setup

            double[][] defects;

            N_defektov = (int)N_defects.Value;

            defects = new double[N_defektov][];
            for (int i = 0; i < N_defektov; i++)
            {
                defects[i] = new double[3];
            }

            #endregion

            #region Special case for N = 16

            if (N_defektov == 16)
            {
                if (up_down == "Upper boundary ")
                {
                    phi0_upper = Math.PI / 4.0;
                }
                
                else if (up_down == "Lower boundary ")
                {
                    phi0_lower = Math.PI / 4.0;
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        defects[4 * i + j][0] = (i + 1) * (double)Nx / 5.0;
                        defects[4 * i + j][1] = (j + 1) * (double)Ny / 5.0;

                        /*if (i == 1)
                        {
                            if (j == 1)
                            {
                                defekti[4 * i + j][0] -= 2.0;
                                defekti[4 * i + j][1] -= 2.0;
                            }
                            if (j == 2)
                            {
                                defekti[4 * i + j][0] -= 2.0;
                                defekti[4 * i + j][1] += 2.0;
                            }

                        }
                        if (i == 2)
                        {
                            if (j == 1)
                            {
                                defekti[4 * i + j][0] += 2.0;
                                defekti[4 * i + j][1] -= 2.0;
                            }
                            if (j == 2)
                            {
                                defekti[4 * i + j][0] += 2.0;
                                defekti[4 * i + j][1] += 2.0;
                            }
                        }*/


                        if ((i + j) % 2 == 0)
                        {
                            defects[4 * i + j][2] = 1.0;
                        }
                        else
                        {
                            defects[4 * i + j][2] = -1.0;
                        }
                    }
                }
            }

            #endregion
            
            #region Special case for N = 9

            else if (N_defektov == 9)
            {
                if (up_down == "Upper boundary ")
                {
                    phi0_upper = Math.PI / 4.0;
                }

                else if (up_down == "Lower boundary ")
                {
                    phi0_lower = Math.PI / 4.0;
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        defects[3 * i + j][0] = i * (double)Nx - 50;
                        defects[3 * i + j][1] = j * (double)Ny - 50;

                        /*if (i == 0)
                        {
                            defects[2 * i + j][0] += 5.0;
                        }
                        if (i == 1)
                        {
                            defects[2 * i + j][0] -= 5.0;
                        }*/

                        if ((i + j) % 2 == 0)
                        {
                            defects[3 * i + j][2] = 0.5;
                        }
                        else
                        {
                            defects[3 * i + j][2] = -0.5;
                        }
                    }
                }
            }

            #endregion
            
            #region Special case for N = 4

            else if (N_defektov == 4)
            {
                if (up_down == "Upper boundary ")
                {
                    phi0_upper = Math.PI / 4.0;
                }

                else if (up_down == "Lower boundary ")
                {
                    phi0_lower = Math.PI / 4.0;
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        defects[2 * i + j][0] = (2 * i + 1) * (double)Nx / 4.0;
                        defects[2 * i + j][1] = (2 * j + 1) * (double)Ny / 4.0;

                        /*if (i == 0)
                        {
                            defects[2 * i + j][0] += 5.0;
                        }
                        if (i == 1)
                        {
                            defects[2 * i + j][0] -= 5.0;
                        }*/

                        if (i + j == 1)
                        {
                            defects[2 * i + j][2] = 0.5;
                        }
                        else
                        {
                            defects[2 * i + j][2] = -0.5;
                        }
                    }
                }
            }

            #endregion

            #region Special case for N = 3

            else if (N_defektov == 3)
            {
                if (up_down == "Upper boundary ")
                {
                    phi0_upper = 0.0;

                    defects[0][0] = 75.0;
                    defects[0][1] = 50.0;
                    defects[0][2] = -0.5;

                    defects[1][0] = 37.5;
                    defects[1][1] = 71.7;
                    defects[1][2] = -0.5;

                    defects[2][0] = 37.5;
                    defects[2][1] = 28.3;
                    defects[2][2] = -0.5;
                }

                else if (up_down == "Lower boundary ")
                {
                    phi0_lower = 0.0;

                    defects[0][0] = 75.0;
                    defects[0][1] = 50.0;
                    defects[0][2] = -0.5;

                    defects[1][0] = 37.5;
                    defects[1][1] = 71.7;
                    defects[1][2] = -0.5;

                    defects[2][0] = 37.5;
                    defects[2][1] = 28.3;
                    defects[2][2] = -0.5;
                }

            }

            #endregion

            #region Special case for N = 6

            else if (N_defektov == 6)
            {
                if (up_down == "Upper boundary ")
                {
                    phi0_upper = Math.PI / 4.0;
                }

                else if (up_down == "Lower boundary ")
                {
                    phi0_lower = Math.PI / 2.0;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        defects[i][0] = 150.0;
                        defects[i][1] = 75.0;
                    }

                    if (i == 1)
                    {
                        defects[i][0] = 75.0;
                        defects[i][1] = 150.0;
                    }

                    if (i == 2)
                    {
                        defects[i][0] = 150.0;
                        defects[i][1] = 300.0;
                    }

                    if (i == 3)
                    {
                        defects[i][0] = 300.0;
                        defects[i][1] = 150.0;
                    }
                    
                    defects[i][2] = 1.0;
                }

                defects[4][0] = 150.0;
                defects[4][1] = 200.0;

                defects[5][0] = 150.0;
                defects[5][1] = 250.0;

                defects[4][2] = -0.5;
                defects[5][2] = -0.5;
            }

            #endregion

            #region Normal

            else
            {
                for (int i = 0; i < N_defektov; i++)
                {
                    Defect_properties defect_dialog = new Defect_properties();
                    defect_dialog.Text = up_down + "defect " + (i + 1).ToString();

                    if (defect_dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        defects[i][0] = (double)defect_dialog.defekt_x.Value;
                        defects[i][1] = (double)defect_dialog.defekt_y.Value;
                        defects[i][2] = (double)defect_dialog.m_value.Value;

                        if (up_down == "Upper boundary ")
                        {
                            phi0_upper = (double)(Top_phi_numeric.Value);
                            phi0_upper = phi0_upper * Math.PI / 180.0;
                        }

                        else if (up_down == "Lower boundary ")
                        {
                            phi0_lower = (double)(Bottom_phi_numeric.Value);
                            phi0_lower = phi0_lower * Math.PI / 180.0;
                        }
                    }

                    else
                    {
                        MessageBox.Show("Error!");
                    }
                    defect_dialog.Dispose();
                }
            }

            #endregion

            return defects;
        }
        
        /// <summary>
        /// Inicializacija spremenljivk in konstant
        /// </summary>
        static void Init()
        {
            #region Setting up the insert

            #region Initialization

            double direktorx, direktory, direktorz;
            double R_0, R_1, c_x, c_y, c_z, R_ij, RR;

            R_0 = 80.0;
            R_1 = 10.0;

            c_x = Nx / 2;
            c_y = Ny / 2;
            c_z = Nz / 2;

            matter = new int[Nx][][];
            for (int i = 0; i < Nx; i++)
            {
                matter[i] = new int[Ny][];

                for (int j = 0; j < Ny; j++)
                {
                    matter[i][j] = new int[Nz];

                    for (int k = 0; k < Nz; k++)
                    {
                        matter[i][j][k] = 0;
                    }
                }
            }

            #endregion

            int insert_type = 1;

            #region 1 Torus

            if (insert_type == 1)
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            R_ij = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);

                            /*if ((Math.Sqrt(R_ij) - R_0) * (Math.Sqrt(R_ij) - R_0) + (k - c_z) * (k - c_z) < ((R_1 - 3) * (R_1 - 3)))
                            {
                                matter[i][j][k] = 1;
                            }*/

                            if ((Math.Sqrt(R_ij) - R_0) * (Math.Sqrt(R_ij) - R_0) + (k - c_z) * (k - c_z) < (R_1 * R_1))
                            {
                                matter[i][j][k] = 2;
                            }
                        }
                    }
                }

                #region Changing internal values to 1

                int ip, im, jp, jm, kp, km;

                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (matter[i][j][k] == 2)
                            {
                                im = i - 1;
                                ip = i + 1;
                                jm = j - 1;
                                jp = j + 1;
                                km = k - 1;
                                kp = k + 1;

                                #region Periodični robni pogoji

                                if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
                                {
                                    if (im < 0)
                                    {
                                        im = Nx - 1;
                                    }
                                    if (ip == Nx)
                                    {
                                        ip = 0;
                                    }

                                    if (jm < 0)
                                    {
                                        jm = Ny - 1;
                                    }
                                    if (jp == Ny)
                                    {
                                        jp = 0;
                                    }

                                    if (km < 0)
                                    {
                                        km = Nz - 1;
                                    }
                                    if (kp == Nz)
                                    {
                                        kp = 0;
                                    }
                                }

                                #endregion

                                if (matter[ip][j][k] != 0 && matter[im][j][k] != 0 && matter[i][jp][k] != 0 && matter[i][jm][k] != 0 && matter[i][j][kp] != 0 && matter[i][j][km] != 0)
                                {
                                    matter[i][j][k] = 1;
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            
            #endregion

            #region 2 Simulated laser tweezer melting

            if (insert_type == 2)
            {
                int melting_size = 1;

                if (melting_size == 1)
                {
                    int rand_x, rand_y, rand_z;

                    for (int i = 0; i < 5; i++)
                    {
                        rand_x = r.Next(Nx - 1);
                        rand_y = r.Next(Ny - 1);
                        rand_z = r.Next(Nz - 1);

                        matter[rand_x][rand_y][rand_z] = 1;
                    }
                }

                else
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                R_ij = (i - 60 - (Nx / 2)) * (i - 60 - (Nx / 2)) + (j - (Ny / 2)) * (j - (Ny / 2)) + (k - 50) * (k - 50);

                                if (R_ij < melting_size * melting_size)
                                {
                                    matter[i][j][k] = 1;
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region 3 Sphere

            int np_size = 3;
            int n_points = 10;
            int[][] np_c = new int[n_points][];
            for (int i = 0; i < n_points; i++)
            {
                np_c[i] = new int[3];
            }

            if (insert_type == 3)
            {
                if (np_size == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        np_c[i][0] = r.Next(Nx - 1);
                        np_c[i][1] = r.Next(Ny - 1);
                        np_c[i][2] = r.Next(Nz - 1);

                        matter[np_c[i][0]][np_c[i][1]][np_c[i][2]] = 1;
                    }
                }

                else
                {
                    for (int point = 0; point < n_points; point++)
                    {
                        np_c[point][0] = r.Next(Nx - 1);
                        np_c[point][1] = r.Next(Ny - 1);
                        np_c[point][2] = r.Next(Nz - 1);

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    R_ij = (i - np_c[point][0]) * (i - np_c[point][0]) + (j - np_c[point][1]) * (j - np_c[point][1]) + (k - np_c[point][2]) * (k - np_c[point][2]);

                                    if (R_ij < np_size * np_size)
                                    {
                                        matter[i][j][k] = 2;
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 1; i < Nx - 1; i++)
                    {
                        for (int j = 1; j < Ny - 1; j++)
                        {
                            for (int k = 1; k < Nz - 1; k++)
                            {
                                if (matter[i][j][k] == 2)
                                {
                                    if (matter[i - 1][j][k] == 2 && matter[i + 1][j][k] == 2 && matter[i][j - 1][k] == 2 && matter[i][j + 1][k] == 2 && matter[i][j][k - 1] == 2 && matter[i][j][k + 1] == 2)
                                    {
                                        matter[i][j][k] = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Output

            using (StreamWriter writer = new StreamWriter("matter_type.txt", false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,4}", i, j, k, matter[i][j][k]);
                        }
                    }
                }
            }
            
            #endregion

            #endregion

            #region Lege

            double theta, phi;

            for (int i = 0; i < Nx; i++)
            {
                xv[i] = dx * i + Rmi;
            }
            for (int j = 0; j < Ny; j++)
            {
                yv[j] = dy * j;
            }
            for (int k = 0; k < Nz; k++)
            {
                zv[k] = dz * k;
            }

            #endregion

            #region Gauss random setup

            string[] datoteka = File.ReadAllLines("erf_tab.dat");
            string[] data;
            string[] separators = { "\t", " " };

            double[][] errf = new double[datoteka.Length][];
            for (int i = 0; i < errf.Length; i++)
            {
                errf[i] = new double[2];
            }

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                errf[i][0] = double.Parse(data[0]);
                errf[i][1] = double.Parse(data[1]);
            }

            #endregion

            #region Začetni Q v snovi

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        #region Zgornja stranica

                        if (k + 1 == Nz)
                        {
                            switch (upper_boundary)
                            {
                                case 0:  //Defect pattern
                                    theta = Math.PI / 2.0;
                                    phi = phi0_upper;

                                    for (int d = 0; d < defekti_up.Length; d++)
                                    {
                                        phi += defekti_up[d][2] * Math.Atan2(j - defekti_up[d][1], i - defekti_up[d][0]);
                                    }
                                    break;
                                case 1:  //Tangential
                                    theta = Math.PI / 2.0;
                                    phi = phi0_upper;
                                    break;
                                case 2:  //Tangential degenerate
                                    theta = Math.PI / 2.0;
                                    phi = Math.PI * r.NextDouble();
                                    break;
                                case 3:  //Homeotropic
                                    theta = 0.0;
                                    phi = 0.0;
                                    break;
                                default:  //Other
                                    theta = Math.PI / 2.0;
                                    R_ij = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);
                                    R_ij = Math.Sqrt(R_ij);
                                    if (Math.Abs(R_ij) > 30.0 && Math.Abs(R_ij) <= 60.0)
                                    {
                                        phi = phi0_upper + (R_ij - 45.0) * Math.PI / 30;
                                    }
                                    else if (Math.Abs(R_ij) > 60.0 && Math.Abs(R_ij) <= 90.0)
                                    {
                                        phi = phi0_upper - (R_ij - 75.0) * Math.PI / 30;
                                    }
                                    else
                                    {
                                        phi = phi0_upper + Math.PI / 2.0;
                                    }
                                    break;
                            }
                        }

                        #endregion

                        #region Spodnja stranica

                        else if (k == 0)
                        {
                            switch (lower_boundary)
                            {
                                case 0:  //Defect pattern
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                    }
                                    break;
                                case 1:  //Tangential
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;
                                    break;
                                case 2:  //Tangential degenerate
                                    theta = Math.PI / 2.0;
                                    phi = Math.PI * r.NextDouble();
                                    break;
                                default:  //Homeotropic
                                    theta = 0.0;
                                    phi = 0.0;
                                    break;
                            }
                        }

                        #endregion

                        #region Stranske stranice
                        /*
                        else if (i == 0 || i + 1 == Nx)
                        {
                            theta = Math.PI / 2.0;
                            phi = 0.0;

                            for (int d = 0; d < defekti.Length; d++)
                            {
                                phi += defekti[d][2] * Math.Atan2(j - defekti[d][1], i - defekti[d][0]);
                            }
                        }

                        else if (j == 0 || j + 1 == Ny)
                        {
                            theta = Math.PI / 2.0;
                            phi = 0.0;

                            for (int d = 0; d < defekti.Length; d++)
                            {
                                phi += defekti[d][2] * Math.Atan2(j - defekti[d][1], i - defekti[d][0]);
                            }
                        }
                        */
                        #endregion

                        #region Bulk
                        
                        else
                        {
                            switch (bulk)
                            {
                                #region Planarno

                                case 1:
                                    theta = Math.PI / 2.0;
                                    phi = phi0_bulk;
                                    break;

                                #endregion

                                #region Homeotropno

                                case 2:
                                    theta = 0.0;
                                    phi = 0.0;
                                    break;

                                #endregion

                                #region Escaped

                                case 3:
                                    phi = Math.Atan2(j - Ny / 2, i - Nx / 2) + phi0_bulk;// + 0.1 * (0.5 - r.NextDouble());

                                    theta = 2.0 * Math.Atan(Math.Sqrt((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2)) / (Nx / 2));

                                    if (Math.Abs(theta) > (Math.PI / 2.0))
                                    {
                                        theta = Math.PI / 2.0;
                                    }
                                    break;

                                #endregion

                                #region Defekti skozi celotno celico

                                case 4:
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                    }

                                    /*if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) > Nx * Ny / 4)
                                    {
                                        theta = 0.0;
                                    }*/
                                    break;

                                #endregion

                                #region Twist

                                case 5:
                                    theta = Math.PI * k / Nz;
                                    phi = -0.5 * Math.Atan2(j - defekti_down[0][1], i - defekti_down[0][0]);

                                    direktorx = Math.Cos(phi);
                                    direktory = Math.Sin(phi) * Math.Cos(theta);
                                    direktorz = Math.Sin(phi) * Math.Sin(theta);

                                    theta = Math.Acos(direktorz);
                                    phi = Math.Atan2(direktory, direktorx);
                                    break;

                                #endregion

                                #region Double twist

                                case 6:
                                    theta = Math.PI / 2.0;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        RR = Math.Sqrt((i - defekti_down[d][0]) * (i - defekti_down[d][0]) + (j - defekti_down[d][1]) * (j - defekti_down[d][1]));

                                        if (RR < 50.0)
                                        {
                                            theta = 2.0 * Math.Atan(RR / 50.0);
                                        }
                                    }

                                    phi = Math.PI / 2.0;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                    }
                                    break;

                                #endregion

                                #region Other

                                default:
                                    theta = Math.PI * k / Nz;
                                    phi = 0.0;

                                    double rad1 = Math.Sqrt((i - 60 - Nx / 2) * (i - 60 - Nx / 2) + (j - Ny / 2) * (j - Ny / 2));
                                    double rad2 = Math.Sqrt((i + 95 - Nx / 2) * (i + 95 - Nx / 2) + (j - Ny / 2) * (j - Ny / 2));

                                    double h_factor = Math.Abs((double)k - (double)Nz / 2.0) / ((double)Nz / 2.0);

                                    if (i > 10 && i < Nx - 10 && j > 10 && j < Ny - 10)//(rad < R_0 && k > 5 && k < Nz - 5)
                                    {
                                        theta = theta / (1.0 - h_factor) - Math.Atan2(rad1, k - Nz / 2) - Math.Atan2(rad2, k - Nz / 2) / (h_factor);
                                        phi = (Math.Atan2(j - Ny / 2, i - 60 - Nx / 2) - Math.Atan2(j - Ny / 2, i + 95 - Nx / 2)) / (h_factor);
                                    }
                                    break;

                                #endregion
                            }
                        }

                        #endregion

                        #region Insert override

                        bool I_decided_it = true;
                        if (I_decided_it)
                        {
                            if (matter[i][j][k] == 2)
                            {
                                for (int point = 0; point < n_points; point++)
                                {
                                    R_ij = (i - np_c[point][0]) * (i - np_c[point][0]) + (j - np_c[point][1]) * (j - np_c[point][1]) + (k - np_c[point][2]) * (k - np_c[point][2]);
                                    R_ij = Math.Sqrt(R_ij);

                                    if (R_ij < np_size)
                                    {
                                        theta = Math.Atan2(R_ij, k - np_c[point][2]);
                                        phi = Math.Atan2(j - np_c[point][1], i - np_c[point][0]);
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Izračun vrednosti

                        Q1[i][j][k] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                        Q2[i][j][k] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                        Q3[i][j][k] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                        Q4[i][j][k] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                        Q5[i][j][k] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                        Q1_n[i][j][k] = Q1[i][j][k];
                        Q2_n[i][j][k] = Q2[i][j][k];
                        Q3_n[i][j][k] = Q3[i][j][k];
                        Q4_n[i][j][k] = Q4[i][j][k];
                        Q5_n[i][j][k] = Q5[i][j][k];

                        #region Boundary

                        if (k == 0)
                        {
                            Q1_plate[i][j][0] = Q1[i][j][k];
                            Q2_plate[i][j][0] = Q2[i][j][k];
                            Q3_plate[i][j][0] = Q3[i][j][k];
                            Q4_plate[i][j][0] = Q4[i][j][k];
                            Q5_plate[i][j][0] = Q5[i][j][k];
                        }

                        if (k == Nz - 1)
                        {
                            Q1_plate[i][j][1] = Q1[i][j][k];
                            Q2_plate[i][j][1] = Q2[i][j][k];
                            Q3_plate[i][j][1] = Q3[i][j][k];
                            Q4_plate[i][j][1] = Q4[i][j][k];
                            Q5_plate[i][j][1] = Q5[i][j][k];
                        }

                        #endregion

                        #endregion

                        #region Melted insert override

                        bool I_decided_it2 = true;
                        if (I_decided_it2)
                        {
                            if (matter[i][j][k] == 1)
                            {
                                Q1[i][j][k] = 0.0;
                                Q2[i][j][k] = 0.0;
                                Q3[i][j][k] = 0.0;
                                Q4[i][j][k] = 0.0;
                                Q5[i][j][k] = 0.0;

                                Q1_n[i][j][k] = 0.0;
                                Q2_n[i][j][k] = 0.0;
                                Q3_n[i][j][k] = 0.0;
                                Q4_n[i][j][k] = 0.0;
                                Q5_n[i][j][k] = 0.0;
                            }
                        }

                        #endregion

                        #region Izotropni override

                        if (bulk == 0)
                        {
                            if (k > 0 && k + 1 < Nz && matter[i][j][k] != 2) // && i > 0 && i + 1 < Nx && j > 0 && j + 1 < Ny)
                            {
                                Q1[i][j][k] = 0.0;// Random_Gauss(errf, 0.001);
                                Q2[i][j][k] = 0.0;// Random_Gauss(errf, 0.001);
                                Q3[i][j][k] = 0.0;// Random_Gauss(errf, 0.001);
                                Q4[i][j][k] = 0.0;// Random_Gauss(errf, 0.0001);
                                Q5[i][j][k] = 0.0;// Random_Gauss(errf, 0.0001);

                                Q1_n[i][j][k] = 0.0;
                                Q2_n[i][j][k] = 0.0;
                                Q3_n[i][j][k] = 0.0;
                                Q4_n[i][j][k] = 0.0;
                                Q5_n[i][j][k] = 0.0;
                                
                                #region Cilinder v izotropnem stanju

                                if (Math.Sqrt((j - Ny / 2) * (j - Ny / 2) + (i - Nx / 2) * (i - Nx / 2)) < 48)
                                {
                                    Q1[i][j][k] = Random_Gauss(errf, 0.001);
                                    Q2[i][j][k] = Random_Gauss(errf, 0.001);
                                    Q3[i][j][k] = Random_Gauss(errf, 0.001);
                                    Q4[i][j][k] = Random_Gauss(errf, 0.001);
                                    Q5[i][j][k] = Random_Gauss(errf, 0.001);

                                    Q1_n[i][j][k] = 0.0;
                                    Q2_n[i][j][k] = 0.0;
                                    Q3_n[i][j][k] = 0.0;
                                    Q4_n[i][j][k] = 0.0;
                                    Q5_n[i][j][k] = 0.0;
                                }

                                else
                                {
                                    theta = Math.PI / 2.0;
                                    phi = 0.0;

                                    Q1[i][j][k] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                    Q2[i][j][k] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                    Q3[i][j][k] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                    Q4[i][j][k] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                    Q5[i][j][k] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                    Q1_n[i][j][k] = Q1[i][j][k];
                                    Q2_n[i][j][k] = Q2[i][j][k];
                                    Q3_n[i][j][k] = Q3[i][j][k];
                                    Q4_n[i][j][k] = Q4[i][j][k];
                                    Q5_n[i][j][k] = Q5[i][j][k];
                                }

                                #endregion
                            }
                        }

                        #endregion
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Ustvari naključno število po gaussovi porazdelitvi
        /// </summary>
        /// <param name="erf">Array z x in y vrednostmi error funkcije do 3*sigma</param>
        /// <param name="sigma">Širina gaussove porazdelitve</param>
        /// <returns></returns>
        static double Random_Gauss(double[][] erf, double sigma)
        {
            bool found;
            double x1, x2, y1, y2, result;
            double rnd, random;

            x1 = 0.0;
            x2 = 0.0;
            y1 = 0.0;
            y2 = 0.0;
            found = false;
            result = 0.0;

            rnd = r.NextDouble();
            random = 2.0 * Math.Abs(rnd - 0.5);

            for (int i = 1; i < erf.Length; i++)
            {
                x2 = erf[i][0];
                y2 = erf[i][1];

                if (y2 > random)
                {
                    x1 = erf[i - 1][0];
                    y1 = erf[i - 1][1];

                    if (Math.Abs(y2 - y1) < Math.Pow(10.0, -6.0))
                    {
                        result = (x1 + x2) / 2.0;
                    }
                    else
                    {
                        result = x1 + ((x2 - x1) * (random - y1)) / (y2 - y1);
                    }

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                result = x2;
            }

            result = sigma * result;

            if (rnd < 0.5)
            {
                result = -result;
            }

            return result;
        }

        static double[][][][] Magnetic_field(double[][][][] Field_B, int Nx, int Ny, int Nz, double B_size, int mode)
        {
            #region Horseshoe magnet

            if (mode == 0)
            {
                int mag1x, mag1y, mag1z, mag2x, mag2y, mag2z;
                double rx, ry, rz, r, Qmag;

                mag1x = Nx / 2 - 30;
                mag1y = 0;
                mag1z = Nz + 40;

                mag2x = Nx / 2 + 40;
                mag2y = Ny;
                mag2z = Nz + 40;

                Qmag = B_size * 5000.0;

                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            rx = 0;// i - mag1x;
                            ry = j - mag1y;
                            rz = k - mag1z;

                            r = rx * rx + ry * ry + rz * rz;

                            Field_B[i][j][k][0] = (Qmag / r) * rx / Math.Sqrt(r);
                            Field_B[i][j][k][1] = (Qmag / r) * ry / Math.Sqrt(r);
                            Field_B[i][j][k][2] = (Qmag / r) * rz / Math.Sqrt(r);

                            rx = 0;// i - mag2x;
                            ry = j - mag2y;
                            rz = k - mag2z;

                            r = rx * rx + ry * ry + rz * rz;

                            Field_B[i][j][k][0] -= (Qmag / r) * rx / Math.Sqrt(r);
                            Field_B[i][j][k][1] -= (Qmag / r) * ry / Math.Sqrt(r);
                            Field_B[i][j][k][2] -= (Qmag / r) * rz / Math.Sqrt(r);
                        }
                    }
                }
            }

            #endregion

            #region Circle magnet

            if (mode == 1)
            {
                double rx, ry, rz, rxy, r, r0, Qmag, angle;

                Qmag = B_size * 50000.0;
                r0 = 70.0;

                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            rx = i - Nx / 2;
                            ry = j - Ny / 2;
                            rz = k - (Nz);

                            angle = Math.Atan2(ry, rx);
                            rx -= r0 * Math.Cos(angle);
                            ry -= r0 * Math.Sin(angle);

                            rxy = rx * rx + ry * ry;
                            r = rxy + rz * rz;
                            
                            Field_B[i][j][k][0] = (Qmag / r) * rx / Math.Sqrt(r);
                            Field_B[i][j][k][1] = (Qmag / r) * ry / Math.Sqrt(r);
                            Field_B[i][j][k][2] = (Qmag / r) * rz / Math.Sqrt(r);

                            rz = k - (Nz + 20);
                            r = rxy + rz * rz;

                            Field_B[i][j][k][0] -= (Qmag / r) * rx / Math.Sqrt(r);
                            Field_B[i][j][k][1] -= (Qmag / r) * ry / Math.Sqrt(r);
                            Field_B[i][j][k][2] -= (Qmag / r) * rz / Math.Sqrt(r);

                            rx += 2.0 * r0 * Math.Cos(angle);
                            ry += 2.0 * r0 * Math.Sin(angle);

                            rxy = rx * rx + ry * ry;
                            r = rxy + rz * rz;

                            Field_B[i][j][k][0] += (Qmag / r) * rx / Math.Sqrt(r);
                            Field_B[i][j][k][1] += (Qmag / r) * ry / Math.Sqrt(r);
                            Field_B[i][j][k][2] += (Qmag / r) * rz / Math.Sqrt(r);

                            rz = k - (Nz + 25);
                            r = rxy + rz * rz;

                            Field_B[i][j][k][0] -= (Qmag / r) * rx / Math.Sqrt(r);
                            Field_B[i][j][k][1] -= (Qmag / r) * ry / Math.Sqrt(r);
                            Field_B[i][j][k][2] -= (Qmag / r) * rz / Math.Sqrt(r);
                        }
                    }
                }
            }

            #endregion

            return Field_B;
        }

        #endregion


        private void Start_Click(object sender, EventArgs e)
        {
            #region Calculation mode selection
            
            int mode = 0;/*
            Mode_selection Selection = new Mode_selection();

            if (Selection.ShowDialog(this) == DialogResult.OK)
            {
                mode = Selection.mode;
            }
            else
            {
                MessageBox.Show("Error!");
            }
            */
            #endregion

            #region 0 - Standard start

            if (Calculation_selection.SelectedIndex == 0)
            {
                #region Nastavitev vrednosti iz menija

                eps = (double)eps_n.Value;
                itmax = (int)itmax_n.Value;
                kor = (double)kor_n.Value;

                Rmi = (double)Rmi_n.Value;
                Rma = (double)Rma_n.Value;

                a = (double)H_ksi_n.Value;
                t = (double)t_n.Value;
                w = (double)w_n.Value;
                BB = (double)BB_n.Value;

                Ex = (double)Ex_n.Value;
                Ey = (double)Ey_n.Value;
                Ez = (double)Ez_n.Value;
                E_max = (double)E_max_n.Value;
                B_f = (double)B_n.Value;

                for (int i = 0; i < Nx; i++)
                {
                    xv[i] = dx * i + Rmi;
                }
                for (int j = 0; j < Ny; j++)
                {
                    yv[j] = dy * j;
                }
                for (int k = 0; k < Nz; k++)
                {
                    zv[k] = dz * k;
                }

                Nx = (int)Nx_n.Value;
                Ny = (int)Ny_n.Value;
                Nz = (int)Nz_n.Value;

                #endregion

                #region Inicializacija arrayev

                xv = new double[Nx];
                yv = new double[Ny];
                zv = new double[Nz];
                E_system = new double[itmax];

                Q1 = new double[Nx][][];
                Q2 = new double[Nx][][];
                Q3 = new double[Nx][][];
                Q4 = new double[Nx][][];
                Q5 = new double[Nx][][];

                Q1_n = new double[Nx][][];
                Q2_n = new double[Nx][][];
                Q3_n = new double[Nx][][];
                Q4_n = new double[Nx][][];
                Q5_n = new double[Nx][][];

                Q1_plate = new double[Nx][][];
                Q2_plate = new double[Nx][][];
                Q3_plate = new double[Nx][][];
                Q4_plate = new double[Nx][][];
                Q5_plate = new double[Nx][][];

                b2 = new double[Nx][][];
                S = new double[Nx][][];

                E = new double[Nx][][][];
                B = new double[Nx][][][];
                direktor = new double[Nx][][][];

                for (int i = 0; i < Nx; i++)
                {
                    Q1[i] = new double[Ny][];
                    Q2[i] = new double[Ny][];
                    Q3[i] = new double[Ny][];
                    Q4[i] = new double[Ny][];
                    Q5[i] = new double[Ny][];

                    Q1_n[i] = new double[Ny][];
                    Q2_n[i] = new double[Ny][];
                    Q3_n[i] = new double[Ny][];
                    Q4_n[i] = new double[Ny][];
                    Q5_n[i] = new double[Ny][];

                    Q1_plate[i] = new double[Ny][];
                    Q2_plate[i] = new double[Ny][];
                    Q3_plate[i] = new double[Ny][];
                    Q4_plate[i] = new double[Ny][];
                    Q5_plate[i] = new double[Ny][];

                    b2[i] = new double[Ny][];
                    S[i] = new double[Ny][];

                    E[i] = new double[Ny][][];
                    B[i] = new double[Ny][][];
                    direktor[i] = new double[Ny][][];

                    for (int j = 0; j < Ny; j++)
                    {
                        Q1[i][j] = new double[Nz];
                        Q2[i][j] = new double[Nz];
                        Q3[i][j] = new double[Nz];
                        Q4[i][j] = new double[Nz];
                        Q5[i][j] = new double[Nz];

                        Q1_n[i][j] = new double[Nz];
                        Q2_n[i][j] = new double[Nz];
                        Q3_n[i][j] = new double[Nz];
                        Q4_n[i][j] = new double[Nz];
                        Q5_n[i][j] = new double[Nz];

                        Q1_plate[i][j] = new double[2];
                        Q2_plate[i][j] = new double[2];
                        Q3_plate[i][j] = new double[2];
                        Q4_plate[i][j] = new double[2];
                        Q5_plate[i][j] = new double[2];

                        b2[i][j] = new double[Nz];
                        S[i][j] = new double[Nz];

                        E[i][j] = new double[Nz][];
                        B[i][j] = new double[Nz][];
                        direktor[i][j] = new double[Nz][];

                        for (int k = 0; k < Nz; k++)
                        {
                            E[i][j][k] = new double[3];
                            B[i][j][k] = new double[3];
                            direktor[i][j][k] = new double[3];
                        }
                    }
                }

                Parameters1.Add(Q1);
                Parameters1.Add(Q2);
                Parameters1.Add(Q3);
                Parameters1.Add(Q4);
                Parameters1.Add(Q5);

                Parameters2.Add(Q1_n);
                Parameters2.Add(Q2_n);
                Parameters2.Add(Q3_n);
                Parameters2.Add(Q4_n);
                Parameters2.Add(Q5_n);

                #endregion

                #region Ureditev na robu in v celici

                upper_boundary = Top_boundary_box.SelectedIndex;
                if (upper_boundary == 0)
                {
                    defekti_up = Defect_setup(Top_N_numeric, "Upper boundary ");
                }
                if (upper_boundary == 1)
                {
                    phi0_upper = (double)Top_phi_numeric.Value;
                    phi0_upper = phi0_upper * Math.PI / 180.0;
                }

                lower_boundary = Bottom_boundary_box.SelectedIndex;
                if (lower_boundary == 0)
                {
                    defekti_down = Defect_setup(Bottom_N_numeric, "Lower boundary ");
                }
                if (lower_boundary == 1)
                {
                    phi0_lower = (double)Bottom_phi_numeric.Value;
                    phi0_lower = phi0_lower * Math.PI / 180.0;
                }

                bulk = Bulk_box.SelectedIndex;
                if (bulk == 1)
                {
                    phi0_bulk = (double)Bulk_phi_numeric.Value;
                    phi0_bulk = phi0_bulk * Math.PI / 180.0;
                }
                if (bulk == 3)
                {
                    phi0_bulk = (double)Bulk_phi_numeric.Value;
                    phi0_bulk = phi0_bulk * Math.PI / 180.0;
                }

                sides = Sides_box.SelectedIndex;

                #endregion

                #region Električno polje

                if (E_homogeneous.Checked)
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k][0] = Ex;
                                E[i][j][k][1] = Ey;
                                E[i][j][k][2] = Ez;
                            }
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k][0] = Ex * (j + 1) / Ny;
                                E[i][j][k][1] = Ey * (j + 1) / Ny;
                                E[i][j][k][2] = Ez * (j + 1) / Ny;
                            }
                        }
                    }
                }

                #endregion

                #region Magnetno polje

                B = Magnetic_field(B, Nx, Ny, Nz, B_f, 0);

                #endregion

                #region Periodični pogoji

                i_lower = 1;
                i_upper = Nx - 1;

                j_lower = 1;
                j_upper = Ny - 1;

                k_lower = 1;
                k_upper = Nz - 1;

                if (periodic_x.Checked)
                {
                    i_lower = 0;
                    i_upper = Nx;
                }

                if (periodic_y.Checked)
                {
                    j_lower = 0;
                    j_upper = Ny;
                }

                if (periodic_z.Checked)
                {
                    k_lower = 0;
                    k_upper = Nz;
                }

                #endregion

                #region Nastavitev dodatnih parametrov

                if (izracun_za_polja.Checked)
                {
                    run_type = 1;
                }

                dx = (Rma - Rmi) / ((double)Nx - 1.0);
                dy = (Rma - Rmi) / ((double)Ny - 1.0);
                dz = 1.0 / ((double)Nz - 1.0);

                dxy = Math.Sqrt(dx * dx + dy * dy);
                dxz = Math.Sqrt(dx * dx + dz * dz);
                dyz = Math.Sqrt(dy * dy + dz * dz);
                dxyz = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                tt = 1.0 + Math.Sqrt(1.0 - t);
                AA = a * a;
                sb = tt;
                gamma = 1.0;
                dt = 2.0 * Math.Pow(10, -3);

                k1 = 11.1;
                k2 = 6.5;
                k3 = 17.1;

                L1 = 1.0;
                L2 = 4.0 * (k1 - k2) / (k3 + 2.0 * k2 - k1);
                L3 = 2.0 * (k3 - k1) / ((k3 + 2.0 * k2 - k1) * tt);

                L_chiral = 4.0 * Math.PI / a;

                deps = 1.0;
                if (neg_d_eps_n.Checked)
                {
                    deps = -1.0;
                }

                dmu = 1.0;
                if (neg_d_mu_n.Checked)
                {
                    dmu = -1.0;
                }

                if (time_dependent_n.Checked)
                {
                    time_dependent = true;
                }
                else
                {
                    time_dependent = false;
                }

                if (Inserts_n.Checked)
                {
                    inserts = true;
                }
                else
                {
                    inserts = false;
                }

                if (unequal_L_n.Checked)
                {
                    unequal_L = true;
                }
                else
                {
                    unequal_L = false;
                }

                if (Chiral_n.Checked)
                {
                    chiral = true;
                }
                else
                {
                    chiral = false;
                }

                skip1 = (int)XYZ_n1.Value;
                skip2 = (int)XYZ_n2.Value;

                draw_size = pictureBox1.Height;
                fname = 0;

                #endregion

                #region Izpis

                using (StreamWriter writer = new StreamWriter("Parameters.txt", false))
                {
                    if (time_dependent)
                    {
                        writer.WriteLine("Model used: Time dependent");
                    }
                    else
                    {
                        writer.WriteLine("Model used: Time independent");
                    }
                    writer.WriteLine();

                    writer.WriteLine("Nx: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                    writer.WriteLine("a: {0}  Rmi: {1}  Rma: {2}", a, Rmi, Rma);
                    writer.WriteLine("eps: {0}  kor: {1}  t: {2}", eps, kor, t);
                    writer.WriteLine("w: {0}  BB: {1}  itmax: {2}", w, BB, itmax);
                    writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                    writer.WriteLine("deps: {0}  homogeneous: {1} ", deps, E_homogeneous.Checked);
                    writer.WriteLine();

                    writer.WriteLine("robni spodaj: {0}  N defektov: {1}", defekt, N_defektov);
                    for (int i = 0; i < N_defektov; i++)
                    {
                        writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_down[i][2], defekti_down[i][0], defekti_down[i][1]);
                    }
                }

                #endregion

                #region Izračun in prikaz

                progressBar1.Visible = true;
                progressBar1.Maximum = itmax;
                progressBar1.Value = 0;

                Thread th = new Thread(Calculation);
                th.IsBackground = true;
                th.Start();

                th.Join();

                progressBar1.Visible = false;

                trackBar_depth.Visible = true;
                trackBar_depth.Maximum = Nx - 1;
                Pogled.Visible = true;

                #endregion
            }

            #endregion

            #region 1 - Continue

            else if (Calculation_selection.SelectedIndex == 1)
            {
                #region Setting proper reading mode

                draw_size = pictureBox1.Height;
                int read = 0;
                new_boundary = false;

                if (Tensor.Checked)
                {
                    read = 0;
                }

                else if (Director.Checked)
                {
                    read = 1;
                }

                if (E_field.Checked)
                {
                    changemode = 0;
                }
                else if (a_size.Checked)
                {
                    changemode = 1;
                }
                else if (Change_boundary.Checked)
                {
                    changemode = 2;
                }

                new_boundary = Use_different_boundary.Checked;
                reset = Reset_tensor.Checked;

                interpolation = Interpolation_.Checked;
                if (interpolation)
                {
                    factor = (int)Factor_n.Value;
                }

                #endregion

                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    #region Uvoz datoteke

                    #region Pomožni arrayi

                    string[] datoteka = File.ReadAllLines(ofd.FileName);
                    string[] data;
                    string[] separators = { "\t", " " };

                    int[] x_i, y_j, z_k;

                    x_i = new int[datoteka.Length];
                    y_j = new int[datoteka.Length];
                    z_k = new int[datoteka.Length];

                    #endregion

                    #region Postavitev lokacij

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

                    #region Inicializacija arrayev

                    xv = new double[Nx];
                    yv = new double[Ny];
                    zv = new double[Nz];
                    E_system = new double[itmax];
                    matter = new int[Nx][][];

                    Q1 = new double[Nx][][];
                    Q2 = new double[Nx][][];
                    Q3 = new double[Nx][][];
                    Q4 = new double[Nx][][];
                    Q5 = new double[Nx][][];

                    Q1_n = new double[Nx][][];
                    Q2_n = new double[Nx][][];
                    Q3_n = new double[Nx][][];
                    Q4_n = new double[Nx][][];
                    Q5_n = new double[Nx][][];

                    Q1_plate = new double[Nx][][];
                    Q2_plate = new double[Nx][][];
                    Q3_plate = new double[Nx][][];
                    Q4_plate = new double[Nx][][];
                    Q5_plate = new double[Nx][][];

                    b2 = new double[Nx][][];
                    S = new double[Nx][][];

                    E = new double[Nx][][][];
                    B = new double[Nx][][][];
                    direktor = new double[Nx][][][];

                    for (int i = 0; i < Nx; i++)
                    {
                        Q1[i] = new double[Ny][];
                        Q2[i] = new double[Ny][];
                        Q3[i] = new double[Ny][];
                        Q4[i] = new double[Ny][];
                        Q5[i] = new double[Ny][];

                        Q1_n[i] = new double[Ny][];
                        Q2_n[i] = new double[Ny][];
                        Q3_n[i] = new double[Ny][];
                        Q4_n[i] = new double[Ny][];
                        Q5_n[i] = new double[Ny][];

                        Q1_plate[i] = new double[Ny][];
                        Q2_plate[i] = new double[Ny][];
                        Q3_plate[i] = new double[Ny][];
                        Q4_plate[i] = new double[Ny][];
                        Q5_plate[i] = new double[Ny][];

                        b2[i] = new double[Ny][];
                        S[i] = new double[Ny][];

                        matter[i] = new int[Ny][];
                        E[i] = new double[Ny][][];
                        B[i] = new double[Ny][][];
                        direktor[i] = new double[Ny][][];

                        for (int j = 0; j < Ny; j++)
                        {
                            Q1[i][j] = new double[Nz];
                            Q2[i][j] = new double[Nz];
                            Q3[i][j] = new double[Nz];
                            Q4[i][j] = new double[Nz];
                            Q5[i][j] = new double[Nz];

                            Q1_n[i][j] = new double[Nz];
                            Q2_n[i][j] = new double[Nz];
                            Q3_n[i][j] = new double[Nz];
                            Q4_n[i][j] = new double[Nz];
                            Q5_n[i][j] = new double[Nz];

                            Q1_plate[i][j] = new double[2];
                            Q2_plate[i][j] = new double[2];
                            Q3_plate[i][j] = new double[2];
                            Q4_plate[i][j] = new double[2];
                            Q5_plate[i][j] = new double[2];

                            b2[i][j] = new double[Nz];
                            S[i][j] = new double[Nz];

                            matter[i][j] = new int[Nz];
                            E[i][j] = new double[Nz][];
                            B[i][j] = new double[Nz][];
                            direktor[i][j] = new double[Nz][];

                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k] = new double[3];
                                B[i][j][k] = new double[3];
                                direktor[i][j][k] = new double[3];
                            }
                        }
                    }

                    Parameters1.Add(Q1);
                    Parameters1.Add(Q2);
                    Parameters1.Add(Q3);
                    Parameters1.Add(Q4);
                    Parameters1.Add(Q5);

                    Parameters2.Add(Q1_n);
                    Parameters2.Add(Q2_n);
                    Parameters2.Add(Q3_n);
                    Parameters2.Add(Q4_n);
                    Parameters2.Add(Q5_n);

                    #endregion

                    #region Tenzorsko polje

                    if (read == 0)
                    {
                        for (int i = 0; i < datoteka.Length; i++)
                        {
                            data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            Q1[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[3]);
                            Q2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[4]);
                            Q3[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[5]);
                            Q4[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[6]);
                            Q5[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[7]);

                            Q1_n[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[3]);
                            Q2_n[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[4]);
                            Q3_n[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[5]);
                            Q4_n[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[6]);
                            Q5_n[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[7]);

                            b2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[8]);

                            if (z_k[i] == 0)
                            {
                                Q1_plate[x_i[i]][y_j[i]][0] = Q1[x_i[i]][y_j[i]][z_k[i]];
                                Q2_plate[x_i[i]][y_j[i]][0] = Q2[x_i[i]][y_j[i]][z_k[i]];
                                Q3_plate[x_i[i]][y_j[i]][0] = Q3[x_i[i]][y_j[i]][z_k[i]];
                                Q4_plate[x_i[i]][y_j[i]][0] = Q4[x_i[i]][y_j[i]][z_k[i]];
                                Q5_plate[x_i[i]][y_j[i]][0] = Q5[x_i[i]][y_j[i]][z_k[i]];
                            }

                            if (z_k[i] == Nz - 1)
                            {
                                Q1_plate[x_i[i]][y_j[i]][1] = Q1[x_i[i]][y_j[i]][z_k[i]];
                                Q2_plate[x_i[i]][y_j[i]][1] = Q2[x_i[i]][y_j[i]][z_k[i]];
                                Q3_plate[x_i[i]][y_j[i]][1] = Q3[x_i[i]][y_j[i]][z_k[i]];
                                Q4_plate[x_i[i]][y_j[i]][1] = Q4[x_i[i]][y_j[i]][z_k[i]];
                                Q5_plate[x_i[i]][y_j[i]][1] = Q5[x_i[i]][y_j[i]][z_k[i]];
                            }
                        }

                        #region Only for now (not used)
                        /*
                        for (int i = 100; i < Nx / 2; i++)
                        {
                            for (int j = Ny / 2; j < Ny - 100; j++)
                            {
                                if ((Ny - j) > i)
                                {
                                    for (int k = Nz / 2; k < Nz - 5; k++)
                                    {
                                        Q1[i][j][k] = tt * (1.0 / 6.0 - (Math.Cos(Math.PI / 2.0) * Math.Cos(Math.PI / 2.0)) / 2.0);
                                        Q2[i][j][k] = tt * (Math.Sin(Math.PI / 2.0) * Math.Sin(Math.PI / 2.0) * Math.Cos(2.0 * 0.0)) / 2.0;
                                        Q3[i][j][k] = tt * (Math.Sin(Math.PI / 2.0) * Math.Sin(Math.PI / 2.0) * Math.Sin(2.0 * 0.0)) / 2.0;
                                        Q4[i][j][k] = tt * (Math.Sin(Math.PI) * Math.Cos(0.0)) / 2.0;
                                        Q5[i][j][k] = tt * (Math.Sin(Math.PI) * Math.Sin(0.0)) / 2.0;

                                        Q1_n[x_i[i]][y_j[i]][z_k[i]] = Q1[i][j][k];
                                        Q2_n[x_i[i]][y_j[i]][z_k[i]] = Q2[i][j][k];
                                        Q3_n[x_i[i]][y_j[i]][z_k[i]] = Q3[i][j][k];
                                        Q4_n[x_i[i]][y_j[i]][z_k[i]] = Q4[i][j][k];
                                        Q5_n[x_i[i]][y_j[i]][z_k[i]] = Q5[i][j][k];
                                    }
                                }

                                else
                                {
                                    for (int k = 5; k < Nz / 2; k++)
                                    {
                                        Q1[i][j][k] = tt * (1.0 / 6.0 - (Math.Cos(Math.PI / 2.0) * Math.Cos(Math.PI / 2.0)) / 2.0);
                                        Q2[i][j][k] = tt * (Math.Sin(Math.PI / 2.0) * Math.Sin(Math.PI / 2.0) * Math.Cos(2.0 * 0.0)) / 2.0;
                                        Q3[i][j][k] = tt * (Math.Sin(Math.PI / 2.0) * Math.Sin(Math.PI / 2.0) * Math.Sin(2.0 * 0.0)) / 2.0;
                                        Q4[i][j][k] = tt * (Math.Sin(Math.PI) * Math.Cos(0.0)) / 2.0;
                                        Q5[i][j][k] = tt * (Math.Sin(Math.PI) * Math.Sin(0.0)) / 2.0;

                                        Q1_n[x_i[i]][y_j[i]][z_k[i]] = Q1[i][j][k];
                                        Q2_n[x_i[i]][y_j[i]][z_k[i]] = Q2[i][j][k];
                                        Q3_n[x_i[i]][y_j[i]][z_k[i]] = Q3[i][j][k];
                                        Q4_n[x_i[i]][y_j[i]][z_k[i]] = Q4[i][j][k];
                                        Q5_n[x_i[i]][y_j[i]][z_k[i]] = Q5[i][j][k];
                                    }
                                }
                            }
                        }
                        */
                        #endregion
                    }

                    #endregion

                    #region Direktorsko polje

                    if (read == 1)
                    {
                        double kot_phi, kot_theta;

                        for (int i = 0; i < datoteka.Length; i++)
                        {
                            data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            direktor[x_i[i]][y_j[i]][z_k[i]][0] = double.Parse(data[3]);
                            direktor[x_i[i]][y_j[i]][z_k[i]][1] = double.Parse(data[4]);
                            direktor[x_i[i]][y_j[i]][z_k[i]][2] = double.Parse(data[5]);

                            kot_theta = Math.Acos(direktor[x_i[i]][y_j[i]][z_k[i]][2]);
                            kot_phi = Math.Atan2(direktor[x_i[i]][y_j[i]][z_k[i]][1], direktor[x_i[i]][y_j[i]][z_k[i]][0]);

                            if (x_i[i] == 0 || x_i[i] + 1 == Nx || y_j[i] == 0 || y_j[i] + 1 == Ny)
                            {
                                kot_theta = Math.PI / 2.0;
                                kot_phi = Math.Atan2(y_j[i] - Ny / 2, x_i[i] - Nx / 2);
                            }

                            Q1[x_i[i]][y_j[i]][z_k[i]] = tt * (1.0 / 6.0 - (Math.Cos(kot_theta) * Math.Cos(kot_theta)) / 2.0);
                            Q2[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(kot_theta) * Math.Sin(kot_theta) * Math.Cos(2.0 * kot_phi)) / 2.0;
                            Q3[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(kot_theta) * Math.Sin(kot_theta) * Math.Sin(2.0 * kot_phi)) / 2.0;
                            Q4[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(2.0 * kot_theta) * Math.Cos(kot_phi)) / 2.0;
                            Q5[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(2.0 * kot_theta) * Math.Sin(kot_phi)) / 2.0;

                            Q1_n[x_i[i]][y_j[i]][z_k[i]] = Q1[x_i[i]][y_j[i]][z_k[i]];
                            Q2_n[x_i[i]][y_j[i]][z_k[i]] = Q2[x_i[i]][y_j[i]][z_k[i]];
                            Q3_n[x_i[i]][y_j[i]][z_k[i]] = Q3[x_i[i]][y_j[i]][z_k[i]];
                            Q4_n[x_i[i]][y_j[i]][z_k[i]] = Q4[x_i[i]][y_j[i]][z_k[i]];
                            Q5_n[x_i[i]][y_j[i]][z_k[i]] = Q5[x_i[i]][y_j[i]][z_k[i]];
                        }
                        /*
                        for (int i = 1; i < Nx / 2; i++)
                        {
                            for (int j = Ny / 2; j < Ny - 1; j++)
                            {
                                for (int k = 1; k < Nz - 1; k++)
                                {
                                    direktor[i][j][k][0] = -direktor[j][Nx - i][k][1];
                                    direktor[i][j][k][1] = direktor[j][Nx - i][k][0];
                                    direktor[i][j][k][2] = direktor[j][Nx - i][k][2];

                                    kot_theta = Math.Acos(direktor[i][j][k][2]);
                                    kot_phi = Math.Atan2(direktor[i][j][k][1], direktor[i][j][k][0]);
                                    
                                    Q1[i][j][k] = tt * (1.0 / 6.0 - (Math.Cos(kot_theta) * Math.Cos(kot_theta)) / 2.0);
                                    Q2[i][j][k] = tt * (Math.Sin(kot_theta) * Math.Sin(kot_theta) * Math.Cos(2.0 * kot_phi)) / 2.0;
                                    Q3[i][j][k] = tt * (Math.Sin(kot_theta) * Math.Sin(kot_theta) * Math.Sin(2.0 * kot_phi)) / 2.0;
                                    Q4[i][j][k] = tt * (Math.Sin(2.0 * kot_theta) * Math.Cos(kot_phi)) / 2.0;
                                    Q5[i][j][k] = tt * (Math.Sin(2.0 * kot_theta) * Math.Sin(kot_phi)) / 2.0;

                                    Q1_n[i][j][k] = Q1[i][j][k];
                                    Q2_n[i][j][k] = Q2[i][j][k];
                                    Q3_n[i][j][k] = Q3[i][j][k];
                                    Q4_n[i][j][k] = Q4[i][j][k];
                                    Q5_n[i][j][k] = Q5[i][j][k];
                                }
                            }
                        }
                        */
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                if (z_k[i] == 0)
                                {
                                    Q1_plate[i][j][0] = Q1[i][j][0];
                                    Q2_plate[i][j][0] = Q2[i][j][0];
                                    Q3_plate[i][j][0] = Q3[i][j][0];
                                    Q4_plate[i][j][0] = Q4[i][j][0];
                                    Q5_plate[i][j][0] = Q5[i][j][0];
                                }

                                if (z_k[i] == Nz - 1)
                                {
                                    Q1_plate[i][j][1] = Q1[i][j][Nz - 1];
                                    Q2_plate[i][j][1] = Q2[i][j][Nz - 1];
                                    Q3_plate[i][j][1] = Q3[i][j][Nz - 1];
                                    Q4_plate[i][j][1] = Q4[i][j][Nz - 1];
                                    Q5_plate[i][j][1] = Q5[i][j][Nz - 1];
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region Nastavitev parametrov

                    #region Nastavitev parametrov

                    eps = (double)eps_n2.Value;
                    itmax = (int)itmax_n2.Value;
                    kor = (double)kor_n2.Value;

                    Rmi = (double)Rmi_n2.Value;
                    Rma = (double)Rma_n2.Value;

                    a = (double)H_ksi_n2.Value;
                    t = (double)t_n2.Value;
                    w = (double)w_n2.Value;
                    BB = (double)BB_n2.Value;

                    Ex = (double)Ex_n.Value;
                    Ey = (double)Ey_n.Value;
                    Ez = (double)Ez_n.Value;
                    E_max = (double)E_max_n.Value;

                    for (int i = 0; i < Nx; i++)
                    {
                        xv[i] = dx * i + Rmi;
                    }
                    for (int j = 0; j < Ny; j++)
                    {
                        yv[j] = dy * j;
                    }
                    for (int k = 0; k < Nz; k++)
                    {
                        zv[k] = dz * k;
                    }

                    #endregion

                    #region Different boundary

                    if (Use_different_boundary.Checked)
                    {
                        upper_boundary = Top_boundary_box_2.SelectedIndex;
                        if (upper_boundary == 0)
                        {
                            defekti_up = Defect_setup(Top_N_numeric_2, "Upper boundary ");
                        }
                        else if (upper_boundary == 1)
                        {
                            phi0_upper = (double)Top_phi_numeric_2.Value;
                            phi0_upper = phi0_upper * Math.PI / 180.0;
                        }

                        lower_boundary = Bottom_boundary_box_2.SelectedIndex;
                        if (lower_boundary == 0)
                        {
                            defekti_down = Defect_setup(Bottom_N_numeric_2, "Lower boundary ");
                        }
                        else if (lower_boundary == 1)
                        {
                            phi0_lower = (double)Bottom_phi_numeric_2.Value;
                            phi0_lower = phi0_lower * Math.PI / 180.0;
                        }


                    }

                    sides = Sides_box_2.SelectedIndex;

                    #endregion

                    #region Električno polje

                    double radius;

                    if (E_homogeneous.Checked)
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    E[i][j][k][0] = Ex;
                                    E[i][j][k][1] = Ey;
                                    E[i][j][k][2] = Ez;
                                }
                            }
                        }
                    }

                    else
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    //radius = (i - 50) * (i - 50) + (j - 50) * (j - 50);
                                    E[i][j][k][0] = Ex * (i + (Ny - j)) / (Nx + Ny);
                                    E[i][j][k][1] = Ey * (i + (Ny - j)) / (Nx + Ny); // (1 - Math.Sqrt(Math.Abs(1 - 2 * j / Ny)));
                                    E[i][j][k][2] = Ez * (i + (Ny - j)) / (Nx + Ny);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Periodični pogoji

                    i_lower = 1;
                    i_upper = Nx - 1;

                    j_lower = 1;
                    j_upper = Ny - 1;

                    k_lower = 1;
                    k_upper = Nz - 1;

                    if (periodic_x.Checked)
                    {
                        i_lower = 0;
                        i_upper = Nx;
                    }

                    if (periodic_y.Checked)
                    {
                        j_lower = 0;
                        j_upper = Ny;
                    }

                    if (periodic_z.Checked)
                    {
                        k_lower = 0;
                        k_upper = Nz;
                    }

                    #endregion

                    #region Nastavitev dodatnih parametrov

                    if (izracun_za_polja.Checked)
                    {
                        run_type = 1;
                    }

                    dx = (Rma - Rmi) / ((double)Nx - 1.0);
                    dy = (Rma - Rmi) / ((double)Ny - 1.0);
                    dz = 1.0 / ((double)Nz - 1.0);

                    dxy = Math.Sqrt(dx * dx + dy * dy);
                    dxz = Math.Sqrt(dx * dx + dz * dz);
                    dyz = Math.Sqrt(dy * dy + dz * dz);
                    dxyz = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                    tt = 1.0 + Math.Sqrt(1.0 - t);
                    AA = a * a;
                    sb = tt;
                    gamma = 1.0;
                    dt = 2.0 * Math.Pow(10, -3);

                    k1 = 2.5;
                    k2 = 2.0;
                    k3 = 2.49;

                    L1 = 1.0;
                    L2 = 4.0 * (k1 - k2) / (k3 + 2.0 * k2 - k1);
                    L3 = 2.0 * (k3 - k1) / ((k3 + 2.0 * k2 - k1) * tt);

                    deps = 1.0;

                    if (neg_d_eps_n2.Checked)
                    {
                        deps = -1.0;
                    }

                    dmu = -1.0;

                    if (time_dependent_n2.Checked)
                    {
                        time_dependent = true;
                    }
                    else
                    {
                        time_dependent = false;
                    }

                    skip1 = (int)XYZ_n1.Value;
                    skip2 = (int)XYZ_n2.Value;

                    #endregion

                    #region Inserts

                    if (Inserts_n2.Checked)
                    {
                        inserts = true;

                        #region New insert

                        if (New_insert.Checked)
                        {
                            int melting_size = 15;
                            double R_ij;

                            if (melting_size == 1)
                            {
                                matter[Nx / 2][Ny / 2][Nz / 2] = 1;
                            }

                            else
                            {
                                for (int i = 0; i < Nx; i++)
                                {
                                    for (int j = 0; j < Ny; j++)
                                    {
                                        for (int k = 0; k < Nz; k++)
                                        {
                                            R_ij = (i - 4 - (Nx / 2)) * (i - 4 - (Nx / 2)) + (j - (Ny / 2)) * (j - (Ny / 2)) + (k - 50) * (k - 50);

                                            if (R_ij < melting_size * melting_size)
                                            {
                                                matter[i][j][k] = 1;
                                            }
                                        }
                                    }
                                }
                            }

                            #region Old stuff
                            /*
                            OpenFileDialog ofd1 = new OpenFileDialog();

                            ofd1.Filter = "Text files (.txt and .dat)|*.txt; *.dat";
                            if (ofd1.ShowDialog() == DialogResult.OK)
                            {
                                string[] datoteka1 = File.ReadAllLines(ofd1.FileName);
                                string[] data1;
                                string[] separators1 = { "\t", " " };
                                int x, y, z, type;

                                for (int i = 0; i < datoteka1.Length; i++)
                                {
                                    data1 = datoteka1[i].Split(separators1, StringSplitOptions.RemoveEmptyEntries);

                                    x = int.Parse(data1[0]);
                                    y = int.Parse(data1[1]);
                                    z = int.Parse(data1[2]);
                                    type = int.Parse(data1[3]);

                                    matter[x][y][z] = type;
                                }
                            }
                            
                            int melting_size = 4;
                            double R_ij;

                            double hedge_phi, hedge_theta;

                            for (int i = 0; i < Nx; i++)
                            {
                                for (int j = 0; j < Ny; j++)
                                {
                                    for (int k = 0; k < Nz; k++)
                                    {
                                        R_ij = (i - 65) * (i - 65) + (j - 65) * (j - 65) + (k - 60) * (k - 60);

                                        if (R_ij < melting_size * melting_size)
                                        {
                                            matter[i][j][k] = 2;
                                            
                                            R_ij = Math.Sqrt(R_ij);

                                            hedge_theta = Math.Acos((k - 60) / R_ij);
                                            hedge_phi = Math.Atan2((j - 65), (i - 65));

                                            #region Izračun vrednosti

                                            Q1[i][j][k] = tt * (1.0 / 6.0 - (Math.Cos(hedge_theta) * Math.Cos(hedge_theta)) / 2.0);
                                            Q2[i][j][k] = tt * (Math.Sin(hedge_theta) * Math.Sin(hedge_theta) * Math.Cos(2.0 * hedge_phi)) / 2.0;
                                            Q3[i][j][k] = tt * (Math.Sin(hedge_theta) * Math.Sin(hedge_theta) * Math.Sin(2.0 * hedge_phi)) / 2.0;
                                            Q4[i][j][k] = tt * (Math.Sin(2.0 * hedge_theta) * Math.Cos(hedge_phi)) / 2.0;
                                            Q5[i][j][k] = tt * (Math.Sin(2.0 * hedge_theta) * Math.Sin(hedge_phi)) / 2.0;

                                            Q1_n[i][j][k] = Q1[i][j][k];
                                            Q2_n[i][j][k] = Q2[i][j][k];
                                            Q3_n[i][j][k] = Q3[i][j][k];
                                            Q4_n[i][j][k] = Q4[i][j][k];
                                            Q5_n[i][j][k] = Q5[i][j][k];

                                            #endregion
                                        }
                                    }
                                }
                            }
                            */
                            #endregion
                        }

                        #endregion

                        #region Old insert

                        else
                        {
                            OpenFileDialog ofd1 = new OpenFileDialog();

                            ofd1.Filter = "Text files (.txt and .dat)|*.txt; *.dat";
                            if (ofd1.ShowDialog() == DialogResult.OK)
                            {
                                string[] datoteka1 = File.ReadAllLines(ofd1.FileName);
                                string[] data1;
                                string[] separators1 = { "\t", " " };
                                int x, y, z;

                                for (int i = 0; i < datoteka1.Length; i++)
                                {
                                    data1 = datoteka1[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    x = int.Parse(data1[0]);
                                    y = int.Parse(data1[1]);
                                    z = int.Parse(data1[2]);

                                    matter[x][y][z] = int.Parse(data1[3]);
                                }
                            }
                        }

                        #endregion

                        #region Setting the insert properties

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    if (matter[i][j][k] == 1)
                                    {
                                        Q1[i][j][k] = 0.0;
                                        Q2[i][j][k] = 0.0;
                                        Q3[i][j][k] = 0.0;
                                        Q4[i][j][k] = 0.0;
                                        Q5[i][j][k] = 0.0;

                                        Q1_n[i][j][k] = 0.0;
                                        Q2_n[i][j][k] = 0.0;
                                        Q3_n[i][j][k] = 0.0;
                                        Q4_n[i][j][k] = 0.0;
                                        Q5_n[i][j][k] = 0.0;
                                    }
                                }
                            }
                        }

                        #endregion
                    }

                    else
                    {
                        inserts = false;
                    }

                    #endregion

                    draw_size = pictureBox1.Height;
                    fname = 0;

                    #region Nastavitev ponovnih računanj

                    if (Ex_b.Checked)
                    {
                        E_changing = 1;
                    }
                    else if (Ey_b.Checked)
                    {
                        E_changing = 2;
                    }
                    else if (Ez_b.Checked)
                    {
                        E_changing = 3;
                    }
                    else
                    {
                        E_changing = 0;
                    }

                    #endregion

                    #region Interpolacija

                    #region v1
                    /*
                    if (interpolation)
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < (int)(Nz / factor); k++)
                                {
                                    Q1_n[i][j][k * factor] = Q1[i][j][k];
                                    Q2_n[i][j][k * factor] = Q2[i][j][k];
                                    Q3_n[i][j][k * factor] = Q3[i][j][k];
                                    Q4_n[i][j][k * factor] = Q4[i][j][k];
                                    Q5_n[i][j][k * factor] = Q5[i][j][k];

                                    for (int f = 1; f > factor; f++)
                                    {
                                        Q1_n[i][j][k * factor + f] = Q1[i][j][k] + f * (Q1[i][j][k + 1] - Q1[i][j][k]) / factor;
                                        Q2_n[i][j][k * factor + f] = Q2[i][j][k] + f * (Q2[i][j][k + 1] - Q2[i][j][k]) / factor;
                                        Q3_n[i][j][k * factor + f] = Q3[i][j][k] + f * (Q3[i][j][k + 1] - Q3[i][j][k]) / factor;
                                        Q4_n[i][j][k * factor + f] = Q4[i][j][k] + f * (Q4[i][j][k + 1] - Q4[i][j][k]) / factor;
                                        Q5_n[i][j][k * factor + f] = Q5[i][j][k] + f * (Q5[i][j][k + 1] - Q5[i][j][k]) / factor;
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < (int)(Nz / factor); k++)
                                {
                                    Q1[i][j][k] = Q1_n[i][j][k];
                                    Q2[i][j][k] = Q2_n[i][j][k];
                                    Q3[i][j][k] = Q3_n[i][j][k];
                                    Q4[i][j][k] = Q4_n[i][j][k];
                                    Q5[i][j][k] = Q5_n[i][j][k];
                                }
                            }
                        }
                    }
                    */
                    #endregion

                    #region v2
                    /*
                    if (interpolation)
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; (k + 1) < Nz; k++)
                                {
                                    Q1_n[i][j][2 * k] = Q1[i][j][k];
                                    Q2_n[i][j][2 * k] = Q2[i][j][k];
                                    Q3_n[i][j][2 * k] = Q3[i][j][k];
                                    Q4_n[i][j][2 * k] = Q4[i][j][k];
                                    Q5_n[i][j][2 * k] = Q5[i][j][k];

                                    Q1_n[i][j][2 * k + 1] = (Q1[i][j][k + 1] + Q1[i][j][k]) / 2.0;
                                    Q2_n[i][j][2 * k + 1] = (Q2[i][j][k + 1] + Q2[i][j][k]) / 2.0;
                                    Q3_n[i][j][2 * k + 1] = (Q3[i][j][k + 1] + Q3[i][j][k]) / 2.0;
                                    Q4_n[i][j][2 * k + 1] = (Q4[i][j][k + 1] + Q4[i][j][k]) / 2.0;
                                    Q5_n[i][j][2 * k + 1] = (Q5[i][j][k + 1] + Q5[i][j][k]) / 2.0;
                                }

                                Q1_n[i][j][2 * Nz - 1] = Q1[i][j][Nz - 1];
                                Q2_n[i][j][2 * Nz - 1] = Q2[i][j][Nz - 1];
                                Q3_n[i][j][2 * Nz - 1] = Q3[i][j][Nz - 1];
                                Q4_n[i][j][2 * Nz - 1] = Q4[i][j][Nz - 1];
                                Q5_n[i][j][2 * Nz - 1] = Q5[i][j][Nz - 1];
                            }
                        }

                        Nz = 2 * Nz;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    Q1[i][j][k] = Q1_n[i][j][k];
                                    Q2[i][j][k] = Q2_n[i][j][k];
                                    Q3[i][j][k] = Q3_n[i][j][k];
                                    Q4[i][j][k] = Q4_n[i][j][k];
                                    Q5[i][j][k] = Q5_n[i][j][k];
                                }
                            }
                        }
                    }
                    */
                    #endregion

                    #region v3
                    /*
                    if (interpolation)
                    {
                        #region Setup

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; (k + 1) < Nz; k++)
                                {
                                    Q1_n[2 * i][2 * j][2 * k] = Q1[i][j][k];
                                    Q2_n[2 * i][2 * j][2 * k] = Q2[i][j][k];
                                    Q3_n[2 * i][2 * j][2 * k] = Q3[i][j][k];
                                    Q4_n[2 * i][2 * j][2 * k] = Q4[i][j][k];
                                    Q5_n[2 * i][2 * j][2 * k] = Q5[i][j][k];
                                }
                            }
                        }

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                Q1_n[2 * i][2 * j][2 * Nz - 1] = Q1[i][j][Nz - 1];
                                Q2_n[2 * i][2 * j][2 * Nz - 1] = Q2[i][j][Nz - 1];
                                Q3_n[2 * i][2 * j][2 * Nz - 1] = Q3[i][j][Nz - 1];
                                Q4_n[2 * i][2 * j][2 * Nz - 1] = Q4[i][j][Nz - 1];
                                Q5_n[2 * i][2 * j][2 * Nz - 1] = Q5[i][j][Nz - 1];

                                Q1_n[2 * i][2 * Nz - 1][2 * j] = Q1[i][Nz - 1][j];
                                Q2_n[2 * i][2 * Nz - 1][2 * j] = Q2[i][Nz - 1][j];
                                Q3_n[2 * i][2 * Nz - 1][2 * j] = Q3[i][Nz - 1][j];
                                Q4_n[2 * i][2 * Nz - 1][2 * j] = Q4[i][Nz - 1][j];
                                Q5_n[2 * i][2 * Nz - 1][2 * j] = Q5[i][Nz - 1][j];

                                Q1_n[2 * Nz - 1][2 * i][2 * j] = Q1[Nz - 1][i][j];
                                Q2_n[2 * Nz - 1][2 * i][2 * j] = Q2[Nz - 1][i][j];
                                Q3_n[2 * Nz - 1][2 * i][2 * j] = Q3[Nz - 1][i][j];
                                Q4_n[2 * Nz - 1][2 * i][2 * j] = Q4[Nz - 1][i][j];
                                Q5_n[2 * Nz - 1][2 * i][2 * j] = Q5[Nz - 1][i][j];
                            }
                        }

                        #endregion

                        for (int i = 0; i < 2 * Nx; i += 2)
                        {
                            for (int j = 0; j < 2 * Ny; j += 2)
                            {
                                for (int k = 0; k < 2 * (Nz - 1); k += 2)
                                {
                                    Q1_n[i][j][k + 1] = (Q1_n[i][j][k] + Q1_n[i][j][k + 2]) / 2.0;
                                    Q2_n[i][j][k + 1] = (Q2_n[i][j][k] + Q2_n[i][j][k + 2]) / 2.0;
                                    Q3_n[i][j][k + 1] = (Q3_n[i][j][k] + Q3_n[i][j][k + 2]) / 2.0;
                                    Q4_n[i][j][k + 1] = (Q4_n[i][j][k] + Q4_n[i][j][k + 2]) / 2.0;
                                    Q5_n[i][j][k + 1] = (Q5_n[i][j][k] + Q5_n[i][j][k + 2]) / 2.0;
                                }
                            }
                        }
                        Nz = 2 * Nz;
                        for (int i = 0; i < 2 * Nx; i += 2)
                        {
                            for (int j = 0; j < 2 * (Ny - 1); j += 2)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    if (Q1_n[i][j + 1][k] == 0)
                                    {
                                        Q1_n[i][j + 1][k] = (Q1_n[i][j][k] + Q1_n[i][j + 2][k]) / 2.0;
                                        Q2_n[i][j + 1][k] = (Q2_n[i][j][k] + Q2_n[i][j + 2][k]) / 2.0;
                                        Q3_n[i][j + 1][k] = (Q3_n[i][j][k] + Q3_n[i][j + 2][k]) / 2.0;
                                        Q4_n[i][j + 1][k] = (Q4_n[i][j][k] + Q4_n[i][j + 2][k]) / 2.0;
                                        Q5_n[i][j + 1][k] = (Q5_n[i][j][k] + Q5_n[i][j + 2][k]) / 2.0;
                                    }
                                    else
                                    {
                                        Q1_n[i][j + 1][k] = (Q1_n[i][j][k] + Q1_n[i][j + 2][k] + 2.0 * Q1_n[i][j + 1][k]) / 4.0;
                                        Q2_n[i][j + 1][k] = (Q2_n[i][j][k] + Q2_n[i][j + 2][k] + 2.0 * Q2_n[i][j + 1][k]) / 4.0;
                                        Q3_n[i][j + 1][k] = (Q3_n[i][j][k] + Q3_n[i][j + 2][k] + 2.0 * Q3_n[i][j + 1][k]) / 4.0;
                                        Q4_n[i][j + 1][k] = (Q4_n[i][j][k] + Q4_n[i][j + 2][k] + 2.0 * Q4_n[i][j + 1][k]) / 4.0;
                                        Q5_n[i][j + 1][k] = (Q5_n[i][j][k] + Q5_n[i][j + 2][k] + 2.0 * Q5_n[i][j + 1][k]) / 4.0;
                                    }
                                }
                            }
                        }
                        Ny = 2 * Ny;
                        for (int i = 0; i < 2 * (Nx - 1); i += 2)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    if (Q1_n[i + 1][j][k] == 0)
                                    {
                                        Q1_n[i + 1][j][k] = (Q1_n[i][j][k] + Q1_n[i + 2][j][k]) / 2.0;
                                        Q2_n[i + 1][j][k] = (Q2_n[i][j][k] + Q2_n[i + 2][j][k]) / 2.0;
                                        Q3_n[i + 1][j][k] = (Q3_n[i][j][k] + Q3_n[i + 2][j][k]) / 2.0;
                                        Q4_n[i + 1][j][k] = (Q4_n[i][j][k] + Q4_n[i + 2][j][k]) / 2.0;
                                        Q5_n[i + 1][j][k] = (Q5_n[i][j][k] + Q5_n[i + 2][j][k]) / 2.0;
                                    }
                                    else
                                    {
                                        Q1_n[i + 1][j][k] = (Q1_n[i][j][k] + Q1_n[i + 2][j][k] + 2.0 * Q1_n[i + 1][j][k]) / 4.0;
                                        Q2_n[i + 1][j][k] = (Q2_n[i][j][k] + Q2_n[i + 2][j][k] + 2.0 * Q2_n[i + 1][j][k]) / 4.0;
                                        Q3_n[i + 1][j][k] = (Q3_n[i][j][k] + Q3_n[i + 2][j][k] + 2.0 * Q3_n[i + 1][j][k]) / 4.0;
                                        Q4_n[i + 1][j][k] = (Q4_n[i][j][k] + Q4_n[i + 2][j][k] + 2.0 * Q4_n[i + 1][j][k]) / 4.0;
                                        Q5_n[i + 1][j][k] = (Q5_n[i][j][k] + Q5_n[i + 2][j][k] + 2.0 * Q5_n[i + 1][j][k]) / 4.0;
                                    }
                                }
                            }
                        }
                        Nx = 2 * Nx;
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    Q1[i][j][k] = Q1_n[i][j][k];
                                    Q2[i][j][k] = Q2_n[i][j][k];
                                    Q3[i][j][k] = Q3_n[i][j][k];
                                    Q4[i][j][k] = Q4_n[i][j][k];
                                    Q5[i][j][k] = Q5_n[i][j][k];
                                }
                            }
                        }
                    }
                    */
                    #endregion

                    #region v4

                    if (interpolation)
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz / 2; k++)
                                {
                                    Q1_n[i][j][Nz / 2 + k] = Q1[i][j][k];
                                    Q2_n[i][j][Nz / 2 + k] = Q2[i][j][k];
                                    Q3_n[i][j][Nz / 2 + k] = Q3[i][j][k];
                                    Q4_n[i][j][Nz / 2 + k] = Q4[i][j][k];
                                    Q5_n[i][j][Nz / 2 + k] = Q5[i][j][k];

                                    Q1_n[i][j][Nz / 2 - (k + 1)] = Q1[i][j][k];
                                    Q2_n[i][j][Nz / 2 - (k + 1)] = Q2[i][j][k];
                                    Q3_n[i][j][Nz / 2 - (k + 1)] = Q3[i][j][k];
                                    Q4_n[i][j][Nz / 2 - (k + 1)] = -Q4[i][j][k];
                                    Q5_n[i][j][Nz / 2 - (k + 1)] = -Q5[i][j][k];
                                }
                            }
                        }

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    Q1[i][j][k] = Q1_n[i][j][k];
                                    Q2[i][j][k] = Q2_n[i][j][k];
                                    Q3[i][j][k] = Q3_n[i][j][k];
                                    Q4[i][j][k] = Q4_n[i][j][k];
                                    Q5[i][j][k] = Q5_n[i][j][k];

                                }
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region Novi spodnji robni pogoji

                    if (new_boundary)
                    {
                        double theta, phi;
                        double c_x, c_y, R_ij;

                        c_x = Nx / 2;
                        c_y = Ny / 2;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                #region Top boundary conditions

                                switch (upper_boundary)
                                {
                                    case 0:  //Defect pattern
                                        theta = Math.PI / 2.0;
                                        phi = phi0_upper;

                                        for (int d = 0; d < defekti_up.Length; d++)
                                        {
                                            phi += defekti_up[d][2] * Math.Atan2(j - defekti_up[d][1], i - defekti_up[d][0]);
                                        }
                                        break;
                                    case 1:  //Tangential
                                        theta = Math.PI / 2.0;
                                        phi = phi0_upper;
                                        break;
                                    case 2:  //Tangential degenerate
                                        theta = Math.PI / 2.0;
                                        phi = Math.PI * r.NextDouble();
                                        break;
                                    case 3:  //Homeotropic
                                        theta = 0.0;
                                        phi = 0.0;
                                        break;
                                    default:  //Other
                                        theta = Math.PI / 2.0;
                                        R_ij = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);
                                        R_ij = Math.Sqrt(R_ij);
                                        if (Math.Abs(R_ij) > 10.0 && Math.Abs(R_ij) <= 26.0)
                                        {
                                            phi = phi0_upper + (R_ij - 20.0) * Math.PI / 20;
                                        }
                                        else if (Math.Abs(R_ij) > 26.0 && Math.Abs(R_ij) <= 42.0)
                                        {
                                            phi = phi0_upper - (R_ij - 32.0) * Math.PI / 20;
                                        }
                                        else
                                        {
                                            phi = phi0_upper + Math.PI / 2.0;
                                        }
                                        break;
                                }
                                #endregion

                                #region Top value calculation

                                Q1[i][j][Nz - 1] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                Q2[i][j][Nz - 1] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                Q3[i][j][Nz - 1] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                Q4[i][j][Nz - 1] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                Q5[i][j][Nz - 1] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                Q1_plate[i][j][1] = Q1[i][j][Nz - 1];
                                Q2_plate[i][j][1] = Q2[i][j][Nz - 1];
                                Q3_plate[i][j][1] = Q3[i][j][Nz - 1];
                                Q4_plate[i][j][1] = Q4[i][j][Nz - 1];
                                Q5_plate[i][j][1] = Q5[i][j][Nz - 1];

                                #endregion

                                #region Bottom boundary conditions

                                switch (lower_boundary)
                                {
                                    case 0:  //Defect pattern
                                        theta = Math.PI / 2.0;
                                        phi = phi0_lower;

                                        for (int d = 0; d < defekti_down.Length; d++)
                                        {
                                            phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                        }
                                        break;
                                    case 1:  //Tangential
                                        theta = Math.PI / 2.0;
                                        phi = phi0_lower;
                                        break;
                                    case 2:  //Tangential degenerate
                                        theta = Math.PI / 2.0;
                                        phi = Math.PI * r.NextDouble();
                                        break;
                                    case 3:  //Homeotropic
                                        theta = 0.0;
                                        phi = 0.0;
                                        break;
                                    default:  //Other
                                        theta = Math.PI / 2.0;
                                        R_ij = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);
                                        R_ij = Math.Sqrt(R_ij);
                                        if (Math.Abs(R_ij) > 10.0 && Math.Abs(R_ij) <= 30.0)
                                        {
                                            phi = phi0_lower + (R_ij - 20.0) * Math.PI / 20;
                                        }
                                        else if (Math.Abs(R_ij) > 30.0 && Math.Abs(R_ij) <= 50.0)
                                        {
                                            phi = phi0_lower - (R_ij - 40.0) * Math.PI / 20;
                                        }
                                        else
                                        {
                                            phi = phi0_lower + Math.PI / 2.0;
                                        }
                                        break;
                                }

                                #endregion

                                #region Bottom value calculation

                                Q1[i][j][0] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                Q2[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                Q3[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                Q4[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                Q5[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                Q1_plate[i][j][0] = Q1[i][j][0];
                                Q2_plate[i][j][0] = Q2[i][j][0];
                                Q3_plate[i][j][0] = Q3[i][j][0];
                                Q4_plate[i][j][0] = Q4[i][j][0];
                                Q5_plate[i][j][0] = Q5[i][j][0];

                                #endregion
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region Prikaz in računanje

                    progressBar1.Visible = true;
                    progressBar1.Maximum = itmax;
                    progressBar1.Value = 0;

                    Thread th = new Thread(Calculation_continued);
                    th.IsBackground = true;
                    th.Start();

                    th.Join();

                    progressBar1.Visible = false;

                    trackBar_depth.Visible = true;
                    trackBar_depth.Maximum = Nx - 1;
                    Pogled.Visible = true;

                    #endregion
                }
            }

            #endregion

            #region 2 - Direktorsko polje

            else if (Calculation_selection.SelectedIndex == 2)
            {
                #region Inicializacija 

                string[] data, datoteka;
                string[] separators = { "\t", " " };

                int[] x_i, y_j, z_k;
                double f_f, F_f;

                draw_size = pictureBox1.Height;
                N_r = (int)N_ravnine_n.Value;

                #endregion

                #region Standard

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        #region Postavitev lokacij

                        datoteka = File.ReadAllLines(file);
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

                        if (N_r > Nz)
                        {
                            N_r = Nz - 1;
                        }

                        #endregion

                        #region Inicializacija arrayev

                        xv = new double[Nx];
                        yv = new double[Ny];
                        zv = new double[Nz];

                        Q1 = new double[Nx][][];
                        Q2 = new double[Nx][][];
                        Q3 = new double[Nx][][];
                        Q4 = new double[Nx][][];
                        Q5 = new double[Nx][][];

                        b2 = new double[Nx][][];
                        S = new double[Nx][][];

                        direktor = new double[Nx][][][];

                        for (int i = 0; i < Nx; i++)
                        {
                            Q1[i] = new double[Ny][];
                            Q2[i] = new double[Ny][];
                            Q3[i] = new double[Ny][];
                            Q4[i] = new double[Ny][];
                            Q5[i] = new double[Ny][];

                            b2[i] = new double[Ny][];
                            S[i] = new double[Ny][];

                            direktor[i] = new double[Ny][][];

                            for (int j = 0; j < Ny; j++)
                            {
                                Q1[i][j] = new double[Nz];
                                Q2[i][j] = new double[Nz];
                                Q3[i][j] = new double[Nz];
                                Q4[i][j] = new double[Nz];
                                Q5[i][j] = new double[Nz];

                                b2[i][j] = new double[Nz];
                                S[i][j] = new double[Nz];

                                direktor[i][j] = new double[Nz][];

                                for (int k = 0; k < Nz; k++)
                                {
                                    direktor[i][j][k] = new double[3];
                                }
                            }
                        }

                        #endregion

                        #region Določitev vrednosti

                        for (int i = 0; i < datoteka.Length; i++)
                        {
                            data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            Q1[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[3]);
                            Q2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[4]);
                            Q3[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[5]);
                            Q4[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[6]);
                            Q5[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[7]);

                            b2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[8]);
                        }

                        #endregion

                        #region Nastavitev vrednosti iz menija

                        Rmi = (double)Rmi_n3.Value;
                        Rma = (double)Rma_n3.Value;

                        a = (double)H_ksi_n3.Value;
                        t = (double)t_n3.Value;
                        w = (double)w_n3.Value;
                        BB = (double)BB_n3.Value;

                        Ex = (double)Ex_n.Value;
                        Ey = (double)Ey_n.Value;
                        Ez = (double)Ez_n.Value;

                        dx = (Rma - Rmi) / ((double)Nx - 1.0);
                        dy = (Rma - Rmi) / ((double)Ny - 1.0);
                        dz = 1.0 / ((double)Nz - 1.0);

                        tt = 1.0 + Math.Sqrt(1.0 - t);
                        AA = a * a;
                        sb = tt;
                        gamma = 1.0;
                        dt = 5.0 * Math.Pow(10, 0);

                        k1 = 2.5;
                        k2 = 2.0;
                        k3 = 2.49;

                        L1 = 1.0;
                        L2 = 4.0 * (k1 - k2) / (k3 + 2.0 * k2 - k1);
                        L3 = 2.0 * (k3 - k1) / ((k3 + 2.0 * k2 - k1) * tt);

                        deps = 1.0;

                        #endregion

                        #region Controls

                        if (neg_d_eps_n3.Checked)
                        {
                            deps = -1.0;
                        }

                        for (int i = 0; i < Nx; i++)
                        {
                            xv[i] = dx * i + Rmi;
                        }
                        for (int j = 0; j < Ny; j++)
                        {
                            yv[j] = dy * j;
                        }
                        for (int k = 0; k < Nz; k++)
                        {
                            zv[k] = dz * k;
                        }

                        if (xy_plane.Checked)
                        {
                            plane = 0;
                        }
                        else if (xz_plane.Checked)
                        {
                            plane = 1;
                        }
                        if (yz_plane.Checked)
                        {
                            plane = 2;
                        }

                        skip1 = (int)XYZ_n1.Value;
                        skip2 = (int)XYZ_n2.Value;

                        #endregion

                        #region Inserts

                        inserts = inserts_included.Checked;

                        if (inserts)
                        {
                            matter = new int[Nx][][];
                            for (int i = 0; i < Nx; i++)
                            {
                                matter[i] = new int[Ny][];
                                for (int j = 0; j < Ny; j++)
                                {
                                    matter[i][j] = new int[Nz];
                                }
                            }

                            OpenFileDialog ofd1 = new OpenFileDialog();
                            ofd1.Multiselect = false;
                            ofd1.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                            if (ofd1.ShowDialog() == DialogResult.OK)
                            {
                                datoteka = File.ReadAllLines(ofd1.FileName);

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    x_i[i] = int.Parse(data[0]);
                                    y_j[i] = int.Parse(data[1]);
                                    z_k[i] = int.Parse(data[2]);

                                    matter[x_i[i]][y_j[i]][z_k[i]] = int.Parse(data[3]);
                                }
                            }
                        }

                        #endregion

                        #region Periodični pogoji

                        i_lower = 1;
                        i_upper = Nx - 1;

                        j_lower = 1;
                        j_upper = Ny - 1;

                        k_lower = 1;
                        k_upper = Nz - 1;

                        if (periodic_x.Checked)
                        {
                            i_lower = 0;
                            i_upper = Nx;
                        }

                        if (periodic_y.Checked)
                        {
                            j_lower = 0;
                            j_upper = Ny;
                        }

                        if (periodic_z.Checked)
                        {
                            k_lower = 0;
                            k_upper = Nz;
                        }

                        #endregion

                        #region Calculating total energy

                        F_f = 0.0;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                                    F_f += f_f * dx * dy * dz;
                                }
                            }
                        }
                        textBox1.Text = F_f.ToString();

                        #endregion

                        Thread th = new Thread(Angle_calculation);
                        th.IsBackground = true;
                        th.Start();

                        th.Join();
                        fname++;
                    }
                }

                #endregion

                #region Repeating the same many times (not used)
                /*
                string dirr = "D:\\Saša - MR\\Paper\\New results\\Ey0,15\\Secondary_director_fields\\";
                if (!Directory.Exists(dirr))
                {
                    Directory.CreateDirectory(dirr);
                }

                for (int count = 100; count <= 20000; count+=100)
                {
                    #region Pomožni arrayi
                    
                    string path = "D:\\Secondary_results\\rezultat" + count.ToString() + ".txt";
                    datoteka = File.ReadAllLines(path);
                    
                    x_i = new int[datoteka.Length];
                    y_j = new int[datoteka.Length];
                    z_k = new int[datoteka.Length];

                    #endregion

                    #region Postavitev lokacij

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

                    if (N_r > Nz)
                    {
                        N_r = Nz - 1;
                    }

                    #endregion

                    #region Določitev vrednosti

                    for (int i = 0; i < datoteka.Length; i++)
                    {
                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        Q1[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[3]);
                        Q2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[4]);
                        Q3[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[5]);
                        Q4[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[6]);
                        Q5[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[7]);

                        b2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[8]);
                    }

                    #endregion

                    #region Calculating total energy
                    
                    F_f = 0.0;

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                                F_f += f_f * dx * dy * dz;
                            }
                        }
                    }
                    textBox1.Text = F_f.ToString();

                    #endregion

                    #region Calculation

                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);
                    
                    using (StreamWriter writer = new StreamWriter(Path.Combine(dirr, "direktorsko_polje" + count.ToString() + ".txt"), false))
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}", i, j, k, direktor[i][j][k][0], direktor[i][j][k][1], direktor[i][j][k][2]);
                                }
                            }
                        }
                    }

                    #endregion
                }
                */
                #endregion
            }

            #endregion

            #region 3 - Interferometrija

            else if (Calculation_selection.SelectedIndex == 3)
            {
                Interferometrija Interferometry = new Interferometrija();

                if (Interferometry.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Done!");
                }

                else
                {
                    MessageBox.Show("Error!");
                }
                Interferometry.Dispose();
            }

            #endregion

            #region 4 - POVray output

            else if (Calculation_selection.SelectedIndex == 4)
            {
                N_r = (int)POV_n_ravnine.Value;
                zoom_in = Zoom_in.Checked;
                zoom_x = (int)Min_x_n.Value;
                zoom_y = (int)Min_y_n.Value;

                #region (0) 3D view with n and S

                if (POVray_mode.SelectedIndex == 0)
                {
                    #region Drawing director

                    int[][][] d_points = new int[][][] { };

                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        d_points = new int[ofd.FileNames.Length][][];
                        for (int i = 0; i < ofd.FileNames.Length; i++)
                        {
                            d_points[i] = new int[4][];
                            for (int j = 0; j < 4; j++) { d_points[i][j] = new int[4]; }
                        }

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };
                        string added0 = null;
                        int add0;

                        int ii, jj, kk, file_n, factor;
                        double n_i, n_j, n_k, angle_x, angle_y;

                        int[] x_i, y_j, z_k;

                        file_n = 0;
                        factor = 8;

                        string dir = "POVray files";
                        dir = Path.Combine(dir, "3D_n+S");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        if (ofd.FileNames.Length > 1000)
                        {
                            add0 = 3;
                        }

                        else if (ofd.FileNames.Length > 100)
                        {
                            add0 = 2;
                        }

                        else if (ofd.FileNames.Length > 10)
                        {
                            add0 = 1;
                        }

                        else
                        {
                            add0 = 0;
                        }

                        #endregion

                        foreach (string file in ofd.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);

                            #region Size of system

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

                            #region Adding zeros in the name

                            if (add0 > 0)
                            {
                                if (file_n < 10)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 100)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 1; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 1000)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 2; i++)
                                    {
                                        added0 += "0";
                                    }
                                }
                            }

                            #endregion

                            //d_points[file_n] = Find_surface_defects_basic(file_n);
                            //d_points[file_n] = Find_surface_defects(datoteka);

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + added0 + file_n.ToString() + ".pov"), false))
                            {
                                #region Setting up the environment

                                writer.WriteLine("#include \"colors.inc\"");
                                writer.WriteLine("#include \"textures.inc\"");
                                writer.WriteLine("#include \"shapes.inc\"");
                                writer.WriteLine();

                                writer.WriteLine("background { color White }");
                                writer.WriteLine();

                                if (Nx == 200)
                                {
                                    writer.WriteLine("camera {");
                                    writer.WriteLine("  location <150, 150, -150>");
                                    writer.WriteLine("  look_at  <80, 0, 120>");
                                    writer.WriteLine("}");
                                    writer.WriteLine();
                                }
                                else
                                {
                                    writer.WriteLine("camera {");
                                    writer.WriteLine("  location <130, 150, -80>");
                                    writer.WriteLine("  look_at  <50, 30, 50>");
                                    writer.WriteLine("}");
                                    writer.WriteLine();
                                }

                                writer.WriteLine("light_source { <0, 0, -50> color White shadowless");
                                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 2");
                                writer.WriteLine("               adaptive 1 jitter }");
                                writer.WriteLine();

                                writer.WriteLine("Wire_Box(<-3,0,-3>,<{0},{2},{1}>, 0.05, 0)", Nx + 6, Ny + 3, Nz);
                                writer.WriteLine();

                                #endregion

                                #region Writing the objects

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    ii = int.Parse(data[0]);
                                    jj = int.Parse(data[1]);
                                    kk = int.Parse(data[2]);

                                    if (ii % factor == 3 && jj % factor == 3 && (kk == 2 || kk == 50 || kk == 99))
                                    {
                                        n_i = double.Parse(data[3]);
                                        n_j = double.Parse(data[4]);
                                        n_k = double.Parse(data[5]);

                                        angle_x = (180.0 * Math.Acos(Math.Abs(n_k))) / Math.PI;
                                        angle_y = -(180.0 * Math.Atan2(n_j, n_i)) / Math.PI;
                                        if (angle_y < 0.0)
                                        {
                                            angle_y += 180.0;
                                        }

                                        writer.WriteLine("object{");
                                        writer.WriteLine("  Round_Cylinder");
                                        writer.WriteLine("   (<0,-3,0>,<0,3,0>, 1.0, 0.2, 1)");
                                        writer.WriteLine("  texture{ pigment{ color Green}");
                                        writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                        writer.WriteLine("  }");
                                        writer.WriteLine("  rotate<0,0,{0}>", (int)angle_x);
                                        writer.WriteLine("  rotate<0,{0},0>", (int)angle_y);
                                        writer.WriteLine("  translate<{0},{1},{2}>", ii, 1.3 * kk, jj);
                                        writer.WriteLine("}");
                                        writer.WriteLine();
                                    }
                                }

                                #endregion

                                #region S isosurface (not currently used)
                                /*
                                writer.WriteLine("#declare F1 = function {");
                                writer.WriteLine("        pigment {");
                                writer.WriteLine("        density_file df3 \"density_map" + file_n.ToString() + ".df3\" ");
                                writer.WriteLine("                interpolate 1");
                                writer.WriteLine("                translate -0.5");
                                writer.WriteLine("        }");
                                writer.WriteLine("}");
                                writer.WriteLine();


                                writer.WriteLine("isosurface {");
                                writer.WriteLine("   function { 1-F1(x,y,z) .gray }");
                                writer.WriteLine("   contained_by {box{0,20}}");
                                writer.WriteLine("   threshold 0.5");
                                writer.WriteLine("   accuracy 0.1");
                                writer.WriteLine("   max_gradient 100");
                                writer.WriteLine("   scale 1");
                                writer.WriteLine("   pigment {rgb <1,0,0>}");
                                writer.WriteLine("   finish { phong 0.8 }");
                                writer.WriteLine("}");
                                */
                                #endregion
                            }

                            file_n++;
                        }
                    }

                    #endregion

                    #region S drawing (not currently used)
                    /*
                    OpenFileDialog ofd2 = new OpenFileDialog();
                    ofd2.Multiselect = true;
                    ofd2.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd2.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };

                        int file_n = 0;

                        #endregion

                        foreach (string file in ofd2.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);

                            #region Postavitev lokacij

                            int[] x_i, y_j, z_k;
                            double S, S_max;
                            int[,,] S_field;

                            S_max = 0.0;

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

                                S = double.Parse(data[3]);
                                if (S > S_max)
                                {
                                    S_max = S;
                                }
                            }
                            Nx++;
                            Ny++;
                            Nz++;

                            #endregion

                            #region Izpis df3

                            S_field = new int[Nx, Ny, Nz];

                            for (int i = 0; i < datoteka.Length; i++)
                            {
                                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                S = double.Parse(data[3]);
                                if (S < 0.0) { S = 0.0; }

                                S_field[x_i[i], y_j[i], z_k[0]] = (int)(255.0 * (S / S_max));
                            }

                            using (BinaryWriter writer = new BinaryWriter(new FileStream("density_map" + file_n.ToString() + ".df3", FileMode.Create), Encoding.BigEndianUnicode))
                            {
                                char[] header = new char[3];
                                char number;

                                header[0] = (char)Nx;
                                header[1] = (char)Ny;
                                header[2] = (char)Nz;

                                writer.Write(header);

                                for (int k = 0; k < Nz; k++)
                                {
                                    for (int j = 0; j < Ny; j++)
                                    {
                                        for (int i = 0; i < Nx; i++)
                                        {
                                            //writer.WriteLine("{0}", S_field[i, j, k]);

                                            number = (char)(S_field[i, j, k]);

                                            writer.Write(number);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    */
                    #endregion

                    #region Risanje S

                    OpenFileDialog ofd3 = new OpenFileDialog();
                    ofd3.Multiselect = true;
                    ofd3.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd3.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };
                        string added0 = null;
                        int add0;
                        //int[][] d_points;
                        double colour;

                        int file_n = 0;

                        string dir = "POVray files";
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        #region Adding zeroes

                        if (ofd.FileNames.Length > 1000)
                        {
                            add0 = 3;
                        }

                        else if (ofd.FileNames.Length > 100)
                        {
                            add0 = 2;
                        }

                        else if (ofd.FileNames.Length > 10)
                        {
                            add0 = 1;
                        }

                        else
                        {
                            add0 = 0;
                        }

                        #endregion

                        #endregion

                        foreach (string file in ofd3.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);
                            int x_i, y_j, z_k;
                            double[][][] set = POVrayblob_color(datoteka, d_points[file_n]);

                            //d_points = Find_surface_defects(datoteka);
                            //d_points = Find_surface_defects_basic(file_n); //Find currently only works in bulk calculations

                            #region Adding zeros in the name

                            if (add0 > 0)
                            {
                                if (file_n < 10)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 100)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 1; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 1000)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 2; i++)
                                    {
                                        added0 += "0";
                                    }
                                }
                            }

                            #endregion

                            #region Izpis

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + added0 + file_n.ToString() + ".pov"), true))
                            {
                                writer.WriteLine("blob {");
                                writer.WriteLine("  threshold 0.9");

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    x_i = int.Parse(data[0]);
                                    y_j = int.Parse(data[1]);
                                    z_k = int.Parse(data[2]);

                                    //colour = POVrayblob_color(x_i, y_j, z_k, d_points);

                                    writer.WriteLine("  sphere {");
                                    writer.Write("           <{0},{1},{2}>, 2.0, 1.0 pigment ", x_i + 1, 1.3 * z_k, y_j + 1, y_j + 2); //<{0},{1},{3}>,
                                    writer.WriteLine("{{rgb<{0:F2},0,{1:F2}>}}", (1.0 + set[x_i][y_j][z_k]), set[x_i][y_j][z_k]);
                                    writer.WriteLine("         }");
                                }

                                writer.WriteLine("   scale 1");
                                writer.WriteLine("   pigment {rgb <1,0,0>}");
                                writer.WriteLine("   finish { phong 0.8 }");
                                writer.WriteLine("}");
                                writer.WriteLine();
                            }

                            #endregion

                            file_n++;
                        }
                    }

                    #endregion
                }

                #endregion

                #region (1) 3D view with S

                if (POVray_mode.SelectedIndex == 1)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };
                        string added0 = null;
                        int add0;

                        int x_i, y_j, z_k, file_n, value;

                        file_n = 0;

                        string dir = "POVray files";
                        dir = Path.Combine(dir, "3D_S");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        if (ofd.FileNames.Length > 1000)
                        {
                            add0 = 3;
                        }

                        else if (ofd.FileNames.Length > 100)
                        {
                            add0 = 2;
                        }

                        else if (ofd.FileNames.Length > 10)
                        {
                            add0 = 1;
                        }

                        else
                        {
                            add0 = 0;
                        }

                        #endregion

                        foreach (string file in ofd.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);

                            #region Adding zeros in the name

                            if (add0 > 0)
                            {
                                if (file_n < 10)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 100)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 1; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 1000)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 2; i++)
                                    {
                                        added0 += "0";
                                    }
                                }
                            }

                            #endregion

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_pov_script" + added0 + file_n.ToString() + ".pov"), false))
                            {
                                #region Setting up the environment

                                writer.WriteLine("#include \"colors.inc\"");
                                writer.WriteLine("#include \"textures.inc\"");
                                writer.WriteLine("#include \"shapes.inc\"");
                                writer.WriteLine();

                                writer.WriteLine("background { color White }");
                                writer.WriteLine();

                                writer.WriteLine("camera {");
                                writer.WriteLine("  location <130, 130, -80>");
                                writer.WriteLine("  look_at  <50, 30, 50>");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                writer.WriteLine("light_source { <0, 0, -50> color White shadowless");
                                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 2");
                                writer.WriteLine("               adaptive 1 jitter }");
                                writer.WriteLine();

                                writer.WriteLine("Wire_Box(<0,0,0>,<100,100,100>, 0.05, 0)");
                                writer.WriteLine();

                                #endregion

                                #region Izpis

                                writer.WriteLine("blob {");
                                writer.WriteLine("  threshold 0.9");

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    x_i = int.Parse(data[0]);
                                    y_j = int.Parse(data[1]);
                                    z_k = int.Parse(data[2]);

                                    writer.WriteLine("  sphere {");
                                    writer.WriteLine("           <{0},{1},{2}>, 2.5, 1.0", x_i + 1, z_k, y_j + 1, y_j + 2); //<{0},{1},{3}>,
                                    writer.WriteLine("         }");
                                }

                                writer.WriteLine("   scale 1");
                                writer.WriteLine("   pigment {rgb <1,0,0>}");
                                writer.WriteLine("   finish { phong 0.8 }");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                #endregion
                            }

                            #region Second colour
                            /*
                            OpenFileDialog ofd1 = new OpenFileDialog();
                            ofd1.Multiselect = true;
                            ofd1.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                            if (ofd1.ShowDialog() == DialogResult.OK)
                            {
                                datoteka = File.ReadAllLines(ofd1.FileName);

                                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_pov_script" + file_n.ToString() + ".pov"), true))
                                {
                                    #region Izpis

                                    writer.WriteLine("blob {");
                                    writer.WriteLine("  threshold 0.9");

                                    for (int i = 0; i < datoteka.Length; i++)
                                    {
                                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                        x_i = int.Parse(data[0]);
                                        y_j = int.Parse(data[1]);
                                        z_k = int.Parse(data[2]);

                                        value = int.Parse(data[3]);

                                        if (value != 0)
                                        {
                                            writer.WriteLine("  sphere {");
                                            writer.WriteLine("           <{0},{1},{2}>, 4.0, 1.0", x_i + 1, z_k, y_j + 1, y_j + 2); //<{0},{1},{3}>,
                                            writer.WriteLine("         }");
                                        }
                                    }

                                    writer.WriteLine("   scale 1");
                                    writer.WriteLine("   pigment {rgb <0,0,1>}");
                                    writer.WriteLine("   finish { phong 0.8 }");
                                    writer.WriteLine("}");
                                    writer.WriteLine();

                                    #endregion
                                }
                            }
                            */
                            #endregion

                            file_n++;
                        }
                    }
                }

                #endregion

                #region (2) 2D with S and defect origins

                if (POVray_mode.SelectedIndex == 2)
                {
                    OpenFileDialog ofd2 = new OpenFileDialog();
                    ofd2.Multiselect = true;
                    ofd2.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd2.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };

                        string added0 = null;
                        int add0;
                        int file_n = 0;

                        string dir = "POVray files";
                        dir = Path.Combine(dir, "2D_S");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        if (ofd2.FileNames.Length > 1000)
                        {
                            add0 = 3;
                        }

                        else if (ofd2.FileNames.Length > 100)
                        {
                            add0 = 2;
                        }

                        else if (ofd2.FileNames.Length > 10)
                        {
                            add0 = 1;
                        }

                        else
                        {
                            add0 = 0;
                        }

                        #endregion

                        #region Setting up the multiple images script

                        using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_pov_script.ini"), false))
                        {
                            writer.WriteLine("Input_File_Name=S_pov_script.pov");
                            writer.WriteLine();
                            writer.WriteLine("; these are the default values");
                            writer.WriteLine("Initial_Clock=0.000");
                            writer.WriteLine("Final_CLock=1.000");
                            writer.WriteLine("Antialias=On");
                            writer.WriteLine("Antialias_Threshold=0.05");
                            writer.WriteLine();
                            writer.WriteLine("Initial_Frame=0");
                            writer.WriteLine("Final_Frame={0}", ofd2.FileNames.Length - 1);
                            writer.WriteLine();
                            writer.WriteLine("Height=1024");
                            writer.WriteLine("Width=1280");
                        }

                        using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_pov_script.pov"), false))
                        {
                            writer.WriteLine("#include concat(\"S_pov_script\", str(frame_number, -3, 0), \".pov\")");
                        }

                        #endregion

                        #region Izpis

                        foreach (string file in ofd2.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);
                            int x_i, y_j, z_k;

                            #region Adding zeros in the name

                            if (add0 > 0)
                            {
                                if (file_n < 10)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 100)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 1; i++)
                                    {
                                        added0 += "0";
                                    }
                                }

                                else if (file_n < 1000)
                                {
                                    added0 = null;
                                    for (int i = 0; i < add0 - 2; i++)
                                    {
                                        added0 += "0";
                                    }
                                }
                            }

                            #endregion

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_pov_script" + added0 + file_n.ToString() + ".pov"), false))
                            {
                                #region Setting up the environment

                                writer.WriteLine("#include \"colors.inc\"");
                                writer.WriteLine("#include \"textures.inc\"");
                                writer.WriteLine("#include \"shapes.inc\"");
                                writer.WriteLine();

                                writer.WriteLine("background { color White }");
                                writer.WriteLine();

                                writer.WriteLine("camera {");
                                writer.WriteLine("  location <50, 50, -100>");
                                writer.WriteLine("  look_at  <50, 50, 0>");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                writer.WriteLine("light_source { <0, 0, -500> color White shadowless");
                                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 2");
                                writer.WriteLine("               adaptive 1 jitter }");
                                writer.WriteLine();

                                #endregion

                                #region Defect locations

                                /*for (int i = 0; i < 4; i++)
                                {
                                    for (int j = 0; j < 4; j++)
                                    {
                                        if ((i + j) % 2 == 0)
                                        {
                                            writer.WriteLine("  sphere { <0,0,0>, 1.5 scale <1,0.2,1> ");
                                        }
                                        else
                                        {
                                            writer.WriteLine("  torus { 1.0, 0.5 scale <1,0.2,1> ");
                                        }

                                        writer.WriteLine("          texture { pigment{ color rgb <1,0,0>}");
                                        writer.WriteLine("                    finish { reflection 0.05 phong 0.1}");
                                        writer.WriteLine("                  }");
                                        writer.WriteLine("          rotate <90,0,0>");
                                        writer.WriteLine("          translate <{0},{1},-2>", (i + 1) * 40, (j + 1) * 40);
                                        writer.WriteLine("        }");
                                        writer.WriteLine();
                                    }
                                }*/

                                #endregion

                                #region S output

                                writer.WriteLine("blob {");
                                writer.WriteLine("  threshold 0.99");

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    x_i = int.Parse(data[0]);
                                    y_j = int.Parse(data[1]);
                                    z_k = int.Parse(data[2]);

                                    writer.WriteLine("  sphere {");
                                    writer.WriteLine("           <{0},{1},0>, 1.2, 1.0", x_i, y_j);
                                    writer.WriteLine("         }");
                                }

                                writer.WriteLine("   scale 1");
                                writer.WriteLine("   pigment {rgb <0,0,0>}");
                                writer.WriteLine("   finish { phong 0.8 }");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                #endregion

                                file_n++;
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                #region (3) 2D with n: xy

                #region v1

                if (POVray_mode.SelectedIndex == 30)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };

                        int ii, jj, kk, file_n, factor;
                        double n_i, n_j, n_k, angle_y, angle_z;

                        file_n = 0;
                        factor = 8;

                        string dir = "POVray files";
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        #endregion

                        foreach (string file in ofd.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + file_n.ToString() + ".pov"), false))
                            {
                                #region Setting up the environment

                                writer.WriteLine("#include \"colors.inc\"");
                                writer.WriteLine("#include \"textures.inc\"");
                                writer.WriteLine("#include \"shapes.inc\"");
                                writer.WriteLine();

                                writer.WriteLine("background { color White }");
                                writer.WriteLine();

                                writer.WriteLine("camera {");
                                writer.WriteLine("  location <50, 50, -120>");
                                writer.WriteLine("  look_at  <50, 50, 0>");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
                                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
                                writer.WriteLine("               adaptive 1 jitter }");
                                writer.WriteLine();

                                #endregion

                                #region Writing the objects

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    ii = int.Parse(data[0]);
                                    jj = int.Parse(data[1]);
                                    kk = int.Parse(data[2]);

                                    if (ii % factor == 2 && jj % factor == 2 && kk == 50)
                                    {
                                        n_i = double.Parse(data[3]);
                                        n_j = double.Parse(data[4]);
                                        n_k = double.Parse(data[5]);

                                        if (n_i == 0.0 && n_j == 0.0)
                                        {
                                            continue;
                                        }

                                        angle_y = -(180.0 * Math.Atan2(n_j, n_i)) / Math.PI;
                                        //if (angle_y < 0.0)
                                        //{
                                        //    angle_y += 180.0;
                                        //}

                                        angle_z = -(180.0 * Math.Asin(n_k)) / Math.PI;
                                        if (angle_z < 0.0 && angle_y > 90.0)
                                        {
                                            angle_z += 180.0;
                                        }

                                        writer.WriteLine("object{");
                                        writer.WriteLine("  Round_Cylinder");
                                        writer.WriteLine("   (<-4,0,0>,<4,0,0>, 1.3, 0.1, 1)");
                                        writer.WriteLine("  texture{ pigment{ color Green}");
                                        writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                        writer.WriteLine("  }");
                                        writer.WriteLine("  rotate<0,{0},{1}>", (int)angle_z, (int)angle_y);
                                        writer.WriteLine("  translate<{0},{1},0>", ii, jj);
                                        writer.WriteLine("}");
                                        writer.WriteLine();
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                }

                #endregion

                #region v2 (used)

                if (POVray_mode.SelectedIndex == 3)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };
                        string add0;

                        int ii, jj, kk, file_n, factor;
                        double n_i, n_j, n_k, angle_y, angle_z;

                        file_n = 0;
                        factor = 3;

                        string dir = "POVray files";
                        dir = Path.Combine(dir, "2D_n_xy");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        #endregion

                        #region Setting up the multiple images script

                        using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script.ini"), false))
                        {
                            writer.WriteLine("Input_File_Name=n_pov_script{0}_.pov", N_r.ToString());
                            writer.WriteLine();
                            writer.WriteLine("; these are the default values");
                            writer.WriteLine("Initial_Clock=0.000");
                            writer.WriteLine("Final_CLock=1.000");
                            writer.WriteLine("Antialias=On");
                            writer.WriteLine("Antialias_Threshold=0.05");
                            writer.WriteLine();
                            writer.WriteLine("Initial_Frame=0");
                            writer.WriteLine("Final_Frame={0}", ofd.FileNames.Length - 1);
                            writer.WriteLine();
                            writer.WriteLine("Height=1024");
                            writer.WriteLine("Width=1280");
                        }

                        using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + N_r.ToString() + "_.pov"), false))
                        {
                            writer.WriteLine("#include concat(\"n_pov_script{0}_\", str(frame_number, -3, 0), \".pov\")", N_r.ToString());
                        }

                        #endregion

                        foreach (string file in ofd.FileNames)
                        {
                            #region Adding zeros in name

                            add0 = null;
                            if (file_n < 10) { add0 = "00"; }
                            else if (file_n < 100) { add0 = "0"; }
                            else { add0 = null;  }

                            #endregion

                            datoteka = File.ReadAllLines(file);

                            if (zoom_in)
                            {
                                //filename = file.Substring();
                                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + N_r.ToString() + "_" + add0 + file_n.ToString() + ".pov"), false))
                                {
                                    #region Setting up the environment

                                    writer.WriteLine("#include \"colors.inc\"");
                                    writer.WriteLine("#include \"textures.inc\"");
                                    writer.WriteLine("#include \"shapes.inc\"");
                                    writer.WriteLine();

                                    writer.WriteLine("background { color White }");
                                    writer.WriteLine();

                                    writer.WriteLine("camera { orthographic");
                                    writer.WriteLine("  location <{0}, {1}, -35>", zoom_x + 15, zoom_y + 15);
                                    writer.WriteLine("  look_at  <{0}, {1}, 0>", zoom_x + 15, zoom_y + 15);
                                    writer.WriteLine("}");
                                    writer.WriteLine();

                                    writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
                                    writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
                                    writer.WriteLine("               adaptive 1 jitter }");
                                    writer.WriteLine();

                                    #endregion

                                    #region Writing the objects

                                    for (int i = 0; i < datoteka.Length; i++)
                                    {
                                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                        ii = int.Parse(data[0]);
                                        jj = int.Parse(data[1]);
                                        kk = int.Parse(data[2]);

                                        if (ii > zoom_x && ii < zoom_x + 30 && jj > zoom_y && jj < zoom_y + 30 && kk == N_r)
                                        {
                                            n_i = double.Parse(data[3]);
                                            n_j = double.Parse(data[4]);
                                            n_k = double.Parse(data[5]);

                                            if (n_i == 0.0 && n_j == 0.0)
                                            {
                                                continue;
                                            }

                                            angle_y = (180.0 * Math.Atan2(n_j, n_i)) / Math.PI;

                                            angle_z = (180.0 * Math.Asin(n_k)) / Math.PI;
                                            //if (angle_z < 0.0) // && angle_y > 90.0
                                            //{
                                            //    angle_z += 180.0;
                                            //}

                                            writer.WriteLine("object{");
                                            writer.WriteLine("  Round_Cylinder");
                                            writer.WriteLine("   (<-0.5,0,0>,<0.5,0,0>, 0.2, 0.1, 1)");
                                            writer.WriteLine("  texture{ pigment{ color Green}");
                                            writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                            writer.WriteLine("  }");
                                            writer.WriteLine("  rotate<0,{0},{1}>", (int)angle_z, (int)angle_y);
                                            writer.WriteLine("  translate<{0},{1},0>", ii, jj);
                                            writer.WriteLine("}");
                                            writer.WriteLine();
                                        }
                                    }

                                    #endregion
                                }
                            }
                            
                            else
                            {
                                //filename = file.Substring();
                                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + N_r.ToString() + "_" + add0 + file_n.ToString() + ".pov"), false))
                                {
                                    #region Setting up the environment

                                    writer.WriteLine("#include \"colors.inc\"");
                                    writer.WriteLine("#include \"textures.inc\"");
                                    writer.WriteLine("#include \"shapes.inc\"");
                                    writer.WriteLine();

                                    writer.WriteLine("background { color White }");
                                    writer.WriteLine();

                                    writer.WriteLine("camera { orthographic");
                                    writer.WriteLine("  location <50, 50, -120>");
                                    writer.WriteLine("  look_at  <50, 50, 0>");
                                    writer.WriteLine("}");
                                    writer.WriteLine();

                                    writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
                                    writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
                                    writer.WriteLine("               adaptive 1 jitter }");
                                    writer.WriteLine();

                                    #endregion

                                    #region Writing the objects

                                    for (int i = 0; i < datoteka.Length; i++)
                                    {
                                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                        ii = int.Parse(data[0]);
                                        jj = int.Parse(data[1]);
                                        kk = int.Parse(data[2]);

                                        if (ii % factor == 0 && jj % factor == 0 && kk == N_r)
                                        {
                                            n_i = double.Parse(data[3]);
                                            n_j = double.Parse(data[4]);
                                            n_k = double.Parse(data[5]);

                                            if (n_i == 0.0 && n_j == 0.0)
                                            {
                                                continue;
                                            }

                                            angle_y = (180.0 * Math.Atan2(n_j, n_i)) / Math.PI;

                                            angle_z = (180.0 * Math.Asin(n_k)) / Math.PI;
                                            //if (angle_z < 0.0) // && angle_y > 90.0
                                            //{
                                            //    angle_z += 180.0;
                                            //}

                                            writer.WriteLine("object{");
                                            writer.WriteLine("  Round_Cylinder");
                                            writer.WriteLine("   (<-2,0,0>,<2,0,0>, 0.8, 0.1, 1)");
                                            writer.WriteLine("  texture{ pigment{ color Green}");
                                            writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                            writer.WriteLine("  }");
                                            writer.WriteLine("  rotate<0,{0},{1}>", (int)angle_z, (int)angle_y);
                                            writer.WriteLine("  translate<{0},{1},0>", ii, jj);
                                            writer.WriteLine("}");
                                            writer.WriteLine();
                                        }
                                    }

                                    #endregion
                                }
                            }
                            
                            file_n++;
                        }
                    }
                }

                #endregion

                #endregion

                #region (4) 2D with n: xz

                if (POVray_mode.SelectedIndex == 4)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };

                        int ii, jj, kk, file_n, factor;
                        double n_i, n_j, n_k, angle_y, angle_z, angle_3D_correctionx, angle_3D_correctiony;

                        file_n = 0;
                        factor = 4;

                        string dir = "POVray files";
                        dir = Path.Combine(dir, "2D_n_xz");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        #endregion

                        foreach (string file in ofd.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + file_n.ToString() + ".pov"), false))
                            {
                                #region Setting up the environment

                                writer.WriteLine("#include \"colors.inc\"");
                                writer.WriteLine("#include \"textures.inc\"");
                                writer.WriteLine("#include \"shapes.inc\"");
                                writer.WriteLine();

                                writer.WriteLine("background { color White }");
                                writer.WriteLine();

                                writer.WriteLine("camera { orthographic");
                                writer.WriteLine("  location <50, 50, -120>");
                                writer.WriteLine("  look_at  <50, 50, 0>");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
                                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
                                writer.WriteLine("               adaptive 1 jitter }");
                                writer.WriteLine();

                                #endregion

                                #region Writing the objects

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    ii = int.Parse(data[0]);
                                    jj = int.Parse(data[1]);
                                    kk = int.Parse(data[2]);

                                    if (ii % factor == 2 && kk % factor == 2 && jj == N_r)
                                    {
                                        n_i = double.Parse(data[3]);
                                        n_j = double.Parse(data[4]);
                                        n_k = double.Parse(data[5]);

                                        angle_y = (180.0 * Math.Atan2(n_j, n_i)) / Math.PI;
                                        //if (angle_y < 0.0)
                                        //{
                                        //    angle_y += 180.0;
                                        //}

                                        angle_z = (180.0 * Math.Acos(n_k)) / Math.PI;
                                        if (angle_z < 0.0 && angle_y > 90.0)
                                        {
                                            angle_z += 180.0;
                                        }

                                        angle_3D_correctiony = -(180.0 * Math.Atan2(kk - 50, 120)) / Math.PI;
                                        angle_3D_correctionx = (180.0 * Math.Atan2(ii - 50, 120)) / Math.PI;

                                        writer.WriteLine("object{");
                                        writer.WriteLine("  Round_Cylinder");
                                        writer.WriteLine("   (<0,-2,0>,<0,2,0>, 0.8, 0.1, 1)");
                                        if (kk == 2 || kk == 98) { writer.WriteLine("  texture{ pigment{ color Blue}"); }
                                        else { writer.WriteLine("  texture{ pigment{ color Green}"); }
                                        writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                        writer.WriteLine("  }");
                                        writer.WriteLine("  rotate<0,0,{0}>", (int)angle_z);
                                        writer.WriteLine("  rotate<0,{0},0>", (int)angle_y);
                                        //writer.WriteLine("  rotate<{0},{1},0>", (int)angle_3D_correctiony, (int)angle_3D_correctionx);
                                        writer.WriteLine("  translate<{0},{1},0>", ii, kk);
                                        writer.WriteLine("}");
                                        writer.WriteLine();
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                }

                #endregion

                #region (5) 2D with n: yz

                if (POVray_mode.SelectedIndex == 5)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };

                        int ii, jj, kk, file_n, factor;
                        double n_i, n_j, n_k, angle_y, angle_z, angle_3D_correctionx, angle_3D_correctiony;

                        file_n = 0;
                        factor = 6;

                        string dir = "POVray files";
                        dir = Path.Combine(dir, "2D_n_yz");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        #endregion

                        foreach (string file in ofd.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + file_n.ToString() + ".pov"), false))
                            {
                                #region Setting up the environment

                                writer.WriteLine("#include \"colors.inc\"");
                                writer.WriteLine("#include \"textures.inc\"");
                                writer.WriteLine("#include \"shapes.inc\"");
                                writer.WriteLine();

                                writer.WriteLine("background { color White }");
                                writer.WriteLine();

                                writer.WriteLine("camera { orthographic");
                                writer.WriteLine("  location <50, 50, -120>");
                                writer.WriteLine("  look_at  <50, 50, 0>");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
                                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
                                writer.WriteLine("               adaptive 1 jitter }");
                                writer.WriteLine();

                                #endregion

                                #region Writing the objects

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    ii = int.Parse(data[0]);
                                    jj = int.Parse(data[1]);
                                    kk = int.Parse(data[2]);

                                    if (jj % factor == 2 && kk % factor == 2 && ii == N_r)
                                    {
                                        n_i = double.Parse(data[3]);
                                        n_j = double.Parse(data[4]);
                                        n_k = double.Parse(data[5]);

                                        angle_y = (180.0 * Math.Atan2(n_j, n_i)) / Math.PI;
                                        /*if (angle_y < 0.0)
                                        {
                                            angle_y += 180.0;
                                        }*/

                                        angle_z = (180.0 * Math.Acos(n_k)) / Math.PI;
                                        if (angle_z < 0.0)// && angle_y > 90.0)
                                        {
                                            angle_z += 180.0;
                                        }

                                        angle_3D_correctiony = -(180.0 * Math.Atan2(kk - 50, 120)) / Math.PI;
                                        angle_3D_correctionx = (180.0 * Math.Atan2(jj - 50, 120)) / Math.PI;

                                        writer.WriteLine("object{");
                                        writer.WriteLine("  Round_Cylinder");
                                        writer.WriteLine("   (<0,-3,0>,<0,3,0>, 1.0, 0.1, 1)");
                                        if (kk == 2 || kk == 98) { writer.WriteLine("  texture{ pigment{ color Blue}"); }
                                        else { writer.WriteLine("  texture{ pigment{ color Green}"); }
                                        writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                        writer.WriteLine("  }");
                                        //writer.WriteLine("  rotate<0,0,{0}>", (int)angle_z);
                                        writer.WriteLine("  rotate<{0},0,0>", (int)angle_z);
                                        writer.WriteLine("  rotate<0,{0},0>", (int)angle_y);
                                        //writer.WriteLine("  rotate<{0},{1},0>", (int)angle_3D_correctiony, (int)angle_3D_correctionx);
                                        writer.WriteLine("  translate<{0},{1},0>", jj, kk);
                                        writer.WriteLine("}");
                                        writer.WriteLine();
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                }

                #endregion

                #region (6) 3D with intermittent n

                if (POVray_mode.SelectedIndex == 6)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;
                    ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        #region Initialization

                        string[] datoteka, data;
                        string[] separators = { "\t", " " };

                        int ii, jj, kk, file_n, factor;
                        double n_i, n_j, n_k, angle_x, angle_y;

                        file_n = 0;
                        factor = 8;

                        string dir = "POVray files";
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        #endregion

                        foreach (string file in ofd.FileNames)
                        {
                            datoteka = File.ReadAllLines(file);

                            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "n_pov_script" + file_n.ToString() + ".pov"), false))
                            {
                                #region Setting up the environment

                                writer.WriteLine("#include \"colors.inc\"");
                                writer.WriteLine("#include \"textures.inc\"");
                                writer.WriteLine("#include \"shapes.inc\"");
                                writer.WriteLine();

                                writer.WriteLine("background { color White }");
                                writer.WriteLine();

                                writer.WriteLine("camera {");
                                writer.WriteLine("  location <150, 150, -150>");
                                writer.WriteLine("  look_at  <80, 0, 120>");
                                writer.WriteLine("}");
                                writer.WriteLine();

                                writer.WriteLine("light_source { <0, 0, -50> color White shadowless");
                                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 2");
                                writer.WriteLine("               adaptive 1 jitter }");
                                writer.WriteLine();

                                writer.WriteLine("Wire_Box(<-3,0,-3>,<206,100,203>, 0.05, 0)");
                                writer.WriteLine();

                                #endregion

                                #region Writing the objects part 1

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    ii = int.Parse(data[0]);
                                    jj = int.Parse(data[1]);
                                    kk = int.Parse(data[2]);

                                    if (ii % factor == 3 && jj % factor == 3 && (kk == 2 || kk == 99))
                                    {
                                        n_i = double.Parse(data[3]);
                                        n_j = double.Parse(data[4]);
                                        n_k = double.Parse(data[5]);

                                        angle_x = (180.0 * Math.Acos(n_k)) / Math.PI;
                                        angle_y = -(180.0 * Math.Atan2(n_j, n_i)) / Math.PI;
                                        if (angle_y < 0.0)
                                        {
                                            angle_y += 180.0;
                                        }

                                        writer.WriteLine("object{");
                                        writer.WriteLine("  Round_Cylinder");
                                        writer.WriteLine("   (<0,-3,0>,<0,3,0>, 1.0, 0.2, 1)");
                                        writer.WriteLine("  texture{ pigment{ color Blue}");
                                        writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                        writer.WriteLine("  }");
                                        writer.WriteLine("  rotate<0,0,{0}>", (int)angle_x);
                                        writer.WriteLine("  rotate<0,{0},0>", (int)angle_y);
                                        writer.WriteLine("  translate<{0},{1},{2}>", ii, kk, jj);
                                        writer.WriteLine("}");
                                        writer.WriteLine();
                                    }
                                }

                                #endregion

                                #region Writing the objects part 2

                                for (int i = 0; i < datoteka.Length; i++)
                                {
                                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    ii = int.Parse(data[0]);
                                    jj = int.Parse(data[1]);
                                    kk = int.Parse(data[2]);

                                    if (ii % 20 == 5 && jj % 20 == 5 && kk % 20 == 5)
                                    {
                                        n_i = double.Parse(data[3]);
                                        n_j = double.Parse(data[4]);
                                        n_k = double.Parse(data[5]);

                                        angle_x = (180.0 * Math.Acos(n_k)) / Math.PI;
                                        angle_y = -(180.0 * Math.Atan2(n_j, n_i)) / Math.PI;
                                        if (angle_y < 0.0)
                                        {
                                            angle_y += 180.0;
                                        }

                                        writer.WriteLine("object{");
                                        writer.WriteLine("  Round_Cylinder");
                                        writer.WriteLine("   (<0,-3,0>,<0,3,0>, 1.0, 0.2, 1)");
                                        writer.WriteLine("  texture{ pigment{ color Green}");
                                        writer.WriteLine("    finish { reflection 0.05 phong 1}");
                                        writer.WriteLine("  }");
                                        writer.WriteLine("  rotate<0,0,{0}>", (int)angle_x);
                                        writer.WriteLine("  rotate<0,{0},0>", (int)angle_y);
                                        writer.WriteLine("  translate<{0},{1},{2}>", ii, kk, jj);
                                        writer.WriteLine("}");
                                        writer.WriteLine();
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                }

                #endregion

            }

            #endregion

            #region 5 - Special cases (Current: Plotting S and beta mid cell)

            else if (Calculation_selection.SelectedIndex == 50)
            {
                #region Inicializacija 

                string[] data, datoteka;
                string[] separators = { "\t", " " };

                int[] x_i, y_j, z_k;
                double[][] values, values2;

                double v1, v2, v3, v4, qq1, qq2, qq3, qq4, qq5;

                #endregion

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        datoteka = File.ReadAllLines(file);

                        #region Determine size

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

                        #region Prepare 2D array

                        values = new double[Nx][];
                        values2 = new double[4 * Nx][];

                        for (int i = 0; i < Nx; i++)
                        {
                            values[i] = new double[Ny];

                            values2[4 * i] = new double[4 * Ny];
                            values2[4 * i + 1] = new double[4 * Ny];
                            values2[4 * i + 2] = new double[4 * Ny];
                            values2[4 * i + 3] = new double[4 * Ny];
                        }

                        v1 = 0.0;
                        for (int i = 0; i < datoteka.Length; i++)
                        {
                            if (z_k[i] == Nz / 2)
                            {
                                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                qq1 = double.Parse(data[3]);
                                qq2 = double.Parse(data[4]);
                                qq3 = double.Parse(data[5]);
                                qq4 = double.Parse(data[6]);
                                qq5 = double.Parse(data[7]);

                                values[x_i[i]][y_j[i]] = double.Parse(data[8]); //Half_trQ_square(qq1, qq2, qq3, qq4, qq5);
                                                                                //values[x_i[i]][y_j[i]] = Math.Sqrt(2.0 * values[x_i[i]][y_j[i]]);

                                //if (v1 < values[x_i[i]][y_j[i]])
                                //{
                                //v1 = values[x_i[i]][y_j[i]];
                                //}
                            }
                        }

                        /*for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                values[i][j] = values[i][j] / v1;
                            }
                        }*/

                        #endregion

                        #region Interpolation

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                values2[4 * i][4 * j] = values[i][j];
                            }
                        }

                        for (int i = 0; i < Nx - 1; i++)
                        {
                            for (int j = 0; j < Ny - 1; j++)
                            {
                                v1 = values[i][j];
                                v2 = values[i + 1][j];
                                v3 = values[i][j + 1];
                                v4 = values[i + 1][j + 1];

                                for (int ii = 0; ii < 4; ii++)
                                {
                                    for (int jj = 0; jj < 4; jj++)
                                    {
                                        values2[4 * i + ii][4 * j + jj] = ((4 - ii) * (4 - jj) * v1 + (4 - jj) * ii * v2 + (4 - ii) * jj * v3 + ii * jj * v4) / 16;
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Output

                        using (Bitmap bmp = new Bitmap(values2.Length, values2[0].Length))
                        {
                            for (int i = 0; i < 4 * Nx; i++)
                            {
                                for (int j = 0; j < 4 * Ny; j++)
                                {
                                    bmp.SetPixel(i, j, Color.FromArgb((int)(values2[i][j] * 255), (int)(values2[i][j] * 255), (int)(values2[i][j] * 255)));
                                }
                            }

                            bmp.Save("Profile.png", System.Drawing.Imaging.ImageFormat.Png);
                        }

                        #endregion
                    }
                }
            }

            #endregion

            #region 6 - Ordering in E

            else if (Calculation_selection.SelectedIndex == 60)
            {
                #region Inicializacija 

                string[] data, datoteka;
                string[] separators = { "\t", " " };

                int[] x_i, y_j, z_k;

                draw_size = pictureBox1.Height;
                N_r = (int)N_ravnine_n.Value;

                #endregion

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //using (StreamWriter writer = new StreamWriter("Ureditev_v_E.txt", false))  {   }

                    foreach (string file in ofd.FileNames)
                    {
                        #region Postavitev lokacij

                        datoteka = File.ReadAllLines(file);

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

                        if (N_r > Nz)
                        {
                            N_r = Nz - 1;
                        }

                        #endregion

                        #region Določitev vrednosti

                        int ii, jj, kk;
                        double[,,,] SV = new double[Nx, Ny, Nz, 3];

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

                        #region Ureditev sistema v smeri električnega polja

                        using (StreamWriter writer = new StreamWriter("Ureditev_v_E.txt", true))
                        {
                            double en = 0.0;

                            for (int i = 0; i < Nx; i++)
                            {
                                for (int j = 0; j < Ny; j++)
                                {
                                    for (int k = 0; k < Nz; k++)
                                    {
                                        en += (3.0 * SV[i, j, k, 0] * SV[i, j, k, 0] - 1.0) / 2.0;
                                    }
                                }
                            }
                            en = en / (Nx * Ny * Nz);

                            writer.WriteLine("{0,6:F4}", en);
                        }

                        #endregion
                    }
                }
            }

            #endregion

            #region Other

            else if (Calculation_selection.SelectedIndex == 50)
            {
                Nx = 100;
                Ny = 100;
                Nz = 100;

                B = new double[Nx][][][];
                for (int i = 0; i < Nx; i++)
                {
                    B[i] = new double[Ny][][];
                    for (int j = 0; j < Ny; j++)
                    {
                        B[i][j] = new double[Nz][];
                        for (int k = 0; k < Nz; k++)
                        {
                            B[i][j][k] = new double[3];
                        }
                    }
                }

                int mag1x, mag1y, mag1z, mag2x, mag2y, mag2z;
                double rx, ry, rz, r, Qmag;

                mag1x = Nx / 2 - 40;
                mag1y = 0;
                mag1z = Nz + 40;

                mag2x = Nx / 2 + 40;
                mag2y = Ny;
                mag2z = Nz + 40;

                Qmag = 1000.0;

                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            rx = 0;// i - mag1x;
                            ry = j - mag1y;
                            rz = k - mag1z;

                            r = rx * rx + ry * ry + rz * rz;

                            B[i][j][k][0] = (Qmag / r) * rx / Math.Sqrt(r);
                            B[i][j][k][1] = (Qmag / r) * ry / Math.Sqrt(r);
                            B[i][j][k][2] = (Qmag / r) * rz / Math.Sqrt(r);

                            rx = 0;// i - mag2x;
                            ry = j - mag2y;
                            rz = k - mag2z;

                            r = rx * rx + ry * ry + rz * rz;

                            B[i][j][k][0] -= (Qmag / r) * rx / Math.Sqrt(r);
                            B[i][j][k][1] -= (Qmag / r) * ry / Math.Sqrt(r);
                            B[i][j][k][2] -= (Qmag / r) * rz / Math.Sqrt(r);
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter("B.txt", false))
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}", i, j, k, B[i][j][k][0], B[i][j][k][1], B[i][j][k][2]);
                            }
                        }
                    }
                }
            }

            else if (Calculation_selection.SelectedIndex == 5)
            {
                string[] datoteka;
                string[] separators = { "\t", " " };
                int[] data;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    data = new int[ofd.FileNames.Length];
                    int file_i = 0;
                    foreach (string file in ofd.FileNames)
                    {
                        datoteka = File.ReadAllLines(file);
                        data[file_i] = datoteka.Length;
                        file_i++;
                    }
                    file_i = 0;

                    using (StreamWriter writer = new StreamWriter("disclination_length.txt", false))
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            writer.WriteLine("{0,3} {1,5}", 2 * i, data[i]);
                        }
                    }
                }
            }

            #endregion
        }

            
        #region Q simulations

        private void Calculation()
        {
            Init();

            #region Calculation type selection

            if (time_dependent)
            {
                if (inserts)
                {
                    Iteration_v2_inserts();
                }

                else
                {
                    Iteration_dt();
                }
            }
            
            else
            {
                if (unequal_L)
                {
                    if (inserts)
                    {
                        Iteration_unequal_L_inserts();
                    }

                    else
                    {
                        Iteration_unequal_L();
                    }
                }

                else if (inserts)
                {
                    Iteration_inserts();
                }

                else if (chiral)
                {
                    Iteration_chiral();
                }

                else
                {
                    Iteration();
                }
            }

            #endregion

            Color color = Color.FromArgb(0, 0, 0);
            Graphics formGraphics = pictureBox1.CreateGraphics();
            Narisi(color, formGraphics);

            Angle_calculation();

            MessageBox.Show("Completed!");
        }
        
        private void Calculation_continued()
        {
            #region Inicializacija rezerve

            string dir;

            double[][][] Q1_r = new double[Nx][][];
            double[][][] Q2_r = new double[Nx][][];
            double[][][] Q3_r = new double[Nx][][];
            double[][][] Q4_r = new double[Nx][][];
            double[][][] Q5_r = new double[Nx][][];

            for (int i = 0; i < Nx; i++)
            {
                Q1_r[i] = new double[Ny][];
                Q2_r[i] = new double[Ny][];
                Q3_r[i] = new double[Ny][];
                Q4_r[i] = new double[Ny][];
                Q5_r[i] = new double[Ny][];

                for (int j = 0; j < Ny; j++)
                {
                    Q1_r[i][j] = new double[Nz];
                    Q2_r[i][j] = new double[Nz];
                    Q3_r[i][j] = new double[Nz];
                    Q4_r[i][j] = new double[Nz];
                    Q5_r[i][j] = new double[Nz];

                    for (int k = 0; k < Nz; k++)
                    {
                        Q1_r[i][j][k] = Q1[i][j][k];
                        Q2_r[i][j][k] = Q2[i][j][k];
                        Q3_r[i][j][k] = Q3[i][j][k];
                        Q4_r[i][j][k] = Q4[i][j][k];
                        Q5_r[i][j][k] = Q5[i][j][k];
                    }
                }
            }

            #endregion

            #region Increasing E

            if (changemode == 0)
            {
                for (; (Ex + Ey + Ez) <= E_max;)
                {
                    #region Resetting the tensor field

                    if (reset)
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    Q1[i][j][k] = Q1_r[i][j][k];
                                    Q2[i][j][k] = Q2_r[i][j][k];
                                    Q3[i][j][k] = Q3_r[i][j][k];
                                    Q4[i][j][k] = Q4_r[i][j][k];
                                    Q5[i][j][k] = Q5_r[i][j][k];
                                }
                            }
                        }
                    }

                    #endregion

                    #region Novi spodnji robni pogoji
                    /*
                    if (new_boundary)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (j == 0)
                                {
                                    defekti_up[2 * i + j][1] += 1.0;
                                }
                                if (j == 1)
                                {
                                    defekti_up[2 * i + j][1] -= 1.0;
                                }
                            }
                        }

                        double theta, phi;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                #region Nastavitev pogojev

                                if (lower_boundary == 0) // Defect
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                    }
                                }

                                else if (lower_boundary == 1) //Tangential
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;
                                }

                                else if (lower_boundary == 2) //Tangential degenerate
                                {
                                    theta = Math.PI / 2.0;
                                    phi = Math.PI * r.NextDouble();
                                }

                                else // Homeotropic
                                {
                                    theta = 0.0;
                                    phi = 0.0;
                                }

                                #endregion

                                #region Izračun vrednosti

                                Q1[i][j][0] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                Q2[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                Q3[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                Q4[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                Q5[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                Q1_n[i][j][0] = Q1[i][j][0];
                                Q2_n[i][j][0] = Q2[i][j][0];
                                Q3_n[i][j][0] = Q3[i][j][0];
                                Q4_n[i][j][0] = Q4[i][j][0];
                                Q5_n[i][j][0] = Q5[i][j][0];

                                #endregion
                            }
                        }
                    }
                    */
                    #endregion

                    #region Izpis

                    #region Folder

                    dir = "Changing E";

                    if (Ex == 0.0 && Ey == 0.0 && Ez == 0.0)
                    {
                        dir = Path.Combine(dir, "E = 0,0");
                    }
                    else if (Ex != 0.0 && Ey == 0.0 && Ez == 0.0)
                    {
                        dir = Path.Combine(dir, "Parallel to x");
                        dir = Path.Combine(dir, "Ex" + Ex.ToString());
                    }
                    else if (Ex == 0.0 && Ey != 0.0 && Ez == 0.0)
                    {
                        dir = Path.Combine(dir, "Parallel to y");
                        dir = Path.Combine(dir, "Ey" + Ey.ToString());
                    }
                    else if (Ex == 0.0 && Ey == 0.0 && Ez != 0.0)
                    {
                        dir = Path.Combine(dir, "Parallel to z");
                        dir = Path.Combine(dir, "Ez" + Ez.ToString());
                    }
                    else
                    {
                        dir = Path.Combine(dir, "General");
                        dir = Path.Combine(dir, "Ex" + Ex.ToString() + "+Ey" + Ey.ToString() + "+Ez" + Ez.ToString());
                    }

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    #endregion

                    #region Parameters

                    using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Parameters.txt"), false))
                    {
                        if (time_dependent)
                        {
                            writer.WriteLine("Model used: Time dependent");
                        }
                        else
                        {
                            writer.WriteLine("Model used: Time independent");
                        }
                        writer.WriteLine();

                        writer.WriteLine("Nx: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                        writer.WriteLine("a: {0}  Rmi: {1}  Rma: {2}", a, Rmi, Rma);
                        writer.WriteLine("eps: {0}  kor: {1}  t: {2}", eps, kor, t);
                        writer.WriteLine("w: {0}  BB: {1}  itmax: {2}", w, BB, itmax);
                        writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                        writer.WriteLine("deps: {0}  homogeneous: {1} ", deps, E_homogeneous.Checked);
                        writer.WriteLine();
                        
                        writer.WriteLine("interpolation : {0}  reset: {1}  different boundary: {1}", interpolation, reset, new_boundary);
                        if (new_boundary)
                        {
                            writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                            for (int i = 0; i < N_defektov; i++)
                            {
                                writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_down[i][2], defekti_down[i][0], defekti_down[i][1]);
                            }
                        }
                    }

                    #endregion

                    #region Insert

                    using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "matter_type.txt"), false))
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,4}", i, j, k, matter[i][j][k]);
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region Izračun

                    if (time_dependent)
                    {
                        Iteration_dt_dir(dir);
                    }

                    else if (inserts)
                    {
                        Iteration_dir_inserts(dir);
                    }

                    else
                    {
                        Iteration_dir(dir);
                    }

                    Angle_calculation_dir(dir);

                    Izpis_rezultatov(dir);

                    #endregion

                    #region Setting up the next round

                    if (E_changing == 1) { Ex += 0.01; }
                    else if (E_changing == 2) { Ey += 0.01; }
                    else if (E_changing == 3) { Ez += 0.01; }
                    else { break; }

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k][0] = Ex;
                                E[i][j][k][1] = Ey;
                                E[i][j][k][2] = Ez;
                            }
                        }
                    }

                    #endregion
                }
            }

            #endregion

            #region Increasing a

            if (changemode == 1)
            {
                for (int count = 0; count < 5; count++)
                {
                    #region Izpis

                    dir = "Changing a";
                    dir = Path.Combine(dir, "a" + a.ToString());

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Parameters.txt"), false))
                    {
                        if (time_dependent)
                        {
                            writer.WriteLine("Model used: Time dependent");
                        }
                        else
                        {
                            writer.WriteLine("Model used: Time independent");
                        }
                        writer.WriteLine();

                        writer.WriteLine("Nx: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                        writer.WriteLine("a: {0}  Rmi: {1}  Rma: {2}", a, Rmi, Rma);
                        writer.WriteLine("eps: {0}  kor: {1}  t: {2}", eps, kor, t);
                        writer.WriteLine("w: {0}  BB: {1}  itmax: {2}", w, BB, itmax);
                        writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                        writer.WriteLine("deps: {0}  homogeneous: {1} ", deps, E_homogeneous.Checked);
                        writer.WriteLine();

                        writer.WriteLine("interpolation : {0}  reset: {1}  different boundary: {1}", interpolation, reset, new_boundary);
                        if (new_boundary)
                        {
                            writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                            for (int i = 0; i < N_defektov; i++)
                            {
                                writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_down[i][2], defekti_down[i][0], defekti_down[i][1]);
                            }
                        }
                    }

                    #endregion

                    #region Resetting the tensor field

                    if (reset)
                    {
                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    Q1[i][j][k] = Q1_r[i][j][k];
                                    Q2[i][j][k] = Q2_r[i][j][k];
                                    Q3[i][j][k] = Q3_r[i][j][k];
                                    Q4[i][j][k] = Q4_r[i][j][k];
                                    Q5[i][j][k] = Q5_r[i][j][k];
                                }
                            }
                        }
                    }

                    #endregion
                    
                    #region Izračun

                    if (time_dependent)
                    {
                        Iteration_dir(dir);
                    }

                    else
                    {
                        Iteration_dt_dir(dir);
                    }

                    Angle_calculation_dir(dir);

                    Izpis_rezultatov(dir);

                    #endregion

                    #region Setting up the next round

                    a = a - 10.0;
                    Rma = 200.0 / a;

                    #endregion
                }
            }

            #endregion

            #region Rotating E
            /*
            for (double theta = 0; theta < 180.0; theta += 1.0)
            {
                Ex = 0.1 * Math.Cos(theta);
                Ey = 0.1 * Math.Sin(theta);

                #region Izpis

                dir = "Changing E";

                if (Ex == 0.0 && Ey == 0.0 && Ez == 0.0)
                {
                    dir = Path.Combine(dir, "E = 0,0");
                }
                else if (Ex != 0.0 && Ey == 0.0 && Ez == 0.0)
                {
                    dir = Path.Combine(dir, "Parallel to x");
                    dir = Path.Combine(dir, "Ex" + Ex.ToString());
                }
                else if (Ex == 0.0 && Ey != 0.0 && Ez == 0.0)
                {
                    dir = Path.Combine(dir, "Parallel to y");
                    dir = Path.Combine(dir, "Ey" + Ey.ToString());
                }
                else if (Ex == 0.0 && Ey == 0.0 && Ez != 0.0)
                {
                    dir = Path.Combine(dir, "Parallel to z");
                    dir = Path.Combine(dir, "Ez" + Ez.ToString());
                }
                else
                {
                    dir = Path.Combine(dir, "General");
                    dir = Path.Combine(dir, "Ex" + Ex.ToString() + "+Ey" + Ey.ToString() + "+Ez" + Ez.ToString());
                }

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Parameters.txt"), false))
                {
                    writer.WriteLine("eps: {0}  itmax: {1}  kor: {2}", eps, itmax, kor);
                    writer.WriteLine("Nr: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                    writer.WriteLine("Rmi: {0}  Rma: {1}", Rmi, Rma);
                    writer.WriteLine("a: {0}  t: {1}  w: {2}  BB: {3}", a, t, w, BB);
                    writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                    writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                    for (int i = 0; i < N_defektov; i++)
                    {
                        writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti[i][2], defekti[i][0], defekti[i][1]);
                    }
                }

                #endregion

                #region Resetting the tensor field

                if (reset)
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                Q1[i][j][k] = Q1_r[i][j][k];
                                Q2[i][j][k] = Q2_r[i][j][k];
                                Q3[i][j][k] = Q3_r[i][j][k];
                                Q4[i][j][k] = Q4_r[i][j][k];
                                Q5[i][j][k] = Q5_r[i][j][k];
                            }
                        }
                    }
                }

                #endregion

                Iteration_dir(dir);

                Angle_calculation_v2(dir);

                Izpis_rezultatov(dir);
            }
            */
            #endregion

            #region Changing the boundary

            if (changemode == 2)
            {
                int ver = 2;

                #region Turning

                if (ver == 1)
                {
                    for (int count = 2; count <= 120; count += 2)
                    {
                        #region Resetting the tensor field

                        if (reset)
                        {
                            for (int i = 0; i < Nx; i++)
                            {
                                for (int j = 0; j < Ny; j++)
                                {
                                    for (int k = 0; k < Nz; k++)
                                    {
                                        Q1[i][j][k] = Q1_r[i][j][k];
                                        Q2[i][j][k] = Q2_r[i][j][k];
                                        Q3[i][j][k] = Q3_r[i][j][k];
                                        Q4[i][j][k] = Q4_r[i][j][k];
                                        Q5[i][j][k] = Q5_r[i][j][k];
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Novi spodnji robni pogoji

                        double move_x1, move_y1, move_x2, move_y2, move_x3, move_y3;

                        move_x1 = Math.Cos((double)count * Math.PI / 180.0);
                        move_y1 = Math.Sin((double)count * Math.PI / 180.0);

                        move_x2 = Math.Cos((double)(count + 120) * Math.PI / 180.0);
                        move_y2 = Math.Sin((double)(count + 120) * Math.PI / 180.0);

                        move_x3 = Math.Cos((double)(count - 120) * Math.PI / 180.0);
                        move_y3 = Math.Sin((double)(count - 120) * Math.PI / 180.0);

                        defekti_down[0][0] = 50 + (int)(25.0 * move_x1);
                        defekti_down[0][1] = 50 + (int)(25.0 * move_y1);

                        defekti_down[1][0] = 50 + (int)(25.0 * move_x2);
                        defekti_down[1][1] = 50 + (int)(25.0 * move_y2);

                        defekti_down[2][0] = 50 + (int)(25.0 * move_x3);
                        defekti_down[2][1] = 50 + (int)(25.0 * move_y3);

                        phi0_lower += Math.PI / 90.0;
                        /*for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (j == 0)
                                {
                                    defekti_down[2 * i + j][1] += 1.0;
                                }
                                if (j == 1)
                                {
                                    defekti_down[2 * i + j][1] -= 1.0;
                                }
                            }
                        }*/

                        double theta, phi;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                #region Nastavitev pogojev

                                if (lower_boundary == 0) // Defect
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                    }
                                }

                                else if (lower_boundary == 1) //Tangential
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;
                                }

                                else if (lower_boundary == 2) //Tangential degenerate
                                {
                                    theta = Math.PI / 2.0;
                                    phi = Math.PI * r.NextDouble();
                                }

                                else // Homeotropic
                                {
                                    theta = 0.0;
                                    phi = 0.0;
                                }

                                #endregion

                                #region Izračun vrednosti

                                Q1[i][j][0] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                Q2[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                Q3[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                Q4[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                Q5[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                Q1_n[i][j][0] = Q1[i][j][0];
                                Q2_n[i][j][0] = Q2[i][j][0];
                                Q3_n[i][j][0] = Q3[i][j][0];
                                Q4_n[i][j][0] = Q4[i][j][0];
                                Q5_n[i][j][0] = Q5[i][j][0];

                                Q1_plate[i][j][0] = Q1[i][j][0];
                                Q2_plate[i][j][0] = Q2[i][j][0];
                                Q3_plate[i][j][0] = Q3[i][j][0];
                                Q4_plate[i][j][0] = Q4[i][j][0];
                                Q5_plate[i][j][0] = Q5[i][j][0];

                                #endregion
                            }
                        }

                        #endregion

                        #region Izpis

                        dir = "Changing boundary";
                        dir = Path.Combine(dir, "angle" + (count).ToString());

                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Parameters.txt"), false))
                        {
                            if (time_dependent)
                            {
                                writer.WriteLine("Model used: Time dependent");
                            }
                            else
                            {
                                writer.WriteLine("Model used: Time independent");
                            }
                            writer.WriteLine();

                            writer.WriteLine("Nx: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                            writer.WriteLine("a: {0}  Rmi: {1}  Rma: {2}", a, Rmi, Rma);
                            writer.WriteLine("eps: {0}  kor: {1}  t: {2}", eps, kor, t);
                            writer.WriteLine("w: {0}  BB: {1}  itmax: {2}", w, BB, itmax);
                            writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                            writer.WriteLine("deps: {0}  homogeneous: {1} ", deps, E_homogeneous.Checked);
                            writer.WriteLine();

                            writer.WriteLine("interpolation : {0}  reset: {1}  different boundary: {1}", interpolation, reset, new_boundary);
                            if (new_boundary)
                            {
                                writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                                for (int i = 0; i < N_defektov; i++)
                                {
                                    writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_down[i][2], defekti_down[i][0], defekti_down[i][1]);
                                }
                            }
                        }

                        #endregion

                        #region Izračun

                        if (time_dependent)
                        {
                            Iteration_dt_dir(dir);
                        }

                        else
                        {
                            Iteration_dir(dir);
                        }

                        Angle_calculation_dir(dir);

                        Izpis_rezultatov(dir);

                        #endregion
                    }
                }

                #endregion

                #region Reducing loop distance

                if (ver == 2)
                {
                    moire_r1 = 60;
                    for (moire_r2 = 30; moire_r2 > 0; moire_r2--)
                    {
                        #region Resetting the tensor field

                        if (reset)
                        {
                            for (int i = 0; i < Nx; i++)
                            {
                                for (int j = 0; j < Ny; j++)
                                {
                                    for (int k = 0; k < Nz; k++)
                                    {
                                        Q1[i][j][k] = Q1_r[i][j][k];
                                        Q2[i][j][k] = Q2_r[i][j][k];
                                        Q3[i][j][k] = Q3_r[i][j][k];
                                        Q4[i][j][k] = Q4_r[i][j][k];
                                        Q5[i][j][k] = Q5_r[i][j][k];
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Novi zgornji robni pogoji

                        double c_x, c_y, c_z, R_ij, theta, phi, c_1, c_2;
                        theta = Math.PI / 2.0;

                        c_x = Nx / 2;
                        c_y = Ny / 2;
                        c_z = Nz / 2;
                        c_1 = moire_r1 - moire_r2 / 2;
                        c_2 = moire_r1 + moire_r2 / 2;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                #region Nastavitev pogojev

                                R_ij = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);
                                R_ij = Math.Sqrt(R_ij);
                                if (Math.Abs(R_ij) > (moire_r1 - moire_r2) && Math.Abs(R_ij) <= moire_r1)
                                {
                                    phi = phi0_upper + (R_ij - c_1) * Math.PI / moire_r2;
                                }
                                else if (Math.Abs(R_ij) > moire_r1 && Math.Abs(R_ij) <= (moire_r1 + moire_r2))
                                {
                                    phi = phi0_upper - (R_ij - c_2) * Math.PI / moire_r2;
                                }
                                else
                                {
                                    phi = phi0_upper + Math.PI / 2.0;
                                }

                                #endregion

                                #region Izračun vrednosti

                                Q1[i][j][Nz - 1] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                Q2[i][j][Nz - 1] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                Q3[i][j][Nz - 1] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                Q4[i][j][Nz - 1] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                Q5[i][j][Nz - 1] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                Q1_n[i][j][Nz - 1] = Q1[i][j][Nz - 1];
                                Q2_n[i][j][Nz - 1] = Q2[i][j][Nz - 1];
                                Q3_n[i][j][Nz - 1] = Q3[i][j][Nz - 1];
                                Q4_n[i][j][Nz - 1] = Q4[i][j][Nz - 1];
                                Q5_n[i][j][Nz - 1] = Q5[i][j][Nz - 1];

                                Q1_plate[i][j][1] = Q1[i][j][Nz - 1];
                                Q2_plate[i][j][1] = Q2[i][j][Nz - 1];
                                Q3_plate[i][j][1] = Q3[i][j][Nz - 1];
                                Q4_plate[i][j][1] = Q4[i][j][Nz - 1];
                                Q5_plate[i][j][1] = Q5[i][j][Nz - 1];

                                #endregion
                            }
                        }

                        #endregion

                        #region Izpis

                        dir = "Changing boundary";
                        dir = Path.Combine(dir, "distance" + (moire_r2).ToString());

                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Parameters.txt"), false))
                        {
                            if (time_dependent)
                            {
                                writer.WriteLine("Model used: Time dependent");
                            }
                            else
                            {
                                writer.WriteLine("Model used: Time independent");
                            }
                            writer.WriteLine();

                            writer.WriteLine("Nx: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                            writer.WriteLine("a: {0}  Rmi: {1}  Rma: {2}", a, Rmi, Rma);
                            writer.WriteLine("eps: {0}  kor: {1}  t: {2}", eps, kor, t);
                            writer.WriteLine("w: {0}  BB: {1}  itmax: {2}", w, BB, itmax);
                            writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                            writer.WriteLine("deps: {0}  homogeneous: {1} ", deps, E_homogeneous.Checked);
                            writer.WriteLine();

                            writer.WriteLine("interpolation : {0}  reset: {1}  different boundary: {1}", interpolation, reset, new_boundary);
                            if (new_boundary)
                            {
                                writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                                for (int i = 0; i < N_defektov; i++)
                                {
                                    writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_down[i][2], defekti_down[i][0], defekti_down[i][1]);
                                }
                            }
                        }

                        #endregion

                        #region Izračun

                        if (time_dependent)
                        {
                            Iteration_dt_dir(dir);
                        }

                        else
                        {
                            Iteration_dir(dir);
                        }

                        Angle_calculation_dir(dir);

                        Izpis_rezultatov(dir);

                        #endregion
                    }
                }

                #endregion
            }

            #endregion

            MessageBox.Show("Completed!");
        }

        private void Calculation_continuous()
        {
            string dir;

            #region Spreminjanje E

            if (run_type == 1)
            {
                for (; (Ex + Ey + Ez) < E_max;)
                {
                    #region Izpis

                    dir = "Changing E";

                    if (Ex == 0.0 && Ey == 0.0 && Ez == 0.0)
                    {
                        dir = Path.Combine(dir, "E = 0,0");
                    }
                    else if (Ex != 0.0 && Ey == 0.0 && Ez == 0.0)
                    {
                        dir = Path.Combine(dir, "Parallel to x");
                        dir = Path.Combine(dir, "Ex" + Ex.ToString());
                    }
                    else if (Ex == 0.0 && Ey != 0.0 && Ez == 0.0)
                    {
                        dir = Path.Combine(dir, "Parallel to y");
                        dir = Path.Combine(dir, "Ey" + Ey.ToString());
                    }
                    else if (Ex == 0.0 && Ey == 0.0 && Ez != 0.0)
                    {
                        dir = Path.Combine(dir, "Parallel to z");
                        dir = Path.Combine(dir, "Ez" + Ez.ToString());
                    }
                    else
                    {
                        dir = Path.Combine(dir, "General");
                        dir = Path.Combine(dir, "Ex" + Ex.ToString() + "+Ey" + Ey.ToString() + "+Ez" + Ez.ToString());
                    }

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Parameters.txt"), false))
                    {
                        writer.WriteLine("eps: {0}  itmax: {1}  kor: {2}", eps, itmax, kor);
                        writer.WriteLine("Nr: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                        writer.WriteLine("Rmi: {0}  Rma: {1}", Rmi, Rma);
                        writer.WriteLine("a: {0}  t: {1}  w: {2}  BB: {3}", a, t, w, BB);
                        writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                        writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                        for (int i = 0; i < N_defektov; i++)
                        {
                            writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_down[i][2], defekti_down[i][0], defekti_down[i][1]);
                        }
                        writer.WriteLine("run type: {0}", run_type);
                    }

                    #endregion

                    Init();

                    Iteration();

                    Angle_calculation_dir(dir);

                    Izpis_rezultatov(dir);

                    if (E_changing == 1)
                    {
                        Ex += 0.01;
                    }
                    else if (E_changing == 2)
                    {
                        Ey += 0.01;
                    }
                    else if (E_changing == 3)
                    {
                        Ez += 0.01;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            #endregion

            #region Spreminjanje a

            else
            {
                for (; Rma >= 0.5;)
                {
                    #region Izpis

                    dir = "Changing size";
                    dir = Path.Combine(dir, "a" + a.ToString());

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Parameters.txt"), false))
                    {
                        writer.WriteLine("eps: {0}  itmax: {1}  kor: {2}", eps, itmax, kor);
                        writer.WriteLine("Nr: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                        writer.WriteLine("Rmi: {0}  Rma: {1}", Rmi, Rma);
                        writer.WriteLine("a: {0}  t: {1}  w: {2}  BB: {3}", a, t, w, BB);
                        writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                        writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                        for (int i = 0; i < N_defektov; i++)
                        {
                            writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_down[i][2], defekti_down[i][0], defekti_down[i][1]);
                        }
                        writer.WriteLine("run type: {0}", run_type);
                    }

                    #endregion

                    Init();

                    Iteration();

                    Angle_calculation_dir(dir);

                    Izpis_rezultatov(dir);

                    a += 20.0;
                    Rma = 200.0 / a;

                    dx = (Rma - Rmi) / ((double)Nx - 1.0);
                    dy = (Rma - Rmi) / ((double)Ny - 1.0);
                    dz = 1.0 / ((double)Nz - 1.0);
                    AA = a * a;
                }
            }

            #endregion

            MessageBox.Show("Completed!");
        }
        
        #endregion


        #region Obsolete functions

        private void Continue_Click(object sender, EventArgs e)
        {
            #region Setting proper reading mode

            draw_size = pictureBox1.Height;
            int read = 0;
            bool new_boundary = false;

            Input_selection Read_mode = new Input_selection();
            Read_mode.Text = "Read Tensor field or Director field?";

            if (Read_mode.ShowDialog(this) == DialogResult.OK)
            {
                read = Read_mode.readmode;
                new_boundary = Read_mode.boundary;
                reset = Read_mode.reset;
            }

            else
            {
                MessageBox.Show("Error!");
            }
            Read_mode.Dispose();

            #endregion

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                #region Uvoz datoteke in nastavitev parametrov

                #region Pomožni arrayi

                string[] datoteka = File.ReadAllLines(ofd.FileName);
                string[] data;
                string[] separators = { "\t", " " };

                int[] x_i, y_j, z_k;

                x_i = new int[datoteka.Length];
                y_j = new int[datoteka.Length];
                z_k = new int[datoteka.Length];

                #endregion

                #region Postavitev lokacij

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

                #region Inicializacija arrayev

                xv = new double[Nx];
                yv = new double[Ny];
                zv = new double[Nz];
                E_system = new double[itmax];

                Q1 = new double[Nx][][];
                Q2 = new double[Nx][][];
                Q3 = new double[Nx][][];
                Q4 = new double[Nx][][];
                Q5 = new double[Nx][][];

                Q1_n = new double[Nx][][];
                Q2_n = new double[Nx][][];
                Q3_n = new double[Nx][][];
                Q4_n = new double[Nx][][];
                Q5_n = new double[Nx][][];

                b2 = new double[Nx][][];
                S = new double[Nx][][];

                E = new double[Nx][][][];
                direktor = new double[Nx][][][];

                for (int i = 0; i < Nx; i++)
                {
                    Q1[i] = new double[Ny][];
                    Q2[i] = new double[Ny][];
                    Q3[i] = new double[Ny][];
                    Q4[i] = new double[Ny][];
                    Q5[i] = new double[Ny][];

                    Q1_n[i] = new double[Ny][];
                    Q2_n[i] = new double[Ny][];
                    Q3_n[i] = new double[Ny][];
                    Q4_n[i] = new double[Ny][];
                    Q5_n[i] = new double[Ny][];

                    b2[i] = new double[Ny][];
                    S[i] = new double[Ny][];

                    E[i] = new double[Ny][][];
                    direktor[i] = new double[Ny][][];

                    for (int j = 0; j < Ny; j++)
                    {
                        Q1[i][j] = new double[Nz];
                        Q2[i][j] = new double[Nz];
                        Q3[i][j] = new double[Nz];
                        Q4[i][j] = new double[Nz];
                        Q5[i][j] = new double[Nz];

                        Q1_n[i][j] = new double[Nz];
                        Q2_n[i][j] = new double[Nz];
                        Q3_n[i][j] = new double[Nz];
                        Q4_n[i][j] = new double[Nz];
                        Q5_n[i][j] = new double[Nz];

                        b2[i][j] = new double[Nz];
                        S[i][j] = new double[Nz];

                        E[i][j] = new double[Nz][];
                        direktor[i][j] = new double[Nz][];

                        for (int k = 0; k < Nz; k++)
                        {
                            E[i][j][k] = new double[3];
                            direktor[i][j][k] = new double[3];
                        }
                    }
                }

                Parameters1.Add(Q1);
                Parameters1.Add(Q2);
                Parameters1.Add(Q3);
                Parameters1.Add(Q4);
                Parameters1.Add(Q5);

                Parameters2.Add(Q1_n);
                Parameters2.Add(Q2_n);
                Parameters2.Add(Q3_n);
                Parameters2.Add(Q4_n);
                Parameters2.Add(Q5_n);


                #endregion

                #region Nastavitev vrednosti iz menija

                eps = (double)eps_n.Value;
                itmax = (int)itmax_n.Value;
                kor = (double)kor_n.Value;

                Rmi = (double)Rmi_n.Value;
                Rma = (double)Rma_n.Value;

                a = (double)H_ksi_n.Value;
                t = (double)t_n.Value;
                w = (double)w_n.Value;
                BB = (double)BB_n.Value;

                Ex = (double)Ex_n.Value;
                Ey = (double)Ey_n.Value;
                Ez = (double)Ez_n.Value;
                E_max = (double)E_max_n.Value;

                for (int i = 0; i < Nx; i++)
                {
                    xv[i] = dx * i + Rmi;
                }
                for (int j = 0; j < Ny; j++)
                {
                    yv[j] = dy * j;
                }
                for (int k = 0; k < Nz; k++)
                {
                    zv[k] = dz * k;
                }

                #endregion

                #region Robni pogoji

                i_lower = 1;
                i_upper = Nx - 1;

                j_lower = 1;
                j_upper = Ny - 1;

                k_lower = 1;
                k_upper = Nz - 1;

                if (periodic_x.Checked)
                {
                    i_lower = 0;
                    i_upper = Nx;
                }

                if (periodic_y.Checked)
                {
                    j_lower = 0;
                    j_upper = Ny;
                }

                if (periodic_z.Checked)
                {
                    k_lower = 0;
                    k_upper = Nz;
                }

                #endregion

                #region Nastavitev parametrov

                if (izracun_za_polja.Checked)
                {
                    run_type = 1;
                }

                if (Ex_b.Checked)
                {
                    E_changing = 1;
                }
                else if (Ey_b.Checked)
                {
                    E_changing = 2;
                }
                else if (Ez_b.Checked)
                {
                    E_changing = 3;
                }
                else
                {
                    E_changing = 0;
                }

                dx = (Rma - Rmi) / ((double)Nx - 1.0);
                dy = (Rma - Rmi) / ((double)Ny - 1.0);
                dz = 1.0 / ((double)Nz - 1.0);
                tt = 1.0 + Math.Sqrt(1.0 - t);
                AA = a * a;
                sb = tt;

                #endregion

                #region Električno polje

                if (E_homogeneous.Checked)
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k][0] = Ex;
                                E[i][j][k][1] = Ey;
                                E[i][j][k][2] = Ez;
                            }
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k][0] = Ex * (j + 1) / Ny;
                                E[i][j][k][1] = Ey * (j + 1) / Ny;
                                E[i][j][k][2] = Ez * (j + 1) / Ny;
                            }
                        }
                    }
                }

                #endregion

                #region Določitev vrednosti

                #region Tenzor

                if (read == 0)
                {
                    for (int i = 0; i < datoteka.Length; i++)
                    {
                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        Q1[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[3]);
                        Q2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[4]);
                        Q3[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[5]);
                        Q4[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[6]);
                        Q5[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[7]);

                        b2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[8]);
                    }
                }

                #endregion

                #region Direktorsko polje

                if (read == 1)
                {
                    double kot_phi, kot_theta;

                    for (int i = 0; i < datoteka.Length; i++)
                    {
                        data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        direktor[x_i[i]][y_j[i]][z_k[i]][0] = double.Parse(data[3]);
                        direktor[x_i[i]][y_j[i]][z_k[i]][1] = double.Parse(data[4]);
                        direktor[x_i[i]][y_j[i]][z_k[i]][2] = double.Parse(data[5]);

                        kot_theta = Math.Acos(direktor[x_i[i]][y_j[i]][z_k[i]][2]);
                        kot_phi = Math.Atan2(direktor[x_i[i]][y_j[i]][z_k[i]][1], direktor[x_i[i]][y_j[i]][z_k[i]][0]);

                        if (x_i[i] == 0 || x_i[i] + 1 == Nx || y_j[i] == 0 || y_j[i] + 1 == Ny)
                        {
                            kot_theta = Math.PI / 2.0;
                            kot_phi = Math.Atan2(y_j[i] - Ny / 2, x_i[i] - Nx / 2);
                        }


                        Q1[x_i[i]][y_j[i]][z_k[i]] = tt * (1.0 / 6.0 - (Math.Cos(kot_theta) * Math.Cos(kot_theta)) / 2.0);
                        Q2[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(kot_theta) * Math.Sin(kot_theta) * Math.Cos(2.0 * kot_phi)) / 2.0;
                        Q3[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(kot_theta) * Math.Sin(kot_theta) * Math.Sin(2.0 * kot_phi)) / 2.0;
                        Q4[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(2.0 * kot_theta) * Math.Cos(kot_phi)) / 2.0;
                        Q5[x_i[i]][y_j[i]][z_k[i]] = tt * (Math.Sin(2.0 * kot_theta) * Math.Sin(kot_phi)) / 2.0;

                        Q1_n[x_i[i]][y_j[i]][z_k[i]] = Q1[x_i[i]][y_j[i]][z_k[i]];
                        Q2_n[x_i[i]][y_j[i]][z_k[i]] = Q2[x_i[i]][y_j[i]][z_k[i]];
                        Q3_n[x_i[i]][y_j[i]][z_k[i]] = Q3[x_i[i]][y_j[i]][z_k[i]];
                        Q4_n[x_i[i]][y_j[i]][z_k[i]] = Q4[x_i[i]][y_j[i]][z_k[i]];
                        Q5_n[x_i[i]][y_j[i]][z_k[i]] = Q5[x_i[i]][y_j[i]][z_k[i]];
                    }
                }

                #endregion

                #region Novi zgornji robni pogoji

                if (new_boundary)
                {
                    defekti_up = Defect_setup(Top_N_numeric, "Upper boundary ");

                    double theta, phi;

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            #region Nastavitev pogojev

                            if (upper_boundary == 0) // Defect
                            {
                                theta = Math.PI / 2.0;
                                phi = 0.0;

                                for (int d = 0; d < defekti_up.Length; d++)
                                {
                                    phi += defekti_up[d][2] * Math.Atan2(j - defekti_up[d][1], i - defekti_up[d][0]);
                                }
                            }

                            else if (upper_boundary == 1) //Tangential degenerate
                            {
                                theta = Math.PI / 2.0;
                                phi = Math.PI * r.NextDouble();
                            }

                            else // Homeotropic
                            {
                                theta = 0.0;
                                phi = 0.0;
                            }

                            #endregion

                            #region Izračun vrednosti

                            Q1[i][j][Nz - 1] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                            Q2[i][j][Nz - 1] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                            Q3[i][j][Nz - 1] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                            Q4[i][j][Nz - 1] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                            Q5[i][j][Nz - 1] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                            Q1_n[i][j][Nz - 1] = Q1[i][j][Nz - 1];
                            Q2_n[i][j][Nz - 1] = Q2[i][j][Nz - 1];
                            Q3_n[i][j][Nz - 1] = Q3[i][j][Nz - 1];
                            Q4_n[i][j][Nz - 1] = Q4[i][j][Nz - 1];
                            Q5_n[i][j][Nz - 1] = Q5[i][j][Nz - 1];

                            #endregion
                        }
                    }
                }

                #endregion

                #endregion

                #region Izpis

                using (StreamWriter writer = new StreamWriter("Parameters.txt", false))
                {
                    writer.WriteLine("eps: {0}  itmax: {1}  kor: {2}", eps, itmax, kor);
                    writer.WriteLine("Nr: {0}  Ny: {1}  Nz: {2}", Nx, Ny, Nz);
                    writer.WriteLine("Rmi: {0}  Rma: {1}", Rmi, Rma);
                    writer.WriteLine("a: {0}  t: {1}  w: {2}  BB: {3}", a, t, w, BB);
                    writer.WriteLine("Ex: {0}  Ey: {1}  Ez: {2} ", Ex, Ey, Ez);
                    writer.WriteLine("robni: {0}  N defektov: {1}", defekt, N_defektov);
                    for (int i = 0; i < N_defektov; i++)
                    {
                        writer.WriteLine("Defect {0,2}: m = {1,3}  x = {2,2}  y = {3,2}", i + 1, defekti_up[i][2], defekti_up[i][0], defekti_up[i][1]);
                    }
                    writer.WriteLine("run type: {0}", run_type);
                }

                #endregion

                #endregion

                #region Prikaz in računanje

                progressBar1.Visible = true;
                progressBar1.Maximum = itmax;
                progressBar1.Value = 0;

                Thread th = new Thread(Calculation_continued);
                th.IsBackground = true;
                th.Start();

                th.Join();

                progressBar1.Visible = false;

                trackBar_depth.Visible = true;
                trackBar_depth.Maximum = Nx - 1;
                Pogled.Visible = true;

                #endregion
            }
        }

        private void Many_runs_Click(object sender, EventArgs e)
        {
            #region Določitev vrednosti

            #region Nastavitev vrednosti iz menija
            
            eps = (double)eps_n.Value;
            itmax = (int)itmax_n.Value;
            kor = (double)kor_n.Value;

            Nx = (int)Nx_n.Value;
            Ny = (int)Ny_n.Value;
            Nz = (int)Nz_n.Value;

            #region Inicializacija arrayev

            xv = new double[Nx];
            yv = new double[Ny];
            zv = new double[Nz];

            Q1 = new double[Nx][][];
            Q2 = new double[Nx][][];
            Q3 = new double[Nx][][];
            Q4 = new double[Nx][][];
            Q5 = new double[Nx][][];

            Q1_n = new double[Nx][][];
            Q2_n = new double[Nx][][];
            Q3_n = new double[Nx][][];
            Q4_n = new double[Nx][][];
            Q5_n = new double[Nx][][];

            b2 = new double[Nx][][];
            S = new double[Nx][][];

            E = new double[Nx][][][];
            direktor = new double[Nx][][][];

            for (int i = 0; i < Nx; i++)
            {
                Q1[i] = new double[Ny][];
                Q2[i] = new double[Ny][];
                Q3[i] = new double[Ny][];
                Q4[i] = new double[Ny][];
                Q5[i] = new double[Ny][];

                Q1_n[i] = new double[Ny][];
                Q2_n[i] = new double[Ny][];
                Q3_n[i] = new double[Ny][];
                Q4_n[i] = new double[Ny][];
                Q5_n[i] = new double[Ny][];

                b2[i] = new double[Ny][];
                S[i] = new double[Ny][];

                E[i] = new double[Ny][][];
                direktor[i] = new double[Ny][][];

                for (int j = 0; j < Ny; j++)
                {
                    Q1[i][j] = new double[Nz];
                    Q2[i][j] = new double[Nz];
                    Q3[i][j] = new double[Nz];
                    Q4[i][j] = new double[Nz];
                    Q5[i][j] = new double[Nz];

                    Q1_n[i][j] = new double[Nz];
                    Q2_n[i][j] = new double[Nz];
                    Q3_n[i][j] = new double[Nz];
                    Q4_n[i][j] = new double[Nz];
                    Q5_n[i][j] = new double[Nz];

                    b2[i][j] = new double[Nz];
                    S[i][j] = new double[Nz];

                    E[i][j] = new double[Nz][];
                    direktor[i][j] = new double[Nz][];

                    for (int k = 0; k < Nz; k++)
                    {
                        E[i][j][k] = new double[3];
                        direktor[i][j][k] = new double[3];
                    }
                }
            }

            Parameters1.Add(Q1);
            Parameters1.Add(Q2);
            Parameters1.Add(Q3);
            Parameters1.Add(Q4);
            Parameters1.Add(Q5);

            Parameters2.Add(Q1_n);
            Parameters2.Add(Q2_n);
            Parameters2.Add(Q3_n);
            Parameters2.Add(Q4_n);
            Parameters2.Add(Q5_n);


            #endregion

            Rmi = (double)Rmi_n.Value;
            Rma = (double)Rma_n.Value;

            a = (double)H_ksi_n.Value;
            t = (double)t_n.Value;
            w = (double)w_n.Value;
            BB = (double)BB_n.Value;

            Ex = (double)Ex_n.Value;
            Ey = (double)Ey_n.Value;
            Ez = (double)Ez_n.Value;
            E_max = (double)E_max_n.Value;

            #endregion

            #region Ureditev na robu in v celici

            upper_boundary = Top_boundary_box.SelectedIndex;
            if (upper_boundary == 0)
            {
                defekti_up = Defect_setup(Top_N_numeric, "Upper boundary ");
            }
            if (upper_boundary == 1)
            {
                phi0_upper = (double)Top_N_numeric.Value;
                phi0_upper = phi0_upper * Math.PI / 180.0;
            }

            lower_boundary = Bottom_boundary_box.SelectedIndex;
            if (lower_boundary == 0)
            {
                defekti_down = Defect_setup(Bottom_N_numeric, "Lower boundary ");
            }
            if (lower_boundary == 1)
            {
                phi0_lower = (double)Bottom_N_numeric.Value;
                phi0_lower = phi0_lower * Math.PI / 180.0;
            }

            bulk = Bulk_box.SelectedIndex;
            if (bulk == 1)
            {
                phi0_bulk = (double)Bulk_phi_numeric.Value;
                phi0_bulk = phi0_bulk * Math.PI / 180.0;
            }

            #endregion
            
            #region Robni pogoji

            i_lower = 1;
            i_upper = Nx - 1;

            j_lower = 1;
            j_upper = Ny - 1;

            k_lower = 1;
            k_upper = Nz - 1;

            if (periodic_x.Checked)
            {
                i_lower = 0;
                i_upper = Nx;
            }

            if (periodic_y.Checked)
            {
                j_lower = 0;
                j_upper = Ny;
            }

            if (periodic_z.Checked)
            {
                k_lower = 0;
                k_upper = Nz;
            }

            #endregion

            #region Nastavitev parametrov

            if (izracun_za_polja.Checked)
            {
                run_type = 1;
            }

            if (Ex_b.Checked)
            {
                E_changing = 1;
            }
            if (Ey_b.Checked)
            {
                E_changing = 2;
            }
            if (Ez_b.Checked)
            {
                E_changing = 3;
            }
            else
            {
                E_changing = 0;
            }

            dx = (Rma - Rmi) / ((double)Nx - 1.0);
            dy = (Rma - Rmi) / ((double)Ny - 1.0);
            dz = 1.0 / ((double)Nz - 1.0);
            tt = 1.0 + Math.Sqrt(1.0 - t);
            AA = a * a;
            sb = tt;

            #endregion

            #region Električno polje

            if (E_homogeneous.Checked)
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            E[i][j][k][0] = Ex;
                            E[i][j][k][1] = Ey;
                            E[i][j][k][2] = Ez;
                        }
                    }
                }
            }

            else
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            E[i][j][k][0] = Ex * (j + 1) / Ny;
                            E[i][j][k][1] = Ey * (j + 1) / Ny;
                            E[i][j][k][2] = Ez * (j + 1) / Ny;
                        }
                    }
                }
            }

            #endregion

            deps = 1.0;

            if (neg_d_eps_n2.Checked)
            {
                deps = -1.0;
            }

            gamma = 1.0;
            dt = 1.0 * Math.Pow(10, 0);

            draw_size = pictureBox1.Height;

            #endregion

            #region Izračun in prikaz

            Thread th = new Thread(Calculation_continuous);
            th.IsBackground = true;
            th.Start();

            th.Join();

            #endregion
        }

        #endregion

        
        #region Q next step calculation

        #region Časovno neodvisne funkcije

        private void Iteration()
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5());

                Task.WaitAll(tasks);
                
                //Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);
                //Zgornja_free_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                
                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                
                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n());

                Task.WaitAll(tasks);
                
                //Zgornja_meja_tangential_degenerate(Q2, Q3);
                //Zgornja_free_meja(Q1, Q2, Q3, Q4, Q5);
                
                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }
                
                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 500 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis(it);
                }
                
                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        private void Iteration_dir(string dir)
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5());

                Task.WaitAll(tasks);

                //Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);
                //Spodnja_free_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                
                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n());

                Task.WaitAll(tasks);

                //Zgornja_meja_tangential_degenerate(Q2, Q3);
                //Spodnja_free_meja(Q1, Q2, Q3, Q4, Q5);
                
                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 1000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis_dir(dir, it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        #region Zanke za Q -> Q_n

        static bool Iteracija_Q1()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q1_n[i][j][k] = Next_value_Q1(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1_n[i][j][k] = Next_value_Q1(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q2_n[i][j][k] = Next_value_Q2(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2_n[i][j][k] = Next_value_Q2(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q3_n[i][j][k] = Next_value_Q3(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3_n[i][j][k] = Next_value_Q3(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q4_n[i][j][k] = Next_value_Q4(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4_n[i][j][k] = Next_value_Q4(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q5_n[i][j][k] = Next_value_Q5(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5_n[i][j][k] = Next_value_Q5(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Zanke za Q_n -> Q

        static bool Iteracija_Q1_n()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q1[i][j][k] = Next_value_Q1(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1[i][j][k] = Next_value_Q1(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_n()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q2[i][j][k] = Next_value_Q2(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2[i][j][k] = Next_value_Q2(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_n()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q3[i][j][k] = Next_value_Q3(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3[i][j][k] = Next_value_Q3(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_n()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q4[i][j][k] = Next_value_Q4(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4[i][j][k] = Next_value_Q4(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_n()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q5[i][j][k] = Next_value_Q5(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5[i][j][k] = Next_value_Q5(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Izračun naslednjih vrednosti

        /// <summary>
        /// Izračuna naslednje stanje za Q1
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q1(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z, B_x, B_y, B_z;
            double d_x, d_y, d_z;
            double result = 0.0;
            
            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q1[im][j][k], Q1[i][j][k], Q1[ip][j][k], dx);
            d_y = Drugi_odvod(Q1[i][jm][k], Q1[i][j][k], Q1[i][jp][k], dy);
            d_z = Drugi_odvod(Q1[i][j][km], Q1[i][j][k], Q1[i][j][kp], dz);
            /*
            d_xy = Drugi_odvod(Q1[im][jm][k], Q1[i][j][k], Q1[ip][jp][k], dxy);
            d_xz = Drugi_odvod(Q1[im][j][km], Q1[i][j][k], Q1[ip][j][kp], dxz);
            d_yz = Drugi_odvod(Q1[i][jm][km], Q1[i][j][k], Q1[i][jp][kp], dyz);

            d_x1 = Drugi_odvod(Q1[im][jp][k], Q1[i][j][k], Q1[ip][jm][k], dxy);
            d_x2 = Drugi_odvod(Q1[im][j][kp], Q1[i][j][k], Q1[ip][j][km], dxz);
            
            d_y1 = Drugi_odvod(Q1[ip][jm][k], Q1[i][j][k], Q1[im][jp][k], dxy);
            d_y2 = Drugi_odvod(Q1[i][jm][kp], Q1[i][j][k], Q1[i][jp][km], dyz);

            d_z1 = Drugi_odvod(Q1[ip][j][km], Q1[i][j][k], Q1[im][j][kp], dxz);
            d_z2 = Drugi_odvod(Q1[i][jp][km], Q1[i][j][k], Q1[i][jm][kp], dyz);

            d_xyz1 = Drugi_odvod(Q1[im][jm][km], Q1[i][j][k], Q1[ip][jp][kp], dxyz);
            d_xyz2 = Drugi_odvod(Q1[im][jm][kp], Q1[i][j][k], Q1[ip][jp][km], dxyz);
            d_xyz3 = Drugi_odvod(Q1[im][jp][km], Q1[i][j][k], Q1[ip][jm][kp], dxyz);
            d_xyz4 = Drugi_odvod(Q1[ip][jm][km], Q1[i][j][k], Q1[im][jp][kp], dxyz);

            d_ext = d_xy + d_xz + d_yz + d_x1 + d_x2 + d_y1 + d_y2 + d_z1 + d_z2 + d_xyz1 + d_xyz2 + d_xyz3 + d_xyz4;
            */
            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t*q1 / 6.0 - (6.0*q1*q1 - 2.0*q2*q2 - 2.0*q3*q3 + q4*q4 + q5*q5) / 6.0 - (q1*tr_Q2) / 2.0;
            EL_en += deps * (E_x*E_x + E_y*E_y - 2.0*E_z*E_z) / 12.0 + dmu * (B_x*B_x + B_y*B_y - 2.0*B_z*B_z) / 12.0;
            dEL_en = -t / 6.0 - 2.0*q1 - (tr_Q2 + 6.0*q1*q1) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q1[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q1[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q2
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q2(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z, B_x, B_y, B_z;
            double d_x, d_y, d_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q2[im][j][k], Q2[i][j][k], Q2[ip][j][k], dx);
            d_y = Drugi_odvod(Q2[i][jm][k], Q2[i][j][k], Q2[i][jp][k], dy);
            d_z = Drugi_odvod(Q2[i][j][km], Q2[i][j][k], Q2[i][j][kp], dz);
            
            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t*q2 / 6.0 + (4.0*q1*q2 + q4*q4 - q5*q5) / 2.0 - (q2*tr_Q2) / 2.0;
            EL_en += deps * (E_x*E_x - E_y*E_y) / 4.0 + dmu * (B_x*B_x - B_y*B_y) / 4.0;
            dEL_en = -t / 6.0 + 2.0*q1 - (tr_Q2 + 2.0*q2*q2) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q2[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q2[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q3
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q3(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z, B_x, B_y, B_z;
            double d_x, d_y, d_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q3[im][j][k], Q3[i][j][k], Q3[ip][j][k], dx);
            d_y = Drugi_odvod(Q3[i][jm][k], Q3[i][j][k], Q3[i][jp][k], dy);
            d_z = Drugi_odvod(Q3[i][j][km], Q3[i][j][k], Q3[i][j][kp], dz);
            
            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t*q3 / 6.0 + 2.0*q1*q3 + q4*q5 - (q3*tr_Q2) / 2.0 + deps*E_x*E_y / 2.0 + dmu*B_x*B_y / 2.0;
            dEL_en = -t / 6.0 + 2.0*q1 - (tr_Q2 + 2.0*q3*q3) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q3[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q3[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q4
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q4(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z, B_x, B_y, B_z;
            double d_x, d_y, d_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q4[im][j][k], Q4[i][j][k], Q4[ip][j][k], dx);
            d_y = Drugi_odvod(Q4[i][jm][k], Q4[i][j][k], Q4[i][jp][k], dy);
            d_z = Drugi_odvod(Q4[i][j][km], Q4[i][j][k], Q4[i][j][kp], dz);
            
            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t*q4 / 6.0 - q1*q4 + q2*q4 + q3*q5 - (q4*tr_Q2) / 2.0 + deps*E_x*E_z / 2.0 + dmu*B_x*B_z / 2.0;
            dEL_en = -t / 6.0 - q1 + q2 - (tr_Q2 + 2.0*q4*q4) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q4[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q4[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q5
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q5(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z, B_x, B_y, B_z;
            double d_x, d_y, d_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q5[im][j][k], Q5[i][j][k], Q5[ip][j][k], dx);
            d_y = Drugi_odvod(Q5[i][jm][k], Q5[i][j][k], Q5[i][jp][k], dy);
            d_z = Drugi_odvod(Q5[i][j][km], Q5[i][j][k], Q5[i][j][kp], dz);
            
            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t*q5 / 6.0 - q1*q5 - q2*q5 + q3*q4 - (q5*tr_Q2) / 2.0 + deps*E_y*E_z / 2.0 + dmu*B_y*B_z / 2.0;
            dEL_en = -t / 6.0 - q1 - q2 - (tr_Q2 + 2.0*q5*q5) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q5[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q5[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        #endregion

        #endregion

        #region Časovno odvisne funkcije

        private void Iteration_dt()
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_dt());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_dt());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_dt());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_dt());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_dt());

                Task.WaitAll(tasks);

                if (upper_boundary == 2)
                {
                    Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);
                }

                if (lower_boundary == 2)
                {
                    Spodnja_meja_tangential_degenerate(Q2_n, Q3_n);
                }

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else if (sides == 2)
                {
                    double tempo = 2.0 * (double)it / (double)itmax;
                    Zunanja_meja_Williams(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n, tempo);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                    Bottom_boundary(Q2_n, Q3_n);
                }
                
                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_dt());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_dt());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_dt());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_dt());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_dt());

                Task.WaitAll(tasks);
                
                if (upper_boundary == 2)
                {
                    Zgornja_meja_tangential_degenerate(Q2, Q3);
                }

                if (lower_boundary == 2)
                {
                    Spodnja_meja_tangential_degenerate(Q2, Q3);
                }
                
                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }
                else if (sides == 2)
                {
                    double tempo = 2.0 * (double)(it + 1) / (double)itmax;
                    Zunanja_meja_Williams(Q1, Q2, Q3, Q4, Q5, tempo);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                    Bottom_boundary(Q2, Q3);
                }
                
                #endregion

                progressBar1.Increment(2);

                if (it % 100 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);
                    Vmesni_izpis(it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        private void Iteration_dt_dir(string dir)
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_dt());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_dt());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_dt());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_dt());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_dt());

                Task.WaitAll(tasks);
                
                //Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);
                //Spodnja_free_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);

                if (upper_boundary == 2)
                {
                    Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);
                }

                if (lower_boundary == 2)
                {
                    Spodnja_meja_tangential_degenerate(Q2_n, Q3_n);
                }

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                else if (sides == 2)
                {
                    double tempo = 2.0 * (double)it / (double)itmax;
                    Zunanja_meja_Williams(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n, tempo);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }
                
                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_dt());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_dt());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_dt());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_dt());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_dt());

                Task.WaitAll(tasks);
                
                //Zgornja_meja_tangential_degenerate(Q2, Q3);
                //Spodnja_free_meja(Q1, Q2, Q3, Q4, Q5);

                if (upper_boundary == 2)
                {
                    Zgornja_meja_tangential_degenerate(Q2, Q3);
                }

                if (lower_boundary == 2)
                {
                    Spodnja_meja_tangential_degenerate(Q2, Q3);
                }

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }
                else if (sides == 2)
                {
                    double tempo = 2.0 * (double)(it + 1) / (double)itmax;
                    Zunanja_meja_Williams(Q1, Q2, Q3, Q4, Q5, tempo);
                }


                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }
                
                #endregion

                progressBar1.Increment(2);

                if (it % 2 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis_dir(dir, it);
                }

                it += 2;

                if (it % 60000 == 0)
                {
                    #region Setting up the next round

                    if (changemode == 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (j == 0)
                                {
                                    defekti_down[2 * i + j][1] += 1.0;
                                }
                                if (j == 1)
                                {
                                    defekti_down[2 * i + j][1] -= 1.0;
                                }
                            }
                        }

                        double theta, phi;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                #region Nastavitev pogojev

                                if (lower_boundary == 0) // Defect
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                    }
                                }

                                else if (lower_boundary == 1) //Tangential
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;
                                }

                                else if (lower_boundary == 2) //Tangential degenerate
                                {
                                    theta = Math.PI / 2.0;
                                    phi = Math.PI * r.NextDouble();
                                }

                                else // Homeotropic
                                {
                                    theta = 0.0;
                                    phi = 0.0;
                                }

                                #endregion

                                #region Izračun vrednosti

                                Q1[i][j][0] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                Q2[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                Q3[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                Q4[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                Q5[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                Q1_n[i][j][0] = Q1[i][j][0];
                                Q2_n[i][j][0] = Q2[i][j][0];
                                Q3_n[i][j][0] = Q3[i][j][0];
                                Q4_n[i][j][0] = Q4[i][j][0];
                                Q5_n[i][j][0] = Q5[i][j][0];

                                #endregion
                            }
                        }
                    }

                    #endregion

                    #region Changing E

                    if (E_changing == 1) { Ex += 0.01; }
                    else if (E_changing == 2) { Ey += 0.01; }
                    else if (E_changing == 3) { Ez += 0.01; }

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k][0] = Ex;
                                E[i][j][k][1] = Ey;
                                E[i][j][k][2] = Ez;
                            }
                        }
                    }

                    #endregion
                }
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        #region Zanke za Q -> Q_n

        static bool Iteracija_Q1_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1_n[i][j][k] = Next_value_Q1_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2_n[i][j][k] = Next_value_Q2_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3_n[i][j][k] = Next_value_Q3_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4_n[i][j][k] = Next_value_Q4_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5_n[i][j][k] = Next_value_Q5_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Zanke za Q_n -> Q

        static bool Iteracija_Q1_n_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1[i][j][k] = Next_value_Q1_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_n_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2[i][j][k] = Next_value_Q2_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_n_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3[i][j][k] = Next_value_Q3_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_n_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4[i][j][k] = Next_value_Q4_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_n_dt()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5[i][j][k] = Next_value_Q5_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Izračun naslednjih vrednosti

        /// <summary>
        /// Izračuna naslednje stanje za Q1
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q1_dt(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, d_x, d_y, d_z, EL_en, tr_Q2, dq, E_x, E_y, E_z, B_x, B_y, B_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            d_x = Drugi_odvod(Q1[im][j][k], Q1[i][j][k], Q1[ip][j][k], dx);
            d_y = Drugi_odvod(Q1[i][jm][k], Q1[i][j][k], Q1[i][jp][k], dy);
            d_z = Drugi_odvod(Q1[i][j][km], Q1[i][j][k], Q1[i][j][kp], dz);

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -t * q1 / 6.0 - (6.0*q1*q1 - 2.0*q2*q2 - 2.0*q3*q3 + q4*q4 + q5*q5) / 6.0 - (q1*tr_Q2) / 2.0;
            EL_en += deps * (E_x*E_x + E_y*E_y - 2.0*E_z*E_z) / 12.0 + dmu * (B_x*B_x + B_y*B_y - 2.0*B_z*B_z) / 12.0;
            dq = 2.0 * ((d_x + d_y + d_z) / AA + EL_en) / gamma;

            result = Q1[i][j][k] + dq * dt;

            if (Math.Abs(Q1[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q2
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q2_dt(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, d_x, d_y, d_z, EL_en, tr_Q2, dq, E_x, E_y, E_z, B_x, B_y, B_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            d_x = Drugi_odvod(Q2[im][j][k], Q2[i][j][k], Q2[ip][j][k], dx);
            d_y = Drugi_odvod(Q2[i][jm][k], Q2[i][j][k], Q2[i][jp][k], dy);
            d_z = Drugi_odvod(Q2[i][j][km], Q2[i][j][k], Q2[i][j][kp], dz);

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0;
            EL_en += deps * (E_x*E_x - E_y*E_y) / 4.0 + dmu * (B_x*B_x - B_y*B_y) / 4.0;
            dq = 2.0 * ((d_x + d_y + d_z) / AA + EL_en) / gamma;

            result = Q2[i][j][k] + dq * dt;

            if (Math.Abs(Q2[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q3
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q3_dt(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, d_x, d_y, d_z, EL_en, tr_Q2, dq, E_x, E_y, E_z, B_x, B_y, B_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            d_x = Drugi_odvod(Q3[im][j][k], Q3[i][j][k], Q3[ip][j][k], dx);
            d_y = Drugi_odvod(Q3[i][jm][k], Q3[i][j][k], Q3[i][jp][k], dy);
            d_z = Drugi_odvod(Q3[i][j][km], Q3[i][j][k], Q3[i][j][kp], dz);

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + deps*E_x*E_y / 2.0 + dmu*B_x*B_y / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / AA + EL_en) / gamma;

            result = Q3[i][j][k] + dq * dt;

            if (Math.Abs(Q3[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q4
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q4_dt(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, d_x, d_y, d_z, EL_en, tr_Q2, dq, E_x, E_y, E_z, B_x, B_y, B_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            d_x = Drugi_odvod(Q4[im][j][k], Q4[i][j][k], Q4[ip][j][k], dx);
            d_y = Drugi_odvod(Q4[i][jm][k], Q4[i][j][k], Q4[i][jp][k], dy);
            d_z = Drugi_odvod(Q4[i][j][km], Q4[i][j][k], Q4[i][j][kp], dz);

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + deps*E_x*E_z / 2.0 + dmu*B_x*B_z / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / AA + EL_en) / gamma;

            result = Q4[i][j][k] + dq * dt;

            if (Math.Abs(Q4[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q5
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q5_dt(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, d_x, d_y, d_z, EL_en, tr_Q2, dq, E_x, E_y, E_z, B_x, B_y, B_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            B_x = B[i][j][k][0];
            B_y = B[i][j][k][1];
            B_z = B[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            d_x = Drugi_odvod(Q5[im][j][k], Q5[i][j][k], Q5[ip][j][k], dx);
            d_y = Drugi_odvod(Q5[i][jm][k], Q5[i][j][k], Q5[i][jp][k], dy);
            d_z = Drugi_odvod(Q5[i][j][km], Q5[i][j][k], Q5[i][j][kp], dz);

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -t*q5 / 6.0 - q1*q5 - q2*q5 + q3*q4 - (q5*tr_Q2) / 2.0 + deps*E_z*E_y / 2.0 + dmu*B_z*B_y / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / AA + EL_en) / gamma;

            result = Q5[i][j][k] + dq * dt;

            if (Math.Abs(Q5[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        #endregion

        #endregion

        #region Različne elastične konstante

        private void Iteration_unequal_L()
        {
            it = 0;
            nap = 1;

            using (StreamWriter writer = new StreamWriter("L1L2L3.txt", false))
            {

            }

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                it += 2;
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_unequal_L());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_unequal_L());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_unequal_L());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_unequal_L());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_unequal_L());

                Task.WaitAll(tasks);
                
                //Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);
                //Zgornja_free_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_unequal_L());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_unequal_L());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_unequal_L());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_unequal_L());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_unequal_L());

                Task.WaitAll(tasks);
                
                //Zgornja_meja_tangential_degenerate(Q2, Q3);
                //Zgornja_free_meja(Q1, Q2, Q3, Q4, Q5);

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }
                #endregion

                progressBar1.Increment(2);

                if (it % 1000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis(it);
                }
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        #region Zanke za Q -> Q_n

        static bool Iteracija_Q1_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1_n[i][j][k] = Next_value_Q1_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2_n[i][j][k] = Next_value_Q2_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3_n[i][j][k] = Next_value_Q3_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4_n[i][j][k] = Next_value_Q4_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5_n[i][j][k] = Next_value_Q5_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Zanke za Q_n -> Q

        static bool Iteracija_Q1_n_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1[i][j][k] = Next_value_Q1_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_n_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2[i][j][k] = Next_value_Q2_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_n_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3[i][j][k] = Next_value_Q3_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_n_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4[i][j][k] = Next_value_Q4_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_n_unequal_L()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5[i][j][k] = Next_value_Q5_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Izračun naslednjih vrednosti

        /// <summary>
        /// Izračuna naslednje stanje za Q1
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q1_L(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            double q1, q2, q3, q4, q5, EL_en, dEL_en, f_e, tr_Q2, E_x, E_y, E_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            #endregion

            #region Izračun

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0 + deps * (E_x * E_x + E_y * E_y - 2.0 * E_z * E_z) / 12.0;
            dEL_en = -t / 6.0 - 2.0 * q1 - (tr_Q2 + 6.0 * q1 * q1) / 2.0;

            f_e = ELelastic_term_Q1(i, j, k, Q1, Q2, Q3, Q4, Q5);
            EL_en = f_e + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q1[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q1[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q2
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q2_L(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            double q1, q2, q3, q4, q5, EL_en, dEL_en, f_e, tr_Q2, E_x, E_y, E_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            #endregion

            #region Izračun

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0 + deps * (E_x * E_x - E_y * E_y) / 4.0;
            dEL_en = -t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q2 * q2) / 2.0;

            f_e = ELelastic_term_Q2(i, j, k, Q1, Q2, Q3, Q4, Q5);
            EL_en = f_e + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q2[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q2[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q3
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q3_L(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            double q1, q2, q3, q4, q5, EL_en, dEL_en, f_e, tr_Q2, E_x, E_y, E_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            #endregion

            #region Izračun

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + deps * E_x * E_y / 2.0;
            dEL_en = -t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q3 * q3) / 2.0;

            f_e = ELelastic_term_Q3(i, j, k, Q1, Q2, Q3, Q4, Q5);
            EL_en = f_e + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q3[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q3[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q4
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q4_L(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            double q1, q2, q3, q4, q5, EL_en, dEL_en, f_e, tr_Q2, E_x, E_y, E_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            #endregion

            #region Izračun

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + deps * E_x * E_z / 2.0;
            dEL_en = -t / 6.0 - q1 + q2 - (tr_Q2 + 2.0 * q4 * q4) / 2.0;

            f_e = ELelastic_term_Q4(i, j, k, Q1, Q2, Q3, Q4, Q5);
            EL_en = f_e + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q4[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q4[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q5
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q5_L(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            double q1, q2, q3, q4, q5, EL_en, dEL_en, f_e, tr_Q2, E_x, E_y, E_z;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            #endregion

            #region Izračun

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0 + deps * E_y * E_z / 2.0;
            dEL_en = -t / 6.0 - q1 - q2 - (tr_Q2 + 2.0 * q5 * q5) / 2.0;

            f_e = ELelastic_term_Q5(i, j, k, Q1, Q2, Q3, Q4, Q5);
            EL_en = f_e + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q5[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q5[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        #endregion

        #region Elastični del

        static double ELelastic_term_Q1(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, l1, l2, l3_1, l3_2;
            double q1dx, q1dy, q1dz, q2dx, q2dy, q3dx, q3dy, q4dx, q4dz, q5dy, q5dz;
            double q1d2x, q1d2y, q1d2z, q2d2x, q2d2y, qd1, qd2, q1dxy, q1dxz, q1dyz, q3dxy, q4dxz, q5dyz;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun odvodov

            #region Prvi odvodi

            q1dx = Odvod(Q1[im][j][k], Q1[ip][j][k], dx);
            q1dy = Odvod(Q1[i][jm][k], Q1[i][jp][k], dy);
            q1dz = Odvod(Q1[i][j][km], Q1[i][j][kp], dz);

            q2dx = Odvod(Q2[im][j][k], Q2[ip][j][k], dx);
            q2dy = Odvod(Q2[i][jm][k], Q2[i][jp][k], dy);

            q3dx = Odvod(Q3[im][j][k], Q3[ip][j][k], dx);
            q3dy = Odvod(Q3[i][jm][k], Q3[i][jp][k], dy);

            q4dx = Odvod(Q4[im][j][k], Q4[ip][j][k], dx);
            q4dz = Odvod(Q4[i][j][km], Q4[i][j][kp], dz);

            q5dy = Odvod(Q5[i][jm][k], Q5[i][jp][k], dy);
            q5dz = Odvod(Q5[i][j][km], Q5[i][j][kp], dz);

            #endregion

            #region Drugi odvodi

            q1d2x = Drugi_odvod(Q1[im][j][k], Q1[i][j][k], Q1[ip][j][k], dx);
            q1d2y = Drugi_odvod(Q1[i][jm][k], Q1[i][j][k], Q1[i][jp][k], dy);
            q1d2z = Drugi_odvod(Q1[i][j][km], Q1[i][j][k], Q1[i][j][kp], dz);

            q2d2x = Drugi_odvod(Q2[im][j][k], Q2[i][j][k], Q2[ip][j][k], dx);
            q2d2y = Drugi_odvod(Q2[i][jm][k], Q2[i][j][k], Q2[i][jp][k], dy);

            #endregion

            #region Mešani odvodi

            qd1 = Odvod(Q1[im][jm][k], Q1[ip][jm][k], dx);
            qd2 = Odvod(Q1[im][jp][k], Q1[ip][jp][k], dx);
            q1dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q1[im][j][km], Q1[ip][j][km], dx);
            qd2 = Odvod(Q1[im][j][kp], Q1[ip][j][kp], dx);
            q1dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q1[i][jm][km], Q1[i][jp][km], dy);
            qd2 = Odvod(Q1[i][jm][kp], Q1[i][jp][kp], dy);
            q1dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q3[im][jm][k], Q3[ip][jm][k], dx);
            qd2 = Odvod(Q3[im][jp][k], Q3[ip][jp][k], dx);
            q3dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q4[im][j][km], Q4[ip][j][km], dx);
            qd2 = Odvod(Q4[im][j][kp], Q4[ip][j][kp], dx);
            q4dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q5[i][jm][km], Q5[i][jp][km], dy);
            qd2 = Odvod(Q5[i][jm][kp], Q5[i][jp][kp], dy);
            q5dyz = Odvod(qd1, qd2, dz);

            #endregion

            #endregion

            l1 = q1d2x + q1d2y + q1d2z;
            l2 = q1d2x + q1d2y + 4.0 * q1d2z + q2d2x - q2d2y + 2.0 * q3dxy - q4dxz - q5dyz;
            l3_1 = 2.0 * q1dz * q1dz - q5dz * q1dy - q1dy * q1dy + q1dy * q2dy - 2.0 * q5 * q1dz + q2 * q1d2y - q4dz * q1dx - q3dy * q1dx - q1dx * q1dx;
            l3_2 = -q1dx * q2dx - q1dy * q3dx - q1dz * (q5dy + q4dx) - 2.0 * q4 * q1dxz - 2.0 * q3 * q1dxy + q1 * (2.0 * q1d2z - q1d2y - q1d2x) - q2 * q1d2x; 
            //l3_1 = q1 * (q1d2x + q1d2y - 4.0 * q1d2z) + q2 * (q1d2x + q1d2y) + 2.0 * q3 * q1dxy + 2.0 * q4 * q1dxz + 2.0 * q5 * q1dyz;
            //  l3_2 = q1dx * q1dx + q1dy * q1dy - 2.0 * q1dz * q1dz + q1dx * q2dx - q1dy * q2dy + q1dx * q3dy + q1dy * q3dx + q1dx * q4dz + q1dz * q4dx + q1dy * q5dz + q1dz * q5dy;

            result = L1 * l1 + L2 * l2 / 6.0 + L3 * (l3_1 + l3_2);

            /*using (StreamWriter writer = new StreamWriter("L1L2L3.txt", true))
            {
                writer.WriteLine("{0,8:F2}  {1,8:F2}  {2,8:F2}  {3,8:F2}  {4,8:F2}  {5,8:F2}  {6,8:F2}  {7,8:F2}", Q1[i][j][km], Q1[i][j][k], Q1[i][j][kp], Q2[im][j][k], Q2[i][j][k], Q2[ip][j][k], Q2[i][jm][k], Q2[i][jp][k]);
            }*/

            return result;
        }

        static double ELelastic_term_Q2(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, l1, l2, l3_1, l3_2;
            double q1dx, q1dy, q1dz, q2dx, q2dy, q2dz, q3dx, q3dy, q4dx, q4dz, q5dy, q5dz;
            double q1d2x, q1d2y, q2d2x, q2d2y, q2d2z, qd1, qd2, q2dxy, q2dxz, q2dyz, q4dxz, q5dyz;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun odvodov

            #region Prvi odvodi

            q1dx = Odvod(Q1[im][j][k], Q1[ip][j][k], dx);
            q1dy = Odvod(Q1[i][jm][k], Q1[i][jp][k], dy);
            q1dz = Odvod(Q1[i][j][km], Q1[i][j][kp], dz);

            q2dx = Odvod(Q2[im][j][k], Q2[ip][j][k], dx);
            q2dy = Odvod(Q2[i][jm][k], Q2[i][jp][k], dy);
            q2dz = Odvod(Q2[i][j][km], Q2[i][j][kp], dz);

            q3dx = Odvod(Q3[im][j][k], Q3[ip][j][k], dx);
            q3dy = Odvod(Q3[i][jm][k], Q3[i][jp][k], dy);

            q4dx = Odvod(Q4[im][j][k], Q4[ip][j][k], dx);
            q4dz = Odvod(Q4[i][j][km], Q4[i][j][kp], dz);

            q5dy = Odvod(Q5[i][jm][k], Q5[i][jp][k], dy);
            q5dz = Odvod(Q5[i][j][km], Q5[i][j][kp], dz);

            #endregion

            #region Drugi odvodi

            q1d2x = Drugi_odvod(Q1[im][j][k], Q1[i][j][k], Q1[ip][j][k], dx);
            q1d2y = Drugi_odvod(Q1[i][jm][k], Q1[i][j][k], Q1[i][jp][k], dy);

            q2d2x = Drugi_odvod(Q2[im][j][k], Q2[i][j][k], Q2[ip][j][k], dx);
            q2d2y = Drugi_odvod(Q2[i][jm][k], Q2[i][j][k], Q2[i][jp][k], dy);
            q2d2z = Drugi_odvod(Q2[i][j][km], Q2[i][j][k], Q2[i][j][kp], dz);

            #endregion

            #region Mešani odvodi

            qd1 = Odvod(Q2[im][jm][k], Q2[ip][jm][k], dx);
            qd2 = Odvod(Q2[im][jp][k], Q2[ip][jp][k], dx);
            q2dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q2[im][j][km], Q2[ip][j][km], dx);
            qd2 = Odvod(Q2[im][j][kp], Q2[ip][j][kp], dx);
            q2dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q2[i][jm][km], Q2[i][jp][km], dy);
            qd2 = Odvod(Q2[i][jm][kp], Q2[i][jp][kp], dy);
            q2dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q4[im][j][km], Q4[ip][j][km], dx);
            qd2 = Odvod(Q4[im][j][kp], Q4[ip][j][kp], dx);
            q4dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q5[i][jm][km], Q5[i][jp][km], dy);
            qd2 = Odvod(Q5[i][jm][kp], Q5[i][jp][kp], dy);
            q5dyz = Odvod(qd1, qd2, dz);

            #endregion

            #endregion

            l1 = q2d2x + q2d2y + q2d2z;
            l2 = q2d2x + q2d2y + q1d2x - q1d2y + q4dxz - q5dyz;
            l3_1 = 2.0 * q1dz * q2dz + q5dz * q2dy + q1dy * q2dy - q2dy * q2dy + q2dz * q5dy + 2.0 * q5 * q2dyz - q2 * q2d2y + q4dz * q2dx + q3dy * q2dx;
            l3_2 = q1dx * q2dx + q2dx * q2dx + q2dy * q3dx + q2dz * q4dx + 2.0 * q4 * q2dxz + 2.0 * q3 * q2dxy + q2dx * q2dx + q1 * (q2d2x + q2d2y - 2.0 * q2d2z);
            //l3_1 = q1 * (q2d2x + q2d2y - 2.0 * q2d2z) + q2 * (q2d2x - q2d2y) + 2.0 * q3 * q2dxy + 2.0 * q4 * q2dxz + 2.0 * q5 * q2dyz;
            //l3_2 = q1dx * q2dx + q1dy * q2dy - 2.0 * q1dz * q2dz + q2dx * q2dx - q2dy * q2dy + q2dx * q3dy + q2dy * q3dx + q2dx * q4dz + q2dz * q4dx + q2dy * q5dz + q2dz * q5dy;

            result = L1 * l1 + L2 * l2 / 2.0 + L3 * (l3_1 + l3_2);

            return result;
        }

        static double ELelastic_term_Q3(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, l1, l2, l3_1, l3_2;
            double q1dx, q1dy, q1dz, q2dx, q2dy, q3dx, q3dy, q3dz, q4dx, q4dz, q5dy, q5dz;
            double q3d2x, q3d2y, q3d2z, qd1, qd2, q1dxy, q3dxy, q3dxz, q3dyz, q4dxz, q4dyz, q5dxz, q5dyz;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun odvodov

            #region Prvi in drugi odvodi

            q1dx = Odvod(Q1[im][j][k], Q1[ip][j][k], dx);
            q1dy = Odvod(Q1[i][jm][k], Q1[i][jp][k], dy);
            q1dz = Odvod(Q1[i][j][km], Q1[i][j][kp], dz);

            q2dx = Odvod(Q2[im][j][k], Q2[ip][j][k], dx);
            q2dy = Odvod(Q2[i][jm][k], Q2[i][jp][k], dy);

            q3dx = Odvod(Q3[im][j][k], Q3[ip][j][k], dx);
            q3dy = Odvod(Q3[i][jm][k], Q3[i][jp][k], dy);
            q3dz = Odvod(Q3[i][j][km], Q3[i][j][kp], dz);

            q4dx = Odvod(Q4[im][j][k], Q4[ip][j][k], dx);
            q4dz = Odvod(Q4[i][j][km], Q4[i][j][kp], dz);

            q5dy = Odvod(Q5[i][jm][k], Q5[i][jp][k], dy);
            q5dz = Odvod(Q5[i][j][km], Q5[i][j][kp], dz);

            q3d2x = Drugi_odvod(Q3[im][j][k], Q3[i][j][k], Q3[ip][j][k], dx);
            q3d2y = Drugi_odvod(Q3[i][jm][k], Q3[i][j][k], Q3[i][jp][k], dy);
            q3d2z = Drugi_odvod(Q3[i][j][km], Q3[i][j][k], Q3[i][j][kp], dz);

            #endregion

            #region Mešani odvodi

            qd1 = Odvod(Q1[im][jm][k], Q1[ip][jm][k], dx);
            qd2 = Odvod(Q1[im][jp][k], Q1[ip][jp][k], dx);
            q1dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q3[im][jm][k], Q3[ip][jm][k], dx);
            qd2 = Odvod(Q3[im][jp][k], Q3[ip][jp][k], dx);
            q3dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q3[im][j][km], Q3[ip][j][km], dx);
            qd2 = Odvod(Q3[im][j][kp], Q3[ip][j][kp], dx);
            q3dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q3[i][jm][km], Q3[i][jp][km], dy);
            qd2 = Odvod(Q3[i][jm][kp], Q3[i][jp][kp], dy);
            q3dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q4[im][j][km], Q4[ip][j][km], dx);
            qd2 = Odvod(Q4[im][j][kp], Q4[ip][j][kp], dx);
            q4dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q4[i][jm][km], Q4[i][jp][km], dy);
            qd2 = Odvod(Q4[i][jm][kp], Q4[i][jp][kp], dy);
            q4dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q5[im][j][km], Q5[ip][j][km], dx);
            qd2 = Odvod(Q5[im][j][kp], Q5[ip][j][kp], dx);
            q5dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q5[i][jm][km], Q5[i][jp][km], dy);
            qd2 = Odvod(Q5[i][jm][kp], Q5[i][jp][kp], dy);
            q5dyz = Odvod(qd1, qd2, dz);

            #endregion

            #endregion

            l1 = q3d2x + q3d2y + q3d2z;
            l2 = q3d2x + q3d2y + 2.0 * q1dxy + q4dyz + q5dxz;
            l3_1 = -2.0 * q1dz * q3dz - 2.0 * q1 * q3d2z + q5dz * q3dy + q1dy * q3dy - q2dy * q3dy + q3dz * q5dy + 2.0 * q5 * q3dyz + q1 * q3d2y - q2 * q3d2y;
            l3_2 = q4dz * q3dx + 2.0 * q3dy * q3dx + q1dx * q3dx + q2dx * q3dx + q3dz * q4dx + 2.0 * q4 * q3dxz + 2.0 * q3 * q3dxy + q1 * q3d2x + q2 * q3d2x;
            //l3_1 = q1 * (q3d2x + q3d2y - 2.0 * q3d2z) + q2 * (q3d2x - q3d2y) + 2.0 * q3 * q3dxy + 2.0 * q4 * q3dxz + 2.0 * q5 * q3dyz;
            //l3_2 = q1dx * q3dx + q1dy * q3dy - 2.0 * q1dz * q3dz + q2dx * q3dx - q2dy * q3dy + 2.0 * q3dx * q3dy + q3dx * q4dz + q3dz * q4dx + q3dy * q5dz + q3dz * q5dy;

            result = L1 * l1 + L2 * l2 / 2.0 + L3 * (l3_1 + l3_2);

            return result;
        }

        static double ELelastic_term_Q4(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, l1, l2, l3_1, l3_2;
            double q1dx, q1dy, q1dz, q2dx, q2dy, q3dx, q3dy, q4dx, q4dy, q4dz, q5dy, q5dz;
            double q4d2x, q4d2y, q4d2z, qd1, qd2, q1dxz, q2dxz, q3dyz, q4dxy, q4dxz, q4dyz, q5dxy;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun odvodov

            #region Prvi in drugi odvodi

            q1dx = Odvod(Q1[im][j][k], Q1[ip][j][k], dx);
            q1dy = Odvod(Q1[i][jm][k], Q1[i][jp][k], dy);
            q1dz = Odvod(Q1[i][j][km], Q1[i][j][kp], dz);

            q2dx = Odvod(Q2[im][j][k], Q2[ip][j][k], dx);
            q2dy = Odvod(Q2[i][jm][k], Q2[i][jp][k], dy);

            q3dx = Odvod(Q3[im][j][k], Q3[ip][j][k], dx);
            q3dy = Odvod(Q3[i][jm][k], Q3[i][jp][k], dy);

            q4dx = Odvod(Q4[im][j][k], Q4[ip][j][k], dx);
            q4dy = Odvod(Q4[i][jm][k], Q4[i][jp][k], dy);
            q4dz = Odvod(Q4[i][j][km], Q4[i][j][kp], dz);

            q5dy = Odvod(Q5[i][jm][k], Q5[i][jp][k], dy);
            q5dz = Odvod(Q5[i][j][km], Q5[i][j][kp], dz);

            q4d2x = Drugi_odvod(Q4[im][j][k], Q4[i][j][k], Q4[ip][j][k], dx);
            q4d2y = Drugi_odvod(Q4[i][jm][k], Q4[i][j][k], Q4[i][jp][k], dy);
            q4d2z = Drugi_odvod(Q4[i][j][km], Q4[i][j][k], Q4[i][j][kp], dz);

            #endregion

            #region Mešani odvodi

            qd1 = Odvod(Q1[im][j][km], Q1[ip][j][km], dx);
            qd2 = Odvod(Q1[im][j][kp], Q1[ip][j][kp], dx);
            q1dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q2[im][j][km], Q2[ip][j][km], dx);
            qd2 = Odvod(Q2[im][j][kp], Q2[ip][j][kp], dx);
            q2dxz = Odvod(qd1, qd2, dz);
            qd1 = Odvod(Q3[i][jm][km], Q3[i][jp][km], dy);
            qd2 = Odvod(Q3[i][jm][kp], Q3[i][jp][kp], dy);
            q3dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q4[im][j][km], Q4[ip][j][km], dx);
            qd2 = Odvod(Q4[im][j][kp], Q4[ip][j][kp], dx);
            q4dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q4[im][j][km], Q4[ip][j][km], dx);
            qd2 = Odvod(Q4[im][j][kp], Q4[ip][j][kp], dx);
            q4dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q4[i][jm][km], Q4[i][jp][km], dy);
            qd2 = Odvod(Q4[i][jm][kp], Q4[i][jp][kp], dy);
            q4dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q5[im][jm][k], Q5[ip][jm][k], dx);
            qd2 = Odvod(Q5[im][jp][k], Q5[ip][jp][k], dx);
            q5dxy = Odvod(qd1, qd2, dy);

            #endregion

            #endregion

            l1 = q4d2x + q4d2y + q4d2z;
            l2 = q4d2x + q4d2z - q1dxz + q2dxz + q3dyz + q5dxy;
            l3_1 = -2.0 * q1dz * q4dz - 2.0 * q1 * q4d2z + q5dz * q4dy + q1dy * q4dy - q2dy * q4dy + q4dz * q5dy + 2.0 * q5 * q4dyz + q1 * q4d2y - q2 * q4d2y;
            l3_2 = q4dy * q3dx + 2.0 * q4dz * q4dx + q3dy * q4dx + q1dx * q4dx + q2dx * q4dx + 2.0 * q4 * q4dxz + 2.0 * q3 * q4dxy + q1 * q4d2x + q2 * q4d2x;
            //l3_1 = q1 * (q4d2x + q4d2y - 2.0 * q4d2z) + q2 * (q4d2x - q4d2y) + 2.0 * q3 * q4dxy + 2.0 * q4 * q4dxz + 2.0 * q5 * q4dyz;
            //l3_2 = q1dx * q4dx + q1dy * q4dy - 2.0 * q1dz * q4dz + q2dx * q4dx - q2dy * q4dy + q3dx * q4dy + q3dy * q4dx + 2.0 * q4dx * q4dz + q4dy * q5dz + q4dz * q5dy;

            result = L1 * l1 + L2 * l2 / 2.0 + L3 * (l3_1 + l3_2);

            return result;
        }

        static double ELelastic_term_Q5(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, l1, l2, l3_1, l3_2;
            double q1dx, q1dy, q1dz, q2dx, q2dy, q3dx, q3dy, q4dx, q4dz, q5dx, q5dy, q5dz;
            double q5d2x, q5d2y, q5d2z, qd1, qd2, q1dyz, q2dyz, q3dxz, q4dxy, q5dxy, q5dxz, q5dyz;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun odvodov

            #region Prvi in drugi odvodi

            q1dx = Odvod(Q1[im][j][k], Q1[ip][j][k], dx);
            q1dy = Odvod(Q1[i][jm][k], Q1[i][jp][k], dy);
            q1dz = Odvod(Q1[i][j][km], Q1[i][j][kp], dz);

            q2dx = Odvod(Q2[im][j][k], Q2[ip][j][k], dx);
            q2dy = Odvod(Q2[i][jm][k], Q2[i][jp][k], dy);

            q3dx = Odvod(Q3[im][j][k], Q3[ip][j][k], dx);
            q3dy = Odvod(Q3[i][jm][k], Q3[i][jp][k], dy);

            q4dx = Odvod(Q4[im][j][k], Q4[ip][j][k], dx);
            q4dz = Odvod(Q4[i][j][km], Q4[i][j][kp], dz);

            q5dx = Odvod(Q5[im][j][k], Q5[ip][j][k], dx);
            q5dy = Odvod(Q5[i][jm][k], Q5[i][jp][k], dy);
            q5dz = Odvod(Q5[i][j][km], Q5[i][j][kp], dz);

            q5d2x = Drugi_odvod(Q5[im][j][k], Q5[i][j][k], Q5[ip][j][k], dx);
            q5d2y = Drugi_odvod(Q5[i][jm][k], Q5[i][j][k], Q5[i][jp][k], dy);
            q5d2z = Drugi_odvod(Q5[i][j][km], Q5[i][j][k], Q5[i][j][kp], dz);

            #endregion

            #region Mešani odvodi

            qd1 = Odvod(Q1[i][jm][km], Q1[i][jp][km], dy);
            qd2 = Odvod(Q1[i][jm][kp], Q1[i][jp][kp], dy);
            q1dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q2[i][jm][km], Q2[i][jp][km], dy);
            qd2 = Odvod(Q2[i][jm][kp], Q2[i][jp][kp], dy);
            q2dyz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q3[im][j][km], Q3[ip][j][km], dx);
            qd2 = Odvod(Q3[im][j][kp], Q3[ip][j][kp], dx);
            q3dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q4[im][j][km], Q4[ip][j][km], dx);
            qd2 = Odvod(Q4[im][j][kp], Q4[ip][j][kp], dx);
            q4dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q5[im][jm][k], Q5[ip][jm][k], dx);
            qd2 = Odvod(Q5[im][jp][k], Q5[ip][jp][k], dx);
            q5dxy = Odvod(qd1, qd2, dy);

            qd1 = Odvod(Q5[im][j][km], Q5[ip][j][km], dx);
            qd2 = Odvod(Q5[im][j][kp], Q5[ip][j][kp], dx);
            q5dxz = Odvod(qd1, qd2, dz);

            qd1 = Odvod(Q5[i][jm][km], Q5[i][jp][km], dy);
            qd2 = Odvod(Q5[i][jm][kp], Q5[i][jp][kp], dy);
            q5dyz = Odvod(qd1, qd2, dz);

            #endregion

            #endregion

            l1 = q5d2x + q5d2y + q5d2z;
            l2 = q5d2y + q5d2z - q1dyz - q2dyz + q3dxz + q4dxy;
            l3_1 = -2.0 * q1dz * q5dz - 2.0 * q1 * q5d2z + 2.0 * q5dz * q5dy + q1dy * q5dy - q2dy * q5dy + 2.0 * q5 * q5dyz + q1 * q5d2y - q2 * q5d2y;
            l3_2 = q5dy * q3dx + q5dz * q4dx + q5dx * (q4dz + q3dy + q1dx + q2dx) + 2.0 * q4 * q5dxz + 2.0 * q3 * q5dxy + q1 * q5d2x + q2 * q5d2x;
            //l3_1 = q1 * (q5d2x + q5d2y - 2.0 * q5d2z) + q2 * (q5d2x - q5d2y) + 2.0 * q3 * q5dxy + 2.0 * q4 * q5dxz + 2.0 * q5 * q5dyz;
            //l3_2 = q1dx * q5dx + q1dy * q5dy - 2.0 * q1dz * q5dz + q2dx * q5dx - q2dy * q5dy + q3dx * q5dy + q3dy * q5dx + q4dx * q5dz + q4dz * q5dx + 2.0 * q5dy * q5dz;

            result = L1 * l1 + L2 * l2 / 2.0 + L3 * (l3_1 + l3_2);

            return result;
        }

        #endregion

        #endregion
        
        #region Časovno neodvisne funkcije z inserti

        private void Iteration_inserts()
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_inserts());

                Task.WaitAll(tasks);
                
                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_inserts());

                Task.WaitAll(tasks);
                
                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 20 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis(it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        private void Iteration_dir_inserts(string dir)
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_inserts());

                Task.WaitAll(tasks);
                
                Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    //Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_inserts());

                Task.WaitAll(tasks);
                
                Zgornja_meja_tangential_degenerate(Q2, Q3);

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    //Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 1000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis_dir(dir, it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        #region Zanke za Q -> Q_n

        static bool Iteracija_Q1_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q1_n[i][j][k] = Next_value_Q1(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q2_n[i][j][k] = Next_value_Q2(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q3_n[i][j][k] = Next_value_Q3(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q4_n[i][j][k] = Next_value_Q4(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q5_n[i][j][k] = Next_value_Q5(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Zanke za Q_n -> Q

        static bool Iteracija_Q1_n_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q1[i][j][k] = Next_value_Q1(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_n_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q2[i][j][k] = Next_value_Q2(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_n_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q3[i][j][k] = Next_value_Q3(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_n_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q4[i][j][k] = Next_value_Q4(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_n_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q5[i][j][k] = Next_value_Q5(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        #endregion

        #endregion

        #region Časovno odvisne funkcije z inserti
        
        private void Iteration_v2_inserts()
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_v2_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_v2_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_v2_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_v2_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_v2_inserts());

                Task.WaitAll(tasks);
                
                Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_v2_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_v2_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_v2_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_v2_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_v2_inserts());

                Task.WaitAll(tasks);
                
                Zgornja_meja_tangential_degenerate(Q2, Q3);

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 1000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);
                    Vmesni_izpis(it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        private void Iteration_v2_dir_inserts(string dir)
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_v2_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_v2_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_v2_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_v2_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_v2_inserts());

                Task.WaitAll(tasks);
                
                Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_v2_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_v2_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_v2_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_v2_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_v2_inserts());

                Task.WaitAll(tasks);
                
                Zgornja_meja_tangential_degenerate(Q2, Q3);

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 2000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis_dir(dir, it);
                }

                it += 2;

                if (it % 60000 == 0)
                {
                    #region Setting up the next round

                    if (changemode == 2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (j == 0)
                                {
                                    defekti_down[2 * i + j][1] += 1.0;
                                }
                                if (j == 1)
                                {
                                    defekti_down[2 * i + j][1] -= 1.0;
                                }
                            }
                        }

                        double theta, phi;

                        for (int i = 0; i < Nx; i++)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                #region Nastavitev pogojev

                                if (lower_boundary == 0) // Defect
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;

                                    for (int d = 0; d < defekti_down.Length; d++)
                                    {
                                        phi += defekti_down[d][2] * Math.Atan2(j - defekti_down[d][1], i - defekti_down[d][0]);
                                    }
                                }

                                else if (lower_boundary == 1) //Tangential
                                {
                                    theta = Math.PI / 2.0;
                                    phi = phi0_lower;
                                }

                                else if (lower_boundary == 2) //Tangential degenerate
                                {
                                    theta = Math.PI / 2.0;
                                    phi = Math.PI * r.NextDouble();
                                }

                                else // Homeotropic
                                {
                                    theta = 0.0;
                                    phi = 0.0;
                                }

                                #endregion

                                #region Izračun vrednosti

                                Q1[i][j][0] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                                Q2[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                                Q3[i][j][0] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                                Q4[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                                Q5[i][j][0] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                                Q1_n[i][j][0] = Q1[i][j][0];
                                Q2_n[i][j][0] = Q2[i][j][0];
                                Q3_n[i][j][0] = Q3[i][j][0];
                                Q4_n[i][j][0] = Q4[i][j][0];
                                Q5_n[i][j][0] = Q5[i][j][0];

                                #endregion
                            }
                        }
                    }

                    #endregion

                    #region Changing E

                    if (E_changing == 1) { Ex += 0.01; }
                    else if (E_changing == 2) { Ey += 0.01; }
                    else if (E_changing == 3) { Ez += 0.01; }

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                E[i][j][k][0] = Ex;
                                E[i][j][k][1] = Ey;
                                E[i][j][k][2] = Ez;
                            }
                        }
                    }

                    #endregion
                }
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        #region Zanke za Q -> Q_n

        static bool Iteracija_Q1_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }
                        
                        Q1_n[i][j][k] = Next_value_Q1_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }
                        
                        Q2_n[i][j][k] = Next_value_Q2_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }
                        
                        Q3_n[i][j][k] = Next_value_Q3_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }
                        
                        Q4_n[i][j][k] = Next_value_Q4_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }
                        
                        Q5_n[i][j][k] = Next_value_Q5_dt(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Zanke za Q_n -> Q

        static bool Iteracija_Q1_n_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q1[i][j][k] = Next_value_Q1_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_n_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q2[i][j][k] = Next_value_Q2_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_n_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q3[i][j][k] = Next_value_Q3_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_n_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q4[i][j][k] = Next_value_Q4_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_n_v2_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q5[i][j][k] = Next_value_Q5_dt(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        #endregion

        #endregion

        #region Različne elastične konstante z inserti

        private void Iteration_unequal_L_inserts()
        {
            it = 0;
            nap = 1;

            using (StreamWriter writer = new StreamWriter("L1L2L3.txt", false))
            {

            }

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_unequal_L_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_unequal_L_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_unequal_L_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_unequal_L_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_unequal_L_inserts());

                Task.WaitAll(tasks);

                Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_unequal_L_inserts());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_unequal_L_inserts());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_unequal_L_inserts());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_unequal_L_inserts());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_unequal_L_inserts());

                Task.WaitAll(tasks);

                Zgornja_meja_tangential_degenerate(Q2, Q3);

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }
                #endregion

                progressBar1.Increment(2);

                if (it % 1000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis(it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        #region Zanke za Q -> Q_n

        static bool Iteracija_Q1_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q1_n[i][j][k] = Next_value_Q1_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q2_n[i][j][k] = Next_value_Q2_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q3_n[i][j][k] = Next_value_Q3_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q4_n[i][j][k] = Next_value_Q4_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q5_n[i][j][k] = Next_value_Q5_L(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Zanke za Q_n -> Q

        static bool Iteracija_Q1_n_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q1[i][j][k] = Next_value_Q1_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_n_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q2[i][j][k] = Next_value_Q2_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_n_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q3[i][j][k] = Next_value_Q3_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_n_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q4[i][j][k] = Next_value_Q4_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_n_unequal_L_inserts()
        {
            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        if (matter[i][j][k] == 1 || matter[i][j][k] == 2)
                        {
                            continue;
                        }

                        Q5[i][j][k] = Next_value_Q5_L(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        #endregion

        #endregion

        #region Časovno neodvisne kiralne funkcije

        private void Iteration_chiral()
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_chiral());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_chiral());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_chiral());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_chiral());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_chiral());

                Task.WaitAll(tasks);

                Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_chiral());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_chiral());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_chiral());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_chiral());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_chiral());

                Task.WaitAll(tasks);

                Zgornja_meja_tangential_degenerate(Q2, Q3);

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 1000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis(it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        private void Iteration_dir_chiral(string dir)
        {
            it = 0;
            nap = 1;

            Task<bool>[] tasks = new Task<bool>[5];

            while (nap > 0 && it < itmax)
            {
                nap = 0;

                #region Q -> Q_n

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_chiral());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_chiral());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_chiral());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_chiral());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_chiral());

                Task.WaitAll(tasks);

                Zgornja_meja_tangential_degenerate(Q2_n, Q3_n);

                if (sides == 0)
                {
                    Zunanja_meja(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    Weak_top_boundary(Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                }
                else
                {
                    Top_boundary(Q2_n, Q3_n);
                }

                #endregion

                #region Q_n -> Q

                tasks[0] = Task.Factory.StartNew(() => Iteracija_Q1_n_chiral());
                tasks[1] = Task.Factory.StartNew(() => Iteracija_Q2_n_chiral());
                tasks[2] = Task.Factory.StartNew(() => Iteracija_Q3_n_chiral());
                tasks[3] = Task.Factory.StartNew(() => Iteracija_Q4_n_chiral());
                tasks[4] = Task.Factory.StartNew(() => Iteracija_Q5_n_chiral());

                Task.WaitAll(tasks);

                Zgornja_meja_tangential_degenerate(Q2, Q3);

                if (sides == 0)
                {
                    Zunanja_meja(Q1, Q2, Q3, Q4, Q5);
                }

                if (w < 1000)
                {
                    Weak_bottom_boundary(Q1, Q2, Q3, Q4, Q5);
                    Weak_top_boundary(Q1, Q2, Q3, Q4, Q5);
                }
                else
                {
                    Top_boundary(Q2, Q3);
                }

                #endregion

                progressBar1.Increment(2);

                if (it % 2000 == 0)
                {
                    Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

                    Vmesni_izpis_dir(dir, it);
                }

                it += 2;
            }

            #region Calculating total energy

            double f_f, F_f;
            F_f = 0.0;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                        F_f += f_f * dx * dy * dz;
                    }
                }
            }
            textBox1.Text = F_f.ToString();

            #endregion
        }

        #region Zanke za Q -> Q_n

        static bool Iteracija_Q1_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q1_n[i][j][k] = Next_value_Q1(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1_n[i][j][k] = Next_value_Q1_chiral(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q2_n[i][j][k] = Next_value_Q2(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2_n[i][j][k] = Next_value_Q2_chiral(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q3_n[i][j][k] = Next_value_Q3(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3_n[i][j][k] = Next_value_Q3_chiral(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q4_n[i][j][k] = Next_value_Q4(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4_n[i][j][k] = Next_value_Q4_chiral(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q5_n[i][j][k] = Next_value_Q5(i, j, k, Q1, Q2, Q3, Q4, Q5);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5_n[i][j][k] = Next_value_Q5_chiral(i, j, k, Q1, Q2, Q3, Q4, Q5);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Zanke za Q_n -> Q

        static bool Iteracija_Q1_n_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q1[i][j][k] = Next_value_Q1(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q1[i][j][k] = Next_value_Q1_chiral(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q2_n_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q2[i][j][k] = Next_value_Q2(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q2[i][j][k] = Next_value_Q2_chiral(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q3_n_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q3[i][j][k] = Next_value_Q3(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q3[i][j][k] = Next_value_Q3_chiral(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q4_n_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q4[i][j][k] = Next_value_Q4(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q4[i][j][k] = Next_value_Q4_chiral(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        static bool Iteracija_Q5_n_chiral()
        {
            /*for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    if ((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2) < Nx * Ny / 4)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            Q5[i][j][k] = Next_value_Q5(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                        }
                    }
                }
            }*/

            for (int i = i_lower; i < i_upper; i++)
            {
                for (int j = j_lower; j < j_upper; j++)
                {
                    for (int k = k_lower; k < k_upper; k++)
                    {
                        Q5[i][j][k] = Next_value_Q5_chiral(i, j, k, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n);
                    }
                }
            }

            return true;
        }

        #endregion

        #region Izračun naslednjih vrednosti

        /// <summary>
        /// Izračuna naslednje stanje za Q1
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q1_chiral(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z;
            double d_x, d_y, d_z, q4dy, q5dx;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q1[im][j][k], Q1[i][j][k], Q1[ip][j][k], dx);
            d_y = Drugi_odvod(Q1[i][jm][k], Q1[i][j][k], Q1[i][jp][k], dy);
            d_z = Drugi_odvod(Q1[i][j][km], Q1[i][j][k], Q1[i][j][kp], dz);

            q4dy = Odvod(Q4[i][jm][k], Q4[i][jp][k], dy);
            q5dx = Odvod(Q5[im][j][k], Q5[ip][j][k], dx);

            #region Mixed stuff (not in use)
            /*
            d_xy = Drugi_odvod(Q1[im][jm][k], Q1[i][j][k], Q1[ip][jp][k], dxy);
            d_xz = Drugi_odvod(Q1[im][j][km], Q1[i][j][k], Q1[ip][j][kp], dxz);
            d_yz = Drugi_odvod(Q1[i][jm][km], Q1[i][j][k], Q1[i][jp][kp], dyz);

            d_x1 = Drugi_odvod(Q1[im][jp][k], Q1[i][j][k], Q1[ip][jm][k], dxy);
            d_x2 = Drugi_odvod(Q1[im][j][kp], Q1[i][j][k], Q1[ip][j][km], dxz);
            
            d_y1 = Drugi_odvod(Q1[ip][jm][k], Q1[i][j][k], Q1[im][jp][k], dxy);
            d_y2 = Drugi_odvod(Q1[i][jm][kp], Q1[i][j][k], Q1[i][jp][km], dyz);

            d_z1 = Drugi_odvod(Q1[ip][j][km], Q1[i][j][k], Q1[im][j][kp], dxz);
            d_z2 = Drugi_odvod(Q1[i][jp][km], Q1[i][j][k], Q1[i][jm][kp], dyz);

            d_xyz1 = Drugi_odvod(Q1[im][jm][km], Q1[i][j][k], Q1[ip][jp][kp], dxyz);
            d_xyz2 = Drugi_odvod(Q1[im][jm][kp], Q1[i][j][k], Q1[ip][jp][km], dxyz);
            d_xyz3 = Drugi_odvod(Q1[im][jp][km], Q1[i][j][k], Q1[ip][jm][kp], dxyz);
            d_xyz4 = Drugi_odvod(Q1[ip][jm][km], Q1[i][j][k], Q1[im][jp][kp], dxyz);

            d_ext = d_xy + d_xz + d_yz + d_x1 + d_x2 + d_y1 + d_y2 + d_z1 + d_z2 + d_xyz1 + d_xyz2 + d_xyz3 + d_xyz4;
            */
            #endregion

            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0 + deps * (E_x * E_x + E_y * E_y - 2.0 * E_z * E_z) / 12.0;
            dEL_en = -t / 6.0 - 2.0 * q1 - (tr_Q2 + 6.0 * q1 * q1) / 2.0;

            EL_en = d_x + d_y + d_z + L_chiral * (-3.0 * q4dy + 3.0 * q5dx) / 12.0 + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q1[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q1[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q2
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q2_chiral(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z;
            double d_x, d_y, d_z, q3dz, q4dy, q5dx;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q2[im][j][k], Q2[i][j][k], Q2[ip][j][k], dx);
            d_y = Drugi_odvod(Q2[i][jm][k], Q2[i][j][k], Q2[i][jp][k], dy);
            d_z = Drugi_odvod(Q2[i][j][km], Q2[i][j][k], Q2[i][j][kp], dz);

            q3dz = Odvod(Q3[i][j][km], Q3[i][j][kp], dz);
            q4dy = Odvod(Q4[i][jm][k], Q4[i][jp][k], dy);
            q5dx = Odvod(Q5[im][j][k], Q5[ip][j][k], dx);

            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0 + deps * (E_x * E_x - E_y * E_y) / 4.0;
            dEL_en = -t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q2 * q2) / 2.0;

            EL_en = d_x + d_y + d_z + L_chiral * (2.0 * q3dz - q4dy - q5dx) / 4.0 + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q2[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q2[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q3
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q3_chiral(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z;
            double d_x, d_y, d_z, q2dz, q4dx, q5dy;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q3[im][j][k], Q3[i][j][k], Q3[ip][j][k], dx);
            d_y = Drugi_odvod(Q3[i][jm][k], Q3[i][j][k], Q3[i][jp][k], dy);
            d_z = Drugi_odvod(Q3[i][j][km], Q3[i][j][k], Q3[i][j][kp], dz);

            q2dz = Odvod(Q2[i][j][km], Q2[i][j][kp], dz);
            q4dx = Odvod(Q4[im][j][k], Q4[ip][j][k], dx);
            q5dy = Odvod(Q5[i][jm][k], Q5[i][jp][k], dy);

            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + deps * E_x * E_y / 2.0;
            dEL_en = -t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q3 * q3) / 2.0;

            EL_en = d_x + d_y + d_z + L_chiral * (-2.0 * q2dz + q4dx - q5dy) / 4.0 + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q3[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q3[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q4
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q4_chiral(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z;
            double d_x, d_y, d_z, q1dy, q2dy, q3dx, q5dz;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q4[im][j][k], Q4[i][j][k], Q4[ip][j][k], dx);
            d_y = Drugi_odvod(Q4[i][jm][k], Q4[i][j][k], Q4[i][jp][k], dy);
            d_z = Drugi_odvod(Q4[i][j][km], Q4[i][j][k], Q4[i][j][kp], dz);

            q1dy = Odvod(Q1[i][jm][k], Q1[i][jp][k], dy);
            q2dy = Odvod(Q2[i][jm][k], Q2[i][jp][k], dy);
            q3dx = Odvod(Q3[im][j][k], Q3[ip][j][k], dx);
            q5dz = Odvod(Q5[i][j][km], Q5[i][j][kp], dz);

            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + deps * E_x * E_z / 2.0;
            dEL_en = -t / 6.0 - q1 + q2 - (tr_Q2 + 2.0 * q4 * q4) / 2.0;

            EL_en = d_x + d_y + d_z + L_chiral * (3.0 * q1dy + q2dy - q3dx + q5dz) / 4.0 + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q4[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q4[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Izračuna naslednje stanje za Q5
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        static double Next_value_Q5_chiral(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z;
            double d_x, d_y, d_z, q1dx, q2dx, q3dy, q4dz;
            double result = 0.0;

            q1 = Q1[i][j][k];
            q2 = Q2[i][j][k];
            q3 = Q3[i][j][k];
            q4 = Q4[i][j][k];
            q5 = Q5[i][j][k];

            E_x = E[i][j][k][0];
            E_y = E[i][j][k][1];
            E_z = E[i][j][k][2];

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (im < 0)
                {
                    im = Nx - 1;
                }
                if (ip == Nx)
                {
                    ip = 0;
                }

                if (jm < 0)
                {
                    jm = Ny - 1;
                }
                if (jp == Ny)
                {
                    jp = 0;
                }

                if (km < 0)
                {
                    km = Nz - 1;
                }
                if (kp == Nz)
                {
                    kp = 0;
                }
            }

            #endregion

            #endregion

            #region Izračun

            #region Odvodi

            d_x = Drugi_odvod(Q5[im][j][k], Q5[i][j][k], Q5[ip][j][k], dx);
            d_y = Drugi_odvod(Q5[i][jm][k], Q5[i][j][k], Q5[i][jp][k], dy);
            d_z = Drugi_odvod(Q5[i][j][km], Q5[i][j][k], Q5[i][j][kp], dz);

            q1dx = Odvod(Q1[im][j][k], Q1[ip][j][k], dx);
            q2dx = Odvod(Q2[im][j][k], Q2[ip][j][k], dx);
            q3dy = Odvod(Q3[i][jm][k], Q3[i][jp][k], dy);
            q4dz = Odvod(Q4[i][j][km], Q4[i][j][kp], dz);

            #endregion

            tr_Q2 = Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0 + deps * E_y * E_z / 2.0;
            dEL_en = -t / 6.0 - q1 - q2 - (tr_Q2 + 2.0 * q5 * q5) / 2.0;

            EL_en = d_x + d_y + d_z + L_chiral * (- 3.0 * q1dx + q2dx + q3dy - q4dz) / 4.0 + EL_en * AA;
            dEL_en = -2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz) + dEL_en * AA;

            result = Q5[i][j][k] - kor * EL_en / dEL_en;

            if (Math.Abs(Q5[i][j][k] - result) > eps)
            {
                nap++;
            }

            #endregion

            return result;
        }

        #endregion

        #endregion

        #endregion


        #region Boundary conditions

        static void Spodnja_free_meja(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 1; j < Ny; j++)
                {
                    q1[i][j][0] = q1[i][j][1]; //tt / 6.0;
                    q2[i][j][0] = q2[i][j][1];
                    q3[i][j][0] = q3[i][j][1];
                    q4[i][j][0] = q4[i][j][1];
                    q5[i][j][0] = q5[i][j][1];
                }
            }
        }

        static void Zgornja_free_meja(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 1; j < Ny; j++)
                {
                    q1[i][j][Nz - 1] = q1[i][j][Nz - 2]; //tt / 6.0;
                    q2[i][j][Nz - 1] = q2[i][j][Nz - 2];
                    q3[i][j][Nz - 1] = q3[i][j][Nz - 2];
                    q4[i][j][Nz - 1] = q4[i][j][Nz - 2];
                    q5[i][j][Nz - 1] = q5[i][j][Nz - 2];
                }
            }
        }

        /// <summary>
        /// Orientira molekule na robu v enaki smeri kot so sosednje eno celico znotraj
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Zunanja_meja(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int k = 1; k < Nz - 1; k++)
                {
                    q1[i][0][k] = q1[i][1][k]; //tt / 6.0;
                    q2[i][0][k] = q2[i][1][k];
                    q3[i][0][k] = q3[i][1][k];
                    q4[i][0][k] = q4[i][1][k];
                    q5[i][0][k] = q5[i][1][k];

                    q1[i][Ny - 1][k] = q1[i][Ny - 2][k];//tt / 6.0; 
                    q2[i][Ny - 1][k] = q2[i][Ny - 2][k];
                    q3[i][Ny - 1][k] = q3[i][Ny - 2][k];
                    q4[i][Ny - 1][k] = q4[i][Ny - 2][k];
                    q5[i][Ny - 1][k] = q5[i][Ny - 2][k];
                }
            }

            for (int j = 0; j < Ny; j++)
            {
                for (int k = 1; k < Nz - 1; k++)
                {
                    q1[0][j][k] = q1[1][j][k];//tt / 6.0;
                    q2[0][j][k] = q2[1][j][k];
                    q3[0][j][k] = q3[1][j][k];
                    q4[0][j][k] = q4[1][j][k];
                    q5[0][j][k] = q5[1][j][k];

                    q1[Nx - 1][j][k] = q1[Nx - 2][j][k];//tt / 6.0;
                    q2[Nx - 1][j][k] = q2[Nx - 2][j][k];
                    q3[Nx - 1][j][k] = q3[Nx - 2][j][k];
                    q4[Nx - 1][j][k] = q4[Nx - 2][j][k];
                    q5[Nx - 1][j][k] = q5[Nx - 2][j][k];
                }
            }
        }
        
        /// <summary>
        /// Orientira molekule na robu kot Williams domene
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Zunanja_meja_Williams(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5, double t)
        {
            double theta_w, phi_w;
            double theta_0 = 15.0 * Math.PI / 180.0;

            for (int i = 0; i < Nx; i++)
            {
                for (int k = 1; k < Nz - 1; k++)
                {
                    theta_w = theta_0 * Math.Sin(k * Math.PI / Nz) * (1.0 - Math.Cos(t * Math.PI)) / 2.0;
                    phi_w = Math.Atan2(-Ny / 2, i - Nx / 2);

                    q1[i][0][k] = tt * (1.0 / 6.0 - (Math.Cos(theta_w) * Math.Cos(theta_w)) / 2.0);
                    q2[i][0][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Cos(2.0 * phi_w)) / 2.0;
                    q3[i][0][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Sin(2.0 * phi_w)) / 2.0;
                    q4[i][0][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Cos(phi_w)) / 2.0;
                    q5[i][0][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Sin(phi_w)) / 2.0;

                    phi_w = Math.Atan2(Ny / 2, i - Nx / 2);

                    q1[i][Ny - 1][k] = tt * (1.0 / 6.0 - (Math.Cos(theta_w) * Math.Cos(theta_w)) / 2.0);
                    q2[i][Ny - 1][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Cos(2.0 * phi_w)) / 2.0;
                    q3[i][Ny - 1][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Sin(2.0 * phi_w)) / 2.0;
                    q4[i][Ny - 1][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Cos(phi_w)) / 2.0;
                    q5[i][Ny - 1][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Sin(phi_w)) / 2.0;
                }
            }

            for (int j = 0; j < Ny; j++)
            {
                for (int k = 1; k < Nz - 1; k++)
                {
                    theta_w = theta_0 * Math.Sin(k * Math.PI / Nz) * (1.0 - Math.Cos((t - 2.0 * j / Ny) * Math.PI)) / 2.0;
                    phi_w = Math.Atan2(j - Ny / 2, -Nx / 2);

                    q1[0][j][k] = tt * (1.0 / 6.0 - (Math.Cos(theta_w) * Math.Cos(theta_w)) / 2.0);
                    q2[0][j][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Cos(2.0 * phi_w)) / 2.0;
                    q3[0][j][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Sin(2.0 * phi_w)) / 2.0;
                    q4[0][j][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Cos(phi_w)) / 2.0;
                    q5[0][j][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Sin(phi_w)) / 2.0;

                    phi_w = Math.Atan2(j - Ny / 2, Nx / 2);

                    q1[Nx - 1][j][k] = tt * (1.0 / 6.0 - (Math.Cos(theta_w) * Math.Cos(theta_w)) / 2.0);
                    q2[Nx - 1][j][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Cos(2.0 * phi_w)) / 2.0;
                    q3[Nx - 1][j][k] = tt * (Math.Sin(theta_w) * Math.Sin(theta_w) * Math.Sin(2.0 * phi_w)) / 2.0;
                    q4[Nx - 1][j][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Cos(phi_w)) / 2.0;
                    q5[Nx - 1][j][k] = tt * (Math.Sin(2.0 * theta_w) * Math.Sin(phi_w)) / 2.0;
                }
            }
        }

        /// <summary>
        /// Orientira molekule na vrhu v enaki smeri kot so sosednje eno celico vniže
        /// </summary>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        static void Zgornja_meja_tangential_degenerate(double[][][] q2, double[][][] q3)
        {
            double dvafi;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    /*if (Math.Abs(q2[i][j][Nz - 2]) < eps)
                    {
                        dvafi = Math.PI / 2.0;
                    }
                    else
                    {
                        dvafi = Math.Atan2(q3[i][j][Nz - 2], q2[i][j][Nz - 2]);
                    }*/

                    dvafi = Math.Atan2(q3[i][j][Nz - 2], q2[i][j][Nz - 2]);

                    Q2_plate[i][j][1] = tt * Math.Cos(dvafi) / 2.0;//q2[i][j][Nz - 2];
                    Q3_plate[i][j][1] = tt * Math.Sin(dvafi) / 2.0;//q3[i][j][Nz - 2];
                }
            }
        }

        /// <summary>
        /// Orientira molekule na dnu v enaki smeri kot so sosednje eno celico vniže
        /// </summary>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        static void Spodnja_meja_tangential_degenerate(double[][][] q2, double[][][] q3)
        {
            double dvafi;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    /*if (Math.Abs(q2[i][j][Nz - 2]) < eps)
                    {
                        dvafi = Math.PI / 2.0;
                    }
                    else
                    {
                        dvafi = Math.Atan2(q3[i][j][Nz - 2], q2[i][j][Nz - 2]);
                    }*/

                    dvafi = Math.Atan2(q3[i][j][1], q2[i][j][1]);

                    Q2_plate[i][j][0] = tt * Math.Cos(dvafi) / 2.0;//q2[i][j][1];
                    Q3_plate[i][j][0] = tt * Math.Sin(dvafi) / 2.0;//q3[i][j][1];
                }
            }
        }

        /// <summary>
        /// Orientira molekule na dnu v enaki smeri kot so sosednje eno celico više
        /// </summary>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        static void Spodnja_meja(double[][][] q2, double[][][] q3)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    q2[i][j][0] = q2[i][j][1];
                    q3[i][j][0] = q3[i][j][1];
                }
            }
        }

        /// <summary>
        /// Orientira molekule na vrhu in dnu
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Zgornja_in_spodnja_meja(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    q1[i][j][0] = q1[i][j][1];
                    q2[i][j][0] = q2[i][j][1];
                    q3[i][j][0] = q3[i][j][1];
                    q4[i][j][0] = q4[i][j][1];
                    q5[i][j][0] = q5[i][j][1];

                    q1[i][j][Nz - 1] = q1[i][j][Nz - 2];
                    q2[i][j][Nz - 1] = q2[i][j][Nz - 2];
                    q3[i][j][Nz - 1] = q3[i][j][Nz - 2];
                    q4[i][j][Nz - 1] = q4[i][j][Nz - 2];
                    q5[i][j][Nz - 1] = q5[i][j][Nz - 2];
                }
            }
        }

        /// <summary>
        /// Orientira molekule na dnu v enaki smeri kot so sosednje eno celico više
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Spodnja_meja_2(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            double fi;
            double[,] matrika;

            double[] eigenvalues = new double[3];
            double[,] eigenvectors = new double[3, 3];

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    matrika = Pretvorba_v_matriko(q1[i][j][1], q2[i][j][1], q3[i][j][1], q4[i][j][1], q5[i][j][1]);
                    alglib.smatrixevd(matrika, 3, 1, true, out eigenvalues, out eigenvectors);

                    fi = Math.Atan2(eigenvectors[0, 2], eigenvectors[1, 2]);
                    q2[i][j][0] = tt * Math.Cos(2.0 * fi) / 2.0;
                    q3[i][j][0] = tt * Math.Sin(2.0 * fi) / 2.0;
                }
            }
        }

        /// <summary>
        /// Orientira molekule na vrhu kot so sosednje eno celico niže z weak boundary
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Weak_top_boundary(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    q1[i][j][Nz - 1] = (q1[i][j][Nz - 2] + w * dz * Q1_plate[i][j][1]) / (1 + w * dz);
                    q2[i][j][Nz - 1] = (q2[i][j][Nz - 2] + w * dz * Q2_plate[i][j][1]) / (1 + w * dz);
                    q3[i][j][Nz - 1] = (q3[i][j][Nz - 2] + w * dz * Q3_plate[i][j][1]) / (1 + w * dz);
                    q4[i][j][Nz - 1] = (q4[i][j][Nz - 2] + w * dz * Q4_plate[i][j][1]) / (1 + w * dz);
                    q5[i][j][Nz - 1] = (q5[i][j][Nz - 2] + w * dz * Q5_plate[i][j][1]) / (1 + w * dz);
                }
            }
        }

        /// <summary>
        /// Orientira molekule na dnu kot so sosednje eno celico više z weak boundary
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Weak_bottom_boundary(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    q1[i][j][0] = (q1[i][j][1] + w * dz * Q1_plate[i][j][0]) / (1 + w * dz);
                    q2[i][j][0] = (q2[i][j][1] + w * dz * Q2_plate[i][j][0]) / (1 + w * dz);
                    q3[i][j][0] = (q3[i][j][1] + w * dz * Q3_plate[i][j][0]) / (1 + w * dz);
                    q4[i][j][0] = (q4[i][j][1] + w * dz * Q4_plate[i][j][0]) / (1 + w * dz);
                    q5[i][j][0] = (q5[i][j][1] + w * dz * Q5_plate[i][j][0]) / (1 + w * dz);
                }
            }
        }
        
        /// <summary>
        /// Orientira molekule na vrhu kot so sosednje eno celico niže z weak boundary
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Top_boundary(double[][][] q2, double[][][] q3)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    q2[i][j][Nz - 1] = Q2_plate[i][j][1];
                    q3[i][j][Nz - 1] = Q3_plate[i][j][1];
                }
            }
        }

        /// <summary>
        /// Orientira molekule na dnu kot so sosednje eno celico više z weak boundary
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        static void Bottom_boundary(double[][][] q2, double[][][] q3)
        {
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    q2[i][j][0] = Q2_plate[i][j][0];
                    q3[i][j][0] = Q3_plate[i][j][0];
                }
            }
        }
        
        #endregion


        #region Analysis

        private void Add_insert_Click(object sender, EventArgs e)
        {
            int i_x, i_y, i_z, d_x, d_y, d_z, i_type, i_shape;

            i_shape = Insert_shape.SelectedIndex;
            i_type = Insert_type.SelectedIndex;

            i_x = (int)x_insert_n.Value;
            i_y = (int)y_insert_n.Value;
            i_z = (int)z_insert_n.Value;

            d_x = (int)insert_size_1_n.Value;
            d_y = (int)insert_size_2_n.Value;
            d_z = (int)insert_size_3_n.Value;

            Inserts.Add(new int[] { i_shape, i_type, i_x, i_y, i_z, d_x, d_y, d_z });
        }

        private static int[][][] Create_insert_field(int Nx, int Ny, int Nz, List<int[]> Inserts)
        {
            #region Initialization

            int[][][] result;

            result = new int[Nx][][];
            for (int i = 0; i < Nx; i++)
            {
                result[i] = new int[Ny][];

                for (int j = 0; j < Ny; j++)
                {
                    result[i][j] = new int[Nz];

                    for (int k = 0; k < Nz; k++)
                    {
                        result[i][j][k] = 0;
                    }
                }
            }

            #endregion

            #region Filling the field

            foreach (int[] insert in Inserts)
            {
                switch (insert[0])
                {
                    case 0: //Single point

                        result[insert[2]][insert[3]][insert[4]] = insert[1] + 1;

                        break;

                    case 1:
                        #region Cube

                        for (int i = insert[2] - insert[5] / 2; i < insert[2] + insert[5] / 2; i++)
                        {
                            for (int j = insert[3] - insert[6] / 2; j < insert[3] + insert[6] / 2; j++)
                            {
                                for (int k = insert[4] - insert[7] / 2; k < insert[4] + insert[7] / 2; k++)
                                {
                                    result[i][j][k] = insert[1] + 1;
                                }
                            }
                        }

                        #endregion
                        break;

                    case 2:
                        #region Ellipsoid

                        double r_i, r_j, r_k;

                        for (int i = insert[2] - insert[5]; i < insert[2] + insert[5]; i++)
                        {
                            r_i = (i - insert[2]) * (i - insert[2]) / (insert[5] * insert[5]);

                            for (int j = insert[3] - insert[6]; j < insert[3] + insert[6]; j++)
                            {
                                r_j = (j - insert[3]) * (j - insert[3]) / (insert[6] * insert[6]);

                                for (int k = insert[4] - insert[7]; k < insert[4] + insert[7]; k++)
                                {
                                    r_k = (k - insert[4]) * (k - insert[4]) / (insert[7] * insert[7]);

                                    if (r_i + r_j + r_k <= 1.0)
                                    {
                                        result[i][j][k] = insert[1] + 1;
                                    }
                                }
                            }
                        }

                        #endregion
                        break;

                    case 3:
                        #region Torus

                        double r_ij, r_ijk;

                        for (int i = insert[2] - insert[5] - insert[5]; i < insert[2] + insert[5] + insert[6]; i++)
                        {
                            for (int j = insert[3] - insert[5] - insert[6]; j < insert[3] + insert[5] + insert[6]; j++)
                            {
                                r_ij = Math.Sqrt((i - insert[2]) * (i - insert[2]) + (j - insert[3]) * (j - insert[3]));

                                for (int k = insert[4] - insert[6]; k < insert[4] + insert[6]; k++)
                                {
                                    r_ijk = (r_ij - insert[5]) * (r_ij - insert[5]) + (k - insert[4]) * (k - insert[4]);

                                    if (r_ijk < insert[6] * insert[6])
                                    {
                                        result[i][j][k] = insert[1] + 1;
                                    }
                                }
                            }
                        }

                        #endregion
                        break;

                    default:
                        break;
                }
            }

            #endregion

            return result;
        }

        private void Koti_Click(object sender, EventArgs e)
        {
            draw_size = pictureBox1.Height;
            N_r = (int)N_ravnine_n.Value;
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Text files (.txt and .dat)|*.txt; *.dat";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                #region Pomožni arrayi
                string[] datoteka = File.ReadAllLines(ofd.FileName);
                string[] data;
                string[] separators = { "\t", " " };

                int[] x_i, y_j, z_k;

                x_i = new int[datoteka.Length];
                y_j = new int[datoteka.Length];
                z_k = new int[datoteka.Length];

                #endregion

                #region Postavitev lokacij

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

                #region Inicializacija arrayev

                xv = new double[Nx];
                yv = new double[Ny];
                zv = new double[Nz];

                Q1 = new double[Nx][][];
                Q2 = new double[Nx][][];
                Q3 = new double[Nx][][];
                Q4 = new double[Nx][][];
                Q5 = new double[Nx][][];

                Q1_n = new double[Nx][][];
                Q2_n = new double[Nx][][];
                Q3_n = new double[Nx][][];
                Q4_n = new double[Nx][][];
                Q5_n = new double[Nx][][];

                b2 = new double[Nx][][];
                S = new double[Nx][][];

                E = new double[Nx][][][];
                direktor = new double[Nx][][][];

                for (int i = 0; i < Nx; i++)
                {
                    Q1[i] = new double[Ny][];
                    Q2[i] = new double[Ny][];
                    Q3[i] = new double[Ny][];
                    Q4[i] = new double[Ny][];
                    Q5[i] = new double[Ny][];

                    Q1_n[i] = new double[Ny][];
                    Q2_n[i] = new double[Ny][];
                    Q3_n[i] = new double[Ny][];
                    Q4_n[i] = new double[Ny][];
                    Q5_n[i] = new double[Ny][];

                    b2[i] = new double[Ny][];
                    S[i] = new double[Ny][];

                    E[i] = new double[Ny][][];
                    direktor[i] = new double[Ny][][];

                    for (int j = 0; j < Ny; j++)
                    {
                        Q1[i][j] = new double[Nz];
                        Q2[i][j] = new double[Nz];
                        Q3[i][j] = new double[Nz];
                        Q4[i][j] = new double[Nz];
                        Q5[i][j] = new double[Nz];

                        Q1_n[i][j] = new double[Nz];
                        Q2_n[i][j] = new double[Nz];
                        Q3_n[i][j] = new double[Nz];
                        Q4_n[i][j] = new double[Nz];
                        Q5_n[i][j] = new double[Nz];

                        b2[i][j] = new double[Nz];
                        S[i][j] = new double[Nz];

                        E[i][j] = new double[Nz][];
                        direktor[i][j] = new double[Nz][];

                        for (int k = 0; k < Nz; k++)
                        {
                            E[i][j][k] = new double[3];
                            direktor[i][j][k] = new double[3];
                        }
                    }
                }

                Parameters1.Add(Q1);
                Parameters1.Add(Q2);
                Parameters1.Add(Q3);
                Parameters1.Add(Q4);
                Parameters1.Add(Q5);

                Parameters2.Add(Q1_n);
                Parameters2.Add(Q2_n);
                Parameters2.Add(Q3_n);
                Parameters2.Add(Q4_n);
                Parameters2.Add(Q5_n);

                if (N_r > Nz)
                {
                    N_r = Nz - 1;
                }

                #endregion

                #region Določitev vrednosti

                for (int i = 0; i < datoteka.Length; i++)
                {
                    data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    Q1[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[3]);
                    Q2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[4]);
                    Q3[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[5]);
                    Q4[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[6]);
                    Q5[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[7]);

                    b2[x_i[i]][y_j[i]][z_k[i]] = double.Parse(data[8]);
                }

                #endregion

                #region Calculating total energy

                double f_f, F_f;
                F_f = 0.0;

                for (int i = i_lower; i < i_upper; i++)
                {
                    for (int j = j_lower; j < j_upper; j++)
                    {
                        for (int k = k_lower; k < k_upper; k++)
                        {
                            f_f = Energy_density(Q1, Q2, Q3, Q4, Q5, i, j, k);
                            F_f += f_f;// * dx * dy * dz;
                        }
                    }
                }
                textBox1.Text = F_f.ToString();

                #endregion

                Thread th = new Thread(Angle_calculation);
                th.IsBackground = true;
                th.Start();

                th.Join();
            }
        }

        /// <summary>
        /// Izračuna direktorsko polje, skalarni ureditveni parameter, in beta
        /// </summary>
        private void Angle_calculation()
        {
            string dir;

            Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

            #region Skalarni ureditveni parameter

            dir = "S";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S" + fname.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}", i, j, k, S[i][j][k]);
                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_points" + fname.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (S[i][j][k] < 0.75)
                            {
                                if (inserts && matter[i][j][k] != 0)
                                {
                                    continue;
                                }

                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Direktorsko polje

            dir = "Direktorsko polje";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "direktorsko_polje" + fname.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}", i, j, k, direktor[i][j][k][0], direktor[i][j][k][1], direktor[i][j][k][2]);
                        }
                    }
                }

                /*for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}", i, j, 2 * Nz - (k + 1), direktor[i][j][k][0], direktor[i][j][k][1], -1.0 * direktor[i][j][k][2]);
                        }
                    }
                }*/
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "pol_direktorsko_polje" + fname.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    if (i % 8 == 0)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            if (j % 8 == 0)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    if (k == 1 || k == Nz - 1)
                                    {
                                        writer.Write("{0,8:F4}  {1,8:F4}  {2,8:F4}", i - direktor[i][j][k][0] / 2.0, j - direktor[i][j][k][1] / 2.0, k - direktor[i][j][k][2] / 2.0);
                                        writer.WriteLine("{0,8:F4}  {1,8:F4}  {2,8:F4}", i + direktor[i][j][k][0] / 2.0, j + direktor[i][j][k][1] / 2.0, k + direktor[i][j][k][2] / 2.0);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Theta skozi celico

            double avg_cos = 0.0;

            using (StreamWriter writer = new StreamWriter("povprecni_theta_od_z.txt", false))
            {
                double avg;

                for (int k = 0; k < Nz; k++)
                {
                    avg = 0.0;

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            avg += Math.Abs(direktor[i][j][k][2]);
                            avg_cos += Math.Abs(direktor[i][j][k][2]);
                        }
                    }

                    avg = Math.Acos(avg / (Nx * Ny));
                    writer.WriteLine("{0,4}  {1,8:F4}", k, avg);
                }

                avg_cos = avg_cos / (Nx * Ny * Nz);
            }

            using (StreamWriter writer = new StreamWriter("odvisnost_theta_od_phi.txt", false))
            {
                double avg1, avg2;

                for (int i = 0; i < Nx; i++)
                {
                    avg1 = 0.0;
                    avg2 = 0.0;

                    for (int k = 0; k < Nz; k++)
                    {
                        avg1 += Math.Abs(direktor[i][i][k][2]);
                        avg2 += Math.Abs(direktor[i][Ny / 2][k][2]);
                    }

                    avg1 = avg1 / Nz;
                    avg2 = avg2 / Nz;

                    writer.WriteLine("{0,4}  {1,8:F4}  {2,8:F4}  {3,8:F4}", (i - 50), avg2, (i - 50) * Math.Sqrt(2), avg1);
                }
            }

            #endregion

            #region Preseki

            #region Folder

            dir = "Preseki";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            #endregion

            #region Ravnina xy

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xy_k" + (Nz / 2).ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(direktor[i][j][(int)(Nz / 2)][1], direktor[i][j][(int)(Nz / 2)][0]), Math.Sin(Math.Acos(direktor[i][j][(int)(Nz / 2)][2])));
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xy_k0.txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(direktor[i][j][0][1], direktor[i][j][0][0]), Math.Sin(Math.Acos(direktor[i][j][0][2])));
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xy_k" + Nz.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(direktor[i][j][Nz - 1][1], direktor[i][j][Nz - 1][0]), Math.Sin(Math.Acos(direktor[i][j][Nz - 1][2])));
                    }
                }
            }

            #endregion

            #region Ravnina xz

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xz_j" + (Ny / 2).ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    if (i % 2 == 0)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (k % 2 == 0)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, k, Math.Atan2(direktor[i][(int)(Ny / 2)][k][2], direktor[i][(int)(Ny / 2)][k][0]), Math.Cos(Math.Atan2(direktor[i][(int)(Ny / 2)][k][1], direktor[i][(int)(Ny / 2)][k][0])));
                            }
                        }
                    }
                }
            }

            #endregion

            #region Ravnina yz

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_yz_i" + (Nx / 2).ToString() + ".txt"), false))
            {
                for (int j = 0; j < Ny; j++)
                {
                    if (j % 3 == 0)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (k % 3 == 0)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", j, k, Math.Atan2(direktor[(int)(Nx / 2)][j][k][2], direktor[(int)(Nx / 2)][j][k][1]), Math.Abs(Math.Sin(Math.Atan2(direktor[(int)(Nx / 2)][j][k][1], direktor[(int)(Nx / 2)][j][k][0]))));
                            }
                        }
                    }
                }
            }

            #endregion

            #region Selected plane and level
            
            if (plane == 0)
            {
                if (N_r > Nz)
                {
                    N_r = Nz - 1;
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xy_k" + N_r.ToString() + ".txt"), false))
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        if (i % skip1 == 0)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                if (j % skip2 == 0)
                                {
                                    writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(direktor[i][j][N_r][1], direktor[i][j][N_r][0]), Math.Sin(Math.Acos(direktor[i][j][N_r][2])));
                                }
                            }
                        }
                    }
                }

            }

            else if (plane == 1)
            {
                if (N_r > Ny)
                {
                    N_r = Ny - 1;
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xz_k" + N_r.ToString() + ".txt"), false))
                {
                    for (int i = 0; i < Nx; i++)
                    {
                        if (i % skip1 == 0)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                if (k % skip2 == 0)
                                {
                                    writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, k, Math.Atan2(direktor[i][N_r][k][2], direktor[i][N_r][k][0]), Math.Cos(Math.Atan2(direktor[i][N_r][k][1], direktor[i][N_r][k][0])));
                                }
                            }
                        }
                    }
                }
            }
            
            else if (plane == 2)
            {
                if (N_r > Nx)
                {
                    N_r = Nx - 1;
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_yz_k" + N_r.ToString() + ".txt"), false))
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        if (j % skip1 == 0)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                if (k % skip2 == 0)
                                {
                                    writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", j, k, Math.Atan2(direktor[N_r][j][k][2], direktor[N_r][j][k][1]), Math.Abs(Math.Sin(Math.Atan2(direktor[N_r][j][k][1], direktor[N_r][j][k][0]))));
                                }
                            }
                        }
                    }
                }
            }
            
            #endregion

            #endregion

            #region Theta skozi razdaljo od defekta

            using (StreamWriter writer = new StreamWriter("povprecni_theta_od_r.txt", false))
            {
                double r, d;
                double[] cos_r;
                int[] count;

                d = 2.0;
                cos_r = new double[25];
                count = new int[25];

                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        r = Math.Sqrt((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2));

                        for (int rr = 0; rr < count.Length; rr++)
                        {
                            if (r <= (rr + 1) * d)
                            {
                                cos_r[rr] += Math.Abs(direktor[i][j][Nz / 2][2]);
                                count[rr] += 1;
                            }
                        }
                    }
                }

                for (int i = 0; i < count.Length; i++)
                {
                    cos_r[i] = cos_r[i] / count[i];
                    writer.WriteLine("{0,4}  {1,8:F4}", (i * d + 1), cos_r[i]);
                }
            }

            #endregion

            #region Beta

            using (StreamWriter writer = new StreamWriter("beta.txt", false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            b2[i][j][k] = Beta(Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k]);

                            if (b2[i][j][k] > 0.8)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Porazdelitev velikosti domen

            if (preveri_domene.Checked)
            {
                using (StreamWriter writer = new StreamWriter("Velikosti domen.txt", false))
                {
                    int[] domene = Domain_size_distribution(direktor);

                    for (int i = 0; i < domene.Length; i++)
                    {
                        if (domene[i] > 0)
                        {
                            writer.WriteLine("{0,6}  {1}", i + 1, domene[i]);
                        }
                    }
                }
            }

            #endregion

            Streamline(direktor, "xy", Nz / 2);
        }

        /// <summary>
        /// Izračuna streamline za določeno direktorsko polje
        /// </summary>
        /// <param name="director">Direktorsko polje</param>
        /// <param name="plane">Izbrana ravnina</param>
        /// <param name="plane_h">Lega ravnine</param>
        private void Streamline(double[][][][] director, string plane, int plane_h)
        {
            #region Initialization

            int N1, N2, loc1_i, loc1_j, loc2_i, loc2_j;
            double x_l, y_l, z_l;
            bool exited = false;
            bool[][] visited;
            double[][][] director_n;
            double[] vector = new double[3];
            List<List<double[]>> lines = new List<List<double[]>>();
            List<double[]> line = new List<double[]>();

            switch (plane)
            {
                #region Plane xy

                case "xy":
                    N1 = director.Length;
                    N2 = director[0].Length;

                    visited = new bool[N1][];
                    director_n = new double[N1][][];
                    for (int i = 0; i < N1; i++)
                    {
                        visited[i] = new bool[N2];
                        director_n[i] = new double[N2][];
                        for (int j = 0; j < N2; j++)
                        {
                            visited[i][j] = false;
                            director_n[i][j] = new double[3];

                            director_n[i][j][0] = director[i][j][plane_h][0];
                            director_n[i][j][1] = director[i][j][plane_h][1];
                            director_n[i][j][2] = director[i][j][plane_h][2];
                        }
                    }
                    break;

                #endregion
                #region Plane xz

                case "xz":
                    N1 = director.Length;
                    N2 = director[plane_h][0].Length;

                    visited = new bool[N1][];
                    director_n = new double[N1][][];
                    for (int i = 0; i < N1; i++)
                    {
                        visited[i] = new bool[N2];
                        director_n[i] = new double[N2][];
                        for (int j = 0; j < N2; j++)
                        {
                            visited[i][j] = false;
                            director_n[i][j] = new double[3];

                            director_n[i][j][0] = director[i][plane_h][j][0];
                            director_n[i][j][1] = director[i][plane_h][j][1];
                            director_n[i][j][2] = director[i][plane_h][j][2];
                        }
                    }
                    break;

                #endregion
                #region Plane yz

                case "yz":
                    N1 = director[0].Length;
                    N2 = director[plane_h][0].Length;

                    visited = new bool[N1][];
                    director_n = new double[N1][][];
                    for (int i = 0; i < N1; i++)
                    {
                        visited[i] = new bool[N2];
                        director_n[i] = new double[N2][];
                        for (int j = 0; j < N2; j++)
                        {
                            visited[i][j] = false;
                            director_n[i][j] = new double[3];

                            director_n[i][j][0] = director[plane_h][i][j][0];
                            director_n[i][j][1] = director[plane_h][i][j][1];
                            director_n[i][j][2] = director[plane_h][i][j][2];
                        }
                    }
                    break;

                #endregion
                #region Default - xy

                default:
                    N1 = director.Length;
                    N2 = director[0].Length;

                    visited = new bool[N1][];
                    director_n = new double[N1][][];
                    for (int i = 0; i < N1; i++)
                    {
                        visited[i] = new bool[N2];
                        director_n[i] = new double[N2][];
                        for (int j = 0; j < N2; j++)
                        {
                            visited[i][j] = true;
                            director_n[i][j] = new double[3];

                            director_n[i][j][0] = director[i][j][plane_h][0];
                            director_n[i][j][1] = director[i][j][plane_h][1];
                            director_n[i][j][2] = director[i][j][plane_h][2];
                        }
                    }
                    break;

                    #endregion
            }

            for (int i = 0; i < N1 - 1; i++)
            {
                for (int j = 0; j < N2 - 1; j++)
                {
                    if (director_n[i][j][0] * director_n[i + 1][j][0] + director_n[i][j][1] * director_n[i + 1][j][1] + director_n[i][j][2] * director_n[i + 1][j][2] < -0.0)
                    {
                        director_n[i + 1][j][0] = -director_n[i + 1][j][0];
                        director_n[i + 1][j][1] = -director_n[i + 1][j][1];
                        director_n[i + 1][j][2] = -director_n[i + 1][j][2];
                    }
                    if (director_n[i][j][0] * director_n[i][j + 1][0] + director_n[i][j][1] * director_n[i][j + 1][1] + director_n[i][j][2] * director_n[i][j + 1][2] < -0.0)
                    {
                        director_n[i][j + 1][0] = -director_n[i][j + 1][0];
                        director_n[i][j + 1][1] = -director_n[i][j + 1][1];
                        director_n[i][j + 1][2] = -director_n[i][j + 1][2];
                    }
                }
            }

            #endregion

            #region Calculation

            #region Random

            for (int j = 0; j < 400; j++)
            {
                loc1_i = r.Next(N1 - 1);
                loc1_j = r.Next(N2 - 1);

                if (visited[loc1_i][loc1_j]) { continue; }
                else
                {
                    x_l = (double)loc1_i;
                    y_l = (double)loc1_j;
                    z_l = 0.0;
                    line.Add(new double[] { x_l, y_l });
                    exited = false;

                    while(!visited[loc1_i][loc1_j] && !exited)
                    {
                        vector = Interpolate_2D_vector(director_n, x_l, y_l);

                        x_l += vector[0] * 0.1;
                        y_l += vector[1] * 0.1;
                        z_l += vector[2] * 0.1;

                        if (Math.Abs(z_l) > 1.0) { exited = true; break; }
                        if (x_l < 0.0 || y_l < 0.0 || x_l >= N1 - 1 || y_l >= N2 - 1) { exited = true; break; }

                        line.Add(new double[] { x_l, y_l });
                        loc2_i = (int)Math.Floor(x_l);
                        loc2_j = (int)Math.Floor(y_l);

                        if (loc2_i != loc1_i || loc2_j != loc1_j)
                        {
                            visited[loc1_i][loc1_j] = true;
                            loc1_i = loc2_i;
                            loc1_j = loc2_j;
                        }
                    } //while (!visited[loc_i][loc_j] && !exited);

                    lines.Add(new List<double[]>(line));
                    line.Clear();
                }
            }

            int line_l = 0;
            List<double[]> temp;

            for (int i = 0; i < lines.Count; i++)
            {
                line_l = i;
                for (int j = i + 1; j < lines.Count; j++)
                {
                    if (lines[j].Count > lines[line_l].Count)
                    {
                        line_l = j;
                    }
                }
                temp = new List<double[]>(lines[line_l]);
                lines.RemoveAt(line_l);
                lines.Insert(i, new List<double[]>(temp));
            }

            #endregion

            #region v2 Even
            /*
            for (int i = 0; i < N1 - 1; i += 10)
            {
                for (int j = 0; j < N2 - 1; j += 10)
                {
                    if (visited[i][j]) { continue; }
                    else
                    {
                        x_l = (double)i;
                        y_l = (double)j;
                        z_l = 0.0;
                        line.Add(new double[] { x_l, y_l });

                        loc1_i = i;
                        loc1_j = j;
                        exited = false;

                        while (!visited[loc1_i][loc1_j] && !exited)
                        {
                            vector = Interpolate_2D_vector(director_n, x_l, y_l);

                            x_l += vector[0] * 0.1;
                            y_l += vector[1] * 0.1;
                            z_l += vector[2] * 0.1;

                            if (Math.Abs(z_l) > 1.0) { exited = true; break; }
                            if (x_l < 0.0 || y_l < 0.0 || x_l >= N1 - 1 || y_l >= N2 - 1) { exited = true; break; }

                            line.Add(new double[] { x_l, y_l });
                            loc2_i = (int)Math.Floor(x_l);
                            loc2_j = (int)Math.Floor(y_l);

                            if (loc2_i != loc1_i || loc2_j != loc1_j)
                            {
                                visited[loc1_i][loc1_j] = true;
                                loc1_i = loc2_i;
                                loc1_j = loc2_j;
                            }
                        } //while (!visited[loc_i][loc_j] && !exited);

                        lines.Add(new List<double[]>(line));
                        line.Clear();
                    }
                }
            }
            */
            #endregion

            #endregion

            using (StreamWriter writer = new StreamWriter("streamlines.txt", false))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    for (int j = 0; j < lines[i].Count; j+=10)
                    {
                        writer.Write("{0,5:F3} {1,5:F3}  ", lines[i][j][0], lines[i][j][1]);
                    }
                    writer.WriteLine();
                }
            }
            using (StreamWriter writer = new StreamWriter("streamlines.pov", false))
            {
                #region Setting up the environment

                writer.WriteLine("#include \"colors.inc\"");
                writer.WriteLine("#include \"textures.inc\"");
                writer.WriteLine("#include \"shapes.inc\"");
                writer.WriteLine();

                writer.WriteLine("background { color White }");
                writer.WriteLine();

                writer.WriteLine("camera {");
                writer.WriteLine("  location <50, 50, -120>");
                writer.WriteLine("  look_at  <50, 50, 0>");
                writer.WriteLine("}");
                writer.WriteLine();

                writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
                writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
                writer.WriteLine("               adaptive 1 jitter }");
                writer.WriteLine();

                #endregion

                for (int i = 0; i < lines.Count / 2; i++)
                {
                    if (lines[i].Count < 50)
                    {
                        continue;
                    }

                    writer.Write("sphere_sweep { cubic_spline ");
                    if (lines[i].Count % 10 == 0) { writer.Write(lines[i].Count / 10); }
                    else { writer.Write(lines[i].Count / 10 + 1); }
                    writer.WriteLine(", ");
                    for (int j = 0; j < lines[i].Count; j+=10)
                    {
                        writer.WriteLine("  <{0:F3},{1:F3},0>, 0.2", lines[i][j][0], lines[i][j][1]);
                    }
                    writer.WriteLine("  texture{ pigment{ color Black}");
                    writer.WriteLine("    finish { reflection 0.05 phong 1}");
                    writer.WriteLine("  }");
                    writer.WriteLine("  no_shadow");
                    writer.WriteLine("}");
                }
            }
        }

        private double[] Interpolate_2D_vector(double[][][] field, double x, double y)
        {
            #region Initialization

            double f1, f2;
            double[] vector = new double[3];

            double x_i = Math.Floor(x);
            double y_j = Math.Floor(y);

            double x_z = x - x_i;
            double y_z = y - y_j;

            int i = (int)x_i;
            int j = (int)y_j;

            #endregion

            #region Ensuring head-to-toe symmetry

            if (field[i][j][0] * field[i + 1][j][0] + field[i][j][1] * field[i + 1][j][1] + field[i][j][2] * field[i + 1][j][2] < -0.0)
            {
                field[i + 1][j][0] = -field[i + 1][j][0];
                field[i + 1][j][1] = -field[i + 1][j][1];
                field[i + 1][j][2] = -field[i + 1][j][2];
            }
            if (field[i][j][0] * field[i][j + 1][0] + field[i][j][1] * field[i][j + 1][1] + field[i][j][2] * field[i][j + 1][2] < -0.0)
            {
                field[i][j + 1][0] = -field[i][j + 1][0];
                field[i][j + 1][1] = -field[i][j + 1][1];
                field[i][j + 1][2] = -field[i][j + 1][2];
            }
            if (field[i][j][0] * field[i + 1][j + 1][0] + field[i][j][1] * field[i + 1][j + 1][1] + field[i][j][2] * field[i + 1][j + 1][2] < -0.0)
            {
                field[i + 1][j + 1][0] = -field[i + 1][j + 1][0];
                field[i + 1][j + 1][1] = -field[i + 1][j + 1][1];
                field[i + 1][j + 1][2] = -field[i + 1][j + 1][2];
            }

            #endregion

            for (int l = 0; l < 3; l++)
            {
                f1 = field[i][j][l] * (1.0 - x_z) + field[i + 1][j][l] * x_z;
                f2 = field[i][j + 1][l] * (1.0 - x_z) + field[i + 1][j + 1][l] * x_z;

                vector[l] = f1 * (1.0 - y_z) + f2 * y_z;
            }

            return vector;
        }

        /// <summary>
        /// Izračuna direktorsko polje, skalarni ureditveni parameter, in beta
        /// </summary>
        private void Angle_calculation_dir(string dir)
        {
            Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

            #region Skalarni ureditveni parameter

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S.txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}", i, j, k, S[i][j][k] / tt);
                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_points.txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (S[i][j][k] < 0.5)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Direktorsko polje

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "direktorsko_polje.txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}", i, j, k, direktor[i][j][k][0], direktor[i][j][k][1], direktor[i][j][k][2]);
                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "pol_direktorsko_polje" + fname.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    if (i % 8 == 0)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            if (j % 8 == 0)
                            {
                                for (int k = 0; k < Nz; k++)
                                {
                                    if (k == 1 || k == Nz - 1)
                                    {
                                        writer.Write("{0,8:F4}  {1,8:F4}  {2,8:F4}", i - direktor[i][j][k][0] / 2.0, j - direktor[i][j][k][1] / 2.0, k - direktor[i][j][k][2] / 2.0);
                                        writer.WriteLine("{0,8:F4}  {1,8:F4}  {2,8:F4}", i + direktor[i][j][k][0] / 2.0, j + direktor[i][j][k][1] / 2.0, k + direktor[i][j][k][2] / 2.0);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Theta skozi celico

            double avg_cos = 0.0;

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "povprecni_theta_od_z.txt"), false))
            {
                double avg;

                for (int k = 0; k < Nz; k++)
                {
                    avg = 0.0;

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            avg += Math.Abs(direktor[i][j][k][2]);
                            avg_cos += Math.Abs(direktor[i][j][k][2]);
                        }
                    }

                    avg = Math.Acos(avg / (Nx * Ny));
                    writer.WriteLine("{0,4}  {1,8:F4}", k, avg);
                }

                avg_cos = avg_cos / (Nx * Ny * Nz);
            }

            #endregion

            #region Preseki

            #region Ravnina xy

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xy_k" + (Nz / 2).ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(direktor[i][j][(int)(Nz / 2)][1], direktor[i][j][(int)(Nz / 2)][0]), Math.Sin(Math.Acos(direktor[i][j][(int)(Nz / 2)][2])));
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xy_k0.txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(direktor[i][j][0][1], direktor[i][j][0][0]), Math.Sin(Math.Acos(direktor[i][j][0][2])));
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xy_k" + Nz.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, j, Math.Atan2(direktor[i][j][Nz - 1][1], direktor[i][j][Nz - 1][0]), Math.Sin(Math.Acos(direktor[i][j][Nz - 1][2])));
                    }
                }
            }
            
            #endregion

            #region Ravnina xz

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_xz_j" + (Ny / 2).ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", i, k, Math.Atan2(direktor[i][(int)(Ny / 2)][k][2], direktor[i][(int)(Ny / 2)][k][0]), Math.Cos(Math.Atan2(direktor[i][(int)(Ny / 2)][k][1], direktor[i][(int)(Ny / 2)][k][0])));
                    }
                }
            }

            #endregion

            #region Ravnina yz

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "presek_yz_i" + (Nx / 2).ToString() + ".txt"), false))
            {
                for (int j = 0; j < Ny; j++)
                {
                    if (j % 4 == 0)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (k % 10 == 0)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,8:F4}  {3,8:F4}", j, k, Math.Atan2(direktor[(int)(Nx / 2)][j][k][2], direktor[(int)(Nx / 2)][j][k][1]), Math.Abs(Math.Sin(Math.Atan2(direktor[(int)(Nx / 2)][j][k][1], direktor[(int)(Nx / 2)][j][k][0]))));
                            }
                        }
                    }
                }
            }

            #endregion

            #endregion

            #region Theta skozi razdaljo od defekta

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "povprecni_theta_od_r.txt"), false))
            {
                double r, d;
                double[] cos_r;
                int[] count;

                d = 2.0;
                cos_r = new double[25];
                count = new int[25];

                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            r = Math.Sqrt((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2));

                            for (int rr = 0; rr < count.Length; rr++)
                            {
                                if (r <= (rr + 1) * d)
                                {
                                    cos_r[rr] += Math.Abs(direktor[i][j][k][2]);
                                    count[rr] += 1;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < count.Length; i++)
                {
                    cos_r[i] = cos_r[i] / count[i];
                    writer.WriteLine("{0,4}  {1,8:F4}", ((i + 1 / 2) * d), cos_r[i]);
                }
            }

            #endregion

            #region Porazdelitev velikosti domen

            if (preveri_domene.Checked)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Velikosti domen.txt"), false))
                {
                    int[] domene = Domain_size_distribution(direktor);

                    for (int i = 0; i < domene.Length; i++)
                    {
                        if (domene[i] > 0)
                        {
                            writer.WriteLine("{0,6}  {1}", i + 1, domene[i]);
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Izračuna direktorsko polje iz parametriziranih spremenljivk
        /// </summary>
        /// <param name="q1">q1</param>
        /// <param name="q2">q2</param>
        /// <param name="q3">q3</param>
        /// <param name="q4">q4</param>
        /// <param name="q5">q5</param>
        static void Direktorsko_polje(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            tt = 1.0 + Math.Sqrt(1.0 - t);
            double[,] matrika;

            double[] eigenvalues = new double[3];
            double[,] eigenvectors = new double[3, 3];

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        matrika = Pretvorba_v_matriko(q1[i][j][k], q2[i][j][k], q3[i][j][k], q4[i][j][k], q5[i][j][k]);

                        alglib.smatrixevd(matrika, 3, 1, true, out eigenvalues, out eigenvectors);

                        S[i][j][k] = Half_trQ_square(q1[i][j][k], q2[i][j][k], q3[i][j][k], q4[i][j][k], q5[i][j][k]);
                        S[i][j][k] = Math.Sqrt(2.0 * S[i][j][k]) / tt;

                        direktor[i][j][k][0] = eigenvectors[0, 2];
                        direktor[i][j][k][1] = eigenvectors[1, 2];
                        direktor[i][j][k][2] = eigenvectors[2, 2];

                        if (direktor[i][j][k][0] < 0.0)
                        {
                            direktor[i][j][k][0] = -direktor[i][j][k][0];
                            direktor[i][j][k][1] = -direktor[i][j][k][1];
                            direktor[i][j][k][2] = -direktor[i][j][k][2];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Izračuna direktorsko polje iz parametriziranih spremenljivk
        /// </summary>
        /// <param name="q1">q1</param>
        /// <param name="q2">q2</param>
        /// <param name="q3">q3</param>
        /// <param name="q4">q4</param>
        /// <param name="q5">q5</param>
        static void Direktorsko_polje_2(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5)
        {
            double fi, teta;
            
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        fi = Math.Atan2(q3[i][j][k], q2[i][j][k]);
                        fi = fi / 2.0;
                        
                        teta = Math.Abs(1.0 / 3.0 - 2 * q1[i][j][k] / tt);
                        //teta = Math.Acos(Math.Sqrt(teta));

                        //teta = 2.0 * q2[i][j][k] / (tt * Math.Cos(2 * fi));
                        if (teta < 0.0)
                        {
                            teta = - Math.Acos(Math.Sqrt(-teta));
                        }
                        else
                        {
                            teta = Math.Acos(Math.Sqrt(teta));
                        }
                        
                        S[i][j][k] = Half_trQ_square(q1[i][j][k], q2[i][j][k], q3[i][j][k], q4[i][j][k], q5[i][j][k]);
                        S[i][j][k] = Math.Sqrt(2.0 * S[i][j][k]) / tt;

                        direktor[i][j][k][0] = Math.Sin(teta) * Math.Cos(fi);
                        direktor[i][j][k][1] = Math.Sin(teta) * Math.Sin(fi);
                        direktor[i][j][k][2] = Math.Cos(teta);

                        if (direktor[i][j][k][0] < 0.0)
                        {
                            direktor[i][j][k][0] = -direktor[i][j][k][0];
                            direktor[i][j][k][1] = -direktor[i][j][k][1];
                            direktor[i][j][k][2] = -direktor[i][j][k][2];
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Pretvori parametre v matrično obliko
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        /// <returns></returns>
        static double[,] Pretvorba_v_matriko(double q1, double q2, double q3, double q4, double q5)
        {
            double[,] matrix = new double[3, 3];

            matrix[0, 0] = q1 + q2;
            matrix[1, 1] = q1 - q2;
            matrix[2, 2] = -2.0 * q1;

            matrix[0, 1] = q3;
            matrix[0, 2] = q4;
            matrix[1, 2] = q5;
            /*
            matrix[1, 0] = q3;
            matrix[2, 0] = q4;
            matrix[2, 1] = q5;
            */
            return matrix;
        }

        /// <summary>
        /// Izračuna gostoto energije v točki z lego (i,j,k)
        /// </summary>
        /// <param name="q1">Parameter q1</param>
        /// <param name="q2">Parameter q2</param>
        /// <param name="q3">Parameter q3</param>
        /// <param name="q4">Parameter q4</param>
        /// <param name="q5">Parameter q5</param>
        /// <param name="i">Lega v x smeri</param>
        /// <param name="j">Lega v y smeri</param>
        /// <param name="k">Lega v z smeri</param>
        /// <returns></returns>
        static double Energy_density(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5, int i, int j, int k)
        {
            int ip, im, jp, jm, kp, km;
            double dq1, dq1x, dq1y, dq1z, dq2, dq2x, dq2y, dq2z, dq3, dq3x, dq3y, dq3z, dq4, dq4x, dq4y, dq4z, dq5, dq5x, dq5y, dq5z;
            double f_f_partial, f_c, f_e, f_f, Tr2, Tr3, result, d_x, d_y, d_z;

            #region Kondenzacijski člen

            Tr2 = 2.0 * Half_trQ_square(q1[i][j][k], q2[i][j][k], q3[i][j][k], q4[i][j][k], q5[i][j][k]);
            Tr3 = TrQ_cubed(q1[i][j][k], q2[i][j][k], q3[i][j][k], q4[i][j][k], q5[i][j][k]);
            f_c = tt * Tr2 / 6.0 - 2.0 * Tr3 / 3.0 + Tr2 * Tr2 / 8.0;

            #endregion

            #region Elastični člen

            #region Nastavitev i+1 ipd.

            d_x = dx;
            d_y = dy;
            d_z = dz;

            im = i - 1;
            ip = i + 1;
            jm = j - 1;
            jp = j + 1;
            km = k - 1;
            kp = k + 1;

            #region Periodični robni pogoji

            if (im < 0 || ip == Nx || jm < 0 || jp == Ny || km < 0 || kp == Nz)
            {
                if (i_lower == 0)
                {
                    if (im < 0)
                    {
                        im = Nx - 1;
                    }
                    if (ip == Nx)
                    {
                        ip = 0;
                    }

                    if (jm < 0)
                    {
                        jm = Ny - 1;
                    }
                    if (jp == Ny)
                    {
                        jp = 0;
                    }
                }

                else
                {
                    if (im < 0)
                    {
                        d_x = dx / 2.0;
                        
                        im = 0;
                    }
                    if (ip == Nx)
                    {
                        d_x = dx / 2.0;
                        
                        ip = Nx - 1;
                    }

                    if (jm < 0)
                    {
                        d_y = dy / 2.0;

                        jm = 0;
                    }
                    if (jp == Ny)
                    {
                        d_y = dy / 2.0;

                        jp = Ny - 1;
                    }
                }

                if (k_lower == 0)
                {
                    if (km < 0)
                    {
                        km = Nz - 1;
                    }
                    if (kp == Nz)
                    {
                        kp = 0;
                    }
                }
                
                else
                {
                    if (km < 0)
                    {
                        d_z = dz / 2.0;

                        km = 0;
                    }
                    if (kp == Nz)
                    {
                        d_z = dz / 2.0;

                        kp = Nz - 1;
                    }
                }
            }

            #endregion

            #endregion

            #region Odvodi

            dq1x = Odvod(Q1[im][j][k], Q1[ip][j][k], d_x);
            dq1y = Odvod(Q1[i][jm][k], Q1[i][jp][k], d_y);
            dq1z = Odvod(Q1[i][j][km], Q1[i][j][kp], d_z);

            dq2x = Odvod(Q2[im][j][k], Q2[ip][j][k], d_x);
            dq2y = Odvod(Q2[i][jm][k], Q2[i][jp][k], d_y);
            dq2z = Odvod(Q2[i][j][km], Q2[i][j][kp], d_z);

            dq3x = Odvod(Q3[im][j][k], Q3[ip][j][k], d_x);
            dq3y = Odvod(Q3[i][jm][k], Q3[i][jp][k], d_y);
            dq3z = Odvod(Q3[i][j][km], Q3[i][j][kp], d_z);

            dq4x = Odvod(Q4[im][j][k], Q4[ip][j][k], d_x);
            dq4y = Odvod(Q4[i][jm][k], Q4[i][jp][k], d_y);
            dq4z = Odvod(Q4[i][j][km], Q4[i][j][kp], d_z);

            dq5x = Odvod(Q5[im][j][k], Q5[ip][j][k], d_x);
            dq5y = Odvod(Q5[i][jm][k], Q5[i][jp][k], d_y);
            dq5z = Odvod(Q5[i][j][km], Q5[i][j][kp], d_z);

            dq1 = dq1x * dq1x + dq1y * dq1y + dq1z * dq1z;
            dq2 = dq2x * dq2x + dq2y * dq2y + dq2z * dq2z;
            dq3 = dq3x * dq3x + dq3y * dq3y + dq3z * dq3z;
            dq4 = dq4x * dq4x + dq4y * dq4y + dq4z * dq4z;
            dq5 = dq5x * dq5x + dq5y * dq5y + dq5z * dq5z;

            #endregion

            f_e = 2.0 * (3.0 * dq1 + dq2 + dq3 + dq4 + dq5) / AA;

            #endregion

            #region Člen zunanjega polja

            f_f_partial = (q1[i][j][k] + q2[i][j][k]) * Ex * Ex + (q1[i][j][k] - q2[i][j][k]) * Ey * Ey - 2.0 * q1[i][j][k] * Ez * Ez;
            f_f_partial += 2.0 * (q3[i][j][k] * Ex * Ey + q4[i][j][k] * Ex * Ez + q5[i][j][k] * Ey * Ez);

            f_f = f_f_partial / 2.0;

            #endregion

            result = f_c + f_e + f_f;

            return result;
        }

        /// <summary>
        /// Izračuna gostoto energije v točki z lego (i,j,k)
        /// </summary>
        /// <param name="q1">Parameter q1</param>
        /// <param name="q2">Parameter q2</param>
        /// <param name="q3">Parameter q3</param>
        /// <param name="q4">Parameter q4</param>
        /// <param name="q5">Parameter q5</param>
        /// <param name="i">Lega v x smeri</param>
        /// <param name="j">Lega v y smeri</param>
        /// <param name="k">Lega v z smeri</param>
        /// <returns></returns>
        static double Energy_density_v2(double[][][] q1, double[][][] q2, double[][][] q3, double[][][] q4, double[][][] q5, int i, int j, int k)
        {
            double dq1, dq1x, dq1y, dq1z, dq2, dq2x, dq2y, dq2z, dq3, dq3x, dq3y, dq3z, dq4, dq4x, dq4y, dq4z, dq5, dq5x, dq5y, dq5z;
            double f_f_partial, f_c, f_e, f_f, Tr2, Tr3, result;

            #region Kondenzacijski člen

            Tr2 = 2.0 * Half_trQ_square(q1[i][j][k], q2[i][j][k], q3[i][j][k], q4[i][j][k], q5[i][j][k]);
            Tr3 = TrQ_cubed(q1[i][j][k], q2[i][j][k], q3[i][j][k], q4[i][j][k], q5[i][j][k]);
            f_c = tt * Tr2 / 6.0 - 2.0 * Tr3 / 3.0 + Tr2 * Tr2 / 8.0;

            #endregion

            #region Elastični člen

            #region Odvodi

            if (i == Nx - 1)
            {
                dq1x = 0.0;
                dq2x = 0.0;
                dq3x = 0.0;
                dq4x = 0.0;
                dq5x = 0.0;
            }
            else
            {
                dq1x = Odvod(Q1[i][j][k], Q1[i + 1][j][k], dx / 2.0);
                dq2x = Odvod(Q2[i][j][k], Q2[i + 1][j][k], dx / 2.0);
                dq3x = Odvod(Q3[i][j][k], Q3[i + 1][j][k], dx / 2.0);
                dq4x = Odvod(Q4[i][j][k], Q4[i + 1][j][k], dx / 2.0);
                dq5x = Odvod(Q5[i][j][k], Q5[i + 1][j][k], dx / 2.0);
            }

            if (j == Ny - 1)
            {
                dq1y = 0.0;
                dq2y = 0.0;
                dq3y = 0.0;
                dq4y = 0.0;
                dq5y = 0.0;
            }
            else
            {
                dq1y = Odvod(Q1[i][j][k], Q1[i][j + 1][k], dy / 2.0);
                dq2y = Odvod(Q2[i][j][k], Q2[i][j + 1][k], dy / 2.0);
                dq3y = Odvod(Q3[i][j][k], Q3[i][j + 1][k], dy / 2.0);
                dq4y = Odvod(Q4[i][j][k], Q4[i][j + 1][k], dy / 2.0);
                dq5y = Odvod(Q5[i][j][k], Q5[i][j + 1][k], dy / 2.0);
            }

            if (k == Nz - 1)
            {
                dq1z = 0.0;
                dq2z = 0.0;
                dq3z = 0.0;
                dq4z = 0.0;
                dq5z = 0.0;
            }
            else
            {
                dq1z = Odvod(Q1[i][j][k], Q1[i][j][k + 1], dz / 2.0);
                dq2z = Odvod(Q2[i][j][k], Q2[i][j][k + 1], dz / 2.0);
                dq3z = Odvod(Q3[i][j][k], Q3[i][j][k + 1], dz / 2.0);
                dq4z = Odvod(Q4[i][j][k], Q4[i][j][k + 1], dz / 2.0);
                dq5z = Odvod(Q5[i][j][k], Q5[i][j][k + 1], dz / 2.0);
            }


            dq1 = dq1x * dq1x + dq1y * dq1y + dq1z * dq1z;
            dq2 = dq2x * dq2x + dq2y * dq2y + dq2z * dq2z;
            dq3 = dq3x * dq3x + dq3y * dq3y + dq3z * dq3z;
            dq4 = dq4x * dq4x + dq4y * dq4y + dq4z * dq4z;
            dq5 = dq5x * dq5x + dq5y * dq5y + dq5z * dq5z;

            #endregion

            f_e = 2.0 * (3.0 * dq1 + dq2 + dq3 + dq4 + dq5) / AA;

            #endregion

            #region Člen zunanjega polja

            f_f_partial = (q1[i][j][k] + q2[i][j][k]) * Ex * Ex + (q1[i][j][k] - q2[i][j][k]) * Ey * Ey - 2.0 * q1[i][j][k] * Ez * Ez;
            f_f_partial += 2.0 * (q3[i][j][k] * Ex * Ey + q4[i][j][k] * Ex * Ez + q5[i][j][k] * Ey * Ez);

            f_f = f_f_partial / 2.0;

            #endregion

            result = f_c + f_e + f_f;

            return result;
        }

        /// <summary>
        /// Izračuna porazdelitev velikosti domen v sistemu
        /// </summary>
        /// <param name="d_n">Direktorsko polje</param>
        /// <returns>Porazdelitev velikosti domen v sistemu</returns>
        static int[] Domain_size_distribution(double[][][][] d_n)
        {
            #region Initialization

            int N_domains = 0;
            int ii, jj, kk, dn_old, dn_new, domena, max;
            double cos_n, cos_n_min;

            int[] Domain_sizes, result;
            int[][][] Numbering;

            Numbering = new int[Nx][][];
            for (int i = 0; i < Nx; i++)
            {
                Numbering[i] = new int[Ny][];
                for (int j = 0; j < Nz; j++)
                {
                    Numbering[i][j] = new int[Nz];
                }
            }

            cos_n_min = 0.8;

            #endregion

            #region Checking each cell individually

            for (int i = 0; i < Nx; i++) // boundary layers evaluated separately
            {
                for (int j = 0; j < Ny; j++)
                {
                    Numbering[i][j][0] = 0;
                    Numbering[i][j][Nz - 1] = 0;
                }
            }

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 1; k < Nz - 1; k++)
                    {
                        cos_n = d_n[i][j][k][0] * d_n[i][j][k - 1][0] + d_n[i][j][k][1] * d_n[i][j][k - 1][1] + d_n[i][j][k][2] * d_n[i][j][k - 1][2];

                        if (cos_n < cos_n_min)
                        {
                            N_domains++;
                        }
                        Numbering[i][j][k] = N_domains;
                    }

                    #region Last cell

                    cos_n = d_n[i][j][Nz - 2][0] * d_n[i][j][Nz - 1][0] + d_n[i][j][Nz - 2][1] * d_n[i][j][Nz - 1][1] + d_n[i][j][Nz - 2][2] * d_n[i][j][Nz - 1][2];
                    //checking if the last cell is a part of the domain of the upper layer

                    if (cos_n > cos_n_min) //making sure that if any previous cells are in the same domain that they get the proper domain number
                    {
                        Numbering[i][j][Nz - 2] = 0;

                        for (int k = Nz - 3; k >= 0; k--)
                        {
                            if (Numbering[i][j][k] == N_domains)
                            {
                                Numbering[i][j][k] = 0;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    #endregion
                }
            }

            #endregion

            #region Filling the list

            List<int[]>[] Groups = new List<int[]>[N_domains + 1]; // array: all the groups, list: each group, int[]: location of each point in the group
            int[] D_list = new int[N_domains + 1]; // Array with the domain every group belongs to

            for (int i = 0; i < Groups.Length; i++)
            {
                Groups[i] = new List<int[]>();

                D_list[i] = i;
            }

            int group_n;
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 1; k < Nz - 1; k++)
                    {
                        group_n = Numbering[i][j][k];

                        Groups[group_n].Add(new int[] { i, j, k });
                    }
                }
            }

            #endregion

            #region Fixing the values between lines

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 1; j < Ny; j++)
                {
                    for (int k = 1; k < Nz - 1; k++)
                    {
                        if (Numbering[i][j][k] != Numbering[i][j - 1][k]) // making sure it doesn't check when not neccessary
                        {
                            cos_n = d_n[i][j][k][0] * d_n[i][j - 1][k][0] + d_n[i][j][k][1] * d_n[i][j - 1][k][1] + d_n[i][j][k][2] * d_n[i][j - 1][k][2];

                            if (cos_n > cos_n_min)
                            {
                                if (Numbering[i][j][k] > Numbering[i][j - 1][k])
                                {
                                    dn_old = Numbering[i][j][k];
                                    dn_new = Numbering[i][j - 1][k];
                                }

                                else
                                {
                                    dn_old = Numbering[i][j - 1][k];
                                    dn_new = Numbering[i][j][k];
                                }

                                for (int l = 0; l < D_list.Length; l++) // checking the array for all groups in the old domain
                                {
                                    if (D_list[l] == dn_old)
                                    {
                                        D_list[l] = dn_new; //changing the domain it belongs to

                                        domena = D_list[l]; // numerical name of the group

                                        for (int m = 0; m < Groups[domena].Count; m++) // renumbering the selected group
                                        {
                                            ii = Groups[domena][m][0];
                                            jj = Groups[domena][m][1];
                                            kk = Groups[domena][m][2];

                                            Numbering[ii][jj][kk] = dn_new;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #region Final line

                for (int k = 1; k < Nz - 1; k++)
                {
                    if (Numbering[i][Ny - 1][k] != Numbering[i][0][k])
                    {
                        cos_n = d_n[i][Ny - 1][k][0] * d_n[i][0][k][0] + d_n[i][Ny - 1][k][1] * d_n[i][0][k][1] + d_n[i][Ny - 1][k][2] * d_n[i][0][k][2];

                        if (cos_n > cos_n_min)
                        {
                            if (Numbering[i][Ny - 1][k] > Numbering[i][0][k])
                            {
                                dn_old = Numbering[i][Ny - 1][k];
                                dn_new = Numbering[i][0][k];
                            }

                            else
                            {
                                dn_old = Numbering[i][0][k];
                                dn_new = Numbering[i][Ny - 1][k];
                            }

                            for (int l = 0; l < D_list.Length; l++) // checking the array for all groups in the old domain
                            {
                                if (D_list[l] == dn_old)
                                {
                                    D_list[l] = dn_new; //changing the domain it belongs to

                                    domena = D_list[l]; // numerical name of the group

                                    for (int m = 0; m < Groups[domena].Count; m++) // renumbering the selected group
                                    {
                                        ii = Groups[domena][m][0];
                                        jj = Groups[domena][m][1];
                                        kk = Groups[domena][m][2];

                                        Numbering[ii][jj][kk] = dn_new;
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            #endregion

            #region Fixing the values between planes

            for (int i = 1; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 1; k < Nz - 1; k++)
                    {
                        if (Numbering[i][j][k] != Numbering[i - 1][j][k])
                        {
                            cos_n = d_n[i][j][k][0] * d_n[i - 1][j][k][0] + d_n[i][j][k][1] * d_n[i - 1][j][k][1] + d_n[i][j][k][2] * d_n[i - 1][j][k][2];

                            if (cos_n > cos_n_min)
                            {
                                if (Numbering[i][j][k] > Numbering[i - 1][j][k])
                                {
                                    dn_old = Numbering[i][j][k];
                                    dn_new = Numbering[i - 1][j][k];
                                }

                                else
                                {
                                    dn_old = Numbering[i - 1][j][k];
                                    dn_new = Numbering[i][j][k];
                                }

                                for (int l = 0; l < D_list.Length; l++) // checking the array for all groups in the old domain
                                {
                                    if (D_list[l] == dn_old)
                                    {
                                        D_list[l] = dn_new; //changing the domain it belongs to

                                        domena = D_list[l]; // numerical name of the group

                                        for (int m = 0; m < Groups[domena].Count; m++) // renumbering the selected group
                                        {
                                            ii = Groups[domena][m][0];
                                            jj = Groups[domena][m][1];
                                            kk = Groups[domena][m][2];

                                            Numbering[ii][jj][kk] = dn_new;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #region Final plane

            for (int j = 0; j < Ny; j++)
            {
                for (int k = 1; k < Nz - 1; k++)
                {
                    if (Numbering[Nx - 1][j][k] != Numbering[0][j][k])
                    {
                        cos_n = d_n[Nx - 1][j][k][0] * d_n[0][j][k][0] + d_n[Nx - 1][j][k][1] * d_n[0][j][k][1] + d_n[Nx - 1][j][k][2] * d_n[0][j][k][2];

                        if (cos_n > cos_n_min)
                        {
                            if (Numbering[Nx - 1][j][k] > Numbering[0][j][k])
                            {
                                dn_old = Numbering[Nx - 1][j][k];
                                dn_new = Numbering[0][j][k];
                            }

                            else
                            {
                                dn_old = Numbering[0][j][k];
                                dn_new = Numbering[Nx - 1][j][k];
                            }

                            for (int l = 0; l < D_list.Length; l++) // checking the array for all groups in the old domain
                            {
                                if (D_list[l] == dn_old)
                                {
                                    D_list[l] = dn_new; //changing the domain it belongs to

                                    domena = D_list[l]; // numerical name of the group

                                    for (int m = 0; m < Groups[domena].Count; m++) // renumbering the selected group
                                    {
                                        ii = Groups[domena][m][0];
                                        jj = Groups[domena][m][1];
                                        kk = Groups[domena][m][2];

                                        Numbering[ii][jj][kk] = dn_new;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #endregion

            #region Finalizing output

            Domain_sizes = new int[N_domains + 1];

            max = 0;
            for (int i = 0; i < D_list.Length; i++)
            {
                Domain_sizes[D_list[i]] += Groups[i].Count; ;

                if (max < Domain_sizes[D_list[i]])
                {
                    max = Domain_sizes[D_list[i]];
                }
            }

            result = new int[max];

            for (int i = 0; i < D_list.Length; i++)
            {
                if (Domain_sizes[i] > 0)
                {
                    result[Domain_sizes[i] - 1] += 1;
                }
            }

            #endregion

            return result;
        }

        static int[][] Find_surface_defects_basic(int file_n)
        {
            int[][] result = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                result[i] = new int[4];
            }

            double move_x, move_y;

            move_x = Math.Cos((double)file_n * 2.0 * Math.PI / 180.0);
            move_y = Math.Sin((double)file_n * 2.0 * Math.PI / 180.0);

            result[0][0] = 50 - (int)(25.0 * move_x);
            result[0][1] = 50 - (int)(25.0 * move_y);
            result[0][2] = 0;
            result[0][3] = 1;

            result[1][0] = 50 + (int)(25.0 * move_x);
            result[1][1] = 50 + (int)(25.0 * move_y);
            result[1][2] = 0;
            result[1][3] = 0;

            result[2][0] = 25;
            result[2][1] = 50;
            result[2][2] = 100;
            result[2][3] = 1;

            result[3][0] = 75;
            result[3][1] = 50;
            result[3][2] = 100;
            result[3][3] = 0;

            return result;
        }

        static int[][] Find_surface_defects(string[] file)
        {
            #region Initialization

            int[][] result = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                result[i] = new int[4];
            }

            bool skip = false;
            string[] data;
            string[] separators = { "\t", " " };
            int x_i, y_j, z_k, Ni, Nj, Nk, defect_n, skip_check1, skip_check2, skip_check3;
            double nx, ny, nz;
            double[][][][] set;

            Ni = 0;
            Nj = 0;
            Nk = 0;
            for (int i = 0; i < file.Length; i++)
            {
                data = file[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                x_i = int.Parse(data[0]);
                y_j = int.Parse(data[1]);
                z_k = int.Parse(data[2]);

                if (x_i > Ni)
                {
                    Ni = x_i;
                }
                if (y_j > Nj)
                {
                    Nj = y_j;
                }
                if (z_k > Nk)
                {
                    Nk = z_k;
                }
            }
            Ni++;
            Nj++;
            Nk++;
            set = new double[Ni][][][];
            for (int i = 0; i < Ni; i++)
            {
                set[i] = new double[Nj][][];
                for (int j = 0; j < Nj; j++)
                {
                    set[i][j] = new double[Nk][];
                    for (int k = 0; k < Nk; k++)
                    {
                        set[i][j][k] = new double[3];
                    }
                }
            }

            for (int i = 0; i < file.Length; i++)
            {
                data = file[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                x_i = int.Parse(data[0]);
                y_j = int.Parse(data[1]);
                z_k = int.Parse(data[2]);

                nx = double.Parse(data[3]);
                ny = double.Parse(data[4]);
                nz = double.Parse(data[5]);

                set[x_i][y_j][z_k][0] = nx;
                set[x_i][y_j][z_k][1] = ny;
                set[x_i][y_j][z_k][2] = nz;
            }

            #endregion

            #region Setting up the comparison

            double[] encircling = new double[8];
            double[] subject = new double[8];
            double[] dfi_e = new double[8];
            double[] dfi_s = new double[8];
            double int_fi_e, int_fi_s, factor;

            encircling[0] = Math.Atan2(0, 1);
            encircling[1] = Math.Atan2(1, 1);
            encircling[2] = Math.Atan2(1, 0);
            encircling[3] = Math.Atan2(1, -1);
            encircling[4] = Math.Atan2(0, 1);
            encircling[5] = Math.Atan2(-1, -1);
            encircling[6] = Math.Atan2(-1, 0);
            encircling[7] = Math.Atan2(-1, 1);

            for (int c = 0; c < 7; c++)
            {
                dfi_e[c] = encircling[c + 1] - encircling[c];

                if (dfi_e[c] > Math.PI / 2.0) { dfi_e[c] -= Math.PI; }
                if (dfi_e[c] < -Math.PI / 2.0) { dfi_e[c] += Math.PI; }
            }
            dfi_e[7] = encircling[0] - encircling[7];

            if (dfi_e[7] > Math.PI / 2.0) { dfi_e[7] -= Math.PI; }
            if (dfi_e[7] < -Math.PI / 2.0) { dfi_e[7] += Math.PI; }

            int_fi_e = 0.0;
            for (int c = 0; c < 8; c++)
            {
                int_fi_e += dfi_e[c];
            }

            #endregion

            defect_n = 0;
            for (int i = 1; i < Ni - 1; i++)
            {
                for (int j = 1; j < Nj - 1; j++)
                {
                    #region Bottom surface

                    subject[0] = Math.Atan2(set[i + 1][j][0][1], set[i + 1][j][0][0]);
                    subject[1] = Math.Atan2(set[i + 1][j + 1][0][1], set[i + 1][j + 1][0][0]) - subject[0];
                    subject[2] = Math.Atan2(set[i][j + 1][0][1], set[i][j + 1][0][0]) - subject[0];
                    subject[3] = Math.Atan2(set[i - 1][j + 1][0][1], set[i - 1][j + 1][0][0]) - subject[0];
                    subject[4] = Math.Atan2(set[i - 1][j][0][1], set[i - 1][j][0][0]) - subject[0];
                    subject[5] = Math.Atan2(set[i - 1][j - 1][0][1], set[i - 1][j - 1][0][0]) - subject[0];
                    subject[6] = Math.Atan2(set[i][j - 1][0][1], set[i][j - 1][0][0]) - subject[0];
                    subject[7] = Math.Atan2(set[i + 1][j - 1][0][1], set[i + 1][j - 1][0][0]) - subject[0];
                    subject[0] = 0.0;

                    for (int c = 0; c < 7; c++)
                    {
                        dfi_s[c] = subject[c + 1] - subject[c];

                        if (dfi_s[c] > Math.PI / 2.0) { dfi_s[c] -= Math.PI; }
                        if (dfi_s[c] < -Math.PI / 2.0) { dfi_s[c] += Math.PI; }
                    }
                    dfi_s[7] = subject[0] - subject[7];

                    if (dfi_s[7] > Math.PI / 2.0) { dfi_s[7] -= Math.PI; }
                    if (dfi_s[7] < -Math.PI / 2.0) { dfi_s[7] += Math.PI; }

                    int_fi_s = 0.0;
                    for (int c = 0; c < 8; c++) { int_fi_s += dfi_s[c]; }

                    factor = int_fi_s / int_fi_e;
                    if (Math.Abs(factor) > 0.4)
                    {
                        for (int s = 0; s < result.Length; s++)
                        {
                            skip_check1 = Math.Abs(i - result[s][0]);
                            skip_check2 = Math.Abs(j - result[s][1]);
                            skip_check3 = Math.Abs(result[s][2]);

                            if (skip_check1 <=2 && skip_check2 <= 2 && skip_check3 <= 2)
                            {
                                skip = true;
                                break;
                            }
                            else { skip = false; }
                        }
                        if (!skip)
                        {
                            factor = Math.Round(2.0 * factor);

                            result[defect_n][0] = i;
                            result[defect_n][1] = j;
                            result[defect_n][2] = 0;
                            if (factor > 0.0) { result[defect_n][3] = 0; }
                            if (factor < 0.0) { result[defect_n][3] = 1; }

                            defect_n++;
                        }
                    }

                    #endregion

                    #region Top surface

                    subject[0] = Math.Atan2(set[i + 1][j][Nk - 1][1], set[i + 1][j][Nk - 1][0]);
                    subject[1] = Math.Atan2(set[i + 1][j + 1][Nk - 1][1], set[i + 1][j + 1][Nk - 1][0]) - subject[0];
                    subject[2] = Math.Atan2(set[i][j + 1][Nk - 1][1], set[i][j + 1][Nk - 1][0]) - subject[0];
                    subject[3] = Math.Atan2(set[i - 1][j + 1][Nk - 1][1], set[i - 1][j + 1][Nk - 1][0]) - subject[0];
                    subject[4] = Math.Atan2(set[i - 1][j][Nk - 1][1], set[i - 1][j][Nk - 1][0]) - subject[0];
                    subject[5] = Math.Atan2(set[i - 1][j - 1][Nk - 1][1], set[i - 1][j - 1][Nk - 1][0]) - subject[0];
                    subject[6] = Math.Atan2(set[i][j - 1][Nk - 1][1], set[i][j - 1][Nk - 1][0]) - subject[0];
                    subject[7] = Math.Atan2(set[i + 1][j - 1][Nk - 1][1], set[i + 1][j - 1][Nk - 1][0]) - subject[0];
                    subject[0] = 0.0;

                    for (int c = 0; c < 7; c++)
                    {
                        dfi_s[c] = subject[c + 1] - subject[c];

                        if (dfi_s[c] > Math.PI / 2.0) { dfi_s[c] -= Math.PI; }
                        if (dfi_s[c] < -Math.PI / 2.0) { dfi_s[c] += Math.PI; }
                    }
                    dfi_s[7] = subject[0] - subject[7];

                    if (dfi_s[7] > Math.PI / 2.0) { dfi_s[7] -= Math.PI; }
                    if (dfi_s[7] < -Math.PI / 2.0) { dfi_s[7] += Math.PI; }

                    int_fi_s = 0.0;
                    for (int c = 0; c < 8; c++) { int_fi_s += dfi_s[c]; }

                    factor = int_fi_s / int_fi_e;
                    if (Math.Abs(factor) > 0.4)
                    {
                        for (int s = 0; s < result.Length; s++)
                        {
                            skip_check1 = Math.Abs(i - result[s][0]);
                            skip_check2 = Math.Abs(j - result[s][1]);
                            skip_check3 = Math.Abs(99 - result[s][2]);

                            if (skip_check1 <= 2 && skip_check2 <= 2 && skip_check3 <= 2)
                            {
                                skip = true;
                                break;
                            }
                            else { skip = false; }
                        }
                        if (!skip)
                        {
                            factor = Math.Round(2.0 * factor);

                            result[defect_n][0] = i;
                            result[defect_n][1] = j;
                            result[defect_n][2] = Nk;
                            if (factor > 0.0) { result[defect_n][3] = 0; }
                            if (factor < 0.0) { result[defect_n][3] = 1; }

                            defect_n++;
                        }
                    }

                    #endregion
                }
            }

            return result;
        }

        static double POVrayblob_color(int x_p, int y_p, int z_p, int[][] d_p)
        {
            #region Initialization

            int n_d1;
            double d_min1, d_c, d_d, result;

            d_min1 = 200.0;

            n_d1 = 0;

            #endregion

            #region Calculation

            for (int i = 0; i < d_p.Length; i++)
            {
                d_c = (x_p - d_p[i][0])*(x_p - d_p[i][0]) + (y_p - d_p[i][1])*(y_p - d_p[i][1]) + (z_p - d_p[i][2])*(z_p - d_p[i][2]);
                d_c = Math.Sqrt(d_c);

                if (d_c < d_min1)
                {
                    d_min1 = d_c;
                    n_d1 = i;
                }
            }
            result = d_p[n_d1][3];
            
            return result;

            #endregion
        }

        static double[][][] POVrayblob_color(string[] file, int[][] s_points)
        {
            #region Initialization

            double[][][] result;
            string[] data;
            string[] separators = { "\t", " " };
            int x_i, y_j, z_k, Ni, Nj, Nk;
            int[][][] set;

            Ni = 100;
            Nj = 100;
            Nk = 100;
            for (int i = 0; i < file.Length; i++)
            {
                data = file[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                x_i = int.Parse(data[0]);
                y_j = int.Parse(data[1]);
                z_k = int.Parse(data[2]);

                if (x_i > Ni)
                {
                    Ni = x_i;
                }
                if (y_j > Nj)
                {
                    Nj = y_j;
                }
                if (z_k > Nk)
                {
                    Nk = z_k;
                }
            }
            Ni++;
            Nj++;
            Nk++;
            set = new int[Ni][][];
            result = new double[Ni][][];
            for (int i = 0; i < Ni; i++)
            {
                set[i] = new int[Nj][];
                result[i] = new double[Nj][];
                for (int j = 0; j < Nj; j++)
                {
                    set[i][j] = new int[Nk];
                    result[i][j] = new double[Nk];
                }
            }

            for (int i = 0; i < file.Length; i++)
            {
                data = file[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                x_i = int.Parse(data[0]);
                y_j = int.Parse(data[1]);
                z_k = int.Parse(data[2]);

                set[x_i][y_j][z_k] = 1;
            }

            #endregion

            #region Rows

            int count_n = 2;

            for (int i = 0; i < Ni; i++)
            {
                for (int j = 0; j < Nj; j++)
                {
                    for (int k = 0; k < Nk; k++)
                    {
                        if (set[i][j][k] == 1)
                        {
                            if (k > 0 && set[i][j][k - 1] > 1)
                            {
                                set[i][j][k] = set[i][j][k - 1];
                            }
                            else if (k > 1 && set[i][j][k - 2] > 1)
                            {
                                set[i][j][k] = set[i][j][k - 2];
                            }
                            else
                            {
                                set[i][j][k] = count_n;
                                count_n++;
                            }
                        }
                    }
                }
            }

            #endregion

            #region Planes

            int n1, n2, n3;

            for (int i = 0; i < Ni; i++)
            {
                for (int k = 0; k < Nk; k++)
                {
                    for (int j = 1; j < Nj; j++)
                    {
                        if (set[i][j][k] > 1)
                        {
                            n1 = set[i][j][k];
                            n2 = n1;

                            if (set[i][j - 1][k] > 1) { n2 = set[i][j - 1][k]; }
                            if (k > 1 && set[i][j - 1][k - 1] > 1 && set[i][j - 1][k - 1] < n2) { n2 = set[i][j - 1][k - 1]; }
                            if (k < Nk - 1 && set[i][j - 1][k + 1] > 1 && set[i][j - 1][k + 1] < n2) { n2 = set[i][j - 1][k + 1]; }

                            if (n2 == n1) { continue; }

                            if (n2 > n1)
                            {
                                for (int m = 0; m < Ni; m++)
                                {
                                    for (int n = 0; n < Nj; n++)
                                    {
                                        for (int o = 0; o < Nk; o++)
                                        {
                                            if (set[m][n][o] == n2) { set[m][n][o] = n1; }
                                        }
                                    }
                                }
                            }
                            else if (n2 < n1)
                            {
                                for (int m = 0; m < Ni; m++)
                                {
                                    for (int n = 0; n < Nj; n++)
                                    {
                                        for (int o = 0; o < Nk; o++)
                                        {
                                            if (set[m][n][o] == n1) { set[m][n][o] = n2; }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Whole cell

            for (int j = 0; j < Nj; j++)
            {
                for (int k = 0; k < Nk; k++)
                {
                    for (int i = 1; i < Ni; i++)
                    {
                        if (set[i][j][k] > 1)
                        {
                            n1 = set[i][j][k];
                            n2 = n1;

                            #region Face diagonals

                            if (set[i - 1][j][k] > 1 && set[i - 1][j][k] != n1) { n2 = set[i - 1][j][k]; }
                            if (j > 1 && set[i - 1][j - 1][k] > 1 && set[i - 1][j - 1][k] < n2) { n2 = set[i - 1][j - 1][k]; }
                            if (j < Nj - 1 && set[i - 1][j + 1][k] > 1 && set[i - 1][j + 1][k] < n2) { n2 = set[i - 1][j + 1][k]; }
                            if (k > 1 && set[i - 1][j][k - 1] > 1 && set[i - 1][j][k - 1] < n2) { n2 = set[i - 1][j][k - 1]; }
                            if (k < Nk - 1 && set[i - 1][j][k + 1] > 1 && set[i - 1][j][k + 1] < n2) { n2 = set[i - 1][j][k + 1]; }

                            #endregion

                            #region Body diagonals

                            if (j > 1 && k > 1 && set[i - 1][j - 1][k - 1] > 1 && set[i - 1][j - 1][k - 1] < n2) { n2 = set[i - 1][j - 1][k - 1]; }
                            if (j > 1 && k < Nk - 1 && set[i - 1][j - 1][k + 1] > 1 && set[i - 1][j - 1][k + 1] < n2) { n2 = set[i - 1][j - 1][k + 1]; }
                            if (j < Nj - 1 && k > 1 && set[i - 1][j + 1][k - 1] > 1 && set[i - 1][j + 1][k - 1] < n2) { n2 = set[i - 1][j + 1][k - 1]; }
                            if (j < Nj - 1 && k < Nk - 1 && set[i - 1][j + 1][k + 1] > 1 && set[i - 1][j + 1][k + 1] < n2) { n2 = set[i - 1][j + 1][k + 1]; }

                            #endregion

                            if (n2 == n1) { continue; }

                            if (n2 > n1)
                            {
                                for (int m = 0; m < Ni; m++)
                                {
                                    for (int n = 0; n < Nj; n++)
                                    {
                                        for (int o = 0; o < Nk; o++)
                                        {
                                            if (set[m][n][o] == n2) { set[m][n][o] = n1; }
                                        }
                                    }
                                }
                            }
                            else if (n2 < n1)
                            {
                                for (int m = 0; m < Ni; m++)
                                {
                                    for (int n = 0; n < Nj; n++)
                                    {
                                        for (int o = 0; o < Nk; o++)
                                        {
                                            if (set[m][n][o] == n1) { set[m][n][o] = n2; }
                                        }
                                    }
                                }
                            }
                        }
                        if (i > 1 && set[i][j][k] > 1 && set[i - 2][j][k] > 1)
                        {
                            n1 = set[i - 2][j][k];
                            n2 = set[i][j][k];

                            if (n2 > n1)
                            {
                                for (int m = 0; m < Ni; m++)
                                {
                                    for (int n = 0; n < Nj; n++)
                                    {
                                        for (int o = 0; o < Nk; o++)
                                        {
                                            if (set[m][n][o] == n2) { set[m][n][o] = n1; }
                                        }
                                    }
                                }
                            }
                            else if (n2 < n1)
                            {
                                for (int m = 0; m < Ni; m++)
                                {
                                    for (int n = 0; n < Nj; n++)
                                    {
                                        for (int o = 0; o < Nk; o++)
                                        {
                                            if (set[m][n][o] == n1) { set[m][n][o] = n2; }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Reducing numbers

            bool contains = false;
            List<int> numbers = new List<int>();

            for (int i = 0; i < Ni; i++)
            {
                for (int j = 0; j < Nj; j++)
                {
                    for (int k = 0; k < Nk; k++)
                    {
                        if (set[i][j][k] > 0)
                        {
                            for (int l = 0; l < numbers.Count; l++)
                            {
                                if (set[i][j][k] == numbers[l])
                                {
                                    contains = true;
                                    set[i][j][k] = l + 1;
                                    break;
                                }
                            }

                            if (!contains)
                            {
                                numbers.Add(set[i][j][k]);
                                set[i][j][k] = numbers.Count;
                            }
                            else { contains = false; }
                        }
                    }
                }
            }

            #endregion

            #region Deciding colour

            #region Arranging points

            List<int[]>[] clusters = new List<int[]>[numbers.Count];
            for (int i = 0; i < numbers.Count; i++)
            {
                clusters[i] = new List<int[]>();
            }

            for (int i = 0; i < Ni; i++)
            {
                for (int j = 0; j < Nj; j++)
                {
                    for (int k = 0; k < Nk; k++)
                    {
                        if (set[i][j][k] > 0)
                        {
                            clusters[set[i][j][k] - 1].Add(new int[] { i, j, k });
                        }
                    }
                }
            }

            #endregion

            #region Determining nearest surface defect

            double sd_d, sdmin;
            bool[][] colouring = new bool[clusters.Length][];
            for (int i = 0; i < clusters.Length; i++)
            {
                colouring[i] = new bool[s_points.Length];
            }

            for (int i = 0; i < clusters.Length; i++)
            {
                for (int j = 0; j < s_points.Length; j++)
                {
                    sdmin = 50.0;
                    for (int k = 0; k < clusters[i].Count; k++)
                    {
                        sd_d = (clusters[i][k][0] - s_points[j][0]) * (clusters[i][k][0] - s_points[j][0]);
                        sd_d += (clusters[i][k][1] - s_points[j][1]) * (clusters[i][k][1] - s_points[j][1]);
                        sd_d += (clusters[i][k][2] - s_points[j][2]) * (clusters[i][k][2] - s_points[j][2]);
                        sd_d = Math.Sqrt(sd_d);

                        if (sd_d < sdmin) { sdmin = sd_d; }
                    }
                    if (sdmin < 8.0) { colouring[i][j] = true; }
                    else { colouring[i][j] = false; }
                }
            }

            #endregion

            bool different_colour = false;
            double colour = 0.0;
            int closest_defect = 0;

            for (int i = 0; i < clusters.Length; i++)
            {
                #region Determining if the cluster is one or more colours

                for (int j = 0; j < s_points.Length; j++)
                {
                    for (int k = 0; k < j; k++)
                    {
                        if (colouring[i][j] && colouring[i][k])
                        {
                            if (s_points[j][3] == s_points[k][3]) { different_colour = false; colour = s_points[j][3]; }
                            else { different_colour = true; break; }
                        }
                    }
                    if (different_colour) { break; }
                }

                #endregion

                #region Assigning colour

                if (!different_colour)
                {
                    for (int j = 0; j < clusters[i].Count; j++)
                    {
                        result[clusters[i][j][0]][clusters[i][j][1]][clusters[i][j][2]] = colour;
                    }
                }

                else
                {
                    for (int j = 0; j < clusters[i].Count; j++)
                    {
                        sdmin = 50.0;
                        for (int k = 0; k < s_points.Length; k++)
                        {
                            if (colouring[i][k])
                            {
                                sd_d = (clusters[i][j][0] - s_points[k][0]) * (clusters[i][j][0] - s_points[k][0]);
                                sd_d += (clusters[i][j][1] - s_points[k][1]) * (clusters[i][j][1] - s_points[k][1]);
                                sd_d += (clusters[i][j][2] - s_points[k][2]) * (clusters[i][j][2] - s_points[k][2]);
                                sd_d = Math.Sqrt(sd_d);

                                if (sd_d < sdmin) { sdmin = sd_d; closest_defect = k; }
                            }
                        }
                        if (sdmin < 48.0) { colour = s_points[closest_defect][3]; }
                        else { colour = 0.5; }
                        result[clusters[i][j][0]][clusters[i][j][1]][clusters[i][j][2]] = colour;
                    }
                }

                #endregion

                different_colour = false;
            }

            #endregion

            return result;
        }

        #region Fundamental values

        /// <summary>
        /// Izračuna trace od Q*Q
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        /// <returns></returns>
        static double Half_trQ_square(double q1, double q2, double q3, double q4, double q5)
        {
            double result = 3.0 * q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4 + q5 * q5;
            return result;
        }

        /// <summary>
        /// Izračuna trace od Q*Q*Q
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        /// <returns></returns>
        static double TrQ_cubed(double q1, double q2, double q3, double q4, double q5)
        {
            double result = -3.0 * (2.0*q1*q1*q1 - 2.0*q3*q4*q5 + q2 * (q5*q5 - q4*q4) + q1 * (-2.0*q2*q2 - 2.0*q3*q3 + q4*q4 + q5*q5));
            return result;
        }

        /// <summary>
        /// Izračuna beta kvadrat za Q
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        /// <returns></returns>
        static double Beta(double q1, double q2, double q3, double q4, double q5)
        {
            double trQ2, trQ3, result;

            trQ2 = 2.0 * Half_trQ_square(q1, q2, q3, q4, q5);
            trQ3 = TrQ_cubed(q1, q2, q3, q4, q5);

            if (trQ2 > 0.0)
            {
                result = 1 - (6.0 * trQ3 * trQ3) / (trQ2 * trQ2 * trQ2);
            }
            else
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Odvajanje po izbrani spremenljivki
        /// </summary>
        /// <param name="x_minus_1">Vrednost v legi i - 1</param>
        /// <param name="x_plus_1">Vrednost v legi i + 1</param>
        /// <param name="dx">Dolžina koraka</param>
        /// <returns>Prvi odvod</returns>
        static double Odvod(double x_minus_1, double x_plus_1, double dx)
        {
            double result = (x_plus_1 - x_minus_1) / (2 * dx);
            return result;
        }

        /// <summary>
        /// Drugo odvajanje po izbrani spremenljivki
        /// </summary>
        /// <param name="x_minus_1">Vrednost v legi i - 1</param>
        /// <param name="x">Vrednost v legi i</param>
        /// <param name="x_plus_1">Vrednost v legi i + 1</param>
        /// <param name="dx">Dolžina koraka</param>
        /// <returns>Drugi odvod</returns>
        static double Drugi_odvod(double x_minus_1, double x, double x_plus_1, double dx)
        {
            double result = (x_plus_1 - 2.0 * x + x_minus_1) / (dx * dx);
            return result;
        }

        #endregion

        #endregion


        #region Display and output

        #region Controls

        private void Mode_selection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Calculation_selection.SelectedIndex == 0)
            {
                Start.Text = "Start calculation";
            }

            if (Calculation_selection.SelectedIndex == 1)
            {
                Start.Text = "Continue calculation";
            }

            if (Calculation_selection.SelectedIndex == 2)
            {
                Start.Text = "Determine director field";
            }

            if (Calculation_selection.SelectedIndex == 3)
            {
                Start.Text = "Create interferometry image";
            }

            if (Calculation_selection.SelectedIndex == 4)
            {
                Start.Text = "Select mode";
                Start.Enabled = false;
            }

            if (Calculation_selection.SelectedIndex == 5)
            {
                Start.Text = "Other";
            }
        }

        private void POVray_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Start.Enabled = true;
            Start.Text = "Create POV-Ray file";
        }

        private void Inserts_n2_CheckedChanged(object sender, EventArgs e)
        {
            if (Inserts_n2.Checked)
            {
                New_insert.Visible = true;
            }
            else
            {
                New_insert.Visible = false;
            }
        }

        private void Switch_mode_Click(object sender, EventArgs e)
        {
            draw_size = pictureBox1.Height;
            if (draw_mode == 0)
            {
                draw_mode = 1;
                textBox1.Text = "Set to: Display S";
            }
            else if (draw_mode == 1)
            {
                draw_mode = 0;
                textBox1.Text = "Set to: Display n";
            }
        }

        private void TrackBar_depth_Scroll(object sender, EventArgs e)
        {
            draw_size = pictureBox1.Height;

            #region Risanje bete

            if (draw_mode == 0)
            {
                int rgb, size;
                Color color = Color.FromArgb(0, 0, 0);
                Graphics formGraphics = pictureBox1.CreateGraphics();

                #region Velikost kvadratkov

                if (Nx <= 100)
                {
                    size = 8;
                }
                else if (Nx <= 200)
                {
                    size = 4;
                }
                else if (Nx <= 400)
                {
                    size = 2;
                }
                else
                {
                    size = 1;
                }

                #endregion

                #region Risanje yz ravnine

                if (x_const.Checked)
                {
                    trackBar_depth.Maximum = Nx - 1;

                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            rgb = (int)(b2[trackBar_depth.Value][j][k] * 255);
                            color = Color.FromArgb(rgb, rgb, rgb);
                            formGraphics.FillRectangle(new SolidBrush(color), size * (j), draw_size - (size * (k + 1) + 1), size, size);
                        }
                    }
                }

                #endregion

                #region Risanje xz ravnine

                if (y_const.Checked)
                {
                    trackBar_depth.Maximum = Ny - 1;

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            rgb = (int)(b2[i][trackBar_depth.Value][k] * 255);
                            color = Color.FromArgb(rgb, rgb, rgb);
                            formGraphics.FillRectangle(new SolidBrush(color), size * (i), draw_size - (size * (k + 1) + 1), size, size);
                        }
                    }
                }

                #endregion

                #region Risanje xy ravnine

                if (z_const.Checked)
                {
                    trackBar_depth.Maximum = Nz - 1;

                    for (int i = 0; i < Nx; i++)
                    {
                        for (int j = 0; j < Ny; j++)
                        {
                            rgb = (int)(b2[i][j][trackBar_depth.Value] * 255);
                            color = Color.FromArgb(rgb, rgb, rgb);
                            formGraphics.FillRectangle(new SolidBrush(color), size * (i), draw_size - (size * (j + 1) + 1), size, size);
                        }
                    }
                }

                #endregion
            }

            #endregion

            #region Risanje direktorskega polja

            else // (draw_mode == 1)
            {
                int n_x, n_y, n_z;

                Color color = Color.FromArgb(0, 0, 0);
                Pen pen = new Pen(color);
                Graphics formGraphics = pictureBox1.CreateGraphics();
                pictureBox1.Refresh();

                #region Risanje xy ravnine

                if (z_const.Checked)
                {
                    trackBar_depth.Maximum = Nz - 1;

                    for (int i = 0; i < Nx; i++)
                    {
                        if (i % 4 == 2)
                        {
                            for (int j = 0; j < Ny; j++)
                            {
                                if (j % 4 == 2)
                                {
                                    n_x = (int)(16.0 * direktor[i][j][trackBar_depth.Value][0]);
                                    n_y = (int)(16.0 * direktor[i][j][trackBar_depth.Value][1]);

                                    formGraphics.DrawLine(pen, 4 * i - n_x / 2, (4 * (j + 1) + 1) - n_y / 2, 4 * i + n_x / 2, (4 * (j + 1) + 1) + n_y / 2);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Risanje xz ravnine

                if (y_const.Checked)
                {
                    trackBar_depth.Maximum = Ny - 1;

                    for (int i = 0; i < Nx; i++)
                    {
                        if (i % 4 == 0)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                if (k % 4 == 0)
                                {
                                    n_x = (int)(16.0 * direktor[i][trackBar_depth.Value][k][0]);
                                    n_z = (int)(16.0 * direktor[i][trackBar_depth.Value][k][2]);

                                    formGraphics.DrawLine(pen, 4 * i - n_x / 2, (4 * (k + 1) + 1) - n_z / 2, 4 * i + n_x / 2, (4 * (k + 1) + 1) + n_z / 2);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Risanje yz ravnine

                if (x_const.Checked)
                {
                    trackBar_depth.Maximum = Nx - 1;

                    for (int j = 0; j < Ny; j++)
                    {
                        if (j % 4 == 0)
                        {
                            for (int k = 0; k < Nz; k++)
                            {
                                if (k % 4 == 0)
                                {
                                    n_y = (int)(16.0 * direktor[trackBar_depth.Value][j][k][1]);
                                    n_z = (int)(16.0 * direktor[trackBar_depth.Value][j][k][2]);

                                    formGraphics.DrawLine(pen, 4 * j - n_y / 2, (4 * (k + 1) + 1) - n_z / 2, 4 * j + n_y / 2, (4 * (k + 1) + 1) + n_z / 2);
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            #endregion
        }

        private void Use_different_boundary_CheckedChanged(object sender, EventArgs e)
        {
            if (Use_different_boundary.Checked)
            {
                Top_boundary_box_2.Visible = true;
                Bottom_boundary_box_2.Visible = true;
            }

            else
            {
                Top_boundary_box_2.Visible = false;
                Bottom_boundary_box_2.Visible = false;
            }
        }

        private void Inserts_n_CheckedChanged(object sender, EventArgs e)
        {
            if (Inserts_n.Checked)
            {
                Create_insert.Visible = true;

                x_insert_n.Maximum = Nx_n.Value;
                y_insert_n.Maximum = Ny_n.Value;
                z_insert_n.Maximum = Nz_n.Value;

                x_insert_n.Value = Nx_n.Value / 2;
                y_insert_n.Value = Ny_n.Value / 2;
                z_insert_n.Value = Nz_n.Value / 2;
            }

            else
            {
                Create_insert.Visible = false;
            }
        }

        private void Insert_shape_SelectedIndexChanged(object sender, EventArgs e)
        {
            Add_insert.Enabled = true;

            switch (Insert_shape.SelectedIndex)
            {
                case 0: //Point
                    Dimensions_l.Visible = false;

                    insert_size_1_n.Value = 0;
                    insert_size_1_n.Visible = false;
                    insert_size_1_l.Visible = false;

                    insert_size_2_n.Value = 0;
                    insert_size_2_n.Visible = false;
                    insert_size_2_l.Visible = false;

                    insert_size_3_n.Value = 0;
                    insert_size_3_n.Visible = false;
                    insert_size_3_l.Visible = false;

                    break;

                case 1: //Cube
                    insert_size_1_l.Text = "x";
                    insert_size_2_l.Text = "y";
                    insert_size_3_l.Text = "z";

                    insert_size_3_n.Visible = true;
                    insert_size_3_l.Visible = true;

                    goto default;

                case 2: //Ellipsoid
                    insert_size_1_l.Text = "r_x";
                    insert_size_2_l.Text = "r_y";
                    insert_size_3_l.Text = "r_z";

                    insert_size_3_n.Visible = true;
                    insert_size_3_l.Visible = true;

                    goto default;

                case 3: //Torus
                    insert_size_1_n.Value = 50;
                    insert_size_1_l.Text = "r_1";
                    insert_size_2_l.Text = "r_2";

                    insert_size_3_n.Visible = false;
                    insert_size_3_l.Visible = false;

                    goto default;

                default:
                    Dimensions_l.Visible = true;

                    insert_size_1_n.Visible = true;
                    insert_size_1_l.Visible = true;

                    insert_size_2_n.Visible = true;
                    insert_size_2_l.Visible = true;
                    break;
            }
        }

        #endregion

        #region Starting condition dropdown menu controls

        private void Top_boundary_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Top_boundary_box.SelectedIndex == 0)
            {
                Top_N_label.Visible = true;
                Top_N_numeric.Visible = true;
                Top_phi_label.Visible = true;
                Top_phi_numeric.Visible = true;
            }

            else if (Top_boundary_box.SelectedIndex == 1)
            {
                Top_N_label.Visible = false;
                Top_N_numeric.Visible = false;
                Top_phi_label.Visible = true;
                Top_phi_numeric.Visible = true;
            }

            else
            {
                Top_N_label.Visible = false;
                Top_N_numeric.Visible = false;
                Top_phi_label.Visible = false;
                Top_phi_numeric.Visible = false;
            }
        }

        private void Bottom_boundary_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Bottom_boundary_box.SelectedIndex == 0)
            {
                Bottom_N_label.Visible = true;
                Bottom_N_numeric.Visible = true;
                Bottom_phi_label.Visible = true;
                Bottom_phi_numeric.Visible = true;
            }

            else if (Bottom_boundary_box.SelectedIndex == 1)
            {
                Bottom_N_label.Visible = false;
                Bottom_N_numeric.Visible = false;
                Bottom_phi_label.Visible = true;
                Bottom_phi_numeric.Visible = true;
            }

            else
            {
                Bottom_N_label.Visible = false;
                Bottom_N_numeric.Visible = false;
                Bottom_phi_label.Visible = false;
                Bottom_phi_numeric.Visible = false;
            }
        }

        private void Bulk_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Bulk_box.SelectedIndex == 1)
            {
                Bulk_phi_label.Visible = true;
                Bulk_phi_numeric.Visible = true;
            }

            else if (Bulk_box.SelectedIndex == 3)
            {
                Bulk_phi_label.Visible = true;
                Bulk_phi_numeric.Visible = true;
            }

            else
            {
                Bulk_phi_label.Visible = false;
                Bulk_phi_numeric.Visible = false;
            }
        }

        private void Top_boundary_box_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Top_boundary_box_2.SelectedIndex == 0)
            {
                Top_N_label_2.Visible = true;
                Top_N_numeric_2.Visible = true;
                Top_phi_label_2.Visible = true;
                Top_phi_numeric_2.Visible = true;
            }

            else if (Top_boundary_box_2.SelectedIndex == 1)
            {
                Top_N_label_2.Visible = false;
                Top_N_numeric_2.Visible = false;
                Top_phi_label_2.Visible = true;
                Top_phi_numeric_2.Visible = true;
            }

            else
            {
                Top_N_label_2.Visible = false;
                Top_N_numeric_2.Visible = false;
                Top_phi_label_2.Visible = false;
                Top_phi_numeric_2.Visible = false;
            }
        }

        private void Bottom_boundary_box_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Bottom_boundary_box_2.SelectedIndex == 0)
            {
                Bottom_N_label_2.Visible = true;
                Bottom_N_numeric_2.Visible = true;
                Bottom_phi_label_2.Visible = true;
                Bottom_phi_numeric_2.Visible = true;
            }

            else if (Bottom_boundary_box_2.SelectedIndex == 1)
            {
                Bottom_N_label_2.Visible = false;
                Bottom_N_numeric_2.Visible = false;
                Bottom_phi_label_2.Visible = true;
                Bottom_phi_numeric_2.Visible = true;
            }

            else
            {
                Bottom_N_label_2.Visible = false;
                Bottom_N_numeric_2.Visible = false;
                Bottom_phi_label_2.Visible = false;
                Bottom_phi_numeric_2.Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// Nariše eno ravnino na okno
        /// </summary>
        /// <param name="color"></param>
        /// <param name="formGraphics"></param>
        static void Narisi(Color color, Graphics formGraphics)
        {
            int rgb, size;

            #region Velikost kvadratkov

            if (Nx <= 100)
            {
                size = 8;
            }
            else if (Nx <= 200)
            {
                size = 4;
            }
            else if (Nx <= 401)
            {
                size = 2;
            }
            else
            {
                size = 1;
            }

            #endregion

            #region Izpis celotnega tenzorja

            using (StreamWriter writer = new StreamWriter("rezultat.txt", false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            b2[i][j][k] = Beta(Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k]);

                            rgb = (int)(b2[i][j][k] * 255);
                            color = Color.FromArgb(rgb, rgb, rgb);

                            if (k == 10)
                            {
                                formGraphics.FillRectangle(new SolidBrush(color), size * (j), draw_size - (size * (k + 1) + 1), size, size);
                            }

                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}  {6,8:F4}  {7,8:F4}  {8,6:F4}", i, j, k, Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k], b2[i][j][k]);
                        }
                    }
                }
            }

            #endregion

            #region Izpis biaksialnosti

            using (StreamWriter writer = new StreamWriter("beta.txt", false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (b2[i][j][k] > 0.8)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Izpiše vmesna stanja biaksialnosti
        /// </summary>
        /// <param name="it"></param>
        static void Vmesni_izpis(int it)
        {
            string dir;

            #region Izpis biaksialnosti

            dir = "betas";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #region Brisanje starih datotek

            if (it == 0)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "beta" + it.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            b2[i][j][k] = Beta(Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k]);

                            if (b2[i][j][k] > 0.8)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Skalarni ureditveni parameter

            dir = "S";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #region Brisanje starih datotek

            if (it == 0)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_points" + it.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (S[i][j][k] < 0.5)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Izpis celotnega tenzorja

            dir = "results";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #region Brisanje starih datotek

            if (it == 0)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "rezultat" + it.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            b2[i][j][k] = Beta(Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k]);

                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}  {6,8:F4}  {7,8:F4}  {8,6:F4}", i, j, k, Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k], b2[i][j][k]);
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Izpiše vmesna stanja biaksialnosti (extra directory)
        /// </summary>
        /// <param name="it"></param>
        static void Vmesni_izpis_dir(string dir0, int it)
        {
            string dir;

            #region Izpis biaksialnosti

            dir = Path.Combine(dir0, "betas");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #region Brisanje starih datotek

            if (it == 0)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "beta" + it.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            b2[i][j][k] = Beta(Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k]);

                            if (b2[i][j][k] > 0.8)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Izris slike
            /*
            using (Bitmap bmp = new Bitmap(Nx * 8, Ny * 8))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (b2[i][j][k] > 0.8)
                            {
                                for (int cx = 0; cx < 8; cx++)
                                {
                                    for (int cy = 0; cy < 8; cy++)
                                    {
                                        bmp.SetPixel(i * 8 + cx, j * 8 + cy, Color.Black);
                                    }
                                }

                                break;
                            }

                            else
                            {
                                for (int cx = 0; cx < 8; cx++)
                                {
                                    for (int cy = 0; cy < 8; cy++)
                                    {
                                        bmp.SetPixel(i * 8 + cx, j * 8 + cy, Color.White);
                                    }
                                }
                            }
                        }
                    }
                }

                bmp.Save(Path.Combine(dir, "Interferometrija" + it.ToString() + ".tif"), System.Drawing.Imaging.ImageFormat.Tiff);
            }
            */
            #endregion

            #region Skalarni ureditveni parameter

            dir = Path.Combine(dir0, "S");
            
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #region Brisanje starih datotek

            if (it == 0)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "S_points" + it.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (S[i][j][k] < 0.6)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Izpis celotnega tenzorja

            dir = Path.Combine(dir0, "results");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #region Brisanje starih datotek

            if (it == 0)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "rezultat" + it.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            b2[i][j][k] = Beta(Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k]);

                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}  {6,8:F4}  {7,8:F4}  {8,6:F4}", i, j, k, Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k], b2[i][j][k]);
                        }
                    }
                }
            }

            #endregion

            #region Izpis direktorskega polja

            dir = Path.Combine(dir0, "Direktorsko polje");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            #region Brisanje starih datotek

            if (it == 0)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            #endregion

            Direktorsko_polje(Q1, Q2, Q3, Q4, Q5);

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "direktorsko_polje" + it.ToString() + ".txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}", i, j, k, direktor[i][j][k][0], direktor[i][j][k][1], direktor[i][j][k][2]);
                        }
                    }
                }
            }

            #endregion

        }

        /// <summary>
        /// Izpiše celotni tenzor in točke visoke biaksialnosti
        /// </summary>
        /// <param name="dir">Mapa, v katero se shrani datoteka</param>
        static void Izpis_rezultatov(string dir)
        {
            #region Izpis celotnega tenzorja

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "rezultat.txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            b2[i][j][k] = Beta(Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k]);

                            writer.WriteLine("{0,4}  {1,4}  {2,4}  {3,8:F4}  {4,8:F4}  {5,8:F4}  {6,8:F4}  {7,8:F4}  {8,6:F4}", i, j, k, Q1[i][j][k], Q2[i][j][k], Q3[i][j][k], Q4[i][j][k], Q5[i][j][k], b2[i][j][k]);
                        }
                    }
                }
            }

            #endregion

            #region Izpis biaksialnosti

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "beta.txt"), false))
            {
                for (int i = 0; i < Nx; i++)
                {
                    for (int j = 0; j < Ny; j++)
                    {
                        for (int k = 0; k < Nz; k++)
                        {
                            if (b2[i][j][k] > 0.8)
                            {
                                writer.WriteLine("{0,4}  {1,4}  {2,4}", i, j, k);
                            }
                        }
                    }
                }
            }

            #endregion
        }

        #endregion


        #region Interferometry

        private void Start_interferometry_Click(object sender, EventArgs e)
        {
            #region Initialization

            draw_size = pictureBox1.Height;

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

                int ii, jj, kk, file_n;
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

                    Risi(file_n);
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

        private void Risi(int file_n)
        {
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
                int count_x, count_y;
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

                                rgbb[count_x][count_y] = (int)(inter1 * 255);

                                if (rgbb[count_x][count_y] < 0)
                                {
                                    rgbb[count_x][count_y] = 0;
                                }

                                count_y++;
                            }
                        }

                        count_x++;
                    }

                    progressBar1.Increment(1);
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

                bmp.Save(Path.Combine(dir, "Interferometrija" + file_n.ToString() + ".tif"), System.Drawing.Imaging.ImageFormat.Tiff);
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "dimenz.txt"), false))
            {
                ttx = (Nx - 1) * (int)delix + 1;
                tty = (Ny - 1) * (int)deliy + 1;

                writer.WriteLine("{0}  {1}", ttx, tty);
            }

            #endregion
        }

        #endregion

    }
}