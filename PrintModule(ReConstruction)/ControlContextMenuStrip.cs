using System;
using System.Windows.Forms;

namespace PrintModule_ReConstruction_
{
    public class ControlContextMenuStrip : ContextMenuStrip
    {
        private ToolStripItem item_Reset;

        private ToolStripItem item_Delete;

        public ControlContextMenuStrip()
            : base()
        {
            InitContextMenuStrip();
            Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.item_Reset,
            this.item_Delete});
            Size = new System.Drawing.Size(125, 48);
        }

        private void InitContextMenuStrip()
        {
            InitToolStripItem(ref item_Reset, "重新设置", item_Reset_Click);
            InitToolStripItem(ref item_Delete, "删除", item_Delete_Click);
        }

        private void InitToolStripItem(ref ToolStripItem item, string text, EventHandler target)
        {
            if (item != null)
            {
                return;
            }
            item = new ToolStripMenuItem();
            item.Size = new System.Drawing.Size(124, 22);
            item.Text = text;
            item.Click += new System.EventHandler(target);
        }

        private void item_Reset_Click(object sender, EventArgs e)
        {
            IPrintPreviewControl source = SourceControl as IPrintPreviewControl;
            if (source != null)
            {
                //source.resetMode = true;
            }
        }

        private void item_Delete_Click(object sender, EventArgs e)
        {
            SourceControl.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ToolStripItemResize(item_Reset);
            ToolStripItemResize(item_Delete);
        }

        private void ToolStripItemResize(ToolStripItem item)
        {
            item.Size = new System.Drawing.Size(Width - 1, Height / 2 - 2);
        }
    }
}