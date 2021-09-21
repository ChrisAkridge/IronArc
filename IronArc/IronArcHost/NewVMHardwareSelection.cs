using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.Hardware.Configuration;

namespace IronArcHost
{
    public sealed class NewVMHardwareSelection
    {
        public string DeviceTypeName { get; }
        public HardwareConfiguration Configuration { get; }
        
        public NewVMHardwareSelection(string deviceTypeName, HardwareConfiguration configuration)
        {
            DeviceTypeName = deviceTypeName;
            Configuration = configuration;
        }
    }
}
