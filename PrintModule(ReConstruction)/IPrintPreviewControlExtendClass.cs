using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    public static class IPrintPreviewControlExtendClass
    {
        #region GetInfoFromExportInfo

        /// <summary>
        /// 从ExportInfo中获取控件信息内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="exinfo"></param>
        public static void GetInfoFromExportInfo<T>(this T control, ExportInfo exinfo) where T : Control, IPrintPreviewControl
        {
            control.Font = exinfo.ForeFont;
            control.ForeColor = exinfo.ForeColor;
            control.BackColor = exinfo.BackColor;
            control.Location = exinfo.Location;
        }

        #endregion GetInfoFromExportInfo

        #region PrintPreviewControl_MouseUp

        /// <summary>
        /// 鼠标抬起，结束缩放或移动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintPreviewControl_MouseUp<T>(this T control, object sender, MouseEventArgs e) where T : Control, IPrintPreviewControl
        {
            control.MoveLock = false;
            control.ExpandLock = false;
        }

        #endregion PrintPreviewControl_MouseUp

        #region PrintPreviewControl_MouseDown

        /// <summary>
        /// 根据鼠标形状，判断鼠标进入移动模式还是缩放模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintPreviewControl_MouseDown<T>(this T control, object sender, MouseEventArgs e) where T : Control, IPrintPreviewControl
        {
            control.BeforeLoc = control.BelongPanel.PointToClient(Control.MousePosition);
            if (control.Cursor == Cursors.Default)
            {
                control.MoveLock = true;
            }
            else
            {
                control.ExpandLock = true;
            }
        }

        #endregion PrintPreviewControl_MouseDown

        #region PrintPreviewControl_MouseMove

        /// <summary>
        /// 鼠标移动时控件移动（或缩放）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintPreviewControl_MouseMove<T>(this T control, object sender, MouseEventArgs e) where T : Control, IPrintPreviewControl
        {
            control.AfterLoc = new Point(
                Math.Min(Math.Max(control.BelongPanel.PointToClient(Control.MousePosition).X, 0), control.BelongPanel.Width - 1),
                Math.Min(Math.Max(control.BelongPanel.PointToClient(Control.MousePosition).Y, 0), control.BelongPanel.Height - 1));
            if (control.MoveLock)
            {
                control.Left = Math.Min(Math.Max(control.Location.X + control.AfterLoc.X - control.BeforeLoc.X, 0), control.BelongPanel.Size.Width - control.Size.Width);
                control.Top = Math.Min(Math.Max(control.Location.Y + control.AfterLoc.Y - control.BeforeLoc.Y, 0), control.BelongPanel.Size.Height - control.Size.Height);
                control.BeforeLoc = control.AfterLoc;
            }
            else if (control.ExpandLock)
            {
                control.ExpandControlSize();
            }
            else if (control as PictureBox != null)
            {
                Point mousepos = control.PointToClient(Control.MousePosition);
                control.ExpandArray = new int[4];
                control.FillExpandArray(control.ExpandArray, mousepos);
                control.ChangeCursor(control.ExpandArray);
            }
        }

        /// <summary>
        /// 缩放控件大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        public static void ExpandControlSize<T>(this T control) where T : Control, IPrintPreviewControl
        {
            control.AfterLoc = new Point(
                control.ExpandArray[0] == 0 ? control.AfterLoc.X :
                Math.Min(control.AfterLoc.X, control.Left + control.Width - control.MinWidth),
                control.ExpandArray[1] == 0 ? control.AfterLoc.Y :
                Math.Min(control.AfterLoc.Y, control.Top + control.Height - control.MinHeight));
            control.Left = control.ExpandArray[0] == 0 ? control.Left : control.AfterLoc.X;
            control.Top = control.ExpandArray[1] == 0 ? control.Top : control.AfterLoc.Y;
            control.Width = control.ExpandArray[2] == 0 ? control.Size.Width : control.ExpandArray[2] == 1 ? Math.Min(Math.Max(control.Size.Width + control.AfterLoc.X - control.BeforeLoc.X, control.MinWidth), control.BelongPanel.Size.Width - control.Location.X) : Math.Min(Math.Max(control.Size.Width + control.BeforeLoc.X - control.AfterLoc.X, control.MinWidth), control.BelongPanel.Size.Width + control.Location.X);
            control.Height = control.ExpandArray[3] == 0 ? control.Size.Height : control.ExpandArray[3] == 1 ? Math.Min(Math.Max(control.Size.Height + control.AfterLoc.Y - control.BeforeLoc.Y, control.MinHeight), control.BelongPanel.Size.Height - control.Location.Y) : Math.Min(Math.Max(control.Size.Height + control.BeforeLoc.Y - control.AfterLoc.Y, control.MinHeight), control.BelongPanel.Size.Height + control.Location.Y);
            control.BeforeLoc = control.AfterLoc;
        }

        /// <summary>
        /// 填充缩放数组
        /// </summary>
        private static void FillExpandArray<T>(this T control, int[] intArray, Point mousePos) where T : Control, IPrintPreviewControl
        {
            FillExpandArray(intArray, mousePos.X, control.Width, 'x');
            FillExpandArray(intArray, mousePos.Y, control.Height, 'y');
        }

        /// <summary>
        /// 填充缩放数组
        /// </summary>
        private static void FillExpandArray(int[] intArray, int posValue, int ctlSize, char flag)
        {
            int index = flag == 'x' ? 0 : 1;
            if (NearLine(posValue, 0))
            {
                intArray[index] = 1;
                intArray[index + 2] = -1;
            }
            else if (NearLine(posValue + 1, ctlSize))
            {
                intArray[index] = 0;
                intArray[index + 2] = 1;
            }
            else
            {
                intArray[index] = 0;
                intArray[index + 2] = 0;
            }
        }

        /// <summary>
        /// 改变鼠标形状
        /// </summary>
        private static void ChangeCursor<T>(this T control, int[] intArray) where T : Control, IPrintPreviewControl
        {
            if (intArray.Count(i => i == 0) == 4)
            {
                control.Cursor = Cursors.Default;
            }
            else if (intArray[1] == 0 && intArray[3] == 0)
            {
                control.Cursor = Cursors.SizeWE;
            }
            else if (intArray[0] == 0 && intArray[2] == 0)
            {
                control.Cursor = Cursors.SizeNS;
            }
            else if (intArray[0] == intArray[1] && intArray[2] == intArray[3])
            {
                control.Cursor = Cursors.SizeNWSE;
            }
            else if (intArray.Sum() == 1)
            {
                control.Cursor = Cursors.SizeNESW;
            }
        }

        /// <summary>
        /// 判断两个值的差距在不在一定范围内（是否与边界呈一定的距离）
        /// </summary>
        private static bool NearLine(int a, int b, int c = 5)
        {
            return Math.Abs(a - b) <= 5;
        }

        #endregion PrintPreviewControl_MouseMove

        #region PrintPreviewControl_MouseLeave

        /// <summary>
        /// 鼠标离开鼠标形状变为默认形状
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintPreviewControl_MouseLeave<T>(this T control, object sender, EventArgs e) where T : Control, IPrintPreviewControl
        {
            if (!control.MoveLock && !control.ExpandLock)
            {
                control.Cursor = Cursors.Default;
            }
        }

        #endregion PrintPreviewControl_MouseLeave
    }
}