using Newbie.AOP.Abstract;
using Newbie.AOP.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.AOP.BasicHandler
{
    public class TransactionScopeCallHandlerAttribute : HandlerAttribute
    {
        public override ICallHandler CreateCallHandler()
        {
            return new TransactionScopeCallHandler() { Ordinal = this.Ordinal, ReturnIfError = this.ReturnIfError };
        }
    }
}
