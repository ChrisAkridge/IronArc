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

			buffer = new byte[bufferSize];
			bufferTop = 0;
        }

        public byte[] Read(int length)
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (bufferTop < length - 1)
            {
                throw new Exception(string.Format("Tried to read past end of stream. Stream size: {0}, Read length: {1}.", bufferTop + 1, length));
            }

            int startIndex = bufferTop - (length - 1);
            int resultIndex = 0;
            byte[] result = new byte[length];

            for (int i = startIndex; i <= bufferTop; i++)
            {
                result[resultIndex] = buffer[i];
				buffer[i] = 0x00;
                resultIndex++;
            }

			bufferTop = startIndex - 1;
            return result;
        }

        public byte[] ReadToEnd()
        {
            if (bufferTop == -1)
            {
                return new byte[] { };
            }

            byte[] result = new byte[bufferTop + 1];
            for (int i = 0; i <= bufferTop; i++)
            {
                result[i] = buffer[i];
				buffer[i] = 0x00;
            }

			bufferTop = -1;
            return result;
        }

        public bool ReadBool()
        {
            return ReadByte() != 0;
        }

        public byte ReadByte()
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }

            byte result = buffer[bufferTop];
			buffer[bufferTop] = 0x00;
			bufferTop--;
            return result;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public short ReadShort()
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (bufferTop == 0)
            {
                throw new Exception("Cannot read short off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = Read(2);
            return BitConverter.ToInt16(bytes, 0);
        }

        public ushort ReadUShort()
        {
            return (ushort)ReadShort();
        }

        public int ReadInt()
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (bufferTop < 3)
            {
                throw new Exception("Cannot read int off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = Read(4);
            return BitConverter.ToInt32(bytes, 0);
        }

        public uint ReadUInt()
        {
            return (uint)ReadInt();
        }

        public long ReadLong()
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (bufferTop < 7)
            {
                throw new Exception("Cannot read long off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = Read(8);
            return BitConverter.ToInt64(bytes, 0);
        }

        public ulong ReadULong()
        {
            return (ulong)ReadLong();
        }

        public float ReadFloat()
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (bufferTop < 3)
            {
                throw new Exception("Cannot read float off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = Read(4);
            return BitConverter.ToSingle(bytes, 0);
        }

        public double ReadDouble()
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (bufferTop < 7)
            {
                throw new Exception("Cannot read double off of stack; there aren't enough bytes left.");
            }

            byte[] bytes = Read(8);
            return BitConverter.ToDouble(bytes, 0);
        }

        public decimal ReadDecimal()
        {
            if (bufferTop == -1)
            {
                throw new Exception("Cannot read empty stream.");
            }
            else if (bufferTop < 13)
            {
                throw new Exception("Cannot read decimal off of stack; there aren't enough bytes left.");
            }

            byte scale = ReadByte();
            byte isNegative = ReadByte();
            int high = ReadInt();
            int mid = ReadInt();
            int lo = ReadInt();

            return new decimal(lo, mid, high, isNegative == 0x80, scale);
        }

        public char ReadChar()
        {
            return (char)ReadShort();
        }

        public string ReadString(int length)
        {
            if (length > 0x7FFFFFFF)
            {
                throw new Exception("Cannot read string from stream - length is too large.");
            }

            byte[] bytes = Read(length);
            return System.Text.Encoding.UTF8.GetString(bytes, 0, length);
        }

        public byte[] Peek(int length)
        {
            if (length > 0x7FFFFFFF) 
            {
                throw new Exception("Cannot peek byte array from stream - length is too large.");
            }

            if (length > bufferTop)
            {
                throw new Exception("Cannot peek byte array from stream - buffer not large enough.");
            }

            int startIndex = bufferTop - length;
            int i = 0;
            byte[] result = new byte[length];

            while (length > 0)
            {
                result[i] = buffer[startIndex];
                startIndex++;
                length--;
                i++;
            }

            return result;
        }

        public byte PeekByte()
        {
            return Peek(1)[0];
        }

        public sbyte PeekSByte()
        {
            return (sbyte)Peek(1)[0];
        }

        public short PeekShort()
        {
            byte[] bytes = Peek(2);
            return (short)((bytes[0] << 8) + bytes[1]);
        }

        public ushort PeekUShort()
        {
            return (ushort)PeekShort();
        }

        public int PeekInt()
        {
            byte[] bytes = Peek(4);
            return (int)((bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3]);
        }

        public uint PeekUInt()
        {
            return (uint)PeekInt();
        }

        public long PeekLong()
        {
            byte[] bytes = Peek(8);
            long result = 0L;

            for (int i = 56; i >= 0; i -= 8)
            {
                result += bytes[7 - (i / 8)] << i;
            }

            return result;
        }

        public ulong PeekULong()
        {
            return (ulong)PeekLong();
        }

        public float PeekFloat()
        {
            byte[] bytes = Peek(4);
            return BitConverter.ToSingle(bytes, 0);
        }

        public double PeekDouble()
        {
            byte[] bytes = Peek(8);
            return BitConverter.ToDouble(bytes, 0);
        }

        public char PeekChar()
        {
            return (char)PeekShort();
        }

        public string PeekString(int length)
        {
            if (length > 0x7FFFFFFF)
            {
                throw new Exception("Cannot read string from stream - length is too large.");
            }

            byte[] bytes = Peek(length);
            return System.Text.Encoding.UTF8.GetString(bytes, 0, length);
        }

        public void Write(byte[] bytes)
        {
			ThrowIfWriteCausesOverflow(bytes.Length);

			bufferTop++;
            for (int i = 0; i < bytes.Length; i++)
            {
				buffer[bufferTop] = bytes[i];
				bufferTop++;
            }

			OnNewData();
        }

        public void WriteBool(bool value)
        {
            if (value)
            {
				WriteByte(1);
            }
            else
            {
				WriteByte(0);
            }

			OnNewData();
        }

        public void WriteByte(byte value)
        {
			ThrowIfWriteCausesOverflow(1);

			bufferTop++;
			buffer[bufferTop] = value;

			OnNewData();
        }

        public void WriteSByte(sbyte value)
        {
			WriteByte((byte)value);
        }

        public void WriteShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
			Write(bytes);
        }

        public void WriteUShort(ushort value)
        {
			WriteShort((short)value);
        }

        public void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
			Write(bytes);
        }

        public void WriteUInt(uint value)
        {
			WriteInt((int)value);
        }

        public void WriteLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
			Write(bytes);
        }

        public void WriteULong(ulong value)
        {
			WriteLong((long)value);
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
			Write(bytes);
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
			Write(bytes);
        }

        public void WriteChar(char value)
        {
			WriteShort((short)value);
        }

        public void WriteString(string value)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
			Write(bytes);
        }

        private void ThrowIfWriteCausesOverflow(int dataLength)
        {
            if (bufferTop + dataLength > buffer.Length - 1)
            {
                throw new Exception(string.Format("Stream buffer overflow. Current buffer size is {0}, max buffer size is {1}, input size is {2}.", bufferTop + 1, buffer.Length, dataLength));
            }
        }

        protected void OnNewData()
        {
            if (NewData != null)
            {
				NewData(this);
            }
        }
    }

    public delegate void NewDataEventHandler(Stream sender);
}
