using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    public sealed class Processor
    {
        // Standard registers
        public uint EAX { get; set; }
        public uint EBX { get; set; }
        public uint ECX { get; set; }
        public uint EDX { get; set; }
        public uint EEX { get; set; }
        public uint EFX { get; set; }
        public uint EGX { get; set; }
        public uint EHX { get; set; }

        // Extended registers
        public ulong EXA { get; set; }
        public ulong EXB { get; set; }
        public ulong EXC { get; set; }
        public ulong EXD { get; set; }

        // System memory
        public byte[] Memory { get; set; }

        // System stack
        public Stack<byte> Stack { get; set; }

        // Instruction pointer
        public uint InstructionPointer { get; set; }

        // Diskfile
        public Diskfile Diskfile { get; set; }
    }
}
