﻿namespace IronArcHost
{
	partial class DebuggerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebuggerForm));
            this.GroupBoxDisassembly = new System.Windows.Forms.GroupBox();
            this.ListDisassembly = new System.Windows.Forms.ListView();
            this.ButtonClearBreakpoint = new System.Windows.Forms.Button();
            this.ButtonSetBreakpoint = new System.Windows.Forms.Button();
            this.ToolStripDebugger = new System.Windows.Forms.ToolStrip();
            this.TSBRun = new System.Windows.Forms.ToolStripButton();
            this.TSBPause = new System.Windows.Forms.ToolStripButton();
            this.TSSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSBStepInto = new System.Windows.Forms.ToolStripButton();
            this.TSBStepOver = new System.Windows.Forms.ToolStripButton();
            this.TSBStepOut = new System.Windows.Forms.ToolStripButton();
            this.TSSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSBAnimate = new System.Windows.Forms.ToolStripButton();
            this.GroupRegisters = new System.Windows.Forms.GroupBox();
            this.TextERP = new System.Windows.Forms.TextBox();
            this.TextESP = new System.Windows.Forms.TextBox();
            this.TextEHX = new System.Windows.Forms.TextBox();
            this.TextEGX = new System.Windows.Forms.TextBox();
            this.TextEFX = new System.Windows.Forms.TextBox();
            this.TextEEX = new System.Windows.Forms.TextBox();
            this.LinkERP = new System.Windows.Forms.LinkLabel();
            this.LinkEFLAGS = new System.Windows.Forms.LinkLabel();
            this.LinkESP = new System.Windows.Forms.LinkLabel();
            this.TextEFLAGS = new System.Windows.Forms.TextBox();
            this.LinkEHX = new System.Windows.Forms.LinkLabel();
            this.TextEIP = new System.Windows.Forms.TextBox();
            this.TextEBP = new System.Windows.Forms.TextBox();
            this.LinkEIP = new System.Windows.Forms.LinkLabel();
            this.LinkEGX = new System.Windows.Forms.LinkLabel();
            this.TextEDX = new System.Windows.Forms.TextBox();
            this.LinkEDX = new System.Windows.Forms.LinkLabel();
            this.TextEAX = new System.Windows.Forms.TextBox();
            this.LinkEFX = new System.Windows.Forms.LinkLabel();
            this.LinkEBP = new System.Windows.Forms.LinkLabel();
            this.LinkEAX = new System.Windows.Forms.LinkLabel();
            this.LinkEEX = new System.Windows.Forms.LinkLabel();
            this.TextEBX = new System.Windows.Forms.TextBox();
            this.LinkECX = new System.Windows.Forms.LinkLabel();
            this.TextECX = new System.Windows.Forms.TextBox();
            this.LinkEBX = new System.Windows.Forms.LinkLabel();
            this.GroupMemory = new System.Windows.Forms.GroupBox();
            this.ComboContexts = new System.Windows.Forms.ComboBox();
            this.HexMemory = new Be.Windows.Forms.HexBox();
            this.GroupCallStackViewer = new System.Windows.Forms.GroupBox();
            this.ListCallStackViewer = new System.Windows.Forms.ListView();
            this.ColumnCalledAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnEBP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TmrQueueListener = new System.Windows.Forms.Timer(this.components);
            this.TimerAnimateExecution = new System.Windows.Forms.Timer(this.components);
            this.TextECC = new System.Windows.Forms.TextBox();
            this.LinkECC = new System.Windows.Forms.LinkLabel();
            this.GroupBoxDisassembly.SuspendLayout();
            this.ToolStripDebugger.SuspendLayout();
            this.GroupRegisters.SuspendLayout();
            this.GroupMemory.SuspendLayout();
            this.GroupCallStackViewer.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBoxDisassembly
            // 
            this.GroupBoxDisassembly.Controls.Add(this.ListDisassembly);
            this.GroupBoxDisassembly.Controls.Add(this.ButtonClearBreakpoint);
            this.GroupBoxDisassembly.Controls.Add(this.ButtonSetBreakpoint);
            this.GroupBoxDisassembly.Location = new System.Drawing.Point(12, 32);
            this.GroupBoxDisassembly.Name = "GroupBoxDisassembly";
            this.GroupBoxDisassembly.Size = new System.Drawing.Size(351, 267);
            this.GroupBoxDisassembly.TabIndex = 0;
            this.GroupBoxDisassembly.TabStop = false;
            this.GroupBoxDisassembly.Text = "Disassembly";
            // 
            // ListDisassembly
            // 
            this.ListDisassembly.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.ListDisassembly.AutoArrange = false;
            this.ListDisassembly.HideSelection = false;
            this.ListDisassembly.Location = new System.Drawing.Point(7, 21);
            this.ListDisassembly.Name = "ListDisassembly";
            this.ListDisassembly.Size = new System.Drawing.Size(344, 213);
            this.ListDisassembly.TabIndex = 5;
            this.ListDisassembly.UseCompatibleStateImageBehavior = false;
            this.ListDisassembly.View = System.Windows.Forms.View.List;
            // 
            // ButtonClearBreakpoint
            // 
            this.ButtonClearBreakpoint.Location = new System.Drawing.Point(134, 240);
            this.ButtonClearBreakpoint.Name = "ButtonClearBreakpoint";
            this.ButtonClearBreakpoint.Size = new System.Drawing.Size(108, 23);
            this.ButtonClearBreakpoint.TabIndex = 4;
            this.ButtonClearBreakpoint.Text = "Clear Breakpoint";
            this.ButtonClearBreakpoint.UseVisualStyleBackColor = true;
            this.ButtonClearBreakpoint.Click += new System.EventHandler(this.ButtonClearBreakpoint_Click);
            // 
            // ButtonSetBreakpoint
            // 
            this.ButtonSetBreakpoint.Location = new System.Drawing.Point(7, 240);
            this.ButtonSetBreakpoint.Name = "ButtonSetBreakpoint";
            this.ButtonSetBreakpoint.Size = new System.Drawing.Size(121, 23);
            this.ButtonSetBreakpoint.TabIndex = 3;
            this.ButtonSetBreakpoint.Text = "Set Breakpoint Here";
            this.ButtonSetBreakpoint.UseVisualStyleBackColor = true;
            this.ButtonSetBreakpoint.Click += new System.EventHandler(this.ButtonSetBreakpoint_Click);
            // 
            // ToolStripDebugger
            // 
            this.ToolStripDebugger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSBRun,
            this.TSBPause,
            this.TSSeparator1,
            this.TSBStepInto,
            this.TSBStepOver,
            this.TSBStepOut,
            this.TSSeparator2,
            this.TSBAnimate});
            this.ToolStripDebugger.Location = new System.Drawing.Point(0, 0);
            this.ToolStripDebugger.Name = "ToolStripDebugger";
            this.ToolStripDebugger.Size = new System.Drawing.Size(764, 25);
            this.ToolStripDebugger.TabIndex = 1;
            this.ToolStripDebugger.Text = "toolStrip1";
            // 
            // TSBRun
            // 
            this.TSBRun.Image = ((System.Drawing.Image)(resources.GetObject("TSBRun.Image")));
            this.TSBRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBRun.Name = "TSBRun";
            this.TSBRun.Size = new System.Drawing.Size(48, 22);
            this.TSBRun.Text = "&Run";
            this.TSBRun.Click += new System.EventHandler(this.TSBRun_Click);
            // 
            // TSBPause
            // 
            this.TSBPause.Image = ((System.Drawing.Image)(resources.GetObject("TSBPause.Image")));
            this.TSBPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBPause.Name = "TSBPause";
            this.TSBPause.Size = new System.Drawing.Size(58, 22);
            this.TSBPause.Text = "&Pause";
            this.TSBPause.Click += new System.EventHandler(this.TSBPause_Click);
            // 
            // TSSeparator1
            // 
            this.TSSeparator1.Name = "TSSeparator1";
            this.TSSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TSBStepInto
            // 
            this.TSBStepInto.Image = ((System.Drawing.Image)(resources.GetObject("TSBStepInto.Image")));
            this.TSBStepInto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBStepInto.Name = "TSBStepInto";
            this.TSBStepInto.Size = new System.Drawing.Size(74, 22);
            this.TSBStepInto.Text = "Step &Into";
            this.TSBStepInto.Click += new System.EventHandler(this.TSBStepInto_Click);
            // 
            // TSBStepOver
            // 
            this.TSBStepOver.Image = ((System.Drawing.Image)(resources.GetObject("TSBStepOver.Image")));
            this.TSBStepOver.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBStepOver.Name = "TSBStepOver";
            this.TSBStepOver.Size = new System.Drawing.Size(78, 22);
            this.TSBStepOver.Text = "Step &Over";
            this.TSBStepOver.Click += new System.EventHandler(this.TSBStepOver_Click);
            // 
            // TSBStepOut
            // 
            this.TSBStepOut.Image = ((System.Drawing.Image)(resources.GetObject("TSBStepOut.Image")));
            this.TSBStepOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBStepOut.Name = "TSBStepOut";
            this.TSBStepOut.Size = new System.Drawing.Size(73, 22);
            this.TSBStepOut.Text = "Step O&ut";
            this.TSBStepOut.Click += new System.EventHandler(this.TSBStepOut_Click);
            // 
            // TSSeparator2
            // 
            this.TSSeparator2.Name = "TSSeparator2";
            this.TSSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // TSBAnimate
            // 
            this.TSBAnimate.Image = ((System.Drawing.Image)(resources.GetObject("TSBAnimate.Image")));
            this.TSBAnimate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBAnimate.Name = "TSBAnimate";
            this.TSBAnimate.Size = new System.Drawing.Size(72, 22);
            this.TSBAnimate.Text = "&Animate";
            this.TSBAnimate.Click += new System.EventHandler(this.TSBAnimate_Click);
            // 
            // GroupRegisters
            // 
            this.GroupRegisters.Controls.Add(this.TextECC);
            this.GroupRegisters.Controls.Add(this.LinkECC);
            this.GroupRegisters.Controls.Add(this.TextERP);
            this.GroupRegisters.Controls.Add(this.TextESP);
            this.GroupRegisters.Controls.Add(this.TextEHX);
            this.GroupRegisters.Controls.Add(this.TextEGX);
            this.GroupRegisters.Controls.Add(this.TextEFX);
            this.GroupRegisters.Controls.Add(this.TextEEX);
            this.GroupRegisters.Controls.Add(this.LinkERP);
            this.GroupRegisters.Controls.Add(this.LinkEFLAGS);
            this.GroupRegisters.Controls.Add(this.LinkESP);
            this.GroupRegisters.Controls.Add(this.TextEFLAGS);
            this.GroupRegisters.Controls.Add(this.LinkEHX);
            this.GroupRegisters.Controls.Add(this.TextEIP);
            this.GroupRegisters.Controls.Add(this.TextEBP);
            this.GroupRegisters.Controls.Add(this.LinkEIP);
            this.GroupRegisters.Controls.Add(this.LinkEGX);
            this.GroupRegisters.Controls.Add(this.TextEDX);
            this.GroupRegisters.Controls.Add(this.LinkEDX);
            this.GroupRegisters.Controls.Add(this.TextEAX);
            this.GroupRegisters.Controls.Add(this.LinkEFX);
            this.GroupRegisters.Controls.Add(this.LinkEBP);
            this.GroupRegisters.Controls.Add(this.LinkEAX);
            this.GroupRegisters.Controls.Add(this.LinkEEX);
            this.GroupRegisters.Controls.Add(this.TextEBX);
            this.GroupRegisters.Controls.Add(this.LinkECX);
            this.GroupRegisters.Controls.Add(this.TextECX);
            this.GroupRegisters.Controls.Add(this.LinkEBX);
            this.GroupRegisters.Location = new System.Drawing.Point(12, 305);
            this.GroupRegisters.Name = "GroupRegisters";
            this.GroupRegisters.Size = new System.Drawing.Size(351, 207);
            this.GroupRegisters.TabIndex = 2;
            this.GroupRegisters.TabStop = false;
            this.GroupRegisters.Text = "Registers";
            // 
            // TextERP
            // 
            this.TextERP.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextERP.Location = new System.Drawing.Point(180, 150);
            this.TextERP.Name = "TextERP";
            this.TextERP.ReadOnly = true;
            this.TextERP.Size = new System.Drawing.Size(104, 20);
            this.TextERP.TabIndex = 28;
            this.TextERP.Text = "0000000000000000";
            // 
            // TextESP
            // 
            this.TextESP.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextESP.Location = new System.Drawing.Point(180, 124);
            this.TextESP.Name = "TextESP";
            this.TextESP.ReadOnly = true;
            this.TextESP.Size = new System.Drawing.Size(104, 20);
            this.TextESP.TabIndex = 27;
            this.TextESP.Text = "0000000000000000";
            // 
            // TextEHX
            // 
            this.TextEHX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEHX.Location = new System.Drawing.Point(180, 98);
            this.TextEHX.Name = "TextEHX";
            this.TextEHX.ReadOnly = true;
            this.TextEHX.Size = new System.Drawing.Size(104, 20);
            this.TextEHX.TabIndex = 26;
            this.TextEHX.Text = "0000000000000000";
            // 
            // TextEGX
            // 
            this.TextEGX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEGX.Location = new System.Drawing.Point(180, 72);
            this.TextEGX.Name = "TextEGX";
            this.TextEGX.ReadOnly = true;
            this.TextEGX.Size = new System.Drawing.Size(104, 20);
            this.TextEGX.TabIndex = 25;
            this.TextEGX.Text = "0000000000000000";
            // 
            // TextEFX
            // 
            this.TextEFX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEFX.Location = new System.Drawing.Point(180, 46);
            this.TextEFX.Name = "TextEFX";
            this.TextEFX.ReadOnly = true;
            this.TextEFX.Size = new System.Drawing.Size(104, 20);
            this.TextEFX.TabIndex = 24;
            this.TextEFX.Text = "0000000000000000";
            // 
            // TextEEX
            // 
            this.TextEEX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEEX.Location = new System.Drawing.Point(180, 20);
            this.TextEEX.Name = "TextEEX";
            this.TextEEX.ReadOnly = true;
            this.TextEEX.Size = new System.Drawing.Size(104, 20);
            this.TextEEX.TabIndex = 23;
            this.TextEEX.Text = "0000000000000000";
            // 
            // LinkERP
            // 
            this.LinkERP.AutoSize = true;
            this.LinkERP.Location = new System.Drawing.Point(149, 152);
            this.LinkERP.Name = "LinkERP";
            this.LinkERP.Size = new System.Drawing.Size(26, 13);
            this.LinkERP.TabIndex = 21;
            this.LinkERP.TabStop = true;
            this.LinkERP.Text = "ERP";
            // 
            // LinkEFLAGS
            // 
            this.LinkEFLAGS.AutoSize = true;
            this.LinkEFLAGS.Location = new System.Drawing.Point(7, 178);
            this.LinkEFLAGS.Name = "LinkEFLAGS";
            this.LinkEFLAGS.Size = new System.Drawing.Size(45, 13);
            this.LinkEFLAGS.TabIndex = 22;
            this.LinkEFLAGS.TabStop = true;
            this.LinkEFLAGS.Text = "EFLAGS";
            // 
            // LinkESP
            // 
            this.LinkESP.AutoSize = true;
            this.LinkESP.Location = new System.Drawing.Point(149, 126);
            this.LinkESP.Name = "LinkESP";
            this.LinkESP.Size = new System.Drawing.Size(25, 13);
            this.LinkESP.TabIndex = 19;
            this.LinkESP.TabStop = true;
            this.LinkESP.Text = "ESP";
            // 
            // TextEFLAGS
            // 
            this.TextEFLAGS.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEFLAGS.Location = new System.Drawing.Point(58, 176);
            this.TextEFLAGS.Name = "TextEFLAGS";
            this.TextEFLAGS.ReadOnly = true;
            this.TextEFLAGS.Size = new System.Drawing.Size(104, 20);
            this.TextEFLAGS.TabIndex = 21;
            this.TextEFLAGS.Text = "0000000000000000";
            // 
            // LinkEHX
            // 
            this.LinkEHX.AutoSize = true;
            this.LinkEHX.Location = new System.Drawing.Point(149, 100);
            this.LinkEHX.Name = "LinkEHX";
            this.LinkEHX.Size = new System.Drawing.Size(27, 13);
            this.LinkEHX.TabIndex = 16;
            this.LinkEHX.TabStop = true;
            this.LinkEHX.Text = "EHX";
            // 
            // TextEIP
            // 
            this.TextEIP.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEIP.Location = new System.Drawing.Point(39, 150);
            this.TextEIP.Name = "TextEIP";
            this.TextEIP.ReadOnly = true;
            this.TextEIP.Size = new System.Drawing.Size(104, 20);
            this.TextEIP.TabIndex = 19;
            this.TextEIP.Text = "0000000000000000";
            // 
            // TextEBP
            // 
            this.TextEBP.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEBP.Location = new System.Drawing.Point(39, 124);
            this.TextEBP.Name = "TextEBP";
            this.TextEBP.ReadOnly = true;
            this.TextEBP.Size = new System.Drawing.Size(104, 20);
            this.TextEBP.TabIndex = 8;
            this.TextEBP.Text = "0000000000000000";
            // 
            // LinkEIP
            // 
            this.LinkEIP.AutoSize = true;
            this.LinkEIP.Location = new System.Drawing.Point(11, 152);
            this.LinkEIP.Name = "LinkEIP";
            this.LinkEIP.Size = new System.Drawing.Size(22, 13);
            this.LinkEIP.TabIndex = 20;
            this.LinkEIP.TabStop = true;
            this.LinkEIP.Text = "EIP";
            // 
            // LinkEGX
            // 
            this.LinkEGX.AutoSize = true;
            this.LinkEGX.Location = new System.Drawing.Point(149, 74);
            this.LinkEGX.Name = "LinkEGX";
            this.LinkEGX.Size = new System.Drawing.Size(27, 13);
            this.LinkEGX.TabIndex = 14;
            this.LinkEGX.TabStop = true;
            this.LinkEGX.Text = "EGX";
            // 
            // TextEDX
            // 
            this.TextEDX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEDX.Location = new System.Drawing.Point(39, 98);
            this.TextEDX.Name = "TextEDX";
            this.TextEDX.ReadOnly = true;
            this.TextEDX.Size = new System.Drawing.Size(104, 20);
            this.TextEDX.TabIndex = 7;
            this.TextEDX.Text = "0000000000000000";
            // 
            // LinkEDX
            // 
            this.LinkEDX.AutoSize = true;
            this.LinkEDX.Location = new System.Drawing.Point(7, 100);
            this.LinkEDX.Name = "LinkEDX";
            this.LinkEDX.Size = new System.Drawing.Size(27, 13);
            this.LinkEDX.TabIndex = 6;
            this.LinkEDX.TabStop = true;
            this.LinkEDX.Text = "EDX";
            // 
            // TextEAX
            // 
            this.TextEAX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEAX.Location = new System.Drawing.Point(39, 20);
            this.TextEAX.Name = "TextEAX";
            this.TextEAX.ReadOnly = true;
            this.TextEAX.Size = new System.Drawing.Size(104, 20);
            this.TextEAX.TabIndex = 1;
            this.TextEAX.Text = "0000000000000000";
            // 
            // LinkEFX
            // 
            this.LinkEFX.AutoSize = true;
            this.LinkEFX.Location = new System.Drawing.Point(149, 48);
            this.LinkEFX.Name = "LinkEFX";
            this.LinkEFX.Size = new System.Drawing.Size(25, 13);
            this.LinkEFX.TabIndex = 12;
            this.LinkEFX.TabStop = true;
            this.LinkEFX.Text = "EFX";
            // 
            // LinkEBP
            // 
            this.LinkEBP.AutoSize = true;
            this.LinkEBP.Location = new System.Drawing.Point(7, 126);
            this.LinkEBP.Name = "LinkEBP";
            this.LinkEBP.Size = new System.Drawing.Size(25, 13);
            this.LinkEBP.TabIndex = 18;
            this.LinkEBP.TabStop = true;
            this.LinkEBP.Text = "EBP";
            // 
            // LinkEAX
            // 
            this.LinkEAX.AutoSize = true;
            this.LinkEAX.Location = new System.Drawing.Point(7, 22);
            this.LinkEAX.Name = "LinkEAX";
            this.LinkEAX.Size = new System.Drawing.Size(26, 13);
            this.LinkEAX.TabIndex = 0;
            this.LinkEAX.TabStop = true;
            this.LinkEAX.Text = "EAX";
            // 
            // LinkEEX
            // 
            this.LinkEEX.AutoSize = true;
            this.LinkEEX.Location = new System.Drawing.Point(149, 22);
            this.LinkEEX.Name = "LinkEEX";
            this.LinkEEX.Size = new System.Drawing.Size(25, 13);
            this.LinkEEX.TabIndex = 10;
            this.LinkEEX.TabStop = true;
            this.LinkEEX.Text = "EEX";
            // 
            // TextEBX
            // 
            this.TextEBX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEBX.Location = new System.Drawing.Point(39, 46);
            this.TextEBX.Name = "TextEBX";
            this.TextEBX.ReadOnly = true;
            this.TextEBX.Size = new System.Drawing.Size(104, 20);
            this.TextEBX.TabIndex = 3;
            this.TextEBX.Text = "0000000000000000";
            // 
            // LinkECX
            // 
            this.LinkECX.AutoSize = true;
            this.LinkECX.Location = new System.Drawing.Point(7, 74);
            this.LinkECX.Name = "LinkECX";
            this.LinkECX.Size = new System.Drawing.Size(26, 13);
            this.LinkECX.TabIndex = 4;
            this.LinkECX.TabStop = true;
            this.LinkECX.Text = "ECX";
            // 
            // TextECX
            // 
            this.TextECX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextECX.Location = new System.Drawing.Point(39, 72);
            this.TextECX.Name = "TextECX";
            this.TextECX.ReadOnly = true;
            this.TextECX.Size = new System.Drawing.Size(104, 20);
            this.TextECX.TabIndex = 5;
            this.TextECX.Text = "0000000000000000";
            // 
            // LinkEBX
            // 
            this.LinkEBX.AutoSize = true;
            this.LinkEBX.Location = new System.Drawing.Point(7, 48);
            this.LinkEBX.Name = "LinkEBX";
            this.LinkEBX.Size = new System.Drawing.Size(25, 13);
            this.LinkEBX.TabIndex = 2;
            this.LinkEBX.TabStop = true;
            this.LinkEBX.Text = "EBX";
            // 
            // GroupMemory
            // 
            this.GroupMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupMemory.Controls.Add(this.ComboContexts);
            this.GroupMemory.Controls.Add(this.HexMemory);
            this.GroupMemory.Location = new System.Drawing.Point(369, 32);
            this.GroupMemory.Name = "GroupMemory";
            this.GroupMemory.Size = new System.Drawing.Size(383, 267);
            this.GroupMemory.TabIndex = 3;
            this.GroupMemory.TabStop = false;
            this.GroupMemory.Text = "Memory";
            // 
            // ComboContexts
            // 
            this.ComboContexts.FormattingEnabled = true;
            this.ComboContexts.Location = new System.Drawing.Point(10, 21);
            this.ComboContexts.Name = "ComboContexts";
            this.ComboContexts.Size = new System.Drawing.Size(363, 21);
            this.ComboContexts.TabIndex = 1;
            this.ComboContexts.SelectedIndexChanged += new System.EventHandler(this.ComboMemorySpace_SelectedIndexChanged);
            // 
            // HexMemory
            // 
            this.HexMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HexMemory.BytesPerLine = 8;
            this.HexMemory.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HexMemory.LineInfoVisible = true;
            this.HexMemory.Location = new System.Drawing.Point(10, 48);
            this.HexMemory.Name = "HexMemory";
            this.HexMemory.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.HexMemory.Size = new System.Drawing.Size(363, 213);
            this.HexMemory.StringViewVisible = true;
            this.HexMemory.TabIndex = 0;
            this.HexMemory.UseFixedBytesPerLine = true;
            this.HexMemory.VScrollBarVisible = true;
            // 
            // GroupCallStackViewer
            // 
            this.GroupCallStackViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupCallStackViewer.Controls.Add(this.ListCallStackViewer);
            this.GroupCallStackViewer.Location = new System.Drawing.Point(369, 305);
            this.GroupCallStackViewer.Name = "GroupCallStackViewer";
            this.GroupCallStackViewer.Size = new System.Drawing.Size(383, 207);
            this.GroupCallStackViewer.TabIndex = 4;
            this.GroupCallStackViewer.TabStop = false;
            this.GroupCallStackViewer.Text = "Call Stack";
            // 
            // ListCallStackViewer
            // 
            this.ListCallStackViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListCallStackViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnCalledAddress,
            this.ColumnEBP});
            this.ListCallStackViewer.HideSelection = false;
            this.ListCallStackViewer.Location = new System.Drawing.Point(10, 22);
            this.ListCallStackViewer.Name = "ListCallStackViewer";
            this.ListCallStackViewer.Size = new System.Drawing.Size(367, 174);
            this.ListCallStackViewer.TabIndex = 0;
            this.ListCallStackViewer.UseCompatibleStateImageBehavior = false;
            this.ListCallStackViewer.View = System.Windows.Forms.View.Details;
            // 
            // ColumnCalledAddress
            // 
            this.ColumnCalledAddress.Text = "Called Address";
            // 
            // ColumnEBP
            // 
            this.ColumnEBP.Text = "Stack Base";
            // 
            // TmrQueueListener
            // 
            this.TmrQueueListener.Enabled = true;
            this.TmrQueueListener.Tick += new System.EventHandler(this.TmrQueueListener_Tick);
            // 
            // TimerAnimateExecution
            // 
            this.TimerAnimateExecution.Tick += new System.EventHandler(this.TimerAnimateExecution_Tick);
            // 
            // TextECC
            // 
            this.TextECC.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextECC.Location = new System.Drawing.Point(199, 176);
            this.TextECC.Name = "TextECC";
            this.TextECC.ReadOnly = true;
            this.TextECC.Size = new System.Drawing.Size(56, 20);
            this.TextECC.TabIndex = 30;
            this.TextECC.Text = "00000000";
            // 
            // LinkECC
            // 
            this.LinkECC.AutoSize = true;
            this.LinkECC.Location = new System.Drawing.Point(168, 178);
            this.LinkECC.Name = "LinkECC";
            this.LinkECC.Size = new System.Drawing.Size(27, 13);
            this.LinkECC.TabIndex = 29;
            this.LinkECC.TabStop = true;
            this.LinkECC.Text = "ECC";
            // 
            // DebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 520);
            this.Controls.Add(this.GroupCallStackViewer);
            this.Controls.Add(this.GroupMemory);
            this.Controls.Add(this.GroupRegisters);
            this.Controls.Add(this.ToolStripDebugger);
            this.Controls.Add(this.GroupBoxDisassembly);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DebuggerForm";
            this.ShowInTaskbar = false;
            this.Text = "VM Debugger";
            this.Load += new System.EventHandler(this.DebuggerForm_Load);
            this.GroupBoxDisassembly.ResumeLayout(false);
            this.ToolStripDebugger.ResumeLayout(false);
            this.ToolStripDebugger.PerformLayout();
            this.GroupRegisters.ResumeLayout(false);
            this.GroupRegisters.PerformLayout();
            this.GroupMemory.ResumeLayout(false);
            this.GroupCallStackViewer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox GroupBoxDisassembly;
		private System.Windows.Forms.ToolStrip ToolStripDebugger;
		private System.Windows.Forms.ToolStripButton TSBRun;
		private System.Windows.Forms.ToolStripButton TSBPause;
		private System.Windows.Forms.ToolStripSeparator TSSeparator1;
		private System.Windows.Forms.ToolStripButton TSBStepInto;
		private System.Windows.Forms.ToolStripButton TSBStepOver;
		private System.Windows.Forms.ToolStripButton TSBStepOut;
		private System.Windows.Forms.ToolStripSeparator TSSeparator2;
		private System.Windows.Forms.ToolStripButton TSBAnimate;
		private System.Windows.Forms.GroupBox GroupRegisters;
		private System.Windows.Forms.LinkLabel LinkEHX;
		private System.Windows.Forms.LinkLabel LinkEGX;
		private System.Windows.Forms.LinkLabel LinkEFX;
		private System.Windows.Forms.LinkLabel LinkEEX;
		private System.Windows.Forms.TextBox TextEDX;
		private System.Windows.Forms.LinkLabel LinkEDX;
		private System.Windows.Forms.TextBox TextECX;
		private System.Windows.Forms.LinkLabel LinkECX;
		private System.Windows.Forms.TextBox TextEBX;
		private System.Windows.Forms.LinkLabel LinkEBX;
		private System.Windows.Forms.TextBox TextEAX;
		private System.Windows.Forms.LinkLabel LinkEAX;
		private System.Windows.Forms.GroupBox GroupMemory;
		private System.Windows.Forms.GroupBox GroupCallStackViewer;
		private System.Windows.Forms.ListView ListCallStackViewer;
		private System.Windows.Forms.ColumnHeader ColumnCalledAddress;
		private System.Windows.Forms.ColumnHeader ColumnEBP;
		private System.Windows.Forms.TextBox TextERP;
		private System.Windows.Forms.TextBox TextESP;
		private System.Windows.Forms.TextBox TextEHX;
		private System.Windows.Forms.TextBox TextEGX;
		private System.Windows.Forms.TextBox TextEFX;
		private System.Windows.Forms.TextBox TextEEX;
		private System.Windows.Forms.LinkLabel LinkERP;
		private System.Windows.Forms.LinkLabel LinkEFLAGS;
		private System.Windows.Forms.LinkLabel LinkESP;
		private System.Windows.Forms.TextBox TextEFLAGS;
		private System.Windows.Forms.TextBox TextEIP;
		private System.Windows.Forms.TextBox TextEBP;
		private System.Windows.Forms.LinkLabel LinkEIP;
		private System.Windows.Forms.LinkLabel LinkEBP;
		private Be.Windows.Forms.HexBox HexMemory;
		private System.Windows.Forms.Button ButtonClearBreakpoint;
		private System.Windows.Forms.Button ButtonSetBreakpoint;
		private System.Windows.Forms.ListView ListDisassembly;
		private System.Windows.Forms.Timer TmrQueueListener;
		private System.Windows.Forms.Timer TimerAnimateExecution;
		private System.Windows.Forms.ComboBox ComboContexts;
        private System.Windows.Forms.TextBox TextECC;
        private System.Windows.Forms.LinkLabel LinkECC;
    }
}