using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newbie.RabbitMQ;
using RabbitMessage;

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

            IEventMessage<MessageEntity> message = new EventMessage<MessageEntity>() {
                IsOperationOk = false,
                MessageEntity = new MessageEntity() { MessageID=1, MessageContent="测试消息队列" },
                deliveryMode = 2
            };

            try
            {
                RabbitMQSender<MessageEntity> sender = new RabbitMQSender<MessageEntity>(context2, message);
                sender.TriggerEventMessage();

                Console.WriteLine(string.Format("发送信息:{0}", message.MessageEntity));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("发送信息失败:{0}", e.Message));
            }
        }
    }
}
