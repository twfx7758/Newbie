using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Caching
{
    [Serializable]
    public class MethodCacheAttribute : PostSharp.Aspects.OnMethodBoundaryAspect
    {
        /// <summary>
        /// 缓存有效时长，单位：秒
        /// </summary>
        private int m_CacheSeconds = 0;

        /// <summary>
        /// 缓存分类
        /// </summary>
        private string m_Category = "";

        /// <summary>
        /// 缓存主键名称
        /// </summary>
        private string m_PrimaryKeyValue = "";

        /// <summary>
        /// 缓存回调
        /// </summary>
        private string m_PrimaryKeyCall = "";
        /// <summary>
        /// 缓存版本号
        /// </summary>
        private string m_Version = "";

        /// <summary>
        /// 缓存存储类型
        /// </summary>
        private Providers.CacheProvider m_CacheProvider;

        private MethodCacheAttribute()
        {
        }



        /// <summary>
        /// 构造方法 
        /// </summary>
        /// <param name="category">存储对象分类</param>
        /// <param name="primaryKeyValue">存储对象分类主键</param>
        /// <param name="CacheMinute">缓存时长(单位：分钟)</param>
        public MethodCacheAttribute(string category, string primaryKeyValue, int CacheSeconds)
            : this(category, primaryKeyValue, CacheSeconds, Providers.CacheProvider.MemoryStorage)
        {
        }


        /// <summary>
        /// 构造方法 
        /// </summary>
        /// <param name="category">存储对象分类</param>
        /// <param name="primaryKeyValue">存储对象分类主键</param>
        /// <param name="CacheMinute">缓存时长(单位：分钟)</param>
        /// <param name="cacheProvider">缓存存储类型</param>
        public MethodCacheAttribute(string category, string primaryKeyValue, int CacheSeconds, Providers.CacheProvider cacheProvider)
        {
            this.m_Category = category;
            this.m_CacheSeconds = CacheSeconds;
            this.m_PrimaryKeyValue = primaryKeyValue;
            this.m_CacheProvider = cacheProvider;
        }

        public MethodCacheAttribute(string category, string primaryKeyValue, int CacheSeconds, Providers.CacheProvider cacheProvider,string version)
        {
            this.m_Category = category;
            this.m_CacheSeconds = CacheSeconds;
            this.m_PrimaryKeyValue = primaryKeyValue;
            this.m_CacheProvider = cacheProvider;
            this.m_Version = version;
        }

        /// <summary>
        /// 方法执行前，判断是否存在有效缓存
        /// </summary>
        /// <param name="args"></param>
        public override void OnEntry(PostSharp.Aspects.MethodExecutionArgs args)
        {
            try
            {
                this.m_PrimaryKeyValue = GetPrimaryKey(args);
            }
            catch (Exception ee)
            {
                WriteLog("OnEntry调用GetPrimaryKey函数失败：" + ee.Message + ee.StackTrace);
                return;
            }

            string key = GetMethodKey(args,this);
            try
            {

                object returnObj = CacheManagerClient.Get<string, object>(m_Category, key, this.m_CacheProvider, ((System.Reflection.MethodInfo)args.Method).ReturnType);
                if (returnObj != null)
                {
                    WriteLog(key + "数据不为空直接返回");
                    args.ReturnValue = returnObj;
                    args.FlowBehavior = PostSharp.Aspects.FlowBehavior.Return;
                    return;
                }
                else
                {
                    WriteLog(key + "数据为空执行函数体");
                }
            }

            catch (Exception ee)
            {
                WriteLog(key + "发生异常" + ee.Message + ee.StackTrace);
            }
        }

        /// <summary>
        /// 方法执行后，置入缓存
        /// </summary>
        /// <param name="args"></param>
        public override void OnSuccess(PostSharp.Aspects.MethodExecutionArgs args)
        {
            try
            {
                string key = GetMethodKey(args,this);
                CacheManagerClient.Add<string, object>(m_Category, key, args.ReturnValue, m_CacheSeconds, this.m_CacheProvider, ((System.Reflection.MethodInfo)args.Method).ReturnType);
            }
            catch (Exception ee)
            {

            }
            base.OnSuccess(args);
        }


        /// <summary>
        /// 获取缓存主键
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private string GetPrimaryKey(PostSharp.Aspects.MethodExecutionArgs args)
        {
            string ret = "";
            if (String.IsNullOrEmpty(m_PrimaryKeyCall))
                return ret;

            string[] methods = m_PrimaryKeyCall.Split('.');

            System.Reflection.ParameterInfo[] pi = args.Method.GetParameters();
            for (int i = 0; i < pi.Length; i++)
            {
                System.Reflection.ParameterInfo para = pi[i];
                if (para.Name == methods[0])
                {
                    if (methods.Length > 1)
                    {
                        object pkey = args.Arguments[pi[i].Position].GetType().InvokeMember(
                            methods[1],
                            System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty,
                            null, args.Arguments[pi[i].Position], null);
                        if (pkey != null)
                            ret = pkey.ToString();
                        else
                            throw new NullReferenceException("GetPrimaryKey函数获得空主键KEY");
                    }
                    else
                    {
                        ret = args.Arguments[para.Position].ToString();
                    }
                }
            }
            return ret;
        }

        private static string GetMethodKey(PostSharp.Aspects.MethodExecutionArgs args,MethodCacheAttribute metadata)
        {
            string key = args.Method.Module.Name + "." + args.Method.DeclaringType.FullName + "." + args.Method.Name + "_";
            if (!string.IsNullOrEmpty(metadata.m_Version))
            {
                key += metadata.m_Version + "_";
            }
            foreach (object arg in args.Arguments)
            {
                if (arg != null)
                {
                    key += arg.ToString();
                }
                key += "_";
            }
            return key.Substring(0, key.Length - 1).Replace("BitAuto.EP.MaiChe.", "").Replace(".dll","");
        }
        private void WriteLog(string msg)
        { }


    }
}
