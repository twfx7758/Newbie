using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    /// <summary>
    /// 内部使用的Log接口
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 写入异常信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="exceptioin">Exception.</param>
        void WriteException(string title, Exception exceptioin);

        /// <summary>
        /// 写入提示信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        void WriteInfo(string title, string message);
    }
}
