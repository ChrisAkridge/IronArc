using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc.Hardware
{
	public interface ITerminal
	{
		void Write(string text);
		void WriteLine(string text);
	}
}
