using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// An address to a part of various memory sources available to the VM.
    /// </summary>
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

        public ByteBlock Value
        {
            get
            {
                switch (this.Type)
                {
                    case AddressType.NumericLiteral:
                    case AddressType.StringLiteral:
                        return this.internalValue;
                    case AddressType.ProcessorRegister:
                        return this.owner.ReadRegister(this.internalValue);
                    case AddressType.MemoryAddress:
                        return this.owner.Memory.ReadAt(this.Length, this.internalValue);
                    case AddressType.DiskfileAddress:
                        return this.owner.Diskfile.File.ReadAt(this.Length, this.internalValue);
                    case AddressType.ConsoleStream:
                        return StandardStreams.GetStream(this.internalValue).Read(this.Length);
                    case AddressType.MemoryPointer:
                        break;
                    case AddressType.DiskfilePointer:
                        break;
                    case AddressType.ShortPointer:
                        break;
                    case AddressType.StackObject:
                        break;
                    case AddressType.StackByte:
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBlock"/> class.
        /// </summary>
        /// <param name="processor">A processor from which to read the memory initializing this value.</param>
        /// <remarks>This constructor will create an address block from the processor's memory where the instruction pointer is pointing to.
        /// The instruction pointer will be moved accordingly.</remarks>
        public AddressBlock(Processor processor)
        {
            this.owner = processor;

            // Read the next byte in memory to determine the address type
            byte idByte = processor.ReadByte();
            this.Type = (AddressType)(byte)(idByte << 2);

            switch ((AddressType)(byte)(idByte << 2))
            {
                case AddressType.NumericLiteral:
                    this.Length = GetLengthFromIDByte(idByte);
                    this.internalValue = processor.Read(this.Length);
                    break;
                case AddressType.StringLiteral:
                    this.Length = processor.ReadInt();
                    this.internalValue = processor.Read(this.Length);
                    break;
                case AddressType.ProcessorRegister:
                    byte register = processor.ReadByte();
                    this.Length = (register != 0x09) ? (register >= 0x00 && register <= 0x07) ? 8 : 4 : 0;
                    this.internalValue = register;
                    break;
                case AddressType.MemoryAddress:
                    ulong fullMemoryAddress = processor.ReadULong();
                    this.Length = (int)(fullMemoryAddress >> 32);
                    this.internalValue = (uint)(fullMemoryAddress & 0xFFFFFFFF00000000UL);
                    break;
                case AddressType.DiskfileAddress:
                    ulong fullDiskfileAddress = processor.ReadULong();
                    this.Length = (int)(fullDiskfileAddress >> 32);
                    this.internalValue = (uint)(fullDiskfileAddress & 0xFFFFFFFF00000000UL);
                    break;
                case AddressType.ConsoleStream:
                    this.internalValue = processor.ReadByte();
                    this.Length = processor.ReadInt();
                    break;
                case AddressType.MemoryPointer:
                    break;
                case AddressType.DiskfilePointer:
                    break;
                case AddressType.ShortPointer:
                    break;
                case AddressType.StackObject:
                    break;
                case AddressType.StackByte:
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
        ShortPointer = 8 << 2,
        StackObject = 9 << 2,
        StackByte = 10 << 2
    }
}
