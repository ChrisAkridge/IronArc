using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronArc.OSDebug.Storage;

namespace IronArcHost
{
    public partial class FormatStorageFileOptions : Form
    {
        private readonly StorageFileManager fileManager;
        
        public FormatStorageFileOptions(StorageFileManager fileManager)
        {
            this.fileManager = fileManager;
            InitializeComponent();
        }

        private async void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            ButtonOK.Enabled = false;
            ButtonCancel.Enabled = false;

            ProgressOverwrite.Maximum = (int)(fileManager.FileSize / fileManager.SectorSize);
            
            var progress = new Progress<long>();
            progress.ProgressChanged += (s, p) => ProgressOverwrite.Invoke((MethodInvoker)(() => ProgressOverwrite.Value = (int)p));

            await Task.Run(() => fileManager.FormatEntireFile(int.Parse(ComboSectorSize.Text),
                CheckOverwrite.Checked,
                progress));

            await Task.Delay(1000);
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            GroupOverwriteProgress.Visible = CheckOverwrite.Checked;
            Size = new Size(Size.Width,
                198 + (CheckOverwrite.Checked ? 77 : 0));
        }
    }
}
