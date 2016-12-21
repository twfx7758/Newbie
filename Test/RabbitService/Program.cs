using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using Newbie.RabbitMQ;

namespace RabbitService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================监听程序开始初始化==================");

            RabbitMQTest();

            Console.WriteLine("==================监听程序已启动，监听中==================");

            Console.ReadKey();
        }

        //测试RabbitMQ的消费者
        static void RabbitMQTest()
        {
            LogLocation.Log = new LogInfo();
            RabbitMQClientContext context = new RabbitMQClientContext()
            {
                ListenQueueName = "LogQueue"
            };

            RabbitMQConsumer<string> consumer = new RabbitMQConsumer<string>(context, new EventMessage<string>()) {
                ActionMessage = b => {
                    Console.WriteLine(b.MessageEntity);
                    b.IsOperationOk = true;
                } 
            };

            consumer.OnListening();
            //多个消费者，共用一个连接，使用不同的Channel
            //consumer.OnListening();
        }
    }
}
