
using System;
using System.Collections.Generic;
using System.Web;

namespace Newbie.Util
{
    /// <summary>
    /// Cookie辅助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 判断是否有该名称的cookie
        /// </summary>
        /// <param name="cookieName">cookie的名称</param>
        /// <returns></returns>
        public static bool IsHaveCookie(String cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName] != null;
        }

        /// <summary>
        /// 新增或更新cookie
        /// </summary>
        /// <param name="cookie">HttpCookie对象</param>
        public static void SetCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 新增或更新cookie
        /// </summary>
        /// <param name="cookieName">cookie的名称<</param>
        /// <param name="cookieValue">cookie的值</param>
        /// <param name="dt">过期时间</param>
        public static void SetCookie(String cookieName, String cookieValue, DateTime dt)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Expires = dt;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 新增或更新cookie【键值对类型，保存结果为key1=value&key2=value2】
        /// </summary>
        /// <param name="cookieName">cookie的名称</param>
        /// <param name="cookieValue">cookie的值</param>
        /// <param name="dt">过期时间<</param>
        public static void SetCookie(String cookieName, Dictionary<String, String> cookieValue, DateTime dt)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            foreach (var d in cookieValue)
            {
                cookie.Values.Add(d.Key, d.Value);
            }
            cookie.Expires = dt;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookieName">cookie的名称<</param>
        public static void DeleteCookie(String cookieName)
        {
            if (IsHaveCookie(cookieName))
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 获取Cookie的值
        /// </summary>
        /// <param name="cookieName">cookie的名称<</param>
        public static HttpCookie GetCookie(String cookieName)
        {
            if (IsHaveCookie(cookieName))
            {
                return HttpContext.Current.Request.Cookies[cookieName];
            }
            else
            {
                return null;
            }
        }
    }
}
