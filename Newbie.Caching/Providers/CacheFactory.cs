using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Caching.Providers
{
    public class CacheFactory
    {
        public static IStorage GetCacheStorage(CacheProvider cacheProvider)
        {
            if (cacheProvider == CacheProvider.RedisStorage)
            {
                return new RedisStorage();
            }
            if (cacheProvider == CacheProvider.STSDBFileStorage)
            {
                return new STSDBFileStorage();
            }
            if (cacheProvider == CacheProvider.STSDBMemoryStorage)
            {
                return new STSDBMemoryStorage();
            }
            if (cacheProvider == CacheProvider.STSDBRemoteStorage)
            {
                return new STSDBRemoteStorage();
            }
            else
            {
                return new MemoryStorage();
            }
        } 

    }
}
