using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
		private DebugVM vm;

		private DisassemblyWindow disassemblyWindow;

		public DebuggerForm(DebugVM vm)
		{
			this.vm = vm;

			InitializeComponent();

			disassemblyWindow = new DisassemblyWindow(vm.CreateMemoryStream(),
				(ListDisassembly.Height / ListDisassembly.ItemHeight));
			disassemblyWindow.InstructionsChanged += DisassemblyWindow_InstructionsChanged;

			RefreshDisassemblyList();

			HexMemory.ByteProvider = new VMMemoryByteProvider(vm);
		}

		private void DisassemblyWindow_InstructionsChanged(object sender, EventArgs e)
		{
			RefreshDisassemblyList();
		}

		private void RefreshDisassemblyList()
		{
			ListDisassembly.Items.Clear();

			int numberOfItems = (ListDisassembly.Height / ListDisassembly.ItemHeight);
			for (int i = 0; i < numberOfItems; i++)
			{
				ListDisassembly.Items.Add(disassemblyWindow.GetInstructionAtWindowPosition(i));
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

		private void LinkLabelEAX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			new RegisterEditor().ShowDialog();
		}

		private void LinkEBX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkECX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEDX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEEX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEFX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEGX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEHX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkIP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEBASE_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEFLAGS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void TextIP_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
