using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexControlLibrary;

namespace IronArcHost
{
	internal sealed class TestByteProvider : IByteProvider
	{
		public int Length => 1024;

		public byte GetByte(int offset)
		{
			return unchecked((byte)offset);
		}
	}
}
