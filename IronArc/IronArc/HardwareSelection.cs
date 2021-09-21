using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.Hardware.Configuration;

namespace IronArc
{
    public sealed class HardwareSelection
    {
        public string DeviceTypeName { get; }
        public HardwareConfiguration Configuration { get; }

        public HardwareSelection(string deviceTypeName, HardwareConfiguration configuration)
        {
            DeviceTypeName = deviceTypeName;
            Configuration = configuration;
        }
    }
}
