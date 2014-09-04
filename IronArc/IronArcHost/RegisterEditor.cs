using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronArcHost
{
	public partial class RegisterEditor : Form
	{
		public RegisterEditor()
		{
			InitializeComponent();
		}

		private void SetValues(RegisterEditorTextBoxes textBox)
		{
			switch (textBox)
			{
				case RegisterEditorTextBoxes.Value:
					ulong value;
					if (!ulong.TryParse(this.TextValue.Text, NumberStyles.HexNumber, null, out value)) { return; }

					this.SetBigEndianValue(value);
					this.SetSigned(value);
					this.SetUnsigned(value);
					this.SetFloatingPoint(value);
					break;
				case RegisterEditorTextBoxes.BigEndianValue:
					break;
				case RegisterEditorTextBoxes.Signed:
					break;
				case RegisterEditorTextBoxes.Unsigned:
					break;
				case RegisterEditorTextBoxes.Floating:
					break;
				default:
					break;
			}
		}

		private void SetValue(ulong value)
		{
			this.TextValue.Text = string.Format("{0:X16}", value);
		}

		private void SetBigEndianValue(ulong value)
		{
			StringBuilder result = new StringBuilder(16);
			byte[] bytes = BitConverter.GetBytes(value);
			foreach (byte b in bytes)
			{
				result.Append(b.ToString("X2"));
			}
			this.TextAsBigEndian.Text = result.ToString();
		}

		public void SetSigned(ulong value)
		{
			this.TextSigned.Text = string.Format("{0}", unchecked((long)value));
		}
		
		public void SetUnsigned(ulong value)
		{
			this.TextUnsigned.Text = value.ToString();
		}

		public void SetFloatingPoint(ulong value)
		{
			double result = BitConverter.Int64BitsToDouble(unchecked((long)value));
			this.TextFloatingPoint.Text = result.ToString();
		}

		private void TextValue_TextChanged(object sender, EventArgs e)
		{
			// Verify that the string contains only hexadecimal digits.
			if(!this.TextValue.Text.ToLower().All(c => "0123456789abcdef".Contains(c)))
			{
				this.TextValue.Text = new string(this.TextValue.Text.ToLower().Where(c => "0123456789abcdef".Contains(c)).ToArray());
			}

			// Verify that the string has up to 16 characters.
			if (this.TextValue.Text.Length > 16)
			{
				this.TextValue.Text = this.TextValue.Text.Substring(0, 16);
			}

			this.SetValues(RegisterEditorTextBoxes.Value);
		}

		private void TextAsBigEndian_TextChanged(object sender, EventArgs e)
		{

		}

		private void TextSigned_TextChanged(object sender, EventArgs e)
		{

		}

		private void TextUnsigned_TextChanged(object sender, EventArgs e)
		{

		}

		private void TextFloatingPoint_TextChanged(object sender, EventArgs e)
		{

		}
	}

	internal enum RegisterEditorTextBoxes
	{
		Value,
		BigEndianValue,
		Signed,
		Unsigned,
		Floating
	}
}
