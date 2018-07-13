namespace PrintModule_ReConstruction_
{
    partial class PrintForm
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
                _printHelper.Dispose();
                _exinfo.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm));
            this.PreviewControlPanel = new System.Windows.Forms.Panel();
            this.txt_ShowString = new System.Windows.Forms.TextBox();
            this.btn_AddLabel = new System.Windows.Forms.Button();
            this.btn_FontDialog = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btn_ReSet = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_ExportInfo = new System.Windows.Forms.Button();
            this.btn_PrintPreview = new System.Windows.Forms.Button();
            this.btn_Print = new System.Windows.Forms.Button();
            this.btn_BackColor = new System.Windows.Forms.Button();
            this.txt_Width = new System.Windows.Forms.TextBox();
            this.txt_Height = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_AddBackground = new System.Windows.Forms.Button();
            this.btn_AddQRCode = new System.Windows.Forms.Button();
            this.btn_AddBarcode = new System.Windows.Forms.Button();
            this.btn_AddImage = new System.Windows.Forms.Button();
            this.btn_ImportInfo = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmb_Printer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_PrintSize = new System.Windows.Forms.Button();
            this.btn_Screenshots = new System.Windows.Forms.Button();
            this.cmb_PrintPaperSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chk_Landscape = new System.Windows.Forms.CheckBox();
            this.num_Brightness = new System.Windows.Forms.NumericUpDown();
            this.menuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Brightness)).BeginInit();
            this.SuspendLayout();
            // 
            // PreviewControlPanel
            // 
            this.PreviewControlPanel.BackColor = System.Drawing.Color.White;
            this.PreviewControlPanel.Location = new System.Drawing.Point(160, 12);
            this.PreviewControlPanel.Name = "PreviewControlPanel";
            this.PreviewControlPanel.Size = new System.Drawing.Size(382, 430);
            this.PreviewControlPanel.TabIndex = 0;
            // 
            // txt_ShowString
            // 
            this.txt_ShowString.Location = new System.Drawing.Point(5, 21);
            this.txt_ShowString.Name = "txt_ShowString";
            this.txt_ShowString.Size = new System.Drawing.Size(130, 21);
            this.txt_ShowString.TabIndex = 2;
            // 
            // btn_AddLabel
            // 
            this.btn_AddLabel.Location = new System.Drawing.Point(13, 329);
            this.btn_AddLabel.Name = "btn_AddLabel";
            this.btn_AddLabel.Size = new System.Drawing.Size(64, 22);
            this.btn_AddLabel.TabIndex = 3;
            this.btn_AddLabel.Tag = "Operate";
            this.btn_AddLabel.Text = "添加文本";
            this.btn_AddLabel.UseVisualStyleBackColor = true;
            this.btn_AddLabel.Click += new System.EventHandler(this.btn_AddLabel_Click);
            // 
            // btn_FontDialog
            // 
            this.btn_FontDialog.Location = new System.Drawing.Point(4, 48);
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
            // 
            // btn_Delete
            // 
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(124, 22);
            this.btn_Delete.Text = "删除";
            // 
            // btn_ExportInfo
            // 
            this.btn_ExportInfo.Location = new System.Drawing.Point(13, 358);
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
            this.btn_PrintPreview.Location = new System.Drawing.Point(60, 386);
            this.btn_PrintPreview.Name = "btn_PrintPreview";
            this.btn_PrintPreview.Size = new System.Drawing.Size(83, 22);
            this.btn_PrintPreview.TabIndex = 6;
            this.btn_PrintPreview.Tag = "Operate";
            this.btn_PrintPreview.Text = "打印预览";
            this.btn_PrintPreview.UseVisualStyleBackColor = true;
            this.btn_PrintPreview.Click += new System.EventHandler(this.btn_PrintPreview_Click);
            // 
            // btn_Print
            // 
            this.btn_Print.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Print.Location = new System.Drawing.Point(56, 414);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(44, 22);
            this.btn_Print.TabIndex = 6;
            this.btn_Print.Tag = "Operate";
            this.btn_Print.Text = "打印";
            this.btn_Print.UseVisualStyleBackColor = true;
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // btn_BackColor
            // 
            this.btn_BackColor.Location = new System.Drawing.Point(73, 48);
            this.btn_BackColor.Name = "btn_BackColor";
            this.btn_BackColor.Size = new System.Drawing.Size(64, 26);
            this.btn_BackColor.TabIndex = 4;
            this.btn_BackColor.Text = "选择背景";
            this.btn_BackColor.UseVisualStyleBackColor = true;
            this.btn_BackColor.Click += new System.EventHandler(this.btn_BackColor_Click);
            // 
            // txt_Width
            // 
            this.txt_Width.Location = new System.Drawing.Point(7, 33);
            this.txt_Width.Name = "txt_Width";
            this.txt_Width.Size = new System.Drawing.Size(61, 21);
            this.txt_Width.TabIndex = 8;
            this.txt_Width.Text = "200";
            this.txt_Width.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numText_KeyPress);
            // 
            // txt_Height
            // 
            this.txt_Height.Location = new System.Drawing.Point(73, 33);
            this.txt_Height.Name = "txt_Height";
            this.txt_Height.Size = new System.Drawing.Size(61, 21);
            this.txt_Height.TabIndex = 8;
            this.txt_Height.Text = "200";
            this.txt_Height.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numText_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "宽";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "高";
            // 
            // btn_AddBackground
            // 
            this.btn_AddBackground.Location = new System.Drawing.Point(79, 329);
            this.btn_AddBackground.Name = "btn_AddBackground";
            this.btn_AddBackground.Size = new System.Drawing.Size(64, 22);
            this.btn_AddBackground.TabIndex = 3;
            this.btn_AddBackground.Tag = "Operate";
            this.btn_AddBackground.Text = "添加背景";
            this.btn_AddBackground.UseVisualStyleBackColor = true;
            this.btn_AddBackground.Click += new System.EventHandler(this.btn_AddBackground_Click);
            // 
            // btn_AddQRCode
            // 
            this.btn_AddQRCode.Location = new System.Drawing.Point(13, 301);
            this.btn_AddQRCode.Name = "btn_AddQRCode";
            this.btn_AddQRCode.Size = new System.Drawing.Size(64, 22);
            this.btn_AddQRCode.TabIndex = 3;
            this.btn_AddQRCode.Tag = "Operate";
            this.btn_AddQRCode.Text = "二维码";
            this.btn_AddQRCode.UseVisualStyleBackColor = true;
            this.btn_AddQRCode.Click += new System.EventHandler(this.btn_AddQRCode_Click);
            // 
            // btn_AddBarcode
            // 
            this.btn_AddBarcode.Location = new System.Drawing.Point(79, 301);
            this.btn_AddBarcode.Name = "btn_AddBarcode";
            this.btn_AddBarcode.Size = new System.Drawing.Size(64, 22);
            this.btn_AddBarcode.TabIndex = 3;
            this.btn_AddBarcode.Tag = "Operate";
            this.btn_AddBarcode.Text = "一维码";
            this.btn_AddBarcode.UseVisualStyleBackColor = true;
            this.btn_AddBarcode.Click += new System.EventHandler(this.btn_AddBarcode_Click);
            // 
            // btn_AddImage
            // 
            this.btn_AddImage.Location = new System.Drawing.Point(61, 273);
            this.btn_AddImage.Name = "btn_AddImage";
            this.btn_AddImage.Size = new System.Drawing.Size(83, 22);
            this.btn_AddImage.TabIndex = 6;
            this.btn_AddImage.Tag = "Operate";
            this.btn_AddImage.Text = "插入图片";
            this.btn_AddImage.UseVisualStyleBackColor = true;
            this.btn_AddImage.Click += new System.EventHandler(this.btn_AddImage_Click);
            // 
            // btn_ImportInfo
            // 
            this.btn_ImportInfo.Location = new System.Drawing.Point(79, 358);
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
            this.btn_Clear.Location = new System.Drawing.Point(11, 414);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(44, 22);
            this.btn_Clear.TabIndex = 10;
            this.btn_Clear.Text = "清空";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txt_Height);
            this.groupBox1.Controls.Add(this.txt_Width);
            this.groupBox1.Location = new System.Drawing.Point(9, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 63);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图片、界面宽高设置";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_BackColor);
            this.groupBox2.Controls.Add(this.btn_FontDialog);
            this.groupBox2.Controls.Add(this.txt_ShowString);
            this.groupBox2.Location = new System.Drawing.Point(9, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(140, 83);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输入文本属性";
            // 
            // cmb_Printer
            // 
            this.cmb_Printer.FormattingEnabled = true;
            this.cmb_Printer.Location = new System.Drawing.Point(15, 215);
            this.cmb_Printer.Name = "cmb_Printer";
            this.cmb_Printer.Size = new System.Drawing.Size(127, 20);
            this.cmb_Printer.TabIndex = 15;
            this.cmb_Printer.SelectedIndexChanged += new System.EventHandler(this.cmb_Printer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "选择打印机：";
            // 
            // btn_PrintSize
            // 
            this.btn_PrintSize.Location = new System.Drawing.Point(15, 245);
            this.btn_PrintSize.Name = "btn_PrintSize";
            this.btn_PrintSize.Size = new System.Drawing.Size(128, 22);
            this.btn_PrintSize.TabIndex = 17;
            this.btn_PrintSize.Text = "设置打印纸张大小";
            this.btn_PrintSize.UseVisualStyleBackColor = true;
            // 
            // btn_Screenshots
            // 
            this.btn_Screenshots.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Screenshots.Location = new System.Drawing.Point(101, 414);
            this.btn_Screenshots.Name = "btn_Screenshots";
            this.btn_Screenshots.Size = new System.Drawing.Size(44, 22);
            this.btn_Screenshots.TabIndex = 10;
            this.btn_Screenshots.Text = "截取";
            this.btn_Screenshots.UseVisualStyleBackColor = true;
            this.btn_Screenshots.Click += new System.EventHandler(this.btn_Screenshots_Click);
            // 
            // cmb_PrintPaperSize
            // 
            this.cmb_PrintPaperSize.FormattingEnabled = true;
            this.cmb_PrintPaperSize.Location = new System.Drawing.Point(15, 173);
            this.cmb_PrintPaperSize.Name = "cmb_PrintPaperSize";
            this.cmb_PrintPaperSize.Size = new System.Drawing.Size(126, 20);
            this.cmb_PrintPaperSize.TabIndex = 18;
            this.cmb_PrintPaperSize.SelectedIndexChanged += new System.EventHandler(this.cmb_PrintPaperSize_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "选择打印纸张大小：";
            // 
            // chk_Landscape
            // 
            this.chk_Landscape.AutoSize = true;
            this.chk_Landscape.Location = new System.Drawing.Point(15, 390);
            this.chk_Landscape.Name = "chk_Landscape";
            this.chk_Landscape.Size = new System.Drawing.Size(48, 16);
            this.chk_Landscape.TabIndex = 19;
            this.chk_Landscape.Text = "横向";
            this.chk_Landscape.UseVisualStyleBackColor = true;
            // 
            // num_Brightness
            // 
            this.num_Brightness.Location = new System.Drawing.Point(15, 273);
            this.num_Brightness.Name = "num_Brightness";
            this.num_Brightness.Size = new System.Drawing.Size(43, 21);
            this.num_Brightness.TabIndex = 20;
            this.num_Brightness.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 451);
            this.Controls.Add(this.num_Brightness);
            this.Controls.Add(this.cmb_PrintPaperSize);
            this.Controls.Add(this.btn_PrintSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb_Printer);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_Screenshots);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Print);
            this.Controls.Add(this.btn_PrintPreview);
            this.Controls.Add(this.btn_AddImage);
            this.Controls.Add(this.btn_ImportInfo);
            this.Controls.Add(this.btn_ExportInfo);
            this.Controls.Add(this.btn_AddBackground);
            this.Controls.Add(this.btn_AddBarcode);
            this.Controls.Add(this.btn_AddQRCode);
            this.Controls.Add(this.btn_AddLabel);
            this.Controls.Add(this.PreviewControlPanel);
            this.Controls.Add(this.chk_Landscape);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 480);
            this.Name = "PrintForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印预览";
            this.Load += new System.EventHandler(this.PrintForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Brightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PreviewControlPanel;
        private System.Windows.Forms.TextBox txt_ShowString;
        private System.Windows.Forms.Button btn_AddLabel;
        private System.Windows.Forms.Button btn_FontDialog;
        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem btn_Delete;
        private System.Windows.Forms.Button btn_ExportInfo;
        private System.Windows.Forms.Button btn_PrintPreview;
        private System.Windows.Forms.Button btn_Print;
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
        private System.Windows.Forms.Button btn_ImportInfo;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmb_Printer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_PrintSize;
        private System.Windows.Forms.Button btn_Screenshots;
        private System.Windows.Forms.ComboBox cmb_PrintPaperSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_Landscape;
        private System.Windows.Forms.NumericUpDown num_Brightness;
    }
}

