using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronArc.Hardware.Configuration;

namespace IronArcHost
{
    public partial class AddHardwareConfigurationForm : Form
    {
        public HardwareConfiguration Configuration { get; }
        
        public AddHardwareConfigurationForm(HardwareConfiguration configuration)
        {
            InitializeComponent();

            Configuration = configuration;
            PropertyHardware.SelectedObject = configuration;
        }

        private void AddHardwareConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
