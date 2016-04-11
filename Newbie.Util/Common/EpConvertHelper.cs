/* 
 * 2015-12-1 create by sunyueqiao 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Newbie.Util.Common
{
    /// <summary>
    /// 类型转换帮助类
    /// </summary>
    public static class EpConvertHelper
    {
        public static byte ToByte(string val)
        {
            byte result = 0;
            byte.TryParse(val, out result);
            return result;
        }

        /// <summary>
        /// string类型转int
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(string val)
        {
            int result = 0;
            int.TryParse(val, out result);
            return result;
        }

        /// <summary>
        /// string类型转short
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static short ToShort(string val)
        {
            short result = 0;
            short.TryParse(val, out result);
            return result;
        }

        /// <summary>
        /// string类型转long
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long ToLong(string val)
        {
            long result = 0;
            long.TryParse(val, out result);
            return result;
        }

        /// <summary>
        /// string类型转double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double ToDouble(string val)
        {
            double result = 0;
            double.TryParse(val, out result);
            return result;
        }

        /// <summary>
        /// string类型转decimal
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string val)
        {
            decimal result = 0;
            decimal.TryParse(val, out result);
            return result;
        }

        /// <summary>
        /// string类型转float
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static float ToFloat(string val)
        {
            float result = 0f;
            float.TryParse(val, out result);
            return result;
        }

        /// <summary>
        /// string类型转Datetime(转换失败返回 1900-01-01)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string val)
        {
            DateTime result;
            if (!DateTime.TryParse(val, out result))
                return new DateTime(1900, 1, 1);

            return result;
        }

        /// <summary>
        /// string类型转bool(转换失败返回false)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool ToBoolean(string val)
        {
            bool result;
            if (bool.TryParse(val, out result))
                return result;

            return false;
        }

        /// <summary>
        /// int类型转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ToEnum<T>(int val) where T : struct
        {
            if (string.IsNullOrEmpty(val.ToString()))
                throw new ArgumentException("无法将空值转换为枚举类型！");

            T t;
            Enum.TryParse(val.ToString(), out t);
            return t;
        }

        /// <summary>
        /// 枚举转字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToDictionary<T>()
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("待转换的类型必须是枚举！", type.Name);

            var enumDic = new Dictionary<int, string>();
            Array enumValues = Enum.GetValues(type);
            foreach (Enum enumValue in enumValues)
            {
                int key = enumValue.GetHashCode();
                string name = Enum.GetName(type, enumValue);
                var field = type.GetField(name);
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                string value = attribute == null ? string.Empty : attribute.Description;
                enumDic.Add(key, value);
            }

            return enumDic;
        }

    }
}
