using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    public class Analysis
    {
        public static void Periodic_conditions(int N, int i, out int im, out int ip)
        {
            im = i - 1;
            ip = i + 1;
            if (im < 0) { im = N - 1; }
            if (ip > N - 1) { ip = 0; }
        }

        public static void Periodic_conditions_v2(int N, int i, out int im, out int ip)
        {
            im = (i - 1 + N) % N;
            ip = (i + 1) % N;
        }

        /// <summary>
        /// Izračuna pol trace od Q*Q
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
        /// Izračuna trace od Q*Q
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <param name="q5"></param>
        /// <returns></returns>
        public static double TrQ_square(double q1, double q2, double q3, double q4, double q5)
        {
            double result = 6.0 * q1 * q1 + 2.0 * q2 * q2 + 2.0 * q3 * q3 + 2.0 * q4 * q4 + 2.0 * q5 * q5;
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
}
