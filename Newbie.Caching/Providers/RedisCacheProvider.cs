using Newbie.Caching.Redis;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Caching.Providers
{
    public class RedisCacheProvider
    {
        private static RedisCacheProvider _intance = new RedisCacheProvider();

        public static RedisCacheProvider Intance
        {
            get { return _intance; }

        }
        private RedisCacheProvider()
        {
            InitRedis();
        }
        private enum ReadWriteType
        {
            ReadWriteHosts = 1,
            ReadOnlyHosts = 2
        }
        private static ServiceStack.Redis.PooledRedisClientManager prcm;
        private const string SpaceName = "MC";
        #region 私有方法
        /// <summary>
        /// 获取主机列表
        /// </summary>
        /// <param name="readWriteType"></param>
        /// <returns></returns>
        private string[] GetHosts(ReadWriteType readWriteType)
        {
            string key = string.Format("Redis.Hosts.{0}", readWriteType.ToString());
            string[] arrHosts = ConfigHelper.GetConfigString(key, "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return arrHosts;
        }
        #endregion


        /// <summary>
        /// 初始化参数
        /// </summary>
        private void InitRedis()
        {
            if (prcm == null)
            {
                RedisClientManagerConfig config = new RedisClientManagerConfig()
                {
                    MaxReadPoolSize = ConfigHelper.GetConfigInt("Redis.Config.MaxReadPoolSize", 5)
                    ,
                    MaxWritePoolSize = ConfigHelper.GetConfigInt("Redis.Config.MaxWritePoolSize", 5)
                    ,
                    DefaultDb = ConfigHelper.GetConfigInt("Redis.Config.DefaultDb", 0)
                     ,
                    AutoStart = true
                };
                prcm = new ServiceStack.Redis.PooledRedisClientManager(
                    GetHosts(ReadWriteType.ReadWriteHosts)
                    , GetHosts(ReadWriteType.ReadOnlyHosts)
                    , config);
            }
        }



        /// <summary>
        /// 从当前的池子里为一个keyspace找一个客户端
        /// </summary>
        /// <param name="key_space"></param>
        /// <returns></returns>
        private RedisNativeClient GetNativeClientForKeySpace(string key_space)
        {
            return (RedisNativeClient)(prcm.GetClient());
        }

        private RedisNativeClient GetReadOnlyNativeClient()
        {
            return (RedisNativeClient)(prcm.GetReadOnlyClient());
        }

        /// <summary>
        /// 将一个字符串中的空格和冒号转义，并trim，然后返回，null会返回空字符串
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public string NormalizeRedisStr(string instr)
        {
            if (instr == null)
                return "";

            return instr.Trim().Replace(" ", "_").Replace(":", "_");

        }

        //******************************以上是有关连接池的部分**********************************//

        /// <summary>
        /// 判断一个key是否存在 
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="key_id"></param>
        /// <returns></returns>
        public bool ExistsKey(string key_space, string key_id)
        {
            if (key_id == null || key_space == null)
                return false;

            string redis_key = key_space + ":" + key_id;

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {
                int rt = RNC.Exists(redis_key);
                return (rt == 1);
            }
        }


        ///自定义的序列化对象，这个跟标准的redis string不同，前面加了个类型

        /// <summary>
        /// 获得一个对象
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <returns></returns>
        private object GetObj(string key_space, string obj_id)
        {
            if (obj_id == null || key_space == null)
                return false;

            string redis_key = key_space + ":" + obj_id;
            using (RedisNativeClient RNC = GetReadOnlyNativeClient())
            {

                byte[] saved_content = RNC.Get(redis_key);

                if (saved_content == null)
                    return null;

                return SerializeHelper.DeSerialize(saved_content);
            }
        }
        public T Get<T>(string key)
        {
            var result = GetObj(SpaceName, key);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        /// <summary>
        /// 保存一个对象，永久的，不缓冲的
        /// 返回true保存成功
        /// 返回false保存失败
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <param name="obj_body"></param>
        /// <returns></returns>
        private bool SaveObj(string key_space, string obj_id, object obj_body)
        {

            if (obj_id == null || key_space == null)
                return false;

            string redis_key = key_space + ":" + obj_id;

            byte[] saved_content = SerializeHelper.Serialize(obj_body);

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {
                RNC.Set(redis_key, saved_content);
            }

            return true;
        }
        public bool Save<T>(string key, T data)
        {
            return SaveObj(SpaceName, key, data);
        }
        public bool Save<T>(string key, T data, DateTime absoluteExpiration)
        {
            return SaveObj(SpaceName, key, data, absoluteExpiration - DateTime.Now);
        }
        /// <summary>
        /// 将对象保存在keyspace:下的缓冲里，缓冲时间是ts
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <param name="obj_body"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private bool SaveObj(string key_space, string obj_id, object obj_body, TimeSpan ts)
        {
            if (obj_id == null || key_space == null)
                return false;

            string redis_key = key_space + ":" + obj_id;

            byte[] saved_content = SerializeHelper.Serialize(obj_body);

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {
                RNC.SetEx(redis_key, (int)ts.TotalSeconds, saved_content);
            }

            return true;
        }

        /// <summary>
        /// 清除一个对象
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <returns></returns>
        private bool DelKey(string key_space, string obj_id)
        {
            if (obj_id == null || key_space == null)
                return false;

            string redis_key = key_space + ":" + obj_id;

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {
                RNC.Del(redis_key);
            }

            return true;
        }
        public bool DelKey(string key)
        {

            return DelKey(SpaceName, key);
        }
        public Dictionary<string, string> GetRedisInfo(string key_space)
        {
            if (string.IsNullOrEmpty(key_space))
            {
                return null;
            }
            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {
                return RNC.Info;

            }
        }

    }

}
