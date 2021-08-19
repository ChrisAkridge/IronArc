using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.HardwareDefinitionGenerator;
using IronArc.HardwareDefinitionGenerator.Models;
using IronArc.Memory;

namespace IronArc.Hardware
{
    public sealed class SystemDevice : HardwareDevice
    {
        public override string DeviceName => "System";
        public override HardwareDeviceStatus Status => HardwareDeviceStatus.Active;

        internal override HardwareDefinitionGenerator.Models.HardwareDevice Definition =>
            new HardwareDefinitionGenerator.Models.HardwareDevice("System",
                new List<HardwareCall>
                {
                    Generator.ParseHardwareCall("uint8 hwcall System::RegisterInterruptHandler(uint32 deviceId, lpstring* interruptName, ptr handlerAddress)"),
                    Generator.ParseHardwareCall("void hwcall System::UnregisterInterruptHandler(uint32 deviceId, lpstring* interruptName, uint8 handlerIndex)"),
                    Generator.ParseHardwareCall("void hwcall System::RaiseError(uint32 errorCode)"),
                    Generator.ParseHardwareCall("void hwcall System::RegisterErrorHandler(uint32 errorCode, ptr handlerAddress)"),
                    Generator.ParseHardwareCall("void hwcall System::UnregisterErrorHandler(uint32 errorCode)"),
                    Generator.ParseHardwareCall("uint64 hwcall System::GetLastErrorDescriptionSize()"),
                    Generator.ParseHardwareCall("void hwcall System::GetLastErrorDescription(ptr destination)"),
                    Generator.ParseHardwareCall("int32 hwcall System::GetHardwareDeviceCount()"),
                    Generator.ParseHardwareCall("uint64 hwcall System::GetHardwareDeviceDescriptionSize(uint32 deviceId)"),
                    Generator.ParseHardwareCall("void hwcall System::GetHardwareDeviceDescription(uint32 deviceId, ptr destination)"),
                    Generator.ParseHardwareCall("uint64 hwcall System::GetAllHardwareDeviceDescriptionsSize()"),
                    Generator.ParseHardwareCall("void hwcall System::GetAllHardwareDeviceDescriptions(ptr destination)"),
                });

        public SystemDevice(Guid machineId, uint deviceId)
        {
            MachineId = machineId;
            DeviceId = deviceId;
        }

        public override void HardwareCall(string functionName, VirtualMachine vm)
        {
            string lowerCased = functionName.ToLowerInvariant();

            if (lowerCased == "registerinterrupthandler")
            {
                uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                ulong interruptNameAddress = vm.Processor.PopExternal(OperandSize.QWord);
                ulong handlerAddress = vm.Processor.PopExternal(OperandSize.QWord);

                string interruptName = vm.Processor.ReadStringFromMemory(interruptNameAddress);
                RegisterInterruptHandler(vm, deviceId, interruptName, handlerAddress);
            }
            else if (lowerCased == "unregisterinterrupthandler")
            {
                uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                ulong interruptNamePointer = vm.Processor.PopExternal(OperandSize.QWord);
                byte handlerIndex = (byte)vm.Processor.PopExternal(OperandSize.Byte);

                string interruptName = vm.Processor.ReadStringFromMemory(interruptNamePointer);
                UnregisterInterruptHandler(vm, deviceId, interruptName, handlerIndex);
            }
            else if (lowerCased == "raiseerror")
            {
                uint errorCode = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                RaiseError(vm, errorCode);
            }
            else if (lowerCased == "registererrorhandler")
            {
                uint errorCode = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                ulong handlerAddress = vm.Processor.PopExternal(OperandSize.QWord);

                RegisterErrorHandler(vm, errorCode, handlerAddress);
            }
            else if (lowerCased == "unregistererrorhandler")
            {
                uint errorCode = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                
                UnregisterErrorHandler(vm, errorCode);
            }
            else if (lowerCased == "getlasterrordescriptionsize")
            {
                ulong size = GetLastErrorDescriptionSize(vm);
                vm.Processor.PushExternal(size, OperandSize.QWord);
            }
            else if (lowerCased == "getlasterrordescription")
            {
                ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                GetLastErrorDescription(vm, destination);
            }
            else if (lowerCased == "gethardwaredevicedescriptionsize")
            {
                uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                GetHardwareDeviceDescriptionSize(vm, deviceId);
            }
            else if (lowerCased == "gethardwaredevicedescription")
            {
                uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                GetHardwareDeviceDescription(vm, deviceId, destination);
            }
            else if (lowerCased == "getallhardwaredevicedescriptionssize")
            {
                GetAllHardwareDeviceDescriptionsSize(vm);
            }
            else if (lowerCased == "getallhardwaredevicedescriptions")
            {
                ulong destination = vm.Processor.PopExternal(OperandSize.QWord);
                GetAllHardwareDeviceDescriptions(vm, destination);
            }
        }

        private void RegisterInterruptHandler(VirtualMachine vm, uint deviceId, string interruptName, ulong handlerAddress)
        {
            if (vm.Hardware.All(h => h.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(Error.HardwareError, $"Cannot register interrupt handler for {deviceId} as no device has that ID.");
                return;
            }
            
            vm.Processor.RegisterInterruptHandler(deviceId, interruptName, handlerAddress);
        }

        private void UnregisterInterruptHandler(VirtualMachine vm, uint deviceId, string interruptName, byte handlerIndex)
        {
            if (vm.Hardware.All(h => h.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot unregister interrupt handler for {deviceId} as no device has that ID.");

                return;
            }
            
            vm.Processor.UnregisterInterruptHandler(deviceId, interruptName, handlerIndex);
        }

        private void RaiseError(VirtualMachine vm, uint errorCode)
        {
            vm.Processor.RaiseError(errorCode, null);
        }

        private void RegisterErrorHandler(VirtualMachine vm, uint errorCode, ulong handlerAddress)
        {
            vm.Processor.RegisterErrorHandler(errorCode, handlerAddress);
        }

        private void UnregisterErrorHandler(VirtualMachine vm, uint errorCode)
        {
            vm.Processor.UnregisterErrorHandler(errorCode);
        }

        private ulong GetLastErrorDescriptionSize(VirtualMachine vm) => vm.LastError.GetErrorDescriptionSize();

        private void GetLastErrorDescription(VirtualMachine vm, ulong destination)
        {
            var errorDescription = vm.LastError.GetErrorDescription(destination);
            vm.MemoryManager.Write(errorDescription, destination);
        }

        private void GetHardwareDeviceDescriptionSize(VirtualMachine vm, uint deviceId)
        {
            if (vm.Hardware.All(d => d.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(IronArc.Error.HardwareError, $"No device with ID {deviceId} exists.");

                return;
            }

            var description = vm.GetHardwareDescription(deviceId, 0UL);
            vm.Processor.PushExternal((ulong)description.Count(), OperandSize.QWord);
        }

        private void GetHardwareDeviceDescription(VirtualMachine vm, uint deviceId, ulong destination)
        {
            if (vm.Hardware.All(d => d.DeviceId != deviceId))
            {
                vm.Processor.RaiseError(IronArc.Error.HardwareError, $"No device with ID {deviceId} exists.");

                return;
            }

            var description = vm.GetHardwareDescription(deviceId, destination).ToArray();
            vm.MemoryManager.Write(description, destination);
        }

        private void GetAllHardwareDeviceDescriptionsSize(VirtualMachine vm)
        {
            var descriptions = vm.GetAllHardwareDescriptions(0UL);
            vm.Processor.PushExternal((ulong)descriptions.Length, OperandSize.QWord);
        }

        private void GetAllHardwareDeviceDescriptions(VirtualMachine vm, ulong destination)
        {
            var descriptions = vm.GetAllHardwareDescriptions(destination);
            vm.MemoryManager.Write(descriptions, destination);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose() { }
    }
}
