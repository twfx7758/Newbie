
using System;
using System.Collections;
using System.Collections.Generic;

namespace Newbie.Util
{
    /// <summary>
    /// Json辅助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 对象Json化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static String SerializeJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反Json化字符串
        /// </summary>
        /// <param name="s">json字符串</param>
        /// <returns></returns>
        public static T DeserializeJson<T>(String s)
        {
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
        }

        /// <summary>
        /// 反Json化字符串
        /// </summary>
        /// <param name="s">json字符串</param>
        /// <returns></returns>
        public static List<T> DeserializeJsonReturnList<T>(String s)
        {
            return (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(s);
        }
    }
}
