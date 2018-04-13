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
		private VirtualMachine vm;
		private IntPtr vmMemoryPointer;
		private ulong vmMemoryLength;

		private DisassemblyWindow disassemblyWindow;

		public DebuggerForm(VirtualMachine vm)
		{
			this.vm = vm;
			vmMemoryPointer = vm.Memory.Pointer;
			vmMemoryLength = vm.Memory.Length;

			InitializeComponent();

			disassemblyWindow = new DisassemblyWindow(vm.Memory.CreateStream(),
				(ListDisassembly.Height / ListDisassembly.ItemHeight));
			disassemblyWindow.InstructionsChanged += DisassemblyWindow_InstructionsChanged;

			RefreshDisassemblyList();
			// WYLO: implementing debugger features. I downloaded Be.HexEditor, which is in my
			// source code folder; it has the same IByteProvider mechanism that HexControlLibrary
			// does, so go ahead and add that
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
			HexMemoryViewer.Model.ByteProvider = new UnmanagedByteProvider(vmMemoryPointer, (int)vmMemoryLength);
			// WYLO: the hex view control here may not be good enough
			// we may be able to fix it, but check for other options
			// right now scrollbars aren't appearing and scrolling doesn't work

			UpdateRegisterDisplay();
		}

		private void UpdateRegisterDisplay()
		{
			TextEAX.Text = vm.Processor.EAX.ToString("X16");
			TextEBX.Text = vm.Processor.EBX.ToString("X16");
			TextECX.Text = vm.Processor.ECX.ToString("X16");
			TextEDX.Text = vm.Processor.EDX.ToString("X16");
			TextEEX.Text = vm.Processor.EEX.ToString("X16");
			TextEFX.Text = vm.Processor.EFX.ToString("X16");
			TextEGX.Text = vm.Processor.EGX.ToString("X16");
			TextEHX.Text = vm.Processor.EHX.ToString("X16");
			TextEBP.Text = vm.Processor.EBP.ToString("X16");
			TextESP.Text = vm.Processor.ESP.ToString("X16");
			TextEIP.Text = vm.Processor.EIP.ToString("X16");
			TextERP.Text = vm.Processor.ERP.ToString("X16");
			TextEFLAGS.Text = vm.Processor.EFLAGS.ToString("X16");
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

		private void HexMemoryViewer_Load(object sender, EventArgs e)
		{
			
		}
	}
}
