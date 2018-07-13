using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace PrintModule_ReConstruction_
{
    public class ExportInfo : IDisposable
    {
        public ExportInfo()
        {
            location = new Point();
            taginfo = new TagInfo("", "");
            size = new Size();
            foreFont = new Font("宋体", 11f);
            foreColor = Color.Black;
            backColor = Color.Transparent;
        }
        #region 参数
        /// <summary>
        /// 位置信息
        /// </summary>
        public Point location;
        /// <summary>
        /// 标签信息
        /// </summary>
        public TagInfo taginfo;
        /// <summary>
        /// 控件大小
        /// </summary>
        public Size size;
        /// <summary>
        /// 字体信息
        /// </summary>
        public Font foreFont;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color foreColor;
        /// <summary>
        /// 背景信息
        /// </summary>
        public Color backColor;
        /// <summary>
        /// 是否已经Dispose
        /// </summary>
        private bool disposed = false;
        #endregion
        /// <summary>
        /// 导出信息到XML
        /// </summary>
        /// <param name="xmldoc"></param>
        public void AddIntoXMLDocument(ref XmlDocument xmldoc)
        {
            XmlElement root, parent;
            root = xmldoc.DocumentElement;
            parent = xmldoc.CreateElement("Control");
            AddNodeToXMLNode(xmldoc, parent, "Taginfo", new string[] { "Type," + taginfo.Type, "Info," + taginfo.Info });
            AddNodeToXMLNode(xmldoc, parent, "Location", new string[] { "X," + location.X, "Y," + location.Y });
            AddNodeToXMLNode(xmldoc, parent, "Size", new string[] { "Width," + size.Width, "Height," + size.Height });
            AddNodeToXMLNode(xmldoc, parent, "ForeFont", new string[] { "Name," + foreFont.Name, "Style," + foreFont.Style, "Size," + foreFont.Size });
            AddNodeToXMLNode(xmldoc, parent, "ForeColor", new string[] { "Name," + foreColor.Name });
            AddNodeToXMLNode(xmldoc, parent, "BackColor", new string[] { "Name," + backColor.Name });
            root.AppendChild(parent);
        }

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
        /// <param name="parent"></param>
        /// <returns></returns>
        public ExportInfo GetInfoFromXML(XmlElement parent)
        {
            ExportInfo retinfo = new ExportInfo();
            try
            {
                retinfo.taginfo.Type = GetNodeValue(parent, "Taginfo", "Type");
                retinfo.taginfo.Info = GetNodeValue(parent, "Taginfo", "Info");
                retinfo.location.X = Convert.ToInt32(GetNodeValue(parent, "Location", "X"));
                retinfo.location.Y = Convert.ToInt32(GetNodeValue(parent, "Location", "Y"));
                retinfo.size.Width = Convert.ToInt32(GetNodeValue(parent, "Size", "Width"));
                retinfo.size.Height = Convert.ToInt32(GetNodeValue(parent, "Size", "Height"));
                string FontName = GetNodeValue(parent, "ForeFont", "Name");
                FontStyle FontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), GetNodeValue(parent, "ForeFont", "Style"));
                float FontSize = (float)Convert.ToDouble(GetNodeValue(parent, "ForeFont", "Size"));
                retinfo.foreFont = new Font(FontName, FontSize, FontStyle);
                retinfo.foreColor = Color.FromName(GetNodeValue(parent, "ForeColor", "Name"));
                retinfo.backColor = Color.FromName(GetNodeValue(parent, "BackColor", "Name"));
                return retinfo;
            }
            catch (Exception)
            {
                retinfo.Dispose();
                return null;
            }
        }

        public string GetNodeValue(XmlElement parent, string nodename, string tagname)
        {
            XmlElement child = (XmlElement)parent.GetElementsByTagName(nodename).Item(0);
            return ((XmlElement)child.GetElementsByTagName(tagname).Item(0)).InnerText;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                foreFont.Dispose();
            }
            disposed = true;
        }
    }
}
