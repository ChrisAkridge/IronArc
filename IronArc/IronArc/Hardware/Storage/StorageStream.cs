using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Hardware.Storage
{
    internal sealed class StorageStream
    {
        private static uint previousStreamID;
        
        private FileStream fileStream;
        
        public uint StreamID { get; }
        public long StartAddress { get; }
        public long Length { get; }
        public long Position { get; private set; }

        public StorageStream(FileStream fileStream, long startAddress, long length)
        {
            if (startAddress + length >= fileStream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startAddress),
                    $"Cannot open stream at 0x{StartAddress:X16} to 0x{(StartAddress + Length):X16} - underlying file is only 0x{fileStream.Length:X16} bytes long");
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Provided length was not positive");
            }
            
            this.fileStream = fileStream;
            StartAddress = startAddress;
            Length = length;
            StreamID = previousStreamID++;
        }

        public void Seek(long newPosition)
        {
            if (newPosition < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newPosition),
                    "Position to seek to was negative");
            }
            if (newPosition > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(newPosition),
                    $"Cannot seek to 0x{newPosition:X16} - stream is only 0x{Length:X16} bytes long");
            }

            Position = newPosition;
        }

        public void Advance(long distance)
        {
            var newPosition = Position + distance;

            if (newPosition < 0 || newPosition > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(distance),
                    $"Cannot advance stream by 0x{distance:X16} bytes - new position would be 0x{newPosition:X16}, which is out of range (stream is 0x{Length:X16}) bytes long");
            }
            
            Seek(newPosition);
        }
    }
}
