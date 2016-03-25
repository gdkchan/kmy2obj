using System.Collections.Generic;
using System.IO;

using kmy2obj.IO;

namespace kmy2obj.Mdl3D.Formats
{
    /// <summary>
    ///     Metal Gear Solid Twin Snakes *.kmy model handling.
    /// </summary>
    class KMY
    {
        /// <summary>
        ///     Reads a static Model from Metal Gear Twin Snakes.
        /// </summary>
        /// <param name="Input">The Stream of the *.kmy model file</param>
        /// <returns></returns>
        public static Model FromStream(Stream Input)
        {
            Model Mdl = new Model();
            BigEndianBinaryReader Reader = new BigEndianBinaryReader(Input);

            Reader.Seek(0x40, SeekOrigin.Begin);
            ushort UnknownBlockCount = Reader.ReadUInt16();
            ushort PositionVerticesCount = Reader.ReadUInt16();
            ushort NormalVerticesCount = Reader.ReadUInt16();
            ushort UVVerticesCount = Reader.ReadUInt16();

            Reader.Seek(0x4c, SeekOrigin.Begin);
            uint UnknownBlockOffset = Reader.ReadUInt32();
            uint PositionVerticesOffset = Reader.ReadUInt32();
            uint NormalVerticesOffset = Reader.ReadUInt32();
            uint UVVerticesOffset = Reader.ReadUInt32();
            uint BaseOffset = Reader.ReadUInt32();

            UnknownBlockOffset += BaseOffset;
            PositionVerticesOffset += BaseOffset;
            NormalVerticesOffset += BaseOffset;
            UVVerticesOffset += BaseOffset;

            //Mesh Section Header
            Reader.Seek(BaseOffset, SeekOrigin.Begin);
            Reader.Seek(8, SeekOrigin.Current);
            ushort MeshTable1Count = Reader.ReadUInt16();
            ushort MeshTable2Count = Reader.ReadUInt16();
            uint OptionalSectionCount = Reader.ReadUInt32(); //???
            uint MeshTableOffset = Reader.ReadUInt32() + BaseOffset;

            List<ushort> PositionIndicesBuffer = new List<ushort>();
            List<ushort> NormalIndicesBuffer = new List<ushort>();
            List<ushort> UVIndicesBuffer = new List<ushort>();

            int MeshIndex = 0;
            uint MeshTableSectionEnd = (uint)Input.Length;
            for (;;)
            {
                Reader.Seek(MeshTableOffset + MeshIndex++ * 0x48, SeekOrigin.Begin);
                if (Input.Position >= MeshTableSectionEnd) break; //gdkchan self note: Find the actual values on the file!

                Reader.Seek(0x38, SeekOrigin.Current);
                ushort Entries = Reader.ReadUInt16();
                Reader.Seek(0xa, SeekOrigin.Current);
                uint Table2EntryOffset = Reader.ReadUInt32() + BaseOffset;
                if (Table2EntryOffset < MeshTableSectionEnd) MeshTableSectionEnd = Table2EntryOffset;

                for (int i = 0; i < Entries; i++)
                {
                    Reader.Seek(Table2EntryOffset + i * 0x24, SeekOrigin.Begin);
                    Reader.Seek(0x10, SeekOrigin.Current);

                    ushort FacesCount = Reader.ReadUInt16();
                    ushort NodesCount = Reader.ReadUInt16();
                    uint UVSet1NodesOffset = Reader.ReadUInt32() + BaseOffset;
                    uint UVSet2NodesOffset = Reader.ReadUInt32() + BaseOffset;
                    Reader.ReadUInt32(); //0x3
                    uint FacesTableOffset = Reader.ReadUInt32() + BaseOffset;

                    for (int j = 0; j < FacesCount; j++)
                    {
                        Reader.Seek(FacesTableOffset + j * 0x20, SeekOrigin.Begin);

                        uint PositionIndicesBufferOffset = Reader.ReadUInt32() + BaseOffset;
                        uint UVIndicesBufferOffset = Reader.ReadUInt32();
                        uint NormalIndicesBufferOffset = Reader.ReadUInt32() + BaseOffset;
                        Reader.Seek(0x10, SeekOrigin.Current);
                        ushort VerticesCount = Reader.ReadUInt16();

                        PositionIndicesBuffer.AddRange(GetIndicesBuffer(Reader, PositionIndicesBufferOffset, VerticesCount));
                        NormalIndicesBuffer.AddRange(GetIndicesBuffer(Reader, NormalIndicesBufferOffset, VerticesCount));

                        if (UVIndicesBufferOffset > 0)
                        {
                            List<ushort> NodesIndices = GetIndicesBuffer(Reader, UVIndicesBufferOffset + BaseOffset, VerticesCount);
                            foreach (ushort Index in NodesIndices)
                            {
                                Reader.Seek(UVSet1NodesOffset + Index * 2, SeekOrigin.Begin);
                                UVIndicesBuffer.Add(Reader.ReadUInt16());
                            }
                        }
                        else
                        {
                            Reader.Seek(UVSet1NodesOffset, SeekOrigin.Begin);
                            ushort Value = Reader.ReadUInt16();
                            for (int k = 0; k < (VerticesCount - 2) * 3; k++) UVIndicesBuffer.Add(Value);
                        }
                    }
                }
            }

            Mdl.PositionVerticesBuffer = GetVerticesBuffer3D(Reader, PositionVerticesOffset, PositionVerticesCount);
            Mdl.NormalVerticesBuffer = GetVerticesBuffer3D8b(Reader, NormalVerticesOffset, NormalVerticesCount);
            Mdl.UVVerticesBuffer = GetVerticesBuffer2D(Reader, UVVerticesOffset, UVVerticesCount);

            Mdl.PositionIndicesBuffer = PositionIndicesBuffer.ToArray();
            Mdl.NormalIndicesBuffer = NormalIndicesBuffer.ToArray();
            Mdl.UVIndicesBuffer = UVIndicesBuffer.ToArray();

            return Mdl;
        }

        /// <summary>
        ///     Reads a static Model from Metal Gear 2 Twin Snakes.
        /// </summary>
        /// <param name="FileName">The full path to the *.kmy model file</param>
        /// <returns></returns>
        public static Model FromFile(string FileName)
        {
            using (Stream Input = new FileStream(FileName, FileMode.Open))
            {
                return FromStream(Input);
            }
        }

        #region "Vertices Buffer methods"
        /// <summary>
        ///     Read a Buffer with 7.8 Fixed Point 3-D vertices values from the Stream.
        /// </summary>
        /// <param name="Reader">The Big Endian Binary Reader of the Input Stream</param>
        /// <param name="Offset">Offset where the Buffer is located</param>
        /// <param name="Count">Total number of vertices to read from the Buffer</param>
        /// <returns></returns>
        private static Vector3[] GetVerticesBuffer3D(BigEndianBinaryReader Reader, uint Offset, uint Count)
        {
            List<Vector3> Output = new List<Vector3>();

            Reader.Seek(Offset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++) Output.Add(new Vector3(
                Reader.ReadInt16(),
                Reader.ReadInt16(),
                Reader.ReadInt16()));

            return Output.ToArray();
        }

        /// <summary>
        ///     Read a Buffer with 8 bits 3-D vertices values from the Stream.
        /// </summary>
        /// <param name="Reader">The Big Endian Binary Reader of the Input Stream</param>
        /// <param name="Offset">Offset where the Buffer is located</param>
        /// <param name="Count">Total number of vertices to read from the Buffer</param>
        /// <returns></returns>
        private static Vector3[] GetVerticesBuffer3D8b(BigEndianBinaryReader Reader, uint Offset, uint Count)
        {
            List<Vector3> Output = new List<Vector3>();

            Reader.Seek(Offset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++) Output.Add(new Vector3(
                Reader.ReadInt8(),
                Reader.ReadInt8(),
                Reader.ReadInt8()));

            return Output.ToArray();
        }

        /// <summary>
        ///     Read a Buffer with 3.12 Fixed Point 2-D vertices values from the Stream.
        /// </summary>
        /// <param name="Reader">The Big Endian Binary Reader of the Input Stream</param>
        /// <param name="Offset">Offset where the Buffer is located</param>
        /// <param name="Count">Total number of vertices to read from the Buffer</param>
        /// <returns></returns>
        private static Vector2[] GetVerticesBuffer2D(BigEndianBinaryReader Reader, uint Offset, uint Count)
        {
            List<Vector2> Output = new List<Vector2>();

            Reader.Seek(Offset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++) Output.Add(new Vector2(
                Reader.ReadInt16(),
                Reader.ReadInt16()));

            return Output.ToArray();
        }
        #endregion

        #region "Indices Buffer methods"
        /// <summary>
        ///     Read a Buffer with 16-bits Indices from the Stream.
        /// </summary>
        /// <param name="Reader">The Big Endian Binary Reader of the Input Stream</param>
        /// <param name="Offset">Offset where the Buffer is located</param>
        /// <param name="Count">Total number of indices to read from the Buffer</param>
        /// <returns></returns>
        private static List<ushort> GetIndicesBuffer(BigEndianBinaryReader Reader, uint Offset, uint Count)
        {
            List<ushort> Output = new List<ushort>();

            Reader.Seek(Offset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++)
            {
                if (i < 3)
                    Output.Add(Reader.ReadUInt16());
                else
                {
                    int Index = Output.Count - 1;
                    Output.Add(Output[Index - 1]);
                    Output.Add(Output[Index]);
                    Output.Add(Reader.ReadUInt16());
                }
            }

            return Output;
        }
        #endregion
    }
}
