using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using STSdb4;
using STSdb4.Data;
using STSdb4.Database;

namespace Newbie.Caching.Providers
{
    public class STSDBRemoteStorage : IStorage
    {
        private const string KeyExpiration = "Expiration";
        private static object syncRoot = new object();
        private static IStorageEngine memoryInstance = null;



        private string RemoteServer 
       {
           get
           {
               //读取应用程序中的默认配置
               string serveraname = System.Configuration.ConfigurationManager.AppSettings.Get("STSDBRemoteServer");

               if (string.IsNullOrEmpty(serveraname))
                   throw new Exception("未检测到有效的STSDB RemoteServer 配置。");


               return serveraname;
           }
        }


        private IStorageEngine Engine
        {

            get
            {
                lock (syncRoot)
                {
                    if (memoryInstance == null)
                        memoryInstance = STSdb.FromNetwork(RemoteServer,7182);
                }
                return memoryInstance;
            }
        }


        /// <summary>
        /// 添加指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        /// <param name="o"></param>
        public void Add<TKey, TRecord>(string CollectionName, TKey key, TRecord value, DateTime absoluteExpiration)
        {
            var engine = Engine;

            //写入缓存对象值
            var table = engine.OpenXTable<TKey, TRecord>(CollectionName);
            table[key] = value;

            //写入缓存有效时间
            var expiration = engine.OpenXTable<TKey, DateTime>(KeyExpiration);
            var expirationDate = absoluteExpiration == null || absoluteExpiration <= DateTime.Now ? DateTime.Now.AddMinutes(30) : absoluteExpiration;
            expiration[key] = expirationDate;

            engine.Commit();
            engine.Dispose();
        }

        /// <summary>
        /// 移除指定key的对象
        /// </summary>
        /// <param name="objKey"></param>
        public void Remove<TKey, TRecord>(string CollectionName, TKey key)
        {
            var engine = Engine;

            //移除缓存对象值
            var table = engine.OpenXTable<TKey, TRecord>(CollectionName);
            table.Delete(key);

            //移除缓存有效时间
            var expiration = engine.OpenXTable<TKey, DateTime>(KeyExpiration);
            expiration.Delete(key);
            engine.Commit();
            engine.Dispose();

        }

        /// <summary>
        /// 返回指定key的对象
        /// </summary>
        /// <param name="TRecord"></param>
        /// <returns></returns>
        public TRecord Get<TKey, TRecord>(string CollectionName, TKey key)
        {
            TRecord result;
            var engine = Engine;
            var table = engine.OpenXTable<TKey, TRecord>(CollectionName);
            if (table.TryGet(key, out result))
            {
                result = table.Find(key);
                var expiration = engine.OpenXTable<TKey, DateTime>(KeyExpiration);
                DateTime expirationDate;
                if (expiration.TryGet(key, out expirationDate))
                {
                    //判断是否过期
                    if (expirationDate < DateTime.Now)
                    {
                        result = default(TRecord);
                        table.Delete(key);
                        expiration.Delete(key);
                        engine.Commit();
                    }
                }

            }

            else
                result = default(TRecord);


            engine.Dispose();
            return result;
        }

    }
}
