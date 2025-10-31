using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    public class Q_tensor
    {
        private readonly int _Nx, _Ny, _Nz;
        public double[][][] Q1, Q2, Q3, Q4, Q5, Q1_n, Q2_n, Q3_n, Q4_n, Q5_n;
        public double[][][] Q1_plate, Q2_plate, Q3_plate, Q4_plate, Q5_plate;
        public double[][][][] direktor, E, B;
        public int[][][] Q_type;

        public int Nx { get { return this._Nx; } }
        public int Ny { get { return this._Ny; } }
        public int Nz { get { return this._Nz; } }

        public Q_tensor(int Nx, int Ny, int Nz)
        {
            this._Nx = Nx;
            this._Ny = Ny;
            this._Nz = Nz;

            #region Tensor field arrays

            this.Q1 = new double[_Nx][][];
            this.Q2 = new double[_Nx][][];
            this.Q3 = new double[_Nx][][];
            this.Q4 = new double[_Nx][][];
            this.Q5 = new double[_Nx][][];

            this.Q1_n = new double[_Nx][][];
            this.Q2_n = new double[_Nx][][];
            this.Q3_n = new double[_Nx][][];
            this.Q4_n = new double[_Nx][][];
            this.Q5_n = new double[_Nx][][];

            this.Q1_plate = new double[_Nx][][];
            this.Q2_plate = new double[_Nx][][];
            this.Q3_plate = new double[_Nx][][];
            this.Q4_plate = new double[_Nx][][];
            this.Q5_plate = new double[_Nx][][];

            this.Q_type = new int[_Nx][][];

            for (int i = 0; i < _Nx; i++)
            {
                this.Q1[i] = new double[_Ny][];
                this.Q2[i] = new double[_Ny][];
                this.Q3[i] = new double[_Ny][];
                this.Q4[i] = new double[_Ny][];
                this.Q5[i] = new double[_Ny][];

                this.Q1_n[i] = new double[_Ny][];
                this.Q2_n[i] = new double[_Ny][];
                this.Q3_n[i] = new double[_Ny][];
                this.Q4_n[i] = new double[_Ny][];
                this.Q5_n[i] = new double[_Ny][];

                this.Q1_plate[i] = new double[_Ny][];
                this.Q2_plate[i] = new double[_Ny][];
                this.Q3_plate[i] = new double[_Ny][];
                this.Q4_plate[i] = new double[_Ny][];
                this.Q5_plate[i] = new double[_Ny][];

                this.Q_type[i] = new int[_Ny][];

                for (int j = 0; j < _Ny; j++)
                {
                    this.Q1[i][j] = new double[_Nz];
                    this.Q2[i][j] = new double[_Nz];
                    this.Q3[i][j] = new double[_Nz];
                    this.Q4[i][j] = new double[_Nz];
                    this.Q5[i][j] = new double[_Nz];

                    this.Q1_n[i][j] = new double[_Nz];
                    this.Q2_n[i][j] = new double[_Nz];
                    this.Q3_n[i][j] = new double[_Nz];
                    this.Q4_n[i][j] = new double[_Nz];
                    this.Q5_n[i][j] = new double[_Nz];

                    this.Q1_plate[i][j] = new double[_Nz];
                    this.Q2_plate[i][j] = new double[_Nz];
                    this.Q3_plate[i][j] = new double[_Nz];
                    this.Q4_plate[i][j] = new double[_Nz];
                    this.Q5_plate[i][j] = new double[_Nz];

                    this.Q_type[i][j] = new int[_Nz];
                }
            }

            #endregion
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

    public class Parameters
    {
        private readonly int _itmax;
        private readonly double _eps, _kor, _Rmin, _Rmax, _a, _t, _w;
        private readonly double _AA, _tt, _sb, _gamma, _delta_eps, _delta_mu;
        private double _dx, _dy, _dz, _dxy, _dxz, _dyz, _dxyz;

        public int itmax { get { return this._itmax; } }
        public double eps { get { return this._eps; } }
        public double kor { get { return this._kor; } }
        public double Rmin { get { return this._Rmin; } }
        public double Rmax { get { return this._Rmax; } }
        public double a { get { return this._a; } }
        public double t { get { return this._t; } }
        public double w { get { return this._w; } }

        public double AA { get { return this._AA; } }
        public double tt { get { return this._tt; } }
        public double sb { get { return this._sb; } }
        public double gamma { get { return this._gamma; } }
        public double delta_eps { get { return this._delta_eps; } }
        public double delta_mu { get { return this._delta_mu; } }

        public double dx { get { return this._dx; } }
        public double dy { get { return this._dy; } }
        public double dz { get { return this._dz; } }
        public double dxy { get { return this._dxy; } }
        public double dxz { get { return this._dxz; } }
        public double dyz { get { return this._dyz; } }
        public double dxyz { get { return this._dxyz; } }

        public Parameters(int itmax, double eps, double kor, double Rmin, double Rmax, double a, double t, double w)
        {
            this._itmax = itmax;
            this._eps = eps;
            this._kor = kor;
            this._Rmin = Rmin;
            this._Rmax = Rmax;
            this._a = a;
            this._t = t;
            this._w = w;

            this._tt = 1.0 + Math.Sqrt(1.0 - t);
            this._AA = this._a * this._a;
            this._sb = this._tt;
            this._gamma = 1.0;
        }

        public void Dimensions(int Nx, int Ny, int Nz)
        {
            this._dx = (this._Rmax - this._Rmin) / ((double)Nx - 1.0);
            this._dy = (this._Rmax - this._Rmin) / ((double)Ny - 1.0);
            this._dz = 1.0 / ((double)Nz - 1.0);

            this._dxy = Math.Sqrt(this._dx * this._dx + this._dy * this._dy);
            this._dxz = Math.Sqrt(this._dx * this._dx + this._dz * this._dz);
            this._dyz = Math.Sqrt(this._dy * this._dy + this._dz * this._dz);
            this._dxyz = Math.Sqrt(this._dx * this._dx + this._dy * this._dy + this._dz * this._dz);
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
