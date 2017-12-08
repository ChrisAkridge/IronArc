namespace IronArcHost
{
	partial class LauncherForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.TSBAddVM = new System.Windows.Forms.ToolStripButton();
			this.TSBRemoveVM = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.TSBToggleVMState = new System.Windows.Forms.ToolStripButton();
			this.TSBShowTerminal = new System.Windows.Forms.ToolStripButton();
			this.TSBShowDebugger = new System.Windows.Forms.ToolStripButton();
			this.TSBHardware = new System.Windows.Forms.ToolStripButton();
			this.ListVMs = new System.Windows.Forms.ListView();
			this.LVCState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.LVCMemory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.LVCHardwareDeviceCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSBAddVM,
            this.TSBRemoveVM,
            this.toolStripSeparator1,
            this.TSBToggleVMState,
            this.TSBShowTerminal,
            this.TSBShowDebugger,
            this.TSBHardware});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(566, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// TSBAddVM
			// 
			this.TSBAddVM.Image = ((System.Drawing.Image)(resources.GetObject("TSBAddVM.Image")));
			this.TSBAddVM.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBAddVM.Name = "TSBAddVM";
			this.TSBAddVM.Size = new System.Drawing.Size(79, 22);
			this.TSBAddVM.Text = "&Add VM...";
			this.TSBAddVM.Click += new System.EventHandler(this.TSBAddVM_Click);
			// 
			// TSBRemoveVM
			// 
			this.TSBRemoveVM.Enabled = false;
			this.TSBRemoveVM.Image = ((System.Drawing.Image)(resources.GetObject("TSBRemoveVM.Image")));
			this.TSBRemoveVM.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBRemoveVM.Name = "TSBRemoveVM";
			this.TSBRemoveVM.Size = new System.Drawing.Size(100, 22);
			this.TSBRemoveVM.Text = "&Remove VM...";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// TSBToggleVMState
			// 
			this.TSBToggleVMState.Image = ((System.Drawing.Image)(resources.GetObject("TSBToggleVMState.Image")));
			this.TSBToggleVMState.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBToggleVMState.Name = "TSBToggleVMState";
			this.TSBToggleVMState.Size = new System.Drawing.Size(88, 22);
			this.TSBToggleVMState.Text = "&Pause VM...";
			this.TSBToggleVMState.Click += new System.EventHandler(this.TSBToggleVMState_Click);
			// 
			// TSBShowTerminal
			// 
			this.TSBShowTerminal.Image = ((System.Drawing.Image)(resources.GetObject("TSBShowTerminal.Image")));
			this.TSBShowTerminal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBShowTerminal.Name = "TSBShowTerminal";
			this.TSBShowTerminal.Size = new System.Drawing.Size(82, 22);
			this.TSBShowTerminal.Text = "&Terminal...";
			this.TSBShowTerminal.Click += new System.EventHandler(this.TSBShowTerminal_Click);
			// 
			// TSBShowDebugger
			// 
			this.TSBShowDebugger.Image = ((System.Drawing.Image)(resources.GetObject("TSBShowDebugger.Image")));
			this.TSBShowDebugger.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBShowDebugger.Name = "TSBShowDebugger";
			this.TSBShowDebugger.Size = new System.Drawing.Size(88, 22);
			this.TSBShowDebugger.Text = "&Debugger...";
			this.TSBShowDebugger.Click += new System.EventHandler(this.TSBShowDebugger_Click);
			// 
			// TSBHardware
			// 
			this.TSBHardware.Image = ((System.Drawing.Image)(resources.GetObject("TSBHardware.Image")));
			this.TSBHardware.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBHardware.Name = "TSBHardware";
			this.TSBHardware.Size = new System.Drawing.Size(87, 22);
			this.TSBHardware.Text = "&Hardware...";
			this.TSBHardware.Click += new System.EventHandler(this.TSBHardware_Click);
			// 
			// ListVMs
			// 
			this.ListVMs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LVCState,
            this.LVCMemory,
            this.LVCHardwareDeviceCount});
			this.ListVMs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ListVMs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ListVMs.Location = new System.Drawing.Point(0, 25);
			this.ListVMs.Name = "ListVMs";
			this.ListVMs.Size = new System.Drawing.Size(566, 323);
			this.ListVMs.TabIndex = 1;
			this.ListVMs.UseCompatibleStateImageBehavior = false;
			this.ListVMs.View = System.Windows.Forms.View.Details;
			// 
			// LVCState
			// 
			this.LVCState.Text = "State";
			this.LVCState.Width = 65;
			// 
			// LVCMemory
			// 
			this.LVCMemory.Text = "Memory";
			// 
			// LVCHardwareDeviceCount
			// 
			this.LVCHardwareDeviceCount.Text = "HW Device Count";
			this.LVCHardwareDeviceCount.Width = 105;
			// 
			// LauncherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(566, 348);
			this.Controls.Add(this.ListVMs);
			this.Controls.Add(this.toolStrip1);
			this.Name = "LauncherForm";
			this.Text = "IronArc";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LauncherForm_FormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton TSBAddVM;
		private System.Windows.Forms.ToolStripButton TSBRemoveVM;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton TSBToggleVMState;
		private System.Windows.Forms.ToolStripButton TSBShowTerminal;
		private System.Windows.Forms.ToolStripButton TSBShowDebugger;
		private System.Windows.Forms.ListView ListVMs;
		private System.Windows.Forms.ColumnHeader LVCState;
		private System.Windows.Forms.ColumnHeader LVCMemory;
		private System.Windows.Forms.ColumnHeader LVCHardwareDeviceCount;
		private System.Windows.Forms.ToolStripButton TSBHardware;
	}
}