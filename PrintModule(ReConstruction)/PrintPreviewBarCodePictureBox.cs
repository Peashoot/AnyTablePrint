using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

namespace PrintModule_ReConstruction_
{
    internal class PrintPreviewBarCodePictureBox : PrintPreviewPictureBox
    {
        public PrintPreviewBarCodePictureBox(Panel panel, ExportInfo exinfo)
            : base(panel, exinfo)
        {
        }

        /// <summary>
        /// 图片填充PictureBox
        /// </summary>
        /// <param name="exinfo"></param>
        public override void GeneratePictureBoxFillImage(ExportInfo exinfo)
        {
            if (String.IsNullOrEmpty(exinfo.TagInfo.Info))
            {
                MessageBox.Show(this, "条码内容不能为空！");
                return;
            }
            PicImage = GetBarCodeByZXingNet(exinfo.TagInfo.Info, BelongPanel.Width, BelongPanel.Width / 3);
            AddPictureBox(PicImage, exinfo);
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
    }
}