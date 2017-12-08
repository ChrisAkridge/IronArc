using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public sealed class CallStackFrame
	{
		internal ulong returnAddress;
		internal ulong calledAddress;
		internal ulong EAX;
		internal ulong EBX;
		internal ulong ECX;
		internal ulong EDX;
		internal ulong EEX;
		internal ulong EFX;
		internal ulong EGX;
		internal ulong EHX;
		internal ulong EFLAGS;
		internal ulong EBP;

		public CallStackFrame(ulong calledAddress, ulong returnAddress, ulong eax, ulong ebx, ulong ecx,
		ulong edx, ulong eex, ulong efx, ulong egx, ulong ehx, ulong eflags, ulong ebp)
		{
			this.calledAddress = calledAddress;
			this.returnAddress = returnAddress;

			EAX = eax;
			EBX = ebx;
			ECX = ecx;
			EDX = edx;
			EEX = eex;
			EFX = efx;
			EGX = egx;
			EHX = ehx;

			EFLAGS = eflags;
			EBP = ebp;
		}
	}
}
