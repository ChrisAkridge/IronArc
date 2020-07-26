using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    public struct InterruptHandler
    {
        public uint DeviceId { get; set; }
        public byte Index { get; set; }
        public ulong CallAddress { get; set; }
    }
}
