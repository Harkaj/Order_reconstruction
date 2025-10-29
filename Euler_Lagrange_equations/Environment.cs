using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    public class Environment
    {
        public double dx, dy, dz, dxy, dxz, dyz, dxyz, eps, Rmi, Rma;
        public double t, tt, a, AA, kor, w, sb, BB, B_f, Ex, Ey, Ez, E_max;
        public double gamma, dt, k1, k2, k3, L1, L2, L3, L_chiral, deps, dmu;
        public double[][][] Q1, Q2, Q3, Q4, Q5, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n, b2, S;
        public double[][][] Q1_plate, Q2_plate, Q3_plate, Q4_plate, Q5_plate;
        public double[][][][] direktor, E, B;
        public int[][][] Q_type;

        public void Initialize_Q(int Nx, int Ny, int Nz)
        {
            this.Q1 = new double[Nx][][];
            this.Q2 = new double[Nx][][];
            this.Q3 = new double[Nx][][];
            this.Q4 = new double[Nx][][];
            this.Q5 = new double[Nx][][];

            this.Q1_n = new double[Nx][][];
            this.Q2_n = new double[Nx][][];
            this.Q3_n = new double[Nx][][];
            this.Q4_n = new double[Nx][][];
            this.Q5_n = new double[Nx][][];

            this.Q_type = new int[Nx][][];

            for (int i = 0; i < Nx; i++)
            {
                this.Q1[i] = new double[Ny][];
                this.Q2[i] = new double[Ny][];
                this.Q3[i] = new double[Ny][];
                this.Q4[i] = new double[Ny][];
                this.Q5[i] = new double[Ny][];

                this.Q1_n[i] = new double[Ny][];
                this.Q2_n[i] = new double[Ny][];
                this.Q3_n[i] = new double[Ny][];
                this.Q4_n[i] = new double[Ny][];
                this.Q5_n[i] = new double[Ny][];

                this.Q_type[i] = new int[Ny][];

                for (int j = 0; j < Ny; j++)
                {
                    this.Q1[i][j] = new double[Nz];
                    this.Q2[i][j] = new double[Nz];
                    this.Q3[i][j] = new double[Nz];
                    this.Q4[i][j] = new double[Nz];
                    this.Q5[i][j] = new double[Nz];

                    this.Q1_n[i][j] = new double[Nz];
                    this.Q2_n[i][j] = new double[Nz];
                    this.Q3_n[i][j] = new double[Nz];
                    this.Q4_n[i][j] = new double[Nz];
                    this.Q5_n[i][j] = new double[Nz];

                    this.Q_type[i][j] = new int[Nz];
                }
            }
        }

        public void Initialize_fields(int Nx, int Ny, int Nz)
        {
            this.direktor = new double[Nx][][][];
            this.E = new double[Nx][][][];
            this.B = new double[Nx][][][];

            for (int i = 0; i < Nx; i++)
            {
                this.direktor[i] = new double[Ny][][];
                this.E[i] = new double[Ny][][];
                this.B[i] = new double[Ny][][];
                
                for (int j = 0; j < Ny; j++)
                {
                    this.direktor[i][j] = new double[Nz][];
                    this.E[i][j] = new double[Nz][];
                    this.B[i][j] = new double[Nz][];

                    for (int k = 0; k < Nz; k++)
                    {
                        this.direktor[i][j][k] = new double[3];
                        this.E[i][j][k] = new double[3];
                        this.B[i][j][k] = new double[3];
                    }
                }
            }
        }

    }

    public static class Environment_static
    {
        public static void Initialize_Q(int Nx, int Ny, int Nz, double[][][] Q1, 
                                        double[][][] Q2, double[][][] Q3, double[][][] Q4, 
                                        double[][][] Q5, double[][][] Q1_n, double[][][] Q2_n, 
                                        double[][][] Q3_n, double[][][] Q4_n, double[][][] Q5_n)
        {
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

            for (int i = 0; i < Nx; i++)
            {
                Q1[i] = new double[Nx][];
                Q2[i] = new double[Nx][];
                Q3[i] = new double[Nx][];
                Q4[i] = new double[Nx][];
                Q5[i] = new double[Nx][];

                Q1_n[i] = new double[Nx][];
                Q2_n[i] = new double[Nx][];
                Q3_n[i] = new double[Nx][];
                Q4_n[i] = new double[Nx][];
                Q5_n[i] = new double[Nx][];

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
                }
            }
        }


    }
}
