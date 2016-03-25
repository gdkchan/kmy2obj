namespace kmy2obj.Mdl3D
{
    /// <summary>
    ///     Basic structure of a 3-D point vector.
    /// </summary>
    class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        /// <summary>
        ///     Creates a new 3-D point from 8 bits values that are converted to the -1~1 range.
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Z">Z coordinate</param>
        public Vector3(sbyte X, sbyte Y, sbyte Z)
        {
            this.X = X / 128f;
            this.Y = Y / 128f;
            this.Z = Z / 128f;
        }

        /// <summary>
        ///     Creates a new 3-D point from 7.8 fixed point values.
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Z">Z coordinate</param>
        public Vector3(short X, short Y, short Z)
        {
            this.X = X / 256f;
            this.Y = Y / 256f;
            this.Z = Z / 256f;
        }

        /// <summary>
        ///     Creates a new 3-D point.
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Z">Z coordinate</param>
        public Vector3(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>
        ///     Creates a new 3-D point.
        /// </summary>
        public Vector3()
        {
        }
    }
}
