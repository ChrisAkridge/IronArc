namespace IronArcHost
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebuggerForm));
			this.GroupBoxDisassembly = new System.Windows.Forms.GroupBox();
			this.ListDisassembly = new System.Windows.Forms.ListBox();
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
			this.textBox6 = new System.Windows.Forms.TextBox();
			this.LinkEBASE = new System.Windows.Forms.LinkLabel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.LinkEFLAGS = new System.Windows.Forms.LinkLabel();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.LinkEHX = new System.Windows.Forms.LinkLabel();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.LinkEGX = new System.Windows.Forms.LinkLabel();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.LinkEFX = new System.Windows.Forms.LinkLabel();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.LinkEEX = new System.Windows.Forms.LinkLabel();
			this.TextIP = new System.Windows.Forms.TextBox();
			this.LinkIP = new System.Windows.Forms.LinkLabel();
			this.TextEDX = new System.Windows.Forms.TextBox();
			this.LinkEDX = new System.Windows.Forms.LinkLabel();
			this.TextECX = new System.Windows.Forms.TextBox();
			this.LinkECX = new System.Windows.Forms.LinkLabel();
			this.TextEBX = new System.Windows.Forms.TextBox();
			this.LinkEBX = new System.Windows.Forms.LinkLabel();
			this.TextEAX = new System.Windows.Forms.TextBox();
			this.LinkLabelEAX = new System.Windows.Forms.LinkLabel();
			this.GroupMemory = new System.Windows.Forms.GroupBox();
			this.HexMemoryViewer = new HexControlLibrary.HexControl();
			this.ComboMemorySpaces = new System.Windows.Forms.ComboBox();
			this.StaticLabelMemorySpace = new System.Windows.Forms.Label();
			this.GroupStackViewer = new System.Windows.Forms.GroupBox();
			this.ListStackViewer = new System.Windows.Forms.ListView();
			this.ColumnAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ColumnElementSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ColumnValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.GroupBoxDisassembly.SuspendLayout();
			this.ToolStripDebugger.SuspendLayout();
			this.GroupRegisters.SuspendLayout();
			this.GroupMemory.SuspendLayout();
			this.GroupStackViewer.SuspendLayout();
			this.SuspendLayout();
			// 
			// GroupBoxDisassembly
			// 
			this.GroupBoxDisassembly.Controls.Add(this.ListDisassembly);
			this.GroupBoxDisassembly.Location = new System.Drawing.Point(12, 32);
			this.GroupBoxDisassembly.Name = "GroupBoxDisassembly";
			this.GroupBoxDisassembly.Size = new System.Drawing.Size(315, 267);
			this.GroupBoxDisassembly.TabIndex = 0;
			this.GroupBoxDisassembly.TabStop = false;
			this.GroupBoxDisassembly.Text = "Disassembly (Main Program)";
			// 
			// ListDisassembly
			// 
			this.ListDisassembly.FormattingEnabled = true;
			this.ListDisassembly.Location = new System.Drawing.Point(7, 22);
			this.ListDisassembly.Name = "ListDisassembly";
			this.ListDisassembly.Size = new System.Drawing.Size(302, 238);
			this.ListDisassembly.TabIndex = 0;
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
			this.ToolStripDebugger.Size = new System.Drawing.Size(728, 25);
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
			// 
			// TSBPause
			// 
			this.TSBPause.Image = ((System.Drawing.Image)(resources.GetObject("TSBPause.Image")));
			this.TSBPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBPause.Name = "TSBPause";
			this.TSBPause.Size = new System.Drawing.Size(58, 22);
			this.TSBPause.Text = "&Pause";
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
			// 
			// TSBStepOver
			// 
			this.TSBStepOver.Image = ((System.Drawing.Image)(resources.GetObject("TSBStepOver.Image")));
			this.TSBStepOver.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBStepOver.Name = "TSBStepOver";
			this.TSBStepOver.Size = new System.Drawing.Size(78, 22);
			this.TSBStepOver.Text = "Step &Over";
			// 
			// TSBStepOut
			// 
			this.TSBStepOut.Image = ((System.Drawing.Image)(resources.GetObject("TSBStepOut.Image")));
			this.TSBStepOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSBStepOut.Name = "TSBStepOut";
			this.TSBStepOut.Size = new System.Drawing.Size(73, 22);
			this.TSBStepOut.Text = "Step O&ut";
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
			// 
			// GroupRegisters
			// 
			this.GroupRegisters.Controls.Add(this.textBox6);
			this.GroupRegisters.Controls.Add(this.LinkEBASE);
			this.GroupRegisters.Controls.Add(this.textBox1);
			this.GroupRegisters.Controls.Add(this.LinkEFLAGS);
			this.GroupRegisters.Controls.Add(this.textBox2);
			this.GroupRegisters.Controls.Add(this.LinkEHX);
			this.GroupRegisters.Controls.Add(this.textBox3);
			this.GroupRegisters.Controls.Add(this.LinkEGX);
			this.GroupRegisters.Controls.Add(this.textBox4);
			this.GroupRegisters.Controls.Add(this.LinkEFX);
			this.GroupRegisters.Controls.Add(this.textBox5);
			this.GroupRegisters.Controls.Add(this.LinkEEX);
			this.GroupRegisters.Controls.Add(this.TextIP);
			this.GroupRegisters.Controls.Add(this.LinkIP);
			this.GroupRegisters.Controls.Add(this.TextEDX);
			this.GroupRegisters.Controls.Add(this.LinkEDX);
			this.GroupRegisters.Controls.Add(this.TextECX);
			this.GroupRegisters.Controls.Add(this.LinkECX);
			this.GroupRegisters.Controls.Add(this.TextEBX);
			this.GroupRegisters.Controls.Add(this.LinkEBX);
			this.GroupRegisters.Controls.Add(this.TextEAX);
			this.GroupRegisters.Controls.Add(this.LinkLabelEAX);
			this.GroupRegisters.Location = new System.Drawing.Point(12, 305);
			this.GroupRegisters.Name = "GroupRegisters";
			this.GroupRegisters.Size = new System.Drawing.Size(309, 165);
			this.GroupRegisters.TabIndex = 2;
			this.GroupRegisters.TabStop = false;
			this.GroupRegisters.Text = "Registers";
			// 
			// textBox6
			// 
			this.textBox6.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox6.Location = new System.Drawing.Point(69, 139);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new System.Drawing.Size(56, 20);
			this.textBox6.TabIndex = 21;
			this.textBox6.Text = "00000000";
			// 
			// LinkEBASE
			// 
			this.LinkEBASE.AutoSize = true;
			this.LinkEBASE.Location = new System.Drawing.Point(30, 141);
			this.LinkEBASE.Name = "LinkEBASE";
			this.LinkEBASE.Size = new System.Drawing.Size(39, 13);
			this.LinkEBASE.TabIndex = 20;
			this.LinkEBASE.TabStop = true;
			this.LinkEBASE.Text = "EBASE";
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(182, 115);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(103, 20);
			this.textBox1.TabIndex = 19;
			this.textBox1.Text = "0000000000000000";
			// 
			// LinkEFLAGS
			// 
			this.LinkEFLAGS.AutoSize = true;
			this.LinkEFLAGS.Location = new System.Drawing.Point(131, 117);
			this.LinkEFLAGS.Name = "LinkEFLAGS";
			this.LinkEFLAGS.Size = new System.Drawing.Size(45, 13);
			this.LinkEFLAGS.TabIndex = 18;
			this.LinkEFLAGS.TabStop = true;
			this.LinkEFLAGS.Text = "EFLAGS";
			// 
			// textBox2
			// 
			this.textBox2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox2.Location = new System.Drawing.Point(182, 91);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(103, 20);
			this.textBox2.TabIndex = 17;
			this.textBox2.Text = "0000000000000000";
			// 
			// LinkEHX
			// 
			this.LinkEHX.AutoSize = true;
			this.LinkEHX.Location = new System.Drawing.Point(149, 91);
			this.LinkEHX.Name = "LinkEHX";
			this.LinkEHX.Size = new System.Drawing.Size(27, 13);
			this.LinkEHX.TabIndex = 16;
			this.LinkEHX.TabStop = true;
			this.LinkEHX.Text = "EHX";
			// 
			// textBox3
			// 
			this.textBox3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox3.Location = new System.Drawing.Point(180, 67);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(105, 20);
			this.textBox3.TabIndex = 15;
			this.textBox3.Text = "0000000000000000";
			// 
			// LinkEGX
			// 
			this.LinkEGX.AutoSize = true;
			this.LinkEGX.Location = new System.Drawing.Point(149, 67);
			this.LinkEGX.Name = "LinkEGX";
			this.LinkEGX.Size = new System.Drawing.Size(27, 13);
			this.LinkEGX.TabIndex = 14;
			this.LinkEGX.TabStop = true;
			this.LinkEGX.Text = "EGX";
			// 
			// textBox4
			// 
			this.textBox4.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox4.Location = new System.Drawing.Point(180, 43);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(105, 20);
			this.textBox4.TabIndex = 13;
			this.textBox4.Text = "0000000000000000";
			// 
			// LinkEFX
			// 
			this.LinkEFX.AutoSize = true;
			this.LinkEFX.Location = new System.Drawing.Point(149, 46);
			this.LinkEFX.Name = "LinkEFX";
			this.LinkEFX.Size = new System.Drawing.Size(25, 13);
			this.LinkEFX.TabIndex = 12;
			this.LinkEFX.TabStop = true;
			this.LinkEFX.Text = "EFX";
			// 
			// textBox5
			// 
			this.textBox5.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox5.Location = new System.Drawing.Point(180, 19);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(105, 20);
			this.textBox5.TabIndex = 11;
			this.textBox5.Text = "0000000000000000";
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
			// TextIP
			// 
			this.TextIP.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextIP.Location = new System.Drawing.Point(69, 115);
			this.TextIP.Name = "TextIP";
			this.TextIP.Size = new System.Drawing.Size(56, 20);
			this.TextIP.TabIndex = 9;
			this.TextIP.Text = "00000000";
			// 
			// LinkIP
			// 
			this.LinkIP.AutoSize = true;
			this.LinkIP.Location = new System.Drawing.Point(47, 115);
			this.LinkIP.Name = "LinkIP";
			this.LinkIP.Size = new System.Drawing.Size(16, 13);
			this.LinkIP.TabIndex = 8;
			this.LinkIP.TabStop = true;
			this.LinkIP.Text = "IP";
			// 
			// TextEDX
			// 
			this.TextEDX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextEDX.Location = new System.Drawing.Point(39, 91);
			this.TextEDX.Name = "TextEDX";
			this.TextEDX.Size = new System.Drawing.Size(104, 20);
			this.TextEDX.TabIndex = 7;
			this.TextEDX.Text = "0000000000000000";
			// 
			// LinkEDX
			// 
			this.LinkEDX.AutoSize = true;
			this.LinkEDX.Location = new System.Drawing.Point(7, 91);
			this.LinkEDX.Name = "LinkEDX";
			this.LinkEDX.Size = new System.Drawing.Size(27, 13);
			this.LinkEDX.TabIndex = 6;
			this.LinkEDX.TabStop = true;
			this.LinkEDX.Text = "EDX";
			// 
			// TextECX
			// 
			this.TextECX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextECX.Location = new System.Drawing.Point(39, 67);
			this.TextECX.Name = "TextECX";
			this.TextECX.Size = new System.Drawing.Size(104, 20);
			this.TextECX.TabIndex = 5;
			this.TextECX.Text = "0000000000000000";
			// 
			// LinkECX
			// 
			this.LinkECX.AutoSize = true;
			this.LinkECX.Location = new System.Drawing.Point(7, 67);
			this.LinkECX.Name = "LinkECX";
			this.LinkECX.Size = new System.Drawing.Size(26, 13);
			this.LinkECX.TabIndex = 4;
			this.LinkECX.TabStop = true;
			this.LinkECX.Text = "ECX";
			// 
			// TextEBX
			// 
			this.TextEBX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextEBX.Location = new System.Drawing.Point(39, 43);
			this.TextEBX.Name = "TextEBX";
			this.TextEBX.Size = new System.Drawing.Size(104, 20);
			this.TextEBX.TabIndex = 3;
			this.TextEBX.Text = "0000000000000000";
			// 
			// LinkEBX
			// 
			this.LinkEBX.AutoSize = true;
			this.LinkEBX.Location = new System.Drawing.Point(7, 46);
			this.LinkEBX.Name = "LinkEBX";
			this.LinkEBX.Size = new System.Drawing.Size(26, 13);
			this.LinkEBX.TabIndex = 2;
			this.LinkEBX.TabStop = true;
			this.LinkEBX.Text = "EBX";
			// 
			// TextEAX
			// 
			this.TextEAX.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextEAX.Location = new System.Drawing.Point(39, 19);
			this.TextEAX.Name = "TextEAX";
			this.TextEAX.Size = new System.Drawing.Size(104, 20);
			this.TextEAX.TabIndex = 1;
			this.TextEAX.Text = "0000000000000000";
			// 
			// LinkLabelEAX
			// 
			this.LinkLabelEAX.AutoSize = true;
			this.LinkLabelEAX.Location = new System.Drawing.Point(7, 22);
			this.LinkLabelEAX.Name = "LinkLabelEAX";
			this.LinkLabelEAX.Size = new System.Drawing.Size(26, 13);
			this.LinkLabelEAX.TabIndex = 0;
			this.LinkLabelEAX.TabStop = true;
			this.LinkLabelEAX.Text = "EAX";
			// 
			// GroupMemory
			// 
			this.GroupMemory.Controls.Add(this.HexMemoryViewer);
			this.GroupMemory.Controls.Add(this.ComboMemorySpaces);
			this.GroupMemory.Controls.Add(this.StaticLabelMemorySpace);
			this.GroupMemory.Location = new System.Drawing.Point(333, 32);
			this.GroupMemory.Name = "GroupMemory";
			this.GroupMemory.Size = new System.Drawing.Size(383, 267);
			this.GroupMemory.TabIndex = 3;
			this.GroupMemory.TabStop = false;
			this.GroupMemory.Text = "Memory";
			// 
			// HexMemoryViewer
			// 
			this.HexMemoryViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.HexMemoryViewer.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HexMemoryViewer.Location = new System.Drawing.Point(10, 51);
			this.HexMemoryViewer.Name = "HexMemoryViewer";
			this.HexMemoryViewer.Size = new System.Drawing.Size(367, 209);
			this.HexMemoryViewer.TabIndex = 2;
			// 
			// ComboMemorySpaces
			// 
			this.ComboMemorySpaces.FormattingEnabled = true;
			this.ComboMemorySpaces.Items.AddRange(new object[] {
            "Memory",
            "Stack",
            "System Program"});
			this.ComboMemorySpaces.Location = new System.Drawing.Point(97, 19);
			this.ComboMemorySpaces.Name = "ComboMemorySpaces";
			this.ComboMemorySpaces.Size = new System.Drawing.Size(280, 21);
			this.ComboMemorySpaces.TabIndex = 1;
			// 
			// StaticLabelMemorySpace
			// 
			this.StaticLabelMemorySpace.AutoSize = true;
			this.StaticLabelMemorySpace.Location = new System.Drawing.Point(7, 22);
			this.StaticLabelMemorySpace.Name = "StaticLabelMemorySpace";
			this.StaticLabelMemorySpace.Size = new System.Drawing.Size(84, 13);
			this.StaticLabelMemorySpace.TabIndex = 0;
			this.StaticLabelMemorySpace.Text = "Memory Space:";
			// 
			// GroupStackViewer
			// 
			this.GroupStackViewer.Controls.Add(this.ListStackViewer);
			this.GroupStackViewer.Location = new System.Drawing.Point(333, 305);
			this.GroupStackViewer.Name = "GroupStackViewer";
			this.GroupStackViewer.Size = new System.Drawing.Size(383, 165);
			this.GroupStackViewer.TabIndex = 4;
			this.GroupStackViewer.TabStop = false;
			this.GroupStackViewer.Text = "Stack";
			// 
			// ListStackViewer
			// 
			this.ListStackViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnAddress,
            this.ColumnElementSize,
            this.ColumnValue});
			this.ListStackViewer.Location = new System.Drawing.Point(10, 22);
			this.ListStackViewer.Name = "ListStackViewer";
			this.ListStackViewer.Size = new System.Drawing.Size(367, 132);
			this.ListStackViewer.TabIndex = 0;
			this.ListStackViewer.UseCompatibleStateImageBehavior = false;
			this.ListStackViewer.View = System.Windows.Forms.View.Details;
			// 
			// ColumnAddress
			// 
			this.ColumnAddress.Text = "Address";
			// 
			// ColumnElementSize
			// 
			this.ColumnElementSize.Text = "Size";
			// 
			// ColumnValue
			// 
			this.ColumnValue.Text = "Value";
			// 
			// DebuggerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(728, 482);
			this.Controls.Add(this.GroupStackViewer);
			this.Controls.Add(this.GroupMemory);
			this.Controls.Add(this.GroupRegisters);
			this.Controls.Add(this.ToolStripDebugger);
			this.Controls.Add(this.GroupBoxDisassembly);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "DebuggerForm";
			this.ShowInTaskbar = false;
			this.Text = "VM Debugger";
			this.GroupBoxDisassembly.ResumeLayout(false);
			this.ToolStripDebugger.ResumeLayout(false);
			this.ToolStripDebugger.PerformLayout();
			this.GroupRegisters.ResumeLayout(false);
			this.GroupRegisters.PerformLayout();
			this.GroupMemory.ResumeLayout(false);
			this.GroupMemory.PerformLayout();
			this.GroupStackViewer.ResumeLayout(false);
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
		private System.Windows.Forms.ListBox ListDisassembly;
		private System.Windows.Forms.GroupBox GroupRegisters;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.LinkLabel LinkEBASE;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.LinkLabel LinkEFLAGS;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.LinkLabel LinkEHX;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.LinkLabel LinkEGX;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.LinkLabel LinkEFX;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.LinkLabel LinkEEX;
		private System.Windows.Forms.TextBox TextIP;
		private System.Windows.Forms.LinkLabel LinkIP;
		private System.Windows.Forms.TextBox TextEDX;
		private System.Windows.Forms.LinkLabel LinkEDX;
		private System.Windows.Forms.TextBox TextECX;
		private System.Windows.Forms.LinkLabel LinkECX;
		private System.Windows.Forms.TextBox TextEBX;
		private System.Windows.Forms.LinkLabel LinkEBX;
		private System.Windows.Forms.TextBox TextEAX;
		private System.Windows.Forms.LinkLabel LinkLabelEAX;
		private System.Windows.Forms.GroupBox GroupMemory;
		private HexControlLibrary.HexControl HexMemoryViewer;
		private System.Windows.Forms.ComboBox ComboMemorySpaces;
		private System.Windows.Forms.Label StaticLabelMemorySpace;
		private System.Windows.Forms.GroupBox GroupStackViewer;
		private System.Windows.Forms.ListView ListStackViewer;
		private System.Windows.Forms.ColumnHeader ColumnAddress;
		private System.Windows.Forms.ColumnHeader ColumnElementSize;
		private System.Windows.Forms.ColumnHeader ColumnValue;
	}
}