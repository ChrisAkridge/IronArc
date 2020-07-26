using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// Represents an array of bytes with several helper functions.
    /// </summary>
    public unsafe struct ByteBlock : IDisposable
    {
        private byte* pointer;

        public ulong Length { get; private set; }
        public IntPtr Pointer => (IntPtr)pointer;

        public byte this[ulong index]
        {
            get => *(pointer + index);
            set => *(pointer + index) = value;
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[Length];

            Marshal.Copy((IntPtr)pointer, result, 0, (int)Length);

            return result;
        }

        public UnmanagedMemoryStream CreateStream() => new UnmanagedMemoryStream(pointer, (long)Length);

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">An array of bytes used to initialize this byte block.</param>
        public ByteBlock(byte[] value) : this()
        {
            pointer = (byte*)Marshal.AllocHGlobal(value.Length);
            Length = (ulong)value.Length;

            Marshal.Copy(value, 0, (IntPtr)pointer, value.Length);
        }

        /// <summary>
        /// Converts a byte array into a ByteBlock.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <returns>A ByteBlock containing the bytes of the array.</returns>
        public static implicit operator ByteBlock(byte[] value) => new ByteBlock(value);

        #region Read At Methods
        public byte[] ReadAt(ulong address, ulong length)
        {
            if (address + length >= Length)
            {
                throw new ArgumentOutOfRangeException(
                    $"Cannot read data at 0x{address:X2} of length {length}. Arguement(s) out of range.");
            }

            byte[] result = new byte[length];
            for (ulong i = 0; i < length; i++)
            {
                result[i] = *(pointer + address);
                address++;
            }

            return result;
        }

        public void ReadIntoBuffer(byte[] buffer, int startIndex, ulong address, int length)
        {
            if (address + (ulong)length >= Length)
            {
                throw new ArgumentOutOfRangeException(
                    $"Cannot read data at 0x{address:X2} of length {length}. Arguement(s) out of range.");
            }

            var addressPointer = Pointer + length;
            Marshal.Copy(addressPointer, buffer, startIndex, length);
        }

        public bool ReadBoolAt(ulong address)
        {
            if (address >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read bool at 0x{address:X2}. Argument out of range.");
            }

            return *(pointer + address) != 0;
        }

        public byte ReadByteAt(ulong address)
        {
            if (address >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read byte at 0x{address:X2}. Argument out of range.");
            }

            return *(pointer + address);
        }

        public sbyte ReadSByteAt(ulong address) => (sbyte)ReadByteAt(address);

        public short ReadShortAt(ulong address)
        {
            if (address + 2 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read short at 0x{address:X2}. Argument out of range.");
            }

            return *(short*)(pointer + address);
        }

        public ushort ReadUShortAt(ulong address) => (ushort)ReadShortAt(address);

        public int ReadIntAt(ulong address)
        {
            if (address + 4 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read int at 0x{address:X2}. Argument out of range.");
            }

            return *(int*)(pointer + address);
        }

        public uint ReadUIntAt(ulong address) => (uint)ReadIntAt(address);

        public long ReadLongAt(ulong address)
        {
            if (address + 8 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read long at 0x{address:X2}. Argument out of range.");
            }

            return *(long*)(pointer + address);
        }

        public ulong ReadULongAt(ulong address) => (ulong)ReadLongAt(address);

        public float ReadFloatAt(ulong address)
        {
            if (address + 4 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read float at 0x{address:X2}. Argument out of range.");
            }

            return *(float*)(pointer + address);
        }

        public double ReadDoubleAt(ulong address)
        {
            if (address + 8 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read double at 0x{address:X2}. Argument out of range.");
            }

            return *(double*)(pointer + address);
        }

        public char ReadCharAt(uint address) => (char)ReadShortAt(address);

        public string ReadStringAt(ulong address, out uint lengthInBytes)
        {
            int stringLength = ReadIntAt(address);
            address += 4;

            if (address + (ulong)stringLength >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot read string at 0x{address:X2}. Argument out of range.");
            }

            byte[] utf8 = new byte[stringLength];
            for (int i = 0; i < stringLength; i++)
            {
                utf8[i] = *(pointer + address);
                address++;
            }

            lengthInBytes = (uint)(utf8.Length + 4u);
            return Encoding.UTF8.GetString(utf8);
        }

        public string ReadStringAt(ulong address) => ReadStringAt(address, out var _);

        public ulong ReadDataAt(ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    return ReadByteAt(address);
                case OperandSize.Word:
                    return ReadUShortAt(address);
                case OperandSize.DWord:
                    return ReadUIntAt(address);
                case OperandSize.QWord:
                    return ReadULongAt(address);
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }
        #endregion

        #region Write At Methods
        public void WriteAt(byte[] bytes, ulong address)
        {
            if (address + (ulong)bytes.Length > Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write at 0x{address:X2}. Argument out of range.");
            }

            Marshal.Copy(bytes, 0, (IntPtr)(pointer + address), bytes.Length);
        }

        public void WriteAt(byte[] bytes, int startIndex, int length, ulong address)
        {
            if (address + (ulong)length > Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write at 0x{address:X2}. Argument out of range.");
            }
            
            Marshal.Copy(bytes, startIndex, (IntPtr)(pointer + address), length);
        }

        public void WriteAt(ByteBlock bytes, ulong address)
        {
            if (address + bytes.Length >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write at 0x{address:X2}. Argument out of range.");
            }

            for (ulong i = 0; i < bytes.Length; i++)
            {
                *(pointer + address) = *(bytes.pointer + i);
                address++;
            }
        }

        public void WriteAt(ulong sourceAddress, ulong destAddress, uint length)
        {
            byte* sourceStart = pointer + sourceAddress;
            byte* destStart = pointer + destAddress;

            for (uint i = 0u; i < length; i++)
            {
                *(destStart + i) = *(sourceStart + i);
            }
        }

        public void WriteBoolAt(bool value, ulong address)
        {
            if (address >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write bool at 0x{address:X2}. Argument out of range.");
            }

            *(pointer + address) = (value) ? (byte)1 : (byte)0;
        }

        public void WriteByteAt(byte value, ulong address)
        {
            if (address >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write byte at 0x{address:X2}. Argument out of range.");
            }

            *(pointer + address) = value;
        }

        public void WriteSByteAt(sbyte value, ulong address) => WriteByteAt((byte)value, address);

        public void WriteShortAt(short value, ulong address)
        {
            if (address + 2 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write short at 0x{address:X2}. Argument out of range.");
            }

            *(short*)(pointer + address) = value;
        }

        public void WriteUShortAt(ushort value, ulong address) => WriteShortAt((short)value, address);

        public void WriteIntAt(int value, ulong address)
        {
            if (address + 4 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write int at 0x{address:X2}. Argument out of range.");
            }

            *(int*)(pointer + address) = value;
        }

        public void WriteUIntAt(uint value, ulong address) => WriteIntAt((int)value, address);

        public void WriteLongAt(long value, ulong address)
        {
            if (address + 8 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write long at 0x{address:X2}. Argument out of range.");
            }

            *(long*)(pointer + address) = value;
        }

        public void WriteULongAt(ulong value, ulong address) => WriteLongAt((long)value, address);

        public void WriteFloatAt(float value, ulong address)
        {
            if (address + 4 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write float at 0x{address:X2}. Argument out of range.");
            }

            *(float*)(pointer + address) = value;
        }

        public void WriteDoubleAt(double value, ulong address)
        {
            if (address + 8 >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write double at 0x{address:X2}. Argument out of range.");
            }

            *(double*)(pointer + address) = value;
        }

        public void WriteCharAt(char value, ulong address) => WriteShortAt((short)value, address);

        public void WriteStringAt(string value, ulong address)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(value);
            int stringLength = utf8.Length;

            if (address + 4UL + (ulong)stringLength >= Length)
            {
                throw new ArgumentOutOfRangeException($"Cannot write string at 0x{address:X2}. Argument out of range.");
            }

            WriteIntAt(stringLength, address);
            address += 4;

            for (int i = 0; i < stringLength; i++)
            {
                *(pointer + address) = utf8[i];
                address++;
            }
        }

        public void WriteDataAt(ulong data, ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    WriteByteAt((byte)data, address);
                    break;
                case OperandSize.Word:
                    WriteUShortAt((ushort)data, address);
                    break;
                case OperandSize.DWord:
                    WriteUIntAt((uint)data, address);
                    break;
                case OperandSize.QWord:
                    WriteULongAt(data, address);
                    break;
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }
        #endregion

        public void Dispose()
        {
            if ((IntPtr)pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pointer);
                pointer = (byte*)0;
            }
        }

        public static ByteBlock FromLength(ulong length)
        {
            var result = new ByteBlock
            {
                pointer = (byte*)Marshal.AllocHGlobal((int)length),
                Length = length
            };

            for (int i = 0; i < (uint)length; i++)
            {
                *(result.pointer + i) = 0;
            }

            return result;
        }
    }
}