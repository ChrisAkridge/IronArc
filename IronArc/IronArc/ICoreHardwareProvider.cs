using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronArc.Hardware;

namespace IronArc
{
	public interface ICoreHardwareProvider
	{
		ConcurrentQueue<Message> UIMessageQueue { get; }

		ITerminal CreateTerminal();
		void DestroyTerminal(ITerminal terminal);
	}
}
