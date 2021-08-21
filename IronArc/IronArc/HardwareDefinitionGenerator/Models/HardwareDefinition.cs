using System.Collections.Generic;

namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareDefinition
    {
        private readonly List<HardwareDevice> hardwareDevices;

        public string Version { get; }
        public IReadOnlyList<HardwareDevice> HardwareDevices => hardwareDevices.AsReadOnly();

        public HardwareDefinition(string version, IList<HardwareDevice> hardwareDevices)
        {
            Version = version;
            this.hardwareDevices = (List<HardwareDevice>)hardwareDevices;
        }
    }
}
