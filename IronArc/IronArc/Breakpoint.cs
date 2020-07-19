using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc
{
    public sealed class Breakpoint
    {
        public ulong Address { get; }
        public bool IsUserVisible { get; }

        public Breakpoint(ulong address, bool isUserVisible)
        {
            Address = address;
            IsUserVisible = isUserVisible;
        }
    }
}
