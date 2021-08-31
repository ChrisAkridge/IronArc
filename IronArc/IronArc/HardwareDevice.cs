using System;
using IronArc.Memory;
using DefinitionDevice = IronArc.HardwareDefinitionGenerator.Models.HardwareDevice;

namespace IronArc
{
    public abstract class HardwareDevice : IDisposable
    {
        public uint DeviceId { get; protected set; }
        public Guid MachineId { get; internal set; }
        public abstract string DeviceName { get; }
        public abstract HardwareDeviceStatus Status { get; protected set; }
        internal abstract DefinitionDevice Definition { get; }
        internal HardwareMemoryMapping MemoryMapping { get; }
        public abstract void HardwareCall(string functionName, VirtualMachine vm);
        public abstract void Dispose();
    }
}
