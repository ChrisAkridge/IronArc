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

namespace IronArcHost
{
	public partial class NewVMForm : Form
	{
		public NewVMForm()
		{
			InitializeComponent();
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			string initialProgramPath = this.TextBoxInitialProgram.Text;
			string systemProgramPath = this.TextBoxSystemProgram.Text;
			int systemMemorySize = (int)this.NumUDSystemMemory.Value;
			int stackSize = (int)this.NumUDSystemStack.Value;
			int loadAddress = (int)this.NumUDLoadAtAddress.Value;

			// Validate the selected files, memory sizes, and program load address.
			if (!File.Exists(initialProgramPath))
			{
				MessageBox.Show("The initial program file does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!File.Exists(systemProgramPath))
			{
				MessageBox.Show("The system program file does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (loadAddress >= systemMemorySize)
			{
				MessageBox.Show("The loading address of the initial program is past the allocated amount of system memory.", "Loading Address Out of Bounds", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			FileInfo initialProgramInfo = new FileInfo(initialProgramPath);
			if ((systemMemorySize - loadAddress) < initialProgramInfo.Length)
			{
				MessageBox.Show("The initial program is larger than the space given to it; it is loaded too far into the system memory.", "Program Too Large", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// TODO: Add checks for the hardware devices
			// TODO: Create a VM with these parameters and give it to the host

			this.DialogResult = DialogResult.OK;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void ButtonSelectInitialProgram_Click(object sender, EventArgs e)
		{
			if (this.OFDInitialProgram.ShowDialog() == DialogResult.OK)
			{
				string filePath = this.OFDInitialProgram.FileName;
				this.TextBoxInitialProgram.Text = filePath;
			}
		}

		private void ButtonSelectSystemProgram_Click(object sender, EventArgs e)
		{
			if (this.OFDSystemProgram.ShowDialog() == DialogResult.OK)
			{
				string filePath = this.OFDSystemProgram.FileName;
				this.TextBoxSystemProgram.Text = filePath;
			}
		}
	}
}
