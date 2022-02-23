using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.OSDebug.Extensions
{
    public static class Int32Extensions
    {
        public static bool IsValidSectorSize(this int sectorSize) =>
            (sectorSize & (sectorSize - 1)) == 0 && sectorSize >= 32;
    }
}
