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

		public int Length { get; private set; }

        public byte this[int index]
        {
            get
            {
				byte* resultPointer = this.pointer + index;
				return *resultPointer;
            }

            set
            {
				byte* resultPointer = this.pointer + index;
				*resultPointer = value;
            }
        }

        public byte[] ToByteArray()
        {
			byte[] result = new byte[this.Length];
			byte* current = this.pointer;

			for (int i = 0; i < this.Length; i++)
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
        public ByteBlock(byte[] value)
        {
			this.pointer = (byte*)Marshal.AllocHGlobal(value.Length);
			this.Length = value.Length;

			Marshal.Copy(value, 0, (IntPtr)this.pointer, value.Length);
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="length">The number of bytes from the array to use.</param>
        /// <param name="value">An array of bytes of which a portion is used to initialize this byte block.</param>
        /// <param name="startIndex">The index at which to start reading bytes in the array.</param>
        public ByteBlock(int length, byte[] value, int startIndex)
        {
			this.pointer = (byte*)Marshal.AllocHGlobal(length);
			this.Length = length;

			Marshal.Copy(value, startIndex, (IntPtr)this.pointer, length);
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A Boolean value. True becomes 0xFF and false becomes 0x00.</param>
        public ByteBlock(bool value)
        {
			this = ByteBlock.FromLength(1);
			*this.pointer = (value) ? (byte)1 : (byte)0;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A byte value.</param>
        public ByteBlock(byte value)
        {
			this = ByteBlock.FromLength(1);
			*this.pointer = value;
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
			*(short*)this.pointer = value;
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
			*(int*)this.pointer = value;
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
			*(long*)this.pointer = value;
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
			*(float*)this.pointer = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A double-precision floating point value.</param>
        public ByteBlock(double value)
        {
			this = ByteBlock.FromLength(8);
			*(double*)this.pointer = value;
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
			this = ByteBlock.FromLength(4 + utf8.Length);

			*(int*)this.pointer = utf8.Length;
			byte* stringPointer = this.pointer + 4;

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
			if (this.Length != 1)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to bool (length 1).", this.Length));
			}

			return *this.pointer != 0;
        }

        public byte ToByte()
        {
			if (this.Length != 1)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to byte/sbyte (length 1).", this.Length));
			}

			return *this.pointer;
        }

        public sbyte ToSByte()
        {
            return (sbyte)this.ToByte();
        }

        public short ToShort()
        {
			if (this.Length != 2)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to short/ushort (length 2).", this.Length));
			}

			return *(short*)this.pointer;
        }

        public ushort ToUShort()
        {
            return (ushort)this.ToShort();
        }

        public int ToInt()
        {
			if (this.Length != 4)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to int/uint (length 4).", this.Length));
			}

			return *(int*)this.pointer;
        }

        public uint ToUInt()
        {
            return (uint)this.ToInt();
        }

        public long ToLong()
        {
			if (this.Length != 8)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to long/ulong (length 8).", this.Length));
			}

			return *(long*)this.pointer;
        }

        public ulong ToULong()
        {
            return (uint)this.ToLong();
        }

        public float ToFloat()
        {
			if (this.Length != 4)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to float (length 4).", this.Length));
			}

			return *(float*)this.pointer;
        }

        public double ToDouble()
        {
			if (this.Length != 8)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to double (length 8).", this.Length));
			}

			return *(double*)this.pointer;
        }

        public char ToChar()
        {
            return (char)this.ToShort();
        }

        public override string ToString()
        {
			if (this.Length < 4)
			{
				throw new InvalidCastException(string.Format("Cannot convert a ByteBlock of length {0} to string (at least length 4).", this.Length));
			}

			int stringLength = *(int*)this.pointer;
			byte[] utf8 = new byte[stringLength];

			for (int i = 0; i < stringLength; i++)
			{
				utf8[i] = *(this.pointer + i);
			}

			return Encoding.UTF8.GetString(utf8);
        }
        #endregion

		// Where you left off: fix the read at methods
        #region Read At Methods
        public byte[] ReadAt(uint length, uint address)
        {
            Assert.IsTrue(address >= 0 && this.Length - address > length);

            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = this.bytes[address++];
            }

            return result;
        }

        public bool ReadBoolAt(uint address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);

            return this.bytes[address] != 0;
        }

        public byte ReadByteAt(uint address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);

            return this.bytes[address];
        }

        public sbyte ReadSByteAt(uint address)
        {
            return (sbyte)this.ReadByteAt(address);
        }

        public short ReadShortAt(uint address)
        {
            return BitConverter.ToInt16(this.ReadAt(2, address), 0);
        }

        public ushort ReadUShortAt(uint address)
        {
            return (ushort)this.ReadShortAt(address);
        }

        public int ReadIntAt(uint address)
        {
            return BitConverter.ToInt32(this.ReadAt(4, address), 0);
        }

        public uint ReadUIntAt(uint address)
        {
            return (uint)this.ReadIntAt(address);
        }

        public long ReadLongAt(uint address)
        {
            return BitConverter.ToInt64(this.ReadAt(8, address), 0);
        }

        public ulong ReadULongAt(uint address)
        {
            return (ulong)this.ReadLongAt(address);
        }

        public float ReadFloatAt(uint address)
        {
            return BitConverter.ToSingle(this.ReadAt(4, address), 0);
        }

        public double ReadDoubleAt(uint address)
        {
            return BitConverter.ToDouble(this.ReadAt(8, address), 0);
        }

        public char ReadCharAt(uint address)
        {
            return (char)this.ToShort();
        }

        public string ReadStringAt(uint length, uint address)
        {
			byte[] bytes = this.ReadAt((uint)length, address);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        #endregion

        #region Write At Methods
        public void WriteAt(byte[] bytes, int address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);
            Assert.IsTrue(address + bytes.Length < this.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                this.bytes[address] = bytes[i];
                address++;
            }
        }

        public void WriteAt(ByteBlock bytes, int address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);
            Assert.IsTrue(address + bytes.Length < this.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                this.bytes[address] = bytes[i];
                address++;
            }
        }

        public void WriteBoolAt(bool value, int address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);

            if (value)
            {
                this.bytes[address] = 0xFF;
            }
            else
            {
                this.bytes[address] = 0x00;
            }
        }

        public void WriteByteAt(byte value, int address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);

            this.bytes[address] = value;
        }

        public void WriteSByteAt(sbyte value, int address)
        {
            this.WriteByteAt((byte)value, address);
        }

        public void WriteShortAt(short value, int address)
        {
            this.WriteAt(BitConverter.GetBytes(value), address);
        }

        public void WriteUShortAt(ushort value, int address)
        {
            this.WriteShortAt((short)value, address);
        }

        public void WriteIntAt(int value, int address)
        {
            this.WriteAt(BitConverter.GetBytes(value), address);
        }

        public void WriteUIntAt(uint value, int address)
        {
            this.WriteIntAt((int)value, address);
        }

        public void WriteLongAt(long value, int address)
        {
            this.WriteAt(BitConverter.GetBytes(value), address);
        }

        public void WriteULongAt(ulong value, int address)
        {
            this.WriteLongAt((long)value, address);
        }

        public void WriteFloatAt(float value, int address)
        {
            this.WriteAt(BitConverter.GetBytes(value), address);
        }

        public void WriteDoubleAt(double value, int address)
        {
            this.WriteAt(BitConverter.GetBytes(value), address);
        }

        public void WriteCharAt(char value, int address)
        {
            this.WriteShortAt((short)value, address);
        }

        public void WriteStringAt(string value, int address)
        {
            this.WriteAt(System.Text.Encoding.UTF8.GetBytes(value), address);
        }
        #endregion

		public static ByteBlock FromLength(int length)
		{
			ByteBlock result = new ByteBlock();
			result.pointer = (byte*)Marshal.AllocHGlobal(length);
			result.Length = length;

			for (int i = 0; i < length; i++)
			{
				*(result.pointer + i) = 0;
			}

			return result;
		}
    }
}