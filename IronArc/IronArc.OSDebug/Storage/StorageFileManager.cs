using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.OSDebug.Extensions;
using IronArc.OSDebug.Storage.Model;

namespace IronArc.OSDebug.Storage
{
    public sealed class StorageFileManager : IDisposable
    {
        private readonly FileStream stream;
        private readonly PartitionTableHeader tableHeader;
        private List<StorageFileDivision> divisions;
        
        public string FilePath { get; }
        public IReadOnlyList<StorageFileDivision> Divisions => divisions.AsReadOnly();
        public int SectorSize => tableHeader?.SectorSize ?? throw new InvalidOperationException();
        public long FileSize => stream.Length;

        public event EventHandler DivisionListUpdated;

        public StorageFileManager(string storageFilePath)
        {
            stream = File.Open(storageFilePath, FileMode.Open, FileAccess.ReadWrite);
            FilePath = storageFilePath;

            tableHeader = ParsePartitionTableHeader();
            divisions = DetermineAllocatedSpace();
        }

        public static StorageFileManager CreateAndFormatNewFile(string filePath, long fileSize, int sectorSize)
        {
            if (!sectorSize.IsValidSectorSize())
            {
                throw new ArgumentException($"Tried to create a file with an invalid sector size of {sectorSize} bytes; must be a power of 2 that is at least 32 bytes.",
                    nameof(sectorSize));
            }
            
            var newTableHeader = new PartitionTableHeader(new List<Partition>())
            {
                MagicNumber = PartitionTableHeader.PartitionTableMagicNumber,
                PartitionCount = 0,
                SectorSize = sectorSize
            };

            // https://stackoverflow.com/a/8417606/2709212
            using (var fileStream =
                new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
            {
                fileStream.SetLength(fileSize);
                fileStream.Position = 0L;
                newTableHeader.Write(new BinaryWriter(fileStream));
            }
            
            return new StorageFileManager(filePath);
        }
        
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            stream?.Dispose();
        }
        
        private PartitionTableHeader ParsePartitionTableHeader()
        {
            var reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: true);

            var magicNumber = reader.ReadInt32();

            if (magicNumber != PartitionTableHeader.PartitionTableMagicNumber)
            {
                throw new ArgumentException("Storage file does not have a valid partition table (magic number doesn't match).");
            }

            var sectorSize = reader.ReadInt32();
            var partitionCount = reader.ReadInt32();
            var partitions = new List<Partition>(partitionCount);

            if (partitionCount <= 0)
            {
                throw new ArgumentException("Partition table header declares zero or fewer partitions");    
            }
            
            for (int i = 0; i < partitionCount; i++)
            {
                partitions[i] = ParsePartition(reader);
            }
            
            reader.Dispose();

            return new PartitionTableHeader(partitions)
            {
                MagicNumber = magicNumber,
                PartitionCount = partitionCount,
                SectorSize = sectorSize
            };
        }

        private static Partition ParsePartition(BinaryReader reader)
        {
            var absoluteStartAddress = reader.ReadInt64();
            var length = reader.ReadInt64();

            var nameBytes = new byte[64];
            var readNameBytes = reader.Read(nameBytes, 0, 64);

            if (readNameBytes != 64)
            {
                throw new ArgumentException("Storage file ended when reading partition name");
            }

            var name = Encoding.UTF8.GetString(nameBytes);
            var isBootable = reader.ReadByte() != 0;
            var osBootProgramPartitionAddress = reader.ReadInt64();
            var osBootProgramLength = reader.ReadInt32();

            return new Partition
            {
                AbsoluteStartAddress = absoluteStartAddress,
                Length = length,
                Name = name,
                IsBootable = isBootable,
                OSBootProgramPartitionAddress = osBootProgramPartitionAddress,
                OSBootProgramLength = osBootProgramLength
            };
        }

        private List<StorageFileDivision> DetermineAllocatedSpace()
        {
            var partitionRanges = new List<Tuple<Partition, AddressRange>>();
            var unallocatedRanges = new List<Tuple<UnallocatedSpace, AddressRange>>();

            // gotta say, this whole project feels kinda haphazard
            var partitionsByAddress = Enumerable.Range(0, tableHeader.PartitionCount)
                .Select(i => tableHeader.GetPartition(i))
                .OrderBy(p => p.AbsoluteStartAddress);
            partitionRanges.AddRange(partitionsByAddress
                .Select(p => new Tuple<Partition, AddressRange>(p, new AddressRange
                {
                    Start = p.AbsoluteStartAddress,
                    End = p.AbsoluteEndAddress
                })));

            if (partitionRanges[0].Item2.Start > tableHeader.TableSize)
            {
                // There's a gap between the table and the first partition.
                unallocatedRanges.Add(new Tuple<UnallocatedSpace, AddressRange>(new UnallocatedSpace
                {
                    AbsoluteStartAddress = tableHeader.TableSize,
                    Length = partitionRanges[0].Item2.Start - tableHeader.TableSize
                }, new AddressRange
                {
                    Start = tableHeader.TableSize,
                    End = partitionRanges[0].Item2.Start - 1
                }));
            }

            for (int i = 0; i < partitionRanges.Count - 1; i++)
            {
                var prevPartitionEndAddress = partitionRanges[i].Item2.End;
                var nextPartitionStartAddress = partitionRanges[i + 1].Item2.Start;

                if (nextPartitionStartAddress - prevPartitionEndAddress > 1L)
                {
                    unallocatedRanges.Add(new Tuple<UnallocatedSpace, AddressRange>(new UnallocatedSpace
                    {
                        AbsoluteStartAddress = prevPartitionEndAddress + 1,
                        Length = nextPartitionStartAddress - prevPartitionEndAddress
                    }, new AddressRange
                    {
                        Start = prevPartitionEndAddress + 1,
                        End = nextPartitionStartAddress - 1
                    }));
                }
            }

            if (partitionRanges.Last().Item2.End < stream.Length - 1)
            {
                // There's a gap between the last partition and the end of the file.
                unallocatedRanges.Add(new Tuple<UnallocatedSpace, AddressRange>(new UnallocatedSpace
                {
                    AbsoluteStartAddress = partitionRanges.Last().Item2.End + 1,
                    Length = stream.Length - partitionRanges.Last().Item2.End
                }, new AddressRange
                {
                    Start = partitionRanges.Last().Item2.End + 1,
                    End = stream.Length - 1
                }));
            }

            // okay now there's no doubt
            return partitionRanges
                .Select(t => new Tuple<StorageFileDivision, AddressRange>(t.Item1, t.Item2))
                .Concat(unallocatedRanges
                    .Select(t => new Tuple<StorageFileDivision, AddressRange>(t.Item1, t.Item2)))
                .OrderBy(t => t.Item2.Start)
                .Select(t => t.Item1)
                .ToList();
        }

        private void WritePartitionTable()
        {
            using (var writer = new BinaryWriter(stream)) { tableHeader.Write(writer); }
        }

        public void FormatEntireFile(int sectorSize, bool eraseSectors, IProgress<long> progress)
        {
            if (!sectorSize.IsValidSectorSize())
            {
                throw new ArgumentException($"Sector size must be a power of two which is at least 32 bytes. Got {sectorSize} bytes, instead.",
                    nameof(sectorSize));
            }

            divisions.Clear();
            tableHeader.ClearPartitions();
            stream.Position = 0L;

            WritePartitionTable();

            if (!eraseSectors) { return; }

            long writtenBytes = 0L;
            while (stream.Position < stream.Length - 1)
            {
                stream.WriteByte(0);
                writtenBytes += 1L;

                if (writtenBytes % sectorSize == 0)
                {
                    progress.Report(writtenBytes / sectorSize);
                }
            }
        }

        public void AllocateUnallocatedSpace(int divisionIndex, string name, bool isBootable, long osBootProgramPartitionAddress,
            int osBootProgramLength)
        {
            if (divisionIndex < 0 || divisionIndex >= divisions.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(divisionIndex),
                    $"Tried to allocate storage division #{divisionIndex + 1}, but there are only {divisions.Count} divisions.");
            }

            var division = divisions[divisionIndex];

            if (division is Partition existingPartition)
            {
                throw new ArgumentException($"Tried to allocate storage division #{divisionIndex + 1}, but it was already allocated as {existingPartition.Name}");
            }

            var unallocatedSpace = division as UnallocatedSpace;
            var nameByteCount = Encoding.UTF8.GetByteCount(name);

            if (nameByteCount > 64)
            {
                throw new ArgumentOutOfRangeException($"IronArc partition names can only be 64 bytes or less; received a name that was {nameByteCount} bytes.");
            }
            
            var newPartition = new Partition
            {
                AbsoluteStartAddress = unallocatedSpace.AbsoluteStartAddress,
                Length = unallocatedSpace.Length,
                IsBootable = isBootable,
                Name = name,
                OSBootProgramPartitionAddress = osBootProgramPartitionAddress,
                OSBootProgramLength = osBootProgramLength
            };
            
            tableHeader.InsertPartitionAtAddress(newPartition);
            WritePartitionTable();
            
            divisions = DetermineAllocatedSpace();
            OnDivisionListUpdated();
        }

        public void RemovePartition(int divisionIndex)
        {
            if (divisionIndex < 0 || divisionIndex >= divisions.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(divisionIndex),
                    $"Tried to remove partition in storage division #{divisionIndex + 1}, but there are only {divisions.Count} divisions.");
            }

            var division = divisions[divisionIndex];

            if (division is UnallocatedSpace)
            {
                throw new ArgumentException(
                    $"Tried to delete partition in storage division #{divisionIndex + 1}, but it already not allocated.");
            }
            
            tableHeader.RemovePartition(division as Partition);
            WritePartitionTable();
            
            divisions = DetermineAllocatedSpace();
            OnDivisionListUpdated();
        }

        public void ResizePartition(int divisionIndex, long newSize)
        {
            if (newSize % SectorSize != 0)
            {
                throw new ArgumentException($"Tried to resize sector to {newSize} bytes, which was not evenly divisble by the sector size of {SectorSize} bytes.");
            }
            
            if (divisionIndex < 0 || divisionIndex >= divisions.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(divisionIndex),
                    $"Tried to resize partition in storage division #{divisionIndex + 1}, but there are only {divisions.Count} divisions.");
            }

            var division = divisions[divisionIndex];

            if (division is UnallocatedSpace)
            {
                throw new ArgumentException(
                    $"Tried to resize partition in storage division #{divisionIndex + 1}, but it already not allocated.");
            }

            var existingPartition = division as Partition;
            var nextPartition = tableHeader.GetPartitionOrNull(existingPartition.ImplicitId + 1);

            if (nextPartition != null
                && existingPartition.AbsoluteStartAddress + newSize > nextPartition.AbsoluteStartAddress)
            {
                throw new ArgumentOutOfRangeException(nameof(newSize),
                    $"Cannot resize partition #{existingPartition.ImplicitId} to {newSize} bytes - overlaps next partition.");
            }
            
            tableHeader.RemovePartition(existingPartition);
            
            var newPartition = new Partition
            {
                AbsoluteStartAddress = existingPartition.AbsoluteStartAddress,
                IsBootable = existingPartition.IsBootable,
                Length = newSize,
                Name = existingPartition.Name,
                OSBootProgramLength = existingPartition.OSBootProgramLength,
                OSBootProgramPartitionAddress = existingPartition.OSBootProgramPartitionAddress
            };
            
            tableHeader.InsertPartitionAtAddress(newPartition);
            
            WritePartitionTable();

            divisions = DetermineAllocatedSpace();
            OnDivisionListUpdated();
        }

        private void OnDivisionListUpdated() => DivisionListUpdated?.Invoke(this, new EventArgs());
    }
}
