using Newbie.AOP.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.AOP.BasicHandler
{
    public class ExceptionCallHandler : CallHandlerBase
    {
        public string MessageTemplate { get; set; }
        public bool Rethrow { get; set; }

        public ExceptionCallHandler()
        {
            this.MessageTemplate = "{Message}";
        }

        public override object PreInvoke(InvocationContext context)
        {
            return null;
        }

        public override void PostInvoke(InvocationContext context, object correlationState)
        {
            if (context.Reply.Exception != null)
            {
                string message = this.MessageTemplate.Replace("{Message}", context.Reply.Exception.InnerException.Message)
                    .Replace("{Source}", context.Reply.Exception.InnerException.Source)
                    .Replace("{StackTrace}", context.Reply.Exception.InnerException.StackTrace)
                    .Replace("{HelpLink}", context.Reply.Exception.InnerException.HelpLink)
                    .Replace("{TargetSite}", context.Reply.Exception.InnerException.TargetSite.ToString());
                Console.WriteLine(message);
                if (!this.Rethrow)
                {
                    context.Reply = new ReturnMessage(null, null, 0, context.Request.LogicalCallContext, context.Request);
                }
            }
        }
    }
}
