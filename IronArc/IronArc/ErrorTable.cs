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
		HeaderInvalid,
		StringIndexOutOfRange,
		StackUnderflow,
        NoSuchPageTable,
        CannotDestroyCurrentPageTable,
        CrossPlaneAccess,
        ReservedPlaneAccess,
        CrossHardwareMemoryAccess,
        NoHardwareMemoryHere,
        OutOfVirtualMemory
	}

    public static class ErrorMessages
    {
        public static string GetDefaultMessage(Error error)
        {
            switch (error)
            {
                case Error.GeneralError: return "An error has occurred.";
                case Error.InvalidAddressType: return "This address block cannot be used this way.";
                case Error.AddressOutOfRange: return "The address is beyond the range of system memory.";
                case Error.CallStackUnderflow:
                    return "Attempted to return from a call, but there was nothing on the call stack.";
                case Error.NotImplemented: return "This feature is not yet implemented.";
                case Error.HardwareDeviceNotPresent: return "This hardware device is not attached to the VM.";
                case Error.InvalidDestinationType: return "This value cannot be written here.";
                case Error.HeaderInvalid: return "The loaded program has an invalid header.";
                case Error.StringIndexOutOfRange: return "There is no string at the requested index.";
                case Error.StackUnderflow: return "The stack has popped more item than it has.";
                case Error.NoSuchPageTable: return "There is no page table with this ID.";
                case Error.CannotDestroyCurrentPageTable:
                    return "Attempted to destroy the page table that is currently in use.";
                case Error.CrossPlaneAccess: return "A memory access crossed a plane boundary.";
                case Error.ReservedPlaneAccess: return "A memory access occurred in a reserved plane.";
                case Error.CrossHardwareMemoryAccess:
                    return "A memory access occurred across two or more hardware device memory spaces.";
                case Error.NoHardwareMemoryHere: return "Attempted to access hardware memory where none was mapped.";
                case Error.OutOfVirtualMemory: return "A page fault could not allocate another page".
                default: return $"Attempted to raise non-existant error {(uint)error}. Congratulations.";
            }
        }
    }
}
