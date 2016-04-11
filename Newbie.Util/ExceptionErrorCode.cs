using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Util
{
    /*
     * Author:Chenqc
     * Des:异常字典
     * Format:ErrorCode:异常类型（Check,Format,Overload,Version）递进数字
     */
   public static class ExceptionErrorCode
   {

       /// <summary>
       /// Check_LimitNum_AllocNum  数据库约束，op人员连续 ，导致可分配数量大于或等于限制数量
       /// </summary>
       public const string CHECK_LIMITNUM_ALLOCNUM = "ErrorCode:Check_Num_001";
   }
}
