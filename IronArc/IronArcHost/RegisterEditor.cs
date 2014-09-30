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
		private bool withinSetValuesCall = false;

		public RegisterEditor()
		{
			InitializeComponent();
		}

		private void SetValues(RegisterEditorTextBoxes textBox)
		{
			this.withinSetValuesCall = true;
			switch (textBox)
			{
				case RegisterEditorTextBoxes.Value:
					ulong value;
					if (!ulong.TryParse(this.TextValue.Text, NumberStyles.HexNumber, null, out value)) { goto cleanup; }

					this.SetBigEndianValue(value);
					this.SetSigned(value);
					this.SetUnsigned(value);
					this.SetFloatingPoint(value);
					break;
				case RegisterEditorTextBoxes.BigEndianValue:
					ulong valueBigEndian = 0UL;
					if (!ulong.TryParse(this.BigEndianToLittleEndian(this.TextAsBigEndian.Text.PadLeft(16, '0')), NumberStyles.HexNumber, null, out valueBigEndian)) { goto cleanup; }

					this.SetValue(valueBigEndian);
					this.SetSigned(valueBigEndian);
					this.SetUnsigned(valueBigEndian);
					this.SetFloatingPoint(valueBigEndian);
					break;
				case RegisterEditorTextBoxes.Signed:
					long valueSigned = 0L;
					if (!long.TryParse(this.TextSigned.Text, out valueSigned)) { goto cleanup; }

					unchecked
					{
						this.SetValue((ulong)valueSigned);
						this.SetBigEndianValue((ulong)valueSigned);
						this.SetUnsigned((ulong)valueSigned);
						this.SetFloatingPoint((ulong)valueSigned);
					}
					break;
				case RegisterEditorTextBoxes.Unsigned:
					ulong valueUnsigned = 0UL;
					if (!ulong.TryParse(this.TextUnsigned.Text, out valueUnsigned)) { goto cleanup; }

					this.SetValue(valueUnsigned);
					this.SetBigEndianValue(valueUnsigned);
					this.SetSigned(valueUnsigned);
					this.SetFloatingPoint(valueUnsigned);
					break;
				case RegisterEditorTextBoxes.Floating:
					break;
				default:
					break;
			}

			cleanup:
			this.withinSetValuesCall = false;
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
			if (!this.withinSetValuesCall)
			{
				// Verify that the string contains only hexadecimal digits.
				this.TextValue.Text = this.RemoveInvalidCharacters(this.TextValue.Text, RemoveCharactersFor.Hexadecimal);

				// Verify that the string has up to 16 characters.
				if (this.TextValue.Text.Length > 16)
				{
					this.TextValue.Text = this.TextValue.Text.Substring(0, 16);
				}

				this.SetValues(RegisterEditorTextBoxes.Value);
			}
		}

		private void TextAsBigEndian_TextChanged(object sender, EventArgs e)
		{
			if (!this.withinSetValuesCall)
			{
				this.TextAsBigEndian.Text = this.RemoveInvalidCharacters(this.TextAsBigEndian.Text, RemoveCharactersFor.Hexadecimal);

				if (this.TextAsBigEndian.Text.Length > 16)
				{
					this.TextAsBigEndian.Text = this.TextAsBigEndian.Text.Substring(0, 16);
				}

				this.SetValues(RegisterEditorTextBoxes.BigEndianValue);
			}
		}

		private void TextSigned_TextChanged(object sender, EventArgs e)
		{
			if (!this.withinSetValuesCall)
			{
				this.TextSigned.Text = this.RemoveInvalidCharacters(this.TextSigned.Text, RemoveCharactersFor.SignedDecimal);

				if (this.TextSigned.Text.Length > 20)
				{
					this.TextSigned.Text = this.TextSigned.Text.Substring(0, 20);
				}

				this.SetValues(RegisterEditorTextBoxes.Signed);
			}
		}

		private void TextUnsigned_TextChanged(object sender, EventArgs e)
		{
			if (!this.withinSetValuesCall)
			{
				// where you left off: fix below line, finish the floating point textchanged thing, 
				// add a public method to get the ulong that the register should be set to
				this.TextUnsigned.Text = this.RemoveInvalidCharacters(this.TextUnsigned.Text, isHexadecimal: false, isSigned: false);

				if (this.TextUnsigned.Text.Length > 20)
				{
					this.TextUnsigned.Text = this.TextUnsigned.Text.Substring(0, 20);
				}

				this.SetValues(RegisterEditorTextBoxes.Unsigned);
			}
		}

		private void TextFloatingPoint_TextChanged(object sender, EventArgs e)
		{
			if (!this.withinSetValuesCall)
			{
				this.TextFloatingPoint.Text = 
			}
		}

		private string RemoveInvalidCharacters(string input, RemoveCharactersFor removeFor)
		{
			string validCharacters = "";
			switch (removeFor)
			{
				case RemoveCharactersFor.SignedDecimal:
					validCharacters = "0123456789-";
					break;
				case RemoveCharactersFor.UnsignedDecimal:
					validCharacters = "0123456789";
					break;
				case RemoveCharactersFor.Hexadecimal:
					validCharacters = "0123456789abcdef";
					break;
				case RemoveCharactersFor.FloatingPoint:
					validCharacters = "0123456789.-fnaininite";
					break;
				default:
					break;
			}

			if (!input.ToLower().All(c => validCharacters.Contains(c)))
			{
				input = new string(input.ToLower().Where(c => validCharacters.Contains(c)).ToArray());
			}

			return input;
		}

		private string BigEndianToLittleEndian(string bigEndian)
		{
			StringBuilder littleEndianBuilder = new StringBuilder();

			for (int i = 7; i >= 0; i--)
			{
				littleEndianBuilder.Append(bigEndian.Substring(i * 2, 2));
			}

			return littleEndianBuilder.ToString();
		}
	}

	internal enum RegisterEditorTextBoxes
	{
		Invalid,
		Value,
		BigEndianValue,
		Signed,
		Unsigned,
		Floating
	}

	internal enum RemoveCharactersFor
	{
		SignedDecimal,
		UnsignedDecimal,
		Hexadecimal,
		FloatingPoint
	}
}
