using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Memory
{
    public readonly struct HardwareMemoryMapping
    {
        public ulong StartAddress { get; }
        public ulong EndAddress { get; }
        public ByteBlock MemoryReference { get; }

        public HardwareMemoryMapping(ulong startAddress, ulong endAddress, ByteBlock memoryReference)
        {
            StartAddress = startAddress;
            EndAddress = endAddress;
            MemoryReference = memoryReference;
        }
    }
}
