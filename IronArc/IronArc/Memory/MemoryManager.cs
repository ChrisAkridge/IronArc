using System;
using System.Text;

namespace IronArc.Memory
{
    public class MemoryManager
    {
        private ByteBlock systemMemory;

        public ulong SystemMemoryLength => systemMemory.Length;

        public MemoryManager(ByteBlock systemMemory) => this.systemMemory = systemMemory;

        public byte[] Read(ulong address, ulong length) => systemMemory.ReadAt(address, length);

        public byte ReadByte(ulong address) => systemMemory.ReadByteAt(address);

        public sbyte ReadSByte(ulong address) => (sbyte)ReadByte(address);

        public ushort ReadUShort(ulong address) => systemMemory.ReadUShortAt(address);

        public short ReadShort(ulong address) => (short)ReadUShort(address);

        public uint ReadUInt(ulong address) => systemMemory.ReadUIntAt(address);

        public int ReadInt(ulong address) => (int)ReadUInt(address);

        public ulong ReadULong(ulong address) => systemMemory.ReadULongAt(address);

        public long ReadLong(ulong address) => (long)ReadULong(address);

        public string ReadString(ulong address, out uint length)
        {
            uint stringLength = ReadUInt(address);

            length = stringLength + 4;
            return Encoding.UTF8.GetString(Read(address + 4, stringLength));
        }

        public ulong ReadData(ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    return ReadByte(address);
                case OperandSize.Word:
                    return ReadUShort(address);
                case OperandSize.DWord:
                    return ReadUInt(address);
                case OperandSize.QWord:
                    return ReadULong(address);
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }

        public void Write(byte[] bytes, ulong address) => systemMemory.WriteAt(bytes, address);

        public void Write(ulong source, ulong destination, ulong length)
        {
            var bytes = Read(source, length);
            Write(bytes, destination);
        }

        public void WriteByte(byte value, ulong address) => systemMemory.WriteByteAt(value, address);

        public void WriteSByte(sbyte value, ulong address) => WriteByte((byte)value, address);

        public void WriteUShort(ushort value, ulong address) => systemMemory.WriteUShortAt(value, address);

        public void WriteShort(short value, ulong address) => WriteUShort((ushort)value, address);

        public void WriteUInt(uint value, ulong address) => systemMemory.WriteUIntAt(value, address);

        public void WriteInt(int value, ulong address) => WriteUInt((uint)value, address);

        public void WriteULong(ulong value, ulong address) => systemMemory.WriteULongAt(value, address);

        public void WriteLong(long value, ulong address) => WriteULong((ulong)value, address);

        public void WriteString(string value, ulong address)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(value);
            uint stringLength = (uint)utf8.Length;
            WriteUInt(stringLength, address);
            Write(utf8, address + 4);
        }

        public void WriteData(ulong data, ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    WriteByte((byte)data, address);
                    break;
                case OperandSize.Word:
                    WriteUShort((ushort)data, address);
                    break;
                case OperandSize.DWord:
                    WriteUInt((uint)data, address);
                    break;
                case OperandSize.QWord:
                    WriteULong(data, address);
                    break;
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }
    }
}
