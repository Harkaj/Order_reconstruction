using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library
{
    class Inserts
    {
        int shape, type, c_x, c_y, c_z;
    }

    class Point : Inserts
    {

    }

    class Cube : Inserts
    {
        int a, b, c;
    }

    class Ellipsoid : Inserts
    {
        int a, b, c;
    }

    class Torus :  Inserts
    {
        int R_0, R_1;
    }
}
