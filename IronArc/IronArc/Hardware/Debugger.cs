using System;
using System.Collections.Generic;
using IronArc.HardwareDefinitionGenerator.Models;
using DefinitionDevice = IronArc.HardwareDefinitionGenerator.Models.HardwareDevice;
// ReSharper disable InconsistentNaming

namespace IronArc.Hardware
{
    public sealed class Debugger : HardwareDevice
    {
        public override string DeviceName => "Debugger";

        public override HardwareDeviceStatus Status =>
            (System.Diagnostics.Debugger.IsAttached)
                ? HardwareDeviceStatus.Active
                : HardwareDeviceStatus.Inactive;

        internal override DefinitionDevice Definition =>
            new DefinitionDevice(nameof(Debugger),
                new List<HardwareCall>
                {
                    new HardwareCall(null, "Break", new List<HardwareCallParameter>())
                });

        public Debugger(Guid machineId, uint deviceId)
        {
            MachineId = machineId;
            DeviceId = deviceId;
        }

        public override void Dispose() { }

        public override void HardwareCall(string functionName, VirtualMachine vm)
        {
            if (functionName.ToLowerInvariant() == "break")
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}
