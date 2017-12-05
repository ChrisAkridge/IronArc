namespace IronArcHost
{
	partial class HardwareForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.StaticLabelSelectedDevices = new System.Windows.Forms.Label();
			this.ListSelectedDevices = new System.Windows.Forms.ListView();
			this.ColumnDeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ColumnDeviceStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ListAvailableDevices = new System.Windows.Forms.ListBox();
			this.ButtonAddDevice = new System.Windows.Forms.Button();
			this.ButtonRemoveDevice = new System.Windows.Forms.Button();
			this.ButtonChangeStatus = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// StaticLabelSelectedDevices
			// 
			this.StaticLabelSelectedDevices.AutoSize = true;
			this.StaticLabelSelectedDevices.Location = new System.Drawing.Point(13, 9);
			this.StaticLabelSelectedDevices.Name = "StaticLabelSelectedDevices";
			this.StaticLabelSelectedDevices.Size = new System.Drawing.Size(94, 13);
			this.StaticLabelSelectedDevices.TabIndex = 0;
			this.StaticLabelSelectedDevices.Text = "Selected Devices:";
			// 
			// ListSelectedDevices
			// 
			this.ListSelectedDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnDeviceName,
            this.ColumnDeviceStatus});
			this.ListSelectedDevices.Location = new System.Drawing.Point(12, 25);
			this.ListSelectedDevices.Name = "ListSelectedDevices";
			this.ListSelectedDevices.Size = new System.Drawing.Size(154, 280);
			this.ListSelectedDevices.TabIndex = 1;
			this.ListSelectedDevices.UseCompatibleStateImageBehavior = false;
			this.ListSelectedDevices.View = System.Windows.Forms.View.Details;
			// 
			// ColumnDeviceName
			// 
			this.ColumnDeviceName.Text = "Device Name";
			this.ColumnDeviceName.Width = 83;
			// 
			// ColumnDeviceStatus
			// 
			this.ColumnDeviceStatus.Text = "Status";
			// 
			// ListAvailableDevices
			// 
			this.ListAvailableDevices.FormattingEnabled = true;
			this.ListAvailableDevices.Location = new System.Drawing.Point(224, 25);
			this.ListAvailableDevices.Name = "ListAvailableDevices";
			this.ListAvailableDevices.Size = new System.Drawing.Size(120, 277);
			this.ListAvailableDevices.TabIndex = 2;
			// 
			// ButtonAddDevice
			// 
			this.ButtonAddDevice.Enabled = false;
			this.ButtonAddDevice.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.ButtonAddDevice.Location = new System.Drawing.Point(173, 133);
			this.ButtonAddDevice.Name = "ButtonAddDevice";
			this.ButtonAddDevice.Size = new System.Drawing.Size(45, 23);
			this.ButtonAddDevice.TabIndex = 3;
			this.ButtonAddDevice.Text = "<<";
			this.ButtonAddDevice.UseVisualStyleBackColor = true;
			// 
			// ButtonRemoveDevice
			// 
			this.ButtonRemoveDevice.Enabled = false;
			this.ButtonRemoveDevice.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.ButtonRemoveDevice.Location = new System.Drawing.Point(173, 162);
			this.ButtonRemoveDevice.Name = "ButtonRemoveDevice";
			this.ButtonRemoveDevice.Size = new System.Drawing.Size(45, 23);
			this.ButtonRemoveDevice.TabIndex = 4;
			this.ButtonRemoveDevice.Text = ">>";
			this.ButtonRemoveDevice.UseVisualStyleBackColor = true;
			// 
			// ButtonChangeStatus
			// 
			this.ButtonChangeStatus.Location = new System.Drawing.Point(12, 312);
			this.ButtonChangeStatus.Name = "ButtonChangeStatus";
			this.ButtonChangeStatus.Size = new System.Drawing.Size(154, 23);
			this.ButtonChangeStatus.TabIndex = 5;
			this.ButtonChangeStatus.Text = "&Change Device Status...";
			this.ButtonChangeStatus.UseVisualStyleBackColor = true;
			// 
			// ButtonOK
			// 
			this.ButtonOK.Location = new System.Drawing.Point(224, 312);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(119, 23);
			this.ButtonOK.TabIndex = 6;
			this.ButtonOK.Text = "&OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			// 
			// HardwareForm
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(356, 340);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.ButtonChangeStatus);
			this.Controls.Add(this.ButtonRemoveDevice);
			this.Controls.Add(this.ButtonAddDevice);
			this.Controls.Add(this.ListAvailableDevices);
			this.Controls.Add(this.ListSelectedDevices);
			this.Controls.Add(this.StaticLabelSelectedDevices);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "HardwareForm";
			this.Text = "Hardware Devices";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label StaticLabelSelectedDevices;
		private System.Windows.Forms.ListView ListSelectedDevices;
		private System.Windows.Forms.ColumnHeader ColumnDeviceName;
		private System.Windows.Forms.ColumnHeader ColumnDeviceStatus;
		private System.Windows.Forms.ListBox ListAvailableDevices;
		private System.Windows.Forms.Button ButtonAddDevice;
		private System.Windows.Forms.Button ButtonRemoveDevice;
		private System.Windows.Forms.Button ButtonChangeStatus;
		private System.Windows.Forms.Button ButtonOK;
	}
}