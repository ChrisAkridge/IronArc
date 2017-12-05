using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronArc.Hardware;

namespace IronArc
{
	public sealed class VMThreadingTest
	{
		// This class is mostly to test how we're going to implement cross-thread messaging.
		string programPath;
		int totalMemory;
		int programLoadAddress;
		TerminalDevice terminal;
		Stack stack = new Stack(1024);

		public VMThreadingTest(string programPath, int totalMemory,
			int programLoadAddress, List<HardwareDevice> hardware)
		{
			terminal = (TerminalDevice)hardware.First(h => h is TerminalDevice);

			this.programPath = programPath;
			this.totalMemory = totalMemory;
			this.programLoadAddress = programLoadAddress;
		}

		public void Start()
		{
			stack.Push($"\tloaded at 0x{programLoadAddress:X8}");
			stack.Push($"\twith {totalMemory} byte(s) of memory");
			stack.Push($"\twith a program at {programPath}");
			stack.Push($"Loaded a VM...");

			terminal.HardwareCall("WriteLine", stack);
			terminal.HardwareCall("WriteLine", stack);
			terminal.HardwareCall("WriteLine", stack);
			terminal.HardwareCall("WriteLine", stack);
		}
	}
}
