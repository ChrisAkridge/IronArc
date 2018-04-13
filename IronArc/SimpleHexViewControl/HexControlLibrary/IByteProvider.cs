using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexControlLibrary
{
    public interface IByteProvider
    {
        byte GetByte(int offset);

		// TODO: figure out a good length type for EVERYTHING in all of the IronArc projects
		// (some places are int, some are long, some are ulong). At least change as much as we can
        int Length { get; }
    }
}
