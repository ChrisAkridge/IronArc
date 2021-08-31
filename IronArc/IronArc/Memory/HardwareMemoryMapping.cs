using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Memory
{
    public sealed class HardwareMemoryMapping
    {
        public Guid MachineID { get; set; }
        public uint DeviceID { get; set; }
        public ulong MemoryLength { get; set; }
        public ByteBlock Memory { get; set; }
    }
}
