using System.IO;
using System.Text;
using System.Globalization;

namespace kmy2obj.Mdl3D.Formats
{
    /// <summary>
    ///     *.obj Model creation.
    /// </summary>
    class OBJ
    {
        /// <summary>
        ///     Saves a Model to a *.obj file.
        /// </summary>
        /// <param name="Mdl">Model that will be saved</param>
        /// <param name="OutFile">Path to save the *.obj file</param>
        public static void ToFile(Model Mdl, string OutFile)
        {
            StringBuilder Output = new StringBuilder();

            Output.AppendLine("#MGS TS kmy2obj conversion");

            Output.AppendLine(null);
            Output.AppendLine("#Geometry vertices");
            for (int i = 0; i < Mdl.PositionVerticesBuffer.Length; i++)
            {
                Vector3 v = Mdl.PositionVerticesBuffer[i];
                Output.AppendLine(string.Format("v {0} {1} {2}", GetFloat(v.X), GetFloat(v.Y), GetFloat(v.Z)));
            }

            Output.AppendLine(null);
            Output.AppendLine("#Normal vertices");
            for (int i = 0; i < Mdl.NormalVerticesBuffer.Length; i++)
            {
                Vector3 v = Mdl.NormalVerticesBuffer[i];
                Output.AppendLine(string.Format("vn {0} {1} {2}", GetFloat(v.X), GetFloat(v.Y), GetFloat(v.Z)));
            }

            Output.AppendLine(null);
            Output.AppendLine("#Texture vertices");
            for (int i = 0; i < Mdl.UVVerticesBuffer.Length; i++)
            {
                Vector2 v = Mdl.UVVerticesBuffer[i];
                Output.AppendLine(string.Format("vt {0} {1}", GetFloat(v.X), GetFloat(v.Y)));
            }

            Output.AppendLine(null);
            Output.AppendLine("#Faces");
            for (int i = 0; i < Mdl.PositionIndicesBuffer.Length; i += 3)
            {
                int p1 = Mdl.PositionIndicesBuffer[i] + 1;
                int p2 = Mdl.PositionIndicesBuffer[i + 1] + 1;
                int p3 = Mdl.PositionIndicesBuffer[i + 2] + 1;

                int t1 = Mdl.UVIndicesBuffer[i] + 1;
                int t2 = Mdl.UVIndicesBuffer[i + 1] + 1;
                int t3 = Mdl.UVIndicesBuffer[i + 2] + 1;

                int n1 = Mdl.NormalIndicesBuffer[i] + 1;
                int n2 = Mdl.NormalIndicesBuffer[i + 1] + 1;
                int n3 = Mdl.NormalIndicesBuffer[i + 2] + 1;

                Output.AppendLine(string.Format("f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8}", 
                    p1, t1, n1,
                    p2, t2, n2,
                    p3, t3, n3));
            }

            File.WriteAllText(OutFile, Output.ToString());
        }

        private static string GetFloat(float Value)
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
