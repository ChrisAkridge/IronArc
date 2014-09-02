using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HexControlLibrary
{
    public class HexViewModel
    {
        private int currentColumn = 0;
        private int currentRow = 0;

        private int bytesPerColumn = 1;
        private int columnsPerRow = 8;
        private bool drawLegend = true;
        private int bytesTotal = 0;

        private IByteProvider byteProvider = new DefaultBytePrivider();

        [Category("Appearance")]
        [DefaultValue(1)]
        public int BytesPerColumn
        {
            get
            {
                return bytesPerColumn;
            }
            set
            {
                if (value < 0 || value > 255)
                {
                    return;
                }
                bytesPerColumn = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(8)]
        public int ColumnsPerRow
        {
            get
            {
                return columnsPerRow;
            }
            set
            {
                if (value < 1 || value > 255)
                {
                    return;
                }
                columnsPerRow = value;
            }
        }

        [Category("Appearance")]
        public int MaxRows { get { return bytesTotal / (bytesPerColumn * columnsPerRow); } }

        [Category("Appearance")]
        public int MaxColumns { get { return columnsPerRow; } }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool DrawLegend
        {
            get
            {
                return drawLegend;
            }
            set
            {
                drawLegend = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IByteProvider ByteProvider
        {
            get
            {
                return byteProvider;
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    return;
                }
                byteProvider = value;
                bytesTotal = value.Length;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Row
        {
            get
            {
                return currentRow;
            }
            set
            {
                if (value < 0 || value > MaxRows)
                {
                    return;
                }
                currentRow = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Column
        {
            get
            {
                return currentColumn;
            }
            set
            {
                if (value < 0 || value > MaxColumns)
                {
                    return;
                }
                currentColumn = value;
            }
        }
    }
}
