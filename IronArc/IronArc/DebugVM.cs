using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IronArc
{
	public sealed class DebugVM
	{
		private VirtualMachine vm;
		private List<Breakpoint> breakpoints;
		private ConcurrentQueue<Message> uiMessageQueue;
		private DebugRunUntilType runUntilType = DebugRunUntilType.NotRunning;

		public event EventHandler CallOccurred;
		public event EventHandler ReturnOccurred;
		public event EventHandler DebugDisplayInvalidated;

		public long MemorySize => (long)vm.Memory.Length;
		public IReadOnlyList<Breakpoint> Breakpoints => breakpoints.AsReadOnly();
		public ConcurrentQueue<Message> MessageQueue { get; }
		public VMState VMState => vm.State;
		public Guid MachineID => vm.MachineID;

		public DebugVM(VirtualMachine vm, ConcurrentQueue<Message> uiMessageQueue)
		{
			this.vm = vm;
			this.uiMessageQueue = uiMessageQueue;
			breakpoints = new List<Breakpoint>();
			MessageQueue = new ConcurrentQueue<Message>();
		}

		#region Register Properties
		public ulong EAX
		{
			get => vm.Processor.EAX;
			set => vm.Processor.EAX = value;
		}

		public ulong EBX
		{
			get => vm.Processor.EBX;
			set => vm.Processor.EBX = value;
		}

		public ulong ECX
		{
			get => vm.Processor.ECX;
			set => vm.Processor.ECX = value;
		}

		public ulong EDX
		{
			get => vm.Processor.EDX;
			set => vm.Processor.EDX = value;
		}

		public ulong EEX
		{
			get => vm.Processor.EEX;
			set => vm.Processor.EEX = value;
		}

		public ulong EFX
		{
			get => vm.Processor.EFX;
			set => vm.Processor.EFX = value;
		}

		public ulong EGX
		{
			get => vm.Processor.EGX;
			set => vm.Processor.EGX = value;
		}

		public ulong EHX
		{
			get => vm.Processor.EHX;
			set => vm.Processor.EHX = value;
		}

		public ulong EBP
		{
			get => vm.Processor.EBP;
			set => vm.Processor.EBP = value;
		}

		public ulong ESP
		{
			get => vm.Processor.ESP;
			set => vm.Processor.ESP = value;
		}

		public ulong EIP
		{
			get => vm.Processor.EIP;
			set => vm.Processor.EIP = value;
		}

		public ulong ERP
		{
			get => vm.Processor.ERP;
			set => vm.Processor.ERP = value;
		}

		public ulong EFLAGS
		{
			get => vm.Processor.EFLAGS;
			set => vm.Processor.EFLAGS = value;
		}
		#endregion

		public UnmanagedMemoryStream CreateMemoryStream() => vm.Memory.CreateStream();
		public CallStackFrame GetCallStackTop() => vm.Processor.callStack.Peek();

		public IEnumerable<CallStackFrame> GetCallStack()
		{
			foreach (CallStackFrame frame in vm.Processor.callStack)
			{
				yield return frame;
			}
		}

		public byte ReadByte(long address) => vm.Memory.ReadByteAt((ulong)address);
		public void WriteByte(long address, byte value) => vm.Memory.WriteByteAt(value, (ulong)address);

		public void AddBreakpoint(ulong address, bool isUserVisible) =>
			breakpoints.Add(new Breakpoint(address, isUserVisible));

		public bool RemoveBreakpoint(ulong address) =>
			breakpoints.RemoveAll(b => b.Address == address) > 0;

		public bool AddressHasUserVisibleBreakpoint(ulong address)
		{
			foreach (Breakpoint breakpoint in breakpoints)
			{
				if (breakpoint.Address == address)
				{
					return breakpoint.IsUserVisible;
				}
			}
			return false;
		}

		private void StartInstructionLoopThread()
		{
			var uiMessage = new Message(VMMessage.None, UIMessage.VMStateChanged, vm.MachineID,
				(int)VMState.Running, 0L, null);
			uiMessageQueue.Enqueue(uiMessage);

			var worker = new Thread(InstructionLoop);

			if (runUntilType == DebugRunUntilType.StepOver)
			{
				worker.Name = $"Step Over {{{vm.MachineID}}}";
			}
			else
			{
				worker.Name = $"Debug {{{vm.MachineID}}}";
			}

			worker.Start();
		}

		private void InstructionLoop()
		{
			while (runUntilType != DebugRunUntilType.NotRunning)
			{
				vm.ExecuteOneInstruction();

				if (CheckAndBreakOnBreakpoint()) { NotifyUIThreadOfPause(); break; }
				else if (!MessageQueue.IsEmpty)
				{
					MessageQueue.TryDequeue(out Message message);

					if (message.VMMessage == VMMessage.SetVMState)
					{
						if ((VMState)message.WParam == VMState.Paused)
						{
							NotifyUIThreadOfPause();
							break;
						}
					}
				}
			}
		}

		private void NotifyUIThreadOfPause()
		{
			runUntilType = DebugRunUntilType.NotRunning;

			var uiMessage = new Message(VMMessage.None, UIMessage.VMStateChanged, vm.MachineID,
				(int)VMState.Paused, 0L, null);
			uiMessageQueue.Enqueue(uiMessage);
		}

		private bool CheckAndBreakOnBreakpoint()
		{
			Breakpoint breakpointToRemove = null;

			foreach (Breakpoint breakpoint in breakpoints)
			{
				if (EIP == breakpoint.Address)
				{
					if (!breakpoint.IsUserVisible)
					{
						breakpointToRemove = breakpoint;
					}
					else { return true; }
				}
			}

			if (breakpointToRemove != null)
			{
				breakpoints.Remove(breakpointToRemove);
				return true;
			}
			return false;
		}

		public void Run()
		{
			runUntilType = DebugRunUntilType.RunContinously;
			StartInstructionLoopThread();
		}

		public void Pause()
		{
			NotifyUIThreadOfPause();
		}

		public void StepInto()
		{
			vm.ExecuteOneInstruction();
			OnDebugDisplayInvalidated();
		}

		public void StepOver()
		{
			bool isCallInstruction = vm.Memory.ReadUShortAt(EIP) == 0x0003;
			if (!isCallInstruction)
			{
				StepInto();
			}
			else
			{
				runUntilType = DebugRunUntilType.StepOver;

				// Find the length of the call instruction so we can set a breakpoint right after
				// it.
				int callInstructionLength = 3;  // opcode + flags byte
				byte flagsByte = vm.Memory.ReadByteAt(EIP + 2);
				flagsByte = (byte)((flagsByte & 0x30) >> 4);
				if (flagsByte == 0 || flagsByte == 2) { callInstructionLength += 8; }
				else if (flagsByte == 1)
				{
					// Register: can be either 1 byte or 5 bytes if it has an offset
					byte registerByte = vm.Memory.ReadByteAt(EIP + 3);
					if ((registerByte & 0x40) != 0) { callInstructionLength += 5; }
					else { callInstructionLength += 1; }
				}
				else { throw new InvalidDataException($"Found a call instruction at 0x{EIP:X16} that had bad flags/operands."); }

				breakpoints.Add(new Breakpoint(EIP + (ulong)callInstructionLength, false));
				StartInstructionLoopThread();
			}
		}

		public void StepOut()
		{
			runUntilType = DebugRunUntilType.StepOut;

			var uiMessage = new Message(VMMessage.None, UIMessage.VMStateChanged, vm.MachineID,
				(int)VMState.Running, 0L, null);
			uiMessageQueue.Enqueue(uiMessage);

			var worker = new Thread(StepOutInstructionLoop);
			worker.Name = $"Step Out {{{vm.MachineID}}}";
			worker.Start();
		}

		private void StepOutInstructionLoop()
		{
			while (true)
			{
				// We could do what we did in Step Over, which is set an invisible breakpoint
				// right after the call address, but we don't have an easy way to find the next
				// ret or end instruction that will actually be executed. So, instead, we're going
				// to just execute instructions until we reach a ret or end. When we reach a ret,
				// we DO want to execute it, but we DON'T want to execute an end instruction.
				ushort opcode = vm.Memory.ReadUShortAt(EIP);
				if (opcode == 0x0004 /* ret */)
				{
					vm.ExecuteOneInstruction();
					NotifyUIThreadOfPause();
					break;
				}
				else if (opcode == 0x0001 /* end */)
				{
					NotifyUIThreadOfPause();
					break;
				}
				else
				{
					vm.ExecuteOneInstruction();
				}
			}
		}

		private void OnDebugDisplayInvalidated() => DebugDisplayInvalidated?.Invoke(this, new EventArgs());
	}
}
