using System.Collections.Concurrent;
using System.Collections.Generic;
using IronArc;
using IronArc.Hardware;

namespace IronArcHost
{
    public sealed class CoreHardwareProvider : ICoreHardwareProvider
    {
        public List<TerminalForm> Terminals { get; } = new List<TerminalForm>();
        public List<DynamicTerminalForm> DynamicTerminals { get; } = new List<DynamicTerminalForm>();

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

        public IDynamicTerminal CreateDynamicTerminal()
        {
            var dynamicTerminalForm = new DynamicTerminalForm();
            _ = dynamicTerminalForm.Handle;

            DynamicTerminals.Add(dynamicTerminalForm);
            return dynamicTerminalForm;
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

        public void DestroyDynamicTerminal(IDynamicTerminal dynamicTerminal)
		{
			var dynamicTerminalForm = (DynamicTerminalForm)dynamicTerminal;
			dynamicTerminalForm.Invoke((System.Windows.Forms.MethodInvoker)delegate
            {
				dynamicTerminalForm.Close();
				dynamicTerminalForm.Dispose();
			});

			DynamicTerminals.Remove(dynamicTerminalForm);
		}
    }
}
