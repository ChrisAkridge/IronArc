using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefinitionDevice = IronArc.HardwareDefinitionGenerator.Models.HardwareDevice;

namespace IronArc
{
    public abstract class HardwareDevice : IDisposable
    {
        public Guid DeviceId { get; private set; }
        public abstract string DeviceName { get; }
        public abstract HardwareDeviceStatus Status { get; } // add a SetStatus(HardwareDeviceStatus) method to derived classes to set the status. Also use a field, not an autoproperty.
        internal abstract DefinitionDevice Definition { get; }
        public abstract void HardwareCall(string functionName, VirtualMachine vm);

        public abstract void Dispose();
    }
}
