using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    public interface IEventMessage<T>
    {
        /// <summary>
        /// 消息交换模式，1、短暂，2、持久
        /// </summary>
        byte deliveryMode { get; set; }
        /// <summary>
        /// 消息是否成功处理
        /// </summary>
        bool IsOperationOk { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        T MessageEntity { get; set; }
        /// <summary>
        /// 由二进制数据流转换成对应的消息实体
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        IEventMessage<T> BuildEventMessageResult(byte[] body);
    }
}
