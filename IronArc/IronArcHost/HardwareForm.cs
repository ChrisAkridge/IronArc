using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronArc;

namespace IronArcHost
{
	public partial class HardwareForm : Form
	{
		private readonly Guid machineID;
	
		public HardwareForm(Guid machineID)
		{
			InitializeComponent();
			this.machineID = machineID;

			var vm = VMManager.Lookup(machineID);

			// Step 1: List all the hardware devices we can use in ListAvailableDevices
			foreach (var hardwareDeviceName in HardwareSearcher.HardwareDeviceNames)
			{
				ListAvailableDevices.Items.Add(hardwareDeviceName);
			}

			// Step 2: Find which hardware devices are actually running on the VM
			foreach (var hardwareDevice in vm.Hardware)
			{
				int hwDeviceIndex = ListAvailableDevices.Items.IndexOf(hardwareDevice.GetType().FullName);
				ListAvailableDevices.Items.RemoveAt(hwDeviceIndex);

				var selectedDevice = new ListViewItem(hardwareDevice.GetType().FullName);
				selectedDevice.SubItems.Add(hardwareDevice.Status.ToString());
				ListSelectedDevices.Items.Add(selectedDevice);
			}
		}

		private void ListSelectedDevices_SelectedIndexChanged(object sender, EventArgs e)
		{
			ButtonRemoveDevice.Enabled = (ListSelectedDevices.SelectedIndices.Count > 0);
		}

		private void ListAvailableDevices_SelectedIndexChanged(object sender, EventArgs e)
		{
			ButtonAddDevice.Enabled = (ListAvailableDevices.SelectedIndices.Count > 0);
		}

		private void ButtonAddDevice_Click(object sender, EventArgs e)
		{
			int deviceIndex = ListAvailableDevices.SelectedIndex;
			string hwDeviceTypeName = (string)ListAvailableDevices.Items[deviceIndex];

			VMManager.AddHardwareToVM(machineID, hwDeviceTypeName);

			// TODO: we need to add a message to indicate that a hardware device addition was
			// sucessful. Then we need to run the below code when we get the message.
			ListAvailableDevices.Items.RemoveAt(deviceIndex);
			var lvi = new ListViewItem(hwDeviceTypeName);
			lvi.SubItems.Add("Active");

			ListSelectedDevices.Items.Add(lvi);
		}

		private void ButtonRemoveDevice_Click(object sender, EventArgs e)
		{
			int deviceIndex = ListSelectedDevices.SelectedIndices[0];
			string hwDeviceTypeName = ListSelectedDevices.Items[deviceIndex].Text;

			VMManager.RemoveHardwareFromVM(machineID, hwDeviceTypeName);

			ListSelectedDevices.Items.RemoveAt(deviceIndex);
			ListAvailableDevices.Items.Add(hwDeviceTypeName);
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
