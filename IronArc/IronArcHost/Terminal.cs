using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronArcHost
{
	public partial class Terminal : Form
	{
		public Terminal()
		{
			InitializeComponent();
		}

		private void CMISaveOutput_Click(object sender, EventArgs e)
		{
			if (SFDSaveOutput.ShowDialog() == DialogResult.OK)
			{
				string filePath = SFDSaveOutput.FileName;
				File.WriteAllText(filePath, TextTerminalWindow.Text);
			}
		}
	}
}
