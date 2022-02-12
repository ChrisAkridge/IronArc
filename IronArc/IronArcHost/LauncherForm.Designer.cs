﻿namespace IronArcHost
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
            this.components = new System.ComponentModel.Container();
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
            this.LVCMachineID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVCState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVCMemory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVCHardwareDeviceCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVCInstructionExecutedCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LVCInstructionsPerSecond = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TmrMessageQueueCheck = new System.Windows.Forms.Timer(this.components);
            this.TmrUpdateInstructionCount = new System.Windows.Forms.Timer(this.components);
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.TSMITools = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISaveHardwareDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.SFDHardwareDefinition = new System.Windows.Forms.SaveFileDialog();
            this.TSMIPartitionManager = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.MenuStrip.SuspendLayout();
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
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
            this.TSBToggleVMState.Enabled = false;
            this.TSBToggleVMState.Image = ((System.Drawing.Image)(resources.GetObject("TSBToggleVMState.Image")));
            this.TSBToggleVMState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBToggleVMState.Name = "TSBToggleVMState";
            this.TSBToggleVMState.Size = new System.Drawing.Size(88, 22);
            this.TSBToggleVMState.Text = "&Pause VM...";
            this.TSBToggleVMState.Click += new System.EventHandler(this.TSBToggleVMState_Click);
            // 
            // TSBShowTerminal
            // 
            this.TSBShowTerminal.Enabled = false;
            this.TSBShowTerminal.Image = ((System.Drawing.Image)(resources.GetObject("TSBShowTerminal.Image")));
            this.TSBShowTerminal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBShowTerminal.Name = "TSBShowTerminal";
            this.TSBShowTerminal.Size = new System.Drawing.Size(81, 22);
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
            this.TSBHardware.Enabled = false;
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
            this.LVCMachineID,
            this.LVCState,
            this.LVCMemory,
            this.LVCHardwareDeviceCount,
            this.LVCInstructionExecutedCount,
            this.LVCInstructionsPerSecond});
            this.ListVMs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListVMs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListVMs.HideSelection = false;
            this.ListVMs.Location = new System.Drawing.Point(0, 49);
            this.ListVMs.Name = "ListVMs";
            this.ListVMs.Size = new System.Drawing.Size(566, 299);
            this.ListVMs.TabIndex = 1;
            this.ListVMs.UseCompatibleStateImageBehavior = false;
            this.ListVMs.View = System.Windows.Forms.View.Details;
            this.ListVMs.SelectedIndexChanged += new System.EventHandler(this.ListVMs_SelectedIndexChanged);
            // 
            // LVCMachineID
            // 
            this.LVCMachineID.Text = "Machine ID";
            this.LVCMachineID.Width = 104;
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
            // LVCInstructionExecutedCount
            // 
            this.LVCInstructionExecutedCount.Text = "Instructions Executed";
            this.LVCInstructionExecutedCount.Width = 137;
            // 
            // LVCInstructionsPerSecond
            // 
            this.LVCInstructionsPerSecond.Text = "IPS";
            // 
            // TmrMessageQueueCheck
            // 
            this.TmrMessageQueueCheck.Enabled = true;
            this.TmrMessageQueueCheck.Tick += new System.EventHandler(this.TmrMessageQueueCheck_Tick);
            // 
            // TmrUpdateInstructionCount
            // 
            this.TmrUpdateInstructionCount.Enabled = true;
            this.TmrUpdateInstructionCount.Interval = 1000;
            this.TmrUpdateInstructionCount.Tick += new System.EventHandler(this.TmrUpdateInstructionCount_Tick);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMITools});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(566, 24);
            this.MenuStrip.TabIndex = 2;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // TSMITools
            // 
            this.TSMITools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMISaveHardwareDefinition,
            this.TSMIPartitionManager});
            this.TSMITools.Name = "TSMITools";
            this.TSMITools.Size = new System.Drawing.Size(46, 20);
            this.TSMITools.Text = "&Tools";
            // 
            // TSMISaveHardwareDefinition
            // 
            this.TSMISaveHardwareDefinition.Name = "TSMISaveHardwareDefinition";
            this.TSMISaveHardwareDefinition.Size = new System.Drawing.Size(251, 22);
            this.TSMISaveHardwareDefinition.Text = "&Save Hardware Definition to File...";
            this.TSMISaveHardwareDefinition.Click += new System.EventHandler(this.TSMISaveHardwareDefinition_Click);
            // 
            // SFDHardwareDefinition
            // 
            this.SFDHardwareDefinition.DefaultExt = "json";
            this.SFDHardwareDefinition.Filter = "JSON Files (*.json)|*.json|All files|*.*";
            this.SFDHardwareDefinition.Title = "IronArc Host - Save Hardware Definition";
            // 
            // TSMIPartitionManager
            // 
            this.TSMIPartitionManager.Name = "TSMIPartitionManager";
            this.TSMIPartitionManager.Size = new System.Drawing.Size(251, 22);
            this.TSMIPartitionManager.Text = "Partition Manager";
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 348);
            this.Controls.Add(this.ListVMs);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "LauncherForm";
            this.Text = "IronArc Host";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LauncherForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
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
		private System.Windows.Forms.Timer TmrMessageQueueCheck;
		private System.Windows.Forms.ColumnHeader LVCMachineID;
		private System.Windows.Forms.ColumnHeader LVCInstructionExecutedCount;
		private System.Windows.Forms.Timer TmrUpdateInstructionCount;
		private System.Windows.Forms.ColumnHeader LVCInstructionsPerSecond;
		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem TSMITools;
		private System.Windows.Forms.ToolStripMenuItem TSMISaveHardwareDefinition;
		private System.Windows.Forms.SaveFileDialog SFDHardwareDefinition;
        private System.Windows.Forms.ToolStripMenuItem TSMIPartitionManager;
    }
}