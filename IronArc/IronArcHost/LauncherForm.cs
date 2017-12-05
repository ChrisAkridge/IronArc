using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronArcHost
{
	public partial class LauncherForm : Form
	{
		public LauncherForm()
		{
			InitializeComponent();

			// Open a terminal so we can test cross thread messaging
			var terminalForm = new TerminalForm();
			terminalForm.Show();

			var vmTest = new IronArc.VMThreadingTest("C:\\program.iexe", 1048576, 2048, 
				new List<IronArc.HardwareDevice> { terminalForm.HardwareTerminal });

			var thread = new Thread(vmTest.Start);
			thread.Start();
		}

		private void TSBAddVM_Click(object sender, EventArgs e)
		{
			// temporary
			new NewVMForm().ShowDialog();
		}

		private void TSBToggleVMState_Click(object sender, EventArgs e)
		{

		}

		private void TSBShowTerminal_Click(object sender, EventArgs e)
		{
			// temporary
			new TerminalForm().ShowDialog();
		}

		private void TSBShowDebugger_Click(object sender, EventArgs e)
		{
			// temporary
			new DebuggerForm().ShowDialog();
		}

		private void TSBHardware_Click(object sender, EventArgs e)
		{
			// temporary
			new HardwareForm().ShowDialog();
		}
	}
}
