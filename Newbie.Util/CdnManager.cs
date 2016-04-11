using System;
using System.Net;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

namespace Newbie.Util
{
    public class CdnManager
    {
        private static string CdnUserName = ConfigurationManager.AppSettings["CdnUserName"];
        private static string CdnPassword = ConfigurationManager.AppSettings["CdnPassword"];
        private static string CdnPurgeServiceUrl = ConfigurationManager.AppSettings["CdnPurgeServiceUrl"];

        /// <summary>
        ///  刷新制定url的cdn缓存
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static bool Refresh(string url)
        {
            url = HttpUtility.UrlEncode(url);
            url = CdnPurgeServiceUrl + "?" + "user=" + CdnUserName + "&password=" + CdnPassword + "&urls=" + url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/4.0";
            request.Method = "GET";
            request.ContentType = "text/html";
            request.AllowAutoRedirect = true;
            request.Timeout = 120 * 1000;
            //单位为毫秒

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                request.Abort();
                return false;
            }

            if (response.GetResponseHeader("whatsup").IndexOf("succeed") != -1)
            {
                return true;
            }

            return false;
        }
    }
}
