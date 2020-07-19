using System;
using System.Collections.Generic;
using System.Text;

namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareDevice
    {
        private readonly List<HardwareCall> hardwareCalls;

        public string DeviceName { get; }
        public IReadOnlyList<HardwareCall> HardwareCalls => hardwareCalls.AsReadOnly();

        public HardwareDevice(string deviceName, IList<HardwareCall> hardwareCalls)
        {
            DeviceName = deviceName;
            this.hardwareCalls = (List<HardwareCall>)hardwareCalls;
        }
    }
}
