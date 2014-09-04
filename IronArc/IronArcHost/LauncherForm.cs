using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronArcHost
{
	public partial class LauncherForm : Form
	{
		public LauncherForm()
		{
			InitializeComponent();
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
			new Terminal().ShowDialog();
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
