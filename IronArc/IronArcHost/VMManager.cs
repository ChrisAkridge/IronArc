using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IronArc;

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
			IEnumerable<string> hardwareDeviceNames)
		{
			var vm = new VirtualMachine(memorySize, programPath, loadAddress,
				hardwareDeviceNames);
			VirtualMachines.Add(vm);

			return vm.MachineID;
		}

		public static VirtualMachine Lookup(Guid machineID)
		{
			var result = VirtualMachines.FirstOrDefault(vm => vm.MachineID == machineID);

			if (result == null)
			{
				throw new ArgumentException($"No VM by the ID of {{{machineID}}} exists.");
			}

			return result;
		}

		public static void ResumeVM(Guid machineID)
		{
			if (VMThreads.ContainsKey(machineID))
			{
				throw new ArgumentException($"The VM by the ID of {{{machineID}}} is already running.");
			}

			var vm = Lookup(machineID);
			Thread vmWorker = new Thread(vm.MainLoop);
			vmWorker.Name = vm.MachineID.ToString();
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

		public static void CheckMessageQueue()
		{
			// We can use TryDequeue by itself here since performance isn't a concern.
			// Also no one but VMManager on the main thread can actually dequeue anything.
			Message message = null;
			while (UIMessageQueue.TryDequeue(out message))
			{
				if (message.UIMessage == UIMessage.VMStateChanged)
				{
					if ((VMState)message.WParam == VMState.Running)
					{
						
					}
					else if ((VMState)message.WParam == VMState.Paused)
					{
						// The MainLoop thread should already be done by this point
						// If it's not, it will be soon
						VMThreads.Remove(message.MachineID);
					}

					OnVMStateChanged(message);
				}
			}
		}

		private static void OnVMStateChanged(Message message)
		{
			VMStateChangeEvent?.Invoke(null, message);
		}
	}
}
