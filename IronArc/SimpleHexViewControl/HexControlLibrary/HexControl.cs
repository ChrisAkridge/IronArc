using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HexControlLibrary
{
    public partial class HexControl : UserControl
    {
        public HexControl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
        }

        void HexControl_FontChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        void HexControl_Resize(object sender, EventArgs e)
        {
            UpdateView();
        }

        public void UpdateView()
        {
            hexView1.UpdateModel();
            int visibleColumns = hexView1.GetVisibleColumns();
            int visibleRows = hexView1.GetVisibleRows();
            if (visibleColumns < hexView1.Model.MaxColumns)
            {
                this.hexView1.Model.Column = 0;
                this.hScrollBar1.Minimum = 0;
                this.hScrollBar1.Value = 0;
                this.hScrollBar1.Maximum = hexView1.Model.MaxColumns - visibleColumns;
                this.hScrollBar1.Visible = true;
            }
            else
            {
                this.hScrollBar1.Visible = false;
            }
            if (visibleRows < hexView1.Model.MaxRows)
            {
                this.hexView1.Model.Row = 0;
                this.vScrollBar1.Minimum = 0;
                this.vScrollBar1.Value = 0;
                this.vScrollBar1.Maximum = hexView1.Model.MaxRows - visibleRows;
                this.vScrollBar1.Visible = true;
            }
            else
            {
                this.vScrollBar1.Visible = false;
            }
            hexView1.Invalidate();
        }


        [Category("Appearance")]
        [DefaultValue(1)]
        public int BytesPerColumn
        {
            get
            {
                return hexView1.Model.BytesPerColumn;
            }
            set
            {
                if (value < 0 || value > 255)
                {
                    return;
                }
                hexView1.Model.BytesPerColumn = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(8)]
        public int ColumnsPerRow
        {
            get
            {
                return hexView1.Model.ColumnsPerRow;
            }
            set
            {
                if (value < 1 || value > 255)
                {
                    return;
                }
                hexView1.Model.ColumnsPerRow = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HexViewModel Model
        {
            get
            {
                return hexView1.Model;
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.hexView1.Model.Row += e.NewValue - e.OldValue;
            this.hexView1.Invalidate();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.hexView1.Model.Column += e.NewValue - e.OldValue;
            this.hexView1.Invalidate();
        }

        public void DrawBlock(int byteIndex)
        {
            hexView1.DrawBlock(byteIndex);
        }
    }
}
