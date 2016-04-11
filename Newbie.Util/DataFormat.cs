using System;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
namespace Newbie.Util
{
    public partial class DataFormat
    {
        public DataFormat()
        {
            //
            // 在此处添加构造函数逻辑
            //
        }

        #region 格式化字符串统一显示,检查字符串，防止null数值报错
        /// <summary>
        /// 格式化字符串统一显示,检查字符串，防止null数值报错
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static string FormatString(object obj)
        {
            if (obj != null)
            {
                return obj.ToString().Trim();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 格式化字符串统一显示,检查字符串，防止null数值报错
        /// </summary>
        /// <param name="str"></param>
        /// <param name="NullReturn">如果str为null或者空，就返回NullReturn</param>
        /// <returns></returns>
        public static string FormatString(object obj, string NullReturn)
        {
            if (obj != null && obj.ToString() != "")
            {
                return obj.ToString().Trim();
            }
            else
            {
                return NullReturn;
            }
        }
        #endregion

        #region 格式化整数统一显示,检查字符串，防止null数值报错

        public static short FormatShort(object obj)
        {
            return FormatShort(obj, 0);
        }
        public static short FormatShort(object obj, short NullReturn)
        {
            if (obj == null)
            {
                return NullReturn;
            }
            short i = 0;
            try
            {
                i = Convert.ToInt16(obj);
            }
            catch
            {
                i = NullReturn;
            }
            return i;
        }

        public static short FormatInt(object obj)
        {
            return FormatShort(obj);
        }
        public static short FormatInt(object obj, short NullReturn)
        {
            return FormatShort(obj, NullReturn);
        }
        /// <summary>
        /// 格式化整数统一显示,检查字符串，防止null数值报错
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static int FormatInt32(object obj)
        {

            if (obj == null)
            {
                return 0;
            }
            int i = 0;
            try
            {
                i = Convert.ToInt32(obj);
            }
            catch
            {
                i = 0;
            }
            return i;
        }
        public static Int64 FormatInt64(object obj)
        {

            if (obj == null)
            {
                return 0;
            }
            Int64 i = 0;
            try
            {
                i = Convert.ToInt64(obj);
            }
            catch
            {
                i = 0;
            }
            return i;
        }

        public static Int64 FormatInt64(object obj, long NullReturn)
        {

            if (obj == null)
            {
                return NullReturn;
            }
            Int64 i = 0L;
            try
            {
                i = Convert.ToInt64(obj);
            }
            catch
            {
                i = NullReturn;
            }
            return i;
        }


        /// <summary>
        /// 整数，防止null数值报错
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static int FormatInt32(object obj, int NullReturn)
        {
            if (obj == null)
            {
                return NullReturn;
            }
            int i = 0;
            try
            {
                i = Convert.ToInt32(obj);
            }
            catch
            {
                i = NullReturn;
            }
            return i;
        }
        #endregion

        #region 格式化货币统一显示,检查字符串，防止null数值报错
        /// <summary>
        /// 格式化货币统一显示,检查字符串，防止null数值报错
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        [Obsolete("请使用FormatDouble方法")]
        public static double Formatdouble(object obj)
        {
            return FormatDouble(obj);
        }
        [Obsolete("请使用FormatDouble方法")]
        public static double Formatdouble(object obj, double NullReturn)
        {
            return FormatDouble(obj, NullReturn);
        }

        /// <summary>
        /// 格式化货币统一显示,检查字符串，防止null数值报错
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static double FormatDouble(object obj)
        {

            if (obj == null)
            {
                return 0;
            }
            double i = 0;
            try
            {
                i = Convert.ToDouble(obj);
            }
            catch
            {
                i = 0;
            }
            return i;
        }
        /// <summary>
        /// 格式化货币统一显示,检查字符串，防止null数值报错
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static double FormatDouble(object obj, double NullReturn)
        {
            if (obj == null)
            {
                return NullReturn;
            }
            double i = 0;
            try
            {
                i = Convert.ToDouble(obj);
            }
            catch
            {
                i = NullReturn;
            }
            return i;
        }
        /// <summary>
        /// 格式化货币统一显示,检查字符串，防止null数值报错
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        #endregion

        #region 格式化Decimal
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="NullReturn"></param>
        /// <returns></returns>
        public static decimal FormatDecimal(object obj, decimal NullReturn)
        {
            if (obj == null)
            {
                return NullReturn;
            }
            decimal i = 0;
            try
            {
                i = Convert.ToDecimal(obj);
            }
            catch
            {
                i = NullReturn;
            }
            return i;
        }

        public static decimal FormatDecimal(object obj)
        {
            return FormatDecimal(obj, 0);
        }
        #endregion

        #region 格式化Single
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="NullReturn"></param>
        /// <returns></returns>
        public static Single FormatSingle(object obj, Single NullReturn)
        {
            if (obj == null)
            {
                return NullReturn;
            }
            Single i = 0;
            try
            {
                i = Convert.ToSingle(obj);
            }
            catch
            {
                i = NullReturn;
            }
            return i;
        }

        public static Single FormatSingle(object obj)
        {
            return FormatSingle(obj, 0);
        }
        #endregion

        #region 格式化Float
        /// <summary>
        /// 格式化Float
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static float FormatFloat(object obj)
        {
            return FormatFloat(obj, 0);
        }

        /// <summary>
        /// 格式化Float
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static float FormatFloat(object obj, float NullReturn)
        {
            if (obj == null)
            {
                return NullReturn;
            }
            float i = NullReturn;
            bool result = float.TryParse(obj.ToString(), out i);
            if (!result) return NullReturn;
            return i;
        }
        #endregion

        #region 格式化boolean
        /// <summary>
        /// 格式化boolean
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static Boolean FormatBoolean(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            string str = obj.ToString().Trim().ToUpper();
            if (str == "TRUE" || str == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean FormatBoolean(object obj, bool dufaultvalue)
        {
            if (obj == null)
            {
                return dufaultvalue;
            }
            string str = obj.ToString().Trim().ToUpper();
            if (str == "TRUE" || str == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 用户保存时间，空的转成 System.DateTime.Today
        /// <summary>
        /// 用户保存时间，空的转成 System.DateTime.Today
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime FormatDateTime(object obj)
        {
            //切记切记
            //当实例化一个实体的时候,实体的时间字段并不是null,是0001-01-01
            if (obj == null || obj.ToString() == "" || obj == System.DBNull.Value)
            {
                return System.DateTime.Now;
            }

            DateTime sj;

            try
            {
                sj = Convert.ToDateTime(obj);
            }
            catch
            {
                return System.DateTime.Now;
            }

            if (sj.Year == 1)
            {
                return System.DateTime.Now;
            }

            return sj;
        }
        #endregion

        #region 格式化日期
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime FormatDateTime(object obj, System.DateTime DefauleTime)
        {

            if (obj == null)
            {
                return DefauleTime;
            }

            if ((obj.Equals(System.String.Empty)) || (obj.Equals(System.DBNull.Value)))
            {
                return DefauleTime;
            }

            DateTime sj;
            try
            {
                sj = Convert.ToDateTime(obj);
            }
            catch
            {
                return DefauleTime;
            }

            if (sj.Year == 1)
            {
                return DefauleTime;
            }

            return sj;
        }
        #endregion

        #region 格式化开始日期及结束日期,主要用于日期段搜索

        /// <summary>
        /// 返回开始日期,并格式化为:2008-08-11 0:00:00
        /// </summary>
        /// <param name="dateTime">开始日期,为空时默认为当天日期</param>
        /// <returns></returns>
        public static DateTime FormatStartDateTime(DateTime dateTime)
        {
            DateTime dt = DateTime.Now.Date;
            if (dateTime != null)
            {
                dt = dateTime.Date;
            }
            return dt;
        }

        public static DateTime FormatStartDateTime(string dateTime)
        {
            DateTime dt = FormatDateTime(dateTime);
            return dt.Date;
        }

        /// <summary>
        /// 返回结束日期,形式为:2008-08-11 23:59:59
        /// </summary>
        /// <param name="dateTime">结束日期,为空时默认为当天日期</param>
        /// <returns></returns>
        public static DateTime FormatEndDateTime(DateTime dateTime)
        {
            DateTime dt = DateTime.Now.Date;
            if (dateTime != null)
            {
                dt = dateTime.Date;
            }
            return dt.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
        public static DateTime FormatEndDateTime(string dateTime)
        {
            DateTime dt = FormatDateTime(dateTime).Date;
            return dt.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
        #endregion

        #region 格式化Guid
        /// <summary>
        /// 格式化Guid
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="NullReturn"></param>
        /// <returns></returns>
        public static Guid FormatGuid(object obj, Guid NullReturn)
        {
            if (obj == null)
            {
                return NullReturn;
            }
            try
            {
                string guid = FormatString(obj, "");
                return new Guid(guid);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 格式化Guid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Guid FormatGuid(object obj)
        {
            return FormatGuid(obj, Guid.Empty);
        }
        #endregion

        #region 时间统一显示,本函数将时间转换为8位长度的时间字符串,过滤掉1900
        /// <summary>
        /// 时间统一显示,本函数将时间转换为8位长度的时间字符串,过滤掉1900
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static string ShowDataTimeyyyyMMddHide1900(object obj)
        {
            string s;
            if (obj == null)
            {
                return "";
            }

            try
            {
                DateTime sj;
                string yue;
                string ri;
                sj = Convert.ToDateTime(obj);
                if (Convert.ToInt32(sj.Month) < 10 && sj.Month.ToString().Trim().Length < 2)
                {
                    yue = "0" + sj.Month.ToString().Trim();
                }
                else
                {
                    yue = sj.Month.ToString().Trim();
                }

                if (Convert.ToInt32(sj.Day) < 10 && sj.Day.ToString().Trim().Length < 2)
                {
                    ri = "0" + sj.Day.ToString().Trim();
                }
                else
                {
                    ri = sj.Day.ToString().Trim();
                }

                s = sj.Year.ToString().Trim() + "-" + yue.ToString().Trim() + "-" + ri.ToString().Trim();
            }
            catch
            {
                s = "";
            }

            if (s == "1900-1-1" || s == "1900-01-01")
            {
                s = "";
            }
            if (s == "1-1-1" || s == "1-01-01")
            {
                s = "";
            }
            return s;
        }
        #endregion

        #region 时间统一显示,过滤掉1900
        /// <summary>
        /// 时间统一显示,过滤掉1900
        /// </summary>
        /// <param name="strdate"></param>
        /// <returns></returns>
        public static string ShowDataTimeyyyyMMddHHmmssHide1900(object obj)
        {
            string s;
            if (obj == null)
            {
                return "";
            }

            try
            {
                DateTime sj = Convert.ToDateTime(obj);
                if (sj.Year == 1900)
                {
                    s = "";
                }
                else
                {
                    s = sj.ToString();
                }

            }
            catch
            {
                s = "";
            }

            return s;
        }
        #endregion

        #region 字符串编码
        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string HtmlEncode(string inputData)
        {
            return HttpUtility.HtmlEncode(inputData);
        }
        #endregion

        #region 字符串解码
        /// <summary>
        /// 字符串解码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string HtmlDecode(string inputData)
        {
            return HttpUtility.HtmlDecode(inputData);
        }
        #endregion


        #region 字符串编码为Base64
        /// <summary>
        /// 字符串编码为Base64
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>Base64编码的字符串</returns>
        public static string ToBase64(string str)
        {
            try
            {
                byte[] by = Encoding.Default.GetBytes(str);
                str = Convert.ToBase64String(by);
            }
            catch (Exception)
            {
                // 忽略错误
            }
            return str;
        }
        #endregion

        #region 将Base64字符串解码为普通字符串
        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="dstr">要解码的字符串</param>
        public static string Base64Decode(string dstr)
        {
            byte[] barray;
            barray = Convert.FromBase64String(dstr);
            return Encoding.Default.GetString(barray);
        }
        #endregion

        #region 校验物理路径,确保有/结束
        /// <summary>
        /// 校验物理路径,确保有/结束
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public static string CheckAppPath(string strpath)
        {
            if (!strpath.EndsWith("\\") && !strpath.EndsWith("/"))
            {
                strpath += "\\";
            }
            return strpath;
        }
        #endregion

        #region 汉字<-->字母互相转换
        /// <summary>
        /// 汉字转换成字母
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ChineseToLetter(string str)
        {
            string strOutput = "";
            foreach (char chr in str)
            {
                byte[] chrArray = System.Text.Encoding.Default.GetBytes(chr.ToString());
                for (int i = 0; i < chrArray.Length; i++)
                {
                    string strA = Convert.ToString(chrArray[i], 16);
                    foreach (char chrB in strA)
                    {
                        byte[] chrBArray = System.Text.Encoding.Default.GetBytes(chrB.ToString());
                        int newValue = ((int)chrBArray[0]);
                        if (chrB >= 'a')//是字母
                        {
                            newValue += 10;
                        }
                        else
                        {
                            //是数字 
                            //0->a,1->b,2->c ,3->d 
                            //即chr(ord($char)-ord('0')+ord('a'));//=chr(ord($char)-48+97) 
                            //=chr(ord($char+49) 
                            newValue += 49;
                        }
                        byte[] newChrBArray = new byte[] { (byte)newValue };
                        strOutput += System.Text.Encoding.Default.GetChars(newChrBArray)[0].ToString();
                    }
                }
            }
            return strOutput;
        }
        /// <summary>
        /// 字母转换成汉字
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static string LetterToChinese(string str)
        {
            if (str.Length % 2 == 1) str = str.Substring(0, str.Length - 1);
            string strOutput = "";
            for (int i = 0; i < str.Length; i += 2)
            {
                string temp = "";
                for (int j = 0; j < 2; j++)
                {
                    char chr = str[i + j];
                    byte[] chrArray = System.Text.Encoding.Default.GetBytes(chr.ToString());
                    int newValue = ((int)chrArray[0]);
                    if (newValue >= 107)
                    {
                        newValue -= 10;
                    }
                    else
                    {
                        newValue -= 49;
                    }
                    byte[] newChrByte = new byte[] { (byte)newValue };
                    temp += System.Text.Encoding.Default.GetChars(newChrByte)[0].ToString();
                }
                strOutput += temp;
            }
            return HexstrToString(strOutput);
        }

        /// <summary>
        /// 十六进制转换成汉字
        /// </summary>
        /// <param name="hexstring"></param>
        /// <returns></returns>
        public static string HexstrToString(string hexstring)
        {
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
            //if (hexstring.Length % 2 == 1) hexstring = hexstring.Substring(0, hexstring.Length - 1);
            byte[] b = new byte[hexstring.Length / 2];
            for (int i = 0; i < hexstring.Length / 2; i++)
            {
                try
                {
                    b[i] = Convert.ToByte("0x" + hexstring.Substring(i * 2, 2), 16);
                }
                catch { }
            }
            return encode.GetString(b);
        }
        #endregion

        #region List转DataTable
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            if (entitys == null || entitys.Count == 0)
            {
                return null;
                //throw new Exception("需要转换的集合为空");
            }

            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            if (entityProperties == null || entityProperties.Length == 0)
            {
                return null;
            }

            string _key = string.Format("ListToDataTable${0}", entityType.FullName);

            DataTable dt0 = (DataTable)HttpRuntime.Cache[_key];
            if (dt0 == null)
            {
                dt0 = new DataTable();//Create Structure
                //实际操作中，应将生成的DataTable结构Cache起来，此处略
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);//构造列并设定该列的类型
                    dt0.Columns.Add(entityProperties[i].Name);
                }
                if (dt0 != null && dt0.Columns.Count > 0)
                {
                    HttpRuntime.Cache.Add(_key, dt0, null, DateTime.Now.AddDays(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
                }
            }
            DataTable dt = dt0.Clone();

            foreach (object entity in entitys)
            {
                if (entity.GetType() != entityType)
                {
                    continue;//如果类型不一样，则跳过
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }


        }

        public static DataTable ListToDataTable(List<string> entitys)
        {
            if (entitys == null || entitys.Count == 0) return null;
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            foreach (string title in entitys)
            {
                DataRow dr = dt.NewRow();
                dr["Title"] = title;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region MD5方法
        /// <summary>
        /// MD5方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(str);
            byte[] result = md5.ComputeHash(data);

            string strresult = "";
            for (int i = 0; i < result.Length; i++)
            {
                strresult += result[i].ToString("x").PadLeft(2, '0');
            }

            return strresult;
        }
        #endregion

        #region 字符转ASCII码
        /// <summary>
        /// 字符转ASCII码
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static int Asc(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("Character is not valid.");
            }

        }
        #endregion

        #region ASCII码转字符
        /// <summary>
        /// ASCII码转字符
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        public static string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }
        #endregion

        #region base62转换
        private static String base62String = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static String ToBase62(String mid)
        {
            long int_mid = Int64.Parse(mid);
            String result = "";
            do
            {
                long a = int_mid % 62;
                result = base62String[(int)a] + result;
                int_mid = (int_mid - a) / 62;
            } while (int_mid > 0);
            return result.PadLeft(4, '0');
        }

        public static String getSinaMid(String mid)
        {
            long int_mid = Int64.Parse(mid);
            String result = "";
            for (int i = mid.Length - 7; i > -7; i -= 7)
            {
                int offset1 = (i < 0) ? 0 : i;
                int offset2 = i + 7;
                String num = ToBase62(mid.Substring(offset1, offset2 - offset1));
                result = num + result;
            }
            return result;
        }
        #endregion
    }
}
