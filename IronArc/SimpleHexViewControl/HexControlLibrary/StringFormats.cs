using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HexControlLibrary
{
    public static class StringFormats
    {
        public static readonly StringFormat Default = new StringFormat(StringFormat.GenericTypographic) { FormatFlags = StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoClip | StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.FitBlackBox };
    }
}
