using System;
using System.Collections.Generic;
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

        public byte this[ulong index]
        {
            get
            {
				byte* resultPointer = pointer + index;
				return *resultPointer;
            }

            set
            {
				byte* resultPointer = pointer + index;
				*resultPointer = value;
            }
        }

		public static ByteBlock Empty
		{
			get
			{
				return new ByteBlock();
			}
		}

        public byte[] ToByteArray()
        {
			byte[] result = new byte[Length];
			byte* current = pointer;

			for (ulong i = 0UL; i < Length; i++)
			{
				result[i] = *current;
				current++;
			}

			return result;
        }

        #region Constructors
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
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="length">The number of bytes from the array to use.</param>
        /// <param name="value">An array of bytes of which a portion is used to initialize this byte block.</param>
        /// <param name="startIndex">The index at which to start reading bytes in the array.</param>
        public ByteBlock(ulong length, byte[] value, int startIndex) : this()
        {
			pointer = (byte*)Marshal.AllocHGlobal((int)length);
			Length = length;

			Marshal.Copy(value, startIndex, (IntPtr)pointer, (int)length);
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A Boolean value. True becomes 0xFF and false becomes 0x00.</param>
        public ByteBlock(bool value)
        {
			this = ByteBlock.FromLength(1);
			*pointer = (value) ? (byte)1 : (byte)0;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A byte value.</param>
        public ByteBlock(byte value)
        {
			this = ByteBlock.FromLength(1);
			*pointer = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A signed byte value.</param>
        public ByteBlock(sbyte value)
            : this((byte)value)
        {
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A signed 16-bit integer value.</param>
        public ByteBlock(short value)
        {
			this = ByteBlock.FromLength(2);
			*(short*)pointer = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">An unsigned 16-bit integer value.</param>
        public ByteBlock(ushort value)
            : this((short)value)
        {
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A signed 32-bit integer value.</param>
        public ByteBlock(int value)
        {
			this = ByteBlock.FromLength(4);
			*(int*)pointer = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">An unsigned 32-bit integer value.</param>
        public ByteBlock(uint value)
            : this((int)value)
        {
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A signed 64-bit integer value.</param>
        public ByteBlock(long value)
        {
			this = ByteBlock.FromLength(8);
			*(long*)pointer = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">An unsigned 64-bit integer value.</param>
        public ByteBlock(ulong value)
            : this((long)value)
        {
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A single-precision floating point value.</param>
        public ByteBlock(float value)
        {
			this = ByteBlock.FromLength(4);
			*(float*)pointer = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A double-precision floating point value.</param>
        public ByteBlock(double value)
        {
			this = ByteBlock.FromLength(8);
			*(double*)pointer = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A UTF-16 character value.</param>
        public ByteBlock(char value) : this((short)value)
        { }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A string value which will be converted to UTF-8 bytes.</param>
        public ByteBlock(string value)
        {
			byte[] utf8 = Encoding.UTF8.GetBytes(value);
			this = ByteBlock.FromLength(4 + (ulong)utf8.Length);

			*(int*)pointer = utf8.Length;
			byte* stringPointer = pointer + 4;

			for (int i = 0; i < utf8.Length; i++)
			{
				*stringPointer = utf8[i];
				stringPointer++;
			}
        }
        #endregion

        #region Implicit Conversions
        /// <summary>
        /// Converts a byte array into a ByteBlock.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <returns>A ByteBlock containing the bytes of the array.</returns>
        public static implicit operator ByteBlock(byte[] value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(bool value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(byte value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(sbyte value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(short value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(ushort value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(int value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(uint value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(long value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(ulong value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(float value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(double value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(char value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator ByteBlock(string value)
        {
            return new ByteBlock(value);
        }

        public static implicit operator bool(ByteBlock value)
        {
            return value.ToBool();
        }

        public static implicit operator byte(ByteBlock value)
        {
            return value.ToByte();
        }

        public static implicit operator sbyte(ByteBlock value)
        {
            return value.ToSByte();
        }

        public static implicit operator short(ByteBlock value)
        {
            return value.ToShort();
        }

        public static implicit operator ushort(ByteBlock value)
        {
            return value.ToUShort();
        }

        public static implicit operator int(ByteBlock value)
        {
            return value.ToInt();
        }

        public static implicit operator uint(ByteBlock value)
        {
            return value.ToUInt();
        }

        public static implicit operator long(ByteBlock value)
        {
            return value.ToLong();
        }

        public static implicit operator ulong(ByteBlock value)
        {
            return value.ToULong();
        }

        public static implicit operator char(ByteBlock value)
        {
            return value.ToChar();
        }

        public static implicit operator string(ByteBlock value)
        {
            return value.ToString();
        }
        #endregion

        #region Conversions
        public bool ToBool()
        {
			if (Length != 1)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to bool (length 1).", Length));
			}

			return *pointer != 0;
        }

        public byte ToByte()
        {
			if (Length != 1)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to byte/sbyte (length 1).", Length));
			}

			return *pointer;
        }

        public sbyte ToSByte()
        {
            return (sbyte)ToByte();
        }

        public short ToShort()
        {
			if (Length != 2)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to short/ushort (length 2).", Length));
			}

			return *(short*)pointer;
        }

        public ushort ToUShort()
        {
            return (ushort)ToShort();
        }

        public int ToInt()
        {
			if (Length != 4)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to int/uint (length 4).", Length));
			}

			return *(int*)pointer;
        }

        public uint ToUInt()
        {
            return (uint)ToInt();
        }

        public long ToLong()
        {
			if (Length != 8)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to long/ulong (length 8).", Length));
			}

			return *(long*)pointer;
        }

        public ulong ToULong()
        {
            return (uint)ToLong();
        }

        public float ToFloat()
        {
			if (Length != 4)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to float (length 4).", Length));
			}

			return *(float*)pointer;
        }

        public double ToDouble()
        {
			if (Length != 8)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to double (length 8).", Length));
			}

			return *(double*)pointer;
        }

        public char ToChar()
        {
            return (char)ToShort();
        }

        public override string ToString()
        {
			if (Length < 4)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to string (at least length 4).", Length));
			}

			int stringLength = *(int*)pointer;
			byte[] utf8 = new byte[stringLength];

			for (int i = 0; i < stringLength; i++)
			{
				utf8[i] = *(pointer + i);
			}

			return Encoding.UTF8.GetString(utf8);
        }
        #endregion

        #region Read At Methods
        public byte[] ReadAt(ulong length, ulong address)
        {
			if (address < 0 || address + length >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read data at 0x{0:X2} of length {1}. Arguement(s) out of range.", address, length));
			}

            byte[] result = new byte[length];
            for (ulong i = 0; i < length; i++)
            {
				result[i] = *(pointer + address);
				address++;
            }

            return result;
        }

        public bool ReadBoolAt(ulong address)
        {
            if (address < 0 || address >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read bool at 0x{0:X2}. Argument out of range.", address));
			}

			return *(pointer + address) != 0;
        }

        public byte ReadByteAt(ulong address)
        {
            if (address < 0 || address >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read byte at 0x{0:X2}. Argument out of range.", address));
			}

			return *(pointer + address);
        }

        public sbyte ReadSByteAt(ulong address)
        {
            return (sbyte)ReadByteAt(address);
        }

        public short ReadShortAt(ulong address)
        {
            if (address < 0 || address + 2 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read short at 0x{0:X2}. Argument out of range.", address));
			}

			return *(short*)(pointer + address);
        }

        public ushort ReadUShortAt(ulong address)
        {
            return (ushort)ReadShortAt(address);
        }

        public int ReadIntAt(ulong address)
        {
			if (address < 0 || address + 4 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read int at 0x{0:X2}. Argument out of range.", address));
			}

			return *(int*)(pointer + address);
        }

        public uint ReadUIntAt(ulong address)
        {
            return (uint)ReadIntAt(address);
        }

        public long ReadLongAt(ulong address)
        {
			if (address < 0 || address + 8 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read long at 0x{0:X2}. Argument out of range.", address));
			}

			return *(long*)(pointer + address);
        }

        public ulong ReadULongAt(ulong address)
        {
            return (ulong)ReadLongAt(address);
        }

        public float ReadFloatAt(ulong address)
        {
			if (address < 0 || address + 4 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read float at 0x{0:X2}. Argument out of range.", address));
			}

			return *(float*)(pointer + address);
        }

        public double ReadDoubleAt(ulong address)
        {
			if (address < 0 || address + 8 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read double at 0x{0:X2}. Argument out of range.", address));
			}

			return *(double*)(pointer + address);
        }

        public char ReadCharAt(uint address)
        {
            return (char)ToShort();
        }

        public string ReadStringAt(ulong length, ulong address)
        {
			int stringLength = ReadIntAt(address);
			address += 4;

			if (address + (ulong)stringLength >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot read string at 0x{0:X2}. Argument out of range.", address));
			}

			byte[] utf8 = new byte[stringLength];
			for (int i = 0; i < stringLength; i++)
			{
				utf8[i] = *(pointer + address);
				address++;
			}

			return Encoding.UTF8.GetString(utf8);
        }
        #endregion

        #region Write At Methods
        public void WriteAt(byte[] bytes, ulong address)
        {
			if (address < 0 || address + (ulong)bytes.Length > Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write at 0x{0:X2}. Argument out of range.", address));
			}

            for (int i = 0; i < bytes.Length; i++)
            {
				*(pointer + address) = bytes[i];
				address++;
            }
        }

        public void WriteAt(ByteBlock bytes, ulong address)
        {
			if (address < 0 || address + bytes.Length >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write at 0x{0:X2}. Argument out of range.", address));
			}

			for (ulong i = 0; i < (ulong)bytes.Length; i++)
			{
				*(pointer + address) = *(bytes.pointer + i);
				address++;
			}
        }

        public void WriteBoolAt(bool value, ulong address)
        {
            if (address < 0 || address >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write bool at 0x{0:X2}. Argument out of range.", address));
			}

			*(pointer + address) = (value) ? (byte)1 : (byte)0;
        }

        public void WriteByteAt(byte value, ulong address)
        {
			if (address < 0 || address >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write byte at 0x{0:X2}. Argument out of range.", address));
			}

			*(pointer + address) = value;
        }

        public void WriteSByteAt(sbyte value, ulong address)
        {
			WriteByteAt((byte)value, address);
        }

        public void WriteShortAt(short value, ulong address)
        {
			if (address < 0 || address + 2 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write short at 0x{0:X2}. Argument out of range.", address));
			}

			*(short*)(pointer + address) = value;
        }

        public void WriteUShortAt(ushort value, ulong address)
        {
			WriteShortAt((short)value, address);
        }

        public void WriteIntAt(int value, ulong address)
        {
			if (address < 0 || address + 4 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write int at 0x{0:X2}. Argument out of range.", address));
			}

			*(int*)(pointer + address) = value;
        }

        public void WriteUIntAt(uint value, ulong address)
        {
			WriteIntAt((int)value, address);
        }

        public void WriteLongAt(long value, ulong address)
        {
			if (address < 0 || address + 8 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write long at 0x{0:X2}. Argument out of range.", address));
			}

			*(long*)(pointer + address) = value;
        }

        public void WriteULongAt(ulong value, ulong address)
        {
			WriteLongAt((long)value, address);
        }

        public void WriteFloatAt(float value, ulong address)
        {
			if (address < 0 || address + 4 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write float at 0x{0:X2}. Argument out of range.", address));
			}

			*(float*)(pointer + address) = value;
        }

        public void WriteDoubleAt(double value, ulong address)
        {
			if (address < 0 || address + 8 >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write double at 0x{0:X2}. Argument out of range.", address));
			}

			*(double*)(pointer + address) = value;
        }

        public void WriteCharAt(char value, ulong address)
        {
			WriteShortAt((short)value, address);
        }

        public void WriteStringAt(string value, ulong address)
        {
			byte[] utf8 = Encoding.UTF8.GetBytes(value);
			int stringLength = utf8.Length;

			if (address < 0 || address + 4UL + (ulong)stringLength >= Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot write string at 0x{0:X2}. Argument out of range.", address));
			}

			WriteIntAt(stringLength, address);
			address += 4;

			for (int i = 0; i < stringLength; i++)
			{
				*(pointer + address) = utf8[i];
				address++;
			}
        }
        #endregion

		#region IDisposable Methods
		public void Dispose()
		{
			if ((IntPtr)pointer != IntPtr.Zero)
			{
				Marshal.FreeHGlobal((IntPtr)pointer);
				pointer = (byte*)0;
			}
		}
		#endregion

		public static ByteBlock FromLength(ulong length)
		{
			ByteBlock result = new ByteBlock();
			result.pointer = (byte*)Marshal.AllocHGlobal((int)length);
			result.Length = length;

			for (int i = 0; i < (uint)length; i++)
			{
				*(result.pointer + i) = 0;
			}

			return result;
		}
    }
}