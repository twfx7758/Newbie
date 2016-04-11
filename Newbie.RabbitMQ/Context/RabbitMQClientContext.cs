using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    public class RabbitMQClientContext
    {
        /// <summary>
        /// 用于发送消息的Connection
        /// </summary>
        public IConnection SendConnection { get; internal set; }

        /// <summary>
        /// 用于发送消息的Channel
        /// </summary>
        public IModel SendChannel { get; internal set; }

        /// <summary>
        /// 用于发送的Queue名称
        /// </summary>
        public string SendQueueName { get; set; }

        /// <summary>
        /// 用于发送的Exchange名称
        /// </summary>
        public string SendExchange { get; set; }

        /// <summary>
        /// 用于监听消息的Connection
        /// </summary>
        public IConnection ListenConnection { get; internal set; }

        /// <summary>
        /// 用于监听消息的Channel
        /// </summary>
        public IModel ListenChannel { get; internal set; }

        /// <summary>
        /// 用于监听的Queue名称
        /// </summary>
        public string ListenQueueName { get; set; }
    }
}
