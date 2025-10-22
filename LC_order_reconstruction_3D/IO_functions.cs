using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LC_order_reconstruction_3D
{
    public static class IO_functions
    {
        public static void POVray_ini_script(string dir, int N_files)
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
                writer.WriteLine("Final_Frame={0}", N_files);
                writer.WriteLine();
                writer.WriteLine("Height=1024");
                writer.WriteLine("Width=1280");
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Script.pov"), false))
            {
                writer.WriteLine("#include concat(\"Script\", str(frame_number, -3, 0), \".pov\")");
            }
        }

        public static void POVray_ini_script(string dir, int N_files, int N_r)
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
                writer.WriteLine("Final_Frame={0}", N_files);
                writer.WriteLine();
                writer.WriteLine("Height=1024");
                writer.WriteLine("Width=1280");
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(dir, "Script" + N_r.ToString() + "_.pov"), false))
            {
                writer.WriteLine("#include concat(\"Script{0}_\", str(frame_number, -3, 0), \".pov\")", N_r.ToString());
            }
        }

        public static void POVray_environment(StreamWriter writer)
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

        public static void POVray_environment(StreamWriter writer, int zoom1, int zoom2)
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

        /// <summary>
        /// Writes the script for drawing the director field
        /// </summary>
        /// <param name="datoteka">Input file</param>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="factor">Display each factor point</param>
        /// <param name="N_r">Plane number</param>
        /// <param name="plane_n">Plane (0-yz, 1-xz. 2-xy)</param>
        public static void POVray_director_field(string[] datoteka, StreamWriter writer, int factor, int N_r, int plane_n)
        {
            string[] data;
            string[] separators = { "\t", " " };
            int ii, jj, plane, axis1, axis2, temp;
            double[] n_img = new double[3];

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                #region Setting up display pane

                plane = int.Parse(data[plane_n]);
                if (plane != N_r) { continue; }

                axis1 = plane_n + 1;
                if (axis1 > 2) { axis1 = 0; }
                axis2 = plane_n - 1;
                if (axis2 < 0) { axis2 = 2; }
                if (axis2 < axis1)
                {
                    temp = axis1;
                    axis1 = axis2;
                    axis2 = temp;
                }

                #endregion

                ii = int.Parse(data[axis1]);
                jj = int.Parse(data[axis2]);

                if (ii % factor == 0 && jj % factor == 0)
                {
                    n_img[0] = double.Parse(data[axis1 + 3]);
                    n_img[1] = double.Parse(data[axis2 + 3]);
                    n_img[2] = double.Parse(data[plane_n + 3]);

                    writer.WriteLine("object{");
                    writer.WriteLine("  Round_Cylinder");
                    writer.WriteLine("   (<{0},{1},{2}>,<{3},{4},{5}>, 0.8, 0.1, 1)", -n_img[0] * 2.0, -n_img[1] * 2.0, -n_img[2] * 2.0, n_img[0] * 2.0, n_img[1] * 2.0, n_img[2] * 2.0);
                    writer.WriteLine("    texture { pigment { color Green }");
                    writer.WriteLine("    finish { reflection 0.05 phong 1 }");
                    writer.WriteLine("  }");
                    writer.WriteLine("  translate<{0},{1},0>", ii, jj);
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Writes the script for drawing the director field
        /// </summary>
        /// <param name="datoteka">Input file</param>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="zoom1">Zoomed in location in first direction</param>
        /// <param name="zoom2">Zoomed in location in first direction</param>
        /// <param name="N_r">Plane number</param>
        /// <param name="plane_n">Plane (0-yz, 1-xz. 2-xy)</param>
        public static void POVray_director_field(string[] datoteka, StreamWriter writer, int zoom1, int zoom2, int N_r, int plane_n)
        {
            string[] data;
            string[] separators = { "\t", " " };
            int ii, jj, plane, axis1, axis2, temp;
            double[] n_img = new double[3];

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                #region Setting up display pane

                plane = int.Parse(data[plane_n]);
                if (plane != N_r) { continue; }

                axis1 = plane_n + 1;
                if (axis1 > 2) { axis1 = 0; }
                axis2 = plane_n - 1;
                if (axis2 < 0) { axis2 = 2; }
                if (axis2 < axis1)
                {
                    temp = axis1;
                    axis1 = axis2;
                    axis2 = temp;
                }

                #endregion

                ii = int.Parse(data[axis1]);
                jj = int.Parse(data[axis2]);

                if (ii > zoom1 && ii < zoom1 + 30 && jj > zoom2 && jj < zoom2 + 30)
                {
                    n_img[0] = double.Parse(data[axis1 + 3]);
                    n_img[1] = double.Parse(data[axis2 + 3]);
                    n_img[2] = double.Parse(data[plane_n + 3]);

                    writer.WriteLine("object{");
                    writer.WriteLine("  Round_Cylinder");
                    writer.WriteLine("   (<{0},{1},{2}>,<{3},{4},{5}>, 0.2, 0.1, 1)", -n_img[0] / 2.0, -n_img[1] / 2.0, -n_img[2] / 2.0, n_img[0] / 2.0, n_img[1] / 2.0, n_img[2] / 2.0);
                    writer.WriteLine("    texture { pigment { color Green }");
                    writer.WriteLine("    finish { reflection 0.05 phong 1 }");
                    writer.WriteLine("  }");
                    writer.WriteLine("  translate<{0},{1},0>", ii, jj);
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }
        }
    }
}
