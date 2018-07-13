using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PrintModule_ReConstruction_
{
    public abstract class PrintPreviewPictureBox : PictureBox, IPrintPreviewControl
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
        /// <summary>
        /// 图片
        /// </summary>
        public Image PicImage { get; set; }
        #endregion 

        public PrintPreviewPictureBox(Panel panel, ExportInfo exinfo = null)
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
        /// 获取图片填充PictureBox
        /// </summary>
        /// <param name="exinfo"></param>
        public abstract void GeneratePictureBoxFillImage(ExportInfo exinfo = null);
        /// <summary>
        /// 添加PictureBox到Panel
        /// </summary>
        /// <param name="img"></param>
        /// <param name="tag"></param>
        protected void AddPictureBox(Image img, TagInfo tag)
        {
            this.SetPanelControlsEnabled(true);
            if (ResetMode)
            {
                PicBoxFillImage(img, tag);
                ResetMode = false;
            }
            else
            {
                PicBoxFillImage(img, tag);
                AddControlEvent();
            }
            PanelForm.SetShowStringText(string.Empty);
            this.SetButtonEnabled(true, null);
        }
        /// <summary>
        /// 填充控件信息
        /// </summary>
        /// <param name="img"></param>
        /// <param name="tag"></param>
        private void PicBoxFillImage(Image img, TagInfo tag)
        {
            if (tag.Type == "background")
            {
                SendToBack();
            }
            else
            {
                SizeMode = PictureBoxSizeMode.StretchImage;
                Image = img;
                BringToFront();
            }
            Size = new Size(PanelForm.GetWidthValue(), PanelForm.GetHeightValue());
            Tag = tag;
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
            SizeChanged += new EventHandler(this.PrintPreviewControl_SizeChanged);
        }
    }
}
