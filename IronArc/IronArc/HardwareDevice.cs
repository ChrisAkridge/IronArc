using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public abstract class HardwareDevice
	{
		// TODO: private VirtualMachine owner;

		public Guid DeviceID { get; private set; }
		public abstract string DeviceName { get; }
		public abstract HardwareDeviceStatus Status { get; } // add a SetStatus(HardwareDeviceStatus) method to derived classes to set the status. Also use a field, not an autoproperty.z

		public abstract void HardwareCall(string functionName, Stack vmStack);
	}
}
