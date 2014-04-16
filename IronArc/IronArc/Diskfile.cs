using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IronArc
{
    public sealed class Diskfile
    {
        private string filePath;
        private int length;
        private byte[] file;

        public byte this[int index]
        {
            get
            {
                if (filePath == null)
                {
                    new SystemError("DiskfileNotLoaded", "Tried to read from a diskfile that was not loaded.").WriteToError();
                }
                else if (index >= this.length)
                {
                    new SystemError("DiskfileReadIndexOutOfRange", "Tried to read a byte above the range of the diskfile.").WriteToError();
                }
                return this.file[index];
            }
            set
            {
                if (filePath == null)
                {
                    new SystemError("DiskfileNotLoaded", "Tried to read from a diskfile that was not loaded.").WriteToError();
                }
                else if (index >= this.length)
                {
                    new SystemError("DiskfileReadIndexOutOfRange", "Tried to read a byte above the range of the diskfile.").WriteToError();
                }
                this.file[index] = value;
            }
        }

        public Diskfile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                new SystemError("DiskfileDoesntExist", string.Format("The diskfile at {0} does not exist.")).WriteToError();
            }

            this.filePath = filePath;
            this.file = File.ReadAllBytes(filePath);
            this.length = this.file.Length;
        }

        public void SaveToDisk()
        {
            File.WriteAllBytes(this.filePath, this.file);
        }
    }
}
