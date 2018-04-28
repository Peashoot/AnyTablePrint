using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintModule
{
    public class TagInfo
    {
        public TagInfo(string Type, string Info)
        {
            this.type = Type;
            this.info = Info;
        }
        /// <summary>
        /// PictureBox的种类
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Image包含的信息
        /// </summary>
        public string info { get; set; }
    }
}
