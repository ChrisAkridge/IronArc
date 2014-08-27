using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// Represents a virtual form of long-term storage.
    /// </summary>
	[Obsolete]
    public sealed class Diskfile
    {
        private string filePath;
        private int length;
        public ByteBlock File { get; private set; }

        public byte this[int index]
        {
            get
            {
                if (this.filePath == null)
                {
                    new SystemError("DiskfileNotLoaded", "Tried to read from a diskfile that was not loaded.").WriteToError();
                }
                else if (index >= this.length)
                {
                    new SystemError("DiskfileReadIndexOutOfRange", "Tried to read a byte above the range of the diskfile.").WriteToError();
                }

                return this.File[index];
            }

            set
            {
                if (this.filePath == null)
                {
                    new SystemError("DiskfileNotLoaded", "Tried to read from a diskfile that was not loaded.").WriteToError();
                }
                else if (index >= this.length)
                {
                    new SystemError("DiskfileReadIndexOutOfRange", "Tried to read a byte above the range of the diskfile.").WriteToError();
                }

                this.File[index] = value;
            }
        }

        public Diskfile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                new SystemError("DiskfileDoesntExist", string.Format("The diskfile at {0} does not exist.")).WriteToError();
            }

            this.filePath = filePath;
            this.File = System.IO.File.ReadAllBytes(filePath);
            this.length = this.File.Length;
        }

        public void SaveToDisk()
        {
            System.IO.File.WriteAllBytes(this.filePath, this.File.ToByteArray());
        }
    }
}
