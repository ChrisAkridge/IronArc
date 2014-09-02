using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexControlLibrary
{
    public interface IByteProvider
    {
        byte GetByte(int offset);
        int Length { get; }
    }
}
