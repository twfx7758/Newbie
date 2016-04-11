
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Newbie.Util
{

    public class Util
    {
        /// <summary>
        /// 按照位数生成字母和数字的混合字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RadomString(int length)
        {
            StringBuilder newRandom = new StringBuilder();
            Random rd = new Random();
            string str = @"0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    newRandom.Append(str[rd.Next(11, 62)].ToString().ToLower());
                }
                else
                {
                    newRandom.Append(str[rd.Next(0, 11)]);
                }
            }
            return newRandom.ToString();
        }
        /// <summary>
        ///  POST 数据到服务器
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="data">post参数</param>
        /// <returns></returns>
        public static string CreateHttpPostRequest(string url, string data)
        {
            string recievedata = "";
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    if (webResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // 获取接收到的流 
                        using (Stream stream = webResponse.GetResponseStream())
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                recievedata = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                recievedata = "发送短信错误：url:" + url + ";data=" + data + e.Message + e.StackTrace;
            }
            return recievedata;
        }


        /// <summary>
        ///  异步形式发短信
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="data">post参数</param>
        /// <returns></returns>
        public static string CreateHttpPostRequestAsync(string url, string data)
        {
            string recievedata = "{\"SmsVerificationResultEntities\":[],\"Result\":true,\"Message\":\"发送短信到栈堆成功!\",\"Id\":0}";
            try
            {

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                //modify by tiansx 2015.10.27改为异步请求短信接口，防止前端页面上长时间处于等待返回结果状态
                request.BeginGetResponse(new AsyncCallback(ReceivedResource), request);
            }
            catch (Exception e)
            {
                recievedata = "{\"SmsVerificationResultEntities\":[],\"Result\":true,\"Message\":\"短信接口调用异常!\",\"Id\":0}";

            }
            return recievedata;
        }

        private static void ReceivedResource(IAsyncResult ar)
        {
            string recievedata = "";
            HttpWebResponse res = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)ar.AsyncState;
                res = (HttpWebResponse)req.EndGetResponse(ar);
                if (res != null && res.StatusCode == HttpStatusCode.OK)
                {
                    // 获取接收到的流 
                    using (Stream stream = res.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            //{"SmsVerificationResultEntities":[],"Result":true,"Message":"发送短信到栈堆成功!","Id":188168013}
                            recievedata = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException we)
            { }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
        }


        /// <summary>
        /// 获取客户机的ＩＰ
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string ip = "";
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;
                System.Web.HttpServerUtility Server = System.Web.HttpContext.Current.Server;

                string addr = "";
                try
                {
                    /*穿过代理服务器取远程用户真实IP地址：*/
                    if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        addr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        addr = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    }
                }
                catch
                {
                    addr = Request.UserHostAddress;
                }

                if (addr == null)
                {
                    addr = "";
                }

                ip = addr.Split(',')[0];
            }
            return ip;
        }

        /// <summary>
        /// 计算Ip地址的整数值
        /// </summary>
        /// <param name="strIp">Ip地址</param>
        /// <returns>返回类型为ulong的长整数值</returns>
        public static long ComputeIpValue(string strIp)
        {
            string[] ipVals = strIp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            long rst = long.Parse(ipVals[3]);
            rst += long.Parse(ipVals[2]) * 256;
            rst += long.Parse(ipVals[1]) * 256 * 256;
            rst += long.Parse(ipVals[0]) * 256 * 256 * 256;
            return rst;
        }

        /// <summary>
        /// 判断是否是搜索引擎抓取
        /// </summary>
        /// <returns></returns>
        public static bool IsSpider()
        {
            bool result = false;
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            List<string> engineList = new List<string> { "googlebot", "baiduspider", "yahoo!", "sogou web spider", "sogou orion spider", "sogou-test-spider", "yodaobot", "msnbot", "360spider", "sosospider", " bingbot" };
            List<string> domainList = new List<string> { "google", "baidu", "yahoo", "sogou", "yodao", "msn", "360", "soso", "bing", "inktomisearch", "live" };
            try
            {
                if (!string.IsNullOrEmpty(userAgent))
                {
                    //判断当前UA是否在搜索引擎UA参考列表中
                    engineList.ForEach(tmp =>
                    {
                        if (userAgent.ToLower().Contains(tmp))
                        {
                            string ipAddress = GetClientIP();
                            if (!string.IsNullOrEmpty(ipAddress))
                            {
                                IPHostEntry host = Dns.GetHostEntry(ipAddress);
                                if (host != null)
                                {
                                    if (!string.IsNullOrEmpty(host.HostName))
                                    {
                                        //判断反向代理域名是否是搜索引擎对应的域名
                                        domainList.ForEach(domain =>
                                        {
                                            if (host.HostName.ToLower().Contains(domain))
                                            {
                                                result = true;
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + userAgent);
            }
            return result;
        }

        /// <summary>
        /// 调用新意核销接口
        /// </summary>
        /// <param name="StyleFlag">POST 或者 GET  （若不是post或者get，则返回请求类型异常）</param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateHttpRequestToCIG(string StyleFlag, string url, string data,out int flag)
        {
            flag = 0;  //是否成功调用接口  0：未调用成功，捕获到异常  1：调用成功
            string recievedata = "";
            if (StyleFlag.ToUpper() == "POST" || StyleFlag.ToUpper() == "GET")
            {
                try
                {
                    HttpWebRequest request;
                    if (StyleFlag.ToUpper() == "POST")
                    {
                        request = WebRequest.Create(url) as HttpWebRequest;
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        byte[] byteArray = Encoding.UTF8.GetBytes(data);
                        request.ContentLength = byteArray.Length;
                        Stream dataStream = request.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();
                    }
                    else
                    {
                        request = (HttpWebRequest)WebRequest.Create(url + "?" + data);
                        request.Method = "GET";
                        request.ContentType = "application/x-www-form-urlencoded";
                    }

                    using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                    {
                        if (webResponse.StatusCode == HttpStatusCode.OK)
                        {
                            // 获取接收到的流 
                            using (Stream stream = webResponse.GetResponseStream())
                            {
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    recievedata = sr.ReadToEnd();
                                }
                            }
                            flag = 1;
                        }
                    }
                }
                catch (Exception e)
                {
                    flag = 0;
                    recievedata = "调用新意核销接口异常：url:" + url + ";data=" + data + e.Message + e.StackTrace;
                }
            }
            else
            {
                flag = 0;
                recievedata = "请求类型异常";
            }
            return recievedata;

        }
        /// <summary>
        /// /获取服务器IP
        /// </summary>
        /// <returns></returns>
        public static string GetServerIP()
        {
            string hostName = System.Net.Dns.GetHostName();
            System.Net.IPAddress[] ips = System.Net.Dns.GetHostAddresses(hostName);

            string getIp = "127.0.0.1";
            foreach (var ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return getIp;
        }

    }
}
