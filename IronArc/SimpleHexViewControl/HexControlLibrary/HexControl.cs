﻿using System;
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

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.StandardDoubleClick, false);
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
				hexView1.Model.Column = 0;
				hScrollBar1.Minimum = 0;
				hScrollBar1.Value = 0;
				hScrollBar1.Maximum = hexView1.Model.MaxColumns - visibleColumns;
				hScrollBar1.Visible = true;
            }
            else
            {
				hScrollBar1.Visible = false;
            }
            if (visibleRows < hexView1.Model.MaxRows)
            {
				hexView1.Model.Row = 0;
				vScrollBar1.Minimum = 0;
				vScrollBar1.Value = 0;
				vScrollBar1.Maximum = hexView1.Model.MaxRows - visibleRows;
				vScrollBar1.Visible = true;
            }
            else
            {
				vScrollBar1.Visible = false;
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
			hexView1.Model.Row += e.NewValue - e.OldValue;
			hexView1.Invalidate();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
			hexView1.Model.Column += e.NewValue - e.OldValue;
			hexView1.Invalidate();
        }

        public void DrawBlock(int byteIndex)
        {
            hexView1.DrawBlock(byteIndex);
        }
    }
}