using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IronArc.Hardware;

namespace IronArc
{
	public sealed class VirtualMachine
	{
		public Guid MachineID { get; }
		public Processor Processor { get; }
		public ByteBlock Memory { get; }
		public ConcurrentQueue<Message> MessageQueue { get; }
		public List<HardwareDevice> Hardware { get; }
		public VMState State { get; private set; }
		public ulong InstructionExecutedCount { get; private set; }

		public VirtualMachine(ulong memorySize, string programPath, ulong loadAddress, 
			IEnumerable<string> hardwareDeviceNames)
		{
			byte[] program = File.ReadAllBytes(programPath);

			MachineID = Guid.NewGuid();
			MessageQueue = new ConcurrentQueue<Message>();

			Memory = ByteBlock.FromLength(memorySize);
			Memory.WriteAt(program, loadAddress);

			Processor = new Processor(Memory, loadAddress, (ulong)program.Length, this);
			Hardware = new List<HardwareDevice>();

			foreach (var hardwareName in hardwareDeviceNames)
			{
				var type = Type.GetType(hardwareName);
				Hardware.Add((HardwareDevice)Activator.CreateInstance(type, MachineID));
			}

			State = VMState.Paused;
		}

		public void AddHardwareDevice(Type hardwareDeviceType)
		{
			if (Hardware.Select(h => h.GetType()).Contains(hardwareDeviceType))
			{
				throw new ArgumentException($"A hardware device named {hardwareDeviceType.FullName} is already running on this VM.");
			}

			Hardware.Add((HardwareDevice)Activator.CreateInstance(hardwareDeviceType, MachineID));
		}

		public void AddHardwareDevice(HardwareDevice device)
		{
			if (Hardware.Any(h => h.GetType() == device.GetType()))
			{
				throw new ArgumentException($"A hardware device named {device.GetType().FullName} is already running on this VM.");
			}

			Hardware.Add(device);
		}

		public bool RemoveHardwareDevice(Type hardwareDeviceType)
		{
			var device = Hardware.FirstOrDefault(h => h.GetType() == hardwareDeviceType);
			if (device == null) { return false; }
			
			device.Dispose();
			Hardware.Remove(device);
			return true;
		}

		public void Resume()
		{
			State = VMState.Running;

			var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineID,
				(int)State, 0L, null);
			HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
		}

		public void Pause()
		{
			State = VMState.Paused;

			var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineID,
				(int)State, 0L, null);
			HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
		}

		public void Halt()
		{
			State = VMState.Halted;

			var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineID,
				(int)State, 0L, null);
			HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
		}

		public void Error(uint errorCode)
		{
			State = VMState.Error;

			var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineID, (int)State,
				errorCode, null);
			HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
		}

		public void MainLoop()
		{
			while (State == VMState.Running)
			{
				Processor.ExecuteNextInstruction();
				InstructionExecutedCount++;

				// Here we check if the message queue has any messages in it. Ordinarily, calling
				// IsEmpty isn't very good since another thread can add a message after the call to
				// IsEmpty but before the dequeue happens. This would typically be a problem, but
				// depending on how often we check for messages, it may not be that bad - we may not
				// respond to messages on this loop, but we will soon.
				//
				// TryDequeue is usually used as the solution to this two-stage check, but it isn't
				// as performant - it calls IsEmpty internally, which makes it less performant than
				// just using IsEmpty. So we'll use IsEmpty until we don't have do.
				// 
				// To maintainers: preserve this comment.
				if (!MessageQueue.IsEmpty)
				{
					// For now, we'll just respond to SetVMState.
					Message message = null;
					if (!MessageQueue.TryDequeue(out message))
					{
						throw new InvalidOperationException("Tried to dequeue a message from an empty queue.");
						// yay race conditions
						// although the only things doing dequeues SHOULD be VMs, hopefully
					}

					if (message.VMMessage == VMMessage.SetVMState)
					{
						// WParam stores the new state to switch to
						if ((VMState)message.WParam == VMState.Paused)
						{
							Pause();
							break;
						}
					}
					else if (message.VMMessage == VMMessage.AddHardwareDevice)
					{
						AddHardwareDevice((HardwareDevice)message.Data);
					}
					else if (message.VMMessage == VMMessage.RemoveHardwareDevice)
					{
						RemoveHardwareDevice((Type)message.Data);
					}
				}
			}
		}

		internal void HardwareCall(string hwcall)
		{
			string[] hwcallParts = hwcall.Split(new[] { "::" }, StringSplitOptions.None);
			
			for (int i = 0; i < Hardware.Count; i++)
			{
				if (Hardware[i].DeviceName == hwcallParts[0])
				{
					Hardware[i].HardwareCall(hwcallParts[1], this);
					return;
				}
			}

			// If we reach here, this hardware device doesn't exist, or the device name is wrong.
			Processor.RaiseError(IronArc.Error.HardwareDeviceNotPresent);
		}
	}
}
