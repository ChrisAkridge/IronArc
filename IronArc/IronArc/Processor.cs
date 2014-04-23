using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
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

        public byte ReadByte()
        {
            return this.Memory[this.InstructionPointer++];
        }

        public sbyte ReadSByte()
        {
            return (sbyte)this.ReadByte();
        }
    }
}
