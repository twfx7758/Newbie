using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Util
{
    public class SendSMSHelper
    {
        /// <summary>
        /// 发送短信工具类(带日志输出)
        /// </summary>
        /// <param name="messageParam">messageParam</param>
        /// <param name="appid">appid</param>
        /// <param name="passkey">passkey</param>
        /// <param name="smsApiUrl">smsApiUrl</param>
        /// <param name="logTitle">日志标题</param>
        /// <param name="phone">发送手机号（多个用逗号分割,最多100个）</param>
        /// <param name="smsContent">短信内容</param>
        /// <returns></returns>
        public static Tuple<bool, string> SendSMS(string messageParam, string appid, string passkey,
            string smsApiUrl, string logTitle, string phone, string smsContent)
        {
            try
            {
                // 时间戳请保证同一个应用每一个时间戳全局唯一,否则可能重复的时间戳短信被屏蔽
                //      (如果应用因为并发量大导致的同一毫秒因并发产生的时间戳相同请在传递时间戳的同时在时间戳内容后面加上8位数字(必须)随机数,
                //      必须确保同1毫秒级8位数字随机数不相同,确保全局唯一)
                var seed = Guid.NewGuid().GetHashCode();
                Random rd = new Random(seed);
                string time = DateTime.Now.Ticks.ToString() + rd.Next(10000000,99999999).ToString();
                string passKey = "";//NoteEncryptHelper.GetNotePassKey(int.Parse(appid),new Guid(passkey),time);
                string data = string.Format(messageParam, phone, time, smsContent, passKey,appid);
                string res = Util.CreateHttpPostRequest(smsApiUrl,data);
                // res:成功格式 -- {result:'True',message:'发送短信到栈堆成功!',id:'23748947'}
                Logger.Log4Net.InfoFormat("日志标题：{0}，日志内容：res={1}-------data={2}-------url={3}", logTitle, res,data,smsApiUrl);
                if (res.StartsWith("{result:'True'"))
                {
                    //发送成功
                    return new Tuple<bool, string>(true, res);
                }
                else
                {
                    //发送失败
                    return new Tuple<bool, string>(false, res);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
