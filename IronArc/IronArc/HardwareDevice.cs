using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronArc.Memory;
using DefinitionDevice = IronArc.HardwareDefinitionGenerator.Models.HardwareDevice;

namespace IronArc
{
    public abstract class HardwareDevice : IDisposable
    {
        public uint DeviceId { get; protected set; }
        public abstract string DeviceName { get; }
        public abstract HardwareDeviceStatus Status { get; } // add a SetStatus(HardwareDeviceStatus) method to derived classes to set the status. Also use a field, not an autoproperty.
        internal abstract DefinitionDevice Definition { get; }
        internal HardwareMemoryMapping MemoryMapping { get; }
        public abstract void HardwareCall(string functionName, VirtualMachine vm);
        internal abstract void AddHardwareMemoryIfNeeded(HardwareMemory hardwareMemory);
        public abstract void Dispose();
    }
}
