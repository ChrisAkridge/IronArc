using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	internal static class EFlags
	{
		public const ulong CarryFlag = 0x80_00_00_00_00_00_00_00;
		public const ulong EqualFlag = 0x40_00_00_00_00_00_00_00;
		public const ulong LessThanFlag = 0x20_00_00_00_00_00_00_00;
		public const ulong GreaterThanFlag = 0x10_00_00_00_00_00_00_00;
		public const ulong LessThanOrEqualToFlag = 0x08_00_00_00_00_00_00_00;
		public const ulong GreaterThanOrEqualToFlag = 0x04_00_00_00_00_00_00_00;

		public const ulong StackArgsSet = 0x00_00_00_00_80_00_00_00;
	}
}
