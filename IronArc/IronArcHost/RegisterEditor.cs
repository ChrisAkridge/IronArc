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
			withinSetValuesCall = true;
			switch (textBox)
			{
				case RegisterEditorTextBoxes.Value:
					ulong value;
					if (!ulong.TryParse(TextValue.Text, NumberStyles.HexNumber, null, out value)) { goto cleanup; }

					SetBigEndianValue(value);
					SetSigned(value);
					SetUnsigned(value);
					SetFloatingPoint(value);
					break;
				case RegisterEditorTextBoxes.BigEndianValue:
					ulong valueBigEndian = 0UL;
					if (!ulong.TryParse(BigEndianToLittleEndian(TextAsBigEndian.Text.PadLeft(16, '0')), NumberStyles.HexNumber, null, out valueBigEndian)) { goto cleanup; }

					SetValue(valueBigEndian);
					SetSigned(valueBigEndian);
					SetUnsigned(valueBigEndian);
					SetFloatingPoint(valueBigEndian);
					break;
				case RegisterEditorTextBoxes.Signed:
					long valueSigned = 0L;
					if (!long.TryParse(TextSigned.Text, out valueSigned)) { goto cleanup; }

					unchecked
					{
						SetValue((ulong)valueSigned);
						SetBigEndianValue((ulong)valueSigned);
						SetUnsigned((ulong)valueSigned);
						SetFloatingPoint((ulong)valueSigned);
					}
					break;
				case RegisterEditorTextBoxes.Unsigned:
					ulong valueUnsigned = 0UL;
					if (!ulong.TryParse(TextUnsigned.Text, out valueUnsigned)) { goto cleanup; }

					SetValue(valueUnsigned);
					SetBigEndianValue(valueUnsigned);
					SetSigned(valueUnsigned);
					SetFloatingPoint(valueUnsigned);
					break;
				case RegisterEditorTextBoxes.Floating:
					double valueFloating = 0d;
					if (!double.TryParse(TextFloatingPoint.Text, out valueFloating)) { goto cleanup; }
					ulong valueFloatingAsULong = unchecked((ulong)BitConverter.DoubleToInt64Bits(valueFloating));

					SetValue(valueFloatingAsULong);
					SetBigEndianValue(valueFloatingAsULong);
					SetSigned(valueFloatingAsULong);
					SetUnsigned(valueFloatingAsULong);
					break;
				default:
					break;
			}

			cleanup:
			withinSetValuesCall = false;
		}

		private void SetValue(ulong value)
		{
			TextValue.Text = string.Format("{0:X16}", value);
		}

		private void SetBigEndianValue(ulong value)
		{
			StringBuilder result = new StringBuilder(16);
			byte[] bytes = BitConverter.GetBytes(value);
			foreach (byte b in bytes)
			{
				result.Append(b.ToString("X2"));
			}
			TextAsBigEndian.Text = result.ToString();
		}

		public void SetSigned(ulong value)
		{
			TextSigned.Text = string.Format("{0}", unchecked((long)value));
		}
		
		public void SetUnsigned(ulong value)
		{
			TextUnsigned.Text = value.ToString();
		}

		public void SetFloatingPoint(ulong value)
		{
			double result = BitConverter.Int64BitsToDouble(unchecked((long)value));
			TextFloatingPoint.Text = result.ToString();
		}

		private void TextValue_TextChanged(object sender, EventArgs e)
		{
			if (!withinSetValuesCall)
			{
				// Verify that the string contains only hexadecimal digits.
				TextValue.Text = RemoveInvalidCharacters(TextValue.Text, RemoveCharactersFor.Hexadecimal);

				// Verify that the string has up to 16 characters.
				if (TextValue.Text.Length > 16)
				{
					TextValue.Text = TextValue.Text.Substring(0, 16);
				}

				SetValues(RegisterEditorTextBoxes.Value);
			}
		}

		private void TextAsBigEndian_TextChanged(object sender, EventArgs e)
		{
			if (!withinSetValuesCall)
			{
				TextAsBigEndian.Text = RemoveInvalidCharacters(TextAsBigEndian.Text, RemoveCharactersFor.Hexadecimal);

				if (TextAsBigEndian.Text.Length > 16)
				{
					TextAsBigEndian.Text = TextAsBigEndian.Text.Substring(0, 16);
				}

				SetValues(RegisterEditorTextBoxes.BigEndianValue);
			}
		}

		private void TextSigned_TextChanged(object sender, EventArgs e)
		{
			if (!withinSetValuesCall)
			{
				TextSigned.Text = RemoveInvalidCharacters(TextSigned.Text, RemoveCharactersFor.SignedDecimal);

				if (TextSigned.Text.Length > 20)
				{
					TextSigned.Text = TextSigned.Text.Substring(0, 20);
				}

				SetValues(RegisterEditorTextBoxes.Signed);
			}
		}

		private void TextUnsigned_TextChanged(object sender, EventArgs e)
		{
			if (!withinSetValuesCall)
			{
				// where you left off: fix below line, finish the floating point textchanged thing, 
				// add a public method to get the ulong that the register should be set to
				TextUnsigned.Text = RemoveInvalidCharacters(TextUnsigned.Text, RemoveCharactersFor.UnsignedDecimal);

				if (TextUnsigned.Text.Length > 20)
				{
					TextUnsigned.Text = TextUnsigned.Text.Substring(0, 20);
				}

				SetValues(RegisterEditorTextBoxes.Unsigned);
			}
		}

		private void TextFloatingPoint_TextChanged(object sender, EventArgs e)
		{
			if (!withinSetValuesCall)
			{
				TextFloatingPoint.Text = RemoveInvalidCharacters(TextFloatingPoint.Text, RemoveCharactersFor.FloatingPoint);

				SetValues(RegisterEditorTextBoxes.Floating);
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
					validCharacters = "0123456789.+-fnaite";
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
