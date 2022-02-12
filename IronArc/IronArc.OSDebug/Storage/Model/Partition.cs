using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.OSDebug.Storage.Model
{
    public sealed class Partition : StorageFileDivision
    {
        public long AbsoluteStartAddress { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public bool IsBootable { get; set; }
        public long OSBootProgramPartitionAddress { get; set; }
        public int OSBootProgramLength { get; set; }
        
        public int ImplicitId { get; set; }
        public long AbsoluteEndAddress => AbsoluteStartAddress + Length;
    }
}
