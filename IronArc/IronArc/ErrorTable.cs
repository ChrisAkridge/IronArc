using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public enum Error : uint
	{
		GeneralError,
		InvalidAddressType,
		AddressOutOfRange,
		CallStackUnderflow,
		NotImplemented,
		HardwareDeviceNotPresent,
		InvalidDestinationType,
		StackOverflow
	}
}
