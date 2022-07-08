using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{/// <summary>
    /// A label that can be transparent.
    /// </summary>
    public class TransparentLabelEx : Control
    {
        /// <summary>
        /// Creates a new <see cref="TransparentLabel"/> instance.
        /// </summary>
        public TransparentLabelEx()
        {
            TabStop = false;
            innerLabel = new TransientLabel();
            innerLabel.Size = new System.Drawing.Size(2, 2);
            innerLabel.Font = Font;
            Controls.Add(innerLabel);
            innerLabel.Show();
        }

        private TransientLabel innerLabel;

        /// <summary>
        /// Gets the creation parameters.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <returns>
        /// The text associated with this control.
        /// </returns>
        public override string Text
        {
            get
            {
                return innerLabel.Text;
            }
            set
            {
                innerLabel.Text = value;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (DisableOnSizeChange || OldWidth == 0) { return; }
            Font = new Font(Font.FontFamily, Font.Size * Graphics.FromHwnd(Handle).DpiX / 98 * Width / OldWidth, Font.Style);
            base.OnSizeChanged(e);
            OldWidth = Width;
            innerLabel.Width = Width;
            innerLabel.Height = Height;
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Drawing.Font"/> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"/> property.
        /// </returns>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return innerLabel.BackColor;
            }
            set
            {
                innerLabel.BackColor = value;
            }
        }
        /// <summary>
        /// 不执行大小改变的事件
        /// </summary>
        private bool DisableOnSizeChange = false;
        /// <summary>
        /// 之前的宽度
        /// </summary>
        private int OldWidth;

        protected override void OnPaint(PaintEventArgs e)
        {
            // 计算字符串长度，修正控件大小
            var sizef = e.Graphics.MeasureString(Text, Font);
            DisableOnSizeChange = true;
            Width = (int)Math.Ceiling(sizef.Width);
            Height = (int)Math.Ceiling(sizef.Height);
            OldWidth = Width;
            DisableOnSizeChange = false;
            base.OnPaint(e);
        }
    }
}
