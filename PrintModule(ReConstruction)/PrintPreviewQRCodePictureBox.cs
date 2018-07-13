using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ZXing;

namespace PrintModule_ReConstruction_
{
    public class PrintPreviewQRCodePictureBox : PrintPreviewPictureBox
    {
        public PrintPreviewQRCodePictureBox(Panel panel, ExportInfo exinfo = null) : base(panel, exinfo) { }
        /// <summary>
        /// 图片填充PictureBox
        /// </summary>
        /// <param name="exinfo"></param>
        public override void GeneratePictureBoxFillImage(ExportInfo exinfo = null)
        {
            if (string.IsNullOrEmpty(PanelForm.GetShowStringValue()))
            {
                MessageBox.Show(this, "二维码内容不能为空");
                return;
            }
            PicImage = GetQRCodeByZXingNet(PanelForm.GetShowStringValue(), PanelForm.GetWidthValue(), PanelForm.GetHeightValue());
            AddPictureBox(PicImage, new TagInfo("qrcode", PanelForm.GetShowStringValue()));
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
            catch (Exception)
            {
                //异常输出
            }
            return result;
        }
    }
}
