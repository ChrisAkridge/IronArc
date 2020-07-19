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

        public ulong RegisterValue { get; private set; }

        public RegisterEditor(ulong registerValue)
        {
            InitializeComponent();
            RegisterValue = registerValue;

            SetValue(registerValue);
            SetBigEndianValue(registerValue);
            SetSigned(registerValue);
            SetUnsigned(registerValue);
            SetFloatingPoint(registerValue);
        }

        private void SetValuesOfOtherTextboxes(RegisterEditorTextBoxes textBox)
        {
            withinSetValuesCall = true;
            switch (textBox)
            {
                case RegisterEditorTextBoxes.Value:
                    if (!ulong.TryParse(TextValue.Text, NumberStyles.HexNumber, null, out var value)) { goto cleanup; }
                    RegisterValue = value;

                    SetBigEndianValue(value);
                    SetSigned(value);
                    SetUnsigned(value);
                    SetFloatingPoint(value);
                    break;
                case RegisterEditorTextBoxes.BigEndianValue:
                    if (!ulong.TryParse(BigEndianToLittleEndian(TextAsBigEndian.Text.PadLeft(16, '0')), NumberStyles.HexNumber, null, out var valueBigEndian)) { goto cleanup; }
                    RegisterValue = valueBigEndian;

                    SetValue(valueBigEndian);
                    SetSigned(valueBigEndian);
                    SetUnsigned(valueBigEndian);
                    SetFloatingPoint(valueBigEndian);
                    break;
                case RegisterEditorTextBoxes.Signed:
                    if (!long.TryParse(TextSigned.Text, out var valueSigned)) { goto cleanup; }
                    RegisterValue = unchecked((ulong)valueSigned);

                    unchecked
                    {
                        SetValue((ulong)valueSigned);
                        SetBigEndianValue((ulong)valueSigned);
                        SetUnsigned((ulong)valueSigned);
                        SetFloatingPoint((ulong)valueSigned);
                    }
                    break;
                case RegisterEditorTextBoxes.Unsigned:
                    if (!ulong.TryParse(TextUnsigned.Text, out var valueUnsigned)) { goto cleanup; }
                    RegisterValue = valueUnsigned;

                    SetValue(valueUnsigned);
                    SetBigEndianValue(valueUnsigned);
                    SetSigned(valueUnsigned);
                    SetFloatingPoint(valueUnsigned);
                    break;
                case RegisterEditorTextBoxes.Floating:
                    if (!double.TryParse(TextFloatingPoint.Text, out var valueFloating)) { goto cleanup; }
                    ulong valueFloatingAsULong = unchecked((ulong)BitConverter.DoubleToInt64Bits(valueFloating));
                    RegisterValue = valueFloatingAsULong;

                    SetValue(valueFloatingAsULong);
                    SetBigEndianValue(valueFloatingAsULong);
                    SetSigned(valueFloatingAsULong);
                    SetUnsigned(valueFloatingAsULong);
                    break;
                case RegisterEditorTextBoxes.Invalid:
                    break;
                default:
                    break;
            }

            cleanup:
            withinSetValuesCall = false;
        }

        private void SetValue(ulong value)
        {
            TextValue.Text = $"{value:X16}";
        }

        private void SetBigEndianValue(ulong value)
        {
            var result = new StringBuilder(16);
            byte[] bytes = BitConverter.GetBytes(value);
            foreach (byte b in bytes)
            {
                result.Append(b.ToString("X2"));
            }
            TextAsBigEndian.Text = result.ToString();
        }

        public void SetSigned(ulong value)
        {
            TextSigned.Text = $"{unchecked((long)value)}";
        }

        public void SetUnsigned(ulong value)
        {
            TextUnsigned.Text = value.ToString();
        }

        public void SetFloatingPoint(ulong value)
        {
            double result = BitConverter.Int64BitsToDouble(unchecked((long)value));
            TextFloatingPoint.Text = result.ToString(CultureInfo.InvariantCulture);
        }

        private void TextValue_TextChanged(object sender, EventArgs e)
        {
            if (withinSetValuesCall) { return; }

            // Verify that the string contains only hexadecimal digits.
            TextValue.Text = RemoveInvalidCharacters(TextValue.Text, RemoveCharactersFor.Hexadecimal);

            // Verify that the string has up to 16 characters.
            if (TextValue.Text.Length > 16)
            {
                TextValue.Text = TextValue.Text.Substring(0, 16);
            }

            SetValuesOfOtherTextboxes(RegisterEditorTextBoxes.Value);
        }

        private void TextAsBigEndian_TextChanged(object sender, EventArgs e)
        {
            if (withinSetValuesCall) { return; }

            TextAsBigEndian.Text = RemoveInvalidCharacters(TextAsBigEndian.Text, RemoveCharactersFor.Hexadecimal);

            if (TextAsBigEndian.Text.Length > 16)
            {
                TextAsBigEndian.Text = TextAsBigEndian.Text.Substring(0, 16);
            }

            SetValuesOfOtherTextboxes(RegisterEditorTextBoxes.BigEndianValue);
        }

        private void TextSigned_TextChanged(object sender, EventArgs e)
        {
            if (withinSetValuesCall) { return; }

            TextSigned.Text = RemoveInvalidCharacters(TextSigned.Text, RemoveCharactersFor.SignedDecimal);

            if (TextSigned.Text.Length > 20)
            {
                TextSigned.Text = TextSigned.Text.Substring(0, 20);
            }

            SetValuesOfOtherTextboxes(RegisterEditorTextBoxes.Signed);
        }

        private void TextUnsigned_TextChanged(object sender, EventArgs e)
        {
            if (withinSetValuesCall) { return; }

            // WYLO: fix below line, finish the floating point textchanged thing,
            // add a public method to get the ulong that the register should be set to
            TextUnsigned.Text = RemoveInvalidCharacters(TextUnsigned.Text, RemoveCharactersFor.UnsignedDecimal);

            if (TextUnsigned.Text.Length > 20)
            {
                TextUnsigned.Text = TextUnsigned.Text.Substring(0, 20);
            }

            SetValuesOfOtherTextboxes(RegisterEditorTextBoxes.Unsigned);
        }

        private void TextFloatingPoint_TextChanged(object sender, EventArgs e)
        {
            if (withinSetValuesCall) { return; }

            TextFloatingPoint.Text = RemoveInvalidCharacters(TextFloatingPoint.Text, RemoveCharactersFor.FloatingPoint);

            if (TextFloatingPoint.Text.Length > 20)
            {
                TextFloatingPoint.Text = TextFloatingPoint.Text.Substring(0, 20);
            }

            SetValuesOfOtherTextboxes(RegisterEditorTextBoxes.Floating);
        }

        private static string RemoveInvalidCharacters(string input, RemoveCharactersFor removeFor)
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
                    validCharacters = "0123456789.+-aefint";
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

        private static string BigEndianToLittleEndian(string bigEndian)
        {
            var littleEndianBuilder = new StringBuilder();

            for (int i = 7; i >= 0; i--)
            {
                littleEndianBuilder.Append(bigEndian.Substring(i * 2, 2));
            }

            return littleEndianBuilder.ToString();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
