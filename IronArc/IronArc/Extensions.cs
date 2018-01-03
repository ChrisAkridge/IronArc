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
	}
}
