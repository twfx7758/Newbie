using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newbie.RabbitMQ;

namespace RabbitClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMQTest();

            Console.ReadKey();
        }

        //测试RabbitMQ
        static void RabbitMQTest()
        {
            //持久化的Exchange、持久化的消息、持久化的队列
            RabbitMQClientContext context = new RabbitMQClientContext() { SendQueueName = "SendQueueName", SendExchange = "amq.fanout" };
            //持久化的Exchange、持久化的消息、非持久化的队列
            RabbitMQClientContext context2 = new RabbitMQClientContext() { SendQueueName = "SendQueueName", SendExchange = "amq.fanout" };

            IEventMessage<string> message = new EventMessage<string>() {
                IsOperationOk = false,
                MessageEntity = "测试客户端类库",
                deliveryMode = 2
            };

            RabbitMQSender<string> sender = new RabbitMQSender<string>(context2, message);
            sender.TriggerEventMessage();
        }
    }
}
