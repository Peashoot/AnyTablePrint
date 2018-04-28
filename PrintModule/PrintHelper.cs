using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;

namespace PrintModule
{
    public class PrintHelper
    {
        #region 成员字段

        /// <summary>
        /// 打印机操作对象
        /// </summary>
        PrintDocument printDocument = null;

        private List<Bitmap> list_Bitmap = new List<Bitmap>();

        private int pageNumber = 0;
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PrintHelper(List<Bitmap> list_Bitmap, string PrinterName)
        {
            //初始化
            printDocument = new PrintDocument();
            //启用页边距
            printDocument.OriginAtMargins = true;
            //绑定打印的图片
            this.list_Bitmap = list_Bitmap;
            //设置打印机相关信息
            PrintSetting(ConfigInfo.PageSizeWeight, ConfigInfo.PageSizeHeight, PrinterName);
            //PrintSetting(315, 400, "TG890GZ");
            //绑定打印事件
            printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            //打印完成进行释放
            printDocument.EndPrint += new PrintEventHandler(this.EndPrintEvent);
        }

        public PrintHelper(string PrinterName)
        {
            //初始化
            printDocument = new PrintDocument();
            //启用页边距
            printDocument.OriginAtMargins = true;
            //设置打印机相关信息
            PrintSetting(ConfigInfo.PageSizeWeight, ConfigInfo.PageSizeHeight, PrinterName);
            //绑定打印事件
            printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            //打印完成进行释放
            printDocument.EndPrint += new PrintEventHandler(this.EndPrintEvent);
        }

        #endregion

        #region 函数事件方法

        public void PrintBitmapSet(List<Bitmap> list_Bitmap)
        {
            this.list_Bitmap = list_Bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Wedth"></param>
        /// <param name="Height"></param>
        /// <param name="PrinterName"></param>
        public void PrintSetting(int Wedth, int Height, string PrinterName)
        {
            //设置页面大小
            PaperSize paperSize = new PaperSize("用户自己定页面", Wedth, Height);
            //使用这个页面设置
            printDocument.DefaultPageSettings.PaperSize = paperSize;

            //设置打印的时候的所使用re打印机
            printDocument.DefaultPageSettings.PrinterSettings.PrinterName = PrinterName;
            //逐分打印
            printDocument.DefaultPageSettings.PrinterSettings.Collate = true;
            //打印的分数
            printDocument.DefaultPageSettings.PrinterSettings.Copies = 1;
            //指定边距
            printDocument.DefaultPageSettings.Margins.Top = 0;
            printDocument.DefaultPageSettings.Margins.Left = 0;
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (list_Bitmap.Count <= 0)
            {
                e.Cancel = true;//如果没有要打印的那么就取消打印
            }

            try
            {
                if (pageNumber < list_Bitmap.Count)
                {
                    e.Graphics.DrawImage(list_Bitmap[pageNumber], new PointF(0, 0));
                    pageNumber++;//先加
                }
            }
            catch (Exception ex)
            {
                pageNumber++;
            }

            if (pageNumber < list_Bitmap.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                pageNumber = 0;//打印完了
                e.HasMorePages = false;
            }
        }

        /// <summary>
        /// 开始打印
        /// </summary>
        public void PrintPageStart()
        {
            //PrintDialog printDialog = new PrintDialog();
            //printDialog.Document = printDocument;
            //if (printDialog.ShowDialog() == DialogResult.OK)
            //{
            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {

            }
            //}
        }

        //释放资源
        private void EndPrintEvent(object obj, PrintEventArgs e)
        {
            list_Bitmap = null;
        }
        #endregion 
    }
}
