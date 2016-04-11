// =================================================================== 
// 项目说明
//====================================================================
// LiuHaoJie。@Copy Right 2014
// 文件： DealerInfo.cs
// 项目名称：买车网
// 创建时间：2014/10/27
// 负责人：LiuHaoJie
// ===================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Data;

namespace Newbie.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class FacadeHelper
    {
        private static Dictionary<string, Dictionary<int, string>> _EnumList =
            new Dictionary<string, Dictionary<int, string>>(); //枚举缓存池

        /// <summary>
        /// 将枚举绑定到ListControl
        /// </summary>
        /// <param name="listControl">ListControl</param>
        /// <param name="enumType">枚举类型</param>
        public static void FillDropDownList(DropDownList listControl, Type enumType)
        {
            listControl.Items.Clear();
            listControl.DataSource = GetEnumToDictionary(enumType);
            listControl.DataValueField = "key";
            listControl.DataTextField = "value";
            listControl.DataBind();
        }

        /// <summary>
        /// 返回枚举值、描述的字典
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumDictionaryForName<T>() where T : struct
        {
            var dic = new Dictionary<string, string>();
            var t = typeof (T);
            var names = Enum.GetNames(t);
            foreach (var name in names)
            {
                var fieldInfo = t.GetField(name);
                var attr =
                    Attribute.GetCustomAttribute(fieldInfo, typeof (DescriptionAttribute), false) as
                        DescriptionAttribute;
                if (Equals(attr, null))
                    continue;

                dic.Add(name, attr.Description);
            }

            return dic;
        }

        /// <summary>
        /// 返回(枚举值对应的数值,描述)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumDictionaryForValue<T>() where T : struct
        {
            var dic = new Dictionary<string, string>();
            var t = typeof (T);
            var names = Enum.GetNames(t);
            foreach (var name in names)
            {
                T tmpEnum;
                Enum.TryParse(name, out tmpEnum);
                var fieldInfo = t.GetField(name);
                var attr =
                    Attribute.GetCustomAttribute(fieldInfo, typeof (DescriptionAttribute), false) as
                        DescriptionAttribute;
                if (Equals(attr, null))
                    continue;

                dic.Add(tmpEnum.GetHashCode().ToString(), attr.Description);
            }

            return dic;
        }

        /// <summary>
        /// 将枚举转换成Dictionary&lt;int, string&gt;
        /// Dictionary中，key为枚举项对应的int值；value为：若定义了name属性，则取它，否则取name
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumToDictionary(Type enumType)
        {
            string keyName = enumType.FullName;
            if (!_EnumList.ContainsKey(keyName))
            {
                Dictionary<int, string> list = new Dictionary<int, string>();

                foreach (int i in Enum.GetValues(enumType))
                {
                    string name = Enum.GetName(enumType, i);

                    //取显示名称
                    string showName = string.Empty;
                    object[] atts = enumType.GetField(name).GetCustomAttributes(typeof (DescriptionAttribute), false);
                    if (atts.Length > 0) showName = ((DescriptionAttribute) atts[0]).Description;

                    list.Add(i, string.IsNullOrEmpty(showName) ? name : showName);
                }

                object syncObj = new object();

                if (!_EnumList.ContainsKey(keyName))
                {
                    lock (syncObj)
                    {
                        if (!_EnumList.ContainsKey(keyName))
                        {
                            _EnumList.Add(keyName, list);
                        }
                    }
                }
            }

            return _EnumList[keyName];
        }

        /// <summary>
        /// DataTable转化成Dictionary
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<int, string> DataTableToDictionary(DataTable dt, string key, string value)
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int keyName = Convert.ToInt32(dt.Rows[i][key]);
                if (!list.ContainsKey(keyName))
                {
                    list.Add(keyName, dt.Rows[i][value].ToString());
                }
            }
            return list;
        }

        /// <summary>
        /// 获取枚举值对应的显示名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="intValue">枚举项对应的int值</param>
        /// <returns></returns>
        public static string GetEnumDescriptionName(Type enumType, int intValue)
        {
            var keyValus = GetEnumToDictionary(enumType);
            return keyValus.ContainsKey(intValue) ? keyValus[intValue] : string.Empty;
        }

        /// <summary>
        /// 获取枚举值对应的显示名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDescriptionName<T>(string val)
        {
            int intVal;
            if (!int.TryParse(val, out intVal))
                return string.Empty;

            var keyValus = GetEnumToDictionary(typeof (T));
            return keyValus.ContainsKey(intVal) ? keyValus[intVal] : string.Empty;
        }

        /// <summary>
        /// 获取枚举的显示名称
        /// </summary>
        /// <param name="enumVal">枚举值</param>
        /// <returns>枚举名称</returns>
        private static string GetEnumName<T>(object enumVal)
        {
            return Enum.GetName(typeof (T), enumVal);
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string FormatMoney(string money)
        {
            double tmp;
            double.TryParse(money, out tmp);
            tmp = tmp/10000;
            return tmp.ToString("f2");
        }
    }
}