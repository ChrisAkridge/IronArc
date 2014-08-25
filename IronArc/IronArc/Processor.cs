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

        // Flags register

        public ByteBlock EFLAGS { get; set; }

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

            this.EFLAGS = ByteBlock.FromLength(4);

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

        #region Opcodes
        // Each opcode has a value in one of these enums, which is the value of its second byte
        private enum CFOpcode : byte
        {
            NOP = 0x00,
            JMP = 0X01,
            JMPA = 0X02,
            JE = 0X03,
		    JNE = 0x04,
		    JLT = 0x05,
		    JGT = 0x06,
		    JLTE = 0x07,
		    JGTE = 0x08,
		    JZ = 0x09,
		    JNZ = 0x0A,
	    	CALL = 0x0B,
    		CALLA = 0x0C,
		    RET = 0x0D,
		    END = 0x0E
        };

        private enum DataOpcode : byte
        {
            MOV = 0x00,
            ADD = 0x01,
            SUB = 0x02,
            MULT = 0x03,
            DIV = 0x04,
            MOD = 0x05,
            INV = 0x06,
            EQ = 0x07,
            INEQ = 0x08,
            LT = 0x09,
            GT = 0x0A,
            LTEQ = 0x0B,
            GTEQ = 0x0C,
            AND = 0x0D,
            OR = 0x0E,
            NOT = 0x0F,
            BWNOT = 0x10,
            BWAND = 0x11,
            BWOR = 0x12,
            BWXOR = 0x13,
            BWLSHIFT = 0x14,
            BWRSHIFT = 0x15,
            ADDL = 0x40,
            SUBL = 0x41,
            MULTL = 0x42,
            DIVL = 0x43,
            MODL = 0x44,
            INVL = 0x45,
            EQL = 0x46,
            INEQL = 0x47,
            LTL = 0x48,
            GTL = 0x49,
            LTEQL = 0x4A,
            GTEQL = 0x4B,
            ANDL = 0x4C,
            ORL = 0x4D,
            NOTL = 0x4E,
            BWNOTL = 0x4F,
            BWANDL = 0x50,
            BWORL = 0x51,
            BWXORL = 0x52,
            BWLSHIFTL = 0x53,
            BWRSHIFTL = 0x54,
            PUSH = 0x80,
            POP = 0x81,
            PEEK = 0x82,
            STACKALLOC = 0x83,
            ARRAYALLOC = 0x84,
            DEREF = 0x85,
            ARRAYACCESS = 0x86,
            CBYTE = 0xA0,
            CSBYTE = 0xA1,
            CSHORT = 0xA2,
            CUSHORT = 0xA3,
            CINT = 0xA4,
            CUINT = 0xA5,
            CLONG = 0xA6,
            CULONG = 0xA7,
            CSING = 0xA8,
            CDOUBLE = 0xA9
        };

        private enum SysOpcode : byte
        {
            HWCALL = 0x00,
            SETFLAG = 0x10,
            CLEARFLAG = 0x11,
            TESTFLAG = 0x12,
            TOGGLEFLAG = 0x13
        };
        #endregion

        #region Execution
        public void Execute()
        {
            ushort fullopcode = this.ReadUShort();
            byte opcode = (byte)((fullopcode << 8) >> 8);
            ushort arity = 0;
            switch (fullopcode>>8)
            {
                case 0x00: // Control flow instruction
                    CFOpcode cfopcode = (CFOpcode)opcode;
                    switch (cfopcode)
                    {
                        case CFOpcode.NOP:
                        case CFOpcode.RET:
                        case CFOpcode.END: // no operands
                            break;
                        case CFOpcode.JMP:
                        case CFOpcode.JMPA:
                        case CFOpcode.CALL:
                        case CFOpcode.CALLA: // one operand, etc
                            arity = 1;
                            break;
                        case CFOpcode.JZ:
                        case CFOpcode.JNZ:
                            arity = 2;
                            break;
                        case CFOpcode.JE:
                        case CFOpcode.JNE:
                        case CFOpcode.JLT:
                        case CFOpcode.JGT:
                        case CFOpcode.JLTE:
                        case CFOpcode.JGTE:
                            arity = 3;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(); // will likely change
                    }
                    break;
                case 0x01: // data instructions
                    DataOpcode dopcode = (DataOpcode)opcode;
                    switch (dopcode & (DataOpcode)0xE0) // seems only the top 3 bits are needed
                    {
                        case (DataOpcode)0x00: // stack arithmetic (or a move)
                            if (dopcode == DataOpcode.MOV) { arity = 2; }
                            break;
                        case (DataOpcode)0x40: // long arithmetic
                            if (dopcode == DataOpcode.INVL || dopcode == DataOpcode.NOTL || dopcode == DataOpcode.BWNOTL)
                            {
                                arity = 2;
                            }
                            else
                            {
                                arity = 3;
                            }
                            break;
                        case (DataOpcode)0x80: // stack operations
                            if (dopcode == DataOpcode.ARRAYALLOC)
                            {
                                arity = 2;
                            }
                            else
                            {
                                arity = 1;
                            }
                            break;
                        case (DataOpcode)0xA0: // conversions
                            arity = 2;
                            break;
                    }
                    break;
                case 0x02: // system instructions
                    SysOpcode sopcode = (SysOpcode)opcode;
                    arity = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (arity > 0)
            {
                object[] operands = new object[3];
                byte flags = this.ReadByte();
                int i;
                for (i = 1; i <= arity; i++)
                {
                    switch (flags >> 6)
                    {
                        case 0: // memory addr
                            operands[i] = new AddressBlock(this); // addressblock code to be redone
                            break;
                        case 1: // register
                            byte register = this.ReadByte();
                            if ((register & 0x80) != 0x00)
                            {
                                // add later
                            }
                            else
                            {
                                if (register > 0x0A) { throw new ArgumentOutOfRangeException(); }
                                operands[i] = register;
                            }
                            break;
                        case 2: // stack index
                            operands[i] = this.ReadUInt();
                            break;
                        case 3: // literal
                            byte type = this.ReadByte();
                            switch (type)
                            {
                                case 0x00:
                                    operands[i] = this.ReadByte();
                                    break;
                                case 0x01:
                                    operands[i] = this.ReadSByte();
                                    break;
                                case 0x02:
                                    operands[i] = this.ReadShort();
                                    break;
                                case 0x03:
                                    operands[i] = this.ReadUShort();
                                    break;
                                case 0x04:
                                    operands[i] = this.ReadInt();
                                    break;
                                case 0x05:
                                    operands[i] = this.ReadUInt();
                                    break;
                                case 0x06:
                                    operands[i] = this.ReadLong();
                                    break;
                                case 0x07:
                                    operands[i] = this.ReadULong();
                                    break;
                                case 0x08:
                                    operands[i] = this.ReadFloat();
                                    break;
                                case 0x09:
                                    operands[i] = this.ReadDouble();
                                    break;
                                case 0x0A:
                                    int size = this.ReadInt();
                                    operands[i] = (string)this.Read(size);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                    }
                    flags <<= 2;
                }
                // check operand types
            }
        }
        #endregion
    }
}
