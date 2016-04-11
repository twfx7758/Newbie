using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Newbie.Util.Security
{
    /// <summary>
    /// Des加密解密帮助类
    /// </summary>
    public class DesHelper
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        private static string _encryptKey = "#*d@_c&?";

        /// <summary>
        /// 16位加密字符串
        /// </summary>
        private static string _encryptKey16 = "#*d@_c&?#*d@_c&?";

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="inputStr">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DesEncrypt(string inputStr)
        {
            try
            {
                return DesEncrypt(inputStr, _encryptKey);
            }
            catch (Exception)
            {
                return inputStr;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="inputStr">加密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecrypt(string inputStr)
        {
            try
            {
                return DesDecrypt(inputStr, _encryptKey);
            }
            catch (Exception)
            {
                return inputStr;
            }
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="inputStr">需要加密的字符串</param>
        /// <param name="encryptKey"></param>
        /// <returns>加密后的字符串</returns>
        public static string DesEncrypt(string inputStr, string encryptKey)
        {
            try
            {
                byte[] byKey = null;
                byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

                byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey.Substring(0, IV.Length));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(inputStr);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception)
            {
                return inputStr;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="inputStr">加密的字符串</param>
        /// <param name="decryptKey"></param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecrypt(string inputStr, string decryptKey)
        {
            try
            {
                byte[] byKey = null;
                byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] inputByteArray = new Byte[inputStr.Length];

                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, IV.Length));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(inputStr);
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    System.Text.Encoding encoding = new System.Text.UTF8Encoding();
                    return encoding.GetString(ms.ToArray());
                }
            }
            catch (Exception)
            {
                return inputStr;
            }
        }

        /// <summary>
        /// DES加密16位
        /// </summary>
        /// <param name="inputString">加密字符串</param>
        /// <param name="key">加密Key</param>
        /// <returns>返回结果</returns>
        public static string DesEncrypt16(string inputString, string key)
        {
            StringBuilder strRetValue = new StringBuilder();

            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();

                //组织成16进制字符串            
                foreach (byte b in mStream.ToArray())
                {
                    strRetValue.AppendFormat("{0:x2}", b);
                }
            }
            catch (Exception)
            {
                return strRetValue.ToString();
            }

            return strRetValue.ToString();
        }

        /// <summary>
        /// DES加密16位
        /// </summary>
        /// <param name="inputString">加密字符串</param>
        /// <returns>返回结果</returns>
        public static string DesEncrypt16(string inputString)
        {
            return DesEncrypt16(inputString, _encryptKey16);
        }

        /// <summary>
        /// Des解密16位
        /// </summary>
        /// <param name="inputString">加密字符串</param>
        /// <param name="key">加密Key</param>
        /// <returns>返回结果</returns>
        public static string DesDecrypt16(string inputString, string key)
        {
            string strRetValue = "";


            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;

                //16进制转换为byte字节
                byte[] inputByteArray = new byte[inputString.Length / 2];
                for (int x = 0; x < inputString.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(inputString.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();

                strRetValue = Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception)
            {
                return strRetValue;
            }

            return strRetValue;
        }

        /// <summary>
        /// Des解密16位
        /// </summary>
        /// <param name="inputString">加密字符串</param>
        /// <returns>返回结果</returns>
        public static string DesDecrypt16(string inputString)
        {
            return DesDecrypt16(inputString, _encryptKey16);
        }

    }
}
