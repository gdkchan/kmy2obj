namespace kmy2obj.Mdl3D
{
    /// <summary>
    ///     Represents a 3-D model.
    /// </summary>
    class Model
    {
        //Position
        public Vector3[] PositionVerticesBuffer;
        public ushort[] PositionIndicesBuffer;

        //Normal
        public Vector3[] NormalVerticesBuffer;
        public ushort[] NormalIndicesBuffer;

        //UV0
        public Vector2[] UVVerticesBuffer;
        public ushort[] UVIndicesBuffer;
    }
}
