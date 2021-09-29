using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Hardware.Configuration
{
    public sealed class StorageDeviceConfiguration : HardwareConfiguration
    {
        public string BaseFileAbsolutePath { get; set; }
    }
}
