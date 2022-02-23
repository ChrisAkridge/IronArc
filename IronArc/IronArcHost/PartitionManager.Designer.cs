
namespace IronArcHost
{
    partial class PartitionManager
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
            this.StaticLabelFilePath = new System.Windows.Forms.Label();
            this.TextFilePath = new System.Windows.Forms.TextBox();
            this.ButtonChooseFile = new System.Windows.Forms.Button();
            this.GroupSectionList = new System.Windows.Forms.GroupBox();
            this.ListFileSections = new System.Windows.Forms.ListView();
            this.ColumnSectionType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnPartitionID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonFormatFile = new System.Windows.Forms.Button();
            this.ButtonAllocateSection = new System.Windows.Forms.Button();
            this.ButtonRemovePartition = new System.Windows.Forms.Button();
            this.ButtonResizePartition = new System.Windows.Forms.Button();
            this.ButtonCreateNewFile = new System.Windows.Forms.Button();
            this.OFDChooseFile = new System.Windows.Forms.OpenFileDialog();
            this.ButtonLoadFile = new System.Windows.Forms.Button();
            this.ColumnPartitionName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GroupSectionList.SuspendLayout();
            this.SuspendLayout();
            // 
            // StaticLabelFilePath
            // 
            this.StaticLabelFilePath.AutoSize = true;
            this.StaticLabelFilePath.Location = new System.Drawing.Point(13, 13);
            this.StaticLabelFilePath.Name = "StaticLabelFilePath";
            this.StaticLabelFilePath.Size = new System.Drawing.Size(133, 15);
            this.StaticLabelFilePath.TabIndex = 0;
            this.StaticLabelFilePath.Text = "Storage device file path:";
            // 
            // TextFilePath
            // 
            this.TextFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextFilePath.Location = new System.Drawing.Point(152, 10);
            this.TextFilePath.Name = "TextFilePath";
            this.TextFilePath.Size = new System.Drawing.Size(234, 23);
            this.TextFilePath.TabIndex = 1;
            // 
            // ButtonChooseFile
            // 
            this.ButtonChooseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonChooseFile.Location = new System.Drawing.Point(392, 9);
            this.ButtonChooseFile.Name = "ButtonChooseFile";
            this.ButtonChooseFile.Size = new System.Drawing.Size(26, 23);
            this.ButtonChooseFile.TabIndex = 2;
            this.ButtonChooseFile.Text = "...";
            this.ButtonChooseFile.UseVisualStyleBackColor = true;
            this.ButtonChooseFile.Click += new System.EventHandler(this.ButtonChooseFile_Click);
            // 
            // GroupSectionList
            // 
            this.GroupSectionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSectionList.Controls.Add(this.ListFileSections);
            this.GroupSectionList.Location = new System.Drawing.Point(16, 39);
            this.GroupSectionList.Name = "GroupSectionList";
            this.GroupSectionList.Size = new System.Drawing.Size(491, 216);
            this.GroupSectionList.TabIndex = 3;
            this.GroupSectionList.TabStop = false;
            this.GroupSectionList.Text = "File Sections";
            // 
            // ListFileSections
            // 
            this.ListFileSections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListFileSections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnSectionType,
            this.ColumnPartitionID,
            this.ColumnPartitionName,
            this.ColumnSize});
            this.ListFileSections.Enabled = false;
            this.ListFileSections.HideSelection = false;
            this.ListFileSections.Location = new System.Drawing.Point(7, 23);
            this.ListFileSections.Name = "ListFileSections";
            this.ListFileSections.Size = new System.Drawing.Size(478, 187);
            this.ListFileSections.TabIndex = 0;
            this.ListFileSections.UseCompatibleStateImageBehavior = false;
            this.ListFileSections.View = System.Windows.Forms.View.Details;
            this.ListFileSections.SelectedIndexChanged += new System.EventHandler(this.ListFileSections_SelectedIndexChanged);
            // 
            // ColumnSectionType
            // 
            this.ColumnSectionType.Text = "Type";
            this.ColumnSectionType.Width = 112;
            // 
            // ColumnPartitionID
            // 
            this.ColumnPartitionID.Text = "Partition ID";
            this.ColumnPartitionID.Width = 105;
            // 
            // ColumnSize
            // 
            this.ColumnSize.Text = "Size";
            this.ColumnSize.Width = 112;
            // 
            // ButtonFormatFile
            // 
            this.ButtonFormatFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonFormatFile.Enabled = false;
            this.ButtonFormatFile.Location = new System.Drawing.Point(16, 261);
            this.ButtonFormatFile.Name = "ButtonFormatFile";
            this.ButtonFormatFile.Size = new System.Drawing.Size(160, 23);
            this.ButtonFormatFile.TabIndex = 4;
            this.ButtonFormatFile.Text = "&Format Entire File";
            this.ButtonFormatFile.UseVisualStyleBackColor = true;
            this.ButtonFormatFile.Click += new System.EventHandler(this.ButtonFormatFile_Click);
            // 
            // ButtonAllocateSection
            // 
            this.ButtonAllocateSection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAllocateSection.Enabled = false;
            this.ButtonAllocateSection.Location = new System.Drawing.Point(182, 261);
            this.ButtonAllocateSection.Name = "ButtonAllocateSection";
            this.ButtonAllocateSection.Size = new System.Drawing.Size(160, 23);
            this.ButtonAllocateSection.TabIndex = 5;
            this.ButtonAllocateSection.Text = "&Allocate Unallocated Space";
            this.ButtonAllocateSection.UseVisualStyleBackColor = true;
            // 
            // ButtonRemovePartition
            // 
            this.ButtonRemovePartition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonRemovePartition.Enabled = false;
            this.ButtonRemovePartition.Location = new System.Drawing.Point(348, 261);
            this.ButtonRemovePartition.Name = "ButtonRemovePartition";
            this.ButtonRemovePartition.Size = new System.Drawing.Size(160, 23);
            this.ButtonRemovePartition.TabIndex = 6;
            this.ButtonRemovePartition.Text = "&Remove Partition";
            this.ButtonRemovePartition.UseVisualStyleBackColor = true;
            this.ButtonRemovePartition.Click += new System.EventHandler(this.ButtonRemovePartition_Click);
            // 
            // ButtonResizePartition
            // 
            this.ButtonResizePartition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonResizePartition.Enabled = false;
            this.ButtonResizePartition.Location = new System.Drawing.Point(16, 290);
            this.ButtonResizePartition.Name = "ButtonResizePartition";
            this.ButtonResizePartition.Size = new System.Drawing.Size(160, 23);
            this.ButtonResizePartition.TabIndex = 7;
            this.ButtonResizePartition.Text = "&Resize Partition";
            this.ButtonResizePartition.UseVisualStyleBackColor = true;
            this.ButtonResizePartition.Click += new System.EventHandler(this.ButtonResizePartition_Click);
            // 
            // ButtonCreateNewFile
            // 
            this.ButtonCreateNewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCreateNewFile.Enabled = false;
            this.ButtonCreateNewFile.Location = new System.Drawing.Point(348, 290);
            this.ButtonCreateNewFile.Name = "ButtonCreateNewFile";
            this.ButtonCreateNewFile.Size = new System.Drawing.Size(160, 23);
            this.ButtonCreateNewFile.TabIndex = 8;
            this.ButtonCreateNewFile.Text = "&Create and Format...";
            this.ButtonCreateNewFile.UseVisualStyleBackColor = true;
            // 
            // OFDChooseFile
            // 
            this.OFDChooseFile.Filter = "IronArc Storage Device File (*.isdf)|*.isdf|All files (*.*)|*.*";
            this.OFDChooseFile.Title = "IronArc - Choose Storage Device File";
            // 
            // ButtonLoadFile
            // 
            this.ButtonLoadFile.Location = new System.Drawing.Point(424, 9);
            this.ButtonLoadFile.Name = "ButtonLoadFile";
            this.ButtonLoadFile.Size = new System.Drawing.Size(75, 23);
            this.ButtonLoadFile.TabIndex = 9;
            this.ButtonLoadFile.Text = "&Load";
            this.ButtonLoadFile.UseVisualStyleBackColor = true;
            this.ButtonLoadFile.Click += new System.EventHandler(this.ButtonLoadFile_Click);
            // 
            // ColumnPartitionName
            // 
            this.ColumnPartitionName.Text = "Name";
            // 
            // PartitionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 321);
            this.Controls.Add(this.ButtonLoadFile);
            this.Controls.Add(this.ButtonCreateNewFile);
            this.Controls.Add(this.ButtonResizePartition);
            this.Controls.Add(this.ButtonRemovePartition);
            this.Controls.Add(this.ButtonAllocateSection);
            this.Controls.Add(this.ButtonFormatFile);
            this.Controls.Add(this.GroupSectionList);
            this.Controls.Add(this.ButtonChooseFile);
            this.Controls.Add(this.TextFilePath);
            this.Controls.Add(this.StaticLabelFilePath);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(535, 350);
            this.Name = "PartitionManager";
            this.ShowIcon = false;
            this.Text = "Partition Manager";
            this.GroupSectionList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StaticLabelFilePath;
        private System.Windows.Forms.TextBox TextFilePath;
        private System.Windows.Forms.Button ButtonChooseFile;
        private System.Windows.Forms.GroupBox GroupSectionList;
        private System.Windows.Forms.ListView ListFileSections;
        private System.Windows.Forms.ColumnHeader ColumnSectionType;
        private System.Windows.Forms.ColumnHeader ColumnPartitionID;
        private System.Windows.Forms.ColumnHeader ColumnSize;
        private System.Windows.Forms.Button ButtonFormatFile;
        private System.Windows.Forms.Button ButtonAllocateSection;
        private System.Windows.Forms.Button ButtonRemovePartition;
        private System.Windows.Forms.Button ButtonResizePartition;
        private System.Windows.Forms.Button ButtonCreateNewFile;
        private System.Windows.Forms.OpenFileDialog OFDChooseFile;
        private System.Windows.Forms.Button ButtonLoadFile;
        private System.Windows.Forms.ColumnHeader ColumnPartitionName;
    }
}