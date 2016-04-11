using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Configuration;
using ServiceStack.Redis;


namespace Newbie.Caching.Redis
{
    public class RedisCache
    {
        protected enum ReadWriteType
        {
            ReadWriteHosts = 1,
            ReadOnlyHosts = 2
        }
        private static ServiceStack.Redis.PooledRedisClientManager prcm;

        static RedisCache()
        {
            InitRedis();
        }


        #region 私有方法
        /// <summary>
        /// 获取主机列表
        /// </summary>
        /// <param name="readWriteType"></param>
        /// <returns></returns>
        private static string[] GetHosts(ReadWriteType readWriteType)
        {
            string key = string.Format("Redis.Hosts.{0}", readWriteType.ToString());
            string[] arrHosts = ConfigHelper.GetConfigString(key, "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return arrHosts;
        }
        #endregion


        /// <summary>
        /// 初始化参数
        /// </summary>
        private static void InitRedis()
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
        private static RedisNativeClient GetNativeClientForKeySpace(string key_space)
        {
            return (RedisNativeClient)(prcm.GetClient());
        }

        private static RedisNativeClient GetReadOnlyNativeClient()
        {
            return (RedisNativeClient)(prcm.GetReadOnlyClient());
        }

        /// <summary>
        /// 将一个字符串中的空格和冒号转义，并trim，然后返回，null会返回空字符串
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public static string NormalizeRedisStr(string instr)
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
        public static bool ExistsKey(string key_space, string key_id)
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
        public static object GetObj(string key_space, string obj_id)
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


        /// <summary>
        /// 保存一个对象，永久的，不缓冲的
        /// 返回true保存成功
        /// 返回false保存失败
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <param name="obj_body"></param>
        /// <returns></returns>
        public static bool SaveObj(string key_space, string obj_id, object obj_body)
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


        /// <summary>
        /// 将对象保存在keyspace:下的缓冲里，缓冲时间是ts
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <param name="obj_body"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool SaveObj(string key_space, string obj_id, object obj_body, TimeSpan ts)
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
        public static bool DelKey(string key_space, string obj_id)
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

        /// <summary>
        /// 获取通配符key
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static List<string> Keys(string key_space, string pattern)
        {
            List<string> keys = new List<string>();
            if (pattern == null || key_space == null)
                return keys;
            string redis_key = key_space + ":" + pattern + "*";
            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {
                byte[][] keysByte = RNC.Keys(redis_key);
                if (keysByte.Length > 0)
                {
                    for (int i = 0; i < keysByte.Length; i++)
                    {
                        keys.Add(System.Text.Encoding.Default.GetString(keysByte[i]));
                    }
                }
            }
            return keys;
        }
        /// <summary>
        /// 设置一个Key的过期时间
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool ExpireKey(string key_space, string obj_id, TimeSpan ts)
        {
            if (obj_id == null || key_space == null)
                return false;

            string redis_key = key_space + ":" + obj_id;

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {
                RNC.Expire(redis_key, Convert.ToInt32(ts.TotalSeconds));
            }

            return true;
        }

        /// <summary>
        /// 设置一个key的过期时间，minutes
        /// </summary>
        /// <param name="key_space"></param>
        /// <param name="obj_id"></param>
        /// <param name="expire_minutes"></param>
        /// <returns></returns>
        public static bool ExpireKey(string key_space, string obj_id, int expire_minutes)
        {
            return ExpireKey(key_space, obj_id, new TimeSpan(0, expire_minutes, 0));
        }
        public static Dictionary<string, string> GetRedisInfo(string key_space)
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


        #region 集合
        /// <summary>
        /// 将元素加入集合
        /// </summary>
        /// <param name="key_space">键命名空间</param>
        /// <param name="obj_id">集合名</param>
        /// <param name="value">元素值</param>
        /// <param name="expire">过期</param>
        public static void AddToSet(string key_space, string obj_id, string value,TimeSpan expire)
        {
            if (obj_id == null || key_space == null)
            {
                throw new ArgumentNullException("key_space或者obj_id是null");
            }

            string redis_key = key_space + ":" + obj_id;
            using (IRedisClient client = prcm.GetClient())
            {
                if (client.ContainsKey(redis_key))
                {
                    client.AddItemToSet(redis_key, value);
                }
                else 
                {
                    client.AddItemToSet(redis_key, value);
                    if (expire != null && expire.Ticks > 0)
                    {
                        client.ExpireEntryIn(redis_key, expire);
                    }
                } 
            }
        }
  
        /// <summary>
        /// 从集合里删除一个元素
        /// </summary>
        /// <param name="obj_id"></param>
        /// <param name="item"></param>
        public static void RemoveFromSet(string key_space, string obj_id, string item)
        {
            if (obj_id == null || key_space == null)
            {
                throw new ArgumentNullException("key_space或者obj_id是null");
            }

            string redis_key = key_space + ":" + obj_id;
            using (IRedisClient client = prcm.GetClient())
            {
                client.RemoveItemFromSet(redis_key, item);
            }
        }

        /// <summary>
        /// 元素是否在集合中
        /// </summary>
        /// <param name="obj_id"></param>
        /// <returns></returns>
        public static bool IsExistInSet(string key_space, string obj_id,string value)
        {
            if (obj_id == null || key_space == null)
            {
                throw new ArgumentNullException("key_space或者obj_id是null");
            }

            string redis_key = key_space + ":" + obj_id;
            using (IRedisClient client = prcm.GetClient())
            {
                return client.SetContainsItem(redis_key, value);
            }
        }

        /// <summary>
        /// 获取集合里面的所有元素
        /// </summary>
        /// <param name="obj_id"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllMemberFromSet(string key_space, string obj_id)
        {
            if (obj_id == null || key_space == null)
            {
                throw new ArgumentNullException("key_space或者obj_id是null");
            }

            string redis_key = key_space + ":" + obj_id;
            using (IRedisClient client = prcm.GetClient())
            {
                return client.GetAllItemsFromSet(redis_key);
            }
        }
        #endregion
    }

    internal class ConfigHelper
    {
        public static string GetConfigString(string key, string defaultValue)
        {
            string reV = defaultValue;
            try
            {
                reV = ConfigurationManager.AppSettings[key];
            }
            catch
            {
                reV = defaultValue;
            }
            return reV;
        }
        public static int GetConfigInt(string key, int defaultValue)
        {
            int reV = defaultValue;
            try
            {
                reV = int.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                reV = defaultValue;
            }
            return reV;
        }
    }
}
