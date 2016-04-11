using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Newbie.Util
{
    public static class CacheManager<T>
    {

        /// <summary>
        /// 添加缓存项，绝对过期，时间自己指定
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="minute"></param>
        public static void AddToCache(string key, T value, int minute)
        {
            //  HttpRuntime.Cache.Add(key, value, null, DateTime.Now.AddMinutes(minute), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(minute), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 从缓存中读取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetFromCache(string key)
        {
            T value = default(T);
            if (HttpRuntime.Cache[key] != null)
            {
                try
                {
                    value = (T)HttpRuntime.Cache[key];
                }
                catch (Exception ex)
                {
                    value = default(T);
                }
            }
            return value;

        }
        /// <summary>
        /// 获取缓存值，如果缓存不存在，从提供的获取方法中获取
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="getValueFromSource">如缓存项不存在，从源数据读取并加入自己的缓存的委托</param>
        /// <returns></returns>
        public static T BaseGetValue(String key, Func<T> getValueFromSource)
        {
            T value = GetFromCache(key);

            if (value == null || value.Equals(default(T)))
            {
                value = getValueFromSource();
            }

            return value;
        }
        /// <summary>
        /// 获取缓存值，如果缓存不存在，从提供的获取方法中获取，并加入到缓存中
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="getValueFromSource">委托方法</param>
        /// <param name="minute">缓存时间，分钟</param>
        /// <returns></returns>
        public static T BaseGetValue(String key, Func<T> getValueFromSource, int minute)
        {
            T value = GetFromCache(key);

            if (value == null || value.Equals(default(T)))
            {
                value = getValueFromSource();
            }
            if (value != null)
            {
                AddToCache(key, value, minute);
            }

            return value;
        }

        public static void AddToCache(string uri, T doc, CacheDependency filedependency)
        {
            HttpRuntime.Cache.Insert(uri, doc, filedependency);

        }

        public static void RemoveCache(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}
