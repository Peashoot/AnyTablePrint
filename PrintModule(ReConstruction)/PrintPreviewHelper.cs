using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;

namespace PrintModule_ReConstruction_
{
    public class PrintPreviewHelper : IDisposable
    {

        /// <summary>
        /// 是否已经Dispose
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// 打印文档
        /// </summary>
        private PrintDocument _printDocument;
        /// <summary>
        /// 要打印的Panel
        /// </summary>
        private Panel _printPanel;

        public PrintPreviewHelper(Panel panel)
        {
            _printPanel = panel;
            _printDocument = new PrintDocument();
            //逐份打印
            _printDocument.DefaultPageSettings.PrinterSettings.Collate = true;
            //打印的份数
            _printDocument.DefaultPageSettings.PrinterSettings.Copies = 1;
            //指定边距
            _printDocument.DefaultPageSettings.Margins.Top = 0;
            _printDocument.DefaultPageSettings.Margins.Left = 0;
            _printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            foreach (Control control in _printPanel.Controls)
            {
                if (control.GetType() == typeof(PrintPreviewLabel))
                {
                    using (Brush brush = new SolidBrush(control.ForeColor))
                    {
                        e.Graphics.DrawString(control.Text, control.Font, brush, control.Location);
                    }
                }
                else if (control.GetType() == typeof(PrintPreviewBackgroundPictureBox))
                {
                    using (Brush brush = new SolidBrush(control.BackColor))
                    {
                        e.Graphics.FillRegion(brush, control.Region);
                    }
                }
                else if (control.GetType().BaseType == typeof(PrintPreviewPictureBox))
                {
                    e.Graphics.DrawImage((control as PictureBox).Image, control.Location);
                }
            }
        }

        public void BindDefaultPrinter(string printerName)
        {
            _printDocument.DefaultPageSettings.PrinterSettings.PrinterName = printerName;
        }

        public PaperSize[] GetPaperSizeCollection()
        {
            PaperSize[] paperSizeList = new PaperSize[_printDocument.PrinterSettings.PaperSizes.Count];
            _printDocument.PrinterSettings.PaperSizes.CopyTo(paperSizeList, 0);
            return paperSizeList;
        }

        private void SetPrintPagperSize()
        {
            PaperSize paperSize = new PaperSize("自定义页面", _printPanel.Width, _printPanel.Height);
            _printDocument.DefaultPageSettings.PaperSize = paperSize;
        }

        private void SetPrintLandScape(bool landScape)
        {
            _printDocument.DefaultPageSettings.Landscape = landScape;
        }

        private void PrinterSetting(string printerName, bool landScape)
        {
            BindDefaultPrinter(printerName);
            SetPrintPagperSize();
            SetPrintLandScape(landScape);
        }

        public void ShowPrintDialog(string printerName, bool landScape)
        {
            PrinterSetting(printerName, landScape);
            using (PrintPreviewDialog printpreviewdialog = new PrintPreviewDialog())
            {
                printpreviewdialog.Document = _printDocument;
                printpreviewdialog.ShowDialog();
            }
        }

        public void BeginPrint(string printerName, bool landScape)
        {
            PrinterSetting(printerName, landScape);
            _printDocument.Print();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                _printDocument.Dispose();
            }
            disposed = true;
        }
    }
}
