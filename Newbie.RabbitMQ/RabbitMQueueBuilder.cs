using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Newbie.RabbitMQ
{
    public class RabbitMQueueBuilder : IDisposable
    {
        private IConnection _conn = RabbitMQClientFactory.CreateConnectionForSend();
        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="durable">是否为持久化队列（true:RabbitMQ重启队列删除）</param>
        /// <param name="exclusive">是否为排它队列，如果一个队列被声明为排他队列，该队列仅对首次声明它的连接可见，并在连接断开时自动删除</param>
        /// <param name="autoDelete">自动删除，如果该队列没有任何订阅的消费者的话，该队列会被自动删除</param>
        /// <param name="args"></param>
        public IModel CreateQueue(string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> args = null)
        {
            var channel = _conn.CreateModel();
            channel.QueueDeclare(queueName, durable, exclusive, autoDelete, args);
            /*事务开启
            channel.TxSelect()
            channel.TxCommit();
            channel.TxRollback();
            */
            return channel;
        }

        public void Dispose()
        {
            _conn.Close();
        }
    }
}
