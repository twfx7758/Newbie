using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Util
{
    #region 邮件
    /// <summary>
    /// 发送邮件的类
    /// </summary>
    public static class Email
    {
        private static string smtpServer;
        private static string password;
        private static string from;
        private static string to;

        /// <summary>
        /// SMTP服务器的地址
        /// </summary>
        public static string SmtpServer
        {
            get
            {
                if (string.IsNullOrEmpty(smtpServer))
                {
                    smtpServer = ConfigurationUtil.GetAppSettingValue("Mail.SmtpServer");
                }
                return smtpServer;
            }
            set
            {
                smtpServer = value;
            }
        }

        /// <summary>
        /// SMTP服务器的密码
        /// </summary>
        public static string Password
        {
            get
            {
                if (string.IsNullOrEmpty(password))
                {
                    password = ConfigurationUtil.GetAppSettingValue("Mail.Password");
                }
                return password;
            }
            set
            {
                password = value;
            }
        }

        /// <summary>
        /// 发送业务邮件的地址
        /// </summary>
        public static string From
        {
            get
            {
                if (string.IsNullOrEmpty(from))
                {
                    from = ConfigurationUtil.GetAppSettingValue("Mail.From");
                }
                return from;
            }
            set
            {
                from = value;
            }
        }

        /// <summary>
        /// 接收email地址
        /// </summary>
        public static string To
        {
            get
            {
                if (string.IsNullOrEmpty(to))
                {
                    to = ConfigurationUtil.GetAppSettingValue("Mail.To");
                }
                return to;
            }
            set
            {
                to = value;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Title">邮件标题</param>
        /// <param name="Msg">邮件内容</param>
        public static void Send(string Title, string Msg)
        {
            MailHelper.SendEmail(SmtpServer, From, Password, To, Title, Msg, true);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="msg">邮件内容</param>
        /// <param name="to">接收人</param>
        public static void Send(string title, string msg, string to)
        {
            MailHelper.SendEmail(SmtpServer, From, Password, to, title, msg, true);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="msg">邮件内容</param>
        /// <param name="to">接收人</param>
        /// <param name="files">附件</param>
        public static void Send(string title, string msg, string to, string[] files)
        {
           MailHelper.SendEmail(SmtpServer, From, Password, to, title, msg, true, files);
        }   
    }
    #endregion
}
