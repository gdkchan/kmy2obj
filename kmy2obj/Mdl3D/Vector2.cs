namespace kmy2obj.Mdl3D
{
    /// <summary>
    ///     Basic structure of a 2-D point vector.
    /// </summary>
    class Vector2
    {
        public float X;
        public float Y;

        /// <summary>
        ///     Creates a new 2-D point from a 3.12 Fixed Point value.
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        public Vector2(short X, short Y)
        {
            this.X = X / 4096f;
            this.Y = Y / 4096f;
        }

        /// <summary>
        ///     Creates a new 2-D point.
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>
        ///     Creates a new 2-D point.
        /// </summary>
        public Vector2()
        {
        }
    }
}
