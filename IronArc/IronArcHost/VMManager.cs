using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IronArc;
using IronArc.Hardware.Configuration;

namespace IronArcHost
{
    internal static class VMManager
    {
        public static List<VirtualMachine> VirtualMachines = new List<VirtualMachine>();
        public static Dictionary<Guid, Thread> VMThreads = new Dictionary<Guid, Thread>();
        public static CoreHardwareProvider Provider = new CoreHardwareProvider();
        public static ConcurrentQueue<Message> UIMessageQueue = new ConcurrentQueue<Message>();
        public static event EventHandler<Message> VMStateChangeEvent;

        public static void Initialize()
        {
            HardwareSearcher.FindHardwareInIronArc();
            HardwareProvider.Provider = Provider;
        }

        public static Guid CreateVM(string programPath, ulong memorySize, ulong loadAddress,
            IEnumerable<NewVMHardwareSelection> hardwareSelections)
        {
            var vm = new VirtualMachine(memorySize, programPath, loadAddress,
                hardwareSelections.Select(s => new HardwareSelection(s.DeviceTypeName, s.Configuration)));
            VirtualMachines.Add(vm);

            return vm.MachineId;
        }

        public static VirtualMachine Lookup(Guid machineID)
        {
            var result = VirtualMachines.FirstOrDefault(vm => vm.MachineId == machineID);

            return result ?? throw new ArgumentException($"No VM by the ID of {{{machineID}}} exists.");
        }

        public static void ResumeVM(Guid machineID)
        {
            if (VMThreads.ContainsKey(machineID))
            {
                throw new ArgumentException($"The VM by the ID of {{{machineID}}} is already running.");
            }

            var vm = Lookup(machineID);
            var vmWorker = new Thread(vm.MainLoop)
            {
                Name = vm.MachineId.ToString()
            };
            VMThreads.Add(machineID, vmWorker);
            vm.Resume();

            vmWorker.Start();
        }

        internal static void PauseVM(Guid machineID)
        {
            var vm = Lookup(machineID);

            var message = new Message(VMMessage.SetVMState, UIMessage.None, machineID, (int)VMState.Paused,
                0L, null);
            vm.MessageQueue.Enqueue(message);
        }

        internal static void AddHardwareToVM(Guid machineID, string hwDeviceTypeName,
            HardwareConfiguration configuration)
        {
            // Always ensure we actually make hardware on the main thread so we don't accidentally
            // make forms on non-UI threads
            var vm = Lookup(machineID);
            var type = HardwareSearcher.LookupDeviceByName(hwDeviceTypeName);
            var deviceID = vm.GetNextHardwareDeviceID();
            var device = (HardwareDevice)Activator.CreateInstance(type, machineID, deviceID);
            var message = new Message(VMMessage.AddHardwareDevice, UIMessage.None, machineID, 0, 0L, device);

            device.Configure(configuration);

            vm.MessageQueue.Enqueue(message);
        }

        internal static void RemoveHardwareFromVM(Guid machineID, string hwDeviceTypeName)
        {
            var type = HardwareSearcher.LookupDeviceByName(hwDeviceTypeName);
            var message = new Message(VMMessage.RemoveHardwareDevice, UIMessage.None, machineID, 0, 0L, type);

            Lookup(machineID).MessageQueue.Enqueue(message);
        }

        internal static ulong ReadInstructionExecutedCount(Guid machineID) =>
            Lookup(machineID).InstructionExecutedCount;

        public static void CheckMessageQueue()
        {
            // We can use TryDequeue by itself here since performance isn't a concern.
            // Also no one but VMManager on the main thread can actually dequeue anything.
            while (UIMessageQueue.TryDequeue(out var message))
            {
                if (message.UIMessage != UIMessage.VMStateChanged) { continue; }

                switch ((VMState)message.WParam)
                {
                    case VMState.Running:
                        break;
                    case VMState.Paused:
                        // The MainLoop thread should already be done by this point
                        // If it's not, it will be soon
                        VMThreads.Remove(message.MachineID);

                        break;
                    case VMState.Error:
                    {
                        var error = (Error)message.LParam;
                        System.Windows.Forms.MessageBox.Show(
                            $"An unhandled error occured on the VM.\r\n\r\nMachine ID: {message.MachineID}\r\nError: {error}",
                            "IronArc", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error,
                            System.Windows.Forms.MessageBoxDefaultButton.Button1);

                        break;
                    }
                    case VMState.Halted:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                OnVMStateChanged(message);
            }
        }

        private static void OnVMStateChanged(Message message) => VMStateChangeEvent?.Invoke(null, message);
    }
}
