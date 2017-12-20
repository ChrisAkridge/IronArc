using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public sealed class Processor
	{
		private VirtualMachine vm;

		public ulong EAX;
		public ulong EBX;
		public ulong ECX;
		public ulong EDX;
		public ulong EEX;
		public ulong EFX;
		public ulong EGX;
		public ulong EHX;

		public ulong EBP;
		public ulong ESP;

		public ulong EIP;
		public ulong EFLAGS;
		public ulong ERP;

		public ulong stackArgsMarker;
		private ByteBlock memory;

		public Dictionary<string, List<InterruptHandler>> interruptTable;
		public Dictionary<uint, ulong> errorTable;
		public Stack<CallStackFrame> callStack;

		public Processor(ByteBlock memory, ulong firstInstructionAddress, ulong programSize,
			VirtualMachine vm)
		{
			this.memory = memory;
			this.vm = vm;

			interruptTable = new Dictionary<string, List<InterruptHandler>>();
			errorTable = new Dictionary<uint, ulong>();
			callStack = new Stack<CallStackFrame>();

			EIP = ERP = firstInstructionAddress;
			EAX = programSize;
		}

		#region Memory Read/Write
		private byte ReadProgramByte() => memory.ReadByteAt(EIP++);
		private sbyte ReadProgramSByte() => (sbyte)ReadProgramByte();

		private ushort ReadProgramWord()
		{
			var result = memory.ReadUShortAt(EIP);
			EIP += 2;
			return result;
		}
		private short ReadProgramSWord() => (short)ReadProgramWord();

		private uint ReadProgramDWord()
		{
			var result = memory.ReadUIntAt(EIP);
			EIP += 4;
			return result;
		}
		private int ReadProgramSDWord() => (int)ReadProgramDWord();

		private ulong ReadProgramQWord()
		{
			var result = memory.ReadULongAt(EIP);
			EIP += 8;
			return result;
		}
		private long ReadProgramSQWord() => (long)ReadProgramQWord();

		private string ReadProgramLPString()
		{
			uint stringLength;
			var result = memory.ReadStringAt(EIP, out stringLength);
			EIP += stringLength;
			return result;
		}
		#endregion

		public void ExecuteNextInstruction()
		{
			// Step 1: decode the opcode
			ushort opcode = ReadProgramWord();
			switch ((opcode >> 8))
			{
				case 0x00:				/* Control Flow Instructions */
					switch ((opcode & 0xFF))
					{
						case 0x00: NoOperation(); break;
						case 0x01: End(); break;
						case 0x02: Jump(); break;
						case 0x03: Call(); break;
						case 0x04: Return(); break;
						case 0x05: JumpIfEqual(); break;
						case 0x06: JumpIfNotEqual(); break;
						case 0x07: JumpIfLessThan(); break;
						case 0x08: JumpIfGreaterThan(); break;
						case 0x09: JumpIfLessThanOrEqualTo(); break;
						case 0x0A: JumpIfGreaterThanOrEqualTo(); break;
						case 0x0B: AbsoluteJump(); break;
						case 0x0C: HardwareCall(); break;
						case 0x0D: StackArgumentPrologue(); break;
						default: break;
					}
					break;
				case 0x01:
					switch ((opcode & 0xFF))
					{
						case 0x00: MoveData(); break;
						case 0x01: MoveDataWithLength(); break;
						case 0x02: PushToStack(); break;
						default: break;
					}
					break;
				default:
					break;
			}
		}

		#region Internal Helpers
		private ulong ReadRegisterByIndex(ulong registerNumber)
		{
			switch (registerNumber)
			{
				case 0UL: return EAX;
				case 1UL: return EBX;
				case 2UL: return ECX;
				case 3UL: return EDX;
				case 4UL: return EEX;
				case 5UL: return EFX;
				case 6UL: return EGX;
				case 7UL: return EHX;
				case 8UL: return EBP;
				case 9UL: return ESP;
				case 10UL: return EIP;
				case 11UL: return EFLAGS;
				case 12UL: return ERP;
				default:
					throw new ArgumentException($"There is no register numbered {registerNumber}. Please ensure you've masked out the high two bits.");
			}
		}

		private void WriteRegisterByIndex(ulong registerNumber, ulong value)
		{
			switch (registerNumber)
			{
				case 0UL: EAX = value; break;
				case 1UL: EBX = value; break;
				case 2UL: ECX = value; break;
				case 3UL: EDX = value; break;
				case 4UL: EEX = value; break;
				case 5UL: EFX = value; break;
				case 6UL: EGX = value; break;
				case 7UL: EHX = value; break;
				case 8UL: EBP = value; break;
				case 9UL: ESP = value; break;
				case 10UL: EIP = value; break;
				case 11UL: EFLAGS = value; break;
				case 12UL: ERP = value; break;
				default:
					throw new ArgumentException($"There is no register numbered {registerNumber}. Please ensure you've masked out the high two bits.");
			}
		}

		private OperandSize ReadOperandSize(byte operandSizeBits)
		{
			switch (operandSizeBits)
			{
				case 0: return OperandSize.Byte;
				case 1: return OperandSize.Word;
				case 2: return OperandSize.DWord;
				case 3: return OperandSize.QWord;
				default:
					throw new ArgumentException($"Implementation error: Received a value of {operandSizeBits} when a value from 0 to 3 is needed.");
			}
		}

		private AddressType ReadAddressType(byte addressTypeBits)
		{
			switch (addressTypeBits)
			{
				case 0: return AddressType.MemoryAddress;
				case 1: return AddressType.Register;
				case 2: return AddressType.NumericLiteral;
				case 3: return AddressType.StringEntry;
				default:
					throw new ArgumentException("Implementation error: You didn't give me a value between 0 and 3 for an address type in a flags byte.");
			}
		}

		private ulong MaskDataBySize(ulong data, OperandSize size)
		{
			switch (size)
			{
				case OperandSize.Byte: return data & 0xFF;
				case OperandSize.Word: return data & 0xFFFF;
				case OperandSize.DWord: return data & 0xFFFF_FFFF;
				case OperandSize.QWord: return data;
				default:
					throw new ArgumentException($"Implementation error: Invalid operand size {size}");
			}
		}

		private void WriteDataToMemoryBySize(ulong data, ulong address, OperandSize size)
		{
			switch (size)
			{
				case OperandSize.Byte: memory.WriteByteAt((byte)data, address); break;
				case OperandSize.Word: memory.WriteUShortAt((ushort)data, address); break;
				case OperandSize.DWord: memory.WriteUIntAt((uint)data, address); break;
				case OperandSize.QWord: memory.WriteULongAt(data, address); break;
				default:
					throw new ArgumentException($"Implementation error: Invalid operand size {size}");
			}
		}

		private ulong GetMemoryAddressFromAddressBlock(byte addressTypeBits)
		{
			var addressBlock = new AddressBlock(OperandSize.QWord, ReadAddressType(addressTypeBits),
				memory, EIP);
			EIP += addressBlock.operandLength;

			return GetMemoryAddressFromAddressBlock(addressBlock);
		}

		private ulong GetMemoryAddressFromAddressBlock(AddressBlock block)
		{
			switch (block.type)
			{
				case AddressType.MemoryAddress:
					if (block.isPointer)
					{ return memory.ReadULongAt(block.value); }
					else
					{ return block.value; }
				case AddressType.Register:
					ulong valueWithOffset = ReadRegisterByIndex(block.value) + (ulong)block.offset;
					if (block.isPointer)
					{ return memory.ReadULongAt(valueWithOffset); }
					else
					{ return valueWithOffset; }
				case AddressType.NumericLiteral:
				case AddressType.StringEntry:
					RaiseError(Error.InvalidAddressType);
					break;
				default:
					throw new ArgumentException("Implementation error: An address block has a wrong type.");
			}

			return 0UL;
		}

		private ulong ReadDataFromAddressBlock(AddressBlock block, OperandSize size)
		{
			switch (block.type)
			{
				case AddressType.MemoryAddress:
					if (block.isPointer)
					{
						return memory.ReadDataAt(memory.ReadULongAt(block.value), size);
					}
					return memory.ReadDataAt(block.value, size);
				case AddressType.Register:
					if (block.offset != 0 && !block.isPointer)
					{
						RaiseError(Error.InvalidAddressType);
						return 0UL;
					}
					else if (block.isPointer)
					{
						ulong address = ReadRegisterByIndex(block.value) + (ulong)block.offset;
						return memory.ReadDataAt(address, size);
					}
					else
					{
						return MaskDataBySize(ReadRegisterByIndex(block.value), size);
					}
				case AddressType.NumericLiteral:
					return block.value;
				case AddressType.StringEntry:
					throw new NotImplementedException();
				default:
					throw new ArgumentException($"Invalid address block type {block.type}");
			}
		}

		private void WriteDataToAddressBlock(ulong data, AddressBlock block, OperandSize size)
		{
			switch (block.type)
			{
				case AddressType.MemoryAddress:
					if (block.isPointer)
					{
						memory.WriteDataAt(data, memory.ReadULongAt(block.value), size);
					}
					else
					{
						memory.WriteDataAt(data, block.value, size);
					}
					break;
				case AddressType.Register:
					if (block.offset != 0 && !block.isPointer)
					{
						RaiseError(Error.InvalidAddressType);
						return;
					}
					else if (block.isPointer)
					{
						ulong address = ReadRegisterByIndex(block.value) + (ulong)block.offset;
						memory.WriteDataAt(data, address, size);
					}
					else
					{
						data = MaskDataBySize(data, size);
						WriteRegisterByIndex(block.value, data);
					}
					break;
				case AddressType.NumericLiteral:
				case AddressType.StringEntry:
					RaiseError(Error.InvalidAddressType);
					break;
				default:
					throw new ArgumentException($"Implementation error: Invalid address type {block.type}");
			}
		}

		internal void RaiseError(uint errorCode)
		{
			// Look up an error handler for the code and call it if there is one.
			if (!errorTable.ContainsKey(errorCode))
			{
				vm.Error(errorCode);
				return;
			}

			var handlerAddress = errorTable[errorCode] + ERP;
			CallImpl(handlerAddress);
		}

		internal void RaiseError(Error error) => RaiseError((uint)error);

		private void CallImpl(ulong callAddress)
		{
			var oldFlags = EFLAGS;

			// Clear the stackargs flag.
			EFLAGS &= ~(EFlags.StackArgsSet);

			// Create and push the call stack frame.
			var frame = new CallStackFrame(callAddress, EIP, EAX, EBX, ECX, EDX, EEX, EFX, EGX,
				EHX, EFLAGS, EBP);
			callStack.Push(frame);

			// Clear most registers.
			EAX = EBX = ECX = EDX = EEX = EFX = EGX = EHX = EFLAGS = 0UL;

			// Check stackargs and set EBP accordingly
			if ((oldFlags & EFlags.StackArgsSet) != 0)
			{
				EBP = stackArgsMarker;
				stackArgsMarker = 0;
			}
			else { EBP = ESP; }

			EIP = callAddress;
		}

		private void ReturnImpl()
		{
			if (!callStack.Any())
			{
				RaiseError(Error.CallStackUnderflow);
				return;
			}

			// Pop the top frame off the call stack.
			var frame = callStack.Pop();

			// Reset most registers.
			EAX = frame.EAX;
			EBX = frame.EBX;
			ECX = frame.ECX;
			EDX = frame.EDX;
			EEX = frame.EEX;
			EFX = frame.EFX;
			EGX = frame.EGX;
			EHX = frame.EHX;
			EFLAGS = frame.EFLAGS;
			EBP = frame.EBP;

			// We don't need to touch ESP - since return values are pushed on the stack, they can
			// become part of the previous stack frame.

			EIP = frame.returnAddress;
		}
		#endregion

		#region Control Flow Instructions (0x00)
		private void NoOperation()
		{
			// Here, at 2:43pm EST on Thursday, December 14, 2017, the first IronArc instruction
			// was executed.
		}

		private void End()
		{
			vm.Halt();
		}

		private void Jump()
		{
			// jmp <address>
			//	<address>: An addressing block containing the address + ERP to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			EIP = jumpAddress;
		}
		
		private void Call()
		{
			// call <address>
			//	<address>: An addressing block containing the address + ERP to call.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong callAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (callAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			CallImpl(callAddress);
		}

		private void Return()
		{
			// ret
			// Errors:
			//	CallStackUnderflow: Raised if the call stack is empty.

			ReturnImpl();
		}

		private void JumpIfEqual()
		{
			// je <address>
			//	<address>: An addressing block containing the address + ERP to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			if ((EFLAGS & EFlags.EqualFlag) != 0)
			{
				EIP = jumpAddress;
			}
		}

		private void JumpIfNotEqual()
		{
			// jne <address>
			//	<address>: An addressing block containing the address + ERP to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			if ((EFLAGS & EFlags.EqualFlag) == 0)
			{
				EIP = jumpAddress;
			}
		}

		private void JumpIfLessThan()
		{
			// jlt <address>
			//	<address>: An addressing block containing the address + ERP to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			if ((EFLAGS & EFlags.LessThanFlag) != 0)
			{
				EIP = jumpAddress;
			}
		}

		private void JumpIfGreaterThan()
		{
			// jgt <address>
			//	<address>: An addressing block containing the address + ERP to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			if ((EFLAGS & EFlags.GreaterThanFlag) != 0)
			{
				EIP = jumpAddress;
			}
		}

		private void JumpIfLessThanOrEqualTo()
		{
			// jlte <address>
			//	<address>: An addressing block containing the address + ERP to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			if (((EFLAGS & EFlags.LessThanFlag) != 0) && ((EFLAGS & EFlags.EqualFlag) != 0))
			{
				EIP = jumpAddress;
			}
		}

		private void JumpIfGreaterThanOrEqualTo()
		{
			// jgte <address>
			//	<address>: An addressing block containing the address + ERP to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			if (((EFLAGS & EFlags.GreaterThanFlag) != 0) && ((EFLAGS & EFlags.EqualFlag) != 0))
			{
				EIP = jumpAddress;
			}
		}

		private void AbsoluteJump()
		{
			// jmpa <address>
			//	<address>: An addressing block containing the address to jump to.
			// Errors:
			//	InvalidAddressType: Raised if the addressing block does not name a memory address.
			//	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
			// Flags byte: 00XX0000 where the X names the address block type.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong jumpAddress = GetMemoryAddressFromAddressBlock(flagsByte);

			if (jumpAddress >= memory.Length)
			{
				RaiseError(Error.AddressOutOfRange);
				return;
			}

			EIP = jumpAddress;
		}

		private void HardwareCall()
		{
			// hwcall <addressToString>
			//	<addressToString>: An address to a length-prefixed UTF-8 sequence of bytes representing the string containing the hardware call to perform.
			// Errors:
			//	InvalidAddressType: Raised if the address block is a numeric literal.
			//	AddressOutOfRange: Raised if the address is beyond the range of memory.
			// Flags byte: 00XX0000 where XX is the type of the address block.

			byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
			ulong stringAddress = 0UL;
			if (flagsByte == 3)
			{
				// String Literal
				RaiseError(Error.NotImplemented);
			}
			else { stringAddress = GetMemoryAddressFromAddressBlock(flagsByte); }

			string hwcall = memory.ReadStringAt(stringAddress);
			vm.HardwareCall(hwcall);
		}

		private void StackArgumentPrologue()
		{
			// stackargs
			// Errors: None.
			// Flags Byte: None.

			// Set the stackargs flag on EFLAGS.
			EFLAGS |= EFlags.StackArgsSet;

			// Set the stackargs marker to the current value of ESP.
			stackArgsMarker = ESP;
		}
		#endregion

		#region Data Operation Instruction (0x01)
		private void MoveData()
		{
			// mov <SIZE> <source> <dest>
			//	<SIZE>: The size of the operand to move.
			//	<source>: A memory address, register, or numeric literal.
			//	<destination>: A memory address or register.
			// Errors:
			//	InvalidAddressType: Raised if source is a string table entry or destination is a numeric literal or string entry table.
			//	AddressOutOfRange: Raised if any address is out of range.
			// Flags Byte: ZZSSDD00, where Z is the size, S is the source type, D is the destination type.

			byte flagsByte = ReadProgramByte();
			OperandSize size = ReadOperandSize((byte)(flagsByte >> 6));
			AddressType sourceType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));
			AddressType destinationType = ReadAddressType((byte)((flagsByte & 0x0C) >> 2));

			AddressBlock sourceBlock = new AddressBlock(size, sourceType, memory, EIP);
			EIP += sourceBlock.operandLength;
			AddressBlock destinationBlock = new AddressBlock(size, destinationType, memory, EIP);
			EIP += destinationBlock.operandLength;

			ulong sourceData = ReadDataFromAddressBlock(sourceBlock, size);
			WriteDataToAddressBlock(sourceData, destinationBlock, size);
		}

		private void MoveDataWithLength()
		{
			// movln <sourceAddr> <destAddr> <length>
			//	sourceAddr: An addressing block (memory address, pointer-to-memory, or 
			//		pointer-in-register) pointing to the source data.
			//	destAddr: An addressing block (memory address, pointer-to-memory, or
			//		pointer-in-register) pointing to the destination address.
			//	length: An addressing block containing the length of bytes to write. Always assumed
			//		to be a DWORD.
			// Errors:
			//	InsufficientDestinationSize: Raised if the destination address is too close to the
			//		end of memory.
			//	InvalidAddressType: Raised if either the source or destination address blocks are
			//		registers, numeric literals, or string table entries.
			// Flags Byte: 00SSDDLL where SS is the source type, DD is the destination type, and
			//	LL is the length type.

			byte flagsByte = ReadProgramByte();
			AddressType sourceType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));
			AddressType destinationType = ReadAddressType((byte)((flagsByte & 0x0C) >> 2));
			AddressType lengthType = ReadAddressType((byte)(flagsByte & 0x03));

			AddressBlock sourceBlock = new AddressBlock(OperandSize.QWord, sourceType, memory, EIP);
			EIP += sourceBlock.operandLength;
			AddressBlock destinationBlock = new AddressBlock(OperandSize.QWord, destinationType, memory, EIP);
			EIP += destinationBlock.operandLength;
			AddressBlock lengthBlock = new AddressBlock(OperandSize.DWord, lengthType, memory, EIP);
			EIP += lengthBlock.operandLength;

			uint length = (uint)ReadDataFromAddressBlock(lengthBlock, OperandSize.DWord);

			ulong sourceAddress = GetMemoryAddressFromAddressBlock(sourceBlock);
			ulong destAddress = GetMemoryAddressFromAddressBlock(destinationBlock);

			memory.WriteAt(sourceAddress, destAddress, length);
		}

		private void PushToStack()
		{
			// push <SIZE> <value>
			//	<SIZE>: The size of the data to push.
			//	<value>: An address block containing either the value to push or an address to it.
			// Errors:
			//	AddressOutOfRange: Raised if the address given is outside memory.
			//	StackOverflow: Raised if ESP leaves the bounds of memory after the push.
			// Flags Byte: SSDD0000, where SS is the size and DD is the type of the data.

			byte flagsByte = ReadProgramByte();
			OperandSize size = ReadOperandSize((byte)(flagsByte >> 6));
			AddressType dataType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));

			AddressBlock dataBlock = new AddressBlock(size, dataType, memory, EIP);
			EIP += dataBlock.operandLength;

			ulong data = ReadDataFromAddressBlock(dataBlock, size);

			memory.WriteDataAt(data, ESP, size);
			ESP += size.SizeInBytes();
			if (ESP >= memory.Length || ESP < EBP)
			{
				RaiseError(Error.StackOverflow);
				return;
			}
		}
		#endregion
	}
}
