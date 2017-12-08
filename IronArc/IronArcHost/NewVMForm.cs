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
		public string ProgramPath { get; private set; }
		public ulong MemorySize { get; private set; }
		public ulong ProgramLoadAddress { get; private set; }

		public List<string> HardwareDeviceNames { get; private set; }

		public NewVMForm()
		{
			InitializeComponent();

			foreach (var hardware in HardwareSearcher.HardwareDeviceTypes)
			{
				CLBInitialHardwareDevices.Items.Add(hardware.FullName, CheckState.Unchecked);
			}
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			string initialProgramPath = TextBoxInitialProgram.Text;
			int systemMemorySize = (int)NumUDSystemMemory.Value;
			int loadAddress = (int)NumUDLoadAtAddress.Value;

			// Validate the selected files, memory sizes, and program load address.
			if (!File.Exists(initialProgramPath))
			{
				MessageBox.Show("The initial program file does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show("The initial program is larger than the space given to it or it is loaded too far into the system memory.", "Program Too Large", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ProgramPath = initialProgramPath;
			MemorySize = (ulong)systemMemorySize;
			ProgramLoadAddress = (ulong)loadAddress;

			HardwareDeviceNames = new List<string>();
			for (int i = 0; i < CLBInitialHardwareDevices.Items.Count; i++)
			{
				if (CLBInitialHardwareDevices.GetItemCheckState(i) == CheckState.Checked)
				{
					HardwareDeviceNames.Add((string)CLBInitialHardwareDevices.Items[i]);
				}
			}

			DialogResult = DialogResult.OK;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void ButtonSelectInitialProgram_Click(object sender, EventArgs e)
		{
			if (OFDInitialProgram.ShowDialog() == DialogResult.OK)
			{
				string filePath = OFDInitialProgram.FileName;
				TextBoxInitialProgram.Text = filePath;
			}
		}
	}
}
