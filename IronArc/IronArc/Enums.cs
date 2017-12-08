using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public enum VMState
	{
		Paused,
		Running,
		Halted,
		Error
	}

	public enum HardwareDeviceStatus
	{
		Active,
		Inactive,
		Error,
		Finalized
	}

	public enum OperandSize
	{
		Byte,
		Word,
		DWord,
		QWord
	}

	public enum AddressType
	{
		MemoryAddress,
		Register,
		NumericLiteral,
		StringEntry
	}

	public enum VMMessage
	{
		None,
		QueryVMState,
		SetVMState,
		AddHardwareDevice,
		RemoveHardwareDevice,
		DebugYieldVM,
		DebugResumeVM
	}

	public enum UIMessage
	{
		None
	}
}
