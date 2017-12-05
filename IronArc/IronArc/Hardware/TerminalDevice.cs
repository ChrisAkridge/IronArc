using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc.Hardware
{
	public sealed class TerminalDevice : HardwareDevice
	{
		// The first hardware device, created on December 1, 2017.
		private ITerminal terminal;

		public override string DeviceName => "Terminal";

		public override HardwareDeviceStatus Status => HardwareDeviceStatus.Active;

		public TerminalDevice(ITerminal terminal)
		{
			this.terminal = terminal;
		}

		public override void HardwareCall(string functionName, Stack vmStack)
		{
			string text = vmStack.PopString();
			if (functionName == "Write")
			{
				Write(text);
			}
			else if (functionName == "WriteLine")
			{
				WriteLine(text);
			}
		}

		private void Write(string text) => terminal.Write(text);
		private void WriteLine(string text) => terminal.WriteLine(text);
	}
}
