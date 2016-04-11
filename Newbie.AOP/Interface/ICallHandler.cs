using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.AOP.Interface
{
    public interface ICallHandler
    {
        object PreInvoke(InvocationContext context);
        void PostInvoke(InvocationContext context, object correlationiState);
        int Ordinal { get; set; }
        bool ReturnIfError { get; set; }
    }
}
