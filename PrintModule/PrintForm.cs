using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ZXing;

namespace PrintModule
{
    public partial class PrintForm : Form
    {
        public PrintForm()
        {
            InitializeComponent();
        }

        #region 成员变量

        /// <summary>
        /// 字体
        /// </summary>
        private Font foreFont;

        /// <summary>
        /// 背景颜色
        /// </summary>
        private Color backColor;

        /// <summary>
        /// 字体颜色
        /// </summary>
        private Color foreColor;

        /// <summary>
        /// 拖动前鼠标的位置
        /// </summary>
        private Point beforeLoc;

        /// <summary>
        /// 拖动后鼠标的位置
        /// </summary>
        private Point afterLoc;

        /// <summary>
        /// 添加的图片
        /// </summary>
        private Image image;

        /// <summary>
        /// 重新设置模式
        /// </summary>
        private bool resetMode;

        /// <summary>
        /// 控件拖动
        /// </summary>
        private bool moveLock;

        /// <summary>
        /// 控件缩放
        /// </summary>
        private bool expandLock;

        /// <summary>
        /// 缩放字符串（表示缩放的类型）
        /// </summary>
        private string expandStr;

        /// <summary>
        /// 控件的最小宽度
        /// </summary>
        private int minWidth;

        /// <summary>
        /// 控件的最小高度
        /// </summary>
        private int minHeight;

        /// <summary>
        /// 存储的Label
        /// </summary>
        private Label labstore;

        /// <summary>
        /// 存储的PictureBox
        /// </summary>
        private PictureBox pbstore;

        /// <summary>
        /// 存储的panel
        /// </summary>
        private PictureBox panelPicBox;

        /// <summary>
        /// 截图模式
        /// </summary>
        private bool _clipmode;

        private bool clipMode
        {
            get { return _clipmode; }
            set
            {
                _clipmode = value;
                if (value)
                {
                    panelPicBox.BackColor = Color.Transparent;
                    panelPicBox.Size = panel.Size;
                    Bitmap panelImg = new Bitmap(panel.Width, panel.Height);
                    panel.DrawToBitmap(panelImg, panel.ClientRectangle);
                    panelPicBox.Image = panelImg;
                    panelPicBox.Visible = true;
                    panelPicBox.BringToFront();
                    SetButtonEnabled(false, btn_Screenshots);
                    btn_FontDialog.Enabled = false;
                    btn_BackColor.Enabled = false;
                }
                else
                {
                    panelPicBox.Visible = false;
                    SetButtonEnabled(true, null);
                    btn_FontDialog.Enabled = true;
                    btn_BackColor.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 是否正在裁剪
        /// </summary>
        private bool isClipping;

        /// <summary>
        /// 裁剪矩阵
        /// </summary>
        private Rectangle clippingRect;

        /// <summary>
        /// 打印文档
        /// </summary>
        private PrintDocument printDocument = new PrintDocument();

        /// <summary>
        /// 控件命名标记
        /// </summary>
        private Dictionary<string, int> ControlNameIndex = new Dictionary<string, int>();

        #endregion 成员变量

        #region 窗体加载

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            panelStoreInit();
            memberInit();
            BindComboBox();
        }

        /// <summary>
        /// 初始化成员变量
        /// </summary>
        private void memberInit()
        {
            resetMode = false;
            moveLock = false;
            expandLock = false;
            clipMode = false;
            clippingRect = new Rectangle();
            isClipping = false;
            minWidth = 10;
            minHeight = 10;
            foreColor = Color.Black;
            foreFont = new Font("宋体", 11f, FontStyle.Regular);
            backColor = Color.Black;
            beforeLoc = afterLoc = new Point();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        }

        /// <summary>
        /// panelstore初始化
        /// </summary>
        private void panelStoreInit()
        {
            panelPicBox = new PictureBox();
            this.Controls.Add(panelPicBox);
            panelPicBox.BringToFront();
            panelPicBox.Location = panel.Location;
            panelPicBox.Size = panel.Size;
            panelPicBox.Visible = false;
            panelPicBox.MouseDown += new MouseEventHandler(panel_MouseDown);
            panelPicBox.MouseMove += new MouseEventHandler(panel_MouseMove);
            panelPicBox.MouseUp += new MouseEventHandler(panel_MouseUp);
            panelPicBox.Paint += new PaintEventHandler(panel_Paint);
        }

        /// <summary>
        /// 绑定下拉框
        /// </summary>
        private void BindComboBox()
        {
            List<string> list_printName = new List<string>();
            foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                list_printName.Add(printerName);
            }
            cmb_Printer.Items.AddRange(list_printName.ToArray());
            cmb_Printer.SelectedIndex = 0;
        }

        #endregion 窗体加载

        #region 选择属性

        #region 选择字体

        /// <summary>
        /// 选择字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FontDialog_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.ShowColor = true;
                fontDialog.Font = foreFont;
                fontDialog.Color = foreColor;
                if (DialogResult.OK == fontDialog.ShowDialog())
                {
                    foreFont = fontDialog.Font;
                    foreColor = fontDialog.Color;
                }
            }
        }

        #endregion 选择字体

        #region 选择背景颜色

        /// <summary>
        /// 选择背景颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ColorDialog_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = backColor;
                if (DialogResult.OK == colorDialog.ShowDialog())
                {
                    backColor = colorDialog.Color;
                }
            }
        }

        #endregion 选择背景颜色

        #region 数字框只允许输入数字

        /// <summary>
        /// 数字框只允许输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numText_KeyPress(object sender, KeyPressEventArgs e)
        {
            //值允许数字或者是退格和del输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != (char)20 && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        #endregion 数字框只允许输入数字

        #endregion 选择属性

        #region 增加控件

        #region 增加Label文本标签

        /// <summary>
        /// 增加Label文本标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddLabel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_ShowString.Text.Trim()))
                return;
            AddLabel();
        }

        private void AddLabel(Point location = new Point())
        {
            SetPanelControlsEnabled(true);
            if (resetMode)
            {
                labstore.Text = txt_ShowString.Text;
                labstore.AutoSize = true;
                labstore.Font = foreFont;
                labstore.ForeColor = foreColor;
                labstore.BackColor = backColor;
                labstore.BringToFront();
                labstore.Tag = new TagInfo("text", txt_ShowString.Text.Trim());
                resetMode = false;
            }
            else
            {
                Label additem = new Label();
                panel.Controls.Add(additem);
                additem.Text = txt_ShowString.Text;
                additem.AutoSize = true;
                additem.Font = foreFont;
                additem.ForeColor = foreColor;
                additem.BackColor = backColor;
                additem.BringToFront();
                additem.Location = location;
                additem.Tag = new TagInfo("text", txt_ShowString.Text.Trim());
                additem.Name = GetControlName("text");
                AddControlEvent(additem);
            }
            txt_ShowString.Text = string.Empty;
            SetButtonEnabled(true, null);
        }

        #endregion 增加Label文本标签

        #region 添加图片

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Image_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialog.Title = "选择要使用的图片";
            openFileDialog.Filter = "图片文件|*.jpg;*.bmp;*.png;*.jpeg;*.gif";
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                image = Image.FromStream(openFileDialog.OpenFile());
                image = ZoomPicture(image, txt_Height.Text.ToInt32(), txt_Height.Text.ToInt32());
            }
            AddPictureBox(image, new TagInfo("image", openFileDialog.FileName));
        }

        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="SourceImage"></param>
        /// <param name="TargetWidth"></param>
        /// <param name="TargetHeight"></param>
        /// <returns></returns>
        public Image ZoomPicture(Image SourceImage, int TargetWidth, int TargetHeight)
        {
            int IntWidth; //新的图片宽
            int IntHeight; //新的图片高
            try
            {
                System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;
                System.Drawing.Bitmap SaveImage = new System.Drawing.Bitmap(TargetWidth, TargetHeight);
                Graphics g = Graphics.FromImage(SaveImage);
                g.Clear(Color.White);

                if (SourceImage.Width > TargetWidth && SourceImage.Height <= TargetHeight)//宽度比目的图片宽度大，长度比目的图片长度小
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height > TargetHeight)//宽度比目的图片宽度小，长度比目的图片长度大
                {
                    IntHeight = TargetHeight;
                    IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height <= TargetHeight) //长宽比目的图片长宽都小
                {
                    IntHeight = SourceImage.Width;
                    IntWidth = SourceImage.Height;
                }
                else//长宽比目的图片的长宽都大
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                    if (IntHeight > TargetHeight)//重新计算
                    {
                        IntHeight = TargetHeight;
                        IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                    }
                }

                g.DrawImage(SourceImage, (TargetWidth - IntWidth) / 2, (TargetHeight - IntHeight) / 2, IntWidth, IntHeight);
                SourceImage.Dispose();

                return SaveImage;
            }
            catch (Exception)
            {
            }

            return null;
        }

        #endregion 添加图片

        #region 增加条码

        /// <summary>
        /// 增加条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Barcode_Click(object sender, EventArgs e)
        {
            if (txt_ShowString.Text.Trim().IsEmpty())
            {
                MessageBox.Show(this, "条码内容不能为空！");
            }
            image = GetBarCodeByZXingNet(txt_ShowString.Text.Trim(), txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
            AddPictureBox(image, new TagInfo("barcode", txt_ShowString.Text.Trim()));
        }

        /// <summary>
        /// 生成条码图片
        /// </summary>
        /// <param name="strMessage">要生成二维码的字符串</param>
        /// <param name="width">二维码图片宽度</param>
        /// <param name="height">二维码图片高度</param>
        /// <returns></returns>
        private Bitmap GetBarCodeByZXingNet(String strMessage, Int32 width, Int32 height)
        {
            Bitmap result = null;
            try
            {
                BarcodeWriter barCodeWriter = new BarcodeWriter();
                barCodeWriter.Format = BarcodeFormat.CODE_128;
                barCodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
                barCodeWriter.Options.Height = height;
                barCodeWriter.Options.Width = width;
                barCodeWriter.Options.Margin = 0;
                ZXing.Common.BitMatrix bm = barCodeWriter.Encode(strMessage);
                result = barCodeWriter.Write(bm);
            }
            catch
            {
                throw;
            }
            return result;
        }

        #endregion 增加条码

        #region 增加二维码

        /// <summary>
        /// 增加二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QRCode_Click(object sender, EventArgs e)
        {
            if (txt_ShowString.Text.Trim().IsEmpty())
            {
                MessageBox.Show(this, "二维码内容不能为空");
                return;
            }
            image = GetQRCodeByZXingNet(txt_ShowString.Text.Trim(), txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
            AddPictureBox(image, new TagInfo("qrcode", txt_ShowString.Text.Trim()));
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="strMessage">要生成二维码的字符串</param>
        /// <param name="width">二维码图片宽度</param>
        /// <param name="height">二维码图片高度</param>
        /// <returns></returns>
        private Bitmap GetQRCodeByZXingNet(String strMessage, Int32 width, Int32 height)
        {
            Bitmap result = null;
            try
            {
                BarcodeWriter barCodeWriter = new BarcodeWriter();
                barCodeWriter.Format = BarcodeFormat.QR_CODE;
                barCodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
                barCodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
                barCodeWriter.Options.Height = height;
                barCodeWriter.Options.Width = width;
                barCodeWriter.Options.Margin = 0;
                ZXing.Common.BitMatrix bm = barCodeWriter.Encode(strMessage);
                result = barCodeWriter.Write(bm);
            }
            catch (Exception)
            {
                //异常输出
            }
            return result;
        }

        #endregion 增加二维码

        #region 添加背景

        /// <summary>
        /// 添加背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Background_Click(object sender, EventArgs e)
        {
            AddPictureBox(null, new TagInfo("background", backColor.Name));
        }

        #endregion 添加背景

        #region 增加PictureBox

        /// <summary>
        /// 增加PictureBox
        /// </summary>
        /// <param name="img"></param>
        private void AddPictureBox(Image img, TagInfo tag, Point location = new Point())
        {
            SetPanelControlsEnabled(true);
            if (resetMode)
            {
                if (pbstore != null)
                {
                    if (img != null)
                    {
                        pbstore.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbstore.Image = img;
                        pbstore.BringToFront();
                    }
                    else
                    {
                        pbstore.SendToBack();
                    }
                    pbstore.Size = new Size(txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
                    pbstore.BackColor = backColor;
                    pbstore.Tag = tag;
                    resetMode = false;
                }
            }
            else
            {
                PictureBox pb = new PictureBox();
                panel.Controls.Add(pb);
                if (img != null)
                {
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Image = img;
                    pb.BringToFront();
                }
                else
                {
                    pb.SendToBack();
                }
                pb.Location = location;
                pb.Size = new Size(txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
                pb.BackColor = backColor;
                pb.Tag = tag;
                pb.Name = GetControlName("image");
                AddControlEvent(pb);
            }
            txt_ShowString.Text = string.Empty;
            //AllButtonEnabled();
            SetButtonEnabled(true, null);
        }

        #endregion 增加PictureBox

        #region 给控件添加事件

        /// <summary>
        /// 给控件添加事件
        /// </summary>
        /// <param name="control"></param>
        public void AddControlEvent(Control control)
        {
            control.ContextMenuStrip = menuStrip;
            control.MouseUp += new MouseEventHandler(Control_MouseUp);
            control.MouseDown += new MouseEventHandler(Control_MouseDown);
            control.MouseMove += new MouseEventHandler(Control_MouseMove);
            control.MouseLeave += new EventHandler(Control_MouseLeave);
            control.SizeChanged += new EventHandler(Control_SizeChanged);
        }

        /// <summary>
        /// 给控件删除事件
        /// </summary>
        /// <param name="control"></param>
        private void DeleteControlEvent(Control control)
        {
            control.ContextMenuStrip = null;
            control.MouseUp -= new MouseEventHandler(Control_MouseUp);
            control.MouseDown -= new MouseEventHandler(Control_MouseDown);
            control.MouseMove -= new MouseEventHandler(Control_MouseMove);
            control.MouseLeave -= new EventHandler(Control_MouseLeave);
            control.SizeChanged -= new EventHandler(Control_SizeChanged);
        }

        #endregion 给控件添加事件

        #region 鼠标拖动事件

        /// <summary>
        /// 鼠标按下记录当前鼠标位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseDown(object sender, EventArgs e)
        {
            beforeLoc = panel.PointToClient(MousePosition);
            if (this.Cursor == Cursors.Default)
            {
                moveLock = true;
            }
            else
            {
                expandLock = true;
            }
        }

        /// <summary>
        /// 鼠标抬起移动控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseUp(object sender, EventArgs e)
        {
            moveLock = false;
            expandLock = false;
        }

        /// <summary>
        /// 鼠标抬起移动控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseMove(object sender, EventArgs e)
        {
            Control current = sender as Control;
            afterLoc = new Point(
                Math.Min(Math.Max(panel.PointToClient(MousePosition).X, 0), panel.Size.Width - 1),
                Math.Min(Math.Max(panel.PointToClient(MousePosition).Y, 0), panel.Size.Height - 1));
            if (moveLock)
            {
                current.Location = new Point(
                    Math.Min(Math.Max(current.Location.X + afterLoc.X - beforeLoc.X, 0), panel.Size.Width - current.Size.Width),
                    Math.Min(Math.Max(current.Location.Y + afterLoc.Y - beforeLoc.Y, 0), panel.Size.Height - current.Size.Height));
                beforeLoc = afterLoc;
            }
            else if (expandLock)
            {
                int[] expandArray = expandStr.Split(',').ToInt32();
                afterLoc = new Point(
                    expandArray[0] == 0 ? afterLoc.X : Math.Min(afterLoc.X, current.Location.X + current.Size.Width - minWidth),
                    expandArray[1] == 0 ? afterLoc.Y : Math.Min(afterLoc.Y, current.Location.Y + current.Size.Height - minHeight));
                current.Location = new Point(
                    expandArray[0] == 0 ? current.Location.X : afterLoc.X,
                    expandArray[1] == 0 ? current.Location.Y : afterLoc.Y);
                current.Size = new Size(
                    expandArray[2] == 0 ? current.Size.Width : expandArray[2] == 1 ? Math.Min(Math.Max(current.Size.Width + afterLoc.X - beforeLoc.X, minWidth), panel.Size.Width - current.Location.X) : Math.Min(Math.Max(current.Size.Width + beforeLoc.X - afterLoc.X, minWidth), panel.Size.Width + current.Location.X),
                    expandArray[3] == 0 ? current.Size.Height : expandArray[3] == 1 ? Math.Min(Math.Max(current.Size.Height + afterLoc.Y - beforeLoc.Y, minHeight), panel.Size.Height - current.Location.Y) : Math.Min(Math.Max(current.Size.Height + beforeLoc.Y - afterLoc.Y, minHeight), panel.Size.Height + current.Location.Y));
                beforeLoc = afterLoc;
            }
            else if (current as PictureBox != null)
            {
                Point mousepos = current.PointToClient(MousePosition);
                if (NearLine(mousepos.X, 0) && NearLine(mousepos.Y, 0))                                                 //-1,-1
                {
                    this.Cursor = Cursors.SizeNWSE;
                    expandStr = "1,1,-1,-1";
                }
                else if (NearLine(mousepos.X + 1, current.Size.Width) && NearLine(mousepos.Y + 1, current.Size.Height)) //1,1
                {
                    this.Cursor = Cursors.SizeNWSE;
                    expandStr = "0,0,1,1";
                }
                else if (NearLine(mousepos.X + 1, current.Size.Width) && NearLine(mousepos.Y, 0))                       //1,-1
                {
                    this.Cursor = Cursors.SizeNESW;
                    expandStr = "0,1,1,-1";
                }
                else if (NearLine(mousepos.X, 0) && NearLine(mousepos.Y + 1, current.Size.Height))                      //-1,1
                {
                    this.Cursor = Cursors.SizeNESW;
                    expandStr = "1,0,-1,1";
                }
                else if (NearLine(mousepos.X, 0))                                                                       //-1,0
                {
                    this.Cursor = Cursors.SizeWE;
                    expandStr = "1,0,-1,0";
                }
                else if (NearLine(mousepos.X + 1, current.Size.Width))                                                  //1,0
                {
                    this.Cursor = Cursors.SizeWE;
                    expandStr = "0,0,1,0";
                }
                else if (NearLine(mousepos.Y, 0))                                                                       //0,-1
                {
                    this.Cursor = Cursors.SizeNS;
                    expandStr = "0,1,0,-1";
                }
                else if (NearLine(mousepos.Y + 1, current.Size.Height))                                                 //0,1
                {
                    this.Cursor = Cursors.SizeNS;
                    expandStr = "0,0,0,1";
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    expandStr = "0,0,0,0";
                }
            }
        }

        /// <summary>
        /// 判断一个数是否在一个区间内
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool NearLine(int a, int b, int c = 5)
        {
            if (b + c >= a && b - c <= a)
                return true;
            return false;
        }

        /// <summary>
        /// 鼠标离开控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseLeave(object sender, EventArgs e)
        {
            Control current = sender as Control;
            if (!moveLock && !expandLock)
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion 鼠标拖动事件

        #region 右键按钮事件

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            menuStrip.SourceControl.Dispose();
        }

        /// <summary>
        /// 重新设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSet_Click(object sender, EventArgs e)
        {
            Label label = menuStrip.SourceControl as Label;
            PictureBox picbox = menuStrip.SourceControl as PictureBox;
            resetMode = true;
            SetPanelControlsEnabled(false);
            if (label != null)
            {
                label.Enabled = true;
                labstore = label;
                foreFont = label.Font;
                txt_ShowString.Text = label.Text;
                foreColor = label.ForeColor;
                backColor = label.BackColor;
                SetButtonEnabled(false, btn_AddLabel);
            }
            if (picbox != null)
            {
                picbox.Enabled = true;
                pbstore = picbox;
                Button btn = null;
                TagInfo tag = picbox.Tag as TagInfo;
                if (tag != null)
                {
                    switch (tag.Type)
                    {
                        case "image":
                            btn = btn_AddImage;
                            break;

                        case "barcode":
                            btn = btn_AddBarcode;
                            break;

                        case "qrcode":
                            btn = btn_AddQRCode;
                            break;

                        case "background":
                            btn = btn_AddBackground;
                            break;
                    }
                    SetButtonEnabled(false, btn);
                    image = picbox.Image;
                    txt_ShowString.Text = tag.Info;
                    backColor = picbox.BackColor;
                    txt_Width.Text = picbox.Width.ToString();
                    txt_Height.Text = picbox.Height.ToString();
                }
            }
        }

        /// <summary>
        /// 设置panel里所有控件的Enabled
        /// </summary>
        /// <param name="enabled"></param>
        private void SetPanelControlsEnabled(bool enabled)
        {
            foreach (Control c in panel.Controls)
            {
                c.Enabled = enabled;
            }
        }

        /// <summary>
        /// 重置模式下，宽高label实时显示控件宽高
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_SizeChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (resetMode)
            {
                txt_Width.Text = c.Size.Width.ToString();
                txt_Height.Text = c.Size.Height.ToString();
            }
        }

        #endregion 右键按钮事件

        #region 获取控件名称
        /// <summary>
        /// 获取控件名称
        /// </summary>
        private string GetControlName(string type)
        {
            if (ControlNameIndex.ContainsKey(type))
            {
                ControlNameIndex[type]++;
            }
            else
            {
                ControlNameIndex[type] = 1;
            }
            return type + ControlNameIndex[type];
        }
        #endregion

        #endregion 增加控件

        #region 改变Enabled属性

        /// <summary>
        ///  设置按钮Enabled
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="exceptButton"></param>
        private void SetButtonEnabled(bool enabled, Button exceptButton)
        {
            foreach (Control c in this.Controls)
            {
                Button btn = c as Button;
                if (btn != null && btn != exceptButton)
                {
                    btn.Enabled = enabled;
                }
            }
        }

        #endregion 改变Enabled属性

        #region 导出到XML

        /// <summary>
        /// 导出信息到XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ExportInfo_Click(object sender, EventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmlSM = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(xmlSM);
            XmlElement root = xmldoc.CreateElement("", "Root", "");
            xmldoc.AppendChild(root);
            //把控件信息添加到xml信息中
            foreach (Control c in panel.Controls)
            {
                ExportInfo exportinfo = new ExportInfo();
                exportinfo.backColor = c.BackColor;
                exportinfo.foreColor = c.ForeColor;
                exportinfo.foreFont = c.Font;
                exportinfo.location = c.Location;
                exportinfo.size = c.Size;
                exportinfo.taginfo = c.Tag as TagInfo;
                exportinfo.AddIntoXMLDocument(ref xmldoc);
            }
            //把纸张信息添加到xml信息中
            XmlElement parent = xmldoc.CreateElement("PrintPaper");
            XmlElement child = xmldoc.CreateElement("PaperSizeName");
            child.InnerText = (cmb_PrintPaperSize.SelectedItem as PaperSize).PaperName;
            parent.AppendChild(child);
            child = xmldoc.CreateElement("PaperSize");
            XmlElement grandchild = xmldoc.CreateElement("Width");
            grandchild.InnerText = panel.Size.Width.ToString();
            child.AppendChild(grandchild);
            grandchild = xmldoc.CreateElement("Height");
            grandchild.InnerText = panel.Size.Height.ToString();
            child.AppendChild(grandchild);
            parent.AppendChild(child);
            root.AppendChild(parent);
            //生成的xml保存本地
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML文件|*.xml";
                saveFileDialog.Title = "选择保存的路径：";
                if (DialogResult.OK == saveFileDialog.ShowDialog())
                {
                    if (!saveFileDialog.FileName.IsEmpty())
                    {
                        using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                        {
                            xmldoc.Save(fs);
                            MessageBox.Show("导出成功");
                        }
                    }
                }
            }
        }

        #endregion 导出到XML

        #region 从XML导入信息

        /// <summary>
        /// 从XML导入信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImportInfo_Click(object sender, EventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
                openFileDialog.Title = "选择要加载的模板";
                openFileDialog.Filter = "XML文件|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    xmldoc.Load(openFileDialog.FileName);
                }
                XmlElement root = xmldoc.DocumentElement;
                List<ExportInfo> exportlist = new List<ExportInfo>();
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name != "PrintPaper")
                    {
                        exportlist.Add(new ExportInfo().GetInfoFromXML(((XmlElement)node)));
                    }
                    else
                    {
                        XmlNode child = ((XmlElement)node).GetElementsByTagName("PaperSizeName").Item(0);
                        cmb_PrintPaperSize.Text = child.InnerText;
                        child = ((XmlElement)node).GetElementsByTagName("PaperSize").Item(0);
                        XmlNode grandchild = ((XmlElement)child).GetElementsByTagName("Width").Item(0);
                        txt_Width.Text = grandchild.InnerText;
                        grandchild = ((XmlElement)child).GetElementsByTagName("Height").Item(0);
                        txt_Height.Text = grandchild.InnerText;
                        panel.Size = new Size(txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
                    }
                }
                while (panel.Controls.Count > 0)
                {
                    panel.Controls[0].Dispose();
                }
                //exportlist.Reverse();
                //添加控件信息
                foreach (ExportInfo exportinfo in exportlist)
                {
                    txt_ShowString.Text = exportinfo.taginfo.Info;
                    txt_Width.Text = exportinfo.size.Width.ToString();
                    txt_Height.Text = exportinfo.size.Height.ToString();
                    foreFont = exportinfo.foreFont;
                    foreColor = exportinfo.foreColor;
                    backColor = exportinfo.backColor;
                    switch (exportinfo.taginfo.Type)
                    {
                        case "text":
                            AddLabel(exportinfo.location);
                            break;

                        case "image":
                            image = Image.FromFile(exportinfo.taginfo.Info);
                            image = ZoomPicture(image, txt_Height.Text.ToInt32(), txt_Height.Text.ToInt32());
                            AddPictureBox(image, exportinfo.taginfo, exportinfo.location);
                            break;

                        case "qrcode":
                            image = GetQRCodeByZXingNet(txt_ShowString.Text.Trim(), txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
                            AddPictureBox(image, exportinfo.taginfo, exportinfo.location);
                            break;

                        case "barcode":
                            image = GetBarCodeByZXingNet(txt_ShowString.Text.Trim(), txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
                            AddPictureBox(image, exportinfo.taginfo, exportinfo.location);
                            break;

                        case "background":
                            AddPictureBox(null, exportinfo.taginfo, exportinfo.location);
                            break;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion 从XML导入信息

        #region 清空Panel

        /// <summary>
        /// 清空Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                panel.Controls[i].Dispose();
            }
        }

        #endregion 清空Panel

        #region 打印

        #region 打印页的生成

        /// <summary>
        /// 打印页的生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            foreach (Control c in panel.Controls)
            {
                TagInfo info = c.Tag as TagInfo;
                switch (info.Type)
                {
                    case "text":
                        e.Graphics.DrawString(info.Info, c.Font, new SolidBrush(c.ForeColor), c.Location);
                        break;

                    case "image":
                    case "qrcode":
                    case "barcode":
                        PictureBox pb = c as PictureBox;
                        if (pb != null && pb.Image != null)
                        {
                            e.Graphics.DrawImage(pb.Image, pb.Location);
                        }
                        break;

                    case "background":
                        e.Graphics.FillRegion(new SolidBrush(c.BackColor), c.Region);
                        break;
                }
            }
        }

        #endregion 打印页的生成

        #region 打印预览

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PrintPreview_Click(object sender, EventArgs e)
        {
            if (cmb_Printer.SelectedIndex < 0)
            {
                MessageBox.Show(this, "打印机未选择");
                return;
            }
            PrintSetting(panel.Size.Width, panel.Size.Height, cmb_Printer.SelectedItem.ToString());
            printDocument.DefaultPageSettings.Landscape = chk_Landscape.Checked;
            if (cmb_PrintPaperSize.SelectedIndex != cmb_PrintPaperSize.Items.Count - 1)
            {
                txt_Width.Text = (cmb_PrintPaperSize.SelectedItem as PaperSize).Width.ToString();
                txt_Height.Text = (cmb_PrintPaperSize.SelectedItem as PaperSize).Height.ToString();
            }
            panel.Size = new Size(txt_Width.Text.ToInt32(), txt_Height.Text.ToInt32());
            using (PrintPreviewDialog printpreviewdialog = new PrintPreviewDialog())
            {
                printpreviewdialog.Document = printDocument;
                printpreviewdialog.ShowDialog();
            }
        }

        #endregion 打印预览

        #region 打印设置

        /// <summary>
        /// 打印设置
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="PrinterName"></param>
        public void PrintSetting(int Width, int Height, string PrinterName)
        {
            //设置页面大小
            PaperSize paperSize = new PaperSize("自定义页面", Width, Height);
            //使用这个页面设置
            printDocument.DefaultPageSettings.PaperSize = paperSize;
            //设置打印的时候的所使用re打印机
            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = PrinterName;
            //逐份打印
            printDocument.DefaultPageSettings.PrinterSettings.Collate = true;
            //打印的份数
            printDocument.DefaultPageSettings.PrinterSettings.Copies = 1;
            //指定边距
            printDocument.DefaultPageSettings.Margins.Top = 0;
            printDocument.DefaultPageSettings.Margins.Left = 0;
        }

        #endregion 打印设置

        #region 打印

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, EventArgs e)
        {
            //if (cmb_Printer.SelectedIndex < 0)
            //{
            //    MessageBox.Show(this, "打印机未选择");
            //    return;
            //}
            //PrintSetting(panel.Size.Width, panel.Size.Height, cmb_Printer.SelectedItem.ToString());
            //printDocument.DefaultPageSettings.Landscape = chk_Landscape.Checked;
            //printDocument.Print();
            IssueLayoutHelper.SendLayout(IssueLayoutHelper.GetLayoutFromPanel(panel));
            var propertyList = IssueLayoutHelper.GetPropertyFromPanel(panel);
            foreach (var propertyInfo in propertyList)
            {
                IssueLayoutHelper.SendProperty(propertyInfo);
            }
        }

        #endregion 打印

        #region 改变打印纸张大小

        /// <summary>
        /// 打印纸张大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PrintSize_Click(object sender, EventArgs e)
        {
            if (txt_Width.Text.IsEmpty() || txt_Height.Text.IsEmpty())
            {
                MessageBox.Show(this, "打印纸张大小未选择");
                return;
            }
            if (cmb_PrintPaperSize.SelectedIndex != cmb_PrintPaperSize.Items.Count - 1)
            {
                txt_Width.Text = (cmb_PrintPaperSize.SelectedItem as PaperSize).Width.ToString();
                txt_Height.Text = (cmb_PrintPaperSize.SelectedItem as PaperSize).Height.ToString();
            }
            int panelWidth = txt_Width.Text.ToInt32();
            int panelHeight = txt_Height.Text.ToInt32();
            foreach (Control c in panel.Controls)
            {
                if (c.Location.X + c.Size.Width > panelWidth || c.Location.Y + c.Size.Height > panelHeight)
                {
                    if (MessageBox.Show(this, "更改纸张大小可能会导致部分内容丢失，是否更改？", "更改纸张大小", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                    {
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            panel.Size = new Size(panelWidth, panelHeight);
        }

        /// <summary>
        /// 当panel的大小改变时，窗口的大小也改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_Resize(object sender, EventArgs e)
        {
            this.Size = new Size(Math.Max(200, panel.Size.Width + 185), Math.Max(480, panel.Size.Height + 63));
        }

        #endregion 改变打印纸张大小

        #endregion 打印

        #region 把Panel截取放入剪切板中并可选地保存为图片

        /// <summary>
        /// 截取图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Screenshots_Click(object sender, EventArgs e)
        {
            clipMode = !clipMode;
        }

        /// <summary>
        /// 鼠标按下，记录截图起始点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (clipMode)
            {
                PictureBox pb = sender as PictureBox;
                if (pb != null)
                {
                    isClipping = true;
                    beforeLoc = pb.PointToClient(MousePosition);
                    clippingRect.Location = MousePosition;
                    clippingRect.Width = 1;
                    clippingRect.Height = 1;
                }
            }
        }

        /// <summary>
        /// 鼠标移动，显示截图范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                afterLoc = pb.PointToClient(MousePosition);
                afterLoc.X = afterLoc.X < 0 ? 0 : afterLoc.X >= panel.Width ? panel.Width - 1 : afterLoc.X;
                afterLoc.Y = afterLoc.Y < 0 ? 0 : afterLoc.Y >= panel.Width ? panel.Width - 1 : afterLoc.Y;
                clippingRect.Width = Math.Min(Math.Abs(afterLoc.X - beforeLoc.X), panel.Width - clippingRect.Location.X);
                clippingRect.Height = Math.Min(Math.Abs(afterLoc.Y - beforeLoc.Y), panel.Height - clippingRect.Location.Y);
                if (afterLoc.X < clippingRect.Location.X)
                {
                    clippingRect.Location = new Point(Math.Max(Math.Min(afterLoc.X, panel.Width - 1), 0), Math.Max(Math.Min(clippingRect.Location.Y, panel.Height - 1), 0));
                }
                if (afterLoc.Y < clippingRect.Location.Y)
                {
                    clippingRect.Location = new Point(Math.Max(Math.Min(clippingRect.Location.X, panel.Width - 1), 0), Math.Max(Math.Min(afterLoc.Y, panel.Height - 1), 0));
                }
                pb.Refresh();
            }
        }

        /// <summary>
        /// 鼠标抬起，截图完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (clipMode && isClipping)
            {
                isClipping = false;
                PictureBox pb = sender as PictureBox;
                if (pb != null)
                {
                    Bitmap clipImg = (Bitmap)pb.Image;
                    clipImg = clipImg.Clone(clippingRect, clipImg.PixelFormat);
                    txt_ShowString.Text = clippingRect.Location.ToString() + clippingRect.Size.ToString();
                    Clipboard.SetImage(clipImg);
                    if (MessageBox.Show(this, "是否保存图片？", "保存", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        using (SaveFileDialog savefileDialog = new SaveFileDialog())
                        {
                            savefileDialog.Filter = "JPeg格式图片|*.jpg|Bitmap格式图片|*.bmp|Gif格式图片|*.gif|Png格式图片|*.png|Tiff格式图片|*.tif";
                            savefileDialog.Title = "选择保存的路径：";
                            if (DialogResult.OK == savefileDialog.ShowDialog() && !savefileDialog.FileName.IsEmpty())
                            {
                                using (FileStream fs = (FileStream)savefileDialog.OpenFile())
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
                            }
                        }
                    }
                    clipImg.Dispose();
                    pb.Refresh();
                }
                clipMode = false;
            }
        }

        /// <summary>
        /// panel重绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            if (clipMode && isClipping)
            {
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), clippingRect);
                //e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.Red)), clippingRect);
            }
        }

        #region 截椭圆图片

        /// <summary>
        /// 截椭圆图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private Bitmap WayOne(Bitmap bmp)
        {
            Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(ret))
            {
                g.FillEllipse(new TextureBrush(bmp), 0, 0, bmp.Width, bmp.Height);
            }
            return ret;
        }

        /// <summary>
        /// 截椭圆图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private Bitmap WayTwo(Bitmap bmp)
        {
            Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(ret))
            {
                g.DrawImage(bmp, 0, 0, ret.Width, ret.Height);
                int r = Math.Min(ret.Width, ret.Height) / 2;
                PointF c = new PointF(ret.Width / 2.0F, ret.Height / 2.0F);
                for (int h = 0; h < ret.Height; h++)
                {
                    for (int w = 0; w < ret.Width; w++)
                    {
                        if ((int)Math.Pow(r, 2) < ((int)Math.Pow(w * 1.0 - c.X, 2) + (int)Math.Pow(h * 1.0 - c.Y, 2)))
                        {
                            ret.SetPixel(w, h, Color.Transparent);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 截椭圆图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private Bitmap WaySOne(Bitmap bmp)
        {
            Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(ret))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                using (System.Drawing.Drawing2D.GraphicsPath p = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Alternate))
                {
                    p.AddEllipse(0, 0, bmp.Width, bmp.Height);
                    g.FillPath(new TextureBrush(bmp), p);
                }
            }
            return ret;
        }

        /// <summary>
        /// 截椭圆图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private Bitmap WaySTwo(Bitmap bmp)
        {
            Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(ret))
            {
                g.DrawImage(bmp, 0, 0, ret.Width, ret.Height);
                int r = Math.Min(ret.Width, ret.Height) / 2;
                PointF c = new PointF(ret.Width / 2.0F, ret.Height / 2.0F);
                for (int h = 0; h < ret.Height; h++)
                {
                    for (int w = 0; w < ret.Width; w++)
                    {
                        if ((int)Math.Pow(r, 2) < ((int)Math.Pow(w * 1.0 - c.X, 2) + (int)Math.Pow(h * 1.0 - c.Y, 2)))
                        {
                            ret.SetPixel(w, h, Color.Transparent);
                        }
                    }
                }
                //画背景色圆
                using (Pen p = new Pen(System.Drawing.SystemColors.Control))
                {
                    g.DrawEllipse(p, 0, 0, ret.Width, ret.Height);
                }
            }
            return ret;
        }

        #endregion 截椭圆图片

        #endregion 把Panel截取放入剪切板中并可选地保存为图片

        #region 更改打印机或打印纸张

        /// <summary>
        /// 切换打印机时更改打印机支持的纸张大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_Printer_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_PrintPaperSize.Items.Clear();
            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = cmb_Printer.Text;
            IEnumerator iterator = printDocument.PrinterSettings.PaperSizes.GetEnumerator();
            while (iterator.MoveNext())
            {
                PaperSize current = iterator.Current as PaperSize;
                cmb_PrintPaperSize.Items.Add(current);
            }
            cmb_PrintPaperSize.DisplayMember = "PaperName";
            cmb_PrintPaperSize.SelectedIndex = cmb_PrintPaperSize.Items.Count - 1;
        }

        /// <summary>
        /// 切换纸张大小时将纸张大小显示在textbox中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_PrintPaperSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Width.Text = (cmb_PrintPaperSize.SelectedItem as PaperSize).Width.ToString();
            txt_Height.Text = (cmb_PrintPaperSize.SelectedItem as PaperSize).Height.ToString();
        }

        #endregion 更改打印机或打印纸张
    }
}