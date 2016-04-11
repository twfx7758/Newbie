using System;
using System.Collections;
using System.Collections.Generic;

namespace Newbie.Caching
{
    /// <summary>
    /// Json辅助类
    /// </summary>
    public class JsonHelper
    {

        private static Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

        /// <summary>
        /// 对象Json化
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string SerializeJson(object args)
        {
            string ret = "";
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                serializer.Serialize(sw, args);
                sw.Flush();
                ret = sw.GetStringBuilder().ToString();
            }
            return ret;
        }

        /// <summary>
        /// 反Json化字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object Deserializer(string content, Type t)
        {
            using (System.IO.StringReader reader = new System.IO.StringReader(content))
            {
                return serializer.Deserialize(reader, t);
            }
        }

    }
}
