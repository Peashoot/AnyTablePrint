using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PrintModule
{
    static class Extend
    {
        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(this string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 字符串数组转int数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] ToInt32(this string[] str)
        {
            try
            {
                int[] ret = new int[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    ret[i] = Convert.ToInt32(str[i]);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Double ToDouble(this string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// TrimEnd字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string TrimEnd(this string str, string end)
        {
            try
            {
                while (str.EndsWith(end))
                {
                    str = str.Substring(0, str.LastIndexOf(end));
                }
                return str;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return str == string.Empty;
        }
        /// <summary>
        /// 字符串重复叠加
        /// </summary>
        /// <param name="strbud"></param>
        /// <param name="appstr"></param>
        /// <param name="repeat"></param>
        /// <returns></returns>
        public static StringBuilder Append(StringBuilder strbud, string appstr, int repeat)
        {
            try
            {
                for (int i = 0; i < repeat; i++)
                {
                    strbud.Append(appstr);
                }
                return strbud;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 字符串转日期
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, string format)
        {
            try
            {
                DateTime dt;
                DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                dtFormat.ShortDatePattern = format;
                dt = Convert.ToDateTime(str, dtFormat);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
