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
			this.ToolStripDebugger = new System.Windows.Forms.ToolStrip();
			this.TSBRun = new System.Windows.Forms.ToolStripButton();
			this.TSBPause = new System.Windows.Forms.ToolStripButton();
			this.TSSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.TSBStepInto = new System.Windows.Forms.ToolStripButton();
			this.TSBStepOver = new System.Windows.Forms.ToolStripButton();
			this.TSBStepOut = new System.Windows.Forms.ToolStripButton();
			this.TSSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.TSBAnimate = new System.Windows.Forms.ToolStripButton();
			this.ToolStripDebugger.SuspendLayout();
			this.SuspendLayout();
			// 
			// GroupBoxDisassembly
			// 
			this.GroupBoxDisassembly.Location = new System.Drawing.Point(12, 32);
			this.GroupBoxDisassembly.Name = "GroupBoxDisassembly";
			this.GroupBoxDisassembly.Size = new System.Drawing.Size(315, 287);
			this.GroupBoxDisassembly.TabIndex = 0;
			this.GroupBoxDisassembly.TabStop = false;
			this.GroupBoxDisassembly.Text = "Disassembly";
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
			// DebuggerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(728, 482);
			this.Controls.Add(this.ToolStripDebugger);
			this.Controls.Add(this.GroupBoxDisassembly);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "DebuggerForm";
			this.ShowInTaskbar = false;
			this.Text = "VM Debugger";
			this.ToolStripDebugger.ResumeLayout(false);
			this.ToolStripDebugger.PerformLayout();
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
	}
}