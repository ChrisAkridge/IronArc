using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// Represents an array of bytes with several helper functions.
    /// </summary>
    public sealed class ByteBlock
    {
        private byte[] bytes;
        private int location = 0;

        public int Length
        {
            get
            {
                return this.bytes.Length;
            }
        }

        public byte this[int index]
        {
            get
            {
                Assert.IsTrue(index >= 0 && index < this.Length);
                return this.bytes[index];
            }

            set
            {
                Assert.IsTrue(index >= 0 && index < this.Length);
                this.bytes[index] = value;
            }
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">An array of bytes used to initialize this byte block.</param>
        public ByteBlock(byte[] value)
        {
            this.bytes = value;
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="length">The number of bytes from the array to use.</param>
        /// <param name="value">An array of bytes of which a portion is used to initialize this byte block.</param>
        /// <param name="startIndex">The index at which to start reading bytes in the array.</param>
        public ByteBlock(int length, byte[] value, int startIndex)
        {
            this.bytes = new byte[length];
            int i = 0;

            while (length > 0)
            {
                this.bytes[i] = value[startIndex];
                i++;
                startIndex++;
                length--;
            }
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A Boolean value. True becomes 0xFF and false becomes 0x00.</param>
        public ByteBlock(bool value)
        {
            if (value)
            {
                this.bytes = new byte[] { 0xFF };
            }
            else
            {
                this.bytes = new byte[] { 0x00 };
            }
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A byte value.</param>
        public ByteBlock(byte value)
        {
            this.bytes = new byte[] { value };
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
            this.bytes = new byte[2];
            this.bytes[0] = (byte)(value >> 8);
            this.bytes[1] = (byte)((value << 8) >> 8);
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
            this.bytes = new byte[4];

            for (int i = 0; i < 3; i++)
            {
                this.bytes[i] = (byte)((value << (8 * i)) >> 24);
            }
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
            this.bytes = new byte[8];

            for (int i = 0; i < 7; i++)
            {
                this.bytes[i] = (byte)((value << (8 * i)) >> 56);
            }
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
        public ByteBlock(float value) : this(BitConverter.ToInt32(BitConverter.GetBytes(value), 0))
        {
        }

        /// <summary>
        /// Initializes a new instance of this <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="value">A double-precision floating point value.</param>
        public ByteBlock(double value) : this(BitConverter.ToInt64(BitConverter.GetBytes(value), 0))
        {
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
            this.bytes = System.Text.Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// Returns an empty ByteBlock instance initialized to a given length.
        /// </summary>
        /// <param name="length">The length of the ByteBlock.</param>
        /// <returns>An empty ByteBlock of the given length.</returns>
        public static ByteBlock FromLength(int length)
        {
            byte[] bytes = new byte[length];
            ByteBlock result = new ByteBlock(0);
            result.bytes = bytes;
            return result;
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
            Assert.IsTrue(this.Length == 1);

            return this.bytes[0] != 0x00;
        }

        public byte ToByte()
        {
            Assert.IsTrue(this.Length == 1);

            return this.bytes[0];
        }

        public sbyte ToSByte()
        {
            return (sbyte)this.ToByte();
        }

        public short ToShort()
        {
            Assert.IsTrue(this.Length == 2);

            return BitConverter.ToInt16(this.bytes, 0);
        }

        public ushort ToUShort()
        {
            return (ushort)this.ToShort();
        }

        public int ToInt()
        {
            Assert.IsTrue(this.Length == 4);

            return BitConverter.ToInt32(this.bytes, 0);
        }

        public uint ToUInt()
        {
            return (uint)this.ToInt();
        }

        public long ToLong()
        {
            Assert.IsTrue(this.Length == 8);

            return BitConverter.ToInt64(this.bytes, 0);
        }

        public ulong ToULong()
        {
            return (uint)this.ToLong();
        }

        public float ToFloat()
        {
            Assert.IsTrue(this.Length == 4);

            return BitConverter.ToSingle(this.bytes, 0);
        }

        public double ToDouble()
        {
            Assert.IsTrue(this.Length == 8);

            return BitConverter.ToDouble(this.bytes, 0);
        }

        public char ToChar()
        {
            return (char)this.ToShort();
        }

        public override string ToString()
        {
            return System.Text.Encoding.UTF8.GetString(this.bytes);
        }
        #endregion

        #region Read Methods
        public byte[] Read(int length)
        {
            Assert.IsTrue(this.Length - this.location >= length);

            byte[] result = new byte[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = this.bytes[this.location];
                this.location++;
            }

            return result;
        }

        public bool ReadBool()
        {
            Assert.IsTrue(this.location != this.Length);

            byte resultByte = this.bytes[this.location];
            this.location++;
            return resultByte != 0;
        }

        public byte ReadByte()
        {
            Assert.IsTrue(this.location != this.Length);

            byte result = this.bytes[this.location];
            this.location++;
            return result;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)this.ReadByte();
        }

        public short ReadShort()
        {
            Assert.IsTrue(this.Length - this.location >= 2);

            short result = BitConverter.ToInt16(this.bytes, this.location);
            this.location += 2;
            return result;
        }

        public ushort ReadUShort()
        {
            return (ushort)this.ReadUShort();
        }

        public int ReadInt()
        {
            Assert.IsTrue(this.Length - this.location >= 4);

            int result = BitConverter.ToInt32(this.bytes, this.location);
            this.location += 4;
            return result;
        }

        public uint ReadUInt()
        {
            return (uint)this.ReadInt();
        }

        public long ReadLong()
        {
            Assert.IsTrue(this.Length - this.location >= 8);

            long result = BitConverter.ToInt64(this.bytes, this.location);
            this.location += 8;
            return result;
        }

        public ulong ReadULong()
        {
            return (ulong)this.ReadLong();
        }
        
        public float ReadFloat()
        {
            return BitConverter.ToSingle(this.Read(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(this.Read(8), 0);
        }

        public char ReadChar()
        {
            return (char)this.ReadShort();
        }

        public string ReadString(int length)
        {
            return System.Text.Encoding.UTF8.GetString(this.Read(length));
        }
        #endregion

        #region Read At Methods
        public byte[] ReadAt(int length, int address)
        {
            Assert.IsTrue(address >= 0 && this.Length - address > length);

            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = this.bytes[address++];
            }

            return result;
        }

        public bool ReadBoolAt(int address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);

            return this.bytes[address] != 0;
        }

        public byte ReadByteAt(int address)
        {
            Assert.IsTrue(address >= 0 && address < this.Length);

            return this.bytes[address];
        }

        public sbyte ReadSByteAt(int address)
        {
            return (sbyte)this.ReadByteAt(address);
        }

        public short ReadShortAt(int address)
        {
            return BitConverter.ToInt16(this.ReadAt(2, address), 0);
        }

        public ushort ReadUShortAt(int address)
        {
            return (ushort)this.ReadShortAt(address);
        }

        public int ReadIntAt(int address)
        {
            return BitConverter.ToInt32(this.ReadAt(4, address), 0);
        }

        public uint ReadUIntAt(int address)
        {
            return (uint)this.ReadIntAt(address);
        }

        public long ReadLongAt(int address)
        {
            return BitConverter.ToInt64(this.ReadAt(8, address), 0);
        }

        public ulong ReadULongAt(int address)
        {
            return (ulong)this.ReadLongAt(address);
        }

        public float ReadFloatAt(int address)
        {
            return BitConverter.ToSingle(this.ReadAt(4, address), 0);
        }

        public double ReadDoubleAt(int address)
        {
            return BitConverter.ToDouble(this.ReadAt(8, address), 0);
        }

        public char ReadCharAt(int address)
        {
            return (char)this.ToShort();
        }

        public string ReadStringAt(int length, int address)
        {
            byte[] bytes = this.ReadAt(length, address);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        #endregion

        #region Write Methods
        public void Write(byte[] bytes)
        {
            Assert.IsTrue(bytes.Length + this.location < this.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                this.bytes[this.location] = bytes[i];
                this.location++;
            }
        }

        public void WriteBool(bool value)
        {
            Assert.IsTrue(this.location + 1 < this.Length);

            if (value)
            {
                this.bytes[this.location] = 0xFF;
            }
            else
            {
                this.bytes[this.location] = 0x00;
            }

            this.location++;
        }

        public void WriteByte(byte value)
        {
            Assert.IsTrue(this.location + 1 < this.Length);

            this.bytes[this.location] = value;
            this.location++;
        }

        public void WriteSByte(sbyte value)
        {
            this.WriteByte((byte)value);
        }

        public void WriteShort(short value)
        {
            this.Write(BitConverter.GetBytes(value));
        }

        public void WriteUShort(ushort value)
        {
            this.WriteShort((short)value);
        }

        public void WriteInt(int value)
        {
            this.Write(BitConverter.GetBytes(value));
        }

        public void WriteUInt(uint value)
        {
            this.WriteInt((int)value);
        }

        public void WriteLong(long value)
        {
            this.Write(BitConverter.GetBytes(value));
        }

        public void WriteULong(ulong value)
        {
            this.WriteLong((long)value);
        }

        public void WriteFloat(float value)
        {
            this.Write(BitConverter.GetBytes(value));
        }

        public void WriteDouble(double value)
        {
            this.Write(BitConverter.GetBytes(value));
        }

        public void WriteChar(char value)
        {
            this.WriteShort((short)value);
        }

        public void WriteString(string value)
        {
            this.Write(System.Text.Encoding.UTF8.GetBytes(value));
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
    }
}