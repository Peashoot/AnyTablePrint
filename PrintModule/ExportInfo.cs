using System;
using System.Drawing;
using System.Xml;

namespace PrintModule
{
    public class ExportInfo
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
        /// 导出信息到XML
        /// </summary>
        /// <param name="xmldoc"></param>
        public void AddIntoXMLDocument(ref XmlDocument xmldoc)
        {
            XmlElement root, parent, child, grandchild;
            if (xmldoc != null)
            {
                root = xmldoc.DocumentElement;
                parent = xmldoc.CreateElement("Control");
                /***************************taginfo*******************************/
                child = xmldoc.CreateElement("Taginfo");

                grandchild = xmldoc.CreateElement("Type");
                grandchild.InnerText = this.taginfo.Type;
                child.AppendChild(grandchild);

                grandchild = xmldoc.CreateElement("Info");
                grandchild.InnerText = this.taginfo.Info;
                child.AppendChild(grandchild);

                parent.AppendChild(child);
                /***************************location*******************************/
                child = xmldoc.CreateElement("Location");

                grandchild = xmldoc.CreateElement("X");
                grandchild.InnerText = this.location.X.ToString();
                child.AppendChild(grandchild);

                grandchild = xmldoc.CreateElement("Y");
                grandchild.InnerText = this.location.Y.ToString();
                child.AppendChild(grandchild);

                parent.AppendChild(child);
                /******************************size*******************************/
                child = xmldoc.CreateElement("Size");

                grandchild = xmldoc.CreateElement("Width");
                grandchild.InnerText = this.size.Width.ToString();
                child.AppendChild(grandchild);

                grandchild = xmldoc.CreateElement("Height");
                grandchild.InnerText = this.size.Height.ToString();
                child.AppendChild(grandchild);

                parent.AppendChild(child);
                /**************************foreFont*******************************/
                child = xmldoc.CreateElement("ForeFont");

                grandchild = xmldoc.CreateElement("Name");
                grandchild.InnerText = this.foreFont.Name;
                child.AppendChild(grandchild);

                grandchild = xmldoc.CreateElement("Style");
                grandchild.InnerText = this.foreFont.Style.ToString();
                child.AppendChild(grandchild);

                grandchild = xmldoc.CreateElement("Size");
                grandchild.InnerText = this.foreFont.Size.ToString();
                child.AppendChild(grandchild);

                parent.AppendChild(child);
                /****************************foreColor*****************************/
                child = xmldoc.CreateElement("ForeColor");

                grandchild = xmldoc.CreateElement("Name");
                grandchild.InnerText = this.foreColor.Name;
                child.AppendChild(grandchild);

                parent.AppendChild(child);
                /****************************backColor*****************************/
                child = xmldoc.CreateElement("BackColor");

                grandchild = xmldoc.CreateElement("Name");
                grandchild.InnerText = this.backColor.Name;
                child.AppendChild(grandchild);

                parent.AppendChild(child);

                root.AppendChild(parent);
            }
        }

        /// <summary>
        /// 从XML获取信息
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ExportInfo GetInfoFromXML(XmlElement parent)
        {
            ExportInfo retinfo = new ExportInfo();
            XmlElement child, grandchild;
            /**********************************taginfo*******************************/
            child = (XmlElement)parent.GetElementsByTagName("Taginfo").Item(0);

            grandchild = (XmlElement)child.GetElementsByTagName("Type").Item(0);
            retinfo.taginfo.Type = grandchild.InnerText;
            grandchild = (XmlElement)child.GetElementsByTagName("Info").Item(0);
            retinfo.taginfo.Info = grandchild.InnerText;

            /**********************************location*******************************/
            child = (XmlElement)parent.GetElementsByTagName("Location").Item(0);

            grandchild = (XmlElement)child.GetElementsByTagName("X").Item(0);
            retinfo.location.X = grandchild.InnerText.ToInt32();
            grandchild = (XmlElement)child.GetElementsByTagName("Y").Item(0);
            retinfo.location.Y = grandchild.InnerText.ToInt32();

            /**********************************size*******************************/
            child = (XmlElement)parent.GetElementsByTagName("Size").Item(0);

            grandchild = (XmlElement)child.GetElementsByTagName("Width").Item(0);
            retinfo.size.Width = grandchild.InnerText.ToInt32();
            grandchild = (XmlElement)child.GetElementsByTagName("Height").Item(0);
            retinfo.size.Height = grandchild.InnerText.ToInt32();

            /**********************************foreFont*******************************/
            child = (XmlElement)parent.GetElementsByTagName("ForeFont").Item(0);

            grandchild = (XmlElement)child.GetElementsByTagName("Name").Item(0);
            string FontName = grandchild.InnerText;
            grandchild = (XmlElement)child.GetElementsByTagName("Style").Item(0);
            FontStyle FontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), grandchild.InnerText);
            grandchild = (XmlElement)child.GetElementsByTagName("Size").Item(0);
            float FontSize = (float)grandchild.InnerText.ToDouble();
            retinfo.foreFont = new Font(FontName, FontSize, FontStyle);

            /*******************************foreColor*******************************/
            child = (XmlElement)parent.GetElementsByTagName("ForeColor").Item(0);

            grandchild = (XmlElement)child.GetElementsByTagName("Name").Item(0);
            retinfo.foreColor = Color.FromName(grandchild.InnerText);

            /*******************************foreColor*******************************/
            child = (XmlElement)parent.GetElementsByTagName("BackColor").Item(0);

            grandchild = (XmlElement)child.GetElementsByTagName("Name").Item(0);
            retinfo.backColor = Color.FromName(grandchild.InnerText);

            return retinfo;
        }
    }
}