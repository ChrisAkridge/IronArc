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

        public HardwareMemoryMapping Add(ulong length, byte[] memoryReference)
        {
            var mapping = new HardwareMemoryMapping(nextHardwareAllocationAddress, nextHardwareAllocationAddress + length, memoryReference);
            mappings.Add(mapping);
            nextHardwareAllocationAddress += length;

            return mapping;
        }

        public void Remove(ulong mappingStartAddress) => mappings.RemoveAll(m => m.StartAddress == mappingStartAddress);

        public byte[] Read(ulong address, int length)
        {
            ulong endAddress = address + (ulong)length;

            var mapping = FirstMapping(address);
            if (endAddress > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Read 0x{address:X16}-0x{endAddress:X16} crossed hardware memory boundaries");
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

        public sbyte ReadSByte(ulong address) => (sbyte)ReadByte(address);
        
        public ushort ReadUShort(ulong address)
        {
            var mapping = FirstMapping(address);
            if (address + 1 > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Read 2 bytes at 0x{address:X16} crossed hardware memory boundaries");
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            return memory.ReadUShortAt(offsetInMemory);
        }

        public short ReadShort(ulong address) => (short)ReadUShort(address);

        public uint ReadUInt(ulong address)
        {
            var mapping = FirstMapping(address);
            if (address + 3 > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Read 4 bytes at 0x{address:X16} crossed hardware memory boundaries");
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            return memory.ReadUIntAt(offsetInMemory);
        }

        public int ReadInt(ulong address) => (int)ReadUInt(address);

        public ulong ReadULong(ulong address)
        {
            var mapping = FirstMapping(address);
            if (address + 7 > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Read 8 bytes at 0x{address:X16} crossed hardware memory boundaries");
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            return memory.ReadULongAt(offsetInMemory);
        }

        public long ReadLong(ulong address) => (long)ReadULong(address);

        public void Write(byte[] bytes, ulong address)
        {
            ulong endAddress = address + (ulong)bytes.Length;

            var mapping = FirstMapping(address);

            if (endAddress > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Write 0x{address:X16}-0x{endAddress:X16} crossed hardware memory boundaries");
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetIntoMemory = address % memory.Length;
            memory.WriteAt(bytes, address);
        }

        public void WriteByte(byte value, ulong address)
        {
            var mapping = FirstMapping(address);
            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;
            
            memory.WriteByteAt(value, offsetInMemory);
        }

        public void WriteSByte(sbyte value, ulong address) => WriteByte((byte)value, address);
        
        public void WriteUShort(ushort value, ulong address)
        {
            var mapping = FirstMapping(address);

            if (address + 1 > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Write 2 bytes at 0x{address:X16} crossed hardware memory boundaries");
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            memory.WriteUShortAt(value, offsetInMemory);
        }

        public void WriteShort(short value, ulong address) => WriteUShort((ushort)value, address);

        public void WriteUInt(uint value, ulong address)
        {
            var mapping = FirstMapping(address);

            if (address + 3 > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Write 4 bytes at 0x{address:X16} crossed hardware memory boundaries");
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            memory.WriteUIntAt(value, offsetInMemory);
        }

        public void WriteInt(int value, ulong address) => WriteUInt((uint)value, address);

        public void WriteULong(ulong value, ulong address)
        {
            var mapping = FirstMapping(address);

            if (address + 3 > mapping.EndAddress)
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Write 4 bytes at 0x{address:X16} crossed hardware memory boundaries");
            }

            ByteBlock memory = mapping.MemoryReference;
            ulong offsetInMemory = address % memory.Length;

            memory.WriteULongAt(value, offsetInMemory);
        }

        public void WriteLong(long value, ulong address) => WriteULong((ulong)value, address);

        private HardwareMemoryMapping FirstMapping(ulong address)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < mappings.Count; i++)
            {
                var mapping = mappings[i];

                if (address >= mapping.StartAddress && address <= mapping.EndAddress) { return mapping; }
            }

            throw new VMErrorException(Error.NoHardwareMemoryHere, $"Access 0x{address:X16} in area with no mapped hardware memory");
        }
    }
}
