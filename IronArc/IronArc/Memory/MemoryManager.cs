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
        private readonly Dictionary<uint, PageTable> pageTables = new Dictionary<uint, PageTable>();
        private readonly HardwareMemory hardwareMemory;

        public bool PerformAddressTranslation { get; set; }
        public uint CurrentPageTableId { get; set; }

        public MemoryManager(ulong systemMemorySize, HardwareMemory hardwareMemory)
        {
            systemMemory = ByteBlock.FromLength(systemMemorySize);
            this.hardwareMemory = hardwareMemory;
        }

        public uint CreatePageTable()
        {
            uint createdPageTableId = nextPageTableId;
            pageTables.Add(createdPageTableId, new PageTable());
            nextPageTableId += 1;

            return createdPageTableId;
        }

        public void DestroyPageTable(uint pageTableId)
        {
            if (!pageTables.ContainsKey(pageTableId))
            {
                throw new VMErrorException(Error.NoSuchPageTable, $"Cannot destroy non-existant page table {pageTableId}");
            }

            if (CurrentPageTableId == pageTableId)
            {
                throw new VMErrorException(Error.CannotDestroyCurrentPageTable, $"Page table {CurrentPageTableId} is in use; cannot destroy");
            }

            pageTables.Remove(pageTableId);
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

            }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Read 2 bytes at 0x{address:X16} in reserved plane");
        }

        public short ReadShort(ulong address) => (short)ReadUShort(address);

        public uint ReadUInt(ulong address)
        {
            ulong plane = GetPlaneOfAddress(address);

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

            if (plane == 1) { }

            throw new VMErrorException(Error.ReservedPlaneAccess, $"Read 8 bytes 0x{address:X16} in reserved plane");
        }

        public long ReadLong(ulong address) => (long)ReadULong(address);

        // TODO: Write the write methods
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

            ulong startRealAddress = nextPageAllocationAddress;
            pageTables[CurrentPageTableId].Add(page, startRealAddress);

            nextPageAllocationAddress += PageSize;
        }

        private void CompactPages()
        {
            int pagesOfSystemMemory = (int)(systemMemory.Length / PageSize);
            ulong?[] virtualAddressForPageIndexes = new ulong?[pagesOfSystemMemory];

            foreach (KeyValuePair<uint, PageTable> pageTable in pageTables)
            {
                foreach (KeyValuePair<ulong, ulong> pageTableEntry in pageTable.Value)
                {
                    int pageIndex = (int)(pageTableEntry.Value / PageSize);
                    virtualAddressForPageIndexes[pageIndex] = pageTableEntry.Key;
                }
            }

            var newVirtualAddressMappings = new List<KeyValuePair<int, ulong>>();

            for (int i = 0; i < pagesOfSystemMemory; i++)
            {
                if (virtualAddressForPageIndexes[i] == null) { continue; }

                newVirtualAddressMappings.Add(new KeyValuePair<int, ulong>(i, virtualAddressForPageIndexes[i].Value));
            }

            if (newVirtualAddressMappings.Count == pagesOfSystemMemory)
            {
                throw new VMErrorException(Error.OutOfVirtualMemory);
            }

            byte[] copyBuffer = new byte[4096];

            for (var i = 0; i < newVirtualAddressMappings.Count; i++)
            {
                var kvp = newVirtualAddressMappings[i];

                ulong sourcePage = (ulong)kvp.Key * PageSize;
                ulong destinationPage = (ulong)i * PageSize;

                systemMemory.ReadIntoBuffer(copyBuffer, 0, sourcePage, PageSize);
                systemMemory.WriteAt(copyBuffer, destinationPage);

                // TODO: page compaction algorithm is wrong - we need a separate class for this, probably
                //  and update the RealStartAddresses in the page table entry
                // TODO: catch a VMErrorException in the execute instruction method and raise an error there
                //  and figure out a way to pass the message to the debugger (last error message?)
                // TODO: fix the docs to list the details of the hardware calls and errors
                //  and remove ENP, we don't need it
                // TODO: add WriteXXX methods
                // TODO: add System device and the hardware calls it needs
            }

            nextPageAllocationAddress = (ulong)newVirtualAddressMappings.Count * PageSize;
        }

        private static ulong GetPlaneOfAddress(ulong address) => (address & PlaneMask) >> 47;
        private static ulong GetPage(ulong address) => address & PageMask;
    }
}
