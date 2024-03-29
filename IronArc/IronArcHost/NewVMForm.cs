﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace IronArcHost
{
    public partial class NewVMForm : Form
    {
        private bool inItemCheckHandler;
        
        public string ProgramPath { get; private set; }
        public ulong MemorySize { get; private set; }
        public ulong ProgramLoadAddress { get; private set; }
        public List<NewVMHardwareSelection> HardwareSelections { get; } =
            new List<NewVMHardwareSelection>();
        public bool StartInDebugger => CheckStartInDebugger.Checked;

        public NewVMForm()
        {
            InitializeComponent();

            foreach (var hardware in HardwareSearcher.HardwareDeviceTypes)
            {
                CLBInitialHardwareDevices.Items.Add(hardware.FullName ?? throw new InvalidOperationException(), CheckState.Unchecked);
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

        private void CLBInitialHardwareDevices_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (inItemCheckHandler) { return; }
            
            inItemCheckHandler = true;
            var hwDeviceTypeName = (string)CLBInitialHardwareDevices.Items[e.Index];
            
            if (e.NewValue != CheckState.Checked)
            {
                HardwareSelections.RemoveAll(s => s.DeviceTypeName == hwDeviceTypeName);
                return;
            }
            
            if (!HardwareSearcher.InquireForHardwareConfiguration(hwDeviceTypeName, out var configuration))
            {
                CLBInitialHardwareDevices.SetItemCheckState(e.Index, CheckState.Unchecked);
                return;
            }

            HardwareSelections.Add(new NewVMHardwareSelection(hwDeviceTypeName, configuration));
            
            inItemCheckHandler = false;
        }
    }
}
