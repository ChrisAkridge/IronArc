using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Memory
{
    public sealed class HardwareMemory
    {
        private const ulong Plane1Start = 0x0080000000000000;

        private ulong nextHardwareAllocationAddress = Plane1Start;
        private readonly List<HardwareMemoryMapping> mappings = new List<HardwareMemoryMapping>();

        public void Add(ulong length, byte[] memoryReference)
        {
            var mapping = new HardwareMemoryMapping(nextHardwareAllocationAddress, nextHardwareAllocationAddress + length, memoryReference);
            mappings.Add(mapping);
            nextHardwareAllocationAddress += length;
        }
        
        public void Remove(ulong mappingStartAddress) => mappings.RemoveAll(m => m.StartAddress == mappingStartAddress);

        public byte[] Read(ulong address, int length)
        {
            ulong endAddress = address + (ulong)length;

            var mapping = FirstMapping(address);
            if (endAddress > mapping.EndAddress)
            {
                // RaiseError because a read has to be entirely within a single mapping
                throw new Exception();
            }

            ByteBlock memory = mapping.MemoryReference;
            byte[] buffer = new byte[length];
            ulong offsetInMemory = address % memory.Length;
            memory.ReadIntoBuffer(buffer, 0, offsetInMemory, length);

            return buffer;
        }

        public byte ReadByte(ulong address)
        {
            var mapping = FirstMapping(address);
            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            return memory.ReadByteAt(offsetInMemory);
        }

        public ushort ReadUShort(ulong address)
        {
            var mapping = FirstMapping(address);
            if (address + 1 > mapping.EndAddress)
            {
                // RaiseError because a read has to be entirely within a single mapping
                throw new Exception();
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            return memory.ReadUShortAt(offsetInMemory);
        }

        public uint ReadUInt(ulong address)
        {
            var mapping = FirstMapping(address);
            if (address + 3 > mapping.EndAddress)
            {
                // RaiseError because a read has to be entirely within a single mapping
                throw new Exception();
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            return memory.ReadUIntAt(offsetInMemory);
        }

        public ulong ReadULong(ulong address)
        {
            var mapping = FirstMapping(address);
            if (address + 7 > mapping.EndAddress)
            {
                // RaiseError because a read has to be entirely within a single mapping
                throw new Exception();
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            return memory.ReadULongAt(offsetInMemory);
        }

        private HardwareMemoryMapping FirstMapping(ulong address)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < mappings.Count; i++)
            {
                var mapping = mappings[i];

                if (address >= mapping.StartAddress && address <= mapping.EndAddress) { return mapping; }
            }

            // RaiseError because the address wasn't mapped to anything
            throw new Exception();
        }
    }
}
