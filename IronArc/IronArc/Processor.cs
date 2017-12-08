using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public sealed class Processor
	{
		public ulong EAX;
		public ulong EBX;
		public ulong ECX;
		public ulong EDX;
		public ulong EEX;
		public ulong EFX;
		public ulong EGX;
		public ulong EHX;

		public ulong EBP;
		public ulong ESP;

		public ulong EIP;
		public ulong EFLAGS;
		public ulong ERP;

		public ulong stackArgsMarker;
		private ByteBlock memory;

		public Dictionary<string, List<InterruptHandler>> interruptTable;
		public Dictionary<uint, ulong> errorTable;
		public Stack<CallStackFrame> callStack;

		public Processor(ByteBlock memory, ulong firstInstructionAddress)
		{
			this.memory = memory;

			interruptTable = new Dictionary<string, List<InterruptHandler>>();
			errorTable = new Dictionary<uint, ulong>();
			callStack = new Stack<CallStackFrame>();

			EIP = ERP = firstInstructionAddress;
		}
	}
}
