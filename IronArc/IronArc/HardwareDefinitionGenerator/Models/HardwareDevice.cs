using System.Collections.Generic;

namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareDevice
    {
        private readonly List<HardwareCall> hardwareCalls;

        public string DeviceName { get; }
        public IReadOnlyList<HardwareCall> HardwareCalls => hardwareCalls.AsReadOnly();
        
        // TODO: add Interrupts to this, and also to Cix so we can have a more type-safe way to register interrupt handlers

        public HardwareDevice(string deviceName, IList<HardwareCall> hardwareCalls)
        {
            DeviceName = deviceName;
            this.hardwareCalls = (List<HardwareCall>)hardwareCalls;
        }
    }
}
