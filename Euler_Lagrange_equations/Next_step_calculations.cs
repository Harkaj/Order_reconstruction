using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    interface INext_step_calculations
    {
        double Next_value_Q1(int i, int j, int k);
        double Next_value_Q2(int i, int j, int k);
        double Next_value_Q3(int i, int j, int k);
        double Next_value_Q4(int i, int j, int k);
        double Next_value_Q5(int i, int j, int k);
    }

    public class Minimization : INext_step_calculations
    {
        private Q_tensor _Q;
        private Parameters _param;

        public Minimization(Q_tensor Q, Parameters param)
        {
            this._Q = Q;
            this._param = param;
        }

        
        public double Next_value_Q1(int i, int j, int k)
        {
            #region Inicializacija spremenljivk
            
            double EL_en, dEL_en, tr_Q2;
            double d_x, d_y, d_z;
            double result = 0.0;

            _Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            _Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(_Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(_Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(_Q.Nz, k, out int km, out int kp);
            
            #endregion

            #region Izračun

            d_x = Calculus.D2(_Q.Q1[im][j][k], _Q.Q1[i][j][k], _Q.Q1[ip][j][k], _param.dx);
            d_y = Calculus.D2(_Q.Q1[i][jm][k], _Q.Q1[i][j][k], _Q.Q1[i][jp][k], _param.dy);
            d_z = Calculus.D2(_Q.Q1[i][j][km], _Q.Q1[i][j][k], _Q.Q1[i][j][kp], _param.dz);
            
            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q1 / 6.0 - (6.0 * q1 * q1 - 2.0 * q2 * q2 - 2.0 * q3 * q3 + q4 * q4 + q5 * q5) / 6.0 - (q1 * tr_Q2) / 2.0 + _param.delta_eps * (E[0] * E[0] + E[1] * E[1] - 2.0 * E[2] * E[2]) / 2.0;
            dEL_en = -_param.t / 6.0 - 2.0 * q1 - (tr_Q2 + 6.0 * q1 * q1) / 2.0;

            EL_en = (d_x + d_y + d_z) / _param.AA + EL_en;
            dEL_en = (-2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz)) / _param.AA + dEL_en;

            result = _Q.Q1[i][j][k] - _param.kor * EL_en / dEL_en;

            //if (Math.Abs(_Q.Q1[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q2(int i, int j, int k)
        {
            #region Inicializacija spremenljivk

            double EL_en, dEL_en, tr_Q2;
            double d_x, d_y, d_z;
            double result = 0.0;

            _Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            _Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(_Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(_Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(_Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(_Q.Q2[im][j][k], _Q.Q2[i][j][k], _Q.Q2[ip][j][k], _param.dx);
            d_y = Calculus.D2(_Q.Q2[i][jm][k], _Q.Q2[i][j][k], _Q.Q2[i][jp][k], _param.dy);
            d_z = Calculus.D2(_Q.Q2[i][j][km], _Q.Q2[i][j][k], _Q.Q2[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q2 / 6.0 + (4.0 * q1 * q2 + q4 * q4 - q5 * q5) / 2.0 - (q2 * tr_Q2) / 2.0;
            EL_en += _param.delta_eps * (E[0] * E[0] - E[1] * E[1]) / 4.0 + _param.delta_mu * (B[0] * B[0] - B[1] * B[1]) / 4.0;
            dEL_en = -_param.t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q2 * q2) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = _Q.Q2[i][j][k] - _param.kor * EL_en / dEL_en;

            //if (Math.Abs(_Q.Q2[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q3(int i, int j, int k)
        {
            #region Inicializacija spremenljivk

            double EL_en, dEL_en, tr_Q2;
            double d_x, d_y, d_z;
            double result = 0.0;

            _Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            _Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(_Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(_Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(_Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(_Q.Q3[im][j][k], _Q.Q3[i][j][k], _Q.Q3[ip][j][k], _param.dx);
            d_y = Calculus.D2(_Q.Q3[i][jm][k], _Q.Q3[i][j][k], _Q.Q3[i][jp][k], _param.dy);
            d_z = Calculus.D2(_Q.Q3[i][j][km], _Q.Q3[i][j][k], _Q.Q3[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q3 / 6.0 + 2.0 * q1 * q3 + q4 * q5 - (q3 * tr_Q2) / 2.0 + _param.delta_eps * E[0] * E[1] / 2.0 + _param.delta_mu * B[0] * B[1] / 2.0;
            dEL_en = -_param.t / 6.0 + 2.0 * q1 - (tr_Q2 + 2.0 * q3 * q3) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = _Q.Q3[i][j][k] - _param.kor * EL_en / dEL_en;

            //if (Math.Abs(_Q.Q3[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q4(int i, int j, int k)
        {
            #region Inicializacija spremenljivk

            double EL_en, dEL_en, tr_Q2;
            double d_x, d_y, d_z;
            double result = 0.0;

            _Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            _Q.E_B_values(i, j, k, out double[] E, out double[] B);
            
            Analysis.Periodic_conditions(_Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(_Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(_Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(_Q.Q4[im][j][k], _Q.Q4[i][j][k], _Q.Q4[ip][j][k], _param.dx);
            d_y = Calculus.D2(_Q.Q4[i][jm][k], _Q.Q4[i][j][k], _Q.Q4[i][jp][k], _param.dy);
            d_z = Calculus.D2(_Q.Q4[i][j][km], _Q.Q4[i][j][k], _Q.Q4[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q4 / 6.0 - q1 * q4 + q2 * q4 + q3 * q5 - (q4 * tr_Q2) / 2.0 + _param.delta_eps * E[0] * E[2] / 2.0 + _param.delta_mu * B[0] * B[2] / 2.0;
            dEL_en = -_param.t / 6.0 - q1 + q2 - (tr_Q2 + 2.0 * q4 * q4) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = _Q.Q4[i][j][k] - _param.kor * EL_en / dEL_en;

            //if (Math.Abs(_Q.Q4[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

        public double Next_value_Q5(int i, int j, int k)
        {
            #region Inicializacija spremenljivk

            double EL_en, dEL_en, tr_Q2;
            double d_x, d_y, d_z;
            double result = 0.0;

            _Q.Q_values(i, j, k, out double q1, out double q2, out double q3, out double q4, out double q5);
            _Q.E_B_values(i, j, k, out double[] E, out double[] B);

            Analysis.Periodic_conditions(_Q.Nx, i, out int im, out int ip);
            Analysis.Periodic_conditions(_Q.Ny, j, out int jm, out int jp);
            Analysis.Periodic_conditions(_Q.Nz, k, out int km, out int kp);

            #endregion

            #region Izračun

            d_x = Calculus.D2(_Q.Q5[im][j][k], _Q.Q5[i][j][k], _Q.Q5[ip][j][k], _param.dx);
            d_y = Calculus.D2(_Q.Q5[i][jm][k], _Q.Q5[i][j][k], _Q.Q5[i][jp][k], _param.dy);
            d_z = Calculus.D2(_Q.Q5[i][j][km], _Q.Q5[i][j][k], _Q.Q5[i][j][kp], _param.dz);

            tr_Q2 = Analysis.Half_trQ_square(q1, q2, q3, q4, q5);

            EL_en = 0.0;
            dEL_en = 0.0;

            EL_en = -_param.t * q5 / 6.0 - q1 * q5 - q2 * q5 + q3 * q4 - (q5 * tr_Q2) / 2.0 + _param.delta_eps * E[1] * E[2] / 2.0 + _param.delta_mu * B[1] * B[2] / 2.0;
            dEL_en = -_param.t / 6.0 - q1 - q2 - (tr_Q2 + 2.0 * q5 * q5) / 2.0;

            EL_en = d_x + d_y + d_z + EL_en * _param.AA;
            dEL_en = -2.0 / (_param.dx * _param.dx) - 2.0 / (_param.dy * _param.dy) - 2.0 / (_param.dz * _param.dz) + dEL_en * _param.AA;

            result = _Q.Q5[i][j][k] - _param.kor * EL_en / dEL_en;

            //if (Math.Abs(_Q.Q5[i][j][k] - result) > _param.eps) { nap++; }

            #endregion

            return result;
        }

    }

    public class Time_evolution : INext_step_calculations
    {
        private Q_tensor _Q;
        private Parameters _param;

        public Time_evolution(Q_tensor Q, Parameters param)
        {
            this._Q = Q;
            this._param = param;
        }

        public double Next_value_Q1(int i, int j, int k)
        {
            return 0.0;
        }
        public double Next_value_Q2(int i, int j, int k)
        {
            return 0.0;
        }
        public double Next_value_Q3(int i, int j, int k)
        {
            return 0.0;
        }
        public double Next_value_Q4(int i, int j, int k)
        {
            return 0.0;
        }
        public double Next_value_Q5(int i, int j, int k)
        {
            return 0.0;
        }
    }
}
