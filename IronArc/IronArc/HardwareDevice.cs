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
        public abstract HardwareDeviceStatus Status { get; } // add a SetStatus(HardwareDeviceStatus) method to derived classes to set the status. Also use a field, not an autoproperty.
        internal abstract DefinitionDevice Definition { get; }
        internal HardwareMemoryMapping MemoryMapping { get; }
        public abstract void HardwareCall(string functionName, VirtualMachine vm);
        public abstract void Dispose();
        
        // TODO: interrupts
        // and we should probably solve the multithreaded version first for things like network devices
        // I imagine ConcurrentQueue will be more than enough for that, as I don't need an interrupt to
        // be fired off as soon as it could be
        // maybe just add it on to the VM queues we already have
    }
}
