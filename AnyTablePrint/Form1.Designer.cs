namespace AnyTablePrint
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
                fmt.Dispose();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.num_RowsCount = new System.Windows.Forms.NumericUpDown();
            this.num_ColumnsCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgv_PreviewTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cms_dgvRightOperation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btn_InsertRow = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_DeleteRow = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_InsertColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_DeleteColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_ConbineTheseCells = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_CancelCombine = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_ClearCombine = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_PrintPreview = new System.Windows.Forms.Button();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.btn_test = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_RowsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ColumnsCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PreviewTable)).BeginInit();
            this.cms_dgvRightOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "行数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "列数";
            // 
            // num_RowsCount
            // 
            this.num_RowsCount.Location = new System.Drawing.Point(68, 28);
            this.num_RowsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_RowsCount.Name = "num_RowsCount";
            this.num_RowsCount.ReadOnly = true;
            this.num_RowsCount.Size = new System.Drawing.Size(120, 21);
            this.num_RowsCount.TabIndex = 2;
            this.num_RowsCount.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.num_RowsCount.ValueChanged += new System.EventHandler(this.num_RowsCount_ValueChanged);
            // 
            // num_ColumnsCount
            // 
            this.num_ColumnsCount.Location = new System.Drawing.Point(287, 28);
            this.num_ColumnsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_ColumnsCount.Name = "num_ColumnsCount";
            this.num_ColumnsCount.ReadOnly = true;
            this.num_ColumnsCount.Size = new System.Drawing.Size(120, 21);
            this.num_ColumnsCount.TabIndex = 3;
            this.num_ColumnsCount.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.num_ColumnsCount.ValueChanged += new System.EventHandler(this.num_ColumnsCount_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgv_PreviewTable);
            this.groupBox1.Location = new System.Drawing.Point(12, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1122, 738);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预览";
            // 
            // dgv_PreviewTable
            // 
            this.dgv_PreviewTable.AllowUserToAddRows = false;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_PreviewTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_PreviewTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_PreviewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_PreviewTable.ColumnHeadersVisible = false;
            this.dgv_PreviewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgv_PreviewTable.GridColor = System.Drawing.Color.Black;
            this.dgv_PreviewTable.Location = new System.Drawing.Point(10, 20);
            this.dgv_PreviewTable.Name = "dgv_PreviewTable";
            this.dgv_PreviewTable.RowHeadersVisible = false;
            this.dgv_PreviewTable.RowTemplate.Height = 23;
            this.dgv_PreviewTable.Size = new System.Drawing.Size(1100, 712);
            this.dgv_PreviewTable.TabIndex = 0;
            this.dgv_PreviewTable.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_PreviewTable_CellMouseClick);
            this.dgv_PreviewTable.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv_PreviewTable_CellPainting);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // cms_dgvRightOperation
            // 
            this.cms_dgvRightOperation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_InsertRow,
            this.btn_DeleteRow,
            this.btn_InsertColumn,
            this.btn_DeleteColumn,
            this.btn_ConbineTheseCells,
            this.btn_CancelCombine,
            this.btn_Refresh,
            this.btn_ClearCombine});
            this.cms_dgvRightOperation.Name = "cms_RightOperation";
            this.cms_dgvRightOperation.Size = new System.Drawing.Size(161, 180);
            // 
            // btn_InsertRow
            // 
            this.btn_InsertRow.Name = "btn_InsertRow";
            this.btn_InsertRow.Size = new System.Drawing.Size(160, 22);
            this.btn_InsertRow.Text = "在上方插入行";
            this.btn_InsertRow.Click += new System.EventHandler(this.btn_InsertRow_Click);
            // 
            // btn_DeleteRow
            // 
            this.btn_DeleteRow.Name = "btn_DeleteRow";
            this.btn_DeleteRow.Size = new System.Drawing.Size(160, 22);
            this.btn_DeleteRow.Text = "删除当前行";
            this.btn_DeleteRow.Click += new System.EventHandler(this.btn_DeleteRow_Click);
            // 
            // btn_InsertColumn
            // 
            this.btn_InsertColumn.Name = "btn_InsertColumn";
            this.btn_InsertColumn.Size = new System.Drawing.Size(160, 22);
            this.btn_InsertColumn.Text = "在左边插入列";
            this.btn_InsertColumn.Click += new System.EventHandler(this.btn_InsertColumn_Click);
            // 
            // btn_DeleteColumn
            // 
            this.btn_DeleteColumn.Name = "btn_DeleteColumn";
            this.btn_DeleteColumn.Size = new System.Drawing.Size(160, 22);
            this.btn_DeleteColumn.Text = "删除当前列";
            this.btn_DeleteColumn.Click += new System.EventHandler(this.btn_DeleteColumn_Click);
            // 
            // btn_ConbineTheseCells
            // 
            this.btn_ConbineTheseCells.Name = "btn_ConbineTheseCells";
            this.btn_ConbineTheseCells.Size = new System.Drawing.Size(160, 22);
            this.btn_ConbineTheseCells.Text = "合并选中单元格";
            this.btn_ConbineTheseCells.Click += new System.EventHandler(this.btn_ConbineTheseCells_Click);
            // 
            // btn_CancelCombine
            // 
            this.btn_CancelCombine.Name = "btn_CancelCombine";
            this.btn_CancelCombine.Size = new System.Drawing.Size(160, 22);
            this.btn_CancelCombine.Text = "取消合并单元格";
            this.btn_CancelCombine.Click += new System.EventHandler(this.btn_CancelCombine_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(160, 22);
            this.btn_Refresh.Text = "刷新界面";
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // btn_ClearCombine
            // 
            this.btn_ClearCombine.Name = "btn_ClearCombine";
            this.btn_ClearCombine.Size = new System.Drawing.Size(160, 22);
            this.btn_ClearCombine.Text = "清空合并";
            this.btn_ClearCombine.Click += new System.EventHandler(this.btn_ClearCombine_Click);
            // 
            // btn_PrintPreview
            // 
            this.btn_PrintPreview.Location = new System.Drawing.Point(466, 25);
            this.btn_PrintPreview.Name = "btn_PrintPreview";
            this.btn_PrintPreview.Size = new System.Drawing.Size(81, 23);
            this.btn_PrintPreview.TabIndex = 5;
            this.btn_PrintPreview.Text = "打印预览";
            this.btn_PrintPreview.UseVisualStyleBackColor = true;
            this.btn_PrintPreview.Click += new System.EventHandler(this.btn_PrintPreview_Click);
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // btn_test
            // 
            this.btn_test.Location = new System.Drawing.Point(1017, 24);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(72, 23);
            this.btn_test.TabIndex = 7;
            this.btn_test.Text = "测试";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1146, 818);
            this.Controls.Add(this.btn_test);
            this.Controls.Add(this.btn_PrintPreview);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.num_ColumnsCount);
            this.Controls.Add(this.num_RowsCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_RowsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ColumnsCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PreviewTable)).EndInit();
            this.cms_dgvRightOperation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown num_RowsCount;
        private System.Windows.Forms.NumericUpDown num_ColumnsCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgv_PreviewTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.ContextMenuStrip cms_dgvRightOperation;
        private System.Windows.Forms.ToolStripMenuItem btn_ConbineTheseCells;
        private System.Windows.Forms.ToolStripMenuItem btn_InsertRow;
        private System.Windows.Forms.ToolStripMenuItem btn_DeleteRow;
        private System.Windows.Forms.ToolStripMenuItem btn_InsertColumn;
        private System.Windows.Forms.ToolStripMenuItem btn_DeleteColumn;
        private System.Windows.Forms.ToolStripMenuItem btn_CancelCombine;
        private System.Windows.Forms.Button btn_PrintPreview;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.Button btn_test;
        private System.Windows.Forms.ToolStripMenuItem btn_Refresh;
        private System.Windows.Forms.ToolStripMenuItem btn_ClearCombine;
    }
}

