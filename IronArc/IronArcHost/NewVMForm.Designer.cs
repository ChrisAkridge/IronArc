namespace IronArcHost
{
	partial class NewVMForm
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
			this.StaticLabelInitialProgram = new System.Windows.Forms.Label();
			this.TextBoxInitialProgram = new System.Windows.Forms.TextBox();
			this.ButtonSelectInitialProgram = new System.Windows.Forms.Button();
			this.ButtonSelectSystemProgram = new System.Windows.Forms.Button();
			this.TextBoxSystemProgram = new System.Windows.Forms.TextBox();
			this.StaticLabelSystemProgram = new System.Windows.Forms.Label();
			this.StaticLabelSystemMemory = new System.Windows.Forms.Label();
			this.StaticLabelLoadAtAddress = new System.Windows.Forms.Label();
			this.NumUDLoadAtAddress = new System.Windows.Forms.NumericUpDown();
			this.GroupBoxSeparator1 = new System.Windows.Forms.GroupBox();
			this.NumUDSystemMemory = new System.Windows.Forms.NumericUpDown();
			this.StaticLabelStackMemory = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.GroupBoxSeparator2 = new System.Windows.Forms.GroupBox();
			this.StaticLabelInitialHardwareDevices = new System.Windows.Forms.Label();
			this.CLBInitialHardwareDevices = new System.Windows.Forms.CheckedListBox();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.NumUDLoadAtAddress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NumUDSystemMemory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// StaticLabelInitialProgram
			// 
			this.StaticLabelInitialProgram.AutoSize = true;
			this.StaticLabelInitialProgram.Location = new System.Drawing.Point(13, 13);
			this.StaticLabelInitialProgram.Name = "StaticLabelInitialProgram";
			this.StaticLabelInitialProgram.Size = new System.Drawing.Size(85, 13);
			this.StaticLabelInitialProgram.TabIndex = 0;
			this.StaticLabelInitialProgram.Text = "Initial Program:";
			// 
			// TextBoxInitialProgram
			// 
			this.TextBoxInitialProgram.Location = new System.Drawing.Point(112, 10);
			this.TextBoxInitialProgram.Name = "TextBoxInitialProgram";
			this.TextBoxInitialProgram.Size = new System.Drawing.Size(214, 22);
			this.TextBoxInitialProgram.TabIndex = 1;
			// 
			// ButtonSelectInitialProgram
			// 
			this.ButtonSelectInitialProgram.Location = new System.Drawing.Point(334, 8);
			this.ButtonSelectInitialProgram.Name = "ButtonSelectInitialProgram";
			this.ButtonSelectInitialProgram.Size = new System.Drawing.Size(25, 23);
			this.ButtonSelectInitialProgram.TabIndex = 2;
			this.ButtonSelectInitialProgram.Text = "...";
			this.ButtonSelectInitialProgram.UseVisualStyleBackColor = true;
			// 
			// ButtonSelectSystemProgram
			// 
			this.ButtonSelectSystemProgram.Location = new System.Drawing.Point(334, 67);
			this.ButtonSelectSystemProgram.Name = "ButtonSelectSystemProgram";
			this.ButtonSelectSystemProgram.Size = new System.Drawing.Size(25, 23);
			this.ButtonSelectSystemProgram.TabIndex = 5;
			this.ButtonSelectSystemProgram.Text = "...";
			this.ButtonSelectSystemProgram.UseVisualStyleBackColor = true;
			// 
			// TextBoxSystemProgram
			// 
			this.TextBoxSystemProgram.Location = new System.Drawing.Point(112, 69);
			this.TextBoxSystemProgram.Name = "TextBoxSystemProgram";
			this.TextBoxSystemProgram.Size = new System.Drawing.Size(214, 22);
			this.TextBoxSystemProgram.TabIndex = 4;
			// 
			// StaticLabelSystemProgram
			// 
			this.StaticLabelSystemProgram.AutoSize = true;
			this.StaticLabelSystemProgram.Location = new System.Drawing.Point(13, 72);
			this.StaticLabelSystemProgram.Name = "StaticLabelSystemProgram";
			this.StaticLabelSystemProgram.Size = new System.Drawing.Size(91, 13);
			this.StaticLabelSystemProgram.TabIndex = 3;
			this.StaticLabelSystemProgram.Text = "System Program:";
			// 
			// StaticLabelSystemMemory
			// 
			this.StaticLabelSystemMemory.AutoSize = true;
			this.StaticLabelSystemMemory.Location = new System.Drawing.Point(13, 115);
			this.StaticLabelSystemMemory.Name = "StaticLabelSystemMemory";
			this.StaticLabelSystemMemory.Size = new System.Drawing.Size(67, 13);
			this.StaticLabelSystemMemory.TabIndex = 6;
			this.StaticLabelSystemMemory.Text = "Memory (B):";
			// 
			// StaticLabelLoadAtAddress
			// 
			this.StaticLabelLoadAtAddress.AutoSize = true;
			this.StaticLabelLoadAtAddress.Location = new System.Drawing.Point(13, 44);
			this.StaticLabelLoadAtAddress.Name = "StaticLabelLoadAtAddress";
			this.StaticLabelLoadAtAddress.Size = new System.Drawing.Size(93, 13);
			this.StaticLabelLoadAtAddress.TabIndex = 7;
			this.StaticLabelLoadAtAddress.Text = "Load At Address:";
			// 
			// NumUDLoadAtAddress
			// 
			this.NumUDLoadAtAddress.Hexadecimal = true;
			this.NumUDLoadAtAddress.Location = new System.Drawing.Point(112, 42);
			this.NumUDLoadAtAddress.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.NumUDLoadAtAddress.Name = "NumUDLoadAtAddress";
			this.NumUDLoadAtAddress.Size = new System.Drawing.Size(247, 22);
			this.NumUDLoadAtAddress.TabIndex = 8;
			// 
			// GroupBoxSeparator1
			// 
			this.GroupBoxSeparator1.Location = new System.Drawing.Point(16, 97);
			this.GroupBoxSeparator1.Name = "GroupBoxSeparator1";
			this.GroupBoxSeparator1.Size = new System.Drawing.Size(343, 10);
			this.GroupBoxSeparator1.TabIndex = 9;
			this.GroupBoxSeparator1.TabStop = false;
			// 
			// NumUDSystemMemory
			// 
			this.NumUDSystemMemory.Location = new System.Drawing.Point(112, 107);
			this.NumUDSystemMemory.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.NumUDSystemMemory.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NumUDSystemMemory.Name = "NumUDSystemMemory";
			this.NumUDSystemMemory.Size = new System.Drawing.Size(247, 22);
			this.NumUDSystemMemory.TabIndex = 10;
			this.NumUDSystemMemory.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// StaticLabelStackMemory
			// 
			this.StaticLabelStackMemory.AutoSize = true;
			this.StaticLabelStackMemory.Location = new System.Drawing.Point(13, 137);
			this.StaticLabelStackMemory.Name = "StaticLabelStackMemory";
			this.StaticLabelStackMemory.Size = new System.Drawing.Size(76, 13);
			this.StaticLabelStackMemory.TabIndex = 11;
			this.StaticLabelStackMemory.Text = "Stack Size (B):";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(112, 135);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(247, 22);
			this.numericUpDown1.TabIndex = 12;
			this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// GroupBoxSeparator2
			// 
			this.GroupBoxSeparator2.Location = new System.Drawing.Point(16, 163);
			this.GroupBoxSeparator2.Name = "GroupBoxSeparator2";
			this.GroupBoxSeparator2.Size = new System.Drawing.Size(343, 10);
			this.GroupBoxSeparator2.TabIndex = 10;
			this.GroupBoxSeparator2.TabStop = false;
			// 
			// StaticLabelInitialHardwareDevices
			// 
			this.StaticLabelInitialHardwareDevices.AutoSize = true;
			this.StaticLabelInitialHardwareDevices.Location = new System.Drawing.Point(16, 180);
			this.StaticLabelInitialHardwareDevices.Name = "StaticLabelInitialHardwareDevices";
			this.StaticLabelInitialHardwareDevices.Size = new System.Drawing.Size(133, 13);
			this.StaticLabelInitialHardwareDevices.TabIndex = 13;
			this.StaticLabelInitialHardwareDevices.Text = "Initial Hardware Devices:";
			// 
			// CLBInitialHardwareDevices
			// 
			this.CLBInitialHardwareDevices.FormattingEnabled = true;
			this.CLBInitialHardwareDevices.Location = new System.Drawing.Point(19, 197);
			this.CLBInitialHardwareDevices.Name = "CLBInitialHardwareDevices";
			this.CLBInitialHardwareDevices.Size = new System.Drawing.Size(340, 89);
			this.CLBInitialHardwareDevices.TabIndex = 14;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(284, 293);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 15;
			this.ButtonCancel.Text = "&Cancel";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// ButtonOK
			// 
			this.ButtonOK.Location = new System.Drawing.Point(203, 292);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 16;
			this.ButtonOK.Text = "&OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// NewVMForm
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(371, 326);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.CLBInitialHardwareDevices);
			this.Controls.Add(this.StaticLabelInitialHardwareDevices);
			this.Controls.Add(this.GroupBoxSeparator2);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.StaticLabelStackMemory);
			this.Controls.Add(this.NumUDSystemMemory);
			this.Controls.Add(this.GroupBoxSeparator1);
			this.Controls.Add(this.NumUDLoadAtAddress);
			this.Controls.Add(this.StaticLabelLoadAtAddress);
			this.Controls.Add(this.StaticLabelSystemMemory);
			this.Controls.Add(this.ButtonSelectSystemProgram);
			this.Controls.Add(this.TextBoxSystemProgram);
			this.Controls.Add(this.StaticLabelSystemProgram);
			this.Controls.Add(this.ButtonSelectInitialProgram);
			this.Controls.Add(this.TextBoxInitialProgram);
			this.Controls.Add(this.StaticLabelInitialProgram);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewVMForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Add a Virtual Machine";
			((System.ComponentModel.ISupportInitialize)(this.NumUDLoadAtAddress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NumUDSystemMemory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label StaticLabelInitialProgram;
		private System.Windows.Forms.TextBox TextBoxInitialProgram;
		private System.Windows.Forms.Button ButtonSelectInitialProgram;
		private System.Windows.Forms.Button ButtonSelectSystemProgram;
		private System.Windows.Forms.TextBox TextBoxSystemProgram;
		private System.Windows.Forms.Label StaticLabelSystemProgram;
		private System.Windows.Forms.Label StaticLabelSystemMemory;
		private System.Windows.Forms.Label StaticLabelLoadAtAddress;
		private System.Windows.Forms.NumericUpDown NumUDLoadAtAddress;
		private System.Windows.Forms.GroupBox GroupBoxSeparator1;
		private System.Windows.Forms.NumericUpDown NumUDSystemMemory;
		private System.Windows.Forms.Label StaticLabelStackMemory;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.GroupBox GroupBoxSeparator2;
		private System.Windows.Forms.Label StaticLabelInitialHardwareDevices;
		private System.Windows.Forms.CheckedListBox CLBInitialHardwareDevices;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonOK;
	}
}