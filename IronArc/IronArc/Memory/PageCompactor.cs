using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Memory
{
    public sealed class PageCompactor
    {
        private const int PageSize = 4096;

        private readonly struct TotalVirtualAddress : IComparable<TotalVirtualAddress>
        {
            public uint PageTableId { get; }
            public ulong VirtualStartAddress { get; }

            public TotalVirtualAddress(uint pageTableId, ulong virtualStartAddress)
            {
                PageTableId = pageTableId;
                VirtualStartAddress = virtualStartAddress;
            }

            /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object. </summary>
            /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order. </returns>
            /// <param name="other">An object to compare with this instance. </param>
            public int CompareTo(TotalVirtualAddress other)
            {
                var pageTableIdComparison = PageTableId.CompareTo(other.PageTableId);

                return pageTableIdComparison != 0
                    ? pageTableIdComparison
                    : VirtualStartAddress.CompareTo(other.VirtualStartAddress);
            }
        }

        private readonly Dictionary<TotalVirtualAddress, ulong> vrPageTable = new Dictionary<TotalVirtualAddress, ulong>();
        private readonly Dictionary<ulong, TotalVirtualAddress> rvPageTable = new Dictionary<ulong, TotalVirtualAddress>();
        private ulong nextRealPageStartAddress;
        private ByteBlock memory;

        public PageCompactor(IDictionary<uint, Dictionary<ulong, ulong>> pageTables, ByteBlock memory)
        {
            this.memory = memory;

            foreach (var pageTable in pageTables)
            {
                foreach (var pageTableEntry in pageTable.Value)
                {
                    var totalVirtualAddress = new TotalVirtualAddress(pageTable.Key, pageTableEntry.Key);
                    vrPageTable.Add(totalVirtualAddress, pageTableEntry.Value);
                    rvPageTable.Add(pageTableEntry.Value, totalVirtualAddress);
                }
            }
        }
        
        public IDictionary<uint, Dictionary<ulong, ulong>> CompactPages(out ulong newNextRealPageStartAddress)
        {
            foreach (var oldPageMapping in vrPageTable.OrderBy(map => map.Key))
            {
                SwapRealPages(oldPageMapping.Value, nextRealPageStartAddress);
                nextRealPageStartAddress += PageSize;
            }

            newNextRealPageStartAddress = nextRealPageStartAddress;
            return BuildNewPageTables();
        }

        private void SwapRealPages(ulong pageA, ulong pageB)
        {
            var virtualAddressForA = rvPageTable[pageA];
            var virtualAddressForB = rvPageTable[pageB];
            rvPageTable[pageA] = virtualAddressForB;
            rvPageTable[pageB] = virtualAddressForA;

            var buffer = new byte[PageSize];
            memory.ReadIntoBuffer(buffer, 0, pageA, PageSize);
            memory.WriteAt(pageB, pageA, PageSize);
            memory.WriteAt(buffer, pageA);
        }

        private IDictionary<uint, Dictionary<ulong, ulong>> BuildNewPageTables()
        {
            var newPageTable = new Dictionary<uint, Dictionary<ulong, ulong>>();

            foreach (var rvPage in rvPageTable)
            {
                var pageTableId = rvPage.Value.PageTableId;

                if (!newPageTable.ContainsKey(pageTableId))
                {
                    newPageTable.Add(pageTableId, new Dictionary<ulong, ulong>());
                }
                
                newPageTable[pageTableId].Add(rvPage.Value.VirtualStartAddress, rvPage.Key);
            }

            return newPageTable;
        }
    }
}
