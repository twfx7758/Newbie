using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;

namespace Newbie.Util.Security
{
    /// <summary>
    /// MD5 加密解密帮助静态类
    /// </summary>
    public class MD5Helper
    {

        /// <summary>
        /// 获取MD5加密字符串
        /// </summary>
        /// <param name="paras">可以传入多个参数，进行加密</param>
        /// <returns>加密后的字符串</returns>
        public static string GetMD5Hash(params string[] paras)
        {
            return GetMD5Hash(Encoding.Default, paras);
        }

        /// <summary>
        /// 获取MD5加密字符串
        /// </summary>
        /// <param name="enconde">加密编码</param>
        /// <param name="paras">可以传入多个参数，进行加密</param>
        /// <returns>加密后的字符串</returns>
        public static string GetMD5Hash(Encoding enconde, params string[] paras)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            byte[] data = md5Hasher.ComputeHash(enconde.GetBytes(string.Concat(paras)));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// 验证md5值是否正确
        /// </summary>
        /// <param name="md5Value">加密的MD5值</param>
        /// <param name="paras">需要加密的字符串</param>
        /// <returns>true：验证正确 False：验证错误</returns>
        public static bool VerifyMd5Hash(string md5Value, params string[] paras)
        {
            string signReslut = GetMD5Hash(string.Concat(paras));

            return signReslut.Equals(md5Value);
        }

        /// <summary>
        /// 验证md5值是否正确
        /// </summary>
        /// <param name="md5Value">加密的MD5值</param>
        /// <param name="encoding">加密编码</param>
        /// <param name="paras">需要加密的字符串</param>
        /// <returns>true：验证正确 False：验证错误</returns>
        public static bool VerifyMd5Hash(string md5Value, Encoding encoding, params string[] paras)
        {
            string signReslut = GetMD5Hash(encoding, string.Concat(paras));

            return signReslut.Equals(md5Value);
        }

        /// <summary>
        /// 采用 Forms 身份验证方式产生MD5加密
        /// </summary>
        /// <param name="paras">参数集合</param>
        /// <returns>加密后的字符串</returns>
        public static string GetMD5String(params string[] paras)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(string.Concat(paras).ToUpper(), "MD5");
        }


        /// <summary>
        /// 采用 Forms 身份验证方式产生MD5加密
        /// </summary>
        /// <param name="paras">参数集合</param>
        /// <returns>加密后的字符串</returns>
        public static string GetMD5StringWithoutToUpper(params string[] paras)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(string.Concat(paras), "MD5");
        }

        /// <summary>
        /// 会将参数转大写后再加密，供API项目函数进行参数验证时使用
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string MD5Key(string key)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key.ToUpper(), "MD5");
        }
    }
}
