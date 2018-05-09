using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

namespace AnyTablePrint
{
    public partial class PrintTable : Form
    {
        public PrintTable()
        {
            InitializeComponent();
        }
        List<int[]> CombineList = new List<int[]>();//合并的单元格参数列表
        Point CellLocation;                         //鼠标右键点击时单元格的位置，X表示行号，Y表示列号
        StringFormat fmt = new StringFormat();      //合并单元格显示的字体格式
        int limitheight = 1100;                      //分页的限制高度
        int startRowIndex = 0, endRowIndex = 0;     //分页打印的起止打印行
        int disHeight = 0;

        #region 界面加载和控件事件
        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            CombineList.Clear();
            dgv_PreviewTable.Columns.Clear();
            dgv_PreviewTable.Rows.Clear();
            int rowcount = Convert.ToInt32(num_RowsCount.Value);
            int columncount = Convert.ToInt32(num_ColumnsCount.Value);
            for (int i = 0; i < columncount; i++)
            {
                using (DataGridViewColumn dc = new DataGridViewTextBoxColumn())
                {
                    dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv_PreviewTable.Columns.Add(dc);
                }
            }
            dgv_PreviewTable.Rows.Add(rowcount);
            fmt.LineAlignment = StringAlignment.Center;
            fmt.Alignment = StringAlignment.Near;
            fmt.FormatFlags = StringFormatFlags.LineLimit;//自动换行
            endRowIndex = dgv_PreviewTable.Rows.Count;
        }
        /// <summary>
        /// 判断是增加行还是删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void num_RowsCount_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(num_RowsCount.Value) > dgv_PreviewTable.Rows.Count)
            {
                //增加行
                dgv_PreviewTable.Rows.Add();
            }
            else
            {
                //删除行
                List<int[]> RemoveList = new List<int[]>();
                RemoveList.AddRange(CombineList.Where(a => a[0] == dgv_PreviewTable.Rows.Count - 1));
                CombineList.RemoveAll(a => RemoveList.Contains(a));
                dgv_PreviewTable.Rows.RemoveAt(dgv_PreviewTable.Rows.Count - 1);
                CombineList.ForEach(a => a[1] -= a[1] > dgv_PreviewTable.Rows.Count - 1 ? 1 : 0);
            }
        }
        /// <summary>
        /// 判断是增加列还是删除列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void num_ColumnsCount_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(num_ColumnsCount.Value) > dgv_PreviewTable.Columns.Count)
            {
                using (DataGridViewColumn dc = new DataGridViewTextBoxColumn())
                {
                    //增加列
                    dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv_PreviewTable.Columns.Add(dc);
                }
            }
            else
            {
                //删除列
                List<int[]> RemoveList = new List<int[]>();
                RemoveList.AddRange(CombineList.Where(a => a[2] == dgv_PreviewTable.Columns.Count - 1));
                CombineList.RemoveAll(a => RemoveList.Contains(a));
                dgv_PreviewTable.Columns.RemoveAt(dgv_PreviewTable.Columns.Count - 1);
                CombineList.ForEach(a => a[3] -= a[3] > dgv_PreviewTable.Columns.Count - 1 ? 1 : 0);
            }
        }
        /// <summary>
        /// 右键显示右键列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_PreviewTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgv_PreviewTable.SelectedCells != null && dgv_PreviewTable.SelectedCells.Count == 1)
                {
                    foreach (DataGridViewCell cell in dgv_PreviewTable.SelectedCells)
                        cell.Selected = false;
                    dgv_PreviewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
                CellLocation = new Point(e.RowIndex, e.ColumnIndex);
                cms_dgvRightOperation.Show(dgv_PreviewTable, dgv_PreviewTable.PointToClient(Control.MousePosition));
            }
        }
        /// <summary>
        /// 列表重绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_PreviewTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            foreach (int[] array in CombineList)
            {
                CombineCells.MergeCells(dgv_PreviewTable, e, array);
            }
        }
        /// <summary>
        /// 测试重绘界面所需要的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_test_Click(object sender, EventArgs e)
        {

            CombineList.Clear();
            for (int i = 0; i < dgv_PreviewTable.Rows.Count; i++, i++)
            {
                for (int j = 0; j < dgv_PreviewTable.Columns.Count; j++)
                {
                    dgv_PreviewTable.Rows[i].Cells[j].Value = "Test";
                    CombineList.Add(new int[4] { i, i + 1, j, j });
                }
            }
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            dgv_PreviewTable.Refresh();
            sw.Stop();

            TimeSpan ts2 = sw.Elapsed;
            MessageBox.Show("Load over use time " + ts2.TotalMilliseconds + "ms");
        }
        #endregion
        #region 右键菜单事件
        /// <summary>
        /// 上方增加一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InsertRow_Click(object sender, EventArgs e)
        {
            dgv_PreviewTable.Rows.Insert(CellLocation.X, 1);
            CombineList.ForEach(a =>
            {
                a[0] += a[0] > CellLocation.X - 1 ? 1 : 0;
                a[1] += a[1] > CellLocation.X - 1 ? 1 : 0;
            });
        }
        /// <summary>
        /// 删除当前行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteRow_Click(object sender, EventArgs e)
        {
            dgv_PreviewTable.Rows.RemoveAt(CellLocation.X);
            CombineList.ForEach(a =>
            {
                a[0] -= a[0] > CellLocation.X ? 1 : 0;
                a[1] -= a[1] > CellLocation.X ? 1 : 0;
            });
        }
        /// <summary>
        /// 左侧增加一列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InsertColumn_Click(object sender, EventArgs e)
        {
            using (DataGridViewColumn dc = new DataGridViewTextBoxColumn())
            {
                dc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv_PreviewTable.Columns.Insert(CellLocation.Y, dc);
            }
            CombineList.ForEach(a =>
            {
                a[2] += a[2] > CellLocation.Y - 1 ? 1 : 0;
                a[3] += a[3] > CellLocation.Y - 1 ? 1 : 0;
            });
        }
        /// <summary>
        /// 删除当前列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteColumn_Click(object sender, EventArgs e)
        {
            dgv_PreviewTable.Columns.RemoveAt(CellLocation.Y);
            CombineList.ForEach(a =>
            {
                a[2] -= a[2] > CellLocation.Y ? 1 : 0;
                a[3] -= a[3] > CellLocation.Y ? 1 : 0;
            });
        }
        /// <summary>
        /// 合并选中的单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ConbineTheseCells_Click(object sender, EventArgs e)
        {
            IEnumerator enumerator = dgv_PreviewTable.SelectedCells.GetEnumerator();
            int minrowindex = dgv_PreviewTable.Rows.Count;
            int maxrowindex = -1;
            int mincolumnindex = dgv_PreviewTable.Columns.Count;
            int maxcolumnindex = -1;
            while (enumerator.MoveNext())
            {
                minrowindex = Math.Min((enumerator.Current as DataGridViewCell).RowIndex, minrowindex);
                maxrowindex = Math.Max((enumerator.Current as DataGridViewCell).RowIndex, maxrowindex);
                mincolumnindex = Math.Min((enumerator.Current as DataGridViewCell).ColumnIndex, mincolumnindex);
                maxcolumnindex = Math.Max((enumerator.Current as DataGridViewCell).ColumnIndex, maxcolumnindex);
            }
            bool banadd = false;
            int[] addarray = new int[4] { minrowindex, maxrowindex, mincolumnindex, maxcolumnindex };
            //判断交集
            foreach (int[] a in CombineList)
            {
                //任意一个单元格处于以合并的单元格内部，判断为有交集
                if (banadd = IntersectionExist(addarray, a))
                    break;
            }
            //添加大的，删除小的
            CombineList.RemoveAll(a => a[0] >= minrowindex && a[1] <= maxrowindex && a[2] >= mincolumnindex && a[3] <= maxcolumnindex);
            if (!banadd)
            {
                CombineList.Add(addarray);
            }
            dgv_PreviewTable.Refresh();
        }
        /// <summary>
        /// 判断arrayA和arrayB是否有交集
        /// </summary>
        /// <param name="arrayA"></param>
        /// <param name="arrayB"></param>
        /// <returns></returns>
        private bool IntersectionExist(int[] arrayA, int[] arrayB)
        {
            if (arrayA[0] <= arrayB[0] && arrayA[1] >= arrayB[1] && arrayA[2] <= arrayB[2] && arrayA[3] >= arrayB[3])
            {
                return false;
            }
            for (int i = arrayA[0]; i <= arrayA[1]; i++)
            {
                for (int j = arrayA[2]; j <= arrayA[3]; j++)
                {
                    if (arrayB[0] <= i && i <= arrayB[1] && arrayB[2] <= j && j <= arrayB[3])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 取消合并单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelCombine_Click(object sender, EventArgs e)
        {
            CombineList.RemoveAll(a => a[0] <= CellLocation.X && a[1] >= CellLocation.X && a[2] <= CellLocation.Y && a[3] >= CellLocation.Y);
            dgv_PreviewTable.Refresh();
        }
        /// <summary>
        /// 界面刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            dgv_PreviewTable.Refresh();
        }
        /// <summary>
        /// 清空合并列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ClearCombine_Click(object sender, EventArgs e)
        {
            CombineList.Clear();
            dgv_PreviewTable.Refresh();
        }
        #endregion
        #region 打印事件
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PrintPreview_Click(object sender, EventArgs e)
        {
            printDocument.DefaultPageSettings.PaperSize = printDocument.PrinterSettings.PaperSizes[8];
            printDocument.DefaultPageSettings.Landscape = true;
            using (PrintPreviewDialog printpreviewdialog = new PrintPreviewDialog())
            {
                printpreviewdialog.Document = printDocument;
                Form frmpreview = printpreviewdialog;
                frmpreview.WindowState = FormWindowState.Maximized;
                printpreviewdialog.ShowDialog();
            }
        }
        /// <summary>
        /// 打印文件生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;  
            #region 分页打印
            #region 判断分页
            int startX = 30, startY = 30;
            DataGridViewCell currentcell = dgv_PreviewTable.Rows[0].Cells[0];                   //当前单元格
            int height = 0;
            for (int i = startRowIndex; i < endRowIndex; i++)
            {
                currentcell = dgv_PreviewTable.Rows[i].Cells[0];
                height += currentcell.Size.Height;
                if (height > limitheight)
                {
                    endRowIndex = i;
                    break;
                }
            }
            foreach (int[] array in CombineList)
            {
                if (array[0] < endRowIndex && array[1] >= endRowIndex)
                    endRowIndex = array[0];
            }
            if (startRowIndex == endRowIndex)
            {
                e.HasMorePages = false;
                startRowIndex = 0;
                disHeight = 0;
                endRowIndex = dgv_PreviewTable.Rows.Count;
                MessageBox.Show("单元格长度超出限定值，无法分页打印！");
                return;
            }
            #endregion
            #region 画单元格
            //根据单元格一个一个算比例一个一个单元格的画,再判断哪些线需要画
            using (Pen lineColor = new Pen(Color.Black, 0.2f))
            {
                Point ltPoint = new Point(startX, startY);                                          //左上顶点
                Point rtPoint = ltPoint, lbPoint = ltPoint, rbPoint = ltPoint;
                for (int i = startRowIndex; i < endRowIndex; i++)
                {
                    for (int j = 0; j < dgv_PreviewTable.Columns.Count; j++)
                    {
                        currentcell = dgv_PreviewTable.Rows[i].Cells[j];
                        rtPoint = new Point(ltPoint.X + currentcell.Size.Width, ltPoint.Y);         //右上顶点
                        lbPoint = new Point(ltPoint.X, ltPoint.Y + currentcell.Size.Height);        //左下顶点
                        rbPoint = new Point(rtPoint.X, lbPoint.Y);                                  //右下顶点
                        int paint = JudgeKindofCell(dgv_PreviewTable, currentcell);
                        if (paint > 15)
                        {
                            FillSingleCell(dgv_PreviewTable, currentcell, e, new Point(startX, startY));
                        }
                        string paintstr = Convert.ToString(paint, 2).PadLeft(5, '0');
                        if (paintstr[1] == '1')
                        {
                            e.Graphics.DrawLine(lineColor, ltPoint, lbPoint);                       //左上左下，左边线
                        }
                        if (paintstr[2] == '1' || currentcell.RowIndex == startRowIndex)
                        {
                            e.Graphics.DrawLine(lineColor, ltPoint, rtPoint);                       //左上右上，上边线
                        }
                        if (paintstr[3] == '1')
                        {
                            e.Graphics.DrawLine(lineColor, rtPoint, rbPoint);                       //右上右下，右边线
                        }
                        if (paintstr[4] == '1')
                        {
                            e.Graphics.DrawLine(lineColor, lbPoint, rbPoint);                       //左下右下，下边线
                        }
                        ltPoint = rtPoint;
                    }
                    ltPoint = new Point(startX, lbPoint.Y);
                }
            }
            #endregion
            #region 打印文本
            //添加打印的文本
            string cellText;                                                               //需要打印的文本
            foreach (int[] array in CombineList)
            {
                if (array[0] >= startRowIndex && array[1] <= endRowIndex)
                {
                    cellText = "";
                    for (int i = array[0]; i <= array[1]; i++)
                    {
                        for (int j = array[2]; j <= array[3]; j++)
                        {
                            cellText += dgv_PreviewTable.Rows[i].Cells[j].Value == null ? "" : dgv_PreviewTable.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                    int Top = startY;
                    int Left = startX;
                    for (int i = startRowIndex; i < array[0]; i++)
                    {
                        Top += dgv_PreviewTable.Rows[i].Cells[0].Size.Height;
                    }
                    for (int i = 0; i < array[2]; i++)
                    {
                        Left += dgv_PreviewTable.Rows[0].Cells[i].Size.Width;
                    }
                    Point startPos = new Point(Left, Top);//起始单元格左上位置
                    Top = 0;
                    Left = 0;
                    for (int i = array[0]; i <= array[1]; i++)
                    {
                        Top += dgv_PreviewTable.Rows[i].Cells[0].Size.Height;
                    }
                    for (int i = array[2]; i <= array[3]; i++)
                    {
                        Left += dgv_PreviewTable.Rows[0].Cells[i].Size.Width;
                    }
                    Font tFont = dgv_PreviewTable.DefaultCellStyle.Font;
                    using (Brush tBrush = new SolidBrush(dgv_PreviewTable.DefaultCellStyle.ForeColor))
                    {
                        Rectangle reBounds = new Rectangle();
                        reBounds.X = startPos.X;
                        reBounds.Y = startPos.Y;
                        reBounds.Width = Left - 1;
                        reBounds.Height = Top - 1;
                        e.Graphics.DrawString(cellText, tFont, tBrush, reBounds, fmt);
                    }
                }
            }
            #endregion
            if (endRowIndex < dgv_PreviewTable.Rows.Count)
            {
                disHeight = dgv_PreviewTable.GetRowDisplayRectangle(endRowIndex, false).Y - 1;
                startRowIndex = endRowIndex;
                endRowIndex = dgv_PreviewTable.Rows.Count;
                e.HasMorePages = true;
            }
            else
            {
                disHeight = 0;
                startRowIndex = 0;
                endRowIndex = dgv_PreviewTable.Rows.Count;
                e.HasMorePages = false;
            }
            #endregion
            #region 单页打印
            ////根据单元格一个一个算比例一个一个单元格的画,再判断哪些线需要画
            //Pen lineColor = new Pen(Color.Black, 0.2f);
            ////表格总宽度1100
            //int startX = 30, startY = 30;                                                       //表格起始位置
            //DataGridViewCell currentcell;                                                       //当前单元格
            //Point ltPoint = new Point(startX, startY);                                          //左上顶点
            //Point rtPoint = ltPoint, lbPoint = ltPoint, rbPoint = ltPoint;
            //for (int i = 0; i < dgv_PreviewTable.Rows.Count; i++)
            //{
            //    for (int j = 0; j < dgv_PreviewTable.Columns.Count; j++)
            //    {
            //        currentcell = dgv_PreviewTable.Rows[i].Cells[j];
            //        rtPoint = new Point(ltPoint.X + currentcell.Size.Width, ltPoint.Y);         //右上顶点
            //        lbPoint = new Point(ltPoint.X, ltPoint.Y + currentcell.Size.Height);        //左下顶点
            //        rbPoint = new Point(rtPoint.X, lbPoint.Y);                                  //右下顶点
            //        int paint = JudgeKindofCell(dgv_PreviewTable, currentcell);
            //        if (paint > 15)
            //            FillSingleCell(dgv_PreviewTable, currentcell, e, new Point(startX, startY));
            //        string paintstr = Convert.ToString(paint, 2).PadLeft(5, '0');
            //        if (paintstr[1] == '1')
            //            e.Graphics.DrawLine(lineColor, ltPoint, lbPoint);                       //左上左下，左边线
            //        if (paintstr[2] == '1')
            //            e.Graphics.DrawLine(lineColor, ltPoint, rtPoint);                       //左上右上，上边线
            //        if (paintstr[3] == '1')
            //            e.Graphics.DrawLine(lineColor, rtPoint, rbPoint);                       //右上右下，右边线
            //        if (paintstr[4] == '1')
            //            e.Graphics.DrawLine(lineColor, lbPoint, rbPoint);                       //左下右下，下边线
            //        ltPoint = rtPoint;
            //    }
            //    ltPoint = new Point(startX, lbPoint.Y);
            //}
            ////添加打印的文本
            //string cellText;                                                               //需要打印的文本
            //foreach (int[] array in CombineList)
            //{
            //    cellText = "";
            //    for (int i = array[0]; i <= array[1]; i++)
            //        for (int j = array[2]; j <= array[3]; j++)
            //            cellText += dgv_PreviewTable.Rows[i].Cells[j].Value == null ? "" : dgv_PreviewTable.Rows[i].Cells[j].Value.ToString();
            //    int Top = startY;
            //    int Left = startX;
            //    for (int i = 0; i < array[0]; i++)
            //        Top += dgv_PreviewTable.Rows[i].Cells[0].Size.Height;
            //    for (int i = 0; i < array[2]; i++)
            //        Left += dgv_PreviewTable.Rows[0].Cells[i].Size.Width;
            //    Point startPos = new Point(Left, Top);//起始单元格左上位置
            //    Top = 0;
            //    Left = 0;
            //    for (int i = array[0]; i <= array[1]; i++)
            //        Top += dgv_PreviewTable.Rows[i].Cells[0].Size.Height;
            //    for (int i = array[2]; i <= array[3]; i++)
            //        Left += dgv_PreviewTable.Rows[0].Cells[i].Size.Width;
            //    Font tFont = dgv_PreviewTable.DefaultCellStyle.Font;
            //    Brush tBrush = new SolidBrush(dgv_PreviewTable.DefaultCellStyle.ForeColor);
            //    Rectangle reBounds = new Rectangle();
            //    reBounds.X = startPos.X;
            //    reBounds.Y = startPos.Y;
            //    reBounds.Width = Left - 1;
            //    reBounds.Height = Top - 1;
            //    e.Graphics.DrawString(cellText, tFont, tBrush, reBounds, fmt);
            //}
            #endregion
        }
        /// <summary>
        /// 根据单元格判断单元格需要画的线
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int JudgeKindofCell(DataGridView dgv, DataGridViewCell cell)
        {
            //用四位二进制数来表示是否画线1，1表示画，0表示不画，从最高位开始分别为左边线，上边线，右边线，下边线
            //四个角
            int[] array = JudgeCollectionofCell(dgv, cell);
            if (array == null)
            {
                #region 不属于合并单元格的范围
                //一、画4条线的
                if (cell.RowIndex == 0 && cell.ColumnIndex == 0)    //四边线全画，返回31,11111
                {
                    return 31;
                }
                //二、画3条线的
                else if (cell.RowIndex == 0)                        //不画左边线，返回23,10111
                {
                    return 23;
                }
                else if (cell.ColumnIndex == 0)                     //不画上边线，返回27,11011
                {
                    return 27;
                }
                //三、画2条线的
                else                                                //画右边线和下边线，返回19,10011
                {
                    return 19;
                }
                #endregion
            }
            else
            {
                #region 属于合并单元格的范围
                //一、画4条线的
                if (array[0] == cell.RowIndex && cell.RowIndex == array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex == array[3])   //画四条线，返回15,01111
                {
                    return 15;
                }
                //二、画3条线的
                else if (array[0] == cell.RowIndex && cell.RowIndex == array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex < array[3])    //不画右边线，返回13,01011
                {
                    return 13;
                }
                else if (array[0] == cell.RowIndex && cell.RowIndex < array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex == array[3])    //不画下边线，返回14,01110
                {
                    return 14;
                }
                else if (array[0] == cell.RowIndex && cell.RowIndex == array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex == array[3])    //不画左边线，返回7,00111
                {
                    return 7;
                }
                else if (array[0] < cell.RowIndex && cell.RowIndex == array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex == array[3])    //不画上边线，返回11,01011
                {
                    return 11;
                }
                //三、画2条线的
                else if (array[0] == cell.RowIndex && cell.RowIndex < array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex < array[3])     //画左边线和上边线，返回12,01100
                {
                    return 12;
                }
                else if (array[0] == cell.RowIndex && cell.RowIndex < array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex == array[3])     //画上边线和右边线，返回6，00110
                {
                    return 6;
                }
                else if (array[0] < cell.RowIndex && cell.RowIndex == array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex == array[3])     //画右边线和下边线，返回3,00011
                {
                    return 3;
                }
                else if (array[0] < cell.RowIndex && cell.RowIndex == array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex < array[3])     //画下边线和左边线，返回9，01001
                {
                    return 9;
                }
                else if (array[0] == cell.RowIndex && cell.RowIndex == array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex < array[3])     //画上边线和下边线，返回5,00101
                {
                    return 5;
                }
                else if (array[0] < cell.RowIndex && cell.RowIndex < array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex == array[3])     //画左边线和右边线，返回10,01010
                {
                    return 10;
                }
                //四、画1条线的
                else if (array[0] < cell.RowIndex && cell.RowIndex < array[1] && array[2] == cell.ColumnIndex && cell.ColumnIndex < array[3])      //画左边线，返回8，01000
                {
                    return 8;
                }
                else if (array[0] == cell.RowIndex && cell.RowIndex < array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex < array[3])      //画上边线，返回4，00100
                {
                    return 4;
                }
                else if (array[0] < cell.RowIndex && cell.RowIndex < array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex == array[3])      //画右边线，返回2,00010
                {
                    return 2;
                }
                else if (array[0] < cell.RowIndex && cell.RowIndex == array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex < array[3])      //画下边线，返回1,00001
                {
                    return 1;
                }
                //五、不画的
                else if (array[0] < cell.RowIndex && cell.RowIndex < array[1] && array[2] < cell.ColumnIndex && cell.ColumnIndex < array[3])       //一条不画,返回0,00000
                {
                    return 0;
                }
                #endregion
            }
            return 0;
        }
        /// <summary>
        /// 判断单元格是否属于某个已合并的单元格内
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int[] JudgeCollectionofCell(DataGridView dgv, DataGridViewCell cell)
        {
            foreach (int[] array in CombineList)
            {
                if (array[0] <= cell.RowIndex && cell.RowIndex <= array[1] && array[2] <= cell.ColumnIndex && cell.ColumnIndex <= array[3])
                {
                    return array;
                }
            }
            return null;
        }
        /// <summary>
        /// 填写每个单元格的内容
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cell"></param>
        /// <param name="e"></param>
        /// <param name="start"></param>
        private void FillSingleCell(DataGridView dgv, DataGridViewCell cell, System.Drawing.Printing.PrintPageEventArgs e, Point start)
        {
            string cellText = cell.Value == null ? "" : cell.Value.ToString();
            int Top = start.Y;
            int Left = start.X;
            for (int i = 0; i < cell.RowIndex; i++)
            {
                Top += dgv_PreviewTable.Rows[i].Cells[0].Size.Height;
            }
            for (int i = 0; i < cell.ColumnIndex; i++)
            {
                Left += dgv_PreviewTable.Rows[0].Cells[i].Size.Width;
            }
            Point startPos = new Point(Left, Top);//起始单元格左上位置
            Font tFont = dgv_PreviewTable.DefaultCellStyle.Font;
            using (Brush tBrush = new SolidBrush(dgv_PreviewTable.DefaultCellStyle.ForeColor))
            {
                Rectangle reBounds = new Rectangle();
                reBounds.X = startPos.X;
                reBounds.Y = startPos.Y - disHeight;
                reBounds.Width = cell.Size.Width - 1;
                reBounds.Height = cell.Size.Height - 1;
                e.Graphics.DrawString(cellText, tFont, tBrush, reBounds, fmt);
            }
        }
        #endregion
    }
}
