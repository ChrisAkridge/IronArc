﻿using System;
using System.Collections.Generic;
using System.IO;
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

        public void Write(BinaryWriter writer)
        {
            writer.Write(AbsoluteStartAddress);
            writer.Write(Length);

            var nameBytes = Encoding.UTF8.GetBytes(Name);
            var nameBuffer = new byte[64];
            Array.Copy(nameBytes, nameBuffer, 64);
            writer.Write(nameBuffer);
            writer.Write(IsBootable ? (byte)1 : (byte)0);
            writer.Write(OSBootProgramPartitionAddress);
            writer.Write(OSBootProgramLength);
        }
    }
}
