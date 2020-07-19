using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    public struct InterruptHandler
    {
        public int Index { get; set; }
        public ulong CallAddress { get; set; }
    }
}
