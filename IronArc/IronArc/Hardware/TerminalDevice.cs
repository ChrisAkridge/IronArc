using System;
using System.Collections.Generic;
using IronArc.HardwareDefinitionGenerator;
using IronArc.HardwareDefinitionGenerator.Models;
using DefinitionDevice = IronArc.HardwareDefinitionGenerator.Models.HardwareDevice;

namespace IronArc.Hardware
{
    public sealed class TerminalDevice : HardwareDevice
    {
        // The first hardware device, created on December 1, 2017.
        private readonly ITerminal terminal;

        public override string DeviceName => "Terminal";

        public override HardwareDeviceStatus Status => HardwareDeviceStatus.Active;

        internal override DefinitionDevice Definition =>
            new DefinitionDevice(nameof(TerminalDevice),
                new List<HardwareCall>
                {
                    new HardwareCall(null, "Write",
                        new List<HardwareCallParameter>
                        {
                            new HardwareCallParameter("text", DefaultDataTypes.LpStringPointer)
                        }),
                    new HardwareCall(null, "WriteLine",
                        new List<HardwareCallParameter>
                        {
                            new HardwareCallParameter("text", DefaultDataTypes.LpStringPointer)
                        }),
                    new HardwareCall(DefaultDataTypes.UInt16, "Read", new List<HardwareCallParameter> { }),
                    new HardwareCall(null, "ReadLine", new List<HardwareCallParameter>
                    {
                        new HardwareCallParameter("destination", DefaultDataTypes.Pointer)
                    })
                });

        public TerminalDevice(Guid machineId, uint deviceId)
        {
            terminal = HardwareProvider.Provider.CreateTerminal();
            terminal.MachineId = machineId;

            MachineId = machineId;
            DeviceId = deviceId;
        }

        public override void HardwareCall(string functionName, VirtualMachine vm)
        {
            if (functionName.StartsWith("Write", StringComparison.Ordinal))
            {
                ulong textAddress = vm.Processor.PopExternal(OperandSize.QWord);
                string text = vm.Processor.ReadStringFromMemory(textAddress);

                switch (functionName)
                {
                    case "Write":
                        Write(text);
                        break;
                    case "WriteLine":
                        WriteLine(text);
                        break;
                }
            }
            else
            {
                switch (functionName)
                {
                    case "Read":
                    {
                        var character = Read();

                        vm.Processor.PushExternal(new[]
                        {
                            (byte)(character & 0xFF), (byte)(character >> 8)
                        });

                        break;
                    }
                    case "ReadLine":
                    {
                        var line = ReadLine();
                        ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                        vm.Processor.WriteStringToMemory(destination, line);

                        break;
                    }
                }
            }
        }

        private void Write(string text) => terminal.Write(text);
        private void WriteLine(string text) => terminal.WriteLine(text);

        private char Read() =>
            terminal.CanPerformWaitingRead ? terminal.Read() : terminal.NonWaitingRead();

        private string ReadLine() =>
            terminal.CanPerformWaitingRead ? terminal.ReadLine() : terminal.NonWaitingReadLine();

        public override void Dispose() => HardwareProvider.Provider.DestroyTerminal(terminal);
    }
}
