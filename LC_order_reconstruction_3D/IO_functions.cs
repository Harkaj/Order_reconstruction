using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LC_order_reconstruction_3D
{
    class IO_functions
    {
        void POVray_ini_script(string dir, int N_file)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Script.ini"), false))
            {
                writer.WriteLine("Input_File_Name=Script.pov");
                writer.WriteLine();
                writer.WriteLine("; these are the default values");
                writer.WriteLine("Initial_Clock=0.000");
                writer.WriteLine("Final_CLock=1.000");
                writer.WriteLine("Antialias=On");
                writer.WriteLine("Antialias_Threshold=0.05");
                writer.WriteLine();
                writer.WriteLine("Initial_Frame=0");
                writer.WriteLine("Final_Frame={0}", N_file);
                writer.WriteLine();
                writer.WriteLine("Height=1024");
                writer.WriteLine("Width=1280");
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Script.pov"), false))
            {
                writer.WriteLine("#include concat(\"Script\", str(frame_number, -3, 0), \".pov\")");
            }
        }

        void POVray_ini_script(string dir, int N_r, int N_file)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Script.ini"), false))
            {
                writer.WriteLine("Input_File_Name=Script{0}_.pov", N_r.ToString());
                writer.WriteLine();
                writer.WriteLine("; these are the default values");
                writer.WriteLine("Initial_Clock=0.000");
                writer.WriteLine("Final_CLock=1.000");
                writer.WriteLine("Antialias=On");
                writer.WriteLine("Antialias_Threshold=0.05");
                writer.WriteLine();
                writer.WriteLine("Initial_Frame=0");
                writer.WriteLine("Final_Frame={0}", N_file);
                writer.WriteLine();
                writer.WriteLine("Height=1024");
                writer.WriteLine("Width=1280");
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Script" + N_r.ToString() + "_.pov"), false))
            {
                writer.WriteLine("#include concat(\"Script{0}_\", str(frame_number, -3, 0), \".pov\")", N_r.ToString());
            }
        }

        void POVray_environment(StreamWriter writer)
        {
            writer.WriteLine("#include \"colors.inc\"");
            writer.WriteLine("#include \"textures.inc\"");
            writer.WriteLine("#include \"shapes.inc\"");
            writer.WriteLine();

            writer.WriteLine("background { color White }");
            writer.WriteLine();

            writer.WriteLine("camera { orthographic");
            writer.WriteLine("  location <50, 50, -120>");
            writer.WriteLine("  look_at  <50, 50, 0>");
            writer.WriteLine("}");
            writer.WriteLine();

            writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
            writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
            writer.WriteLine("               adaptive 1 jitter }");
            writer.WriteLine();
        }

        void POVray_environment(StreamWriter writer, int zoom1, int zoom2)
        {
            writer.WriteLine("#include \"colors.inc\"");
            writer.WriteLine("#include \"textures.inc\"");
            writer.WriteLine("#include \"shapes.inc\"");
            writer.WriteLine();

            writer.WriteLine("background { color White }");
            writer.WriteLine();

            writer.WriteLine("camera { orthographic");
            writer.WriteLine("  location <{0}, {1}, -35>", zoom1 + 15, zoom2 + 15);
            writer.WriteLine("  look_at  <{0}, {1}, 0>", zoom1 + 15, zoom2 + 15);
            writer.WriteLine("}");
            writer.WriteLine();

            writer.WriteLine("light_source { <50, 50, -50> color White shadowless");
            writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 5");
            writer.WriteLine("               adaptive 1 jitter }");
            writer.WriteLine();
        }

        void POVray_director_field(string[] datoteka, StreamWriter writer, int zoom1, int zoom2, int N_r, int plane_n)
        {
            string[] data;
            string[] separators = { "\t", " " };
            int ii, jj, kk, plane;
            double n_i, n_j, n_k, angle1, angle2;

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                plane = int.Parse(data[plane_n]);
                if (plane != N_r) { continue; }

                ii = int.Parse(data[0]);
                jj = int.Parse(data[1]);
                kk = int.Parse(data[2]);

                if (ii > zoom1 && ii < zoom1 + 30 && kk > zoom2 && kk < zoom2 + 30)
                {
                    n_i = double.Parse(data[3]);
                    n_j = double.Parse(data[4]);
                    n_k = double.Parse(data[5]);

                    angle1 = (180.0 * Math.Atan2(n_j, n_i)) / Math.PI;
                    angle2 = (180.0 * Math.Acos(n_k)) / Math.PI;
                    if (angle2 < 0.0 && angle1 > 90.0)
                    {
                        angle2 += 180.0;
                    }

                    writer.WriteLine("object{");
                    writer.WriteLine("  Round_Cylinder");
                    writer.WriteLine("   (<0,-0.5,0>,<0,0.5,0>, 0.2, 0.1, 1)");
                    writer.WriteLine("    texture { pigment { color Green }");
                    writer.WriteLine("    finish { reflection 0.05 phong 1 }");
                    writer.WriteLine("  }");
                    writer.WriteLine("  rotate<0,0,{0}>", (int)angle2);
                    writer.WriteLine("  rotate<0,{0},0>", (int)angle1);
                    writer.WriteLine("  translate<{0},{1},0>", ii, kk);
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }
        }
    }
}
