namespace IronArcHost
{
	partial class RegisterEditor
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
			this.StaticLabelValue = new System.Windows.Forms.Label();
			this.TextValue = new System.Windows.Forms.TextBox();
			this.StaticLabelAsBigEndian = new System.Windows.Forms.Label();
			this.TextAsBigEndian = new System.Windows.Forms.TextBox();
			this.StaticLabelSigned = new System.Windows.Forms.Label();
			this.TextSigned = new System.Windows.Forms.TextBox();
			this.StaticLabelUnsigned = new System.Windows.Forms.Label();
			this.TextUnsigned = new System.Windows.Forms.TextBox();
			this.StaticLabelFloatingPoint = new System.Windows.Forms.Label();
			this.TextFloatingPoint = new System.Windows.Forms.TextBox();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// StaticLabelValue
			// 
			this.StaticLabelValue.AutoSize = true;
			this.StaticLabelValue.Location = new System.Drawing.Point(55, 12);
			this.StaticLabelValue.Name = "StaticLabelValue";
			this.StaticLabelValue.Size = new System.Drawing.Size(39, 13);
			this.StaticLabelValue.TabIndex = 0;
			this.StaticLabelValue.Text = "Value:";
			// 
			// TextValue
			// 
			this.TextValue.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextValue.Location = new System.Drawing.Point(100, 10);
			this.TextValue.Name = "TextValue";
			this.TextValue.Size = new System.Drawing.Size(104, 20);
			this.TextValue.TabIndex = 1;
			this.TextValue.Text = "0000000000000000";
			this.TextValue.TextChanged += new System.EventHandler(this.TextValue_TextChanged);
			// 
			// StaticLabelAsBigEndian
			// 
			this.StaticLabelAsBigEndian.AutoSize = true;
			this.StaticLabelAsBigEndian.Location = new System.Drawing.Point(13, 35);
			this.StaticLabelAsBigEndian.Name = "StaticLabelAsBigEndian";
			this.StaticLabelAsBigEndian.Size = new System.Drawing.Size(81, 13);
			this.StaticLabelAsBigEndian.TabIndex = 2;
			this.StaticLabelAsBigEndian.Text = "As Big Endian:";
			// 
			// TextAsBigEndian
			// 
			this.TextAsBigEndian.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextAsBigEndian.Location = new System.Drawing.Point(100, 32);
			this.TextAsBigEndian.Name = "TextAsBigEndian";
			this.TextAsBigEndian.Size = new System.Drawing.Size(104, 20);
			this.TextAsBigEndian.TabIndex = 3;
			this.TextAsBigEndian.Text = "0000000000000000";
			this.TextAsBigEndian.TextChanged += new System.EventHandler(this.TextAsBigEndian_TextChanged);
			// 
			// StaticLabelSigned
			// 
			this.StaticLabelSigned.AutoSize = true;
			this.StaticLabelSigned.Location = new System.Drawing.Point(48, 56);
			this.StaticLabelSigned.Name = "StaticLabelSigned";
			this.StaticLabelSigned.Size = new System.Drawing.Size(46, 13);
			this.StaticLabelSigned.TabIndex = 4;
			this.StaticLabelSigned.Text = "Signed:";
			// 
			// TextSigned
			// 
			this.TextSigned.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextSigned.Location = new System.Drawing.Point(100, 54);
			this.TextSigned.Name = "TextSigned";
			this.TextSigned.Size = new System.Drawing.Size(104, 20);
			this.TextSigned.TabIndex = 5;
			this.TextSigned.Text = "0";
			this.TextSigned.TextChanged += new System.EventHandler(this.TextSigned_TextChanged);
			// 
			// StaticLabelUnsigned
			// 
			this.StaticLabelUnsigned.AutoSize = true;
			this.StaticLabelUnsigned.Location = new System.Drawing.Point(34, 79);
			this.StaticLabelUnsigned.Name = "StaticLabelUnsigned";
			this.StaticLabelUnsigned.Size = new System.Drawing.Size(60, 13);
			this.StaticLabelUnsigned.TabIndex = 6;
			this.StaticLabelUnsigned.Text = "Unsigned:";
			// 
			// TextUnsigned
			// 
			this.TextUnsigned.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextUnsigned.Location = new System.Drawing.Point(100, 76);
			this.TextUnsigned.Name = "TextUnsigned";
			this.TextUnsigned.Size = new System.Drawing.Size(104, 20);
			this.TextUnsigned.TabIndex = 7;
			this.TextUnsigned.Text = "0";
			this.TextUnsigned.TextChanged += new System.EventHandler(this.TextUnsigned_TextChanged);
			// 
			// StaticLabelFloatingPoint
			// 
			this.StaticLabelFloatingPoint.AutoSize = true;
			this.StaticLabelFloatingPoint.Location = new System.Drawing.Point(13, 100);
			this.StaticLabelFloatingPoint.Name = "StaticLabelFloatingPoint";
			this.StaticLabelFloatingPoint.Size = new System.Drawing.Size(83, 13);
			this.StaticLabelFloatingPoint.TabIndex = 8;
			this.StaticLabelFloatingPoint.Text = "Floating Point:";
			// 
			// TextFloatingPoint
			// 
			this.TextFloatingPoint.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextFloatingPoint.Location = new System.Drawing.Point(100, 98);
			this.TextFloatingPoint.Name = "TextFloatingPoint";
			this.TextFloatingPoint.Size = new System.Drawing.Size(104, 20);
			this.TextFloatingPoint.TabIndex = 9;
			this.TextFloatingPoint.Text = "0.0";
			this.TextFloatingPoint.TextChanged += new System.EventHandler(this.TextFloatingPoint_TextChanged);
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(128, 125);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 10;
			this.ButtonCancel.Text = "C&ancel";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			// 
			// ButtonOK
			// 
			this.ButtonOK.Location = new System.Drawing.Point(47, 125);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 11;
			this.ButtonOK.Text = "&OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			// 
			// RegisterEditor
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(207, 158);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.TextFloatingPoint);
			this.Controls.Add(this.StaticLabelFloatingPoint);
			this.Controls.Add(this.TextUnsigned);
			this.Controls.Add(this.StaticLabelUnsigned);
			this.Controls.Add(this.TextSigned);
			this.Controls.Add(this.StaticLabelSigned);
			this.Controls.Add(this.TextAsBigEndian);
			this.Controls.Add(this.StaticLabelAsBigEndian);
			this.Controls.Add(this.TextValue);
			this.Controls.Add(this.StaticLabelValue);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RegisterEditor";
			this.Text = "Edit Register";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label StaticLabelValue;
		public System.Windows.Forms.TextBox TextValue;
		private System.Windows.Forms.Label StaticLabelAsBigEndian;
		public System.Windows.Forms.TextBox TextAsBigEndian;
		private System.Windows.Forms.Label StaticLabelSigned;
		private System.Windows.Forms.TextBox TextSigned;
		private System.Windows.Forms.Label StaticLabelUnsigned;
		private System.Windows.Forms.TextBox TextUnsigned;
		private System.Windows.Forms.Label StaticLabelFloatingPoint;
		private System.Windows.Forms.TextBox TextFloatingPoint;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonOK;
	}
}