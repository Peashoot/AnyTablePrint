using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    internal class PrintPreviewImagePictureBox : PrintPreviewPictureBox
    {
        public PrintPreviewImagePictureBox(Panel panel, ExportInfo exinfo)
            : base(panel, exinfo)
        {
        }

        /// <summary>
        /// 图片填充PictureBox
        /// </summary>
        /// <param name="exinfo"></param>
        public override void GeneratePictureBoxFillImage(ExportInfo exinfo)
        {
            OpenFileDialog openFileDialog = OpenFileDialogSelectImage();
            AddPictureBox(PicImage, exinfo);
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
                }
                return openFileDialog;
            }
            catch (Exception)
            {
                openFileDialog.Dispose();
                return null;
            }
        }
    }
}