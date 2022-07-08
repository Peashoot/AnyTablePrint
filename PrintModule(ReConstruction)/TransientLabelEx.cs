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
    public partial class TransientLabelEx : Control
    {
        public TransientLabelEx()
        {
            BackColor = Color.Black;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
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
    }
}
