using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    public class PrintPreviewBackgroundPictureBox : PrintPreviewPictureBox
    {
        public PrintPreviewBackgroundPictureBox(Panel panel, ExportInfo exinfo = null) : base(panel, exinfo) { }
        /// <summary>
        /// 设置PictureBox背景
        /// </summary>
        /// <param name="exinfo"></param>
        public override void GeneratePictureBoxFillImage(ExportInfo exinfo = null)
        {
            AddPictureBox(null, new TagInfo("background", BackColor.Name));
        }

    }
}
