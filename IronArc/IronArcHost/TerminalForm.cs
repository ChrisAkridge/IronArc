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
using IronArc.Hardware;

namespace IronArcHost
{
	public partial class TerminalForm : Form, ITerminal
	{
		public Guid MachineID { get; set; }

		public TerminalForm()
		{
			InitializeComponent();
		}

		public void Write(string text)
		{
			
			if (TextTerminalWindow.InvokeRequired)
			{
				TextTerminalWindow.Invoke(new MethodInvoker(delegate
				{
					TextTerminalWindow.AppendText(text);
				}));
			}
			else
			{
				TextTerminalWindow.AppendText(text);
			}
		}

		public void WriteLine(string text)
		{
			Write(string.Concat(text, Environment.NewLine));
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
