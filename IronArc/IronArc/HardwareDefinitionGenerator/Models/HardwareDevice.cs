using System.Collections.Generic;

namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareDevice
    {
        private readonly List<HardwareMethod> hardwareMethods;

        public string DeviceName { get; }
        public IReadOnlyList<HardwareMethod> HardwareMethods => hardwareMethods.AsReadOnly();

        public HardwareDevice(string deviceName, IList<HardwareMethod> hardwareCalls)
        {
            DeviceName = deviceName;
            hardwareMethods = (List<HardwareMethod>)hardwareCalls;
        }
    }
}
