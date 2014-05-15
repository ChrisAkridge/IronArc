using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// Represents a stream of bytes that can be written to or read from.
    /// </summary>
    public class Stream
    {
        private byte[] buffer;
        private int bufferTop = -1;

        public event NewDataEventHandler NewData;

        public Stream(int bufferSize)
        {
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid stream buffer size {0}.", bufferSize));
            }

            this.buffer = new byte[bufferSize];
            this.bufferTop = 0;
        }

        public byte[] Read(int length)
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (this.bufferTop < length - 1)
            {
                throw new Exception(string.Format("Tried to read past end of stream. Stream size: {0}, Read length: {1}.", this.bufferTop + 1, length));
            }

            int startIndex = this.bufferTop - (length - 1);
            int resultIndex = 0;
            byte[] result = new byte[length];

            for (int i = startIndex; i <= this.bufferTop; i++)
            {
                result[resultIndex] = this.buffer[i];
                this.buffer[i] = 0x00;
                resultIndex++;
            }

            this.bufferTop = startIndex - 1;
            return result;
        }

        public byte[] ReadToEnd()
        {
            if (this.bufferTop == -1)
            {
                return new byte[] { };
            }

            byte[] result = new byte[this.bufferTop + 1];
            for (int i = 0; i <= this.bufferTop; i++)
            {
                result[i] = this.buffer[i];
                this.buffer[i] = 0x00;
            }

            this.bufferTop = -1;
            return result;
        }

        public bool ReadBool()
        {
            return this.ReadByte() != 0;
        }

        public byte ReadByte()
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }

            byte result = this.buffer[this.bufferTop];
            this.buffer[this.bufferTop] = 0x00;
            this.bufferTop--;
            return result;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)this.ReadByte();
        }

        public short ReadShort()
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (this.bufferTop == 0)
            {
                throw new Exception("Cannot read short off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = this.Read(2);
            return BitConverter.ToInt16(bytes, 0);
        }

        public ushort ReadUShort()
        {
            return (ushort)this.ReadShort();
        }

        public int ReadInt()
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (this.bufferTop < 3)
            {
                throw new Exception("Cannot read int off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = this.Read(4);
            return BitConverter.ToInt32(bytes, 0);
        }

        public uint ReadUInt()
        {
            return (uint)this.ReadInt();
        }

        public long ReadLong()
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (this.bufferTop < 7)
            {
                throw new Exception("Cannot read long off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = this.Read(8);
            return BitConverter.ToInt64(bytes, 0);
        }

        public ulong ReadULong()
        {
            return (ulong)this.ReadLong();
        }

        public float ReadFloat()
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (this.bufferTop < 3)
            {
                throw new Exception("Cannot read float off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = this.Read(4);
            return BitConverter.ToSingle(bytes, 0);
        }

        public double ReadDouble()
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (this.bufferTop < 7)
            {
                throw new Exception("Cannot read double off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = this.Read(8);
            return BitConverter.ToDouble(bytes, 0);
        }

        public decimal ReadDecimal()
        {
            if (this.bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (this.bufferTop < 13)
            {
                throw new Exception("Cannot read decimal off of stack; there aren't enough bytes left.");
            }

            byte scale = this.ReadByte();
            byte isNegative = this.ReadByte();
            int high = this.ReadInt();
            int mid = this.ReadInt();
            int lo = this.ReadInt();

            return new decimal(lo, mid, high, isNegative == 0x80, scale);
        }

        public char ReadChar()
        {
            return (char)this.ReadShort();
        }

        public string ReadString(int length)
        {
            if (length > 0x7FFFFFFF)
            {
                throw new Exception("Cannot read string from stream - length is too large.");
            }

            byte[] bytes = this.Read(length);
            return System.Text.Encoding.UTF8.GetString(bytes, 0, length);
        }

        public byte[] Peek(int length)
        {
            if (length > 0x7FFFFFFF) 
            {
                throw new Exception("Cannot peek byte array from stream - length is too large.");
            }

            if (length > this.bufferTop)
            {
                throw new Exception("Cannot peek byte array from stream - buffer not large enough.");
            }

            int startIndex = this.bufferTop - length;
            int i = 0;
            byte[] result = new byte[length];

            while (length > 0)
            {
                result[i] = this.buffer[startIndex];
                startIndex++;
                length--;
                i++;
            }

            return result;
        }

        public byte PeekByte()
        {
            return this.Peek(1)[0];
        }

        public sbyte PeekSByte()
        {
            return (sbyte)this.Peek(1)[0];
        }

        public short PeekShort()
        {
            byte[] bytes = this.Peek(2);
            return (short)((bytes[0] << 8) + bytes[1]);
        }

        public ushort PeekUShort()
        {
            return (ushort)this.PeekShort();
        }

        public int PeekInt()
        {
            byte[] bytes = this.Peek(4);
            return (int)((bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3]);
        }

        public uint PeekUInt()
        {
            return (uint)this.PeekInt();
        }

        public long PeekLong()
        {
            byte[] bytes = this.Peek(8);
            long result = 0L;

            for (int i = 56; i >= 0; i -= 8)
            {
                result += bytes[7 - (i / 8)] << i;
            }

            return result;
        }

        public ulong PeekULong()
        {
            return (ulong)this.PeekLong();
        }

        public float PeekFloat()
        {
            byte[] bytes = this.Peek(4);
            return BitConverter.ToSingle(bytes, 0);
        }

        public double PeekDouble()
        {
            byte[] bytes = this.Peek(8);
            return BitConverter.ToDouble(bytes, 0);
        }

        public char PeekChar()
        {
            return (char)this.PeekShort();
        }

        public string PeekString(int length)
        {
            if (length > 0x7FFFFFFF)
            {
                throw new Exception("Cannot read string from stream - length is too large.");
            }

            byte[] bytes = this.Peek(length);
            return System.Text.Encoding.UTF8.GetString(bytes, 0, length);
        }

        public void Write(byte[] bytes)
        {
            this.ThrowIfWriteCausesOverflow(bytes.Length);

            this.bufferTop++;
            for (int i = 0; i < bytes.Length; i++)
            {
                this.buffer[this.bufferTop] = bytes[i];
                this.bufferTop++;
            }

            this.OnNewData();
        }

        public void WriteBool(bool value)
        {
            if (value)
            {
                this.WriteByte(1);
            }
            else
            {
                this.WriteByte(0);
            }

            this.OnNewData();
        }

        public void WriteByte(byte value)
        {
            this.ThrowIfWriteCausesOverflow(1);

            this.bufferTop++;
            this.buffer[this.bufferTop] = value;

            this.OnNewData();
        }

        public void WriteSByte(sbyte value)
        {
            this.WriteByte((byte)value);
        }

        public void WriteShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.Write(bytes);
        }

        public void WriteUShort(ushort value)
        {
            this.WriteShort((short)value);
        }

        public void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.Write(bytes);
        }

        public void WriteUInt(uint value)
        {
            this.WriteInt((int)value);
        }

        public void WriteLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.Write(bytes);
        }

        public void WriteULong(ulong value)
        {
            this.WriteLong((long)value);
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.Write(bytes);
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.Write(bytes);
        }

        public void WriteChar(char value)
        {
            this.WriteShort((short)value);
        }

        public void WriteString(string value)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
            this.Write(bytes);
        }

        private void ThrowIfWriteCausesOverflow(int dataLength)
        {
            if (this.bufferTop + dataLength > this.buffer.Length - 1)
            {
                throw new Exception(string.Format("Stream buffer overflow. Current buffer size is {0}, max buffer size is {1}, input size is {2}.", this.bufferTop + 1, this.buffer.Length, dataLength));
            }
        }

        protected void OnNewData()
        {
            if (this.NewData != null)
            {
                this.NewData(this);
            }
        }
    }

    public delegate void NewDataEventHandler(Stream sender);
}
