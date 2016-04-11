using System;
using System.Transactions;

namespace Newbie.Util.Common
{
    public static class MethodHelper
    {
        public static void WrapTranaction(Action action)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    action();
                    tran.Complete();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
