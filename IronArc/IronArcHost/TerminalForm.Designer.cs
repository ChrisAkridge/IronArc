namespace IronArcHost
{
	partial class TerminalForm
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
			this.TextTerminalWindow = new System.Windows.Forms.TextBox();
			this.TextBoxInput = new System.Windows.Forms.TextBox();
			this.CMSTerminal = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CMISaveOutput = new System.Windows.Forms.ToolStripMenuItem();
			this.CMIClearOutput = new System.Windows.Forms.ToolStripMenuItem();
			this.SFDSaveOutput = new System.Windows.Forms.SaveFileDialog();
			this.CMSTerminal.SuspendLayout();
			this.SuspendLayout();
			// 
			// TextTerminalWindow
			// 
			this.TextTerminalWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextTerminalWindow.BackColor = System.Drawing.Color.Black;
			this.TextTerminalWindow.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextTerminalWindow.ForeColor = System.Drawing.Color.White;
			this.TextTerminalWindow.Location = new System.Drawing.Point(0, 0);
			this.TextTerminalWindow.Multiline = true;
			this.TextTerminalWindow.Name = "TextTerminalWindow";
			this.TextTerminalWindow.ReadOnly = true;
			this.TextTerminalWindow.Size = new System.Drawing.Size(412, 305);
			this.TextTerminalWindow.TabIndex = 0;
			this.TextTerminalWindow.Text = "Hello, world!";
			// 
			// TextBoxInput
			// 
			this.TextBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxInput.BackColor = System.Drawing.Color.Navy;
			this.TextBoxInput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextBoxInput.ForeColor = System.Drawing.Color.White;
			this.TextBoxInput.Location = new System.Drawing.Point(0, 311);
			this.TextBoxInput.Name = "TextBoxInput";
			this.TextBoxInput.Size = new System.Drawing.Size(412, 23);
			this.TextBoxInput.TabIndex = 1;
			this.TextBoxInput.Text = "Input";
			// 
			// CMSTerminal
			// 
			this.CMSTerminal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CMISaveOutput,
            this.CMIClearOutput});
			this.CMSTerminal.Name = "CMSTerminal";
			this.CMSTerminal.Size = new System.Drawing.Size(149, 48);
			// 
			// CMISaveOutput
			// 
			this.CMISaveOutput.Name = "CMISaveOutput";
			this.CMISaveOutput.Size = new System.Drawing.Size(148, 22);
			this.CMISaveOutput.Text = "&Save Output...";
			this.CMISaveOutput.Click += new System.EventHandler(this.CMISaveOutput_Click);
			// 
			// CMIClearOutput
			// 
			this.CMIClearOutput.Name = "CMIClearOutput";
			this.CMIClearOutput.Size = new System.Drawing.Size(148, 22);
			this.CMIClearOutput.Text = "&Clear";
			// 
			// SFDSaveOutput
			// 
			this.SFDSaveOutput.DefaultExt = "txt";
			this.SFDSaveOutput.Filter = "Text File (*.txt)|*.txt|All files|*.*";
			this.SFDSaveOutput.Title = "Save Terminal Output";
			// 
			// Terminal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(412, 334);
			this.Controls.Add(this.TextBoxInput);
			this.Controls.Add(this.TextTerminalWindow);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "Terminal";
			this.ShowIcon = false;
			this.Text = "VM Terminal";
			this.CMSTerminal.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox TextTerminalWindow;
		private System.Windows.Forms.TextBox TextBoxInput;
		private System.Windows.Forms.ContextMenuStrip CMSTerminal;
		private System.Windows.Forms.ToolStripMenuItem CMISaveOutput;
		private System.Windows.Forms.ToolStripMenuItem CMIClearOutput;
		private System.Windows.Forms.SaveFileDialog SFDSaveOutput;
	}
}