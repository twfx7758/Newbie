// =================================================================== 
// 项目说明
//====================================================================
// 苗志国。@Copy Right 2014
// 文件： ICacheStrategy.cs
// 项目名称：买车网
// 创建时间：2014/3/27
// 负责人：苗志国
// ===================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newbie.Caching.Providers;

namespace Newbie.Caching
{
 
    // <summary>
    /// 公共缓存客户端
    /// </summary>
    public class CacheManagerClient
    {
        /// <summary>
        /// 添加指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <param name="o"></param>
        public static void Add<TKey, TRecord>(string CollectionName, TKey key, TRecord value, int CacheSeconds,Providers.CacheProvider cacheProvider,Type ValueType)
        {
            IStorage CacheStorage = CacheFactory.GetCacheStorage(cacheProvider);
            if ((cacheProvider == CacheProvider.STSDBFileStorage) || (cacheProvider == CacheProvider.STSDBMemoryStorage) || (cacheProvider == CacheProvider.STSDBRemoteStorage))
            {
                string ret = JsonHelper.SerializeJson(value);
                CacheStorage.Add<TKey, string>(CollectionName, key, ret, DateTime.Now.AddSeconds(CacheSeconds));
            }
            else
            {
                CacheStorage.Add<TKey, TRecord>(CollectionName, key, value, DateTime.Now.AddSeconds(CacheSeconds));
            }
        }

        /// <summary>
        /// 移除指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        public static void Remove<TKey, TRecord>(string CollectionName, TKey key, Providers.CacheProvider cacheProvider)
        {
            IStorage CacheStorage = CacheFactory.GetCacheStorage(cacheProvider);
            CacheStorage.Remove<TKey, TRecord>(CollectionName, key);
        }

        /// <summary>
        /// 返回指定key的对象
        /// </summary>
        /// <param name="TRecord"></param>
        /// <returns></returns>
        public static TRecord Get<TKey, TRecord>(string CollectionName, TKey key, Providers.CacheProvider cacheProvider,Type ValueType)
        {
            IStorage CacheStorage = CacheFactory.GetCacheStorage(cacheProvider);
            if ((cacheProvider == CacheProvider.STSDBFileStorage) || (cacheProvider == CacheProvider.STSDBMemoryStorage) || (cacheProvider == CacheProvider.STSDBRemoteStorage))
            {
                string ret=CacheStorage.Get<TKey, string>(CollectionName, key);
                return (TRecord)JsonHelper.Deserializer(ret, ValueType);
            }
            else
            {

                return CacheStorage.Get<TKey, TRecord>(CollectionName, key);
            }
        }
    }
}

