using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    public class PrintPreviewLabel : TransparentLabel, IPrintPreviewControl
    {
        #region 参数

        /// <summary>
        /// 拖动前鼠标的位置
        /// </summary>
        public Point BeforeLoc { get; set; }

        /// <summary>
        /// 拖动后鼠标的位置
        /// </summary>
        public Point AfterLoc { get; set; }

        /// <summary>
        /// 重新设置模式
        /// </summary>
        public bool ResetMode { get; set; }

        /// <summary>
        /// 控件拖动
        /// </summary>
        public bool MoveLock { get; set; }

        /// <summary>
        /// 控件缩放
        /// </summary>
        public bool ExpandLock { get; set; }

        /// <summary>
        /// 缩放数组（表示缩放的类型）
        /// </summary>
        public int[] ExpandArray { get; set; }

        /// <summary>
        /// 控件的最小宽度
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>
        /// 控件的最小高度
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// 所属的Panel
        /// </summary>
        public Panel BelongPanel { get; set; }

        /// <summary>
        /// 右键菜单
        /// </summary>
        public ContextMenuStrip MenuStrip { get; set; }

        /// <summary>
        /// 导入信息
        /// </summary>
        public ExportInfo Exinfo { get; set; }

        #endregion 参数

        public PrintPreviewLabel(Panel panel, ExportInfo exinfo = null)
            : base()
        {
            try
            {
                BelongPanel = panel;
                ExpandArray = new int[4];
                MenuStrip = new ControlContextMenuStrip();
                panel.Controls.Add(this);
                Exinfo = exinfo;
                if (Exinfo != null)
                {
                    this.GetInfoFromExportInfo(Exinfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// 增加控件事件
        /// </summary>
        public void AddControlEvent()
        {
            ContextMenuStrip = MenuStrip;
            MouseUp += new MouseEventHandler(this.PrintPreviewControl_MouseUp);
            MouseDown += new MouseEventHandler(this.PrintPreviewControl_MouseDown);
            MouseMove += new MouseEventHandler(this.PrintPreviewControl_MouseMove);
            MouseLeave += new EventHandler(this.PrintPreviewControl_MouseLeave);
        }

        /// <summary>
        /// 添加Label到Panel
        /// </summary>
        public void AddLabel()
        {
            FillPreviewLabel();
            if (ResetMode)
            {
                ResetMode = false;
            }
            else
            {
                AddControlEvent();
            }
        }

        /// <summary>
        /// 填充控件信息
        /// </summary>
        private void FillPreviewLabel()
        {
            Size = new System.Drawing.Size(2, 2);
            Text = Exinfo.TagInfo.Info;
            AutoSize = true;
            BringToFront();
            Tag = new TagInfo("text", Text);
        }
    }
}