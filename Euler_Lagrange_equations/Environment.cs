using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    public class Q_tensor
    {
        public const int LC = 1,
                         FROZEN = 2,
                         I_PLANAR = 4,
                         I_DEGENERATE = 8,
                         I_HOMEOTROPIC = 16,
                         B_FREE = 32,
                         B_PERIODIC = 64;

        private Random r = new Random();
        private readonly int _Nx, _Ny, _Nz;
        private double phi0_upper, phi0_lower, phi0_bulk;
        private bool Q_is_zero = false;

        public double[][][] Q1, Q2, Q3, Q4, Q5;
        public double[][][][] surface_normal, E, B;
        public double[][] defects_up, defects_down;
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

            this.Q_type = new int[_Nx][][];
            this.surface_normal = new double[_Nx][][][];

            for (int i = 0; i < _Nx; i++)
            {
                this.Q1[i] = new double[_Ny][];
                this.Q2[i] = new double[_Ny][];
                this.Q3[i] = new double[_Ny][];
                this.Q4[i] = new double[_Ny][];
                this.Q5[i] = new double[_Ny][];

                this.Q_type[i] = new int[_Ny][];
                this.surface_normal[i] = new double[_Ny][][];

                for (int j = 0; j < _Ny; j++)
                {
                    this.Q1[i][j] = new double[_Nz];
                    this.Q2[i][j] = new double[_Nz];
                    this.Q3[i][j] = new double[_Nz];
                    this.Q4[i][j] = new double[_Nz];
                    this.Q5[i][j] = new double[_Nz];

                    this.Q_type[i][j] = new int[_Nz];
                    this.surface_normal[i][j] = new double[_Nz][];

                    for (int k = 0; k < _Nz; k++)
                    {
                        this.surface_normal[i][i][k] = new double[3];
                    }
                }
            }

            #endregion
        }

        public void Initialize_fields()
        {
            this.E = new double[_Nx][][][];
            this.B = new double[_Nx][][][];

            for (int i = 0; i < _Nx; i++)
            {
                this.E[i] = new double[_Ny][][];
                this.B[i] = new double[_Ny][][];
                
                for (int j = 0; j < _Ny; j++)
                {
                    this.E[i][j] = new double[_Nz][];
                    this.B[i][j] = new double[_Nz][];

                    for (int k = 0; k < _Nz; k++)
                    {
                        this.E[i][j][k] = new double[3];
                        this.B[i][j][k] = new double[3];
                    }
                }
            }
        }

        public void Q_state_setup(bool topbottom_boundary, string upper, string lower, string sides)
        {
            lower = lower.ToUpper();
            upper = upper.ToUpper();
            sides = sides.ToUpper();

            for (int i = 0; i < this.Nx; i++)
            {
                for (int j = 0; j < this.Ny; j++)
                {
                    for (int k = 0; k < this.Nz; k++)
                    {
                        #region Top and bottom boundary

                        if (topbottom_boundary && k == 0)
                        {
                            switch (lower)
                            {
                                case "PLANAR PATTERNED":
                                case "PLANAR":
                                    Q_type[i][j][k] = I_PLANAR;
                                    break;
                                case "DEGENERATE":
                                    Q_type[i][j][k] = I_DEGENERATE;
                                    break;
                                case "HOMEOTROPIC":
                                    Q_type[i][j][k] = I_HOMEOTROPIC;
                                    break;
                                default:
                                    Q_type[i][j][k] = I_PLANAR;
                                    break;
                            }
                            continue;
                        }
                        if (topbottom_boundary && k == this._Nz - 1)
                        {
                            switch (upper)
                            {
                                case "PLANAR PATTERNED":
                                case "PLANAR":
                                    Q_type[i][j][k] = I_PLANAR;
                                    break;
                                case "DEGENERATE":
                                    Q_type[i][j][k] = I_DEGENERATE;
                                    break;
                                case "HOMEOTROPIC":
                                    Q_type[i][j][k] = I_HOMEOTROPIC;
                                    break;
                                default:
                                    Q_type[i][j][k] = I_PLANAR;
                                    break;
                            }
                            continue;
                        }

                        #endregion

                        #region All sides

                        if (i == 0 || i == this._Nx - 1 || 
                            j == 0 || j == this._Ny - 1 || 
                            k == 0 || k == this._Nz - 1)
                        {
                            switch (sides)
                            {
                                case "FREE":
                                    Q_type[i][j][k] = B_FREE;
                                    break;
                                case "PERIODIC":
                                    Q_type[i][j][k] = B_PERIODIC;
                                    break;
                                default:
                                    Q_type[i][j][k] = B_FREE;
                                    break;
                            }
                            continue;
                        }

                        #endregion

                        Q_type[i][j][k] = LC;
                    }
                }
            }
        }

        public void Q_state_cleanup()
        {
            for (int i = 0; i < this.Nx; i++)
            {
                for (int j = 0; j < this.Ny; j++)
                {
                    for (int k = 0; k < this.Nz; k++)
                    {
                        if (Q_type[i][j][k] == LC) { continue; }
                        Analysis.Periodic_conditions(this._Nx, i, out int im, out int ip);
                        Analysis.Periodic_conditions(this._Ny, i, out int jm, out int jp);
                        Analysis.Periodic_conditions(this._Nz, i, out int km, out int kp);
                        if (Q_type[im][j][k] != LC && Q_type[ip][j][k] != LC && 
                            Q_type[i][jm][k] != LC && Q_type[i][jp][k] != LC &&
                            Q_type[i][j][km] != LC && Q_type[i][j][kp] != LC)
                        {
                            Q_type[i][j][k] = FROZEN;
                        }
                    }
                }
            }
        }

        public void Insert_setup(string type, int size1, int size2, int c_x, int c_y, int c_z)
        {
            double R1, R2;
            type = type.ToUpper();
            switch (type)
            {
                case "POINT":
                    Q_type[c_x][c_y][c_z] = FROZEN;
                    break;
                case "SPHERE MELTED":
                    for (int i = c_x - size1; i < c_x + size1; i++)
                    {
                        for (int j = c_y - size1; j < c_y + size1; j++)
                        {
                            for (int k = c_z - size1; k < c_z + size1; k++)
                            {
                                R1 = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y) + (k - c_z) * (k - c_z);
                                if (R1 <= size1 * size1) { Q_type[i][j][k] = FROZEN; }
                            }
                        }
                    }
                    break;
                case "SPHERE HOMEOTROPIC":
                    for (int i = c_x - size1; i < c_x + size1; i++)
                    {
                        for (int j = c_y - size1; j < c_y + size1; j++)
                        {
                            for (int k = c_z - size1; k < c_z + size1; k++)
                            {
                                R1 = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y) + (k - c_z) * (k - c_z);
                                if (R1 <= size1 * size1) { Q_type[i][j][k] = I_HOMEOTROPIC; }
                            }
                        }
                    }
                    break;
                case "SPHERE PLANAR":
                    for (int i = c_x - size1; i < c_x + size1; i++)
                    {
                        for (int j = c_y - size1; j < c_y + size1; j++)
                        {
                            for (int k = c_z - size1; k < c_z + size1; k++)
                            {
                                R1 = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y) + (k - c_z) * (k - c_z);
                                if (R1 <= size1 * size1) { Q_type[i][j][k] = I_DEGENERATE; }
                            }
                        }
                    }
                    break;
                case "TORUS HOMEOTROPIC":
                    for (int i = c_x - size1 - size2; i < c_x + size1 + size2; i++)
                    {
                        for (int j = c_y - size1 - size2; j < c_y + size1 + size2; j++)
                        {
                            for (int k = c_z - size2; k < c_z + size2; k++)
                            {
                                R1 = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);
                                R1 = Math.Sqrt(R1);
                                R2 = (R1 - size1) * (R1 - size1) + (k - c_z) * (k - c_z);
                                if (R2 <= size2 * size2) { Q_type[i][j][k] = I_HOMEOTROPIC; }
                            }
                        }
                    }
                    break;
                case "TORUS PLANAR":
                    for (int i = c_x - size1 - size2; i < c_x + size1 + size2; i++)
                    {
                        for (int j = c_y - size1 - size2; j < c_y + size1 + size2; j++)
                        {
                            for (int k = c_z - size2; k < c_z + size2; k++)
                            {
                                R1 = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);
                                R1 = Math.Sqrt(R1);
                                R2 = (R1 - size1) * (R1 - size1) + (k - c_z) * (k - c_z);
                                if (R2 <= size2 * size2) { Q_type[i][j][k] = I_PLANAR; }
                            }
                        }
                    }
                    break;
                case "CYLINDER":
                    for (int i = c_x - size1; i < c_x + size1; i++)
                    {
                        for (int j = c_y - size1; j < c_y + size1; j++)
                        {
                            for (int k = 0; k < this._Nz; k++)
                            {
                                R1 = (i - c_x) * (i - c_x) + (j - c_y) * (j - c_y);
                                if (R1 <= size1 * size1) { Q_type[i][j][k] = FROZEN; }
                            }
                        }
                    }
                    break;
                default:
                    Q_type[c_x][c_y][c_z] = FROZEN;
                    break;
            }
        }

        public void Initialize_field_values(string upper, string lower, string bulk, double tt)
        {
            double theta = 0.0, phi = 0.0;

            for (int i = 0; i < this.Nx; i++)
            {
                for (int j = 0; j < this.Ny; j++)
                {
                    for (int k = 0; k < this.Nz; k++)
                    {
                        Q_is_zero = false;

                        #region Setting theta and phi

                        switch (this.Q_type[i][j][k])
                        {
                            case LC:
                                Q_bulk(bulk, i, j, k, out theta, out phi);
                                break;
                            case FROZEN:
                                Q_is_zero = true;
                                break;
                            case I_PLANAR:
                            case I_DEGENERATE:
                            case I_HOMEOTROPIC:
                                if (k == 0) { Q_boundary_topbottom(lower, i, j, k, defects_down, phi0_lower, out theta, out phi); }
                                if (k == this._Nz - 1) { Q_boundary_topbottom(upper, i, j, k, defects_up, phi0_upper, out theta, out phi); }
                                else { theta = 0.0; phi = 0.0; }
                                break;
                            case B_FREE:
                            case B_PERIODIC:
                            default:
                                Q_bulk(bulk, i, j, k, out theta, out phi);
                                break;
                        }

                        #endregion

                        #region Isotropic or frozen state 

                        if (Q_is_zero)
                        {
                            Q1[i][j][k] = 0.0;
                            Q2[i][j][k] = 0.0;
                            Q3[i][j][k] = 0.0;
                            Q4[i][j][k] = 0.0;
                            Q5[i][j][k] = 0.0;

                            continue;
                        }

                        #endregion

                        #region Assigning Q values

                        Q1[i][j][k] = tt * (1.0 / 6.0 - (Math.Cos(theta) * Math.Cos(theta)) / 2.0);
                        Q2[i][j][k] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Cos(2.0 * phi)) / 2.0;
                        Q3[i][j][k] = tt * (Math.Sin(theta) * Math.Sin(theta) * Math.Sin(2.0 * phi)) / 2.0;
                        Q4[i][j][k] = tt * (Math.Sin(2.0 * theta) * Math.Cos(phi)) / 2.0;
                        Q5[i][j][k] = tt * (Math.Sin(2.0 * theta) * Math.Sin(phi)) / 2.0;

                        #endregion

                    }
                }
            }
        }

        private void Q_bulk(string bulk, int i, int j, int k, out double theta, out double phi)
        {
            double directorx, directory, directorz, RR;
            bulk = bulk.ToUpper();
            switch (bulk)
            {
                case "ISOTROPIC":
                    Q_is_zero = true;
                    theta = 0.0;
                    phi = 0.0;
                    break;
                case "PLANAR":
                    theta = Math.PI / 2.0;
                    phi = phi0_bulk;
                    break;
                case "HOMEOTROPIC":
                    theta = 0.0;
                    phi = 0.0;
                    break;
                case "ESCAPED":
                    phi = Math.Atan2(j - Ny / 2, i - Nx / 2) + phi0_bulk;// + 0.1 * (0.5 - r.NextDouble());
                    theta = 2.0 * Math.Atan(Math.Sqrt((i - Nx / 2) * (i - Nx / 2) + (j - Ny / 2) * (j - Ny / 2)) / (Nx / 2));

                    if (Math.Abs(theta) > (Math.PI / 2.0))
                    {
                        theta = Math.PI / 2.0;
                    }
                    break;
                case "BOUNDARY DEFECT":
                    theta = Math.PI / 2.0;
                    phi = phi0_lower;

                    for (int d = 0; d < this.defects_down.Length; d++)
                    {
                        phi += this.defects_down[d][2] * Math.Atan2(j - this.defects_down[d][1], i - this.defects_down[d][0]);
                    }
                    break;
                case "TWIST":
                    theta = Math.PI * k / Nz;
                    phi = -0.5 * Math.Atan2(j - this.defects_down[0][1], i - this.defects_down[0][0]);

                    directorx = Math.Cos(phi);
                    directory = Math.Sin(phi) * Math.Cos(theta);
                    directorz = Math.Sin(phi) * Math.Sin(theta);

                    theta = Math.Acos(directorz);
                    phi = Math.Atan2(directory, directorx);
                    break;
                case "DOUBLE TWIST":
                    theta = Math.PI / 2.0;
                    for (int d = 0; d < this.defects_down.Length; d++)
                    {
                        RR = Math.Sqrt((i - this.defects_down[d][0]) * (i - this.defects_down[d][0]) + (j - this.defects_down[d][1]) * (j - this.defects_down[d][1]));

                        if (RR < 50.0)
                        {
                            theta = 2.0 * Math.Atan(RR / 50.0);
                        }
                    }

                    phi = Math.PI / 2.0;
                    for (int d = 0; d < this.defects_down.Length; d++)
                    {
                        phi += this.defects_down[d][2] * Math.Atan2(j - this.defects_down[d][1], i - this.defects_down[d][0]);
                    }
                    break;
                default: //Other
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
            }
        }

        private void Q_boundary_topbottom(string boundary, int i, int j, int k, double[][] defects, double phi0, out double theta, out double phi)
        {
            double R_ij;
            boundary = boundary.ToUpper();
            switch (boundary)
            {
                case "PLANAR PATTERNED":  //Defect pattern
                    theta = Math.PI / 2.0;
                    phi = phi0;
                    for (int d = 0; d < defects.Length; d++)
                    {
                        phi += defects[d][2] * Math.Atan2(j - defects[d][1], i - defects[d][0]);
                    }
                    break;
                case "PLANAR":
                    theta = Math.PI / 2.0;
                    phi = phi0;
                    break;
                case "DEGENERATE":
                    theta = Math.PI / 2.0;
                    phi = Math.PI * r.NextDouble();
                    break;
                case "HOMEOTROPIC":
                    theta = 0.0;
                    phi = 0.0;
                    break;
                default:  //Other
                    theta = Math.PI / 2.0;
                    R_ij = (i - this._Nx / 2) * (i - this._Nx / 2) + (j - this._Ny / 2) * (j - this._Ny / 2);
                    R_ij = Math.Sqrt(R_ij);
                    if (Math.Abs(R_ij) > 30.0 && Math.Abs(R_ij) <= 60.0)
                    {
                        phi = phi0 + (R_ij - 45.0) * Math.PI / 30;
                    }
                    else if (Math.Abs(R_ij) > 60.0 && Math.Abs(R_ij) <= 90.0)
                    {
                        phi = phi0 - (R_ij - 75.0) * Math.PI / 30;
                    }
                    else { phi = phi0 + Math.PI / 2.0; }
                    break;
            }
        }

        public void Q_values(int i, int j, int k, out double q1, out double q2, out double q3, out double q4, out double q5)
        {
            q1 = this.Q1[i][j][k];
            q2 = this.Q2[i][j][k];
            q3 = this.Q3[i][j][k];
            q4 = this.Q4[i][j][k];
            q5 = this.Q5[i][j][k];
        }

        public void E_B_values(int i, int j, int k, out double[] E, out double[] B)
        {
            E = new double[3];
            B = new double[3];

            E[0] = this.E[i][j][k][0];
            E[1] = this.E[i][j][k][1];
            E[2] = this.E[i][j][k][2];

            B[0] = this.B[i][j][k][0];
            B[1] = this.B[i][j][k][1];
            B[2] = this.B[i][j][k][2];
        }

    }

    public class Parameters
    {
        private readonly int _itmax;
        private readonly double _eps, _kor, _R_min, _R_max, _a, _t, _w, _dt;
        private readonly double _AA, _tt, _sb, _gamma, _delta_eps, _delta_mu;
        private readonly double _L1, _L2, _L3, _L_chiral;
        private double _dx, _dy, _dz, _dxy, _dxz, _dyz, _dxyz;
        private int _nap;

        public int itmax { get { return this._itmax; } }
        public double eps { get { return this._eps; } }
        public double kor { get { return this._kor; } }
        public double Rmin { get { return this._R_min; } }
        public double Rmax { get { return this._R_max; } }
        public double a { get { return this._a; } }
        public double t { get { return this._t; } }
        public double w { get { return this._w; } }
        public double dt { get { return this._dt; } }

        public double AA { get { return this._AA; } }
        public double tt { get { return this._tt; } }
        public double sb { get { return this._sb; } }
        public double gamma { get { return this._gamma; } }
        public double delta_eps { get { return this._delta_eps; } }
        public double delta_mu { get { return this._delta_mu; } }

        public double L1 { get { return this._L1; } }
        public double L2 { get { return this._L2; } }
        public double L3 { get { return this._L3; } }
        public double L_chiral { get { return this._L_chiral; } }

        public double dx { get { return this._dx; } }
        public double dy { get { return this._dy; } }
        public double dz { get { return this._dz; } }
        public double dxy { get { return this._dxy; } }
        public double dxz { get { return this._dxz; } }
        public double dyz { get { return this._dyz; } }
        public double dxyz { get { return this._dxyz; } }
        public int nap { get { return this._nap; } set { this._nap = value; } }

        public Parameters(int itmax, double eps, double kor, double Rmin, double Rmax, double a, double t, double w, double dt)
        {
            this._itmax = itmax;
            this._eps = eps;
            this._kor = kor;
            this._R_min = Rmin;
            this._R_max = Rmax;
            this._a = a;
            this._t = t;
            this._w = w;
            this._dt = dt;

            this._tt = 1.0 + Math.Sqrt(1.0 - t);
            this._AA = this._a * this._a;
            this._sb = this._tt;
            this._gamma = 1.0;
        }

        public void Dimensions(int Nx, int Ny, int Nz)
        {
            this._dx = (this._R_max - this._R_min) / ((double)Nx - 1.0);
            this._dy = (this._R_max - this._R_min) / ((double)Ny - 1.0);
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
