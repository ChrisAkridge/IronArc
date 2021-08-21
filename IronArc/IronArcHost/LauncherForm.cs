using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using IronArc;
using IronArc.HardwareDefinitionGenerator;

namespace IronArcHost
{
    public partial class LauncherForm : Form
    {
        private const string HardwareDefinitionVersion = "2020-05-20";
        private Guid? machineIDWaitingToDebug;

        public LauncherForm()
        {
            InitializeComponent();

            VMManager.Initialize();
            VMManager.VMStateChangeEvent += LauncherForm_VMStateChangeEvent;
        }

        private void LauncherForm_VMStateChangeEvent(object sender, IronArc.Message e)
        {
            foreach (var listItem in ListVMs.Items.Cast<ListViewItem>()
                .Where(listItem => listItem.Text == e.MachineID.ToString()))
            {
                listItem.SubItems[1].Text = ((VMState)e.WParam).ToString();
            }

            if (machineIDWaitingToDebug == e.MachineID)
            {
                machineIDWaitingToDebug = null;
                new DebuggerForm(VMManager.Lookup(e.MachineID)).ShowDialog();
            }
        }

        private void TSBAddVM_Click(object sender, EventArgs e)
        {
            var newVMForm = new NewVMForm();

            if (newVMForm.ShowDialog() != DialogResult.OK) { return; }

            var newMachineID = VMManager.CreateVM(newVMForm.ProgramPath, newVMForm.MemorySize, newVMForm.ProgramLoadAddress,
                newVMForm.HardwareDeviceNames);

            var vm = VMManager.Lookup(newMachineID);
            var lvi = new ListViewItem(vm.MachineId.ToString());
            lvi.SubItems.Add(vm.State.ToString());
            lvi.SubItems.Add(newVMForm.MemorySize.ToString());
            lvi.SubItems.Add(vm.Hardware.Count.ToString());
            lvi.SubItems.Add("0");
            lvi.SubItems.Add("0");
            ListVMs.Items.Add(lvi);

            if (!newVMForm.StartInDebugger)
            {
                VMManager.ResumeVM(newMachineID);
            }
            else
            {
                new DebuggerForm(vm).ShowDialog();
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
            VirtualMachine vm = VMManager.Lookup(machineID);

            ShowTerminalForVM(vm, notifyIfNoTerminal: true, terminalShouldBeModal: true);
        }

        private static void ShowTerminalForVM(VirtualMachine vm, bool notifyIfNoTerminal, bool terminalShouldBeModal)
        {
            var terminalForm = VMManager.Provider.Terminals.FirstOrDefault(f => f.MachineId == vm.MachineId);
            if (terminalForm != null)
            {
                if (terminalShouldBeModal) { terminalForm.ShowDialog(); }
                else { terminalForm.Show(); }
            }
            else
            {
                if (notifyIfNoTerminal)
                {
                    MessageBox.Show("No terminal was attached to this VM.", "IronArc", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void TSBShowDebugger_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = ListVMs.SelectedItems[0];
            var machineID = Guid.Parse(lvi.Text);
            machineIDWaitingToDebug = machineID;

            VMManager.PauseVM(machineID);
        }

        private void TSBHardware_Click(object sender, EventArgs e)
        {
            var machineID = Guid.Parse(ListVMs.SelectedItems[0].Text);
            var hardwareForm = new HardwareForm(machineID);
            hardwareForm.Show();
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

        private void TmrUpdateInstructionCount_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ListVMs.Items.Count; i++)
            {
                var item = ListVMs.Items[i];
                var machineID = Guid.Parse(item.SubItems[0].Text);
                var instructionCount = VMManager.ReadInstructionExecutedCount(machineID);
                var lastInstructionCount = ulong.Parse(item.SubItems[4].Text, NumberStyles.AllowThousands);

                item.SubItems[4].Text = $"{instructionCount:n0}";
                item.SubItems[5].Text = $"{(instructionCount - lastInstructionCount):n0}";
            }
        }

        private void TSMISaveHardwareDefinition_Click(object sender, EventArgs e)
        {
            if (SFDHardwareDefinition.ShowDialog() != DialogResult.OK) { return; }
            string filePath = SFDHardwareDefinition.FileName;

            var hardwareDefinition = Generator.GenerateHardwareDefinition(HardwareDefinitionVersion);

            try
            {
                System.IO.File.WriteAllText(filePath, hardwareDefinition);
            }
            catch (Exception ex)
            {
                string message = $"Could not save the hardware definition to \"{filePath}\".\r\n";
                message += $"An exception of type {ex.GetType().Name} was thrown.\r\n";
                message += $"Message: \"{ex.Message}\"\r\n";
                message += $"Call Stack:\r\n";
                message += ex.StackTrace;

                MessageBox.Show(message, "IronArc Host", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
