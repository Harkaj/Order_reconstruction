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

        #region Single full system iterations

        public void Iteration_Q1(Q_tensor Q, Q_tensor Q_n)
        {
            for (int i = 0; i < Q.Nx; i++)
            {
                for (int j = 0; j < Q.Ny; j++)
                {
                    for (int k = 0; k < Q.Nz; k++)
                    {
                        if (Q.Q_type[i][j][k] != Q_tensor.LC && Q.Q_type[i][j][k] != Q_tensor.B_PERIODIC) { continue; }
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
                        if (Q.Q_type[i][j][k] != Q_tensor.LC && Q.Q_type[i][j][k] != Q_tensor.B_PERIODIC) { continue; }
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
                        if (Q.Q_type[i][j][k] != Q_tensor.LC && Q.Q_type[i][j][k] != Q_tensor.B_PERIODIC) { continue; }
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
                        if (Q.Q_type[i][j][k] != Q_tensor.LC && Q.Q_type[i][j][k] != Q_tensor.B_PERIODIC) { continue; }
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
                        if (Q.Q_type[i][j][k] != Q_tensor.LC && Q.Q_type[i][j][k] != Q_tensor.B_PERIODIC) { continue; }
                        Q_n.Q5[i][j][k] = Next_value_Q5(i, j, k, Q);
                    }
                }
            }
        }

        #endregion

        #region Next value calculation

        public virtual double Next_value_Q1(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q1(i, j, k, Q);
            f_e = Elastic_term_Q1(i, j, k, Q);
            f_f = Field_term_Q1(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q1(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q1[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }
        public virtual double Next_value_Q2(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q2(i, j, k, Q);
            f_e = Elastic_term_Q2(i, j, k, Q);
            f_f = Field_term_Q2(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q2(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q2[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }
        public virtual double Next_value_Q3(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q3(i, j, k, Q);
            f_e = Elastic_term_Q3(i, j, k, Q);
            f_f = Field_term_Q3(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q3(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q3[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }
        public virtual double Next_value_Q4(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q4(i, j, k, Q);
            f_e = Elastic_term_Q4(i, j, k, Q);
            f_f = Field_term_Q4(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q4(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q4[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }
        public virtual double Next_value_Q5(int i, int j, int k, Q_tensor Q)
        {
            double EL_en, f_c, f_e, f_f, result;
            
            f_c = Condensation_term_Q5(i, j, k, Q);
            f_e = Elastic_term_Q5(i, j, k, Q);
            f_f = Field_term_Q5(i, j, k, Q);
            EL_en = f_c * _P.AA + f_e + f_f * _P.AA;

            result = Subsequent_value_Q5(i, j, k, Q, EL_en);
            if (Math.Abs(Q.Q5[i][j][k] - result) > _P.eps) { _P.nap++; }

            return result;
        }

        #endregion

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

        #region Elastic terms (Chiral terms to be added)

        public virtual double Elastic_term_Q1(int i, int j, int k, Q_tensor Q)
        {
            double d_x, d_y, d_z;// q4dy, q5dx;

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            d_x = Calculus.D2(Q.Q1[im][j][k], Q.Q1[i][j][k], Q.Q1[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q1[i][jm][k], Q.Q1[i][j][k], Q.Q1[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q1[i][j][km], Q.Q1[i][j][k], Q.Q1[i][j][kp], _P.dz);

            //q4dy = Calculus.D(Q.Q4[i][jm][k], Q.Q4[i][jp][k], _P.dy);
            //q5dx = Calculus.D(Q.Q5[im][j][k], Q.Q5[ip][j][k], _P.dx);

            return d_x + d_y + d_z;// + _P.L_chiral * (-q4dy + q5dx) / 4.0;
        }
        public virtual double Elastic_term_Q2(int i, int j, int k, Q_tensor Q)
        {
            double d_x, d_y, d_z;

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            d_x = Calculus.D2(Q.Q2[im][j][k], Q.Q2[i][j][k], Q.Q2[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q2[i][jm][k], Q.Q2[i][j][k], Q.Q2[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q2[i][j][km], Q.Q2[i][j][k], Q.Q2[i][j][kp], _P.dz);

            return d_x + d_y + d_z;
        }
        public virtual double Elastic_term_Q3(int i, int j, int k, Q_tensor Q)
        {
            double d_x, d_y, d_z;

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            d_x = Calculus.D2(Q.Q3[im][j][k], Q.Q3[i][j][k], Q.Q3[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q3[i][jm][k], Q.Q3[i][j][k], Q.Q3[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q3[i][j][km], Q.Q3[i][j][k], Q.Q3[i][j][kp], _P.dz);

            return d_x + d_y + d_z;
        }
        public virtual double Elastic_term_Q4(int i, int j, int k, Q_tensor Q)
        {
            double d_x, d_y, d_z;

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            d_x = Calculus.D2(Q.Q4[im][j][k], Q.Q4[i][j][k], Q.Q4[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q4[i][jm][k], Q.Q4[i][j][k], Q.Q4[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q4[i][j][km], Q.Q4[i][j][k], Q.Q4[i][j][kp], _P.dz);

            return d_x + d_y + d_z;
        }
        public virtual double Elastic_term_Q5(int i, int j, int k, Q_tensor Q)
        {
            double d_x, d_y, d_z;

            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            d_x = Calculus.D2(Q.Q5[im][j][k], Q.Q5[i][j][k], Q.Q5[ip][j][k], _P.dx);
            d_y = Calculus.D2(Q.Q5[i][jm][k], Q.Q5[i][j][k], Q.Q5[i][jp][k], _P.dy);
            d_z = Calculus.D2(Q.Q5[i][j][km], Q.Q5[i][j][k], Q.Q5[i][j][kp], _P.dz);

            return d_x + d_y + d_z;
        }

        #endregion

        #region Field terms

        public double Field_term_Q1(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * (E[0] * E[0] + E[1] * E[1] - 2.0 * E[2] * E[2]) / 12.0 + _P.delta_mu * (B[0] * B[0] + B[1] * B[1] - 2.0 * B[2] * B[2]) / 12.0;
        }

        public double Field_term_Q2(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * (E[0] * E[0] - E[1] * E[1]) / 4.0 + _P.delta_mu * (B[0] * B[0] - B[1] * B[1]) / 4.0;
        }

        public double Field_term_Q3(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * E[0] * E[1] / 2.0 + _P.delta_mu * B[0] * B[1] / 2.0;
        }

        public double Field_term_Q4(int i, int j, int k, Q_tensor Q)
        {
            Q.E_B_values(i, j, k, out double[] E, out double[] B);
            return _P.delta_eps * E[0] * E[2] / 2.0 + _P.delta_mu * B[0] * B[2] / 2.0;
        }

        public double Field_term_Q5(int i, int j, int k, Q_tensor Q)
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
        public Time_evolution(Q_tensor Q, Q_tensor Q_n, Parameters P) : base(Q, Q_n, P) { }

        public override double Subsequent_value_Q1(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            return Q.Q1[i][j][k] + _P.dt * 2.0 * EL_en / _P.gamma;
        }
        public override double Subsequent_value_Q2(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            return Q.Q2[i][j][k] + _P.dt * 2.0 * EL_en / _P.gamma;
        }
        public override double Subsequent_value_Q3(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            return Q.Q3[i][j][k] + _P.dt * 2.0 * EL_en / _P.gamma;
        }
        public override double Subsequent_value_Q4(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            return Q.Q4[i][j][k] + _P.dt * 2.0 * EL_en / _P.gamma;
        }
        public override double Subsequent_value_Q5(int i, int j, int k, Q_tensor Q, double EL_en)
        {
            return Q.Q5[i][j][k] + _P.dt * 2.0 * EL_en / _P.gamma;
        }

    }

    public class Inequal_L : Minimization
    {
        public Inequal_L(Q_tensor Q, Q_tensor Q_n, Parameters P) : base(Q, Q_n, P) { }

        public override double Elastic_term_Q1(int i, int j, int k, Q_tensor Q)
        {
            Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            double l1, l2, l3_1, l3_2;
            double q1dx, q1dy, q1dz, q2dx, q2dy, q3dx, q3dy, q4dx, q4dz, q5dy, q5dz;
            double q1d2x, q1d2y, q1d2z, q2d2x, q2d2y, qd1, qd2, q1dxy, q1dxz, q1dyz, q3dxy, q4dxz, q5dyz;
            
            Analysis.Periodic_conditions(Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(Q.Nz, k, out int km, out int kp);

            #region First order derivatives

            q1dx = Calculus.D(Q.Q1[im][j][k], Q.Q1[ip][j][k], _P.dx);
            q1dy = Calculus.D(Q.Q1[i][jm][k], Q.Q1[i][jp][k], _P.dy);
            q1dz = Calculus.D(Q.Q1[i][j][km], Q.Q1[i][j][kp], _P.dz);

            q2dx = Calculus.D(Q.Q2[im][j][k], Q.Q2[ip][j][k], _P.dx);
            q2dy = Calculus.D(Q.Q2[i][jm][k], Q.Q2[i][jp][k], _P.dy);

            q3dx = Calculus.D(Q.Q3[im][j][k], Q.Q3[ip][j][k], _P.dx);
            q3dy = Calculus.D(Q.Q3[i][jm][k], Q.Q3[i][jp][k], _P.dy);

            q4dx = Calculus.D(Q.Q4[im][j][k], Q.Q4[ip][j][k], _P.dx);
            q4dz = Calculus.D(Q.Q4[i][jm][k], Q.Q4[i][jp][k], _P.dz);

            q5dy = Calculus.D(Q.Q5[im][j][k], Q.Q5[ip][j][k], _P.dy);
            q5dz = Calculus.D(Q.Q5[i][jm][k], Q.Q5[i][jp][k], _P.dz);

            #endregion

            #region Second order derivatives

            q1d2x = Calculus.D2(Q.Q1[im][j][k], Q.Q1[i][j][k], Q.Q1[ip][j][k], _P.dx);
            q1d2y = Calculus.D2(Q.Q1[i][jm][k], Q.Q1[i][j][k], Q.Q1[i][jp][k], _P.dy);
            q1d2z = Calculus.D2(Q.Q1[i][j][km], Q.Q1[i][j][k], Q.Q1[i][j][kp], _P.dz);

            q2d2x = Calculus.D2(Q.Q2[im][j][k], Q.Q2[i][j][k], Q.Q2[ip][j][k], _P.dx);
            q2d2y = Calculus.D2(Q.Q2[i][jm][k], Q.Q2[i][j][k], Q.Q2[i][jp][k], _P.dy);

            #endregion

            #region Mixed second order derivatives

            qd1 = Calculus.D(Q.Q1[im][jm][k], Q.Q1[ip][jm][k], _P.dx);
            qd2 = Calculus.D(Q.Q1[im][jp][k], Q.Q1[ip][jp][k], _P.dx);
            q1dxy = Calculus.D(qd1, qd2, _P.dy);

            qd1 = Calculus.D(Q.Q1[im][j][km], Q.Q1[ip][j][km], _P.dx);
            qd2 = Calculus.D(Q.Q1[im][j][kp], Q.Q1[ip][j][kp], _P.dx);
            q1dxz = Calculus.D(qd1, qd2, _P.dz);

            qd1 = Calculus.D(Q.Q1[i][jm][km], Q.Q1[i][jp][km], _P.dy);
            qd2 = Calculus.D(Q.Q1[i][jm][kp], Q.Q1[i][jp][kp], _P.dy);
            q1dyz = Calculus.D(qd1, qd2, _P.dz);

            qd1 = Calculus.D(Q.Q3[im][jm][k], Q.Q3[ip][jm][k], _P.dx);
            qd2 = Calculus.D(Q.Q3[im][jp][k], Q.Q3[ip][jp][k], _P.dx);
            q3dxy = Calculus.D(qd1, qd2, _P.dy);

            qd1 = Calculus.D(Q.Q4[im][j][km], Q.Q4[ip][j][km], _P.dx);
            qd2 = Calculus.D(Q.Q4[im][j][kp], Q.Q4[ip][j][kp], _P.dx);
            q4dxz = Calculus.D(qd1, qd2, _P.dz);

            qd1 = Calculus.D(Q.Q5[i][jm][km], Q.Q5[i][jp][km], _P.dy);
            qd2 = Calculus.D(Q.Q5[i][jm][kp], Q.Q5[i][jp][kp], _P.dy);
            q5dyz = Calculus.D(qd1, qd2, _P.dz);

            #endregion

            l1 = q1d2x + q1d2y + q1d2z;
            l2 = q1d2x + q1d2y + 4.0 * q1d2z + q2d2x - q2d2y + 2.0 * q3dxy - q4dxz - q5dyz;
            l3_1 = 2.0 * q1dz * q1dz - q5dz * q1dy - q1dy * q1dy + q1dy * q2dy - 2.0 * q5 * q1dz + q2 * q1d2y - q4dz * q1dx - q3dy * q1dx - q1dx * q1dx;
            l3_2 = -q1dx * q2dx - q1dy * q3dx - q1dz * (q5dy + q4dx) - 2.0 * q4 * q1dxz - 2.0 * q3 * q1dxy + q1 * (2.0 * q1d2z - q1d2y - q1d2x) - q2 * q1d2x;
            //l3_1 = q1 * (q1d2x + q1d2y - 4.0 * q1d2z) + q2 * (q1d2x + q1d2y) + 2.0 * q3 * q1dxy + 2.0 * q4 * q1dxz + 2.0 * q5 * q1dyz;
            //  l3_2 = q1dx * q1dx + q1dy * q1dy - 2.0 * q1dz * q1dz + q1dx * q2dx - q1dy * q2dy + q1dx * q3dy + q1dy * q3dx + q1dx * q4dz + q1dz * q4dx + q1dy * q5dz + q1dz * q5dy;

            return _P.L1 * l1 + _P.L2 * l2 / 6.0 + _P.L3 * (l3_1 + l3_2);
        }
        public override double Elastic_term_Q2(int i, int j, int k, Q_tensor Q)
        {
            return base.Elastic_term_Q2(i, j, k, Q);
        }
        public override double Elastic_term_Q3(int i, int j, int k, Q_tensor Q)
        {
            return base.Elastic_term_Q3(i, j, k, Q);
        }
        public override double Elastic_term_Q4(int i, int j, int k, Q_tensor Q)
        {
            return base.Elastic_term_Q4(i, j, k, Q);
        }
        public override double Elastic_term_Q5(int i, int j, int k, Q_tensor Q)
        {
            return base.Elastic_term_Q5(i, j, k, Q);
        }
    }
}
