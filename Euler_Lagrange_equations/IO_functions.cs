using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Class_library
{
    public static class IO_functions
    {
        /// <summary>
        /// Creates .pov and .ini files for creating multiple POVray scripts
        /// </summary>
        /// <param name="dir">Location for the files</param>
        /// <param name="N_files">Number of files</param>
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

        /// <summary>
        /// Creates .pov and .ini files for creating multiple POVray scripts
        /// </summary>
        /// <param name="dir">Location for the files</param>
        /// <param name="N_files">Number of files</param>
        /// <param name="N_r">Number of the plane</param>
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

        /// <summary>
        /// Writes the POVray script for the 3D view environment setup
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        public static void POVray_environment_3D(StreamWriter writer)
        {
            writer.WriteLine("#include \"colors.inc\"");
            writer.WriteLine("#include \"textures.inc\"");
            writer.WriteLine("#include \"shapes.inc\"");
            writer.WriteLine();

            writer.WriteLine("background { color White }");
            writer.WriteLine();

            writer.WriteLine("camera {");
            writer.WriteLine("  location <130, 130, -80>");
            writer.WriteLine("  look_at  <50, 30, 50>");
            writer.WriteLine("}");
            writer.WriteLine();

            writer.WriteLine("light_source { <0, 0, -50> color White shadowless");
            writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 2");
            writer.WriteLine("               adaptive 1 jitter }");
            writer.WriteLine();

            writer.WriteLine("Wire_Box(<0,0,0>,<100,100,100>, 0.05, 0)");
            writer.WriteLine();
        }

        /// <summary>
        /// Writes the POVray script for the 3D view environment setup
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="Nx">System size in x</param>
        /// <param name="Ny">System size in y</param>
        /// <param name="Nz">System size in z</param>
        public static void POVray_environment_3D(StreamWriter writer, int Nx, int Ny, int Nz)
        {
            writer.WriteLine("#include \"colors.inc\"");
            writer.WriteLine("#include \"textures.inc\"");
            writer.WriteLine("#include \"shapes.inc\"");
            writer.WriteLine();

            writer.WriteLine("background { color White }");
            writer.WriteLine();

            if (Nx == 200)
            {
                writer.WriteLine("camera {");
                writer.WriteLine("  location <150, 150, -150>");
                writer.WriteLine("  look_at  <80, 0, 120>");
                writer.WriteLine("}");
                writer.WriteLine();
            }
            else
            {
                writer.WriteLine("camera {");
                writer.WriteLine("  location <130, 150, -80>");
                writer.WriteLine("  look_at  <50, 30, 50>");
                writer.WriteLine("}");
                writer.WriteLine();
            }

            writer.WriteLine("light_source { <0, 0, -50> color White shadowless");
            writer.WriteLine("               area_light <100, 0, 0>, <0, 100, 0>, 5, 2");
            writer.WriteLine("               adaptive 1 jitter }");
            writer.WriteLine();

            writer.WriteLine("Wire_Box(<-3,0,-3>,<{0},{2},{1}>, 0.05, 0)", Nx + 6, Ny + 3, Nz);
            writer.WriteLine();
        }

        /// <summary>
        /// Writes the POVray script for the environment setup
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        public static void POVray_environment_orthographic(StreamWriter writer)
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

        /// <summary>
        /// Writes the POVray script for the environment setup
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="zoom1">Zoomed in location in first direction</param>
        /// <param name="zoom2">Zoomed in location in second direction</param>
        public static void POVray_environment_orthographic(StreamWriter writer, int zoom1, int zoom2)
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
        /// Writes the POVray script for drawing locations of surface defects
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        public static void POVray_surface_defects(StreamWriter writer)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        writer.WriteLine("  sphere { <0,0,0>, 1.5 scale <1,0.2,1> ");
                    }
                    else
                    {
                        writer.WriteLine("  torus { 1.0, 0.5 scale <1,0.2,1> ");
                    }

                    writer.WriteLine("          texture { pigment{ color rgb <1,0,0>}");
                    writer.WriteLine("                    finish { reflection 0.05 phong 0.1}");
                    writer.WriteLine("                  }");
                    writer.WriteLine("          rotate <90,0,0>");
                    writer.WriteLine("          translate <{0},{1},-2>", (i + 1) * 40, (j + 1) * 40);
                    writer.WriteLine("        }");
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Writes the POVray script for drawing points with high beta on a 2D plane
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="datoteka">Input file</param>
        public static void POVray_beta_2D(StreamWriter writer, string[] datoteka)
        {
            string[] data;
            string[] separators = { "\t", " " };
            int x_i, y_j, z_k;

            writer.WriteLine("blob {");
            writer.WriteLine("  threshold 0.99");

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                x_i = int.Parse(data[0]);
                y_j = int.Parse(data[1]);
                z_k = int.Parse(data[2]);

                writer.WriteLine("  sphere {");
                writer.WriteLine("           <{0},{1},0>, 1.2, 1.0", x_i + 1, y_j + 1);
                writer.WriteLine("         }");
            }

            writer.WriteLine("   scale 1");
            writer.WriteLine("   pigment {rgb <0,0,0>}");
            writer.WriteLine("   finish { phong 0.8 }");
            writer.WriteLine("}");
            writer.WriteLine();
        }

        /// <summary>
        /// Writes the POVray script for drawing points with high beta
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="datoteka">Input file</param>
        public static void POVray_beta(StreamWriter writer, string[] datoteka)
        {
            string[] data;
            string[] separators = { "\t", " " };
            int x_i, y_j, z_k;

            writer.WriteLine("blob {");
            writer.WriteLine("  threshold 0.9");

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                x_i = int.Parse(data[0]);
                y_j = int.Parse(data[1]);
                z_k = int.Parse(data[2]);

                writer.WriteLine("  sphere {");
                writer.WriteLine("           <{0},{1},{2}>, 2.5, 1.0", x_i + 1, z_k, y_j + 1);
                writer.WriteLine("         }");
            }

            writer.WriteLine("   scale 1");
            writer.WriteLine("   pigment {rgb <1,0,0>}");
            writer.WriteLine("   finish { phong 0.8 }");
            writer.WriteLine("}");
            writer.WriteLine();
        }

        /// <summary>
        /// Writes the POVray script for drawing points with high beta
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="datoteka">Input file</param>
        /// <param name="set">Set array determining colour</param>
        public static void POVray_beta(StreamWriter writer, string[] datoteka, double[][][] set)
        {
            string[] data;
            string[] separators = { "\t", " " };
            int x_i, y_j, z_k;

            writer.WriteLine("blob {");
            writer.WriteLine("  threshold 0.9");

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                x_i = int.Parse(data[0]);
                y_j = int.Parse(data[1]);
                z_k = int.Parse(data[2]);

                writer.WriteLine("  sphere {");
                writer.Write("           <{0},{1},{2}>, 2.5, 1.0 pigment ", x_i + 1, z_k, y_j + 1);
                writer.WriteLine("{{rgb<{0:F2},0,{1:F2}>}}", (1.0 + set[x_i][y_j][z_k]), set[x_i][y_j][z_k]);
                writer.WriteLine("         }");
            }

            writer.WriteLine("   scale 1");
            writer.WriteLine("   pigment {rgb <1,0,0>}");
            writer.WriteLine("   finish { phong 0.8 }");
            writer.WriteLine("}");
            writer.WriteLine();
        }

        /// <summary>
        /// Writes the POVray script for drawing the director field
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="datoteka">Input file</param>
        /// <param name="factor">Display each factor point</param>
        /// <param name="N_r">Plane number</param>
        public static void POVray_director_field(StreamWriter writer, string[] datoteka, int factor, int N_r)
        {
            string[] data;
            string[] separators = { "\t", " " };
            int ii, jj, kk;
            double[] n_img = new double[3];

            for (int i = 0; i < datoteka.Length; i++)
            {
                data = datoteka[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                ii = int.Parse(data[0]);
                jj = int.Parse(data[1]);
                kk = int.Parse(data[2]);

                if (kk != N_r) { continue; }

                if (ii % factor == 0 && jj % factor == 0)
                {
                    n_img[0] = double.Parse(data[3]);
                    n_img[1] = double.Parse(data[4]);
                    n_img[2] = double.Parse(data[5]);

                    writer.WriteLine("object{");
                    writer.WriteLine("  Round_Cylinder");
                    writer.WriteLine("   (<{0},{2},{1}>,<{3},{5},{4}>, 1.0, 0.2, 1)", -n_img[0] * 3.0, -n_img[1] * 3.0, -n_img[2] * 3.0, n_img[0] * 3.0, n_img[1] * 3.0, n_img[2] * 3.0);
                    writer.WriteLine("    texture { pigment { color Green }");
                    writer.WriteLine("    finish { reflection 0.05 phong 1 }");
                    writer.WriteLine("  }");
                    writer.WriteLine("  translate<{0},{2},{1}>", ii, jj, kk);
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Writes the POVray script for drawing the director field
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="datoteka">Input file</param>
        /// <param name="factor">Display each factor point</param>
        /// <param name="N_r">Plane number</param>
        /// <param name="plane_n">Plane (0-yz, 1-xz. 2-xy)</param>
        public static void POVray_director_field(StreamWriter writer, string[] datoteka, int factor, int N_r, int plane_n)
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
        /// Writes the POVray script for drawing the director field
        /// </summary>
        /// <param name="writer">Writing tool reference</param>
        /// <param name="datoteka">Input file</param>
        /// <param name="zoom1">Zoomed in location in first direction</param>
        /// <param name="zoom2">Zoomed in location in second direction</param>
        /// <param name="N_r">Plane number</param>
        /// <param name="plane_n">Plane (0-yz, 1-xz. 2-xy)</param>
        public static void POVray_director_field(StreamWriter writer, string[] datoteka, int zoom1, int zoom2, int N_r, int plane_n)
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

        /// <summary>
        /// Writes the POVray script for drawing streamlines
        /// </summary>
        /// <param name="writer">>Writing tool reference</param>
        /// <param name="lines">Input file</param>
        public static void POVray_streamlines(StreamWriter writer, List<List<double[]>> lines)
        {
            for (int i = 0; i < lines.Count / 2; i++)
            {
                if (lines[i].Count < 50)
                {
                    continue;
                }

                writer.Write("sphere_sweep { cubic_spline ");
                if (lines[i].Count % 10 == 0) { writer.Write(lines[i].Count / 10); }
                else { writer.Write(lines[i].Count / 10 + 1); }
                writer.WriteLine(", ");
                for (int j = 0; j < lines[i].Count; j += 10)
                {
                    writer.WriteLine("  <{0:F3},{1:F3},0>, 0.2", lines[i][j][0], lines[i][j][1]);
                }
                writer.WriteLine("  texture{ pigment{ color Black}");
                writer.WriteLine("    finish { reflection 0.05 phong 1}");
                writer.WriteLine("  }");
                writer.WriteLine("  no_shadow");
                writer.WriteLine("}");
            }
        }
    }
}
