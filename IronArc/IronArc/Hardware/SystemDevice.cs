using System;
using System.Collections.Generic;
using System.Linq;
using IronArc.HardwareDefinitionGenerator;
using IronArc.HardwareDefinitionGenerator.Models;

namespace IronArc.Hardware
{
    public sealed class SystemDevice : HardwareDevice
    {
        public override string DeviceName => "System";

        public override HardwareDeviceStatus Status
        {
            get => HardwareDeviceStatus.Active;
            protected set => throw new InvalidOperationException();
        }

        internal override HardwareDefinitionGenerator.Models.HardwareDevice Definition =>
            new HardwareDefinitionGenerator.Models.HardwareDevice("System",
                new List<HardwareMethod>
                {
                    Generator.ParseHardwareMethod("uint8 hwcall System::RegisterInterruptHandler(uint32 deviceId, byte* interruptName, ptr handlerAddress)"),
                    Generator.ParseHardwareMethod("void hwcall System::UnregisterInterruptHandler(uint32 deviceId, byte* interruptName, uint8 handlerIndex)"),
                    Generator.ParseHardwareMethod("void hwcall System::RaiseError(uint32 errorCode)"),
                    Generator.ParseHardwareMethod("void hwcall System::RegisterErrorHandler(uint32 errorCode, ptr handlerAddress)"),
                    Generator.ParseHardwareMethod("void hwcall System::UnregisterErrorHandler(uint32 errorCode)"),
                    Generator.ParseHardwareMethod("uint64 hwcall System::GetLastErrorDescriptionSize()"),
                    Generator.ParseHardwareMethod("void hwcall System::GetLastErrorDescription(ptr destination)"),
                    Generator.ParseHardwareMethod("int32 hwcall System::GetHardwareDeviceCount()"),
                    Generator.ParseHardwareMethod("uint64 hwcall System::GetHardwareDeviceDescriptionSize(uint32 deviceId)"),
                    Generator.ParseHardwareMethod("void hwcall System::GetHardwareDeviceDescription(uint32 deviceId, ptr destination)"),
                    Generator.ParseHardwareMethod("uint64 hwcall System::GetAllHardwareDeviceDescriptionsSize()"),
                    Generator.ParseHardwareMethod("void hwcall System::GetAllHardwareDeviceDescriptions(ptr destination)"),
                    Generator.ParseHardwareMethod("void hwcall System::ReadHardwareMemory(uint32 deviceId, ptr source, ptr destination, uint32 count)"),
                    Generator.ParseHardwareMethod("void hwcall System::WriteHardwareMemory(uint32 deviceId, ptr source, ptr destination, uint32 count)"),
                    Generator.ParseHardwareMethod("void interrupt System::HardwareDeviceAttached(uint32 deviceId)"),
                    Generator.ParseHardwareMethod("void interrupt System::HardwareDeviceRemoved(uint32 deviceId)")
                });

        public SystemDevice(Guid machineId, uint deviceId)
        {
            MachineId = machineId;
            DeviceId = deviceId;
        }

        public override void HardwareCall(string functionName, VirtualMachine vm)
        {
            string lowerCased = functionName.ToLowerInvariant();

            switch (lowerCased)
            {
                case "registerinterrupthandler":
                {
                    uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    ulong interruptNameAddress = vm.Processor.PopExternal(OperandSize.QWord);
                    ulong handlerAddress = vm.Processor.PopExternal(OperandSize.QWord);

                    string interruptName = vm.Processor.ReadStringFromMemory(interruptNameAddress);
                    RegisterInterruptHandler(vm, deviceId, interruptName, handlerAddress);

                    break;
                }
                case "unregisterinterrupthandler":
                {
                    uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    ulong interruptNamePointer = vm.Processor.PopExternal(OperandSize.QWord);
                    byte handlerIndex = (byte)vm.Processor.PopExternal(OperandSize.Byte);

                    string interruptName = vm.Processor.ReadStringFromMemory(interruptNamePointer);
                    UnregisterInterruptHandler(vm, deviceId, interruptName, handlerIndex);

                    break;
                }
                case "raiseerror":
                {
                    uint errorCode = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    RaiseError(vm, errorCode);

                    break;
                }
                case "registererrorhandler":
                {
                    uint errorCode = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    ulong handlerAddress = vm.Processor.PopExternal(OperandSize.QWord);

                    RegisterErrorHandler(vm, errorCode, handlerAddress);

                    break;
                }
                case "unregistererrorhandler":
                {
                    uint errorCode = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                
                    UnregisterErrorHandler(vm, errorCode);

                    break;
                }
                case "getlasterrordescriptionsize":
                {
                    ulong size = GetLastErrorDescriptionSize(vm);
                    vm.Processor.PushExternal(size, OperandSize.QWord);

                    break;
                }
                case "getlasterrordescription":
                {
                    ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                    GetLastErrorDescription(vm, destination);

                    break;
                }
                case "gethardwaredevicedescriptionsize":
                {
                    uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    GetHardwareDeviceDescriptionSize(vm, deviceId);

                    break;
                }
                case "gethardwaredevicedescription":
                {
                    uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                    GetHardwareDeviceDescription(vm, deviceId, destination);

                    break;
                }
                case "getallhardwaredevicedescriptionssize":
                    GetAllHardwareDeviceDescriptionsSize(vm);

                    break;
                case "getallhardwaredevicedescriptions":
                {
                    ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                    GetAllHardwareDeviceDescriptions(vm, destination);

                    break;
                }
                case "readhardwarememory":
                {
                    uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    ulong source = vm.Processor.PopExternal(OperandSize.QWord);
                    ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                    uint length = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    
                    ReadHardwareMemory(vm, deviceId, source, destination, length);
                    break;
                }
                case "writehardwarememory":
                {
                    uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                    ulong source = vm.Processor.PopExternal(OperandSize.QWord);
                    ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                    uint length = (uint)vm.Processor.PopExternal(OperandSize.DWord);

                    WriteHardwareMemory(vm, deviceId, source, destination, length);
                    break;
                }
            }
        }

        private static void RegisterInterruptHandler(VirtualMachine vm, uint deviceId, string interruptName, ulong handlerAddress)
        {
            if (vm.Hardware.All(h => h.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(Error.HardwareError, $"Cannot register interrupt handler for {deviceId} as no device has that ID.");
                return;
            }
            
            vm.Processor.RegisterInterruptHandler(deviceId, interruptName, handlerAddress);
        }

        private static void UnregisterInterruptHandler(VirtualMachine vm, uint deviceId, string interruptName, byte handlerIndex)
        {
            if (vm.Hardware.All(h => h.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot unregister interrupt handler for {deviceId} as no device has that ID.");

                return;
            }
            
            vm.Processor.UnregisterInterruptHandler(deviceId, interruptName, handlerIndex);
        }

        private static void RaiseError(VirtualMachine vm, uint errorCode)
        {
            vm.Processor.RaiseError(errorCode, null);
        }

        private static void RegisterErrorHandler(VirtualMachine vm, uint errorCode, ulong handlerAddress)
        {
            vm.Processor.RegisterErrorHandler(errorCode, handlerAddress);
        }

        private static void UnregisterErrorHandler(VirtualMachine vm, uint errorCode)
        {
            vm.Processor.UnregisterErrorHandler(errorCode);
        }

        private static ulong GetLastErrorDescriptionSize(VirtualMachine vm) => vm.LastError.GetErrorDescriptionSize();

        private static void GetLastErrorDescription(VirtualMachine vm, ulong destination)
        {
            var errorDescription = vm.LastError.GetErrorDescription(destination);
            vm.MemoryManager.Write(errorDescription, destination);
        }

        private static void GetHardwareDeviceDescriptionSize(VirtualMachine vm, uint deviceId)
        {
            if (vm.Hardware.All(d => d.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(Error.HardwareError, $"No device with ID {deviceId} exists.");

                return;
            }

            var description = vm.GetHardwareDescription(deviceId, 0UL);
            vm.Processor.PushExternal((ulong)description.Count(), OperandSize.QWord);
        }

        private static void GetHardwareDeviceDescription(VirtualMachine vm, uint deviceId, ulong destination)
        {
            if (vm.Hardware.All(d => d.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(Error.HardwareError, $"No device with ID {deviceId} exists.");

                return;
            }

            var description = vm.GetHardwareDescription(deviceId, destination).ToArray();
            vm.MemoryManager.Write(description, destination);
        }

        private static void GetAllHardwareDeviceDescriptionsSize(VirtualMachine vm)
        {
            var descriptions = vm.GetAllHardwareDescriptions(0UL);
            vm.Processor.PushExternal((ulong)descriptions.Length, OperandSize.QWord);
        }

        private static void GetAllHardwareDeviceDescriptions(VirtualMachine vm, ulong destination)
        {
            var descriptions = vm.GetAllHardwareDescriptions(destination);
            vm.MemoryManager.Write(descriptions, destination);
        }

        private static void ReadHardwareMemory(VirtualMachine vm, uint deviceId, ulong source, ulong destination,
            uint count)
        {
            var device = vm.Hardware.FirstOrDefault(hw => hw.DeviceId == deviceId);

            ValidateMemoryAccessRanges(vm, device, source, destination, count);

            var hwMemory = device.MemoryMapping.Memory;
            vm.MemoryManager.TransferFrom(hwMemory, source, destination, count);
        }

        private static void WriteHardwareMemory(VirtualMachine vm, uint deviceId, ulong source, ulong destination,
            uint count)
        {
            var device = vm.Hardware.FirstOrDefault(hw => hw.DeviceId == deviceId);
            
            ValidateMemoryAccessRanges(vm, device, source, destination, count);

            var hwMemory = device.MemoryMapping.Memory;
            vm.MemoryManager.TransferTo(hwMemory, source, destination, count);
        }

        private static void ValidateMemoryAccessRanges(VirtualMachine vm,
            HardwareDevice device, ulong source, ulong destination,
            uint count)
        {
            var deviceId = device.DeviceId;
            
            if (device == null) { throw new ArgumentException($"No hardware device has ID #{deviceId}.", nameof(deviceId)); }

            if (device.MemoryMapping == null)
            {
                throw new ArgumentException($"Hardware device #{deviceId} ({device.GetType().Name}) has no mapped memory.");
            }

            if (source + count > device.MemoryMapping.MemoryLength)
            {
                throw new ArgumentOutOfRangeException(
                    $"Hardware memory read out of range. Device #{deviceId} ({device.GetType().Name}) from 0x{source:X16} to 0x{(source + count):X16}.");
            }

            if ((long)(destination + count) > vm.MemoryManager.CurrentContextLength)
            {
                throw new ArgumentOutOfRangeException(
                    $"Hardware memory read out of range. Machine {{{vm.MachineId}}}, context {vm.MemoryManager.CurrentContextIndex} from 0x{destination:X16} to 0x{(destination + count):X16}.");
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose() { }
    }
}
