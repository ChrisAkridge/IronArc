using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HexControlLibrary;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private PrivateFontCollection fonts = new PrivateFontCollection();

        public Form1()
        {
            InitializeComponent();

            fonts.AddFontFile("fonts//consola.ttf");
            fonts.AddFontFile("fonts//consolab.ttf");
            fonts.AddFontFile("fonts//consolai.ttf");
            fonts.AddFontFile("fonts//consolaz.ttf");

            this.Font = new Font(fonts.Families[0], 8.25f, FontStyle.Regular);

            hexControl1.Model.ByteProvider = new ByteProvider();
            hexControl1.UpdateView();
        }
    }

    class ByteProvider : IByteProvider
    {
        public readonly byte[] bytes = new byte[0xffffff];

        public byte GetByte(int offset)
        {
            return bytes[offset];
        }

        public int Length
        {
            get { return bytes.Length; }
        }
    }
}
