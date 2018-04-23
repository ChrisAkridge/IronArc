namespace IronArcHost
{
	partial class DebugTerminalInputForm
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
			this.StaticLabelDescription = new System.Windows.Forms.Label();
			this.TextInput = new System.Windows.Forms.TextBox();
			this.ButtonSubmit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// StaticLabelDescription
			// 
			this.StaticLabelDescription.Location = new System.Drawing.Point(13, 13);
			this.StaticLabelDescription.Name = "StaticLabelDescription";
			this.StaticLabelDescription.Size = new System.Drawing.Size(259, 32);
			this.StaticLabelDescription.TabIndex = 0;
			this.StaticLabelDescription.Text = "VM {ee1290e5-0b97-453a-a46f-59fbc115a2a1} requires a character of input.";
			// 
			// TextInput
			// 
			this.TextInput.Location = new System.Drawing.Point(16, 49);
			this.TextInput.Name = "TextInput";
			this.TextInput.Size = new System.Drawing.Size(256, 22);
			this.TextInput.TabIndex = 1;
			this.TextInput.TextChanged += new System.EventHandler(this.TextInput_TextChanged);
			// 
			// ButtonSubmit
			// 
			this.ButtonSubmit.Enabled = false;
			this.ButtonSubmit.Location = new System.Drawing.Point(196, 78);
			this.ButtonSubmit.Name = "ButtonSubmit";
			this.ButtonSubmit.Size = new System.Drawing.Size(75, 23);
			this.ButtonSubmit.TabIndex = 2;
			this.ButtonSubmit.Text = "&Submit";
			this.ButtonSubmit.UseVisualStyleBackColor = true;
			this.ButtonSubmit.Click += new System.EventHandler(this.ButtonSubmit_Click);
			// 
			// DebugTerminalInputForm
			// 
			this.AcceptButton = this.ButtonSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 106);
			this.ControlBox = false;
			this.Controls.Add(this.ButtonSubmit);
			this.Controls.Add(this.TextInput);
			this.Controls.Add(this.StaticLabelDescription);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DebugTerminalInputForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Terminal Input Required";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label StaticLabelDescription;
		private System.Windows.Forms.TextBox TextInput;
		private System.Windows.Forms.Button ButtonSubmit;
	}
}