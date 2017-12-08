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
		public LauncherForm()
		{
			InitializeComponent();

			VMManager.Initialize();
			VMManager.VMStateChangeEvent += LauncherForm_VMStateChangeEvent;
		}

		private void LauncherForm_VMStateChangeEvent(object sender, IronArc.Message e)
		{
			foreach (ListViewItem listItem in ListVMs.Items)
			{
				if (listItem.Text == e.MachineID.ToString())
				{
					listItem.SubItems[1].Text = ((VMState)e.WParam).ToString();
				}
			}
		}

		private void TSBAddVM_Click(object sender, EventArgs e)
		{
			var newVMForm = new NewVMForm();
			if (newVMForm.ShowDialog() == DialogResult.OK)
			{
				var newMachineID = VMManager.CreateVM(newVMForm.ProgramPath, newVMForm.MemorySize, newVMForm.ProgramLoadAddress,
					newVMForm.HardwareDeviceNames);
				
				var vm = VMManager.Lookup(newMachineID);
				var lvi = new ListViewItem(vm.MachineID.ToString());
				lvi.SubItems.Add(vm.State.ToString());
				lvi.SubItems.Add(newVMForm.MemorySize.ToString());
				lvi.SubItems.Add(vm.Hardware.Count.ToString());
				ListVMs.Items.Add(lvi);

				VMManager.ResumeVM(newMachineID);
			}
		}

		private void TSBToggleVMState_Click(object sender, EventArgs e)
		{
			ListViewItem lvi = ListVMs.SelectedItems[0];
			var machineID = Guid.Parse(lvi.Text);
			var state = lvi.SubItems[1].Text;

			if (state == "Running")
			{
				VMManager.PauseVM(machineID);
				TSBToggleVMState.Text = "Resume VM";
			}
			else if (state == "Paused")
			{
				VMManager.ResumeVM(machineID);
				TSBToggleVMState.Text = "Pause VM";
			}
		}

		private void TSBShowTerminal_Click(object sender, EventArgs e)
		{
			var machineID = Guid.Parse(ListVMs.SelectedItems[0].Text);
			var terminalForm = VMManager.Provider.Terminals.First(f => f.MachineID == machineID);
			terminalForm.Show();
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
			foreach (var terminalForm in VMManager.Provider.Terminals)
			{
				terminalForm.Close();
			}
		}

		private void TmrMessageQueueCheck_Tick(object sender, EventArgs e)
		{
			VMManager.CheckMessageQueue();
		}

		private void ListVMs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ListVMs.SelectedItems.Count == 0)
			{
				TSBToggleVMState.Enabled = false;
				TSBShowTerminal.Enabled = false;
				TSBShowDebugger.Enabled = false;
				TSBHardware.Enabled = false;
				return;
			}

			TSBToggleVMState.Enabled = true;
			TSBShowTerminal.Enabled = true;
			TSBShowDebugger.Enabled = true;
			TSBHardware.Enabled = true;

			ListViewItem lvi = ListVMs.SelectedItems[0];
			if (lvi.SubItems[1].Text == "Running")
			{
				TSBToggleVMState.Text = "Pause VM";
			}
			else if (lvi.SubItems[1].Text == "Paused")
			{
				TSBToggleVMState.Text = "Resume VM";
			}
			else
			{
				TSBToggleVMState.Enabled = false;
			}
		}
	}
}
