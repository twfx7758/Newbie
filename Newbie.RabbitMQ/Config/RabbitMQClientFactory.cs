using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    public static class RabbitMQClientFactory
    {
        private static IConnection _conn = null;
        /// <summary>
        /// 消费者公用一个连接，然后使用不同的Channel
        /// </summary>
        /// <returns></returns>
        public static IConnection CreateConnectionForSumer()
        {
            if (_conn != null)
                return _conn;

            IConnection tempConn = CreateConnection();
            Interlocked.CompareExchange(ref _conn, tempConn, null);

            //添加连接断开日志
            _conn.ConnectionShutdown += (s, e) =>
            {
                if (LogLocation.Log != null)
                {
                    LogLocation.Log.WriteInfo("RabbitMQClient", "connection shutdown" + e.ReplyText);
                }
            };

            return _conn;
        }

        private static IConnection CreateConnection()
        {
            var mqConfigCom = MqConfigComFactory.CreateConfigDomInstance(); //获取MQ的配置
            const ushort heartbeat = 60;
            var factory = new ConnectionFactory()
            {
                HostName = mqConfigCom.MqHost,
                UserName = mqConfigCom.MqUserName,
                Password = mqConfigCom.MqPassword,
                //心跳超时时间，如果是单节点，不设置这个值是没有问题的
                //但如果连接的是类似HAProxy虚拟节点的时候就会出现TCP被断开的可能性
                RequestedHeartbeat = heartbeat,
                AutomaticRecoveryEnabled = true //自动重连
            };

            return factory.CreateConnection();//创建连接对你
        }

        public static IConnection CreateConnectionForSend()
        {
            var mqConfigCom = MqConfigComFactory.CreateConfigDomInstance(); //获取MQ的配置
            const ushort heartbeat = 60;
            var factory = new ConnectionFactory()
            {
                HostName = mqConfigCom.MqHost,
                UserName = mqConfigCom.MqUserName,
                Password = mqConfigCom.MqPassword,
                //心跳超时时间，如果是单节点，不设置这个值是没有问题的
                //但如果连接的是类似HAProxy虚拟节点的时候就会出现TCP被断开的可能性
                RequestedHeartbeat = heartbeat
            };

            return factory.CreateConnection();//创建连接对你
        }


        public static IModel CreateModel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
