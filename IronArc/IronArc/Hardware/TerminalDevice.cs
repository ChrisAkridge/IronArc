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

		public TerminalDevice(Guid machineID)
		{
			terminal = HardwareProvider.Provider.CreateTerminal();
			terminal.MachineID = machineID;
		}

		public override void HardwareCall(string functionName, VirtualMachine vm)
		{
			string text = "Hello, world!";

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

		public override void Dispose()
		{
			HardwareProvider.Provider.DestroyTerminal(terminal);
		}
	}
}
