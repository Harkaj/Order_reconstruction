using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    public class Next_step_calculations
    {
        double Next_value_Q1(int i, int j, int k, Q_tensor Q) { return 0.0; }
        double Next_value_Q2(int i, int j, int k, Q_tensor Q) { return 0.0; }
        double Next_value_Q3(int i, int j, int k, Q_tensor Q) { return 0.0; }
        double Next_value_Q4(int i, int j, int k, Q_tensor Q) { return 0.0; }
        double Next_value_Q5(int i, int j, int k, Q_tensor Q) { return 0.0; }
    }

    public class Minimization : Next_step_calculations
    {
        private Q_tensor _Q, _Q_n;
        private Parameters _param;

        public Minimization(Q_tensor Q, Q_tensor Q_n, Parameters param)
        {
            this._Q = Q;
            this._Q_n = Q_n;
            this._param = param;
        }

        public double Next_value_Q1(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk
            
            double d_x, d_y, d_z, EL_en, dEL_en, tr_Q2;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);
            
            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q1[im][j][k], Q.Q1[i][j][k], Q.Q1[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q1[i][jm][k], Q.Q1[i][j][k], Q.Q1[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q1[i][j][km], Q.Q1[i][j][k], Q.Q1[i][j][kp], _param.dz);
            
            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0 + _param.delta_eps * (E[0] * E[0] + E[1] * E[1] - 2.0 * E[2] * E[2]) / 2.0;
            dEL_en = -_param.t / 6.0 - 2.0 * q1 - (tr_Q2 + 6.0 * q1 * q1) / 2.0;

            EL_en = (d_x + d_y + d_z) / _param.AA + EL_en;
            dEL_en = (-2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz)) / _param.AA + dEL_en;

            result = Q.Q1[i][j][k] - _param.kor * EL_en / dEL_en;
            //if (Math.Abs(Q.Q1[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q2(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, dEL_en, tr_Q2;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q2[im][j][k], Q.Q2[i][j][k], Q.Q2[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q2[i][jm][k], Q.Q2[i][j][k], Q.Q2[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q2[i][j][km], Q.Q2[i][j][k], Q.Q2[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0;
            EL_en += _param.delta_eps * (E[0] * E[0] - E[1] * E[1]) / 4.0 + _param.delta_mu * (B[0] * B[0] - B[1] * B[1]) / 4.0;
            dEL_en = -_param.t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q2 * q2) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = Q.Q2[i][j][k] - _param.kor * EL_en / dEL_en;
            //if (Math.Abs(Q.Q2[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q3(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, dEL_en, tr_Q2;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q3[im][j][k], Q.Q3[i][j][k], Q.Q3[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q3[i][jm][k], Q.Q3[i][j][k], Q.Q3[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q3[i][j][km], Q.Q3[i][j][k], Q.Q3[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + _param.delta_eps * E[0] * E[1] / 2.0 + _param.delta_mu * B[0] * B[1] / 2.0;
            dEL_en = -_param.t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q3 * q3) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = Q.Q3[i][j][k] - _param.kor * EL_en / dEL_en;
            //if (Math.Abs(Q.Q3[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q4(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z,  EL_en, dEL_en, tr_Q2;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            
            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q4[im][j][k], Q.Q4[i][j][k], Q.Q4[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q4[i][jm][k], Q.Q4[i][j][k], Q.Q4[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q4[i][j][km], Q.Q4[i][j][k], Q.Q4[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + _param.delta_eps * E[0] * E[2] / 2.0 + _param.delta_mu * B[0] * B[2] / 2.0;
            dEL_en = -_param.t / 6.0 - q1 + q2 - (tr_Q2 + 2.0 * q4 * q4) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = Q.Q4[i][j][k] - _param.kor * EL_en / dEL_en;
            //if (Math.Abs(Q.Q4[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q5(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, dEL_en, tr_Q2;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q5[im][j][k], Q.Q5[i][j][k], Q.Q5[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q5[i][jm][k], Q.Q5[i][j][k], Q.Q5[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q5[i][j][km], Q.Q5[i][j][k], Q.Q5[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0 + _param.delta_eps * E[1] * E[2] / 2.0 + _param.delta_mu * B[1] * B[2] / 2.0;
            dEL_en = -_param.t / 6.0 - q1 - q2 - (tr_Q2 + 2.0 * q5 * q5) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = Q.Q5[i][j][k] - _param.kor * EL_en / dEL_en;
            //if (Math.Abs(Q.Q5[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

    }

    public class Time_evolution : Next_step_calculations
    {
        private Q_tensor _Q, _Q_n;
        private Parameters _param;

        public Time_evolution(Q_tensor Q, Q_tensor Q_n, Parameters param)
        {
            this._Q = Q;
            this._Q_n = Q_n;
            this._param = param;
        }

        public void Iteration_Q1(Q_tensor Q, Q_tensor QQ)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        QQ.Q1[i][j][k] = Next_value_Q1(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q2(Q_tensor Q, Q_tensor QQ)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        QQ.Q2[i][j][k] = Next_value_Q2(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q3(Q_tensor Q, Q_tensor QQ)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        QQ.Q3[i][j][k] = Next_value_Q3(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q4(Q_tensor Q, Q_tensor QQ)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        QQ.Q4[i][j][k] = Next_value_Q4(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q5(Q_tensor Q, Q_tensor QQ)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        QQ.Q5[i][j][k] = Next_value_Q5(i, j, k, Q);
                    }
                }
            }
        }

        public double Next_value_Q1(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, tr_Q2, dq;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q1[im][j][k], Q.Q1[i][j][k], Q.Q1[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q1[i][jm][k], Q.Q1[i][j][k], Q.Q1[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q1[i][j][km], Q.Q1[i][j][k], Q.Q1[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_param.t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0;
            EL_en += _param.delta_eps * (E[0] * E[0] + E[1] * E[1] - 2.0 * E[2] * E[2]) / 12.0 + _param.delta_mu * (B[0] * B[0] + B[1] * B[1] - 2.0 * B[2] * B[2]) / 12.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _param.AA + EL_en) / _param.gamma;

            result = Q.Q1[i][j][k] + dq * _param.dt;
            //if (Math.Abs(Q.Q1[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public double Next_value_Q2(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, tr_Q2, dq;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q2[im][j][k], Q.Q2[i][j][k], Q.Q2[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q2[i][jm][k], Q.Q2[i][j][k], Q.Q2[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q2[i][j][km], Q.Q2[i][j][k], Q.Q2[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_param.t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0;
            EL_en += _param.delta_eps * (E[0] * E[0] - E[1] * E[1]) / 4.0 + _param.delta_mu * (B[0] * B[0] - B[1] * B[1]) / 4.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _param.AA + EL_en) / _param.gamma;

            result = Q.Q2[i][j][k] + dq * _param.dt;
            //if (Math.Abs(Q.Q2[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public double Next_value_Q3(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, tr_Q2, dq;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q3[im][j][k], Q.Q3[i][j][k], Q.Q3[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q3[i][jm][k], Q.Q3[i][j][k], Q.Q3[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q3[i][j][km], Q.Q3[i][j][k], Q.Q3[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_param.t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + _param.delta_eps * E[0] * E[1] / 2.0 + _param.delta_mu * B[0] * B[1] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _param.AA + EL_en) / _param.gamma;

            result = Q.Q3[i][j][k] + dq * _param.dt;
            //if (Math.Abs(Q.Q3[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public double Next_value_Q4(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, tr_Q2, dq;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q4[im][j][k], Q.Q4[i][j][k], Q.Q4[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q4[i][jm][k], Q.Q4[i][j][k], Q.Q4[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q4[i][j][km], Q.Q4[i][j][k], Q.Q4[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_param.t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + _param.delta_eps * E[0] * E[2] / 2.0 + _param.delta_mu * B[0] * B[2] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _param.AA + EL_en) / _param.gamma;

            result = Q.Q4[i][j][k] + dq * _param.dt;
            //if (Math.Abs(Q.Q4[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public double Next_value_Q5(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, tr_Q2, dq;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q5[im][j][k], Q.Q5[i][j][k], Q.Q5[ip][j][k], _param.dx);
            d_y = Calculus.D2(Q.Q5[i][jm][k], Q.Q5[i][j][k], Q.Q5[i][jp][k], _param.dy);
            d_z = Calculus.D2(Q.Q5[i][j][km], Q.Q5[i][j][k], Q.Q5[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_param.t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0 + _param.delta_eps * E[1] * E[2] / 2.0 + _param.delta_mu * B[1] * B[2] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _param.AA + EL_en) / _param.gamma;

            result = Q.Q5[i][j][k] + dq * _param.dt;
            //if (Math.Abs(Q.Q5[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

    }
}
