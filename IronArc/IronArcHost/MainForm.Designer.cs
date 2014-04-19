namespace IronArcHost
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageInstances = new System.Windows.Forms.TabPage();
            this.TabPageConsole = new System.Windows.Forms.TabPage();
            this.TabPageDebug = new System.Windows.Forms.TabPage();
            this.ButtonAddInstance = new System.Windows.Forms.Button();
            this.ButtonStartInstance = new System.Windows.Forms.Button();
            this.ButtonRemoveInstance = new System.Windows.Forms.Button();
            this.ButtonSaveInstance = new System.Windows.Forms.Button();
            this.ButtonLoadInstance = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ColumnIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnMemorySize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnDiskfileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StaticLabelInstance = new System.Windows.Forms.Label();
            this.ComboInstanceSelector = new System.Windows.Forms.ComboBox();
            this.StaticLabelSeparator = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.StaticLabelInstanceDebug = new System.Windows.Forms.Label();
            this.ComboBoxInstancesDebug = new System.Windows.Forms.ComboBox();
            this.StaticLabelSeparatorDebug = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TSBStart = new System.Windows.Forms.ToolStripButton();
            this.TSBPause = new System.Windows.Forms.ToolStripButton();
            this.TSSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSPStepInto = new System.Windows.Forms.ToolStripButton();
            this.TspStepOver = new System.Windows.Forms.ToolStripButton();
            this.TSBStepOut = new System.Windows.Forms.ToolStripButton();
            this.OFDInstance = new System.Windows.Forms.OpenFileDialog();
            this.SFDInstance = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.TabPageInstances.SuspendLayout();
            this.TabPageConsole.SuspendLayout();
            this.TabPageDebug.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TabPageInstances);
            this.tabControl1.Controls.Add(this.TabPageConsole);
            this.tabControl1.Controls.Add(this.TabPageDebug);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(559, 336);
            this.tabControl1.TabIndex = 0;
            // 
            // TabPageInstances
            // 
            this.TabPageInstances.Controls.Add(this.listView1);
            this.TabPageInstances.Controls.Add(this.ButtonLoadInstance);
            this.TabPageInstances.Controls.Add(this.ButtonSaveInstance);
            this.TabPageInstances.Controls.Add(this.ButtonRemoveInstance);
            this.TabPageInstances.Controls.Add(this.ButtonStartInstance);
            this.TabPageInstances.Controls.Add(this.ButtonAddInstance);
            this.TabPageInstances.Location = new System.Drawing.Point(4, 22);
            this.TabPageInstances.Name = "TabPageInstances";
            this.TabPageInstances.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageInstances.Size = new System.Drawing.Size(551, 310);
            this.TabPageInstances.TabIndex = 0;
            this.TabPageInstances.Text = "Instances";
            this.TabPageInstances.UseVisualStyleBackColor = true;
            // 
            // TabPageConsole
            // 
            this.TabPageConsole.Controls.Add(this.textBox1);
            this.TabPageConsole.Controls.Add(this.StaticLabelSeparator);
            this.TabPageConsole.Controls.Add(this.ComboInstanceSelector);
            this.TabPageConsole.Controls.Add(this.StaticLabelInstance);
            this.TabPageConsole.Location = new System.Drawing.Point(4, 22);
            this.TabPageConsole.Name = "TabPageConsole";
            this.TabPageConsole.Size = new System.Drawing.Size(551, 310);
            this.TabPageConsole.TabIndex = 1;
            this.TabPageConsole.Text = "Console";
            this.TabPageConsole.UseVisualStyleBackColor = true;
            // 
            // TabPageDebug
            // 
            this.TabPageDebug.Controls.Add(this.toolStrip1);
            this.TabPageDebug.Controls.Add(this.StaticLabelSeparatorDebug);
            this.TabPageDebug.Controls.Add(this.ComboBoxInstancesDebug);
            this.TabPageDebug.Controls.Add(this.StaticLabelInstanceDebug);
            this.TabPageDebug.Location = new System.Drawing.Point(4, 22);
            this.TabPageDebug.Name = "TabPageDebug";
            this.TabPageDebug.Size = new System.Drawing.Size(551, 310);
            this.TabPageDebug.TabIndex = 2;
            this.TabPageDebug.Text = "Debug";
            this.TabPageDebug.UseVisualStyleBackColor = true;
            // 
            // ButtonAddInstance
            // 
            this.ButtonAddInstance.Location = new System.Drawing.Point(7, 281);
            this.ButtonAddInstance.Name = "ButtonAddInstance";
            this.ButtonAddInstance.Size = new System.Drawing.Size(103, 23);
            this.ButtonAddInstance.TabIndex = 0;
            this.ButtonAddInstance.Text = "&Add Instance...";
            this.ButtonAddInstance.UseVisualStyleBackColor = true;
            // 
            // ButtonStartInstance
            // 
            this.ButtonStartInstance.Enabled = false;
            this.ButtonStartInstance.Location = new System.Drawing.Point(442, 281);
            this.ButtonStartInstance.Name = "ButtonStartInstance";
            this.ButtonStartInstance.Size = new System.Drawing.Size(103, 23);
            this.ButtonStartInstance.TabIndex = 1;
            this.ButtonStartInstance.Text = "&Start Instance";
            this.ButtonStartInstance.UseVisualStyleBackColor = true;
            // 
            // ButtonRemoveInstance
            // 
            this.ButtonRemoveInstance.Enabled = false;
            this.ButtonRemoveInstance.Location = new System.Drawing.Point(116, 281);
            this.ButtonRemoveInstance.Name = "ButtonRemoveInstance";
            this.ButtonRemoveInstance.Size = new System.Drawing.Size(103, 23);
            this.ButtonRemoveInstance.TabIndex = 2;
            this.ButtonRemoveInstance.Text = "&Remove Instance";
            this.ButtonRemoveInstance.UseVisualStyleBackColor = true;
            // 
            // ButtonSaveInstance
            // 
            this.ButtonSaveInstance.Enabled = false;
            this.ButtonSaveInstance.Location = new System.Drawing.Point(225, 281);
            this.ButtonSaveInstance.Name = "ButtonSaveInstance";
            this.ButtonSaveInstance.Size = new System.Drawing.Size(103, 23);
            this.ButtonSaveInstance.TabIndex = 3;
            this.ButtonSaveInstance.Text = "&Save Instance...";
            this.ButtonSaveInstance.UseVisualStyleBackColor = true;
            // 
            // ButtonLoadInstance
            // 
            this.ButtonLoadInstance.Location = new System.Drawing.Point(334, 281);
            this.ButtonLoadInstance.Name = "ButtonLoadInstance";
            this.ButtonLoadInstance.Size = new System.Drawing.Size(103, 23);
            this.ButtonLoadInstance.TabIndex = 4;
            this.ButtonLoadInstance.Text = "&Open Instance...";
            this.ButtonLoadInstance.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnIndex,
            this.ColumnStatus,
            this.ColumnMemorySize,
            this.ColumnDiskfileSize});
            this.listView1.Location = new System.Drawing.Point(7, 7);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(538, 268);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ColumnIndex
            // 
            this.ColumnIndex.Text = "#";
            this.ColumnIndex.Width = 19;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.Text = "Status";
            // 
            // ColumnMemorySize
            // 
            this.ColumnMemorySize.Text = "Memory";
            // 
            // ColumnDiskfileSize
            // 
            this.ColumnDiskfileSize.Text = "Diskfile";
            // 
            // StaticLabelInstance
            // 
            this.StaticLabelInstance.AutoSize = true;
            this.StaticLabelInstance.Location = new System.Drawing.Point(5, 7);
            this.StaticLabelInstance.Name = "StaticLabelInstance";
            this.StaticLabelInstance.Size = new System.Drawing.Size(53, 13);
            this.StaticLabelInstance.TabIndex = 0;
            this.StaticLabelInstance.Text = "Instance:";
            // 
            // ComboInstanceSelector
            // 
            this.ComboInstanceSelector.FormattingEnabled = true;
            this.ComboInstanceSelector.Location = new System.Drawing.Point(64, 4);
            this.ComboInstanceSelector.Name = "ComboInstanceSelector";
            this.ComboInstanceSelector.Size = new System.Drawing.Size(484, 21);
            this.ComboInstanceSelector.TabIndex = 1;
            // 
            // StaticLabelSeparator
            // 
            this.StaticLabelSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StaticLabelSeparator.Location = new System.Drawing.Point(0, 28);
            this.StaticLabelSeparator.Name = "StaticLabelSeparator";
            this.StaticLabelSeparator.Size = new System.Drawing.Size(548, 2);
            this.StaticLabelSeparator.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(4, 34);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(544, 273);
            this.textBox1.TabIndex = 3;
            // 
            // StaticLabelInstanceDebug
            // 
            this.StaticLabelInstanceDebug.AutoSize = true;
            this.StaticLabelInstanceDebug.Location = new System.Drawing.Point(5, 7);
            this.StaticLabelInstanceDebug.Name = "StaticLabelInstanceDebug";
            this.StaticLabelInstanceDebug.Size = new System.Drawing.Size(53, 13);
            this.StaticLabelInstanceDebug.TabIndex = 0;
            this.StaticLabelInstanceDebug.Text = "Instance:";
            // 
            // ComboBoxInstancesDebug
            // 
            this.ComboBoxInstancesDebug.FormattingEnabled = true;
            this.ComboBoxInstancesDebug.Location = new System.Drawing.Point(64, 4);
            this.ComboBoxInstancesDebug.Name = "ComboBoxInstancesDebug";
            this.ComboBoxInstancesDebug.Size = new System.Drawing.Size(484, 21);
            this.ComboBoxInstancesDebug.TabIndex = 1;
            // 
            // StaticLabelSeparatorDebug
            // 
            this.StaticLabelSeparatorDebug.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StaticLabelSeparatorDebug.Location = new System.Drawing.Point(0, 28);
            this.StaticLabelSeparatorDebug.Name = "StaticLabelSeparatorDebug";
            this.StaticLabelSeparatorDebug.Size = new System.Drawing.Size(548, 2);
            this.StaticLabelSeparatorDebug.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSBStart,
            this.TSBPause,
            this.TSSeparator1,
            this.TSPStepInto,
            this.TspStepOver,
            this.TSBStepOut});
            this.toolStrip1.Location = new System.Drawing.Point(8, 36);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(540, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TSBStart
            // 
            this.TSBStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSBStart.Image = ((System.Drawing.Image)(resources.GetObject("TSBStart.Image")));
            this.TSBStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBStart.Name = "TSBStart";
            this.TSBStart.Size = new System.Drawing.Size(35, 22);
            this.TSBStart.Text = "Start";
            // 
            // TSBPause
            // 
            this.TSBPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSBPause.Image = ((System.Drawing.Image)(resources.GetObject("TSBPause.Image")));
            this.TSBPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBPause.Name = "TSBPause";
            this.TSBPause.Size = new System.Drawing.Size(42, 22);
            this.TSBPause.Text = "Pause";
            // 
            // TSSeparator1
            // 
            this.TSSeparator1.Name = "TSSeparator1";
            this.TSSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TSPStepInto
            // 
            this.TSPStepInto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSPStepInto.Image = ((System.Drawing.Image)(resources.GetObject("TSPStepInto.Image")));
            this.TSPStepInto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSPStepInto.Name = "TSPStepInto";
            this.TSPStepInto.Size = new System.Drawing.Size(58, 22);
            this.TSPStepInto.Text = "Step Into";
            // 
            // TspStepOver
            // 
            this.TspStepOver.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TspStepOver.Image = ((System.Drawing.Image)(resources.GetObject("TspStepOver.Image")));
            this.TspStepOver.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TspStepOver.Name = "TspStepOver";
            this.TspStepOver.Size = new System.Drawing.Size(62, 22);
            this.TspStepOver.Text = "Step Over";
            // 
            // TSBStepOut
            // 
            this.TSBStepOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSBStepOut.Image = ((System.Drawing.Image)(resources.GetObject("TSBStepOut.Image")));
            this.TSBStepOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBStepOut.Name = "TSBStepOut";
            this.TSBStepOut.Size = new System.Drawing.Size(57, 22);
            this.TSBStepOut.Text = "Step Out";
            // 
            // OFDInstance
            // 
            this.OFDInstance.DefaultExt = "arci";
            this.OFDInstance.SupportMultiDottedExtensions = true;
            this.OFDInstance.Title = "Open Instance";
            // 
            // SFDInstance
            // 
            this.SFDInstance.DefaultExt = "arci";
            this.SFDInstance.SupportMultiDottedExtensions = true;
            this.SFDInstance.Title = "Save Instance";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "IronArc";
            this.tabControl1.ResumeLayout(false);
            this.TabPageInstances.ResumeLayout(false);
            this.TabPageConsole.ResumeLayout(false);
            this.TabPageConsole.PerformLayout();
            this.TabPageDebug.ResumeLayout(false);
            this.TabPageDebug.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabPageInstances;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ColumnIndex;
        private System.Windows.Forms.ColumnHeader ColumnStatus;
        private System.Windows.Forms.ColumnHeader ColumnMemorySize;
        private System.Windows.Forms.ColumnHeader ColumnDiskfileSize;
        private System.Windows.Forms.Button ButtonLoadInstance;
        private System.Windows.Forms.Button ButtonSaveInstance;
        private System.Windows.Forms.Button ButtonRemoveInstance;
        private System.Windows.Forms.Button ButtonStartInstance;
        private System.Windows.Forms.Button ButtonAddInstance;
        private System.Windows.Forms.TabPage TabPageConsole;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label StaticLabelSeparator;
        private System.Windows.Forms.ComboBox ComboInstanceSelector;
        private System.Windows.Forms.Label StaticLabelInstance;
        private System.Windows.Forms.TabPage TabPageDebug;
        private System.Windows.Forms.Label StaticLabelSeparatorDebug;
        private System.Windows.Forms.ComboBox ComboBoxInstancesDebug;
        private System.Windows.Forms.Label StaticLabelInstanceDebug;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton TSBStart;
        private System.Windows.Forms.ToolStripButton TSBPause;
        private System.Windows.Forms.ToolStripSeparator TSSeparator1;
        private System.Windows.Forms.ToolStripButton TSPStepInto;
        private System.Windows.Forms.ToolStripButton TspStepOver;
        private System.Windows.Forms.ToolStripButton TSBStepOut;
        private System.Windows.Forms.OpenFileDialog OFDInstance;
        private System.Windows.Forms.SaveFileDialog SFDInstance;
    }
}

