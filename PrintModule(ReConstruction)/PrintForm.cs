using System.Windows.Forms;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;
using System.Collections;

namespace PrintModule_ReConstruction_
{
    public partial class PrintForm : Form
    {
        public PrintForm()
        {
            InitializeComponent();
        }
        #region 成员变量
        /// <summary>
        /// 截图显示窗体
        /// </summary>
        private ClippingRectanglePictureBox ClippingPicBox;
        /// <summary>
        /// 打印设置对象
        /// </summary>
        private PrintPreviewHelper _printHelper;
        /// <summary>
        /// 控件信息
        /// </summary>
        private ExportInfo _exinfo;
        #endregion
        #region 给textBox赋值
        /// <summary>
        /// txtWidth赋值
        /// </summary>
        /// <param name="text"></param>
        public void SetWidthText(string text)
        {
            txt_Width.Text = text;
        }
        /// <summary>
        /// txt_Height赋值
        /// </summary>
        /// <param name="text"></param>
        public void SetHeightText(string text)
        {
            txt_Height.Text = text;
        }
        /// <summary>
        /// txt_ShowString赋值
        /// </summary>
        /// <param name="text"></param>
        public void SetShowStringText(string text)
        {
            txt_ShowString.Text = text;
        }
        #endregion
        #region 获取宽、高、信息
        /// <summary>
        /// 获取宽度信息
        /// </summary>
        /// <returns></returns>
        public int GetWidthValue()
        {
            return Convert.ToInt32(txt_Width.Text.Trim());
        }
        /// <summary>
        /// 获取高度信息
        /// </summary>
        /// <returns></returns>
        public int GetHeightValue()
        {
            return Convert.ToInt32(txt_Height.Text.Trim());
        }
        /// <summary>
        /// 获取控件包含信息
        /// </summary>
        /// <returns></returns>
        public string GetShowStringValue()
        {
            return txt_ShowString.Text.Trim();
        }
        #endregion
        #region 增加ImagePictureBox控件
        /// <summary>
        /// 增加图片控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddImage_Click(object sender, EventArgs e)
        {
            GenerateImagePictureBox("image", _exinfo);
        }
        /// <summary>
        /// 增加二维码控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddQRCode_Click(object sender, EventArgs e)
        {
            GenerateImagePictureBox("qrcode", _exinfo);
        }
        /// <summary>
        /// 增加条形码控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddBarcode_Click(object sender, EventArgs e)
        {
            GenerateImagePictureBox("barcode", _exinfo);
        }
        /// <summary>
        /// 增加背景控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddBackground_Click(object sender, EventArgs e)
        {
            GenerateImagePictureBox("background", _exinfo);
        }
        /// <summary>
        /// 生成显示图片的PictureBox
        /// </summary>
        /// <param name="type"></param>
        /// <param name="exinfo"></param>
        private void GenerateImagePictureBox(string type, ExportInfo exinfo = null)
        {
            GetExportInfoValue(exinfo);
            PrintPreviewPictureBox PicBox = null;
            try
            {
                switch (type)
                {
                    case "image":
                        PicBox = new PrintPreviewImagePictureBox(PreviewControlPanel, exinfo);
                        break;
                    case "qrcode":
                        PicBox = new PrintPreviewQRCodePictureBox(PreviewControlPanel, exinfo);
                        break;
                    case "barcode":
                        PicBox = new PrintPreviewBarCodePictureBox(PreviewControlPanel, exinfo);
                        break;
                    case "background":
                        PicBox = new PrintPreviewBackgroundPictureBox(PreviewControlPanel, exinfo);
                        break;
                }
                PicBox.GeneratePictureBoxFillImage(exinfo);
            }
            finally
            {
                if (PicBox != null)
                {
                    PicBox.Dispose();
                }
            }
        }
        #endregion
        #region 增加TextLabel控件
        /// <summary>
        /// 增加文本控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddLabel_Click(object sender, EventArgs e)
        {
            GeneratePrintPreviewLabel(_exinfo);
        }
        /// <summary>
        /// 生成显示文本的Label控件
        /// </summary>
        /// <param name="exinfo"></param>
        private void GeneratePrintPreviewLabel(ExportInfo exinfo = null)
        {
            GetExportInfoValue(exinfo);
            PrintPreviewLabel previewLabel = null;
            try
            {
                previewLabel = new PrintPreviewLabel(PreviewControlPanel, exinfo);
                previewLabel.AddLabel();
            }
            finally
            {
                if (previewLabel != null)
                {
                    previewLabel.Dispose();
                }
            }
        }
        #endregion
        #region 清空控件
        /// <summary>
        /// 清空Panel中的控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearPanelControls();
        }
        /// <summary>
        /// 清空Panel中的控件
        /// </summary>
        private void ClearPanelControls()
        {
            for (int i = 0; i < PreviewControlPanel.Controls.Count; i++)
            {
                PreviewControlPanel.Controls[i].Dispose();
            }
        }
        #endregion
        #region XML信息导入
        /// <summary>
        /// 从XML文件导入控件信息和打印信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImportInfo_Click(object sender, EventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                OpenXmlDocument(xmldoc);
                List<ExportInfo> exportlist = GetXmlDocumentInfo(xmldoc);
                ReAddControlsToPanel(exportlist);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Can't read xmldocument. Please make sure the format is correct.");
            }
        }
        /// <summary>
        /// 打开XML文件
        /// </summary>
        /// <param name="xmldoc"></param>
        private void OpenXmlDocument(XmlDocument xmldoc)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
                openFileDialog.Title = "选择要加载的模板";
                openFileDialog.Filter = "XML文件|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    xmldoc.Load(openFileDialog.FileName);
                }
            }
        }
        /// <summary>
        /// 从XML文件中获取控件信息和打印信息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        private List<ExportInfo> GetXmlDocumentInfo(XmlDocument xmldoc)
        {
            XmlElement root = xmldoc.DocumentElement;
            List<ExportInfo> exportlist = new List<ExportInfo>();
            using (ExportInfo ei = new ExportInfo())
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name != "PrintPaper")
                    {
                        exportlist.Add(ei.GetInfoFromXML(((XmlElement)node)));
                    }
                }
                cmb_PrintPaperSize.Text = ei.GetNodeValue(root, "PrintPaper", "PaperSizeName");
                SetWidthText(ei.GetNodeValue(root, "PrintPaper", "Width"));
                SetHeightText(ei.GetNodeValue(root, "PrintPaper", "Height"));
            }
            PreviewControlPanel.Size = new Size(GetWidthValue(), GetHeightValue());
            return exportlist;
        }
        /// <summary>
        /// 清空Panel中的控件，并重新添加XML中的控件
        /// </summary>
        /// <param name="exportlist"></param>
        private void ReAddControlsToPanel(List<ExportInfo> exportlist)
        {
            ClearPanelControls();
            //添加控件信息
            foreach (ExportInfo exportinfo in exportlist)
            {
                if (exportinfo.taginfo.Type == "text")
                {
                    GeneratePrintPreviewLabel(exportinfo);
                }
                else
                {
                    GenerateImagePictureBox(exportinfo.taginfo.Type, exportinfo);
                }
            }
        }
        #endregion
        #region XML信息导出
        /// <summary>
        /// 将Panel里的信息保存到本地XML中
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
            foreach (Control c in PreviewControlPanel.Controls)
            {
                AddControlInfoToXmlDocument(xmldoc, c);
            }
            //把纸张信息添加到xml信息中
            AddPrintPaperInfoToXmlDocument(xmldoc, root);
            //生成的xml保存本地
            SaveXmlDocumentToLocal(xmldoc);
            MessageBox.Show("Save to local success.");
        }
        /// <summary>
        /// 选择保存的文件位置和文件名
        /// </summary>
        /// <param name="xmldoc"></param>
        private void SaveXmlDocumentToLocal(XmlDocument xmldoc)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML文件|*.xml";
                saveFileDialog.Title = "选择保存的路径：";
                if (DialogResult.OK == saveFileDialog.ShowDialog())
                {
                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
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
        /// <summary>
        /// 增加控件信息到XML文件
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="c"></param>
        private void AddControlInfoToXmlDocument(XmlDocument xmldoc, Control c)
        {
            using (ExportInfo exportinfo = new ExportInfo())
            {
                exportinfo.backColor = c.BackColor;
                exportinfo.foreColor = c.ForeColor;
                exportinfo.foreFont = c.Font;
                exportinfo.location = c.Location;
                exportinfo.size = c.Size;
                exportinfo.taginfo = c.Tag as TagInfo;
                exportinfo.AddIntoXMLDocument(ref xmldoc);
            }
        }
        /// <summary>
        /// 增加打印纸张信息到XML文件
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="root"></param>
        private void AddPrintPaperInfoToXmlDocument(XmlDocument xmldoc, XmlElement root)
        {
            using (ExportInfo ei = new ExportInfo())
            {
                ei.AddNodeToXMLNode(xmldoc, root, "PrintPaper", new string[] { "PaperSizeName," + (cmb_PrintPaperSize.SelectedItem as PaperSize).PaperName, "Width," + PreviewControlPanel.Width, "Height," + PreviewControlPanel.Height });
            }
        }
        #endregion
        #region 截图和取消截图
        /// <summary>
        /// 截图和取消截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Screenshots_Click(object sender, EventArgs e)
        {
            if (ClippingPicBox == null)
            {
                ClippingPicBox = new ClippingRectanglePictureBox(PreviewControlPanel);
                ClippingPicBox.AddPanelTriggerEvent();
            }
            ClippingPicBox.ClipMode = !ClippingPicBox.ClipMode;
        }
        #endregion
        #region 窗体加载
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintForm_Load(object sender, EventArgs e)
        {
            InitMember();
            BindComboBox();
        }
        /// <summary>
        /// 初始化成员对象
        /// </summary>
        private void InitMember()
        {
            _exinfo = new ExportInfo();
            _printHelper = new PrintPreviewHelper(PreviewControlPanel);
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
        #endregion
        #region 打印
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, EventArgs e)
        {
            _printHelper.BeginPrint(cmb_Printer.Text, chk_Landscape.Checked);
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PrintPreview_Click(object sender, EventArgs e)
        {
            _printHelper.ShowPrintDialog(cmb_Printer.Text, chk_Landscape.Checked);
        }
        /// <summary>
        /// 选择打印纸张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_PrintPaperSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetWidthText((cmb_PrintPaperSize.SelectedItem as PaperSize).Width.ToString());
            SetHeightText((cmb_PrintPaperSize.SelectedItem as PaperSize).Height.ToString());
        }
        /// <summary>
        /// 选择打印机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_Printer_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_PrintPaperSize.Items.Clear();
            _printHelper.BindDefaultPrinter(cmb_Printer.Text);
            cmb_PrintPaperSize.Items.AddRange(_printHelper.GetPaperSizeCollection());
            cmb_PrintPaperSize.DisplayMember = "PaperName";
            cmb_PrintPaperSize.SelectedIndex = cmb_PrintPaperSize.Items.Count - 1;
        }
        #endregion
        #region 设置ExportInfo
        /// <summary>
        /// 设置文本属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FontDialog_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.Font = _exinfo.foreFont;
                fontDialog.Color = _exinfo.foreColor;
                if (DialogResult.OK == fontDialog.ShowDialog())
                {
                    _exinfo.foreFont = fontDialog.Font;
                    _exinfo.foreColor = fontDialog.Color;
                }
            }
        }
        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BackColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = _exinfo.backColor;
                if (DialogResult.OK == colorDialog.ShowDialog())
                {
                    _exinfo.backColor = colorDialog.Color;
                }
            }
        }
        /// <summary>
        /// 设置宽高和文本信息
        /// </summary>
        /// <param name="exinfo"></param>
        private void GetExportInfoValue(ExportInfo exinfo)
        {
            exinfo.size.Width = GetWidthValue();
            exinfo.size.Height = GetHeightValue();
            exinfo.taginfo.Info = GetShowStringValue();
        }
        #endregion
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
        #endregion
    }
}
