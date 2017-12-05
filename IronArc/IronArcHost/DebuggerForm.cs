using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronArcHost
{
	public partial class DebuggerForm : Form
	{
		public DebuggerForm()
		{
			InitializeComponent();
		}

		private void DebuggerForm_Load(object sender, EventArgs e)
		{
		}

		private void LinkLabelEAX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			new RegisterEditor().ShowDialog();
		}

		private void LinkEBX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkECX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEDX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEEX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEFX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEGX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEHX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkIP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEBASE_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void LinkEFLAGS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{

		}

		private void TextIP_TextChanged(object sender, EventArgs e)
		{

		}

		private void HexMemoryViewer_Load(object sender, EventArgs e)
		{

		}
	}
}
