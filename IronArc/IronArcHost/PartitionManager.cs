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
using IronArc.OSDebug.Storage.Model;
using Exception = System.Exception;

namespace IronArcHost
{
    public partial class PartitionManager : Form
    {
        private StorageFileManager fileManager;
        
        public PartitionManager()
        {
            InitializeComponent();
        }

        private void ButtonChooseFile_Click(object sender, EventArgs e)
        {
            if (OFDChooseFile.ShowDialog() != DialogResult.OK) { return; }

            TextFilePath.Text = OFDChooseFile.FileName;
        }

        private void ButtonRemovePartition_Click(object sender, EventArgs e)
        {
            fileManager.RemovePartition(ListFileSections.SelectedIndices[0]);
        }

        private void ButtonLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                fileManager.DivisionListUpdated -= FileManager_OnDivisionListUpdated;
                fileManager?.Dispose();

                fileManager.DivisionListUpdated += FileManager_OnDivisionListUpdated;
                fileManager = new StorageFileManager(OFDChooseFile.FileName);

                ListFileSections.Enabled = true;
                PopulateFileSections();
            }
            catch (Exception ex)
            {
                fileManager = null;
                
                ListFileSections.Items.Clear();

                MessageBox.Show($"Failed to load the IronArc storage device file.\r\n\r\n{ex.Message}",
                    "IronArc - Partition Manager",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            SetControlsEnabledState();
        }

        private void FileManager_OnDivisionListUpdated(object sender, EventArgs e)
        {
            PopulateFileSections();
        }

        private void PopulateFileSections()
        {
            ListFileSections.Items.Clear();

            foreach (var division in fileManager.Divisions)
            {
                if (division is Partition partition)
                {
                    ListFileSections.Items.Add(new ListViewItem(new[]
                    {
                        "Partition", partition.ImplicitId.ToString(), partition.Name,
                        // TODO: Use ChrisAkridge.Common's size formatter here
                        $"{partition.Length:#,###} bytes"
                    }));
                }
                else
                {
                    var unallocatedSpace = division as UnallocatedSpace;

                    ListFileSections.Items.Add(new ListViewItem(new[]
                    {
                        "Unallocated", "", "", $"{unallocatedSpace.Length:#,###} bytes"
                    }));
                }
            }
        }

        private void SetControlsEnabledState()
        {
            var fileLoaded = fileManager != null;
            var sectionSelected = fileLoaded && ListFileSections.SelectedIndices.Count > 0;

            var selectedSection = sectionSelected
                ? GetDivisionAtIndex()
                : null;
            var selectedSectionIsPartition = selectedSection is Partition;
            
            ListFileSections.Enabled = fileLoaded;
            ButtonAllocateSection.Enabled = !selectedSectionIsPartition;
            ButtonFormatFile.Enabled = fileLoaded;
            ButtonRemovePartition.Enabled = selectedSectionIsPartition;
            ButtonResizePartition.Enabled = selectedSectionIsPartition;
        }

        private StorageFileDivision GetDivisionAtIndex() => fileManager.Divisions[ListFileSections.SelectedIndices[0]];

        private void ListFileSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlsEnabledState();
        }

        private void ButtonFormatFile_Click(object sender, EventArgs e) => new FormatStorageFileOptions(fileManager).ShowDialog();

        private void ButtonResizePartition_Click(object sender, EventArgs e)
        {
            
        }
    }
}
