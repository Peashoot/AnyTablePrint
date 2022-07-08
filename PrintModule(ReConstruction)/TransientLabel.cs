using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PrintModule_ReConstruction_
{
    public partial class TransientLabel : Control
    {
        public TransientLabel()
        {
            InitializeComponent();
            BackColor = Color.Black;
            Font = new Font(Font.FontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
        }
        /// <summary>
        /// 不执行大小改变的事件
        /// </summary>
        private bool DisableOnSizeChange = false;
        /// <summary>
        /// 之前的宽度
        /// </summary>
        private int OldWidth;

        protected override void OnPaint(PaintEventArgs pe)
        {
            // 计算字符串长度，修正控件大小
            var sizef = pe.Graphics.MeasureString(Text, Font);
            DisableOnSizeChange = true;
            Width = (int)Math.Ceiling(sizef.Width);
            Height = (int)Math.Ceiling(sizef.Height);
            OldWidth = Width;
            DisableOnSizeChange = false;
            GraphicsPath gp = new GraphicsPath();
            // 绘制字符串
            gp.AddString(Text, Font.FontFamily, (int)Font.Style, Font.Size * pe.Graphics.DpiX / 75,
                new Rectangle(0, 0, Width, Height), new StringFormat(StringFormat.GenericDefault)
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            Region = new Region(gp);
            base.OnPaint(pe);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (DisableOnSizeChange || OldWidth == 0) { return; }
            Font = new Font(Font.FontFamily, Font.Size * Graphics.FromHwnd(Handle).DpiX / 98 * Width / OldWidth, Font.Style);
            base.OnSizeChanged(e);
            OldWidth = Width;
        }
    }
}
