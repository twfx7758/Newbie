using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using Newtonsoft.Json;

namespace Newbie.Util
{
    /// <summary>
    /// 微信分享，需要配置,否则使用默认配置
    /// <add key="WeiXin_JsApi_TicketUrl" value="http://weixin.api.huimaiche.com/jsapi/ticket"/>
    /// <add key="WeiXin_AppID" value="wx885e0e9aeeb88e1a"/>
    /// </summary>
    public class WeinXinShareProvider
    {
        private static readonly string m_param = "<script type=\"text/javascript\">\r\n" +
                                                     "var global_share_title = \"{0}\";\r\n" +
                                                     "var global_share_desc = \"{1}\";\r\n" +
                                                     "var global_share_imgUrl = \"{2}\";\r\n" +
                                                     "var global_share_link = \"{3}\";\r\n" +
                                                 "</script>\r\n";
        private static readonly string m_res = "<script type=\"text/javascript\" src=\"http://res.wx.qq.com/open/js/jweixin-1.0.0.js\"></script>\r\n" +
                                               "<script type=\"text/javascript\" src=\"http://img.huimaiche.cn/uimg/www/v20150526/js/libs/weixinshareinit.min.js\"></script>\r\n";

        private static readonly string m_loader = "<script type=\"text/javascript\">if(typeof WeiXinShare!=='undefined') \r\n" +
                                                  "{{ WeiXinShare.init('{0}', '{1}', '{2}', '{3}'); " +
                                                  "}}\r\n" +
                                                  "</script>";
        private static WeinXinShareProvider _instance = new WeinXinShareProvider();

        public static WeinXinShareProvider Instance
        {
            get
            {
                return _instance;
            }

        }
        private WeinXinShareProvider()
        {

        }
        /// <summary>
        /// 分享到微信
        /// </summary>
        /// <param name="share_title">标题</param>
        /// <param name="share_desc">描述</param>
        /// <param name="share_url">链接</param>
        /// <param name="share_img">图片</param>
        /// <returns></returns>
        public string ShareToMicroMessenger(string share_title, string share_desc, string share_img)
        {
            StringBuilder sb = new StringBuilder();
            WeiXinShare shareObj = WeinXinShareProvider.GetSignature("");
            if (shareObj != null)
            {
                sb.Append(m_res);
                HttpRequest request = HttpContext.Current.Request;
                string url = "http://" + request.Url.Host + request.RawUrl;
                sb.Append(string.Format(m_param, share_title, share_desc, share_img, url));
               
                sb.Append(string.Format(m_loader, shareObj.appId, shareObj.timestamp, shareObj.nonceStr, shareObj.signature, url));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 分享到微信
        /// </summary>
        /// <param name="share_title">标题</param>
        /// <param name="share_desc">描述</param>
        /// <param name="share_url">链接</param>
        /// <param name="share_img">图片</param>
        /// <returns></returns>
        public string ShareToMicroMessenger(string share_title, string share_desc, string share_img,string url)
        {
            StringBuilder sb = new StringBuilder();
            WeiXinShare shareObj = WeinXinShareProvider.GetSignature(url);
            if (shareObj != null)
            {
                sb.Append(m_res);
                HttpRequest request = HttpContext.Current.Request;
                if (string.IsNullOrEmpty(url))
                {
                    url = "http://" + request.Url.Host + request.RawUrl;
                }
                sb.Append(string.Format(m_param, share_title, share_desc, share_img, url));

                sb.Append(string.Format(m_loader, shareObj.appId, shareObj.timestamp, shareObj.nonceStr, shareObj.signature, url));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取JsApi签名
        /// </summary>
        private static WeiXinShare GetSignature(string url)
        {
            string json = string.Empty;
            string nonceStr = "1qaz2wsx3edc";
            string ticket = string.Empty;
            HttpRequest request = HttpContext.Current.Request;
            if (string.IsNullOrEmpty(url))
            {
                url = "http://" + request.Url.Host + request.RawUrl;
            }
            string cacheKey = string.Format("weixinshare_Key_{0}_{1}_{2}", nonceStr, ticket, url.GetHashCode());
            var obj = (WeiXinShare)HttpRuntime.Cache.Get(cacheKey);
            if (obj == null)
            {
                try
                {
                    WebClient client = new WebClient();
                    string ret = client.DownloadString(AppSettingHelper.GetString("WeiXin_JsApi_TicketUrl", "http://weixin.api.huimaiche.com/jsapi/ticket"));
                    //仿真
                    //string ret = client.DownloadString(AppSettingHelper.GetString("WeiXin_JsApi_TicketUrl", "http://weixin.api.maiche.biz/jsapi/ticket"));
                    var ticktResult = JsonConvert.DeserializeObject<TicketResult>(ret);
                    if (ticktResult.code == 0)
                    {
                        ticket = ticktResult.ticket;
                    }
                }
                catch (Exception ex)
                {
                    json = JsonConvert.SerializeObject(new { code = -1, errMsg = ex.Message });
                }

                long timestamp = GetMilliTimeStamp(DateTime.Now);
                obj = CreateSignature(nonceStr, ticket, timestamp, url);
                HttpRuntime.Cache.Insert(cacheKey, obj, null, DateTime.Now.AddSeconds(7000), TimeSpan.Zero, CacheItemPriority.High, null);
            }

            return obj;
        }

        private static WeiXinShare CreateSignature(string nonceStr, string ticket, long timestamp, string url)
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("noncestr", nonceStr);
            dic.Add("jsapi_ticket", ticket);
            dic.Add("timestamp", timestamp.ToString());
            dic.Add("url", url);
            StringBuilder sb = new StringBuilder();
            foreach (var item in dic)
            {
                sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            string enStr = SHA1KeyEncry(sb.ToString());
            var obj = new WeiXinShare
            {
                //仿真
                //appId = AppSettingHelper.GetString("WeiXin_AppID", "wxb97a3dff1353ef11"),
                appId = AppSettingHelper.GetString("WeiXin_AppID", "wx885e0e9aeeb88e1a"),
                timestamp = timestamp,
                nonceStr = nonceStr,
                signature = enStr
            };
            return obj;
        }

        private static string SHA1KeyEncry(string str)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        private static long GetMilliTimeStamp(DateTime dateTime)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dateTime.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long)ts.TotalMilliseconds;
        }
    }

    public class WeiXinShare
    {
        public string appId { get; set; }
        public long timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }

    public class TicketResult
    {
        public int code { get; set; }

        public string errmsg { get; set; }

        public string ticket { get; set; }
    }
}
