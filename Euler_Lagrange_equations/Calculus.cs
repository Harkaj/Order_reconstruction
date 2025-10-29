using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    public class Calculus
    {
        /// <summary>
        /// Odvajanje po izbrani spremenljivki
        /// </summary>
        /// <param name="x_minus_1">Vrednost v legi i - 1</param>
        /// <param name="x_plus_1">Vrednost v legi i + 1</param>
        /// <param name="dx">Dolžina koraka</param>
        /// <returns>Prvi odvod</returns>
        public static double D(double x_minus_1, double x_plus_1, double dx)
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
        public static double D2(double x_minus_1, double x, double x_plus_1, double dx)
        {
            double result = (x_plus_1 - 2.0 * x + x_minus_1) / (dx * dx);
            return result;
        }

    }
}
