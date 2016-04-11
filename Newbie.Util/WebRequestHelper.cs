using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.IO.Compression;
using System.Web;

namespace Newbie.Util
{
    public class WebRequestHelper
    {
        public static string CreateStrHttpResponse(string url, IDictionary<string, string> parameters, Encoding requestEncoding)
        {
            using (var response = CreateHttpResponse(url, parameters, requestEncoding))
            {
                string html = string.Empty;

                if (response != null)
                {
                    // 得到返回的数据流
                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        // 如果有压缩,则进行解压
                        if (response.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            using (GZipStream receiveStreamG = new GZipStream(receiveStream, CompressionMode.Decompress))
                            {
                                // 得到返回的字符串  
                                html = new StreamReader(receiveStreamG).ReadToEnd();
                            }
                        }
                        else
                        {
                            // 得到返回的字符串  
                            html = new StreamReader(receiveStream).ReadToEnd();
                        }
                    }
                }
                return html;
            }
        }

        public static HttpWebResponse CreateHttpResponse(string url, IDictionary<string, string> parameters, Encoding requestEncoding)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }

            HttpWebRequest request = null;

            //如果是发送HTTPS请求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //如果需要POST数据
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }


        public static string GetPageHTML(string url, string referer = "")
        {
            return GetPageHTML(url, Encoding.UTF8, referer);
        }

        /// <summary>
        /// 获取指定字符编码的页面HTML代码
        /// </summary>
        /// <param name="url">页面地址</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>指定URL的Html代码</returns>
        public static string GetPageHTML(string url, Encoding encoding, string referer = "")
        {
            Stream stream = null;
            StreamReader sr = null;

            try
            {
                //Test 解决请求超时问题--李倩
                System.GC.Collect();
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Accept = "text/html,application/xhtml+xml,application/xml;";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.111 Safari/537.36";
                if (!string.IsNullOrEmpty(referer))
                {
                    req.Referer = referer;
                }
                req.Timeout = 1000 * 500;//500秒
                req.ReadWriteTimeout = 1000 * 500;
                var response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();
                sr = new StreamReader(stream, encoding);
                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }

                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }


        /// <summary>
        /// 获取指定字符编码的页面HTML代码
        /// </summary>
        /// <param name="url">页面地址</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="responseUrl">302跳转后的url</param>
        /// <param name="referer"></param>
        /// <returns></returns>
        public static string GetPageHTML(string url, Encoding encoding, out string responseUrl, string referer = "")
        {
            Stream stream = null;
            StreamReader sr = null;
            responseUrl = url;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Accept = "text/html,application/xhtml+xml,application/xml;";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                if (!string.IsNullOrEmpty(referer))
                {
                    req.Referer = referer;
                }
                var response = (HttpWebResponse)req.GetResponse();
                responseUrl = response.ResponseUri == null ? url : response.ResponseUri.AbsoluteUri;
                stream = response.GetResponseStream();
                sr = new StreamReader(stream, encoding);
                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }

                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// 调取接口传一个参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sourceOrderId"></param>
        /// <param name="requestEncoding"></param>
        /// <returns></returns>
        public static string CreateStrHttpResponse(string url, string sourceOrderId, Encoding requestEncoding)
        {
            using (var response = CreateHttpResponse(url, sourceOrderId, requestEncoding))
            {
                string html = string.Empty;

                if (response != null)
                {
                    // 得到返回的数据流
                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        // 如果有压缩,则进行解压
                        if (response.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            using (GZipStream receiveStreamG = new GZipStream(receiveStream, CompressionMode.Decompress))
                            {
                                // 得到返回的字符串  
                                html = new StreamReader(receiveStreamG).ReadToEnd();
                            }
                        }
                        else
                        {
                            // 得到返回的字符串  
                            html = new StreamReader(receiveStream).ReadToEnd();
                        }
                    }
                }
                return html;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sourceOrderId"></param>
        /// <param name="requestEncoding"></param>
        /// <returns></returns>
        public static HttpWebResponse CreateHttpResponse(string url, string sourceOrderId, Encoding requestEncoding)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }

            HttpWebRequest request = null;

            //如果是发送HTTPS请求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //如果需要POST数据
            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("{0}", sourceOrderId);
            byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            return request.GetResponse() as HttpWebResponse;
        }





        /// <summary>
        /// 向指定的URL地址发起一个GET请求
        /// 默认超时时间为60s
        /// </summary>
        /// <param name="url">要请求的URL地址</param>
        /// <returns>服务器的返回结果</returns>
        public static string SendGetRequest(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Timeout = 60000;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return ReadResponse(response);
            }
        }

        /// <summary>
        /// 向指定的URL地址发起一个POST请求，同时可以上传一些数据项。
        /// 默认超时时间为60s
        /// </summary>
        /// <param name="url">要请求的URL地址</param>
        /// <param name="keyvalues">要上传的数据项</param>
        /// <param name="encoding">发送，接收的字符编码方式</param>
        /// <returns>服务器的返回结果</returns>
        public static string SendPostRequest(string url, IDictionary<string, string> keyvalues, Encoding encoding)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            string postData = null;
            // 将数据项转变成 name1=value1&name2=value2 的形式
            if (keyvalues != null && keyvalues.Count > 0)
            {
                postData = string.Join("&",
                    (from kvp in keyvalues
                     let item = kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value)
                     select item
                        ).ToArray()
                    );
            }

            if (encoding == null)
                encoding = Encoding.UTF8;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=" + encoding.WebName;
            request.Timeout = 60000;

            if (postData != null)
            {
                byte[] buffer = encoding.GetBytes(postData);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                return ReadResponse(response);
            }
        }

        /// <summary>
        /// 向指定的URL地址发起一个POST请求，同时可以上传一些数据项以及上传文件。
        /// </summary>
        /// <param name="url">要请求的URL地址</param>
        /// <param name="keyvalues">要上传的数据项</param>
        /// <param name="fileList">要上传的文件列表</param>
        /// <param name="encoding">发送数据项，接收的字符编码方式</param>
        /// <returns>服务器的返回结果</returns>
        public static string SendPostRequest(string url, IDictionary<string, string> keyvalues, Dictionary<string, string> fileList, Encoding encoding)
        {
            if (fileList == null)
                return SendPostRequest(url, keyvalues, encoding);

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            if (encoding == null)
                encoding = Encoding.UTF8;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST"; // 要上传文件，一定要是POST方法

            // 数据块的分隔标记，用于设置请求头，注意：这个地方最好不要使用汉字。
            string boundary = "---------------------------" + Guid.NewGuid().ToString("N");
            // 数据块的分隔标记，用于写入请求体。
            //   注意：前面多了一段： "--" ，而且它们将独占一行。
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            // 设置请求头。指示是一个上传表单，以及各数据块的分隔标记。
            request.ContentType = "multipart/form-data; boundary=" + boundary;


            // 先得到请求流，准备写入数据。
            Stream stream = request.GetRequestStream();


            if (keyvalues != null && keyvalues.Count > 0)
            {
                // 写入非文件的keyvalues部分
                foreach (KeyValuePair<string, string> kvp in keyvalues)
                {
                    // 写入数据块的分隔标记
                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                    // 写入数据项描述，这里的Value部分可以不用URL编码
                    string str = string.Format(
                        "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}",
                        kvp.Key, kvp.Value);

                    byte[] data = encoding.GetBytes(str);
                    stream.Write(data, 0, data.Length);
                }
            }


            // 写入要上传的文件
            foreach (KeyValuePair<string, string> kvp in fileList)
            {
                // 写入数据块的分隔标记
                stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                // 写入文件描述，这里设置一个通用的类型描述：application/octet-stream，具体的描述在注册表里有。
                string description = string.Format(
                    "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                    "Content-Type: application/octet-stream\r\n\r\n",
                    kvp.Key, Path.GetFileName(kvp.Value));

                // 注意：这里如果不使用UTF-8，对于汉字会有乱码。
                byte[] header = Encoding.UTF8.GetBytes(description);
                stream.Write(header, 0, header.Length);

                // 写入文件内容
                byte[] body = File.ReadAllBytes(kvp.Value);
                stream.Write(body, 0, body.Length);
            }


            // 写入结束标记
            boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            stream.Write(boundaryBytes, 0, boundaryBytes.Length);

            stream.Close();

            // 开始发起请求，并获取服务器返回的结果。
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                return ReadResponse(response);
            }
        }

        /// <summary>
        /// 读取响应内容。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string ReadResponse(HttpWebResponse response)
        {
            if (response == null) return string.Empty;
            Stream strem = null;
            if (response.Headers["Content-Encoding"] == "gzip")
                strem = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            else
                strem = response.GetResponseStream();

            if (strem == null) return string.Empty;
            using (StreamReader reader = new StreamReader(strem))
            {
                return reader.ReadToEnd();
            }
        }




    }

}
