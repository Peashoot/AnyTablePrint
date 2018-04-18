using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace AnyTablePrint
{
    /// <summary>
    /// 合并单元格的方向
    /// </summary>
    public enum MergeDirection
    {
        /// <summary>
        /// 横向
        /// </summary>
        Horizontal = 0,
        /// <summary>
        /// 纵向
        /// </summary>
        Vertical = 1,
    }
    /// <summary>
    /// 合并单元格操作类(不支持L形合并)
    /// </summary>
    public class CombineCells
    {
        private static Dictionary<string, Rectangle> rowSpan = new Dictionary<string, Rectangle>();//取得需要重新绘制的单元格
        private static Dictionary<string, string> rowValue = new Dictionary<string, string>();//重新绘制的文本框内容

        #region  单元格绘制合并
        /// <summary>
        /// 
        /// DataGridView合并单元格
        /// </summary>
        /// <param name="dgv">要绘制的DataGridview</param>
        /// <param name="cellArgs">绘制单元格的参数（DataGridview的CellPainting事件中参数）</param>
        /// <param name="minColIndex">起始单元格在DataGridView中的索引号</param>
        /// <param name="maxColIndex">结束单元格在DataGridView中的索引号</param>
        /// <param name="kind">合并单元格的方向</param>
        public static void MergeCells(DataGridView dgv, DataGridViewCellPaintingEventArgs cellArgs, int minColIndex, int maxColIndex, MergeDirection kind)
        {
            if (kind == MergeDirection.Horizontal)
            {
                MergeCells(dgv, cellArgs, new int[] { cellArgs.RowIndex, cellArgs.RowIndex, minColIndex, maxColIndex });
            }
            else
            {
                MergeCells(dgv, cellArgs, new int[] { minColIndex, maxColIndex, cellArgs.ColumnIndex, cellArgs.ColumnIndex });
            }
        }
        /// <summary>
        /// 多行多列合并单元格
        /// </summary>
        /// <param name="dgv">要绘制的DataGridview</param>
        /// <param name="cellArgs">绘制单元格的参数（DataGridview的CellPainting事件中参数）</param>
        /// <param name="indexArray">行最小值，行最大值，列最小值，列最大值</param>
        public static void MergeCells(DataGridView dgv, DataGridViewCellPaintingEventArgs cellArgs, int[] indexArray)
        {
            using (StringFormat fmt = new StringFormat())
            {
                fmt.LineAlignment = StringAlignment.Center;
                fmt.Alignment = StringAlignment.Near;
                fmt.FormatFlags = StringFormatFlags.LineLimit;//自动换行
                MergeCells(dgv, cellArgs, indexArray, dgv.GridColor, dgv.DefaultCellStyle.Font, fmt);
            }
        }
        /// <summary>
        /// 多行多列合并单元格
        /// </summary>
        /// <param name="dgv">要绘制的DataGridview</param>
        /// <param name="cellArgs">绘制单元格的参数（DataGridview的CellPainting事件中参数）</param>
        /// <param name="indexArray">行最小值，行最大值，列最小值，列最大值</param>
        /// <param name="color">边框颜色</param>
        /// <param name="foreFont">文本字体</param>
        /// <param name="alignment">文本对齐方式</param>
        public static void MergeCells(DataGridView dgv, DataGridViewCellPaintingEventArgs cellArgs, int[] indexArray, Color color, Font foreFont, StringFormat foreFormat)
        {
            if (indexArray[0] > indexArray[1] || indexArray[2] > indexArray[3] || indexArray[0] < 0 || indexArray[1] < 0 || indexArray[2] < 0 || indexArray[3] < 0 || cellArgs.RowIndex < indexArray[0] || cellArgs.RowIndex > indexArray[1] || cellArgs.ColumnIndex < indexArray[2] || cellArgs.ColumnIndex > indexArray[3])
            {
                return;
            }
            string Index = cellArgs.RowIndex + "," + cellArgs.ColumnIndex;
            Rectangle rect = new Rectangle();
            using (Brush backColorBrush = new SolidBrush(cellArgs.CellStyle.BackColor))
            {
                cellArgs.Graphics.FillRectangle(backColorBrush, cellArgs.CellBounds);
            }
            cellArgs.Handled = true;

            if (!rowSpan.ContainsKey(Index))
            {
                //首先判断当前单元格是不是需要重绘的单元格
                //保留此单元格的信息，并抹去此单元格的背景
                rect.X = cellArgs.CellBounds.X;
                rect.Y = cellArgs.CellBounds.Y;
                rect.Width = cellArgs.CellBounds.Width;
                rect.Height = cellArgs.CellBounds.Height;
                if (!rowValue.ContainsKey(indexArray[0] + "," + indexArray[2]))
                {
                    rowValue.Add(indexArray[0] + "," + indexArray[2], cellArgs.Value == null ? "" : cellArgs.Value.ToString());
                }
                else
                {
                    rowValue[indexArray[0] + "," + indexArray[2]] += cellArgs.Value == null ? "" : cellArgs.Value.ToString();
                }
                rowSpan.Add(Index, rect);
                if (cellArgs.RowIndex == indexArray[1] && cellArgs.ColumnIndex == indexArray[3])
                {
                    MergePrint(dgv, cellArgs, indexArray, color, foreFont, foreFormat);
                }
                return;
            }
            else
            {
                if (!rowValue.ContainsKey(indexArray[0] + "," + indexArray[2]))
                {
                    rowValue.Add(indexArray[0] + "," + indexArray[2], cellArgs.Value == null ? "" : cellArgs.Value.ToString());
                }
                else
                {
                    rowValue[indexArray[0] + "," + indexArray[2]] += cellArgs.Value == null ? "" : cellArgs.Value.ToString();
                }
                IsPostMerge(dgv, cellArgs, indexArray, color, foreFont, foreFormat);
            }
        }
        /// <summary>
        /// 不是初次单元格绘制
        /// </summary>
        /// <param name="dgv">要绘制的DataGridview</param>
        /// <param name="cellArgs">绘制单元格的参数（DataGridview的CellPainting事件中参数）</param>
        /// <param name="minColIndex">起始单元格在DataGridView中的索引号</param>
        /// <param name="maxColIndex">结束单元格在DataGridView中的索引号</param>
        /// <param name="kind">合并单元格的方向</param>
        /// <param name="indexArray">行最小值，行最大值，列最小值，列最大值</param>
        /// <param name="color">边框颜色</param>
        /// <param name="foreFont">文本字体</param>
        /// <param name="alignment">文本对齐方式</param>
        public static void IsPostMerge(DataGridView dgv, DataGridViewCellPaintingEventArgs cellArgs, int[] indexArray, Color color, Font foreFont, StringFormat foreFormat)
        {
            string Index = cellArgs.RowIndex + "," + cellArgs.ColumnIndex;
            //比较单元是否有变化
            Rectangle rectArgs = (Rectangle)rowSpan[Index];
            if (rectArgs.X != cellArgs.CellBounds.X || rectArgs.Y != cellArgs.CellBounds.Y || rectArgs.Width != cellArgs.CellBounds.Width || rectArgs.Height != cellArgs.CellBounds.Height)
            {
                rectArgs.X = cellArgs.CellBounds.X;
                rectArgs.Y = cellArgs.CellBounds.Y;
                rectArgs.Width = cellArgs.CellBounds.Width;
                rectArgs.Height = cellArgs.CellBounds.Height;
                rowSpan[Index] = rectArgs;
            }
            if (cellArgs.RowIndex == indexArray[1] && cellArgs.ColumnIndex == indexArray[3])
            {
                MergePrint(dgv, cellArgs, indexArray, color, foreFont, foreFormat);
            }
        }
        /// <summary>
        /// 绘制单元格
        /// </summary>
        /// <param name="dgv">要绘制的DataGridview</param>
        /// <param name="cellArgs">绘制单元格的参数（DataGridview的CellPainting事件中参数）</param>
        /// <param name="minColIndex">起始单元格在DataGridView中的索引号</param>
        /// <param name="maxColIndex">结束单元格在DataGridView中的索引号</param>
        /// <param name="kind">合并单元格的方向</param>
        /// <param name="indexArray">行最小值，行最大值，列最小值，列最大值</param>
        /// <param name="color">边框颜色</param>
        /// <param name="foreFont">文本字体</param>
        /// <param name="alignment">文本对齐方式</param>
        private static void MergePrint(DataGridView dgv, DataGridViewCellPaintingEventArgs cellArgs, int[] indexArray, Color color, Font foreFont, StringFormat foreFormat)
        {
            int width = 0;
            int height = 0;
            for (int i = indexArray[2]; i <= indexArray[3]; i++)
            {
                width += rowSpan[indexArray[0] + "," + i].Width;//合并后单元格总宽度
            }
            for (int i = indexArray[0]; i <= indexArray[1]; i++)
            {
                height += rowSpan[i + "," + indexArray[2]].Height;//合并后单元格总高度
            }
            Rectangle rectBegin = rowSpan[indexArray[0] + "," + indexArray[2]];//合并第一个单元格的位置信息
            Rectangle rectEnd = rowSpan[indexArray[1] + "," + indexArray[3]];//合并最后一个单元格的位置信息
            Rectangle reBounds = new Rectangle(new Point(rectBegin.X, rectBegin.Y), new Size(width - 1, height - 1));
            using (Brush gridBrush = new SolidBrush(color))
            {
                using (Pen gridLinePen = new Pen(gridBrush))
                {
                    // 画出上下两条边线，左右边线无
                    Point blPoint = new Point(rectBegin.Left, rectEnd.Bottom - 1);//底线左边位置
                    Point brPoint = new Point(rectEnd.Right - 1, rectEnd.Bottom - 1);//底线右边位置
                    cellArgs.Graphics.DrawLine(gridLinePen, blPoint, brPoint);//下边线

                    Point tlPoint = new Point(rectBegin.Left, rectBegin.Top == 0 ? rectBegin.Top : rectBegin.Top - 1);//上边线左边位置
                    Point trPoint = new Point(rectEnd.Right - 1, rectBegin.Top == 0 ? rectBegin.Top : rectBegin.Top - 1);//上边线右边位置
                    cellArgs.Graphics.DrawLine(gridLinePen, tlPoint, trPoint); //上边线

                    Point ltPoint = new Point(rectBegin.Left == 0 ? rectBegin.Left : rectBegin.Left - 1, rectBegin.Top);//左边线顶部位置
                    Point lbPoint = new Point(rectBegin.Left == 0 ? rectBegin.Left : rectBegin.Left - 1, rectEnd.Bottom - 1);//左边线底部位置
                    cellArgs.Graphics.DrawLine(gridLinePen, ltPoint, lbPoint); //左边线

                    Point rtPoint = new Point(rectEnd.Right - 1, rectBegin.Top);//右边线顶部位置
                    Point rbPoint = new Point(rectEnd.Right - 1, rectEnd.Bottom - 1);//右边线底部位置
                    cellArgs.Graphics.DrawLine(gridLinePen, rtPoint, rbPoint); //右边线
                    using (Brush foreBrush = new SolidBrush(cellArgs.CellStyle.ForeColor))
                    {
                        //画出文本框
                        if (rowValue[indexArray[0] + "," + indexArray[2]] != "")
                        {
                            cellArgs.Graphics.DrawString(rowValue[indexArray[0] + "," + indexArray[2]], foreFont, foreBrush, reBounds, foreFormat);
                            rowValue.Remove(indexArray[0] + "," + indexArray[2]);
                        }
                    }
                }
                cellArgs.Handled = true;
            }
        }
        /// <summary>
        /// 批量合并单元格（已知每个需要合并的单元格的标号）
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellArgs"></param>
        /// <param name="minColIndex"></param>
        /// <param name="maxColIndex"></param>
        /// <param name="kind"></param>
        public static void MergeCellsBatch(DataGridView dgv, DataGridViewCellPaintingEventArgs cellArgs, Point[] pointArray, MergeDirection kind)
        {
            if (pointArray != null && pointArray.Length > 0)
            {
                foreach (Point point in pointArray)
                {
                    MergeCells(dgv, cellArgs, point.X, point.Y, MergeDirection.Horizontal);
                }
            }
        }
        /// <summary> 
        /// 批量合并单元格（已知需要合并的单元格是连在一起的）
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellArgs"></param>
        /// <param name="indexArray"></param>
        /// <param name="kind"></param>
        public static void MergeCellsBatch(DataGridView dgv, DataGridViewCellPaintingEventArgs cellArgs, int[] indexArray, MergeDirection kind)
        {
            if (indexArray != null && indexArray.Length > 0)
            {
                for (int i = 0; i < indexArray.Length - 1; i++)
                {
                    MergeCells(dgv, cellArgs, indexArray[i], indexArray[i + 1] - 1, MergeDirection.Horizontal);
                }
                MergeCells(dgv, cellArgs, indexArray[indexArray.Length - 1], dgv.Columns.Count - 1, MergeDirection.Horizontal);
            }
        }
        #endregion
    }
}
