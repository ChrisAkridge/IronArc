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
        public AddressBlock Address; // only used for address blocks
        public ByteBlock Value;

		public Operand(Processor cpu, OperandType optype)
		{
			Type = optype;
			switch (Type)
			{
				case OperandType.AddressBlock:
                    
					break;
				case OperandType.Register:
					// add
					break;
				case OperandType.StackIndex:
					// add
					break;
				case OperandType.NumericByte:
					Value = new ByteBlock(cpu.ReadByte());
					break;
				case OperandType.NumericSByte:
					Value = new ByteBlock(cpu.ReadSByte());
					break;
				case OperandType.NumericShort:
                    Value = new ByteBlock(cpu.ReadShort());
					break;
				case OperandType.NumericUShort:
                    Value = new ByteBlock(cpu.ReadUShort());
					break;
				case OperandType.NumericInt:
                    Value = new ByteBlock(cpu.ReadInt());
					break;
				case OperandType.NumericUInt:
                    Value = new ByteBlock(cpu.ReadUInt());
					break;
				case OperandType.NumericLong:
                    Value = new ByteBlock(cpu.ReadLong());
					break;
				case OperandType.NumericULong:
                    Value = new ByteBlock(cpu.ReadULong());
					break;
				case OperandType.NumericFloat:
                    Value = new ByteBlock(cpu.ReadFloat());
					break;
				case OperandType.NumericDouble:
                    Value = new ByteBlock(cpu.ReadDouble());
					break;
				case OperandType.LPString:
					uint length = cpu.ReadUInt();
					ByteBlock bytes = cpu.Read(length);
					Value = new ByteBlock(System.Text.Encoding.UTF8.GetString(bytes.ToByteArray()));
					break;
				default:
					break;
			}
		}
	}
}
