using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public class VMHardwareState
	{
		private List<HardwareDevice> devices;

		public IReadOnlyList<HardwareDevice> Devices
		{
			get { return devices; }
		}

		public void AddDevice(HardwareDevice device)
		{
			devices.Add(device);
		}

		public void RemoveDevice(ushort deviceID)
		{
			var device = devices.Where(d => d.DeviceID == deviceID).FirstOrDefault();

			if (device == null)
			{
				throw new ArgumentException(string.Format("No hardware device has ID #{0}.", deviceID), "deviceID");
			}

			devices.Remove(device);
		}
	}
}
