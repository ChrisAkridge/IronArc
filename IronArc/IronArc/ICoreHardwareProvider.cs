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
		ITerminal CreateTerminal();

		ConcurrentQueue<Message> UIMessageQueue { get; }
	}
}
