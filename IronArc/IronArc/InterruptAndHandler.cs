using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc
{
    public sealed class InterruptAndHandler
    {
        public ulong CallAddress { get; }
        public Interrupt Interrupt { get; }

        public InterruptAndHandler(ulong callAddress, Interrupt interrupt)
        {
            CallAddress = callAddress;
            Interrupt = interrupt;
        }
    }
}
