using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Newbie.Util
{
    /// <summary>
    /// 读取配置的工具类
    /// </summary>
    public class ConfigurationUtil
    {
        /// <summary>
        /// 取得AppSettings中配置的值，当不存在配置的值时会抛出异常
        /// </summary>
        /// <param name="configKey">配置的Key</param>
        /// <returns>configKey对应的配置值</returns>
        public static string GetAppSettingValue(string configKey)
        {
            return GetAppSettingValue(configKey, true);
        }

        /// <summary>
        /// 取得AppSettings中配置的值
        /// </summary>
        /// <param name="configKey">配置的Key</param>
        /// <param name="isThrowExceptionIfNotExist">当配置的值不存在时是否抛出异常</param>
        /// <returns>configKey对应的配置值</returns>
        public static string GetAppSettingValue(string configKey, bool isThrowExceptionIfNotExist)
        {
            string rst = ConfigurationManager.AppSettings[configKey];
            if ((rst == null || rst.Trim().Length == 0)
                && isThrowExceptionIfNotExist)
            {
                throw new ApplicationException("请在配置文件的appSettings中配置'" + configKey + "'的值。");
            }
            return rst;
        }

        /// <summary>
        /// 取得ConnectionStrings中配置的值，当不存在配置的值时会抛出异常
        /// </summary>
        /// <param name="configName">配置的连接字符串的名字</param>
        /// <param name="isThrowExceptionIfNotExist">当配置的值不存在时是否抛出异常</param>
        /// <returns>configName对应的配置值</returns>
        public static string GetConnectionString(string configName)
        {
            return GetConnectionString(configName, true);
        }

        /// <summary>
        /// 取得ConnectionStrings中配置的值
        /// </summary>
        /// <param name="configName">配置的连接字符串的名字</param>
        /// <param name="isThrowExceptionIfNotExist">当配置的值不存在时是否抛出异常</param>
        /// <returns>configName对应的配置值</returns>
        public static string GetConnectionString(string configName, bool isThrowExceptionIfNotExist)
        {
            string rst = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
            if ((rst == null || rst.Trim().Length == 0)
                && isThrowExceptionIfNotExist)
            {
                throw new ApplicationException("请在配置文件的ConnectionStrings中配置'" + configName + "'的值。");
            }
            return rst;
        }

    }
}
