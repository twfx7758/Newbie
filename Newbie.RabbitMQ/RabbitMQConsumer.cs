using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RabbitMQClient = RabbitMQ.Client;

namespace Newbie.RabbitMQ
{
    public class RabbitMQConsumer<T>
    {
        public RabbitMQClientContext Context { get; private set; }

        public IEventMessage<T> Message { get; private set; }

        public Action<IEventMessage<T>> ActionMessage = null;

        private RabbitMQConsumer() { }

        public RabbitMQConsumer(RabbitMQClientContext context, IEventMessage<T> message)
        {
            this.Context = context;
            this.Message = message;
        }

        public void OnListening()
        {
            Task.Run(() => ListenInit());
        }

        private void ListenInit()
        {
            try
            {
                //获取连接
                lock(this)
                {
                    Context.ListenConnection = RabbitMQClientFactory.CreateConnectionForSumer();
                    //获取通道
                    Context.ListenChannel = RabbitMQClientFactory.CreateModel(Context.ListenConnection);


                    //创建事件驱动的消费者模型
                    //QueueingBasicConsumer这个是队列的消费者
                    var consumer = new EventingBasicConsumer(Context.ListenChannel);

                    consumer.Received += Consumer_Received;

                    /*一次只获取一条消息进行消费
                    公平分发：
                    你可能会注意到，消息的分发可能并没有如我们想要的那样公平分配。比如，对于两个工作者。
                    当奇数个消息的任务比较重，但是偶数个消息任务比较轻时，奇数个工作者始终处理忙碌状态，
                    而偶数个工作者始终处理空闲状态。但是RabbitMQ并不知道这些，他仍然会平均依次的分发消息。

                    为了改变这一状态，我们可以使用basicQos方法，设置perfetchCount=1 。这样就告诉RabbitMQ 
                    不要在同一时间给一个工作者发送多于1个的消息，或者换句话说。
                    在一个工作者还在处理消息，并且没有响应消息之前，不要给他分发新的消息。
                    相反，将这条新的消息发送给下一个不那么忙碌的工作者。
                    */
                    Context.ListenChannel.BasicQos(0, 1, false);
                    Context.ListenChannel.BasicConsume(Context.ListenQueueName, false, consumer);
                }
            }
            catch (Exception ex)
            {
                if (LogLocation.Log != null)
                {
                    LogLocation.Log.WriteInfo("RabbitMQClient", "ListenInit()方法报错：" + ex.Message);
                }
            }
        }

        private void Consumer_Received(RabbitMQClient.IBasicConsumer sender, BasicDeliverEventArgs args)
        {
            try
            {
                var result = Message.BuildEventMessageResult(args.Body);

                if (ActionMessage != null)
                    ActionMessage(result);//触发外部监听事件，处理此消息

                if (!result.IsOperationOk)
                {
                    //未能消费此消息，重新放入队列头
                    Context.ListenChannel.BasicReject(args.DeliveryTag, true);
                }
                else if (!Context.ListenChannel.IsClosed)
                {
                    //如果通道还未关闭，给RabbitMQ发送一条确认消息(ACK)，
                    //告诉它此消息已成功处理，可以从队列中删除了
                    Context.ListenChannel.BasicAck(args.DeliveryTag, false);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
