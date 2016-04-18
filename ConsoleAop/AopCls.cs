using Newbie.AOP;
using Newbie.AOP.BasicHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn.AOP.Test
{
    public class AopInvocation
    {
        public static void MainMethod()
        {
            InstanceBuilder.Create<AopCls, IAopCls>().GetProcessResult(3);
            InstanceBuilder.Create<AopCls, IAopCls>().TransProcessResult(3);
        }
    }

    public class AopCls : IAopCls
    {
        [ExceptionCallHandler(Ordinal = 1, MessageTemplate = "Encounter error:\nMessage:{Message}")]
        public void GetProcessResult(int args)
        {
            if (args < 10)
            {
                int a = args / 0;
            }
        }

        [ExceptionCallHandler(Ordinal = 1, MessageTemplate = "Encounter error:\nMessage:{Message}")]
        [TransactionScopeCallHandler(Ordinal = 2)]
        public void TransProcessResult(int args)
        {
            int a = args / 0;
        }
    }

    public interface IAopCls
    {
        void GetProcessResult(int args);
        void TransProcessResult(int args);
    }
}
