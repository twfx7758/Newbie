using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;

namespace Newbie.Util
{
    public class FileHelper
    {

        /// <summary>
        /// 以字符流的形式下载文件
        /// </summary>
        /// <param name="URL">web服务器上的指定虚拟路径相对应的物理文件路径</param>
        /// <param name="fileName">客户端保存的文件名</param>
        public static void DownloadFile(string fileURL, string fileName)
        {
            HttpResponse Response = HttpContext.Current.Response;
            if (fileURL != "")
            {
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(fileURL);
                HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
                Stream readStream = myWebResponse.GetResponseStream();
                var picBytes = ReadFully(readStream);
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开        
                Response.AddHeader("Content-Disposition", "attachment;  filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                Response.BinaryWrite(picBytes);
                Response.Flush();
                Response.End();
            }
        }

        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[128];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// 下载网络的图片到本地文件
        /// </summary>
        /// <param name="fileURL">图片url</param>
        /// <param name="fileDir">本地文件夹</param>
        /// <param name="fileName">文件名</param>
        public void DownLoadFileToLocal(string fileURL, string fileDir, string fileName)
        {
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            var fullPath = Path.Combine(fileDir, fileName);
            using (WebClient mywebclient = new WebClient())
            {
                mywebclient.DownloadFile(fileURL, fullPath);
            }
        }
    }
}
