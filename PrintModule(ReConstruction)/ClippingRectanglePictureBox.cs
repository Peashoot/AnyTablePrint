using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    public class ClippingRectanglePictureBox : PictureBox
    {
        public ClippingRectanglePictureBox(Panel panel)
            : base()
        {
            _clippingPanel = panel;
            _clippingPanel.Parent.Controls.Add(this);
            Size = _clippingPanel.Size;
            Location = _clippingPanel.Location;
            BringToFront();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            Visible = false;
        }

        #region 参数

        /// <summary>
        /// 裁剪模式
        /// </summary>
        private bool _clipMode;

        public bool ClipMode
        {
            get { return _clipMode; }
            set
            {
                _clipMode = value;
                if (value)
                {
                    Bitmap panelImg = new Bitmap(Width, Height);
                    try
                    {
                        _clippingPanel.DrawToBitmap(panelImg, ClientRectangle);
                        Image = panelImg;
                        Visible = true;
                    }
                    catch (Exception)
                    {
                        panelImg.Dispose();
                    }
                }
                else
                {
                    Visible = false;
                }
            }
        }

        /// <summary>
        /// 正在裁剪
        /// </summary>
        private bool _isClipping;

        /// <summary>
        /// 裁剪前鼠标坐标
        /// </summary>
        private Point _beforeLoc;

        /// <summary>
        /// 裁剪后鼠标坐标
        /// </summary>
        private Point _afterLoc;

        /// <summary>
        /// 裁剪矩形
        /// </summary>
        private Rectangle _clippingRect;

        /// <summary>
        /// 被裁剪的Panel
        /// </summary>
        private Panel _clippingPanel;

        #endregion 参数

        #region 增加事件

        /// <summary>
        /// 增加事件
        /// </summary>
        public void AddPanelTriggerEvent()
        {
            MouseDown += new MouseEventHandler(PrintPreviewControlPanel_MouseDown);
            MouseMove += new MouseEventHandler(PrintPreviewControlPanel_MouseMove);
            MouseUp += new MouseEventHandler(PrintPreviewControlPanel_MouseUp);
            Paint += new PaintEventHandler(PrintPreviewControlPanel_Paint);
        }

        #endregion 增加事件

        #region 按下裁剪，移动边框

        /// <summary>
        /// 鼠标按下进入裁剪状态
        /// </summary>
        private void PrintPreviewControlPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (ClipMode)
            {
                _isClipping = true;
                _beforeLoc = PointToClient(MousePosition);
                _clippingRect.Location = MousePosition;
                _clippingRect.Width = 1;
                _clippingRect.Height = 1;
            }
        }

        /// <summary>
        /// 鼠标移动画出裁剪框
        /// </summary>
        private void PrintPreviewControlPanel_MouseMove(object sender, MouseEventArgs e)
        {
            _afterLoc = PointToClient(MousePosition);
            _afterLoc.X = _afterLoc.X < 0 ? 0 : _afterLoc.X >= Width ? Width - 1 : _afterLoc.X;
            _afterLoc.Y = _afterLoc.Y < 0 ? 0 : _afterLoc.Y >= Width ? Width - 1 : _afterLoc.Y;
            _clippingRect.Width = Math.Min(Math.Abs(_afterLoc.X - _beforeLoc.X), Width - _clippingRect.Location.X);
            _clippingRect.Height = Math.Min(Math.Abs(_afterLoc.Y - _beforeLoc.Y), Height - _clippingRect.Location.Y);
            if (_afterLoc.X < _clippingRect.Location.X)
            {
                _clippingRect.Location = new Point(Math.Max(Math.Min(_afterLoc.X, Width - 1), 0), Math.Max(Math.Min(_clippingRect.Top, Height - 1), 0));
            }
            if (_afterLoc.Y < _clippingRect.Location.Y)
            {
                _clippingRect.Location = new Point(Math.Max(Math.Min(_clippingRect.Left, Width - 1), 0), Math.Max(Math.Min(_afterLoc.Y, Height - 1), 0));
            }
            Refresh();
        }

        #endregion 按下裁剪，移动边框

        #region 鼠标抬起结束裁剪保存图片

        /// <summary>
        /// 鼠标抬起结束裁剪保存图片
        /// </summary>
        private void PrintPreviewControlPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (ClipMode && _isClipping)
            {
                _isClipping = false;
                Bitmap clipImg = (Bitmap)Image;
                clipImg = clipImg.Clone(_clippingRect, clipImg.PixelFormat);
                Clipboard.SetImage(clipImg);
                if (MessageBox.Show(this, "是否保存图片？", "保存", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SaveBitmapToLocal(clipImg);
                }
                clipImg.Dispose();
                Refresh();
                ClipMode = false;
            }
        }

        /// <summary>
        /// 保存图片至本地
        /// </summary>
        private void SaveBitmapToLocal(Bitmap clipImg)
        {
            using (SaveFileDialog savefileDialog = new SaveFileDialog())
            {
                savefileDialog.Filter = "JPeg格式图片|*.jpg|Bitmap格式图片|*.bmp|Gif格式图片|*.gif|Png格式图片|*.png|Tiff格式图片|*.tif";
                savefileDialog.Title = "选择保存的路径：";
                if (DialogResult.OK == savefileDialog.ShowDialog() && !string.IsNullOrEmpty(savefileDialog.FileName))
                {
                    using (FileStream fs = (FileStream)savefileDialog.OpenFile())
                    {
                        SaveBitmapWithSelectedFormat(clipImg, savefileDialog, fs);
                    }
                }
            }
        }

        /// <summary>
        /// 根据不同的图片格式保存图片
        /// </summary>
        private void SaveBitmapWithSelectedFormat(Bitmap clipImg, SaveFileDialog savefileDialog, FileStream fs)
        {
            switch (savefileDialog.FilterIndex)
            {
                case 1:
                    clipImg.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;

                case 2:
                    clipImg.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;

                case 3:
                    clipImg.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
                    break;

                case 4:
                    clipImg.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                    break;

                case 5:
                    clipImg.Save(fs, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
            }
        }

        #endregion 鼠标抬起结束裁剪保存图片

        #region 重绘

        /// <summary>
        /// 鼠标拖动过程中不断重绘裁剪框
        /// </summary>
        private void PrintPreviewControlPanel_Paint(object sender, PaintEventArgs e)
        {
            if (ClipMode && _isClipping)
            {
                using (Pen pen = new Pen(Color.Red))
                {
                    e.Graphics.DrawRectangle(pen, _clippingRect);
                }
            }
        }

        #endregion 重绘
    }
}