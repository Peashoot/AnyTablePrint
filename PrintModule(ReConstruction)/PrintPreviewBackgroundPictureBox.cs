using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    internal class PrintPreviewBackgroundPictureBox : PrintPreviewPictureBox
    {
        public PrintPreviewBackgroundPictureBox(Panel panel, ExportInfo exinfo)
            : base(panel, exinfo)
        {
        }

        /// <summary>
        /// 设置PictureBox背景
        /// </summary>
        /// <param name="exinfo"></param>
        public override void GeneratePictureBoxFillImage(ExportInfo exinfo)
        {
            AddPictureBox(null, exinfo);
        }
    }
}