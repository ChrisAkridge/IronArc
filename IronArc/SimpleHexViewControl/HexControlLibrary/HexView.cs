using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HexControlLibrary
{
    [DesignTimeVisible(false)]
    public sealed partial class HexView : Control
    {
        private HexViewModel model;

        private SizeF legend;
        private SizeF block;

        public int GetVisibleColumns()
        {
            int columns = 0;
            float width = legend.Width;
            for (int i = 0; i < model.ColumnsPerRow; i++)
            {
                if (width + block.Width > this.ClientSize.Width)
                {
                    break;
                }
                width += block.Width;
                columns++;
            }
            return columns;
        }

        public int GetVisibleRows()
        {
            int rows = 0;
            float height = 0;
            for (int i = 0; i < model.MaxRows; i++)
            {
                if (height + block.Height > this.ClientSize.Height)
                {
                    break;
                }
                height += block.Height;
                rows++;
            }
            return rows;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HexViewModel Model
        {
            get
            {
                return model;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!DesignMode)
            {
                SetRenderingOptions(e.Graphics);
                DrawRows(e.Graphics, model.Row, model.Column);
            }
            base.OnPaint(e);
        }

        private void DrawRows(Graphics g, int row, int col)
        {
            float offset = 0;
            if (model.DrawLegend)
            {
                offset = legend.Width;
            }
            SolidBrush foreBrush = new SolidBrush(this.ForeColor);
            for (int i = row; i < model.MaxRows; i++)
            {
                if ((i - row) * block.Height > this.ClientRectangle.Height)
                {
                    break;
                }
                if (model.DrawLegend)
                {
                    DrawLegend(g, foreBrush, i, row);
                }
                for (int j = col; j < model.MaxColumns; j++)
                {
                    if (offset + (j - col) * block.Width > this.ClientRectangle.Width)
                    {
                        break;
                    }
                    DrawBlock(g, foreBrush, offset, i, j, row, col);
                }
            }
        }

        private void DrawLegend(Graphics g, SolidBrush foreBrush, int i, int row)
        {
            g.DrawString(GetLegend(i), this.Font, foreBrush, new RectangleF(0, (i - row) * block.Height, legend.Width, block.Height), StringFormats.Default);
        }

        private void DrawBlock(Graphics g, SolidBrush foreBrush, float offset, int i, int j, int row, int col)
        {
            RectangleF rect = new RectangleF(offset + (j - col) * block.Width, (i - row) * block.Height, block.Width, block.Height);
            g.DrawString(GetBlock(i, j), this.Font, foreBrush, rect, StringFormats.Default);
        }

        private void DrawBlock(Graphics g, SolidBrush backBrush, SolidBrush foreBrush, float offset, int i, int j, int row, int col)
        {
            RectangleF rect = new RectangleF(offset + (j - col) * block.Width, (i - row) * block.Height, block.Width, block.Height);
            g.FillRectangle(backBrush, rect);
            g.DrawString(GetBlock(i, j), this.Font, foreBrush, rect, StringFormats.Default);
        }

        private SizeF GetSize(Graphics g, string format)
        {
            SizeF result = new SizeF();
            foreach (char ch in chars)
            {
                SizeF current = g.MeasureString(string.Format(format, ch, delimiter), Font, 0, StringFormats.Default);
                if (current.Width > result.Width)
                {
                    result = current;
                }
            }
            return result;
        }

        private char delimiter = ' ';
        private char[] chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', 'c', 'd', 'e', 'f' };

        private SizeF DefaultLegend(Graphics g)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{0}{0}{0}{0}{0}{0}{0}{0}");
            return GetSize(g, sb.ToString());
        }

        private string GetLegend(int row)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:x8}", row * model.ColumnsPerRow * model.BytesPerColumn);
            return sb.ToString();
        }

        private SizeF DefaultBlock(Graphics g)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < model.BytesPerColumn; i++)
            {
                sb.Append("{0}{0}");
            }
            sb.Append("{1}");
            return GetSize(g, sb.ToString());
        }

        private string GetBlock(int row, int col)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0,1}", string.Empty);
            for (int i = 0; i < model.BytesPerColumn; i++)
            {
                sb.AppendFormat("{0:x2}", model.ByteProvider.GetByte((row * model.ColumnsPerRow + col) * model.BytesPerColumn + i));
            }
            return sb.ToString();
        }

        public HexView()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            this.UpdateStyles();

            this.BackColor = Color.White;
            this.FontChanged += new EventHandler(HexView_FontChanged);
            this.Resize += HexView_Resize;

            this.model = new HexViewModel();
        }

        void HexView_Resize(object sender, EventArgs e)
        {
            UpdateModel();
        }

        void HexView_FontChanged(object sender, EventArgs e)
        {
            UpdateModel();
        }

        public void UpdateModel()
        {
            using (Graphics g = this.CreateGraphics())
            {
                SetRenderingOptions(g);
                legend = DefaultLegend(g);
                block = DefaultBlock(g);
            }
        }

        private void GetRectangle(string legend, string block, Graphics g, out SizeF c, out SizeF b)
        {
            c = g.MeasureString(legend, Font, 0, StringFormats.Default);
            b = g.MeasureString(block, Font, 0, StringFormats.Default);
        }

        private static void SetRenderingOptions(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.TextContrast = 0;
        }

        public void DrawBlock(int byteIndex)
        {
            int maxColumns = model.MaxColumns;
            int maxRows = model.MaxRows;
            int modelRow = model.Row;
            int modelColumn = model.Column;

            bool drawLegend = model.DrawLegend;

            int currentIndex = byteIndex / model.BytesPerColumn;
            int currentRow = currentIndex / maxColumns;
            int currentColumn = currentIndex % maxColumns;

            if (currentRow >= modelRow && currentRow <= modelRow + GetVisibleRows() && currentColumn >= modelColumn && currentColumn <= modelColumn + GetVisibleColumns())
            {
                int row = currentIndex / maxColumns;
                int col = currentIndex % maxColumns;
                SolidBrush foreBrush = new SolidBrush(ForeColor);
                SolidBrush backBrush = new SolidBrush(BackColor);
                using (Graphics g = this.CreateGraphics())
                {
                    SetRenderingOptions(g);
                    if (drawLegend)
                    {
                        DrawBlock(g, backBrush, foreBrush, legend.Width, row, col, modelRow, modelColumn);
                    }
                    else
                    {
                        DrawBlock(g, backBrush, foreBrush, 0, row, col, modelRow, modelColumn);
                    }
                }
            }
        }
    }
}
