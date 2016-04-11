using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Newbie.Util
{
    /// <summary>
    /// 封装了日志类
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// log4Net的引用成员
        /// </summary>
        private static ILog m_log4Net = LogManager.GetLogger(typeof(Logger));

        /// <summary>
        /// 通过此Property获得日志实例的引用
        /// </summary>
        public static ILog Log4Net
        {
            get { return m_log4Net; }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void WriteErrorLog(string title, string content)
        {
            if (WebConfigOperate.GetAppSetting("isWriteErrorLog") == "0")
            {
                return;
            }
            Log4Net.Error(string.Format("title:{0} content:{1}", title, content));
        }
        public static void WriteErrorLog(Exception ex)
        {
            if (WebConfigOperate.GetAppSetting("isWriteErrorLog") == "0")
            {
                return;
            }
            Log4Net.Error(ex);
        }


        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message)
        {
            Log4Net.Debug(message);
        }
        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            Log4Net.Info(message);
        }
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            Log4Net.Error(ex.Message, ex);
        }
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message, Exception ex)
        {
            Log4Net.Error(message, ex);
        }
        /// <summary>
        /// 记录致命错误信息
        /// </summary>
        /// <param name="ex"></param>
        public static void Fatal(Exception ex)
        {
            Log4Net.Fatal(ex);
        }
    }
}
