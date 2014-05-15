using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    public sealed class Stack
    {
        private ByteBlock bytes;
        private Stack<int> objectSizes = new Stack<int>();
        private int stackPointer = 0;

        public Stack(int stackSize)
        {
            if (stackSize <= 0)
            {
                new SystemError("InvalidStackSize", "Stack size must be greater than zero.").WriteToError();
            }

            this.bytes = ByteBlock.FromLength(stackSize);
        }

        private int GetObjectSizeAtIndex(int index)
        {
            var field = this.objectSizes.GetType().GetField("_array", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return ((int[])field.GetValue(this.objectSizes))[index];
        }

        public byte[] GetObject(int objectIndex)
        {
            objectIndex = this.objectSizes.Count - objectIndex;

            int objectStartPointer = 0;
            int objectSize = this.objectSizes.ElementAt(objectIndex);

            for (int i = 0; i < objectIndex; i++)
            {
                objectStartPointer += this.objectSizes.ElementAt(i);
            }

            return this.bytes.ReadAt(objectSize, objectStartPointer);
        }

        public byte GetByte(int index)
        {
            index = this.stackPointer - index;
            return this.bytes[index];
        }

        #region Push Methods
        public void Push(byte[] bytes)
        {
            this.objectSizes.Push(bytes.Length);
            this.bytes.WriteAt(bytes, this.stackPointer);
            this.stackPointer += bytes.Length;
        }

        public void Push(ByteBlock bytes)
        {
            this.objectSizes.Push(bytes.Length);
            this.bytes.WriteAt(bytes, this.stackPointer);
            this.stackPointer += bytes.Length;
        }

        public void Push(bool value)
        {
            this.Push(value ? (byte)1 : (byte)0);
        }

        public void Push(byte value)
        {
            this.objectSizes.Push(1);
            this.bytes.WriteByteAt(value, this.stackPointer);
            this.stackPointer++;
        }

        public void Push(sbyte value)
        {
            this.Push((byte)value);
        }

        public void Push(short value)
        {
            this.objectSizes.Push(2);
            this.bytes.WriteShortAt(value, this.stackPointer);
            this.stackPointer += 2;
        }

        public void Push(ushort value)
        {
            this.Push((short)value);
        }

        public void Push(int value)
        {
            this.objectSizes.Push(4);
            this.bytes.WriteIntAt(value, this.stackPointer);
            this.stackPointer += 4;
        }

        public void Push(uint value)
        {
            this.Push((int)value);
        }

        public void Push(long value)
        {
            this.objectSizes.Push(8);
            this.bytes.WriteLongAt(value, this.stackPointer);
            this.stackPointer += 8;
        }

        public void Push(ulong value)
        {
            this.Push((long)value);
        }

        public void Push(float value)
        {
            this.objectSizes.Push(4);
            this.bytes.WriteFloatAt(value, this.stackPointer);
            this.stackPointer += 4;
        }

        public void Push(double value)
        {
            this.objectSizes.Push(8);
            this.bytes.WriteDoubleAt(value, this.stackPointer);
            this.stackPointer += 8;
        }

        public void Push(char value)
        {
            this.Push((short)value);
        }

        public void Push(string value)
        {
            this.Push(Encoding.UTF8.GetBytes(value));
        }
        #endregion

        #region Pop Methods
        public byte[] Pop()
        {
            int objectSize = this.objectSizes.Pop();
            int startPointer = this.stackPointer - objectSize;
            byte[] result = new byte[objectSize];

            for (int i = 0; i < objectSize; i++)
            {
                result[i] = this.bytes[startPointer];
                this.bytes[startPointer] = 0;
                startPointer++;
            }

            stackPointer -= objectSize;
            return result;
        }

        public ByteBlock PopByteBlock()
        {
            byte[] resultBytes = this.Pop();
            return new ByteBlock(resultBytes);
        }

        public bool PopBool()
        {
            return this.PopByte() != 0;
        }

        public byte PopByte()
        {
            this.objectSizes.Pop();
            this.stackPointer--;
            byte result = this.bytes.ReadByteAt(this.stackPointer);
            this.bytes.WriteByteAt(0, this.stackPointer);
            return result;
        }

        public sbyte PopSByte()
        {
            return (sbyte)this.PopByte();
        }

        public short PopShort()
        {
            this.objectSizes.Pop();
            this.stackPointer -= 2;
            short result = this.bytes.ReadShortAt(this.stackPointer);
            this.bytes.WriteShortAt(0, this.stackPointer);
            return result;
        }

        public ushort PopUShort()
        {
            return (ushort)this.PopShort();
        }

        public int PopInt()
        {
            this.objectSizes.Pop();
            this.stackPointer -= 4;
            int result = this.bytes.ReadIntAt(this.stackPointer);
            this.bytes.WriteIntAt(0, this.stackPointer);
            return result;
        }

        public uint PopUInt()
        {
            return (uint)this.PopInt();
        }

        public long PopLong()
        {
            this.objectSizes.Pop();
            this.stackPointer -= 8;
            long result = this.bytes.ReadLongAt(this.stackPointer);
            this.bytes.WriteLongAt(0L, this.stackPointer);
            return result;
        }

        public ulong PopULong()
        {
            return (ulong)this.PopLong();
        }

        public float PopFloat()
        {
            this.objectSizes.Pop();
            this.stackPointer -= 4;
            float result = this.bytes.ReadFloatAt(this.stackPointer);
            this.bytes.WriteIntAt(0, this.stackPointer);
            return result;
        }

        public double PopDouble()
        {
            this.objectSizes.Pop();
            this.stackPointer -= 8;
            double result = this.bytes.ReadDoubleAt(this.stackPointer);
            this.bytes.WriteLongAt(0L, this.stackPointer);
            return result;
        }

        public char PopChar()
        {
            return (char)this.PopShort();
        }

        public string PopString()
        {
            byte[] stringBytes = this.Pop();
            return Encoding.UTF8.GetString(stringBytes);
        }
        #endregion

        #region Peek Methods
        public byte[] Peek()
        {
            int objectSize = this.objectSizes.Peek();
            int startPointer = this.stackPointer - objectSize;
            return this.bytes.ReadAt(objectSize, startPointer);
        }

        public ByteBlock PeekByteBlock()
        {
            return new ByteBlock(this.Peek());
        }

        public bool PeekBool()
        {
            return this.PeekByte() != 0;
        }

        public byte PeekByte()
        {
            return this.bytes.ReadByteAt(this.stackPointer - 1);
        }

        public sbyte PeekSByte()
        {
            return (sbyte)this.PeekByte();
        }

        public short PeekShort()
        {
            return this.bytes.ReadShortAt(this.stackPointer - 2);
        }

        public ushort PeekUShort()
        {
            return (ushort)this.PeekShort();
        }

        public int PeekInt()
        {
            return this.bytes.ReadIntAt(this.stackPointer - 4);
        }

        public uint PeekUInt()
        {
            return (uint)this.PeekInt();
        }

        public long PeekLong()
        {
            return this.bytes.ReadLongAt(this.stackPointer - 8);
        }

        public ulong PeekULong()
        {
            return (ulong)this.PeekLong();
        }

        public float PeekFloat()
        {
            return this.bytes.ReadFloatAt(this.stackPointer - 4);
        }

        public double PeekDouble()
        {
            return this.bytes.ReadDoubleAt(this.stackPointer - 8);
        }

        public char PeekChar()
        {
            return (char)this.PeekShort();
        }

        public string PeekString()
        {
            int stringSize = this.objectSizes.Peek();
            return this.bytes.ReadStringAt(stringSize, this.stackPointer - stringSize);
        }
        #endregion
    }
}
