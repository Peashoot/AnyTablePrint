using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using RestSharp;

namespace PrintModule
{
    public class IssueLayoutHelper
    {
        public static List<ControlLayoutInfo> GetLayoutFromPanel(Panel panel)
        {
            List<ControlLayoutInfo> layoutList = new List<ControlLayoutInfo>();
            foreach (Control control in panel.Controls)
            {
                ControlLayoutInfo layoutInfo = new ControlLayoutInfo();
                layoutInfo.id = control.Name;
                layoutInfo.height = control.Height;
                layoutInfo.width = control.Width;
                layoutInfo.x = control.Location.X;
                layoutInfo.y = control.Location.Y;
                layoutInfo.layer = 2;
                TagInfo tagInfo = control.Tag as TagInfo;
                switch (tagInfo.Type)
                {
                    case "text":
                        layoutInfo.widget = 1;
                        layoutInfo.type = 5;
                        break;
                    case "background":
                        layoutInfo.widget = 0;
                        layoutInfo.layer = 1;
                        break;
                    case "image":
                        layoutInfo.widget = 2;
                        break;
                }
                layoutList.Add(layoutInfo);
            }
            return layoutList;
        }

        public static List<ControlPropertyInfo> GetPropertyFromPanel(Panel panel)
        {
            List<ControlPropertyInfo> propertyList = new List<ControlPropertyInfo>();
            foreach (Control control in panel.Controls)
            {
                ControlPropertyInfo propertyInfo = new ControlPropertyInfo();
                propertyInfo.id = control.Name;
                propertyInfo.style = 0;
                propertyInfo.color = ConvertColorToRGB(control.ForeColor);
                propertyInfo.bg_color = ConvertColorToRGB(control.BackColor);
                propertyInfo.lines = 1;
                propertyInfo.text = control.Text;
                TagInfo tagInfo = control.Tag as TagInfo;
                if (tagInfo.Type == "image")
                {
                    propertyInfo.files = new string[1];
                    propertyInfo.files[0] = tagInfo.Info;
                }
                propertyList.Add(propertyInfo);
            }
            return propertyList;
        }

        public static string SendLayout(List<ControlLayoutInfo> layoutlist)
        {
            try
            {
                RestClient client = new RestClient("http://192.168.20.117:6900/setConfig");
                client.Timeout = 5000;
                RestRequest request = new RestRequest(Method.POST);
                request.AddJsonBody(layoutlist);
                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string SendProperty(ControlPropertyInfo propertyInfo)
        {
            try
            {
                RestClient client = new RestClient("http://192.168.20.3:8080/article/setWidget");
                client.Timeout = 5000;
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "*/*");
                long fileSize = 0;
                if (propertyInfo != null && propertyInfo.files != null && propertyInfo.files.Length > 0)
                {
                    for (int i = 0; i < propertyInfo.files.Length; i++)
                    {
                        FileInfo fileinfo = new FileInfo(propertyInfo.files[i]);
                        fileSize += fileinfo.Length;
                        request.AddFile("files", propertyInfo.files[i]);
                    }
                }
                propertyInfo.filesize = fileSize;
                JObject jobj = JObject.FromObject(propertyInfo);
                SortedDictionary<string, object> target = KeySort(jobj);
                target.Remove("sign");
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(target);
                string sign = DES3.Encrypt(json, DES3.Build3DesKey("ReformerDisplay2021" + propertyInfo.id));
                propertyInfo.sign = sign;
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(propertyInfo);
                request.AddParameter("data", data); request.AlwaysMultipartFormData = true;
                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static string ConvertColorToRGB(Color color)
        {
            return "#" + Convert.ToString(color.ToArgb(), 16).PadLeft(8, '0').Substring(2);
        }

        private static SortedDictionary<string, object> KeySort(JObject obj)
        {
            var res = new SortedDictionary<string, object>();
            foreach (var x in obj)
            {
                if (x.Value is JValue) res.Add(x.Key, x.Value);
                else if (x.Value is JObject) res.Add(x.Key, KeySort((JObject)x.Value));
                else if (x.Value is JArray)
                {
                    var tmp = new SortedDictionary<string, object>[x.Value.Count()];
                    for (var i = 0; i < x.Value.Count(); i++)
                    {
                        tmp[i] = KeySort((JObject)x.Value[i]);
                    }
                    res.Add(x.Key, tmp);
                }
            }
            return res;
        }
    }

    public class DES3
    {
        /// <summary>
        /// 加密转换类型
        /// </summary>
        private const int EncryptType = 0;

        /// <summary>
        /// 解密转换类型
        /// </summary>
        private const int DecryptType = 1;

        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="type">转换类型</param>
        /// <param name="bText">要转换的字节数组</param>
        /// <param name="bKey">加解密的字节密钥</param>
        /// <param name="mode">加解密模式</param>
        /// <param name="iv">加解密填充向量</param>
        /// <returns>转换后的字节数组</returns>
        private static byte[] Transform(int type, byte[] bText, byte[] bKey, CipherMode mode = CipherMode.ECB, string iv = "12345678")
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = bKey,
                    Mode = mode,
                    Padding = PaddingMode.PKCS7
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = Encoding.UTF8.GetBytes(iv);
                }
                var desTransform = type == EncryptType ? des.CreateEncryptor() : des.CreateDecryptor();
                int nLen_bText = bText.Length;
                byte[] result = desTransform.TransformFinalBlock(bText, 0, nLen_bText);
                return result;
            }
            catch (Exception ex)
            {
                return Encoding.UTF8.GetBytes(string.Empty);
            }
        }

        /// <summary>
        /// DES3加密
        /// </summary>
        /// <param name="bPlaintext">待加密字节数组</param>
        /// <param name="bKey">加密密钥字节数组</param>
        /// <param name="mode">加密模式</param>
        /// <param name="iv">填充向量</param>
        /// <returns>加密后字节数组</returns>
        public static byte[] Encrypt(byte[] bPlaintext, byte[] bKey, CipherMode mode = CipherMode.ECB, string iv = "12345678")
        {
            return Transform(EncryptType, bPlaintext, bKey, mode, iv);
        }

        /// <summary>
        /// DES3解密
        /// </summary>
        /// <param name="bCiphertext">待解密字节数组</param>
        /// <param name="bKey">解密密钥字节数组</param>
        /// <param name="mode">解密模式</param>
        /// <param name="iv">填充向量</param>
        /// <returns>解密后字节数组</returns>
        public static byte[] Decrypt(byte[] bCiphertext, byte[] bKey, CipherMode mode = CipherMode.ECB, string iv = "12345678")
        {
            return Transform(DecryptType, bCiphertext, bKey, mode, iv);
        }

        /// <summary>
        /// DES3加密
        /// </summary>
        /// <param name="plainText">待加密字符串</param>
        /// <param name="bKey">加密密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string plainText, byte[] bKey)
        {
            try
            {
                byte[] bPlaintext = Encoding.UTF8.GetBytes(plainText);
                //byte[] bKey = Build3DesKey(key);
                return byteToHexStr(Encrypt(bPlaintext, bKey));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// DES3解密
        /// </summary>
        /// <param name="cipherText">待解密字符串</param>
        /// <param name="bKey">解密密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string cipherText, byte[] bKey)
        {
            try
            {
                byte[] bCliphertext = HexStringToByte(cipherText);
                //byte[] bKey = Build3DesKey(key);
                return Encoding.UTF8.GetString(Decrypt(bCliphertext, bKey));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 构建3DES密钥
        /// <param name="keyStr">密钥字符串</param>
        /// <returns>加密字节数组</returns>
        public static byte[] Build3DesKey(string keyStr)
        {
            byte[] key = new byte[24]; // 声明一个24位的字节数组，默认里面都是0
            byte[] temp = Encoding.UTF8.GetBytes(keyStr); // 将字符串转成字节数组
            Array.Copy(temp, 0, key, 0, Math.Min(key.Length, temp.Length));
            return key;
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        static byte[] HexStringToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }

    public class ControlLayoutInfo
    {
        /// <summary>
        /// 是	string	部件唯一标识 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 是	int	部件类型：0：背景、1：文本；2、静态图片；3、轮播图片、4、视频 
        /// </summary>
        public int widget { get; set; }
        /// <summary>
        /// 是	int	x轴坐标 
        /// </summary>
        public int x { get; set; }
        /// <summary>
        /// 是	int	y轴坐标 
        /// </summary>
        public int y { get; set; }
        /// <summary>
        /// 是	int	长度 
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// 是	int	高度 
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// 否	int	部件用途(文本时必传)：1：显示余位；2：显示箭头方向；3：时间；4：天气；5：其他 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 是	int	部件层级 
        /// </summary>
        public int layer { get; set; }
    }

    public class ControlPropertyInfo
    {
        public ControlPropertyInfo()
        {

        }
        /// <summary>
        /// 是	string	部件唯一标识 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 否	int	部件风格：0：静态；1：滚动 
        /// </summary>
        public int style { get; set; }
        /// <summary>
        /// 否	string	文本颜色(#FFFFFF) 
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// 否	string	显示自定义文本 
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 否	string	背景颜色(#FFFFFF) 
        /// </summary>
        public string bg_color { get; set; }
        /// <summary>
        /// 否	int	文本行数 
        /// </summary>
        public int lines { get; set; }
        /// <summary>
        /// 否	int	所有文件大小之和 
        /// </summary>
        public long filesize { get; set; }
        /// <summary>
        /// 是	string	加密结果 hex 
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        [JsonIgnore]
        public string[] files { get; set; }
    }
}
