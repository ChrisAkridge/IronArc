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
		public int WParam { get; }
		public int LParam { get; }
		public object Data { get; }

		public Message(VMMessage vmMessage, UIMessage uiMessage, int wParam, long lParam, object data)
		{
			VMMessage = vmMessage;
			UIMessage = uiMessage;
			WParam = wParam;
			LParam = wParam;
			Data = data;
		}
	}
}
