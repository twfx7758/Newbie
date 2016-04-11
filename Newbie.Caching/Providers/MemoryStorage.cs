using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Newbie.Caching.Providers
{
    public class MemoryStorage :IStorage
    {

        //System.Web.Caching.Cache appCache = System.Web.HttpContext.Current.Cache;
        System.Web.Caching.Cache appCache = System.Web.HttpRuntime.Cache;


        /// <summary>
        /// 添加指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <param name="o"></param>
        public void Add<TKey, TRecord>(string CollectionName, TKey key, TRecord value,DateTime absoluteExpiration)
        {
            appCache.Add(key.ToString(), value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration,CacheItemPriority.High,null);
        }

        /// <summary>
        /// 移除指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        public void Remove<TKey, TRecord>(string CollectionName, TKey key)
        {
            appCache.Remove(key.ToString());
        }

        /// <summary>
        /// 返回指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <returns></returns>
        public TRecord Get<TKey, TRecord>(string CollectionName, TKey key)
        {
            TRecord re = (TRecord)appCache[key.ToString()];
            return re;

        }
    }
}
