using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.AOP
{
    public class InvocationContext
    {
        public IMethodCallMessage Request { get; set; }
        public ReturnMessage Reply { get; set; }
        public IDictionary<object, object> Properties { get; set; }
    }
}
