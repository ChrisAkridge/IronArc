using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.Memory;

namespace IronArc.Hardware
{
    public sealed class SystemDevice : HardwareDevice
    {
        public override string DeviceName => "System";
        public override HardwareDeviceStatus Status => HardwareDeviceStatus.Active;
        internal override HardwareDefinitionGenerator.Models.HardwareDevice Definition { get; }

        public SystemDevice(uint deviceId, VirtualMachine vm)
        {
            DeviceId = deviceId;
        }

        public override void HardwareCall(string functionName, VirtualMachine vm)
        {
            // TODO: pass vm as a parameter to the hardware call functions
            string lowerCased = functionName.ToLowerInvariant();

            if (lowerCased == "registerinterrupthandler")
            {
                ulong handlerAddress = vm.Processor.PopExternal(OperandSize.QWord);
                ulong interruptNamePointer = vm.Processor.PopExternal(OperandSize.QWord);
                uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);

                string interruptName = vm.Processor.ReadStringFromMemory(interruptNamePointer);
                RegisterInterruptHandler(vm, deviceId, interruptName, handlerAddress);
            }
            else if (lowerCased == "unregisterinterrupthandler")
            {
                byte handlerIndex = (byte)vm.Processor.PopExternal(OperandSize.Byte);
                ulong interruptNamePointer = vm.Processor.PopExternal(OperandSize.QWord);
                uint deviceId = (uint)vm.Processor.PopExternal(OperandSize.DWord);

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
                ulong handlerAddress = vm.Processor.PopExternal(OperandSize.QWord);
                uint errorCode = (uint)vm.Processor.PopExternal(OperandSize.DWord);
                
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

        private void CreatePageTable(VirtualMachine vm)
        {
            vm.MemoryManager.CreatePageTable();
            vm.Processor.PushExternal(vm.MemoryManager.CurrentPageTableId, OperandSize.DWord);
        }

        private void DestroyPageTable(VirtualMachine vm, uint pageTableId)
        {
            if (vm.MemoryManager.CurrentPageTableId == pageTableId)
            {
                vm.Processor.RaiseError(Error.HardwareError, $"Cannot delete the current page table (ID {pageTableId}).");

                return;
            }

            if (pageTableId == 0)
            {
                vm.Processor.RaiseError(Error.HardwareError, $"Cannot delete page table with ID 0.");

                return;
            }
            
            vm.MemoryManager.DestroyPageTable(pageTableId);
        }

        private void ChangeCurrentPageTable(VirtualMachine vm, uint pageTableId)
        {
            if (!vm.MemoryManager.TryChangePageTable(pageTableId))
            {
                vm.Processor.RaiseError(Error.HardwareError, $"Cannot change to page table with ID {pageTableId} because no page table has that ID.");
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose() { }
    }
}
