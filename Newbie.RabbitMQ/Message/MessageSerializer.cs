using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    public class MessageSerializer
    {
        public byte[] SerializerBytes<T>(T message)
            where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, message);
                return ms.GetBuffer();
            }
        }

        public T BytesDeseriallizer<T>(byte[] bMessage)
            where T : class
        {
            using (MemoryStream ms = new MemoryStream(bMessage))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
