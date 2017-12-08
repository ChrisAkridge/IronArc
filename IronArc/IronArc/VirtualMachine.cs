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

		public VirtualMachine(ulong memorySize, string programPath, ulong loadAddress, 
			IEnumerable<string> hardwareDeviceNames)
		{
			byte[] program = File.ReadAllBytes(programPath);

			MachineID = Guid.NewGuid();
			MessageQueue = new ConcurrentQueue<Message>();

			Memory = ByteBlock.FromLength(memorySize);
			Memory.WriteAt(program, loadAddress);

			Processor = new Processor(Memory, loadAddress);
			Hardware = new List<HardwareDevice>();

			foreach (var hardwareName in hardwareDeviceNames)
			{
				var type = Type.GetType(hardwareName);
				Hardware.Add((HardwareDevice)Activator.CreateInstance(type, MachineID));
			}

			State = VMState.Paused;
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

		public void MainLoop()
		{
			// Placeholder for now so we can get threading to work
			int i = 0;
			while (true)
			{
				if (i % 10000000 == 0)
				{
					var terminal = (TerminalDevice)Hardware[0];
					var iTerminal = (ITerminal)(typeof(TerminalDevice).GetField("terminal", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(terminal));
					iTerminal.WriteLine($"Looped {i} times");
				}

				i++;

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
				}
			}
		}
	}
}
