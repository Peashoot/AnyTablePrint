using System;
using System.Drawing;
using System.Xml;

namespace PrintModule_ReConstruction_
{
    public class ExportInfo : IDisposable
    {
        public ExportInfo()
        {
            Location = new Point();
            TagInfo = new TagInfo("", "");
            Size = new Size();
            ForeFont = new Font("宋体", 11f);
            ForeColor = Color.Black;
            BackColor = Color.Transparent;
        }

        #region 参数

        /// <summary>
        /// 位置信息
        /// </summary>
        public Point Location;

        /// <summary>
        /// 标签信息
        /// </summary>
        public TagInfo TagInfo;

        /// <summary>
        /// 控件大小
        /// </summary>
        public Size Size;

        /// <summary>
        /// 字体信息
        /// </summary>
        public Font ForeFont;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color ForeColor;

        /// <summary>
        /// 背景信息
        /// </summary>
        public Color BackColor;

        /// <summary>
        /// 是否已经Dispose
        /// </summary>
        private bool disposed = false;

        #endregion 参数

        /// <summary>
        /// 导出信息到XML
        /// </summary>
        public void AddIntoXMLDocument(ref XmlDocument xmldoc)
        {
            XmlElement root, parent;
            root = xmldoc.DocumentElement;
            parent = xmldoc.CreateElement("Control");
            AddNodeToXMLNode(xmldoc, parent, "Taginfo", new string[] { "Type," + TagInfo.Type, "Info," + TagInfo.Info });
            AddNodeToXMLNode(xmldoc, parent, "Location", new string[] { "X," + Location.X, "Y," + Location.Y });
            AddNodeToXMLNode(xmldoc, parent, "Size", new string[] { "Width," + Size.Width, "Height," + Size.Height });
            AddNodeToXMLNode(xmldoc, parent, "ForeFont", new string[] { "Name," + ForeFont.Name, "Style," + ForeFont.Style, "Size," + ForeFont.Size });
            AddNodeToXMLNode(xmldoc, parent, "ForeColor", new string[] { "Name," + ForeColor.Name });
            AddNodeToXMLNode(xmldoc, parent, "BackColor", new string[] { "Name," + BackColor.Name });
            root.AppendChild(parent);
        }

        /// <summary>
        /// 增加XML节点
        /// </summary>
        public void AddNodeToXMLNode(XmlDocument xmldoc, XmlElement root, string addtext, string[] info)
        {
            XmlElement parent = xmldoc.CreateElement(addtext);
            for (int i = 0; i < info.Length; i++)
            {
                string[] infoarr = info[i].Split(',');
                XmlElement child = xmldoc.CreateElement(infoarr[0]);
                child.InnerText = infoarr[1];
                parent.AppendChild(child);
            }
            root.AppendChild(parent);
        }

        /// <summary>
        /// 从XML获取信息
        /// </summary>
        public ExportInfo GetInfoFromXML(XmlElement parent)
        {
            ExportInfo retinfo = new ExportInfo();
            try
            {
                retinfo.TagInfo.Type = GetNodeValue(parent, "Taginfo", "Type");
                retinfo.TagInfo.Info = GetNodeValue(parent, "Taginfo", "Info");
                retinfo.Location.X = Convert.ToInt32(GetNodeValue(parent, "Location", "X"));
                retinfo.Location.Y = Convert.ToInt32(GetNodeValue(parent, "Location", "Y"));
                retinfo.Size.Width = Convert.ToInt32(GetNodeValue(parent, "Size", "Width"));
                retinfo.Size.Height = Convert.ToInt32(GetNodeValue(parent, "Size", "Height"));
                string FontName = GetNodeValue(parent, "ForeFont", "Name");
                FontStyle FontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), GetNodeValue(parent, "ForeFont", "Style"));
                float FontSize = (float)Convert.ToDouble(GetNodeValue(parent, "ForeFont", "Size"));
                retinfo.ForeFont = new Font(FontName, FontSize, FontStyle);
                retinfo.ForeColor = Color.FromName(GetNodeValue(parent, "ForeColor", "Name"));
                retinfo.BackColor = Color.FromName(GetNodeValue(parent, "BackColor", "Name"));
                return retinfo;
            }
            catch (Exception)
            {
                retinfo.Dispose();
                return null;
            }
        }

        /// <summary>
        /// 获取节点信息
        /// </summary>
        public string GetNodeValue(XmlElement parent, string nodename, string tagname)
        {
            XmlElement child = (XmlElement)parent.GetElementsByTagName(nodename).Item(0);
            return ((XmlElement)child.GetElementsByTagName(tagname).Item(0)).InnerText;
        }

        /// <summary>
        /// 重写Dispose方法
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 重写Dispose方法
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                ForeFont.Dispose();
            }
            disposed = true;
        }
    }
}