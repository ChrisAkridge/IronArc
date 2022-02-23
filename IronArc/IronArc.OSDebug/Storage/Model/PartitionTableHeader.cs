using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.OSDebug.Extensions;

namespace IronArc.OSDebug.Storage.Model
{
    public sealed class PartitionTableHeader
    {
        public const int PartitionTableMagicNumber = 0x49535054;   // "ISPT"
        private readonly List<Partition> partitions;
        
        public int MagicNumber { get; set; }
        public int SectorSize { get; set; }
        public int PartitionCount { get; set; }

        public bool MagicNumberIsValid => MagicNumber == PartitionTableMagicNumber;
        
        public bool SectorSizeIsValid => SectorSize.IsValidSectorSize();

        public long TableSize => 12 + (PartitionCount * 93);

        public PartitionTableHeader(List<Partition> partitions) => this.partitions = partitions;

        public Partition GetPartition(int index) => GetPartitionOrNull(index) ?? throw new ArgumentOutOfRangeException(nameof(index));

        public Partition GetPartitionOrNull(int index)
        {
            if (index < 0 || index >= PartitionCount) { return null; }

            var partition = partitions[index];
            partition.ImplicitId = index;

            return partition;
        }

        public void ClearPartitions() => partitions.Clear();

        public void InsertPartitionAtAddress(Partition partition)
        {
            if (!partitions.Any())
            {
                partitions.Add(partition);

                return;
            }

            int insertIndex = 0;
            foreach (var existingPartition in partitions)
            {
                insertIndex += 1;
                if (existingPartition.AbsoluteStartAddress > partition.AbsoluteEndAddress)
                {
                    break;
                }
            }
            
            partitions.Insert(insertIndex, partition);
            PartitionCount += 1;
        }

        public void RemovePartition(Partition partition)
        {
            partitions.Remove(partition);
            PartitionCount -= 1;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(MagicNumber);
            writer.Write(SectorSize);
            writer.Write(PartitionCount);

            foreach (var partition in partitions)
            {
                partition.Write(writer);
            }
        }
    }
}
