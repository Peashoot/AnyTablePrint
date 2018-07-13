using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PrintModule_ReConstruction_
{
    public class PrintPreviewImagePictureBox : PrintPreviewPictureBox
    {
        public PrintPreviewImagePictureBox(Panel panel, ExportInfo exinfo = null) : base(panel, exinfo) { }
        /// <summary>
        /// 图片填充PictureBox
        /// </summary>
        /// <param name="exinfo"></param>
        public override void GeneratePictureBoxFillImage(ExportInfo exinfo = null)
        {
            OpenFileDialog openFileDialog = null;
            if (exinfo != null)
            {
                PicImage = Image.FromFile(exinfo.taginfo.Info);
                PicImage = ZoomPicture(PicImage, PanelForm.GetWidthValue(), PanelForm.GetHeightValue());
            }
            else
            {
                openFileDialog = OpenFileDialogSelectImage();
            }
            AddPictureBox(PicImage, new TagInfo("image", openFileDialog.FileName));
        }
        /// <summary>
        /// 选择要添加的图片
        /// </summary>
        /// <returns></returns>
        private OpenFileDialog OpenFileDialogSelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
                openFileDialog.Title = "选择要使用的图片";
                openFileDialog.Filter = "图片文件|*.jpg;*.bmp;*.png;*.jpeg;*.gif";
                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    PicImage = Image.FromStream(openFileDialog.OpenFile());
                    PicImage = ZoomPicture(PicImage, PanelForm.GetWidthValue(), PanelForm.GetHeightValue());
                }
                return openFileDialog;
            }
            catch (Exception)
            {
                openFileDialog.Dispose();
                return null;
            }
        }
        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="SourceImage"></param>
        /// <param name="TargetWidth"></param>
        /// <param name="TargetHeight"></param>
        /// <returns></returns>
        public Image ZoomPicture(Image SourceImage, int TargetWidth, int TargetHeight)
        {
            int IntWidth; //新的图片宽
            int IntHeight; //新的图片高
            System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;
            System.Drawing.Bitmap SaveImage = null;
            System.Drawing.Bitmap TempImage = null;
            try
            {
                TempImage = new System.Drawing.Bitmap(TargetWidth, TargetHeight);
                Graphics g = Graphics.FromImage(TempImage);
                g.Clear(Color.White);

                if (SourceImage.Width > TargetWidth && SourceImage.Height <= TargetHeight)//宽度比目的图片宽度大，长度比目的图片长度小
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height > TargetHeight)//宽度比目的图片宽度小，长度比目的图片长度大
                {
                    IntHeight = TargetHeight;
                    IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height <= TargetHeight) //长宽比目的图片长宽都小
                {
                    IntHeight = SourceImage.Width;
                    IntWidth = SourceImage.Height;
                }
                else//长宽比目的图片的长宽都大
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                    if (IntHeight > TargetHeight)//重新计算
                    {
                        IntHeight = TargetHeight;
                        IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                    }
                }

                g.DrawImage(SourceImage, (TargetWidth - IntWidth) / 2, (TargetHeight - IntHeight) / 2, IntWidth, IntHeight);
                SourceImage.Dispose();
                SaveImage = TempImage;
                TempImage = null;
            }
            finally
            {
                if (TempImage != null)
                {
                    TempImage.Dispose();
                }
            }
            return SaveImage;
        }
    }
}
