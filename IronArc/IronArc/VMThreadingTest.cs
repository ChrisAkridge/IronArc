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

			ByteBlock memory = ByteBlock.FromLength(1024UL);
			memory.WriteULongAt(0x7FFFFFFFFFFFFFFFUL, 0UL);
			memory.WriteULongAt(0xFFFFFFFFFFFFFFFFUL, 8UL);
			memory.WriteByteAt(0x00, 16UL);
			memory.WriteByteAt(0x81, 17UL);
			memory.WriteByteAt(0x42, 18UL);
			memory.WriteIntAt(0x00112233, 19UL);
			memory.WriteByteAt(0xC3, 23UL);
			memory.WriteIntAt(0x00223344, 24UL);
			memory.WriteByteAt(0xFF, 28UL);
			memory.WriteUShortAt(0x8FFF, 29UL);
			memory.WriteUIntAt(0x9FFFFFFFU, 31UL);
			memory.WriteULongAt(0xAFFFFFFFFFFFFFFFUL, 35UL);

			AddressBlock address = new AddressBlock(OperandSize.Byte, AddressBlock.AddressType.MemoryAddress, memory, 0UL);
			AddressBlock pointer = new AddressBlock(OperandSize.Word, AddressBlock.AddressType.MemoryAddress, memory, 8UL);
			AddressBlock eax = new AddressBlock(OperandSize.DWord, AddressBlock.AddressType.Register, memory, 16UL);
			AddressBlock ebxPointer = new AddressBlock(OperandSize.QWord, AddressBlock.AddressType.Register, memory, 17UL);
			AddressBlock ecxOffset = new AddressBlock(OperandSize.Byte, AddressBlock.AddressType.Register, memory, 18UL);
			AddressBlock edxPointerOffset = new AddressBlock(OperandSize.Word, AddressBlock.AddressType.Register, memory, 23UL);
			AddressBlock byteLiteral = new AddressBlock(OperandSize.Byte, AddressBlock.AddressType.NumericLiteral, memory, 28UL);
			AddressBlock ushortLiteral = new AddressBlock(OperandSize.Word, AddressBlock.AddressType.NumericLiteral, memory, 29UL);
			AddressBlock uintLiteral = new AddressBlock(OperandSize.DWord, AddressBlock.AddressType.NumericLiteral, memory, 31UL);
			AddressBlock ulongLiteral = new AddressBlock(OperandSize.QWord, AddressBlock.AddressType.NumericLiteral, memory, 35UL);
		}
	}
}
