using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexControlLibrary
{
    class DefaultBytePrivider : IByteProvider
    {
        public byte GetByte(int offset)
        {
            return 0;
        }

        public int Length
        {
            get
            {
                return 0;
            }
        }
    }
}
