using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	/// <summary>
	/// An operand that points to a memory address, processor register, or an entry in the string
	/// table, or is a numeric literal value.
	/// </summary>
	public struct AddressBlock
	{
		public const ulong MemoryPointerMask = ((ulong)1 << 63);
		public const ulong RegisterPointerMask = ((ulong)1 << 7);
		public const ulong RegisterHasOffsetMask = ((ulong)1 << 6);

		// TODO: test the perf of using autoprops here to see if we can take the perf hit
		public AddressType type;
		public OperandSize size;
		public ulong value;
		public int offset;
		public bool isPointer;
		public ulong operandLength;

		public AddressBlock(OperandSize size, AddressType type, ByteBlock memory, ulong operandAddress)
		{
			this.size = size;
			this.type = type;

			// These assignments are just so the compiler doesn't complain
			value = 0UL;
			offset = 0;
			isPointer = false;
			operandLength = 0UL;

			switch (type)
			{
				case AddressType.MemoryAddress:
					value = memory.ReadULongAt(operandAddress);
					isPointer = (value & MemoryPointerMask) != 0;
					if (isPointer) { value &= 0x7FFF_FFFF_FFFF_FFFF; }
					operandLength = 8UL;
					break;
				case AddressType.Register:
					value = memory.ReadByteAt(operandAddress);
					isPointer = (value & RegisterPointerMask) != 0;
					operandLength = 1UL;
					if ((value & RegisterHasOffsetMask) != 0)
					{
						offset = memory.ReadIntAt(operandAddress + 1);
						operandLength = 5UL;
					}

					// Clear the pointer and offset bits from the byte since we already know
					// if this is a pointer/has an offset.
					value &= 0x3F;
					break;
				case AddressType.NumericLiteral:
					switch (size)
					{
						case OperandSize.Byte:
							value = memory.ReadByteAt(operandAddress);
							operandLength = 1UL;
							break;
						case OperandSize.Word:
							value = memory.ReadUShortAt(operandAddress);
							operandLength = 2UL;
							break;
						case OperandSize.DWord:
							value = memory.ReadUIntAt(operandAddress);
							operandLength = 4UL;
							break;
						case OperandSize.QWord:
							value = memory.ReadULongAt(operandAddress);
							operandLength = 8UL;
							break;
						default:
							throw new ArgumentException($"Invalid operand size {size}.", nameof(size));
					}
					break;
				case AddressType.StringEntry:
					value = memory.ReadUIntAt(operandAddress);
					operandLength = 4UL;
					break;
				default:
					throw new ArgumentException($"Invalid address type {type}.", nameof(type));
			}
		}
	}
}