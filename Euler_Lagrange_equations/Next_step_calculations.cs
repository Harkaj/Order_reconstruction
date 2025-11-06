using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    public class Minimization
    {
        protected Q_tensor _Q, _Q_n;
        protected Parameters _P;

        public Minimization(Q_tensor Q, Q_tensor Q_n, Parameters P)
        {
            this._Q = Q;
            this._Q_n = Q_n;
            this._P = P;
        }

        public void Iteration_Q1(Q_tensor Q, Q_tensor Q_n)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        Q_n.Q1[i][j][k] = Next_value_Q1(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q2(Q_tensor Q, Q_tensor Q_n)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        Q_n.Q2[i][j][k] = Next_value_Q2(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q3(Q_tensor Q, Q_tensor Q_n)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        Q_n.Q3[i][j][k] = Next_value_Q3(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q4(Q_tensor Q, Q_tensor Q_n)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        Q_n.Q4[i][j][k] = Next_value_Q4(i, j, k, Q);
                    }
                }
            }
        }
        public void Iteration_Q5(Q_tensor Q, Q_tensor Q_n)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC) { continue; }
                        Q_n.Q5[i][j][k] = Next_value_Q5(i, j, k, Q);
                    }
                }
            }
        }

        public virtual double Next_value_Q1(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q1(i, j, k, Q);
            f_e = Elastic_term(i, j, k, Q, Q.Q1);
            f_f = Field_term_F1(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q1(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q1[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }

        public virtual double Next_value_Q2(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q2(i, j, k, Q);
            f_e = Elastic_term(i, j, k, Q, Q.Q2);
            f_f = Field_term_F2(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q2(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q2[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }

        public virtual double Next_value_Q3(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q3(i, j, k, Q);
            f_e = Elastic_term(i, j, k, Q, Q.Q3);
            f_f = Field_term_F3(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q3(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q3[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }

        public virtual double Next_value_Q4(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q4(i, j, k, Q);
            f_e = Elastic_term(i, j, k, Q, Q.Q4);
            f_f = Field_term_F4(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q4(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q4[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }

        public virtual double Next_value_Q5(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q5(i, j, k, Q);
            f_e = Elastic_term(i, j, k, Q, Q.Q5);
            f_f = Field_term_F4(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q5(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q5[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }

        #region Condensation terms

        public double Condensation_term_Q1(int i, int j, int k, Q_tensor Q)
        {
            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            double result = -_P.t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0;

            return result;
        }

        public double Condensation_term_Q2(int i, int j, int k, Q_tensor Q)
        {
            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            return -_P.t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0;
        }

        public double Condensation_term_Q3(int i, int j, int k, Q_tensor Q)
        {
            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            return -_P.t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0;
        }

        public double Condensation_term_Q4(int i, int j, int k, Q_tensor Q)
        {
            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            return -_P.t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0;
        }

        public double Condensation_term_Q5(int i, int j, int k, Q_tensor Q)
        {
            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            return -_P.t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0;
        }

        #endregion

        public virtual double Elastic_term(int i, int j, int k, Q_tensor Q, double[][][] Qn)
        {
            double d_x, d_y, d_z;

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            d_x = Calculus.D2(Qn[im][j][k], Qn[i][j][k], Qn[ip][j][k], _P.dx);
            d_y = Calculus.D2(Qn[i][jm][k], Qn[i][j][k], Qn[i][jp][k], _P.dy);
            d_z = Calculus.D2(Qn[i][j][km], Qn[i][j][k], Qn[i][j][kp], _P.dz);

            return d_x + d_y + d_z;
        }

        #region Field terms

        public double Field_term_F1(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * (E[0] * E[0] + E[1] * E[1] - 2.0 * E[2] * E[2]) / 12.0 + _P.delta_mu * (B[0] * B[0] + B[1] * B[1] - 2.0 * B[2] * B[2]) / 12.0;
        }

        public double Field_term_F2(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * (E[0] * E[0] - E[1] * E[1]) / 4.0 + _P.delta_mu * (B[0] * B[0] - B[1] * B[1]) / 4.0;
        }

        public double Field_term_F3(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * E[0] * E[1] / 2.0 + _P.delta_mu * B[0] * B[1] / 2.0;
        }

        public double Field_term_F4(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * E[0] * E[2] / 2.0 + _P.delta_mu * B[0] * B[2] / 2.0;
        }

        public double Field_term_F5(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * E[1] * E[2] / 2.0 + _P.delta_mu * B[1] * B[2] / 2.0;
        }

        #endregion

        #region Calculating subsequent value

        public virtual double Subsequent_value_Q1(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            double dEL_en;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            dEL_en = -_P.t / 6.0 - 2.0 * q1 - (tr_Q2 + 6.0 * q1 * q1) / 2.0;
            dEL_en = -2.0 / (_P.dx * _P.dx) - 2.0 / (_P.dy * _P.dy) - 2.0 / (_P.dz * _P.dz) + dEL_en * _P.AA;

            return Q.Q2[i][j][k] - _P.kor * EL_en / dEL_en;
        }

        public virtual double Subsequent_value_Q2(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            double dEL_en;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            dEL_en = -_P.t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q2 * q2) / 2.0;
            dEL_en = -2.0 / (_P.dx * _P.dx) - 2.0 / (_P.dy * _P.dy) - 2.0 / (_P.dz * _P.dz) + dEL_en * _P.AA;

            return Q.Q2[i][j][k] - _P.kor * EL_en / dEL_en;
        }

        public virtual double Subsequent_value_Q3(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            double dEL_en;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            dEL_en = -_P.t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q3 * q3) / 2.0;
            dEL_en = -2.0 / (_P.dx * _P.dx) - 2.0 / (_P.dy * _P.dy) - 2.0 / (_P.dz * _P.dz) + dEL_en * _P.AA;

            return Q.Q3[i][j][k] - _P.kor * EL_en / dEL_en;
        }

        public virtual double Subsequent_value_Q4(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            double dEL_en;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            dEL_en = -_P.t / 6.0 - q1 + q2 - (tr_Q2 + 2.0 * q4 * q4) / 2.0;
            dEL_en = -2.0 / (_P.dx * _P.dx) - 2.0 / (_P.dy * _P.dy) - 2.0 / (_P.dz * _P.dz) + dEL_en * _P.AA;

            return Q.Q4[i][j][k] - _P.kor * EL_en / dEL_en;
        }

        public virtual double Subsequent_value_Q5(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            double dEL_en;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            dEL_en = -_P.t / 6.0 - q1 - q2 - (tr_Q2 + 2.0 * q5 * q5) / 2.0;
            dEL_en = -2.0 / (_P.dx * _P.dx) - 2.0 / (_P.dy * _P.dy) - 2.0 / (_P.dz * _P.dz) + dEL_en * _P.AA;

            return Q.Q5[i][j][k] - _P.kor * EL_en / dEL_en;
        }

        #endregion

    }

    public class Time_evolution : Minimization
    {
        public Time_evolution(Q_tensor Q, Q_tensor Q_n, Parameters param) : base(Q, Q_n, param) { }

        public override double Next_value_Q1(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q1[im][j][k], Q.Q1[i][j][k], Q.Q1[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q1[i][jm][k], Q.Q1[i][j][k], Q.Q1[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q1[i][j][km], Q.Q1[i][j][k], Q.Q1[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0;
            EL_en += _P.delta_eps * (E[0] * E[0] + E[1] * E[1] - 2.0 * E[2] * E[2]) / 12.0 + _P.delta_mu * (B[0] * B[0] + B[1] * B[1] - 2.0 * B[2] * B[2]) / 12.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q1[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q1[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q2(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q2[im][j][k], Q.Q2[i][j][k], Q.Q2[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q2[i][jm][k], Q.Q2[i][j][k], Q.Q2[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q2[i][j][km], Q.Q2[i][j][k], Q.Q2[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0;
            EL_en += _P.delta_eps * (E[0] * E[0] - E[1] * E[1]) / 4.0 + _P.delta_mu * (B[0] * B[0] - B[1] * B[1]) / 4.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q2[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q2[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q3(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q3[im][j][k], Q.Q3[i][j][k], Q.Q3[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q3[i][jm][k], Q.Q3[i][j][k], Q.Q3[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q3[i][j][km], Q.Q3[i][j][k], Q.Q3[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + _P.delta_eps * E[0] * E[1] / 2.0 + _P.delta_mu * B[0] * B[1] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q3[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q3[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q4(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q4[im][j][k], Q.Q4[i][j][k], Q.Q4[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q4[i][jm][k], Q.Q4[i][j][k], Q.Q4[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q4[i][j][km], Q.Q4[i][j][k], Q.Q4[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + _P.delta_eps * E[0] * E[2] / 2.0 + _P.delta_mu * B[0] * B[2] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q4[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q4[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q5(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q5[im][j][k], Q.Q5[i][j][k], Q.Q5[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q5[i][jm][k], Q.Q5[i][j][k], Q.Q5[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q5[i][j][km], Q.Q5[i][j][k], Q.Q5[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0 + _P.delta_eps * E[1] * E[2] / 2.0 + _P.delta_mu * B[1] * B[2] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q5[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q5[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

    }

    public class Inequal_L : Minimization
    {
        public Inequal_L(Q_tensor Q, Q_tensor Q_n, Parameters param) : base(Q, Q_n, param) { }

        public override double Next_value_Q1(int i, int j, int k, Q_tensor Q)
        {
            #region Inicializacija spremenljivk

            double d_x, d_y, d_z, EL_en, dEL_en, tr_Q2, dq;
            double result = 0.0;

            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(Q.Q1[im][j][k], Q.Q1[i][j][k], Q.Q1[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q1[i][jm][k], Q.Q1[i][j][k], Q.Q1[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q1[i][j][km], Q.Q1[i][j][k], Q.Q1[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0;
            EL_en += _P.delta_eps * (E[0] * E[0] + E[1] * E[1] - 2.0 * E[2] * E[2]) / 12.0 + _P.delta_mu * (B[0] * B[0] + B[1] * B[1] - 2.0 * B[2] * B[2]) / 12.0;
            dEL_en = -_P.t / 6.0 - 2.0 * q1 - (tr_Q2 + 6.0 * q1 * q1) / 2.0;

            //f_e = ELelastic_term_Q1(i, j, k, Q1, Q2, Q3, Q4, Q5);
            //EL_en = f_e + EL_en * AA;
            
            EL_en = (d_x + d_y + d_z) / _P.AA + EL_en;
            dEL_en = (-2.0 / (_P.dx * _P.dx) - 2.0 / (_P.dy * _P.dy) - 2.0 / (_P.dz * _P.dz)) / _P.AA + dEL_en;

            result = Q.Q1[i][j][k] - _P.kor * EL_en / dEL_en;
            //if (Math.Abs(Q.Q1[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q2(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q2[im][j][k], Q.Q2[i][j][k], Q.Q2[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q2[i][jm][k], Q.Q2[i][j][k], Q.Q2[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q2[i][j][km], Q.Q2[i][j][k], Q.Q2[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0;
            EL_en += _P.delta_eps * (E[0] * E[0] - E[1] * E[1]) / 4.0 + _P.delta_mu * (B[0] * B[0] - B[1] * B[1]) / 4.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q2[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q2[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q3(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q3[im][j][k], Q.Q3[i][j][k], Q.Q3[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q3[i][jm][k], Q.Q3[i][j][k], Q.Q3[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q3[i][j][km], Q.Q3[i][j][k], Q.Q3[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + _P.delta_eps * E[0] * E[1] / 2.0 + _P.delta_mu * B[0] * B[1] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q3[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q3[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q4(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q4[im][j][k], Q.Q4[i][j][k], Q.Q4[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q4[i][jm][k], Q.Q4[i][j][k], Q.Q4[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q4[i][j][km], Q.Q4[i][j][k], Q.Q4[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + _P.delta_eps * E[0] * E[2] / 2.0 + _P.delta_mu * B[0] * B[2] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q4[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q4[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }
        public override double Next_value_Q5(int i, int j, int k, Q_tensor Q)
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

            d_x = Calculus.D2(Q.Q5[im][j][k], Q.Q5[i][j][k], Q.Q5[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q5[i][jm][k], Q.Q5[i][j][k], Q.Q5[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q5[i][j][km], Q.Q5[i][j][k], Q.Q5[i][j][kp], _P.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = -_P.t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0 + _P.delta_eps * E[1] * E[2] / 2.0 + _P.delta_mu * B[1] * B[2] / 2.0;
            dq = 2.0 * ((d_x + d_y + d_z) / _P.AA + EL_en) / _P.gamma;

            result = Q.Q5[i][j][k] + dq * _P.dt;
            //if (Math.Abs(Q.Q5[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

    }
}
