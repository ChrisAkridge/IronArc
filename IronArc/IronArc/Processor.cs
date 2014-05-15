using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// An instance of an IronArc processor.
    /// </summary>
    public sealed class Processor
    {
        // Registers
        public ByteBlock EAX { get; set; }
        public ByteBlock EBX { get; set; }
        public ByteBlock ECX { get; set; }
        public ByteBlock EDX { get; set; }
        public ByteBlock EEX { get; set; }
        public ByteBlock EFX { get; set; }
        public ByteBlock EGX { get; set; }
        public ByteBlock EHX { get; set; }

        // System memory
        public ByteBlock Memory { get; set; }

        // System stack
        public Stack<long> Stack { get; set; }

        // Instruction pointer
        public int InstructionPointer { get; set; }

        // Diskfile
        public Diskfile Diskfile { get; set; }

        // Call stack
        public Stack<int> CallStack { get; set; }

        public Processor(int memorySize)
        {
            this.EAX = ByteBlock.FromLength(8);
            this.EBX = ByteBlock.FromLength(8);
            this.ECX = ByteBlock.FromLength(8);
            this.EDX = ByteBlock.FromLength(8);
            this.EEX = ByteBlock.FromLength(8);
            this.EFX = ByteBlock.FromLength(8);
            this.EGX = ByteBlock.FromLength(8);
            this.EHX = ByteBlock.FromLength(8);

            this.Memory = ByteBlock.FromLength(memorySize);
            this.Stack = new Stack<long>();
            this.CallStack = new Stack<int>();
        }

        #region Memory Read Methods
        public byte ReadByte()
        {
            return this.Memory[this.InstructionPointer++];
        }

        public sbyte ReadSByte()
        {
            return (sbyte)this.ReadByte();
        }

        public short ReadShort()
        {
            short result = this.Memory.ReadShortAt(this.InstructionPointer);
            this.InstructionPointer += 2;
            return result;
        }

        public ushort ReadUShort()
        {
            return (ushort)this.ReadShort();
        }

        public int ReadInt()
        {
            int result = this.Memory.ReadIntAt(this.InstructionPointer);
            this.InstructionPointer += 4;
            return result;
        }

        public uint ReadUInt()
        {
            return (uint)this.ReadInt();
        }

        public long ReadLong()
        {
            long result = this.Memory.ReadLongAt(this.InstructionPointer);
            this.InstructionPointer += 8;
            return result;
        }

        public ulong ReadULong()
        {
            return (ulong)this.ReadLong();
        }

        public float ReadFloat()
        {
            float result = this.Memory.ReadFloatAt(this.InstructionPointer);
            this.InstructionPointer += 4;
            return result;
        }

        public double ReadDouble()
        {
            double result = this.Memory.ReadDoubleAt(this.InstructionPointer);
            this.InstructionPointer += 8;
            return result;
        }

        public char ReadChar()
        {
            return (char)this.ReadShort();
        }

        public ByteBlock Read(int length)
        {
            ByteBlock result = this.Memory.ReadAt(length, this.InstructionPointer);
            this.InstructionPointer += length;
            return result;
        }
        #endregion

        #region Register Read/Write
        public ByteBlock ReadRegister(byte register)
        {
            switch (register)
            {
                case 0x00:
                    return this.EAX;
                case 0x01:
                    return this.EBX;
                case 0x02:
                    return this.ECX;
                case 0x03:
                    return this.EDX;
                case 0x04:
                    return this.EEX;
                case 0x05:
                    return this.EFX;
                case 0x06:
                    return this.EGX;
                case 0x07:
                    return this.EHX;
                case 0x08:
                    return new ByteBlock(this.InstructionPointer);
                case 0x09:
                    return new ByteBlock((byte)0);
                default:
                    new SystemError("InvalidProcessorRegister", string.Format("Tried to read from invalid processor register 0x{0:X2}."));
                    break;
            }

            return null;
        }

        public void WriteRegister(byte register, ByteBlock value)
        {
            if (register >= 0x00 && register <= 0x07)
            {
                if (value.Length > 8)
                {
                    new SystemError("InvalidWriteLength", string.Format("Cannot write a {0}-byte value to a standard 8-byte register.", value.Length)).WriteToError();
                }
            }
            else if (register == 0x08)
            {
                if (value.Length > 4)
                {
                    new SystemError("InvalidWriteLength", string.Format("Cannot write a {0}-byte value to the 4-byte instruction pointer.", value.Length)).WriteToError();
                }
            }

            switch (register)
            {
                case 0x00:
                    this.EAX = 0UL;
                    this.EAX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x01:
                    this.EBX = 0UL;
                    this.EBX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x02:
                    this.ECX = 0UL;
                    this.ECX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x03:
                    this.EDX = 0UL;
                    this.EDX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x04:
                    this.EEX = 0UL;
                    this.EEX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x05:
                    this.EFX = 0UL;
                    this.EFX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x06:
                    this.EGX = 0UL;
                    this.EGX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x07:
                    this.EHX = 0UL;
                    this.EHX.WriteAt(value, 8 - value.Length);
                    break;
                case 0x08:
                    this.InstructionPointer = value.ToInt();
                    break;
                case 0x09:
                    break;
            }
        }
        #endregion
    }
}
