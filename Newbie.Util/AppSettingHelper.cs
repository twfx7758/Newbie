using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Newbie.Util
{
    public class AppSettingHelper
    {
        public static string GetString(string key)
        {
            string item = ConfigurationManager.AppSettings[key];
            return item;
        }

        public static string GetString(string key, string defaultValue)
        {
            string result = GetString(key);
            if (result == null)
            {
                result = defaultValue;
            }
            return result;
        }

        public static DateTime GetDateTime(string key, DateTime? defaultValue = null)
        {
            if (defaultValue == null)
            {
                defaultValue = new DateTime(1900, 1, 1);
            }
            string item = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(item))
            {
                return defaultValue.Value;
            }

            DateTime result = defaultValue.Value;
            DateTime.TryParse(item, out result);

            return result;
        }

        /// <summary>
        /// 获取bool，“True”、“1”表示True，否则返回False
        /// </summary>
        public static bool GetBool(string key, bool defaultValue = false)
        {
            string str = GetString(key);
            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue;
            }
            str = str.Trim().ToUpper();
            if (str == "TRUE" || str == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetInt32(string key, int defaultValue = 0)
        {
            string str = GetString(key);
            int result = defaultValue;
            int.TryParse(str, out result);
            return result;
        }
        public static long GetInt64(string key, long defaultValue = 0)
        {
            string str = GetString(key);
            long result = defaultValue;
            long.TryParse(str, out result);
            return result;
        }
        public static double GetDouble(string key, double defaultValue = 0)
        {
            string str = GetString(key);
            double result = defaultValue;
            double.TryParse(str, out result);
            return result;
        }
        public static decimal GetDecimal(string key, decimal defaultValue = 0)
        {
            string str = GetString(key);
            decimal result = defaultValue;
            decimal.TryParse(str, out result);
            return result;
        }
    }
}
