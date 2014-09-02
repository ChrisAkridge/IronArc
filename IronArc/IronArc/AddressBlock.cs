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
        private enum AddressType : ushort
        {
            Memory = 0x0000,
            System = 0x4000,
            Stack = 0x8000,
            HW = 0xC000 // don't know why 2B are needed
        };

        private Processor owner;
        public AddressType Type { get; private set; }
        public bool IsPointer { get; private set; }
        public uint Length { get; private set; }
        public uint Address { get; private set; }

        public ByteBlock Value
        {
            get
            {
                switch (this.Type)
                {
                    case AddressType.Memory:
                        return this.owner.Memory.ReadAt(this.Length, this.Address);
                    case AddressType.System:
                        break;
                    case AddressType.Stack:
                        break;
                    case AddressType.HW:
                        break; // add
                    default:
                        throw new FormatException("unrecognized address type");
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBlock"/> class.
        /// </summary>
        /// <param name="cpu">The processor that is creating the AddressBlock.</param>
        /// <param name="bytes">The 8 bytes from which the AddressBlock will be created.</param>
        public AddressBlock(Processor cpu, ulong bytes)
        {
            this.owner = cpu;
            this.Type = (AddressType)(bytes >> 48);
            this.IsPointer = (bytes & (1 << 47)) != 0 ? true : false;
            this.Length = (uint)(bytes & 0x00007FFF00000000UL);
            this.Address = (uint)(bytes & 0x00000000FFFFFFFFUL);
        }
    }
}
