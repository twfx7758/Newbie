using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Newbie.Util
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        private static string Get(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        #region 得到AppSettings中的配置字符串信息
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            return GetConfigString(key, "");
        }
        public static string GetConfigString(string key, string DefaultValue)
        {
            string val = Get(key);
            return !string.IsNullOrEmpty(val) ? val : DefaultValue;
        }
        #endregion

        #region 得到AppSettings中的配置bool信息
        /// <summary>
        /// 得到AppSettings中的配置bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key, bool defaultValue = false)
        {
            bool result = false;
            string val = Get(key);
            if (!string.IsNullOrEmpty(val)
                && (val.Trim().ToUpper() == "TRUE" || val.Trim() == "1"))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region 得到AppSettings中的配置decimal信息
        /// <summary>
        /// 得到AppSettings中的配置decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key, decimal defaultValue = 0)
        {
            decimal result = defaultValue;
            string val = Get(key);
            if (!string.IsNullOrEmpty(val))
            {
                val = val.Trim();
                try
                {
                    result = decimal.Parse(val);
                }
                catch (FormatException ex) //Ignore format exceptions.
                { }
            }
            return result;
        }
        #endregion

        #region 得到AppSettings中的配置int信息
        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key, int defaultValue = 0)
        {
            int result = defaultValue;
            string val = Get(key);
            if (!string.IsNullOrEmpty(val))
            {
                val = val.Trim();
                try
                {
                    result = int.Parse(val);
                }
                catch (FormatException ex) //Ignore format exceptions.
                { }
            }
            return result;

        }
        #endregion

        #region ConnectionString
        private static string GetConnectionString(string connName)
        {
            //string providerName = System.Configuration.ConfigurationManager.ConnectionStrings[connName].ProviderName;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connName].ConnectionString;
            return connectionString;
        }

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static string GetConnString(string connName)
        {
            return GetConnectionString(connName);
        }
        #endregion

    }
}
