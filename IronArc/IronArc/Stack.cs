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

			bytes = ByteBlock.FromLength((ulong)stackSize);
        }

        private int GetObjectSizeAtIndex(int index)
        {
            var field = objectSizes.GetType().GetField("_array", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return ((int[])field.GetValue(objectSizes))[index];
        }

        public byte[] GetObject(int objectIndex)
        {
            objectIndex = objectSizes.Count - objectIndex;

            uint objectStartPointer = 0u;
            uint objectSize = (uint)objectSizes.ElementAt(objectIndex);

            for (int i = 0; i < objectIndex; i++)
            {
                objectStartPointer += (uint)objectSizes.ElementAt(i);
            }

            return bytes.ReadAt(objectSize, objectStartPointer);
        }

        public byte GetByte(int index)
        {
            index = stackPointer - index;
            return bytes[(ulong)index];
        }

        #region Push Methods
        public void Push(byte[] bytes)
        {
			objectSizes.Push(bytes.Length);
            this.bytes.WriteAt(bytes, (ulong)stackPointer);
			stackPointer += bytes.Length;
        }

        public void Push(ByteBlock bytes)
        {
			objectSizes.Push((int)bytes.Length);
            this.bytes.WriteAt(bytes, (ulong)stackPointer);
			stackPointer += (int)bytes.Length;
        }

        public void Push(bool value)
        {
			Push(value ? (byte)1 : (byte)0);
        }

        public void Push(byte value)
        {
			objectSizes.Push(1);
			bytes.WriteByteAt(value, (ulong)stackPointer);
			stackPointer++;
        }

        public void Push(sbyte value)
        {
			Push((byte)value);
        }

        public void Push(short value)
        {
			objectSizes.Push(2);
			bytes.WriteShortAt(value, (ulong)stackPointer);
			stackPointer += 2;
        }

        public void Push(ushort value)
        {
			Push((short)value);
        }

        public void Push(int value)
        {
			objectSizes.Push(4);
			bytes.WriteIntAt(value, (ulong)stackPointer);
			stackPointer += 4;
        }

        public void Push(uint value)
        {
			Push((int)value);
        }

        public void Push(long value)
        {
			objectSizes.Push(8);
			bytes.WriteLongAt(value, (ulong)stackPointer);
			stackPointer += 8;
        }

        public void Push(ulong value)
        {
			Push((long)value);
        }

        public void Push(float value)
        {
			objectSizes.Push(4);
			bytes.WriteFloatAt(value, (ulong)stackPointer);
			stackPointer += 4;
        }

        public void Push(double value)
        {
			objectSizes.Push(8);
			bytes.WriteDoubleAt(value, (ulong)stackPointer);
			stackPointer += 8;
        }

        public void Push(char value)
        {
			Push((short)value);
        }

        public void Push(string value)
        {
			Push(Encoding.UTF8.GetBytes(value));
        }
        #endregion

        #region Pop Methods
        public byte[] Pop()
        {
            int objectSize = objectSizes.Pop();
            int startPointer = stackPointer - objectSize;
            byte[] result = new byte[objectSize];

            for (int i = 0; i < objectSize; i++)
            {
                result[i] = bytes[(ulong)startPointer];
				bytes[(ulong)startPointer] = 0;
                startPointer++;
            }

            stackPointer -= objectSize;
            return result;
        }

        public ByteBlock PopByteBlock()
        {
            byte[] resultBytes = Pop();
            return new ByteBlock(resultBytes);
        }

        public bool PopBool()
        {
            return PopByte() != 0;
        }

        public byte PopByte()
        {
			objectSizes.Pop();
			stackPointer--;
            byte result = bytes.ReadByteAt((uint)stackPointer);
			bytes.WriteByteAt(0, (ulong)stackPointer);
            return result;
        }

        public sbyte PopSByte()
        {
            return (sbyte)PopByte();
        }

        public short PopShort()
        {
			objectSizes.Pop();
			stackPointer -= 2;
			short result = bytes.ReadShortAt((uint)stackPointer);
			bytes.WriteShortAt(0, (ulong)stackPointer);
            return result;
        }

        public ushort PopUShort()
        {
            return (ushort)PopShort();
        }

        public int PopInt()
        {
			objectSizes.Pop();
			stackPointer -= 4;
			int result = bytes.ReadIntAt((uint)stackPointer);
			bytes.WriteIntAt(0, (ulong)stackPointer);
            return result;
        }

        public uint PopUInt()
        {
            return (uint)PopInt();
        }

        public long PopLong()
        {
			objectSizes.Pop();
			stackPointer -= 8;
			long result = bytes.ReadLongAt((uint)stackPointer);
			bytes.WriteLongAt(0L, (ulong)stackPointer);
            return result;
        }

        public ulong PopULong()
        {
            return (ulong)PopLong();
        }

        public float PopFloat()
        {
			objectSizes.Pop();
			stackPointer -= 4;
			float result = bytes.ReadFloatAt((uint)stackPointer);
			bytes.WriteIntAt(0, (ulong)stackPointer);
            return result;
        }

        public double PopDouble()
        {
			objectSizes.Pop();
			stackPointer -= 8;
			double result = bytes.ReadDoubleAt((uint)stackPointer);
			bytes.WriteLongAt(0L, (ulong)stackPointer);
            return result;
        }

        public char PopChar()
        {
            return (char)PopShort();
        }

        public string PopString()
        {
            byte[] stringBytes = Pop();
            return Encoding.UTF8.GetString(stringBytes);
        }
        #endregion

        #region Peek Methods
        public byte[] Peek()
        {
            int objectSize = objectSizes.Peek();
            int startPointer = stackPointer - objectSize;
			return bytes.ReadAt((uint)objectSize, (uint)startPointer);
        }

        public ByteBlock PeekByteBlock()
        {
            return new ByteBlock(Peek());
        }

        public bool PeekBool()
        {
            return PeekByte() != 0;
        }

        public byte PeekByte()
        {
			return bytes.ReadByteAt((uint)stackPointer - 1);
        }

        public sbyte PeekSByte()
        {
            return (sbyte)PeekByte();
        }

        public short PeekShort()
        {
			return bytes.ReadShortAt((uint)stackPointer - 2);
        }

        public ushort PeekUShort()
        {
            return (ushort)PeekShort();
        }

        public int PeekInt()
        {
			return bytes.ReadIntAt((uint)stackPointer - 4);
        }

        public uint PeekUInt()
        {
            return (uint)PeekInt();
        }

        public long PeekLong()
        {
			return bytes.ReadLongAt((uint)stackPointer - 8);
        }

        public ulong PeekULong()
        {
            return (ulong)PeekLong();
        }

        public float PeekFloat()
        {
			return bytes.ReadFloatAt((uint)stackPointer - 4);
        }

        public double PeekDouble()
        {
			return bytes.ReadDoubleAt((uint)stackPointer - 8);
        }

        public char PeekChar()
        {
            return (char)PeekShort();
        }

        public string PeekString()
        {
            int stringSize = objectSizes.Peek();
			return bytes.ReadStringAt((uint)stringSize, (uint)(stackPointer - stringSize));
        }
        #endregion
    }
}
