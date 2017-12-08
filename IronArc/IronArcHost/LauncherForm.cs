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
using IronArc;

namespace IronArcHost
{
	public partial class LauncherForm : Form
	{
		public List<VirtualMachine> VirtualMachines = new List<VirtualMachine>();
		public CoreHardwareProvider provider = new CoreHardwareProvider();

		public LauncherForm()
		{
			InitializeComponent();

			HardwareSearcher.FindHardwareInIronArc();
			HardwareProvider.Provider = provider;
		}

		private void CreateVM(string programPath, ulong memorySize, ulong loadAddress,
			IEnumerable<string> hardwareDeviceNames)
		{
			var vm = new VirtualMachine(memorySize, programPath, loadAddress,
				hardwareDeviceNames);
			VirtualMachines.Add(vm);

			var lvi = new ListViewItem(vm.State.ToString());
			lvi.SubItems.Add(memorySize.ToString());
			lvi.SubItems.Add(vm.Hardware.Count.ToString());
			ListVMs.Items.Add(lvi);
		}

		private void TSBAddVM_Click(object sender, EventArgs e)
		{
			var newVMForm = new NewVMForm();
			if (newVMForm.ShowDialog() == DialogResult.OK)
			{
				CreateVM(newVMForm.ProgramPath, newVMForm.MemorySize, newVMForm.ProgramLoadAddress,
					newVMForm.HardwareDeviceNames);
			}
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

		private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			foreach (var terminalForm in provider.Terminals)
			{
				terminalForm.Close();
			}
		}
	}
}
