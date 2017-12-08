using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
				Hardware.Add((HardwareDevice)Activator.CreateInstance(type));
			}

			State = VMState.Paused;
		}
	}
}
