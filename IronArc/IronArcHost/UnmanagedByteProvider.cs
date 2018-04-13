using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HexControlLibrary;

namespace IronArcHost
{
	internal sealed class UnmanagedByteProvider : IByteProvider
	{
		private IntPtr pointer;
		private int length;

		public int Length => length;

		public UnmanagedByteProvider(IntPtr pointer, int length)
		{
			this.pointer = pointer;
			this.length = length;
		}

		public byte GetByte(int offset)
		{
			return Marshal.ReadByte(pointer, offset);
		}
	}
}
