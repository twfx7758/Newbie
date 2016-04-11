using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Caching.Providers
{
    /// <summary>
    /// 存储方式
    /// </summary>
    public interface IStorage
    {

        /// <summary>
        /// 添加指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <param name="o"></param>
        void Add<TKey, TRecord>(string CollectionName, TKey key, TRecord value,DateTime absoluteExpiration);


        /// <summary>
        /// 移除指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        void Remove<TKey, TRecord>(string CollectionName, TKey key);

        /// <summary>
        /// 返回指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <returns></returns>
        TRecord  Get<TKey, TRecord>(string CollectionName, TKey key);
    }
}
