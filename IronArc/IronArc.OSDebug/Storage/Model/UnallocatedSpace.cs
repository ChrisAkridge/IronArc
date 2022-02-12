using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.OSDebug.Storage.Model
{
    public sealed class UnallocatedSpace : StorageFileDivision
    {
        public long AbsoluteStartAddress { get; set; }
        public long Length { get; set; }
    }
}
