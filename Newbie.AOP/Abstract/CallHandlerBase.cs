using Newbie.AOP.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.AOP.Abstract
{
    public abstract class CallHandlerBase : ICallHandler
    {
        public abstract object PreInvoke(InvocationContext context);

        public abstract void PostInvoke(InvocationContext context, object correlationiState);

        public int Ordinal { get; set; }

        public bool ReturnIfError { get; set; }
    }
}
