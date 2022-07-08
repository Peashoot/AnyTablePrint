using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    public partial class AdjustmentFrame : Control
    {
        public AdjustmentFrame()
        {
            InitializeComponent();
            TabStop = false;
            Height = 200;
            Width = 200;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private float rectSide = 2;

        private void DrawText()
        {
            Invalidate();
            using (Graphics graphics = CreateGraphics())
            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                // 左上 0,0
                // 左中 0,(height-side)/2
                // 左下 0,height-side
                // 中上 (width-side)/2,0
                // 中下 (width-side)/2,height-side
                // 右上 width-side,0
                // 右中 width-side,(height-side)/2
                // 右下 width-side,height-side
                // 上1 
                // 上2
                // 左1
                // 左2
                // 右1
                // 右2
                // 下1
                // 下2
            }
        }
    }
}
