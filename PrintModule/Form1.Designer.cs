namespace PrintModule
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_ShowString = new System.Windows.Forms.TextBox();
            this.btn_AddLabel = new System.Windows.Forms.Button();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.btn_FontDialog = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btn_ReSet = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_ExportInfo = new System.Windows.Forms.Button();
            this.btn_PrintPreview = new System.Windows.Forms.Button();
            this.btn_Print = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_BackColor = new System.Windows.Forms.Button();
            this.txt_Width = new System.Windows.Forms.TextBox();
            this.txt_Height = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_AddBackground = new System.Windows.Forms.Button();
            this.btn_AddQRCode = new System.Windows.Forms.Button();
            this.btn_AddBarcode = new System.Windows.Forms.Button();
            this.btn_AddImage = new System.Windows.Forms.Button();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.peviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.btn_ImportInfo = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Location = new System.Drawing.Point(160, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(382, 430);
            this.panel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "要添加的文本";
            // 
            // txt_ShowString
            // 
            this.txt_ShowString.Location = new System.Drawing.Point(12, 39);
            this.txt_ShowString.Name = "txt_ShowString";
            this.txt_ShowString.Size = new System.Drawing.Size(132, 21);
            this.txt_ShowString.TabIndex = 2;
            // 
            // btn_AddLabel
            // 
            this.btn_AddLabel.Location = new System.Drawing.Point(14, 329);
            this.btn_AddLabel.Name = "btn_AddLabel";
            this.btn_AddLabel.Size = new System.Drawing.Size(64, 22);
            this.btn_AddLabel.TabIndex = 3;
            this.btn_AddLabel.Tag = "Operate";
            this.btn_AddLabel.Text = "添加文本";
            this.btn_AddLabel.UseVisualStyleBackColor = true;
            this.btn_AddLabel.Click += new System.EventHandler(this.btn_AddLabel_Click);
            // 
            // fontDialog
            // 
            this.fontDialog.ShowColor = true;
            // 
            // btn_FontDialog
            // 
            this.btn_FontDialog.Location = new System.Drawing.Point(12, 69);
            this.btn_FontDialog.Name = "btn_FontDialog";
            this.btn_FontDialog.Size = new System.Drawing.Size(64, 26);
            this.btn_FontDialog.TabIndex = 4;
            this.btn_FontDialog.Text = "选择字体";
            this.btn_FontDialog.UseVisualStyleBackColor = true;
            this.btn_FontDialog.Click += new System.EventHandler(this.btn_FontDialog_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_ReSet,
            this.btn_Delete});
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(125, 48);
            // 
            // btn_ReSet
            // 
            this.btn_ReSet.Name = "btn_ReSet";
            this.btn_ReSet.Size = new System.Drawing.Size(124, 22);
            this.btn_ReSet.Text = "重新设置";
            this.btn_ReSet.Click += new System.EventHandler(this.btn_ReSet_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(124, 22);
            this.btn_Delete.Text = "删除";
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_ExportInfo
            // 
            this.btn_ExportInfo.Location = new System.Drawing.Point(14, 358);
            this.btn_ExportInfo.Name = "btn_ExportInfo";
            this.btn_ExportInfo.Size = new System.Drawing.Size(64, 22);
            this.btn_ExportInfo.TabIndex = 6;
            this.btn_ExportInfo.Tag = "Operate";
            this.btn_ExportInfo.Text = "导出信息";
            this.btn_ExportInfo.UseVisualStyleBackColor = true;
            this.btn_ExportInfo.Click += new System.EventHandler(this.btn_ExportInfo_Click);
            // 
            // btn_PrintPreview
            // 
            this.btn_PrintPreview.Location = new System.Drawing.Point(39, 386);
            this.btn_PrintPreview.Name = "btn_PrintPreview";
            this.btn_PrintPreview.Size = new System.Drawing.Size(83, 22);
            this.btn_PrintPreview.TabIndex = 6;
            this.btn_PrintPreview.Tag = "Operate";
            this.btn_PrintPreview.Text = "打印预览";
            this.btn_PrintPreview.UseVisualStyleBackColor = true;
            // 
            // btn_Print
            // 
            this.btn_Print.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Print.Location = new System.Drawing.Point(57, 414);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(44, 22);
            this.btn_Print.TabIndex = 6;
            this.btn_Print.Tag = "Operate";
            this.btn_Print.Text = "打印";
            this.btn_Print.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "要添加的背景框";
            // 
            // btn_BackColor
            // 
            this.btn_BackColor.Location = new System.Drawing.Point(80, 69);
            this.btn_BackColor.Name = "btn_BackColor";
            this.btn_BackColor.Size = new System.Drawing.Size(64, 26);
            this.btn_BackColor.TabIndex = 4;
            this.btn_BackColor.Text = "选择背景";
            this.btn_BackColor.UseVisualStyleBackColor = true;
            this.btn_BackColor.Click += new System.EventHandler(this.btn_ColorDialog_Click);
            // 
            // txt_Width
            // 
            this.txt_Width.Location = new System.Drawing.Point(12, 153);
            this.txt_Width.Name = "txt_Width";
            this.txt_Width.Size = new System.Drawing.Size(61, 21);
            this.txt_Width.TabIndex = 8;
            this.txt_Width.TextChanged += new System.EventHandler(this.numText_TextChanged);
            // 
            // txt_Height
            // 
            this.txt_Height.Location = new System.Drawing.Point(83, 153);
            this.txt_Height.Name = "txt_Height";
            this.txt_Height.Size = new System.Drawing.Size(61, 21);
            this.txt_Height.TabIndex = 8;
            this.txt_Height.TextChanged += new System.EventHandler(this.numText_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "宽";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(105, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "高";
            // 
            // btn_AddBackground
            // 
            this.btn_AddBackground.Location = new System.Drawing.Point(80, 329);
            this.btn_AddBackground.Name = "btn_AddBackground";
            this.btn_AddBackground.Size = new System.Drawing.Size(64, 22);
            this.btn_AddBackground.TabIndex = 3;
            this.btn_AddBackground.Tag = "Operate";
            this.btn_AddBackground.Text = "添加背景";
            this.btn_AddBackground.UseVisualStyleBackColor = true;
            this.btn_AddBackground.Click += new System.EventHandler(this.btn_Background_Click);
            // 
            // btn_AddQRCode
            // 
            this.btn_AddQRCode.Location = new System.Drawing.Point(14, 301);
            this.btn_AddQRCode.Name = "btn_AddQRCode";
            this.btn_AddQRCode.Size = new System.Drawing.Size(64, 22);
            this.btn_AddQRCode.TabIndex = 3;
            this.btn_AddQRCode.Tag = "Operate";
            this.btn_AddQRCode.Text = "二维码";
            this.btn_AddQRCode.UseVisualStyleBackColor = true;
            this.btn_AddQRCode.Click += new System.EventHandler(this.btn_QRCode_Click);
            // 
            // btn_AddBarcode
            // 
            this.btn_AddBarcode.Location = new System.Drawing.Point(80, 301);
            this.btn_AddBarcode.Name = "btn_AddBarcode";
            this.btn_AddBarcode.Size = new System.Drawing.Size(64, 22);
            this.btn_AddBarcode.TabIndex = 3;
            this.btn_AddBarcode.Tag = "Operate";
            this.btn_AddBarcode.Text = "一维码";
            this.btn_AddBarcode.UseVisualStyleBackColor = true;
            this.btn_AddBarcode.Click += new System.EventHandler(this.btn_Barcode_Click);
            // 
            // btn_AddImage
            // 
            this.btn_AddImage.Location = new System.Drawing.Point(39, 273);
            this.btn_AddImage.Name = "btn_AddImage";
            this.btn_AddImage.Size = new System.Drawing.Size(83, 22);
            this.btn_AddImage.TabIndex = 6;
            this.btn_AddImage.Tag = "Operate";
            this.btn_AddImage.Text = "插入图片";
            this.btn_AddImage.UseVisualStyleBackColor = true;
            this.btn_AddImage.Click += new System.EventHandler(this.btn_Image_Click);
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "openFileDialog1";
            // 
            // peviewDialog1
            // 
            this.peviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.peviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.peviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.peviewDialog1.Enabled = true;
            this.peviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("peviewDialog1.Icon")));
            this.peviewDialog1.Name = "peviewDialog1";
            this.peviewDialog1.Visible = false;
            // 
            // btn_ImportInfo
            // 
            this.btn_ImportInfo.Location = new System.Drawing.Point(80, 358);
            this.btn_ImportInfo.Name = "btn_ImportInfo";
            this.btn_ImportInfo.Size = new System.Drawing.Size(64, 22);
            this.btn_ImportInfo.TabIndex = 6;
            this.btn_ImportInfo.Tag = "Operate";
            this.btn_ImportInfo.Text = "导入信息";
            this.btn_ImportInfo.UseVisualStyleBackColor = true;
            this.btn_ImportInfo.Click += new System.EventHandler(this.btn_ImportInfo_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Clear.Location = new System.Drawing.Point(12, 234);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(44, 22);
            this.btn_Clear.TabIndex = 10;
            this.btn_Clear.Text = "清空";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 454);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Height);
            this.Controls.Add(this.txt_Width);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Print);
            this.Controls.Add(this.btn_PrintPreview);
            this.Controls.Add(this.btn_AddImage);
            this.Controls.Add(this.btn_ImportInfo);
            this.Controls.Add(this.btn_ExportInfo);
            this.Controls.Add(this.btn_BackColor);
            this.Controls.Add(this.btn_FontDialog);
            this.Controls.Add(this.btn_AddBackground);
            this.Controls.Add(this.btn_AddBarcode);
            this.Controls.Add(this.btn_AddQRCode);
            this.Controls.Add(this.btn_AddLabel);
            this.Controls.Add(this.txt_ShowString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_ShowString;
        private System.Windows.Forms.Button btn_AddLabel;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button btn_FontDialog;
        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem btn_Delete;
        private System.Windows.Forms.Button btn_ExportInfo;
        private System.Windows.Forms.Button btn_PrintPreview;
        private System.Windows.Forms.Button btn_Print;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_BackColor;
        private System.Windows.Forms.TextBox txt_Width;
        private System.Windows.Forms.TextBox txt_Height;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_AddBackground;
        private System.Windows.Forms.Button btn_AddQRCode;
        private System.Windows.Forms.Button btn_AddBarcode;
        private System.Windows.Forms.ToolStripMenuItem btn_ReSet;
        private System.Windows.Forms.Button btn_AddImage;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintPreviewDialog peviewDialog1;
        private System.Windows.Forms.Button btn_ImportInfo;
        private System.Windows.Forms.Button btn_Clear;
    }
}

