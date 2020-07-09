using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc;
using IronArc.Hardware;

namespace IronArcHost
{
	public sealed class CoreHardwareProvider : ICoreHardwareProvider
	{
		public List<TerminalForm> Terminals { get; } = new List<TerminalForm>();

		public ConcurrentQueue<Message> UIMessageQueue => VMManager.UIMessageQueue;

		public ITerminal CreateTerminal()
		{
			var terminalForm = new TerminalForm();

			// https://stackoverflow.com/a/1275589/2709212
			// Windows Forms don't actually make a handle until requested
			// And when a handle is made, it's made on the thread that asked for the handle
			// If we ask for the handle from the VM thread, it's created on the VM thread
			// And this is bad, so we'll poke it here just to make sure it exists on the UI thread
			_ = terminalForm.Handle;

			Terminals.Add(terminalForm);
			return terminalForm;
		}

		public void DestroyTerminal(ITerminal terminal)
		{
			var terminalForm = (TerminalForm)terminal;
			terminalForm.Invoke((System.Windows.Forms.MethodInvoker)delegate
			{
				terminalForm.Close();
				terminalForm.Dispose();
			});

			Terminals.Remove(terminalForm);
		}
	}
}
