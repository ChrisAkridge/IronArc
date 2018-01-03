using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc.Hardware
{
	public interface ITerminal
	{
		Guid MachineID { get; set; }

		void Write(string text);
		void WriteLine(string text);

		char Read();
		string ReadLine();
	}
}
