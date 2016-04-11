using Newbie.AOP.Abstract;
using Newbie.AOP.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.AOP.BasicHandler
{
    public class ExceptionCallHandlerAttribute : HandlerAttribute
    {

        public string MessageTemplate { get; set; }

        public bool Rethrow { get; set; }

        public ExceptionCallHandlerAttribute()
        {
            this.MessageTemplate = "{Message}";
        }

        public override ICallHandler CreateCallHandler()
        {
            return new ExceptionCallHandler()
            {
                Ordinal = this.Ordinal,
                Rethrow = this.Rethrow,
                MessageTemplate = this.MessageTemplate,
                ReturnIfError = this.ReturnIfError
            };
        }
    }
}
