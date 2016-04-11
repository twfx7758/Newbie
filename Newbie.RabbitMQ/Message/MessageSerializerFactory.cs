using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    public class MessageSerializerFactory
    {
        private static MessageSerializer _serializer = null;

        public static MessageSerializer CreateMessageSerializerInstance()
        {
            if (_serializer == null)
                _serializer = new MessageSerializer();

            return _serializer;
        }
    }
}
