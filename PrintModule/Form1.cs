using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using ZXing;
using System.Xml;

namespace PrintModule
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region 成员变量
        /// <summary>
        /// 字体
        /// </summary>
        private Font foreFont;
        /// <summary>
        /// 字体颜色
        /// </summary>
        private Color foreColor;
        /// <summary>
        /// 背景颜色
        /// </summary>
        private Color backColor;
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
        /// 存储的Label
        /// </summary>
        private Label labstore;
        /// <summary>
        /// 存储的PictureBox
        /// </summary>
        private PictureBox pbstore;
        #endregion
        #region 窗体加载
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            txt_Width.Text = "200";
            txt_Height.Text = "200";
            resetMode = false;
            moveLock = false;
            //字体颜色
            foreColor = Color.Black;
            //字体
            foreFont = new Font("宋体", 11f, FontStyle.Regular);
            //背景颜色
            backColor = Color.Transparent;
            //鼠标位置
            beforeLoc = afterLoc = new Point();
            //OpenFileDialog初始化
            fileDialog.FileName = string.Empty;
            fileDialog.InitialDirectory = System.Environment.CurrentDirectory;
            fileDialog.Filter = "图片文件|*.jpg;*.bmp;*.png;*.jpeg;*.gif";
        }
        #endregion
        #region 选择属性
        #region 选择字体
        /// <summary>
        /// 选择字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FontDialog_Click(object sender, EventArgs e)
        {
            fontDialog.Font = foreFont;
            fontDialog.Color = foreColor;
            if (DialogResult.OK == fontDialog.ShowDialog())
            {
                foreFont = fontDialog.Font;
                foreColor = fontDialog.Color;
            }
        }
        #endregion
        #region 选择背景颜色
        /// <summary>
        /// 选择背景颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ColorDialog_Click(object sender, EventArgs e)
        {
            colorDialog.Color = backColor;
            if (DialogResult.OK == colorDialog.ShowDialog())
            {
                backColor = colorDialog.Color;
            }
        }
        #endregion
        #region 数字框只允许输入数字
        /// <summary>
        /// 数字框只允许输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numText_TextChanged(object sender, EventArgs e)
        {
            Regex pattern = new Regex(@"\D");
            (sender as TextBox).Text = pattern.Replace((sender as TextBox).Text, "");
        }
        #endregion
        #endregion
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
            if (resetMode)
            {
                labstore.Text = txt_ShowString.Text;
                labstore.AutoSize = true;
                labstore.ForeColor = foreColor;
                labstore.Font = foreFont;
                labstore.BackColor = backColor;
                labstore.BringToFront();
                labstore.Tag = new TagInfo("text", txt_ShowString.Text.Trim());
            }
            else
            {
                Label additem = new Label();
                panel.Controls.Add(additem);
                additem.Text = txt_ShowString.Text;
                additem.AutoSize = true;
                additem.ForeColor = foreColor;
                additem.Font = foreFont;
                additem.BackColor = backColor;
                additem.BringToFront();
                additem.Location = location;
                additem.Tag = new TagInfo("text", txt_ShowString.Text.Trim());
                AddControlsEvent(additem);
            }
            txt_ShowString.Text = string.Empty;
            AllButtonEnabled();
        }
        #endregion
        #region 添加图片
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Image_Click(object sender, EventArgs e)
        {
            fileDialog.Filter = "图片文件|*.jpg;*.bmp;*.png;*.jpeg;*.gif";
            if (DialogResult.OK == fileDialog.ShowDialog())
            {
                image = Image.FromStream(fileDialog.OpenFile());
                image = ZoomPicture(image, txt_Height.Text.ToInt32(), txt_Height.Text.ToInt32());
            }
            AddPictureBox(image, new TagInfo("image", fileDialog.FileName));
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
            catch (Exception ex)
            {

            }

            return null;
        }
        #endregion
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
        #endregion
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
            catch (Exception ex)
            {
                //异常输出
            }
            return result;
        }
        #endregion
        #region 添加背景
        /// <summary>
        /// 添加背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Background_Click(object sender, EventArgs e)
        {
            AddPictureBox(null, new TagInfo("background", colorDialog.Color.Name));
        }
        #endregion
        #region 增加PictureBox
        /// <summary>
        /// 增加PictureBox
        /// </summary>
        /// <param name="img"></param>
        private void AddPictureBox(Image img, TagInfo tag, Point location = new Point())
        {
            if (resetMode)
            {
                if (pbstore != null)
                {
                    if (img != null)
                    {
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
                }
            }
            else
            {
                PictureBox pb = new PictureBox();
                panel.Controls.Add(pb);
                if (img != null)
                {
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
                AddControlsEvent(pb);
            }
            txt_ShowString.Text = string.Empty;
            AllButtonEnabled();
        }
        #endregion
        #region 给控件添加事件
        /// <summary>
        /// 给控件添加事件
        /// </summary>
        /// <param name="control"></param>
        public void AddControlsEvent(Control control)
        {
            control.ContextMenuStrip = menuStrip;
            control.MouseUp += new MouseEventHandler(Control_MouseUp);
            control.MouseDown += new MouseEventHandler(Control_MouseDown);
            control.MouseMove += new MouseEventHandler(Control_MouseMove);
        }
        #endregion
        #region 鼠标拖动事件
        /// <summary>
        /// 鼠标按下记录当前鼠标位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseDown(object sender, EventArgs e)
        {
            beforeLoc = MousePosition;
            moveLock = true;
        }
        /// <summary>
        /// 鼠标抬起移动控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseUp(object sender, EventArgs e)
        {
            moveLock = false;
        }
        /// <summary>
        /// 鼠标抬起移动控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseMove(object sender, EventArgs e)
        {
            if (moveLock)
            {
                afterLoc = MousePosition;
                Control current = sender as Control;
                current.Location = new Point(
                    Math.Min(Math.Max(current.Location.X + afterLoc.X - beforeLoc.X, 0), panel.Size.Width - current.Size.Width),
                    Math.Min(Math.Max(current.Location.Y + afterLoc.Y - beforeLoc.Y, 0), panel.Size.Height - current.Size.Height));
                beforeLoc = afterLoc;
            }
        }
        #endregion
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
            if (label != null)
            {
                labstore = label;
                foreFont = label.Font;
                foreColor = label.ForeColor;
                txt_ShowString.Text = label.Text;
                backColor = label.BackColor;
                AllButtonUnabled(btn_AddLabel);
            }
            if (picbox != null)
            {
                pbstore = picbox;
                Button btn = null;
                TagInfo tag = picbox.Tag as TagInfo;
                if (tag != null)
                {
                    switch (tag.type)
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
                    AllButtonUnabled(btn);
                    image = picbox.Image;
                    txt_ShowString.Text = tag.info;
                    backColor = picbox.BackColor;
                    txt_Width.Text = picbox.Width.ToString();
                    txt_Height.Text = picbox.Height.ToString();
                }
            }
        }
        #endregion
        #endregion
        #region 改变Enabled属性
        /// <summary>
        /// 把除了重新设置控件对应的按钮外，全部置为不可用
        /// </summary>
        /// <param name="btn"></param>
        private void AllButtonUnabled(Button btn)
        {
            if (resetMode)
            {
                foreach (Control c in Controls)
                {
                    if (c.Tag != null && c.Tag.ToString() == "Operate")
                    {
                        if (c != btn)
                        {
                            c.Enabled = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 把所有按钮置为可用
        /// </summary>
        private void AllButtonEnabled()
        {
            if (resetMode)
            {
                foreach (Control c in Controls)
                {
                    if (c.Tag != null && c.Tag.ToString() == "Operate")
                    {
                        c.Enabled = true;
                    }
                }
                resetMode = false;
            }
        }
        #endregion
        #region 导出到XML
        /// <summary>
        /// 导出信息到XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ExportInfo_Click(object sender, EventArgs e)
        {
            if (txt_ShowString.Text.Trim().IsEmpty())
            {
                MessageBox.Show("文件名不能为空");
                return;
            }
            string fileName = txt_ShowString.Text.Trim().TrimEnd(".xml");
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmlSM = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(xmlSM);
            XmlElement root = xmldoc.CreateElement("", "Root", "");
            xmldoc.AppendChild(root);

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
            xmldoc.Save(AppDomain.CurrentDomain.BaseDirectory + fileName + ".xml");
            txt_ShowString.Text = string.Empty;
            MessageBox.Show("导出成功");
        }
        #endregion
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
                fileDialog.Filter = "XML文件|*.xml";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    xmldoc.Load(fileDialog.FileName);
                }
                XmlElement root = xmldoc.DocumentElement;
                List<ExportInfo> exportlist = new List<ExportInfo>();
                foreach (XmlNode node in root.ChildNodes)
                {
                    exportlist.Add(ExportInfo.GetInfoFromXML(((XmlElement)node)));
                }
                while (panel.Controls.Count > 0)
                {
                    panel.Controls[0].Dispose();
                }
                foreach (ExportInfo exportinfo in exportlist)
                {
                    txt_ShowString.Text = exportinfo.taginfo.info;
                    txt_Width.Text = exportinfo.size.Width.ToString();
                    txt_Height.Text = exportinfo.size.Height.ToString();
                    foreColor = exportinfo.foreColor;
                    foreFont = exportinfo.foreFont;
                    backColor = exportinfo.backColor;
                    switch (exportinfo.taginfo.type)
                    {
                        case "text":
                            AddLabel(exportinfo.location);
                            break;
                        case "image":
                            image = Image.FromFile(exportinfo.taginfo.info);
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
        #endregion
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
        #endregion
    }
}
