using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.OSDebug.Storage.Model
{
    public sealed class PartitionTableHeader
    {
        public const int PartitionTableMagicNumber = 0x49535054;   // "ISPT"
        private readonly Partition[] partitions;
        
        public int MagicNumber { get; set; }
        public int SectorSize { get; set; }
        public int PartitionCount { get; set; }

        public bool MagicNumberIsValid => MagicNumber == PartitionTableMagicNumber;
        
        public bool SectorSizeIsValid =>
            (SectorSize & (SectorSize - 1)) == 0
            && SectorSize >= 32;

        public long TableSize => 12 + (PartitionCount * 93);

        public PartitionTableHeader(Partition[] partitions) => this.partitions = partitions;

        public Partition GetPartition(int index)
        {
            if (index < 0 || index >= PartitionCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var partition = partitions[index];
            partition.ImplicitId = index;

            return partition;
        }
    }
}
