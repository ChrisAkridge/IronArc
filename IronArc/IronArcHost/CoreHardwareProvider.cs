using System;
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
		public List<TerminalForm> Terminals { get; private set; } = new List<TerminalForm>();

		public ITerminal CreateTerminal()
		{
			var terminalForm = new TerminalForm();
			Terminals.Add(terminalForm);
			return terminalForm;
		}
	}
}
