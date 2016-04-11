using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    public class LogInfo : ILog
    {
        public void WriteException(string title, Exception exceptioin)
        {
            //throw new NotImplementedException();
        }

        public void WriteInfo(string title, string message)
        {
            //throw new NotImplementedException();
        }
    }
}
