using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PrintModule_ReConstruction_
{
    public class PrintPreviewLabel : Label, IPrintPreviewControl
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
        /// BelongPanel所在的窗体
        /// </summary>
        public PrintForm PanelForm { get; set; }
        /// <summary>
        /// 右键菜单
        /// </summary>
        public ContextMenuStrip MenuStrip { get; set; }
        #endregion

        public PrintPreviewLabel(Panel panel, ExportInfo exinfo = null)
            : base()
        {
            try
            {
                BelongPanel = panel;
                PanelForm = panel.Parent as PrintForm;
                if (PanelForm == null)
                {
                    throw new Exception("Can't get PanelForm.");
                }
                ExpandArray = new int[4];
                MenuStrip = new ControlContextMenuStrip();
                panel.Controls.Add(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            if (exinfo != null)
            {
                this.GetInfoFromExportInfo(exinfo);
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
            this.SetPanelControlsEnabled(true);
            if (ResetMode)
            {
                FillPreviewLabel();
                ResetMode = false;
            }
            else
            {
                FillPreviewLabel();
                AddControlEvent();
            }
            PanelForm.SetShowStringText(string.Empty);
            this.SetButtonEnabled(true, null);
        }
        /// <summary>
        /// 填充控件信息
        /// </summary>
        private void FillPreviewLabel()
        {
            Text = PanelForm.GetShowStringValue();
            AutoSize = true;
            BringToFront();
            Tag = new TagInfo("text", PanelForm.GetShowStringValue());
        }
    }
}
