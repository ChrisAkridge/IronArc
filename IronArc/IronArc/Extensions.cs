using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public static class Extensions
	{
		public static byte[] ToLPString(string text)
		{
			int textLength = Encoding.UTF8.GetByteCount(text);
			return BitConverter.GetBytes(textLength).Concat(Encoding.UTF8.GetBytes(text)).ToArray();
		}

		public static ulong GetSizeInBytes(this OperandSize size)
		{
			switch (size)
			{
				case OperandSize.Byte: return 1UL;
				case OperandSize.Word: return 2UL;
				case OperandSize.DWord: return 4UL;
				case OperandSize.QWord: return 8UL;
				default:
					throw new ArgumentException($"Implementation error: Invalid size {size}");
			}
		}
	}
}
