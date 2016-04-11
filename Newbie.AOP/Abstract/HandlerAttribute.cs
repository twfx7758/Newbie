using Newbie.AOP.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.AOP.Abstract
{
    public abstract class HandlerAttribute : Attribute
    {
        public abstract ICallHandler CreateCallHandler();
        public int Ordinal { get; set; }
        public bool ReturnIfError { get; set; }
    }
}
