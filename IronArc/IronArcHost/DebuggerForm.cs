using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronArc;
using IronAssembler.DisassemblyWindows;

namespace IronArcHost
{
	public partial class DebuggerForm : Form
	{
		private const int DisassemblyDisplayedItems = 12;

		private DebugVM vm;
		private DisassemblyWindow disassemblyWindow;
		private ConcurrentQueue<IronArc.Message> messageQueue;
		
		public DebuggerForm(VirtualMachine vm)
		{
			messageQueue = new ConcurrentQueue<IronArc.Message>();
			this.vm = new DebugVM(vm, messageQueue);

			InitializeComponent();

			disassemblyWindow = new DisassemblyWindow(this.vm.CreateMemoryStream(), DisassemblyDisplayedItems);
			disassemblyWindow.InstructionsChanged += DisassemblyWindow_InstructionsChanged;

			RefreshDisassemblyList();

			HexMemory.ByteProvider = new VMMemoryByteProvider(this.vm);

			SubscribeRegisterLinkClickEvents();
			this.vm.DebugDisplayInvalidated += Vm_DebugDisplayInvalidated;
		}
		private void SubscribeRegisterLinkClickEvents()
		{
			LinkEAX.Click += (sender, e) => EditRegister(vm.EAX, v => vm.EAX = v);
			LinkEBX.Click += (sender, e) => EditRegister(vm.EBX, v => vm.EBX = v);
			LinkECX.Click += (sender, e) => EditRegister(vm.ECX, v => vm.ECX = v);
			LinkEDX.Click += (sender, e) => EditRegister(vm.EDX, v => vm.EDX = v);
			LinkEEX.Click += (sender, e) => EditRegister(vm.EEX, v => vm.EEX = v);
			LinkEFX.Click += (sender, e) => EditRegister(vm.EFX, v => vm.EFX = v);
			LinkEGX.Click += (sender, e) => EditRegister(vm.EGX, v => vm.EGX = v);
			LinkEHX.Click += (sender, e) => EditRegister(vm.EHX, v => vm.EHX = v);
			LinkEBP.Click += (sender, e) => EditRegister(vm.EBP, v => vm.EBP = v);
			LinkESP.Click += (sender, e) => EditRegister(vm.ESP, v => vm.ESP = v);
			LinkEIP.Click += (sender, e) => EditRegister(vm.EIP, v => vm.EIP = v);
			LinkERP.Click += (sender, e) => EditRegister(vm.ERP, v => vm.ERP = v);
			LinkEFLAGS.Click += (sender, e) => EditRegister(vm.EFLAGS, v => vm.EFLAGS = v);
		}

		private void RefreshDisassemblyList()
		{
			ListDisassembly.Items.Clear();

			int numberOfItems = DisassemblyDisplayedItems;
			for (int i = 0; i < numberOfItems; i++)
			{
				WindowInstruction instruction = disassemblyWindow.GetInstructionAtWindowPosition(i);
				var lvi = new ListViewItem();
				lvi.Text = instruction.ToString();
				if (vm.AddressHasUserVisibleBreakpoint(instruction.Address))
				{
					lvi.ForeColor = Color.Red;
				}

				ListDisassembly.Items.Add(lvi);
			}
		}

		private void UpdateRegisterDisplay()
		{
			TextEAX.Text = vm.EAX.ToString("X16");
			TextEBX.Text = vm.EBX.ToString("X16");
			TextECX.Text = vm.ECX.ToString("X16");
			TextEDX.Text = vm.EDX.ToString("X16");
			TextEEX.Text = vm.EEX.ToString("X16");
			TextEFX.Text = vm.EFX.ToString("X16");
			TextEGX.Text = vm.EGX.ToString("X16");
			TextEHX.Text = vm.EHX.ToString("X16");
			TextEBP.Text = vm.EBP.ToString("X16");
			TextESP.Text = vm.ESP.ToString("X16");
			TextEIP.Text = vm.EIP.ToString("X16");
			TextERP.Text = vm.ERP.ToString("X16");
			TextEFLAGS.Text = vm.EFLAGS.ToString("X16");
		}

		private void EditRegister(ulong registerValue, Action<ulong> writeRegisterAction)
		{
			var editor = new RegisterEditor(registerValue);
			if (editor.ShowDialog() == DialogResult.OK)
			{
				writeRegisterAction(editor.RegisterValue);
			}

			UpdateRegisterDisplay();
		}

		private void SetControlsEnabledOnVMStateChange(bool vmPaused)
		{
			TSBRun.Enabled = vmPaused;
			TSBStepInto.Enabled = vmPaused;
			TSBStepOver.Enabled = vmPaused;
			TSBStepOut.Enabled = vmPaused;
			TSBAnimate.Enabled = vmPaused;
			ListDisassembly.Enabled = vmPaused;
			HexMemory.Enabled = vmPaused;
			ListCallStackViewer.Enabled = vmPaused;
			ButtonDisassemblyUp.Enabled = vmPaused;
			ButtonDisassemblyDown.Enabled = vmPaused;
			ButtonSetBreakpoint.Enabled = vmPaused;
			ButtonClearBreakpoint.Enabled = vmPaused;
			LinkEAX.Enabled = vmPaused;
			LinkEBX.Enabled = vmPaused;
			LinkECX.Enabled = vmPaused;
			LinkEDX.Enabled = vmPaused;
			LinkEEX.Enabled = vmPaused;
			LinkEFX.Enabled = vmPaused;
			LinkEGX.Enabled = vmPaused;
			LinkEHX.Enabled = vmPaused;
			LinkEBP.Enabled = vmPaused;
			LinkESP.Enabled = vmPaused;
			LinkERP.Enabled = vmPaused;
			LinkEIP.Enabled = vmPaused;
			LinkEFLAGS.Enabled = vmPaused;

			TSBPause.Enabled = !vmPaused;
		}

		private void DisassemblyWindow_InstructionsChanged(object sender, EventArgs e)
		{
			RefreshDisassemblyList();
		}

		private void DebuggerForm_Load(object sender, EventArgs e)
		{
			UpdateRegisterDisplay();
		}

		private void ButtonSetBreakpoint_Click(object sender, EventArgs e)
		{
			if (ListDisassembly.SelectedItems.Count == 0) { return; }

			ListViewItem lvi = ListDisassembly.SelectedItems[0];
			string addressText = lvi.Text.Substring(2, 16);
			ulong address = ulong.Parse(addressText, NumberStyles.AllowHexSpecifier);

			vm.AddBreakpoint(address, isUserVisible: true);
			lvi.ForeColor = Color.Red;
		}

		private void ButtonClearBreakpoint_Click(object sender, EventArgs e)
		{
			if (ListDisassembly.SelectedItems.Count == 0) { return; }

			ListViewItem lvi = ListDisassembly.SelectedItems[0];
			string addressText = lvi.Text.Substring(2, 16);
			ulong address = ulong.Parse(addressText, NumberStyles.HexNumber);

			if (vm.AddressHasUserVisibleBreakpoint(address))
			{
				vm.RemoveBreakpoint(address);
				lvi.ForeColor = Color.Black;
			}
		}

		private void TSBStepInto_Click(object sender, EventArgs e)
		{
			vm.StepInto();
		}

		private void TmrQueueListener_Tick(object sender, EventArgs e)
		{
			if (!messageQueue.IsEmpty)
			{
				messageQueue.TryDequeue(out IronArc.Message message);
				if (message.UIMessage == UIMessage.VMStateChanged)
				{
					if ((VMState)message.WParam == VMState.Paused)
					{
						SetControlsEnabledOnVMStateChange(vmPaused: true);
						UpdateDebugDisplay();
					}
					else if ((VMState)message.WParam == VMState.Running)
					{
						SetControlsEnabledOnVMStateChange(vmPaused: false);
						UpdateDebugDisplay();
					}
				}
			}
		}

		private void TSBStepOver_Click(object sender, EventArgs e)
		{
			// Step over: if the next instruction to be executed is not a call instruction,
			// run it by itself. If it is a call instruction, start the VM on a thread, processing
			// instructions until it reaches the instruction immediately after the call.
			vm.StepOver();
		}

		private void Vm_DebugDisplayInvalidated(object sender, EventArgs e)
		{
			UpdateDebugDisplay();
		}

		private void UpdateDebugDisplay()
		{
			disassemblyWindow.SeekToAddress(vm.EIP);
			UpdateRegisterDisplay();

			if (vm.VMState == VMState.Halted || vm.VMState == VMState.Error)
			{
				SetControlsEnabledOnVMStateChange(vmPaused: false);
				TSBPause.Enabled = false;
				TmrQueueListener.Enabled = false;
			}
		}

		private void TSBStepOut_Click(object sender, EventArgs e)
		{
			vm.StepOut();
		}
	}
}
