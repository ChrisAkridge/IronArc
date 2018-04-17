using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc
{
	public sealed class DebugVM
	{
		private VirtualMachine vm;

		public event EventHandler<EventArgs> CallOccurred;
		public event EventHandler<EventHandler> ReturnOccurred;

		public long MemorySize => (long)vm.Memory.Length;

		public DebugVM(VirtualMachine vm)
		{
			this.vm = vm;
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
	}
}
