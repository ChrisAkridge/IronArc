using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    public sealed class Processor
    {
        // Registers
        public long EAX { get; set; }
        public long EBX { get; set; }
        public long ECX { get; set; }
        public long EDX { get; set; }
        public long EEX { get; set; }
        public long EFX { get; set; }
        public long EGX { get; set; }
        public long EHX { get; set; }

        // System memory
        public byte[] Memory { get; set; }

        // System stack
        public Stack<long> Stack { get; set; }

        // Instruction pointer
        public ulong InstructionPointer { get; set; }

        // Diskfile
        public Diskfile Diskfile { get; set; }
    }
}
