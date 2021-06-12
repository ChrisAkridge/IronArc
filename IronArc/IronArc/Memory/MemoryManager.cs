using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageTable = System.Collections.Generic.Dictionary<ulong, ulong>;

namespace IronArc.Memory
{
    public class MemoryManager
    {
        private const ulong PlaneMask = 0x007FFFFFFFFFFFFF;
        private const ulong PageMask = 0x00FFFFFFFFFFF000;
        private const ulong AddressInPageMask = 0x0000000000000FFF;
        private const int PageSize = 4096;

        private ByteBlock systemMemory;
        private ulong nextPageAllocationAddress;
        private uint nextPageTableId;
        private IDictionary<uint, PageTable> pageTables = new Dictionary<uint, PageTable>();
        private readonly HardwareMemory hardwareMemory;

        public bool PerformAddressTranslation { get; set; }
        public uint CurrentPageTableId { get; private set; }

        public ulong SystemMemoryLength => systemMemory.Length;

        public MemoryManager(ByteBlock systemMemory, HardwareMemory hardwareMemory)
        {
            this.systemMemory = systemMemory;
            this.hardwareMemory = hardwareMemory;
            
            pageTables.Add(0, new PageTable());
        }

        public uint CreatePageTable()
        {
            nextPageTableId += 1;
            uint createdPageTableId = nextPageTableId;
            pageTables.Add(createdPageTableId, new PageTable());

            return createdPageTableId;
        }

        public void DestroyPageTable(uint pageTableId)
        {
            if (!pageTables.ContainsKey(pageTableId))
            {
                return;
            }

            if (CurrentPageTableId == pageTableId)
            {
                throw new VMErrorException(Error.CannotDestroyCurrentPageTable, $"Page table {CurrentPageTableId} is in use; cannot destroy");
            }

            pageTables.Remove(pageTableId);
        }

        public bool TryChangePageTable(uint newPageTableId)
        {
            if (!pageTables.ContainsKey(newPageTableId)) { return false; }

            CurrentPageTableId = newPageTableId;

            return true;
        }

        public byte[] Read(ulong address, ulong length)
        {
            ulong endAddress = address + length;
            ulong plane = GetPlaneOfAddress(address);

            if (plane != GetPlaneOfAddress(endAddress))
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Read 0x{address:X16}-0x{endAddress:X16} crossed planes");
            }

            if (plane == 0)
            {
                return !PerformAddressTranslation
                    ? systemMemory.ReadAt(address, length)
                    : ReadVirtual(address, length);
            }

            if (plane == 1)
            {
                return hardwareMemory.Read(address, (int)length);
            }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Read 0x{address:X16}-0x{endAddress:X16} is in reserved plane");
        }

        private byte[] ReadVirtual(ulong address, ulong length)
        {
            ulong endAddress = address + length;
            byte[] buffer = new byte[length];
            PageTable currentPageTable = pageTables[CurrentPageTableId];
            int bufferIndex = 0;

            for (ulong page = GetPage(address); page < GetPage(endAddress); page += PageSize)
            {
                if (!currentPageTable.ContainsKey(page))
                {
                    PageFault(page);
                }

                ulong addressInPage = address & AddressInPageMask;
                int bytesToRead = (length >= 4096) ? PageSize - (int)addressInPage : (int)length;
                systemMemory.ReadIntoBuffer(buffer, bufferIndex, currentPageTable[page] + addressInPage, bytesToRead);

                address += (ulong)bytesToRead;
                length -= (ulong)bytesToRead;
                bufferIndex += bytesToRead;
            }

            return buffer;
        }

        public byte ReadByte(ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

            if (plane == 0)
            {
                if (!PerformAddressTranslation)
                {
                    return systemMemory.ReadByteAt(address);
                }

                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);

                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }

                ulong addressInPage = address & AddressInPageMask;

                return systemMemory.ReadByteAt(currentPageTable[page] + addressInPage);
            }

            if (plane == 1)
            {
                return hardwareMemory.ReadByte(address);
            }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Read byte at 0x{address:X16} in reserved plane");
        }

        public byte ReadByteInPageTable(ulong address, uint pageTableId)
        {
            PageTable pageTable = pageTables[pageTableId];
            ulong page = GetPage(address);

            if (!pageTable.ContainsKey(page)) { return 0; }

            ulong addressInPage = address & AddressInPageMask;

            return systemMemory.ReadByteAt(pageTable[page] + addressInPage);
        }

        public sbyte ReadSByte(ulong address) => (sbyte)ReadByte(address);

        public ushort ReadUShort(ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

            if (GetPlaneOfAddress(plane + 1) != plane)
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Read 2 bytes at 0x{address:X16} crossed planes");
            }

            if (plane == 0)
            {
                if (!PerformAddressTranslation) { return systemMemory.ReadUShortAt(address); }
                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);

                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }

                // Scenario 1: the ushort occurs entirely within a page
                if (address % PageSize != 4095)
                {
                    ulong addressInPage = address & AddressInPageMask;
                    return systemMemory.ReadUShortAt(currentPageTable[page] + addressInPage);
                }

                // Scenario 2: the ushort is split between two pages
                ulong secondPage = page + PageSize;
                if (!currentPageTable.ContainsKey(secondPage)) { PageFault(page); }

                byte lo = systemMemory.ReadByteAt(currentPageTable[page] + (PageSize - 1));
                byte hi = systemMemory.ReadByteAt(currentPageTable[secondPage]);

                return (ushort)((hi << 8) | lo);
            }

            if (plane == 1)
            {
                return hardwareMemory.ReadUShort(address);
            }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Read 2 bytes at 0x{address:X16} in reserved plane");
        }

        public short ReadShort(ulong address) => (short)ReadUShort(address);

        public uint ReadUInt(ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);
            // WYLO: I think we need to remove the string table addressing mode and
            // instead have IronAssembler rewrite str:0 into a simple pointer directly
            // to the start of each string in the strings table. This also lets us remove
            // the number of strings in the table, plus all their pointers. This is to allow
            // things like hardware calls to get string pointers and not just string table
            // indices.
            //
            // The IronAssembler file format is unchanged, but we will need a number of
            // spec changes.

            if (GetPlaneOfAddress(plane + 3) != plane)
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Read 4 bytes at 0x{address:X16} crossed planes");
            }

            if (plane == 0)
            {
                if (!PerformAddressTranslation) { return systemMemory.ReadUIntAt(address); }

                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);

                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }

                if (address % PageSize <= 4092)
                {
                    ulong addressInPage = address & AddressInPageMask;
                    return systemMemory.ReadUIntAt(currentPageTable[page] + addressInPage);
                }

                ulong secondPage = page + PageSize;
                if (!currentPageTable.ContainsKey(secondPage)) { PageFault(page); }

                byte a = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address));
                byte b = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 1));
                byte c = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 2));
                byte d = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 3));

                return (uint)((a << 24) | (b << 16) | (c << 8) | d);
            }

            if (plane == 1)
            {
                return hardwareMemory.ReadUInt(address);
            }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Read 4 bytes at 0x{address:X16} in reserved plane");
        }

        public int ReadInt(ulong address) => (int)ReadUInt(address);

        public ulong ReadULong(ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

            if (GetPlaneOfAddress(plane + 7) != plane)
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Read 8 bytes at 0x{address:X16} crossed planes");
            }

            if (plane == 0)
            {
                if (!PerformAddressTranslation) { return systemMemory.ReadUIntAt(address); }

                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);

                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }

                if (address % PageSize <= 4088)
                {
                    ulong addressInPage = address & AddressInPageMask;

                    return systemMemory.ReadULongAt(currentPageTable[page] + addressInPage);
                }

                ulong secondPage = page + PageSize;

                if (!currentPageTable.ContainsKey(secondPage)) { PageFault(page); }

                byte a = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address));
                byte b = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 1));
                byte c = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 2));
                byte d = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 3));
                byte e = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 4));
                byte f = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 5));
                byte g = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 6));
                byte h = systemMemory.ReadByteAt(TranslateAddress(currentPageTable, address + 7));

                return ((ulong)a << 56)
                    | ((ulong)b << 48)
                    | ((ulong)c << 40)
                    | ((ulong)d << 32)
                    | ((ulong)e << 24)
                    | ((ulong)f << 16)
                    | ((ulong)g << 8)
                    | h;
            }

            if (plane == 1) { return hardwareMemory.ReadULong(address); }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Read 8 bytes 0x{address:X16} in reserved plane");
        }

        public long ReadLong(ulong address) => (long)ReadULong(address);

        public string ReadString(ulong address, out uint length)
        {
            uint stringLength = ReadUInt(address);

            length = stringLength + 4;
            return Encoding.UTF8.GetString(Read(address + 4, stringLength));
        }

        public ulong ReadData(ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    return ReadByte(address);
                case OperandSize.Word:
                    return ReadUShort(address);
                case OperandSize.DWord:
                    return ReadUInt(address);
                case OperandSize.QWord:
                    return ReadULong(address);
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }

        public void Write(byte[] bytes, ulong address)
        {
            ulong endAddress = address + (ulong)bytes.Length;
            ulong plane = GetPlaneOfAddress(address);

            if (plane != GetPlaneOfAddress(endAddress))
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Write 0x{address:X16}-0x{endAddress:x16} crossed planes");
            }

            if (plane == 0)
            {
                if (PerformAddressTranslation)
                {
                    WriteVirtual(bytes, address);
                }
                else { systemMemory.WriteAt(bytes, address); }

                return;
            }
            else if (plane == 1)
            {
                hardwareMemory.Write(bytes, address);

                return;
            }

            throw new VMErrorException(Error.ReservedPlaneAccess,
                $"Write 0x{address:X16}-0x{endAddress:X16} is in reserved plane");
        }

        private void WriteVirtual(byte[] bytes, ulong address)
        {
            var length = (ulong)bytes.Length;
            int bufferIndex = 0;
            ulong endAddress = address + length;
            PageTable currentPageTable = pageTables[CurrentPageTableId];

            for (ulong page = GetPage(address); page < GetPage(endAddress); page += PageSize)
            {
                if (!currentPageTable.ContainsKey(page))
                {
                    PageFault(page);
                }

                ulong addressInPage = address & AddressInPageMask;
                int bytesToWrite = (length >= PageSize) ? PageSize - (int)addressInPage : (int)length;
                systemMemory.WriteAt(bytes, bufferIndex, bytesToWrite, currentPageTable[page] + addressInPage);

                address += (ulong)bytesToWrite;
                length -= (ulong)bytesToWrite;
                bufferIndex += bytesToWrite;
            }
        }

        public void Write(ulong source, ulong destination, ulong length)
        {
            ulong sourceEndAddress = source + length;
            ulong destinationEndAddress = destination + length;
            ulong sourcePlane = GetPlaneOfAddress(source);
            ulong destinationPlane = GetPlaneOfAddress(destination);

            if (sourcePlane != GetPlaneOfAddress(sourceEndAddress))
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Write from 0x{source:X16}-0x{sourceEndAddress:x16} crossed planes");
            }

            if (destinationPlane != GetPlaneOfAddress(destinationEndAddress))
            {
                throw new VMErrorException(Error.CrossPlaneAccess,
                    $"Write to 0x{destination:X16}-0x{destinationEndAddress:x16} crossed planes");
            }

            byte[] bytes = Read(source, length);
            Write(bytes, destination);
        }
        
        public void WriteByte(byte value, ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

            if (plane == 0)
            {
                if (!PerformAddressTranslation)
                {
                    systemMemory.WriteByteAt(value, address);
                    return;
                }

                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);
                
                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }

                ulong addressInPage = address & AddressInPageMask;
                
                systemMemory.WriteByteAt(value, currentPageTable[page] + addressInPage);
            }
            else if (plane == 1)
            {
                hardwareMemory.WriteByte(value, address);
            }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Write byte at 0x{address:X16} in reserved plane");
        }

        public void WriteByteInPageTable(byte value, ulong address, uint pageTableId)
        {
            PageTable pageTable = pageTables[pageTableId];
            ulong page = GetPage(address);

            if (!pageTable.ContainsKey(page)) { PageFault(page); }

            ulong addressInPage = address & AddressInPageMask;
            systemMemory.WriteByteAt(value, pageTable[page] + addressInPage);
        }

        public void WriteSByte(sbyte value, ulong address) => WriteByte((byte)value, address);

        public void WriteUShort(ushort value, ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

            if (GetPlaneOfAddress(plane + 1) != plane)
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Write 2 bytes at 0x{address:X16} crossed planes");
            }

            if (plane == 0)
            {
                if (!PerformAddressTranslation)
                {
                    systemMemory.WriteUShortAt(value, address);
                    return;
                }

                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);

                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }
                
                if (address % PageSize != 4095)
                {
                    ulong addressInPage = address & AddressInPageMask;

                    systemMemory.WriteUShortAt(value, currentPageTable[page] + addressInPage);
                }
                else
                {
                    ulong secondPage = page + PageSize;

                    if (!currentPageTable.ContainsKey(secondPage)) { PageFault(page); }

                    byte lo = (byte)(value >> 8);
                    byte hi = (byte)(value & 0xFF);

                    systemMemory.WriteByteAt(lo, currentPageTable[page] + (PageSize - 1));
                    systemMemory.WriteByteAt(hi, currentPageTable[secondPage]);
                }
            }
            else if (plane == 1) { hardwareMemory.WriteUShort(value, address); }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Write 2 bytes at 0x{address:X16} in reserved plane");
        }

        public void WriteShort(short value, ulong address) => WriteUShort((ushort)value, address);

        public void WriteUInt(uint value, ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

            if (GetPlaneOfAddress(plane + 3) != plane)
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Write 4 bytes at 0x{address:X16} crossed planes");
            }

            if (plane == 0)
            {
                if (!PerformAddressTranslation)
                {
                    systemMemory.WriteUIntAt(value, address);
                    return;
                }

                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);

                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }

                if (address % PageSize <= 4092)
                {
                    ulong addressInPage = address & AddressInPageMask;

                    systemMemory.WriteUIntAt(value, currentPageTable[page] + addressInPage);
                }
                else
                {
                    ulong secondPage = page + PageSize;

                    if (!currentPageTable.ContainsKey(secondPage)) { PageFault(page); }

                    byte a = (byte)(value >> 24);
                    byte b = (byte)((value >> 16) & 0xFF);
                    byte c = (byte)((value >> 8) & 0xFF);
                    byte d = (byte)(value & 0xFF);

                    systemMemory.WriteByteAt(a, TranslateAddress(currentPageTable, address));
                    systemMemory.WriteByteAt(b, TranslateAddress(currentPageTable, address + 1));
                    systemMemory.WriteByteAt(c, TranslateAddress(currentPageTable, address + 2));
                    systemMemory.WriteByteAt(d, TranslateAddress(currentPageTable, address + 3));
                }
            }
            else if (plane == 1) { hardwareMemory.WriteUInt(value, address); }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Write 4 bytes at 0x{address:X16} in reserved plane");
        }

        public void WriteInt(int value, ulong address) => WriteUInt((uint)value, address);

        public void WriteULong(ulong value, ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

            if (GetPlaneOfAddress(plane + 7) != plane)
            {
                throw new VMErrorException(Error.CrossPlaneAccess, $"Write 8 bytes at 0x{address:X16} crossed planes");
            }

            if (plane == 0)
            {
                if (!PerformAddressTranslation)
                {
                    systemMemory.WriteULongAt(value, address);
                    return;
                }

                PageTable currentPageTable = pageTables[CurrentPageTableId];
                ulong page = GetPage(address);

                if (!currentPageTable.ContainsKey(page)) { PageFault(page); }

                if (address % PageSize <= 4088)
                {
                    ulong addressInPage = address & AddressInPageMask;

                    systemMemory.WriteULongAt(value, currentPageTable[page] + addressInPage);
                }
                else
                {
                    ulong secondPage = page + PageSize;

                    if (!currentPageTable.ContainsKey(secondPage)) { PageFault(page); }

                    byte a = (byte)(value >> 56);
                    byte b = (byte)((value >> 48) & 0xFF);
                    byte c = (byte)((value >> 40) & 0xFF);
                    byte d = (byte)((value >> 32) & 0xFF);
                    byte e = (byte)((value >> 24) & 0xFF);
                    byte f = (byte)((value >> 16) & 0xFF);
                    byte g = (byte)((value >> 8) & 0xFF);
                    byte h = (byte)(value & 0xFF);

                    systemMemory.WriteByteAt(a, TranslateAddress(currentPageTable, address));
                    systemMemory.WriteByteAt(b, TranslateAddress(currentPageTable, address + 1));
                    systemMemory.WriteByteAt(c, TranslateAddress(currentPageTable, address + 2));
                    systemMemory.WriteByteAt(d, TranslateAddress(currentPageTable, address + 3));
                    systemMemory.WriteByteAt(e, TranslateAddress(currentPageTable, address + 4));
                    systemMemory.WriteByteAt(f, TranslateAddress(currentPageTable, address + 5));
                    systemMemory.WriteByteAt(g, TranslateAddress(currentPageTable, address + 6));
                    systemMemory.WriteByteAt(h, TranslateAddress(currentPageTable, address + 7));
                }
            }
            else if (plane == 1) { hardwareMemory.WriteULong(value, address); }

            throw new VMErrorException(Error.ReservedPlaneAccess,
                $"Write 8 bytes at 0x{address:X16} in reserved plane");
        }

        public void WriteLong(long value, ulong address) => WriteULong((ulong)value, address);

        public void WriteString(string value, ulong address)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(value);
            uint stringLength = (uint)utf8.Length;
            WriteUInt(stringLength, address);
            Write(utf8, address + 4);
        }

        public void WriteData(ulong data, ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    WriteByte((byte)data, address);
                    break;
                case OperandSize.Word:
                    WriteUShort((ushort)data, address);
                    break;
                case OperandSize.DWord:
                    WriteUInt((uint)data, address);
                    break;
                case OperandSize.QWord:
                    WriteULong(data, address);
                    break;
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }

        private static ulong TranslateAddress(PageTable pageTable, ulong address)
        {
            ulong offsetIntoPage = address % PageSize;
            return pageTable[GetPage(address)] + offsetIntoPage;
        }

        private void PageFault(ulong page)
        {
            if (nextPageAllocationAddress + PageSize > systemMemory.Length)
            {
                CompactPages();
            }

            if (nextPageAllocationAddress + PageSize > systemMemory.Length)
            {
                throw new VMErrorException(Error.OutOfVirtualMemory);
            }
            
            ulong startRealAddress = nextPageAllocationAddress;
            pageTables[CurrentPageTableId].Add(page, startRealAddress);

            nextPageAllocationAddress += PageSize;
        }

        private void CompactPages()
        {
            var compactor = new PageCompactor(pageTables, systemMemory);
            pageTables = compactor.CompactPages(out ulong newNextRealPageStartAddress);
            nextPageAllocationAddress = newNextRealPageStartAddress;
        }

        private static ulong GetPlaneOfAddress(ulong address) => (address & PlaneMask) >> 47;
        private static ulong GetPage(ulong address) => address & PageMask;
    }
}
