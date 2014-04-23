using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    public sealed class AddressBlock
    {
        private const byte LengthOneMask = 0x00;
        private const byte LengthTwoMask = 0x40;
        private const byte LengthFourMask = 0x80;
        private const byte LengthEightMask = 0xC0;

        private ByteBlock internalValue;
        private Processor owner;
        public AddressType Type { get; private set; }
        public int Length { get; private set; }

        public AddressBlock(Processor processor)
        {
            this.owner = processor;

            // Read the next byte in memory to determine the address type
            byte idByte = processor.ReadByte();

            switch ((AddressType)(idByte << 2))
            {
                case AddressType.NumericLiteral:
                    this.Length = GetLengthFromIDByte(idByte);
                    ////this.internalValue
                    break;

            }
        }

        private static int GetLengthFromIDByte(byte idByte)
        {
            if ((idByte & LengthEightMask) != 0)
            {
                return 8;
            }
            else if ((idByte & LengthFourMask) != 0)
            {
                return 4;
            }
            else if ((idByte & LengthTwoMask) != 0)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }

    public enum AddressType : byte
    {
        NumericLiteral = 0,
        StringLiteral = 1 << 2,
        ProcessorRegister = 2 << 2,
        MemoryAddress = 3 << 2,
        DiskfileAddress = 4 << 2,
        ConsoleStream = 5 << 2,
        MemoryPointer = 6 << 2,
        DiskfilePointer = 7 << 2,
        ShortPointer = 8 << 2
    }
}
