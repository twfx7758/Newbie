using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Caching.Providers
{
    public enum CacheProvider
    {
        /// <summary>
        /// .Net内存缓存
        /// </summary>
        MemoryStorage = 1,

        /// <summary>
        /// Redis内存缓存
        /// </summary>
        RedisStorage = 2,

        /// <summary>
        /// STSDB内存缓存
        /// </summary>
        STSDBMemoryStorage = 3,

        /// <summary>
        /// STSDB文件缓存
        /// </summary>
        STSDBFileStorage = 4,

        /// <summary>
        /// STSDB远程缓存
        /// </summary>
        STSDBRemoteStorage =5
    }
}
