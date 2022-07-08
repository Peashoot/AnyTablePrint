using System.Drawing;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    public interface IPrintPreviewControl
    {
        #region 参数

        /// <summary>
        /// 拖动前鼠标的位置
        /// </summary>
        Point BeforeLoc { get; set; }

        /// <summary>
        /// 拖动后鼠标的位置
        /// </summary>
        Point AfterLoc { get; set; }

        /// <summary>
        /// 重新设置模式
        /// </summary>
        bool ResetMode { get; set; }

        /// <summary>
        /// 控件拖动
        /// </summary>
        bool MoveLock { get; set; }

        /// <summary>
        /// 控件缩放
        /// </summary>
        bool ExpandLock { get; set; }

        /// <summary>
        /// 缩放数组（表示缩放的类型）
        /// </summary>
        int[] ExpandArray { get; set; }

        /// <summary>
        /// 控件的最小宽度
        /// </summary>
        int MinWidth { get; set; }

        /// <summary>
        /// 控件的最小高度
        /// </summary>
        int MinHeight { get; set; }

        /// <summary>
        /// 所属的Panel
        /// </summary>
        Panel BelongPanel { get; set; }

        /// <summary>
        /// 右键菜单
        /// </summary>
        ContextMenuStrip MenuStrip { get; set; }

        /// <summary>
        /// 导入信息
        /// </summary>
        ExportInfo Exinfo { get; set; }

        #endregion 参数

        /// <summary>
        /// 给控件添加事件
        /// </summary>
        /// <param name="control"></param>
        void AddControlEvent();
    }
}