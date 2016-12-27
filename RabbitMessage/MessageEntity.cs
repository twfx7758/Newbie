using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMessage
{
    [Serializable]
    public class MessageEntity
    {
        public int MessageID { get; set; }
        public string MessageContent { get; set; }
    }
}
