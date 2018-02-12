using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Hardware
{
	public sealed class VSDebugger : HardwareDevice
	{
		public override string DeviceName => "VSDebugger";

		public override HardwareDeviceStatus Status => (Debugger.IsAttached) ? HardwareDeviceStatus.Active : HardwareDeviceStatus.Inactive;

		public VSDebugger(Guid machineID) { }

		public override void Dispose() { }

		public override void HardwareCall(string functionName, VirtualMachine vm)
		{
			if (functionName.ToLowerInvariant() == "break")
			{
				Debugger.Break();
			}
		}
	}
}
