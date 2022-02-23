
namespace IronArcHost
{
    partial class FormatStorageFileOptions
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
            this.StaticLabelWarning = new System.Windows.Forms.Label();
            this.StaticLabelSectorSize = new System.Windows.Forms.Label();
            this.ComboSectorSize = new System.Windows.Forms.ComboBox();
            this.CheckOverwrite = new System.Windows.Forms.CheckBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.GroupOverwriteProgress = new System.Windows.Forms.GroupBox();
            this.LabelFormatProgress = new System.Windows.Forms.Label();
            this.ProgressOverwrite = new System.Windows.Forms.ProgressBar();
            this.GroupOverwriteProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // StaticLabelWarning
            // 
            this.StaticLabelWarning.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StaticLabelWarning.Location = new System.Drawing.Point(13, 13);
            this.StaticLabelWarning.Name = "StaticLabelWarning";
            this.StaticLabelWarning.Size = new System.Drawing.Size(280, 67);
            this.StaticLabelWarning.TabIndex = 0;
            this.StaticLabelWarning.Text = "Warning: Formatting this file will, at least, rewrite the partition table and los" +
    "e all allocated partitions! If you select Overwrite, all data in this file will " +
    "be erased!";
            // 
            // StaticLabelSectorSize
            // 
            this.StaticLabelSectorSize.AutoSize = true;
            this.StaticLabelSectorSize.Location = new System.Drawing.Point(13, 80);
            this.StaticLabelSectorSize.Name = "StaticLabelSectorSize";
            this.StaticLabelSectorSize.Size = new System.Drawing.Size(66, 15);
            this.StaticLabelSectorSize.TabIndex = 1;
            this.StaticLabelSectorSize.Text = "Sector Size:";
            // 
            // ComboSectorSize
            // 
            this.ComboSectorSize.DropDownHeight = 250;
            this.ComboSectorSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboSectorSize.FormattingEnabled = true;
            this.ComboSectorSize.IntegralHeight = false;
            this.ComboSectorSize.Items.AddRange(new object[] {
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "8192",
            "16384",
            "32768",
            "65536",
            "131072",
            "262144",
            "524288",
            "1048576",
            "2097152",
            "4194304",
            "8388608",
            "16777216"});
            this.ComboSectorSize.Location = new System.Drawing.Point(85, 77);
            this.ComboSectorSize.Name = "ComboSectorSize";
            this.ComboSectorSize.Size = new System.Drawing.Size(208, 23);
            this.ComboSectorSize.TabIndex = 2;
            // 
            // CheckOverwrite
            // 
            this.CheckOverwrite.AutoSize = true;
            this.CheckOverwrite.Location = new System.Drawing.Point(16, 106);
            this.CheckOverwrite.Name = "CheckOverwrite";
            this.CheckOverwrite.Size = new System.Drawing.Size(77, 19);
            this.CheckOverwrite.TabIndex = 3;
            this.CheckOverwrite.Text = "&Overwrite";
            this.CheckOverwrite.UseVisualStyleBackColor = true;
            this.CheckOverwrite.CheckedChanged += new System.EventHandler(this.CheckOverwrite_CheckedChanged);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(218, 128);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 4;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(137, 128);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 5;
            this.ButtonOK.Text = "&OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // GroupOverwriteProgress
            // 
            this.GroupOverwriteProgress.Controls.Add(this.ProgressOverwrite);
            this.GroupOverwriteProgress.Controls.Add(this.LabelFormatProgress);
            this.GroupOverwriteProgress.Location = new System.Drawing.Point(16, 157);
            this.GroupOverwriteProgress.Name = "GroupOverwriteProgress";
            this.GroupOverwriteProgress.Size = new System.Drawing.Size(277, 71);
            this.GroupOverwriteProgress.TabIndex = 6;
            this.GroupOverwriteProgress.TabStop = false;
            this.GroupOverwriteProgress.Text = "Overwrite Progress";
            this.GroupOverwriteProgress.Visible = false;
            // 
            // LabelFormatProgress
            // 
            this.LabelFormatProgress.AutoSize = true;
            this.LabelFormatProgress.Location = new System.Drawing.Point(6, 19);
            this.LabelFormatProgress.Name = "LabelFormatProgress";
            this.LabelFormatProgress.Size = new System.Drawing.Size(149, 15);
            this.LabelFormatProgress.TabIndex = 0;
            this.LabelFormatProgress.Text = "{0} of {1} bytes overwritten.";
            // 
            // ProgressOverwrite
            // 
            this.ProgressOverwrite.Location = new System.Drawing.Point(9, 37);
            this.ProgressOverwrite.Name = "ProgressOverwrite";
            this.ProgressOverwrite.Size = new System.Drawing.Size(262, 23);
            this.ProgressOverwrite.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressOverwrite.TabIndex = 1;
            // 
            // FormatStorageFileOptions
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(305, 159);
            this.Controls.Add(this.GroupOverwriteProgress);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.CheckOverwrite);
            this.Controls.Add(this.ComboSectorSize);
            this.Controls.Add(this.StaticLabelSectorSize);
            this.Controls.Add(this.StaticLabelWarning);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormatStorageFileOptions";
            this.Text = "IronArc - Format Storage File";
            this.GroupOverwriteProgress.ResumeLayout(false);
            this.GroupOverwriteProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StaticLabelWarning;
        private System.Windows.Forms.Label StaticLabelSectorSize;
        private System.Windows.Forms.ComboBox ComboSectorSize;
        private System.Windows.Forms.CheckBox CheckOverwrite;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.GroupBox GroupOverwriteProgress;
        private System.Windows.Forms.ProgressBar ProgressOverwrite;
        private System.Windows.Forms.Label LabelFormatProgress;
    }
}