using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public class Operand
	{
		public enum OperandType : byte
		{
			AddressBlock, // TODO: change to MemoryAddress
			Register,
			StackIndex,
			NumericByte,
			NumericSByte,
			NumericShort,
			NumericUShort,
			NumericInt,
			NumericUInt,
			NumericLong,
			NumericULong,
			NumericFloat,
			NumericDouble,
			LPString
		};
		public OperandType Type;
		public object Value; // maybe change
		public uint Length;

		public Operand(Processor cpu, OperandType optype)
		{
			Type = optype;
			switch (Type)
			{
				case OperandType.AddressBlock:
					Length = 8;
					// add later
					break;
				case OperandType.Register:
					Length = 1;
					Value = cpu.ReadByte();
					break;
				case OperandType.StackIndex:
					Length = 4;
					Value = cpu.ReadUInt();
					break;
				case OperandType.NumericByte:
					Length = 1;
					Value = cpu.ReadByte();
					break;
				case OperandType.NumericSByte:
					Length = 1;
					Value = cpu.ReadSByte();
					break;
				case OperandType.NumericShort:
					Length = 2;
					Value = cpu.ReadShort();
					break;
				case OperandType.NumericUShort:
					Length = 2;
					Value = cpu.ReadUShort();
					break;
				case OperandType.NumericInt:
					Length = 4;
					Value = cpu.ReadInt();
					break;
				case OperandType.NumericUInt:
					Length = 4;
					Value = cpu.ReadUInt();
					break;
				case OperandType.NumericLong:
					Length = 8;
					Value = cpu.ReadLong();
					break;
				case OperandType.NumericULong:
					Length = 8;
					Value = cpu.ReadULong();
					break;
				case OperandType.NumericFloat:
					Length = 4;
					Value = cpu.ReadFloat();
					break;
				case OperandType.NumericDouble:
					Length = 8;
					Value = cpu.ReadDouble();
					break;
				case OperandType.LPString:
					Length = cpu.ReadUInt();
					ByteBlock bytes = cpu.Read(Length);
					Value = System.Text.Encoding.UTF8.GetString(bytes.ToByteArray());
					break;
				default:
					break;
			}
		}
	}
}
