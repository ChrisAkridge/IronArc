using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
	public sealed class Message
	{
		public VMMessage VMMessage { get; }
		public UIMessage UIMessage { get; }
		public Guid MachineID { get; }
		public int WParam { get; }
		public long LParam { get; }
		public object Data { get; }

		public Message(VMMessage vmMessage, UIMessage uiMessage, Guid machineID, int wParam, long lParam, object data)
		{
			VMMessage = vmMessage;
			UIMessage = uiMessage;
			MachineID = machineID;
			WParam = wParam;
			LParam = lParam;
			Data = data;
		}
	}
}
