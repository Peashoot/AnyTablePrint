using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

namespace PrintModule
{
    public class ConfigInfo
    {
        #region 配置参数
        /// <summary>
        /// 打印纸张宽度
        /// </summary>
        public static int PageSizeWeight;
        /// <summary>
        /// 打印纸张宽度
        /// </summary>
        public static int PageSizeHeight;
        #endregion
        #region 获取配置
        /// <summary>
        /// 获取配置
        /// </summary>
        public static void Get_ConfigInfo()
        {
            try
            {
                PageSizeHeight = Convert.ToInt32(Get_ConfigValue("PageSizeHeight"));
            }
            catch
            {
                PageSizeHeight = 0;
            }
            try
            {
                PageSizeWeight = Convert.ToInt32(Get_ConfigValue("PageSizeWeight"));
            }
            catch
            {
                PageSizeWeight = 0;
            }
        }
        #endregion
        #region 获取config值
        /// <summary>
        /// 获取config值
        /// </summary>
        /// <param name="AppKey"></param>
        /// <returns></returns>
        public static List<string> Get_ConfigValue(string strKey, bool isFuzzy = false)
        {
            List<string> retlist = new List<string>();
            string file = Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (isFuzzy)
                {
                    if (key.Contains(strKey))
                    {
                        retlist.Add(config.AppSettings.Settings[key].Value.ToString());
                    }
                }
                else
                {
                    if (key == strKey)
                    {
                        retlist.Add(config.AppSettings.Settings[key].Value.ToString());
                    }
                }
            }
            return retlist;
        }
        #endregion
        #region 设置config值
        /// <summary>
        /// 设置config值
        ///</summary>  
        ///<param name="newKey"></param>  
        ///<param name="newValue"></param>  
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            string file = Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            bool exist = false;
            foreach (string key in config.AppSettings.Settings.AllKeys)
                if (key == newKey)
                    exist = true;
            if (exist)
                config.AppSettings.Settings.Remove(newKey);
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion
        #region 删除config值
        /// <summary>
        /// 删除config值
        /// </summary>
        /// <param name="deleteKey"></param>
        /// <param name="isFuzzy"></param>
        public static void DeleteAppConfig(string deleteKey, bool isFuzzy = false)
        {
            List<string> dellist = new List<string>();
            string file = Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (isFuzzy)
                {
                    if (key.Contains(deleteKey))
                    {
                        dellist.Add(config.AppSettings.Settings[key].Value.ToString());
                    }
                }
                else
                {
                    if (key == deleteKey)
                    {
                        dellist.Add(config.AppSettings.Settings[key].Value.ToString());
                    }
                }
            }
            foreach (string key in dellist)
            {
                config.AppSettings.Settings.Remove(key);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion
    }
}
