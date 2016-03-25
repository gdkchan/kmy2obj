using System;
using System.IO;

namespace kmy2obj.IO
{
    class BigEndianBinaryReader
    {
        public Stream BaseStream { get; set; }

        /// <summary>
        ///     Creates a new Big Endian Binary Reader from a Stream.
        /// </summary>
        /// <param name="Input"></param>
        public BigEndianBinaryReader(Stream Input)
        {
            BaseStream = Input;
        }

        /// <summary>
        ///     Read a Unsigned 8 bits Integer from the Input.
        /// </summary>
        /// <returns></returns>
        public byte ReadUInt8()
        {
            return (byte)BaseStream.ReadByte();
        }

        /// <summary>
        ///     Read a Signed 8 bits Integer from the Input.
        /// </summary>
        /// <returns></returns>
        public sbyte ReadInt8()
        {
            return (sbyte)ReadUInt8();
        }

        /// <summary>
        ///     Read a Unsigned 16 bits Integer from the Input.
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            return (ushort)((BaseStream.ReadByte() << 8) |
                BaseStream.ReadByte());
        }

        /// <summary>
        ///     Read a Signed 16 bits Integer from the Input.
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        /// <summary>
        ///     Read a Unsigned 32 bits Integer from the Input.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            return (uint)((BaseStream.ReadByte() << 24) |
                (BaseStream.ReadByte() << 16) |
                (BaseStream.ReadByte() << 8) |
                BaseStream.ReadByte());
        }

        /// <summary>
        ///     Read a Signed 32 bits Integer from the Input.
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        /// <summary>
        ///     Read a Floating Point value with Single precision from the Input.
        /// </summary>
        /// <returns></returns>
        public float ReadSingle()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(ReadUInt32()), 0);
        }

        /// <summary>
        ///     Read a block of the Input to a buffer.
        /// </summary>
        /// <param name="Buff">Buffer where the data will be placed</param>
        /// <param name="Index">Index to start writting the data on the buffer</param>
        /// <param name="Length">Number of bytes to copy</param>
        public void Read(byte[] Buff, int Index, int Length)
        {
            BaseStream.Read(Buff, Index, Length);
        }

        /// <summary>
        ///     Seeks to a point on the file.
        /// </summary>
        /// <param name="Offset">Offset to seek</param>
        /// <param name="Origin">Relative position of the file where the Offset starts counting</param>
        public void Seek(long Offset, SeekOrigin Origin)
        {
            BaseStream.Seek(Offset, Origin);
        }

        /// <summary>
        ///     Close the Input Stream and releases all unmanaged resources.
        /// </summary>
        public void Close()
        {
            BaseStream.Close();
        }
    }
}
