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
		None,
		VMStateChanged
	}

	public enum NumericOperation
	{
		Add,
		Subtract,
		Multiply,
		Divide,
		ModDivide,
		Increment,
		Decrement,
		BitwiseAND,
		BitwiseOR,
		BitwiseXOR,
		BitwiseNOT,
		BitwiseShiftLeft,
		BitwiseShiftRight,
		LogicalAND,
		LogicalOR,
		LogicalXOR,
		LogicalNOT,
		Compare,
		SquareRoot
	}

	public static class EnumExtensions
	{
		public static ulong SizeInBytes(this OperandSize size)
		{
			switch (size)
			{
				case OperandSize.Byte: return 1UL;
				case OperandSize.Word: return 2UL;
				case OperandSize.DWord: return 4UL;
				case OperandSize.QWord: return 8UL;
				default:
					throw new ArgumentException($"Implementation error: Invalid size {size}");
			}
		}
	}
}
