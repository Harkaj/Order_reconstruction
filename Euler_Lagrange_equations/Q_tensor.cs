using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler_Lagrange_equations
{
    public class Q_tensor
    {
        public double dx, dy, dz;

        /// <summary>
        /// Izračuna trace od Q*Q
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        /// <returns></returns>
        public static double Half_trQ_square(double q1, double q2, double q3, double q4, double q5)
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
        public static double TrQ_cubed(double q1, double q2, double q3, double q4, double q5)
        {
            double result = -3.0 * (2.0 * q1 * q1 * q1 - 2.0 * q3 * q4 * q5 + q2 * (q5 * q5 - q4 * q4) + q1 * (-2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5));
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
        public static double Beta(double q1, double q2, double q3, double q4, double q5)
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
    }

    public class Next_step_t_independent : Q_tensor
    {
        /// <summary>
        /// Izračuna naslednje stanje za Q1
        /// </summary>
        /// <param name="i">Lega v x</param>
        /// <param name="j">Lega v y</param>
        /// <param name="k">Lega v z</param>
        /// <returns>Naslednje stanje</returns>
        public static double Next_value_Q1(int i, int j, int k, double[][][] Q1, double[][][] Q2, double[][][] Q3, double[][][] Q4, double[][][] Q5, double[][][][] E)
        {
            #region Inicializacija spremenljivk

            int ip, im, jp, jm, kp, km, Nx, Ny, Nz;
            double q1, q2, q3, q4, q5, EL_en, dEL_en, tr_Q2, E_x, E_y, E_z;
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

            Nx = Q1.Length;
            Ny = Q1[0].Length;
            Nz = Q1[0][0].Length;

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
            double dx = 0.01;
            double dy = 0.01;
            double dz = 0.01;
            #region Odvodi

            d_x = Calculus.D2(Q1[im][j][k], Q1[i][j][k], Q1[ip][j][k], dx);
            d_y = Calculus.D2(Q1[i][jm][k], Q1[i][j][k], Q1[i][jp][k], dy);
            d_z = Calculus.D2(Q1[i][j][km], Q1[i][j][k], Q1[i][j][kp], dz);
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

            //EL_en = -t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0 + deps * (E_x * E_x + E_y * E_y - 2.0 * E_z * E_z) / 2.0;
            //dEL_en = -t / 6.0 - 2.0 * q1 - (tr_Q2 + 6.0 * q1 * q1) / 2.0;

            //EL_en = (d_x + d_y + d_z) / AA + EL_en;
            //dEL_en = (-2.0 / (dx * dx) - 2.0 / (dy * dy) - 2.0 / (dz * dz)) / AA + dEL_en;

            //result = Q1[i][j][k] - kor * EL_en / dEL_en;

            //if (Math.Abs(Q1[i][j][k] - result) > eps)
            //{
            //    nap++;
            //}

            #endregion

            return result;
        }
    }

    public class Next_step_t_dependent : Q_tensor
    {

    }

    public class Next_step_inequal_L : Q_tensor
    {

    }
}
