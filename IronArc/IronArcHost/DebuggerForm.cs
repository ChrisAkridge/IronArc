using System;
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

		public DebuggerForm(DebugVM vm)
		{
			this.vm = vm;

			InitializeComponent();

			disassemblyWindow = new DisassemblyWindow(vm.CreateMemoryStream(), DisassemblyDisplayedItems);
			disassemblyWindow.InstructionsChanged += DisassemblyWindow_InstructionsChanged;

			RefreshDisassemblyList();

			HexMemory.ByteProvider = new VMMemoryByteProvider(vm);

			SubscribeRegisterLinkClickEvents();
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

		private void DisassemblyWindow_InstructionsChanged(object sender, EventArgs e)
		{
			RefreshDisassemblyList();
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

		private void DebuggerForm_Load(object sender, EventArgs e)
		{
			UpdateRegisterDisplay();
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

			disassemblyWindow.SeekToAddress(vm.EIP);
			UpdateRegisterDisplay();
		}
	}
}
