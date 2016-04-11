using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newbie.Caching.Redis;

namespace Newbie.Caching.Providers
{
    public class RedisStorage : IStorage
    {
        private const string SpaceName = "MC";
        /// <summary>
        /// 添加指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <param name="o"></param>
        public void Add<TKey, TRecord>(string CollectionName, TKey key, TRecord value, DateTime absoluteExpiration)
        {
            RedisCache.SaveObj(SpaceName, key.ToString(), value, absoluteExpiration - DateTime.Now);
        }

        /// <summary>
        /// 移除指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        public void Remove<TKey, TRecord>(string CollectionName, TKey key)
        {
            RedisCache.DelKey(SpaceName, key.ToString());
        }

        /// <summary>
        /// 返回指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <returns></returns>
        public TRecord Get<TKey, TRecord>(string CollectionName, TKey key)
        {
            TRecord result;
            try
            {
                result = (TRecord)RedisCache.GetObj(SpaceName, key.ToString());
                if (result != null && Object.ReferenceEquals(result.GetType(), typeof(System.IO.MemoryStream)))
                {
                    if ((result as System.IO.MemoryStream).Length == 0)
                        result = default(TRecord);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }


    }
}
