using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Newbie.Util
{
     public static  class SendMsgAndEmail
    {
        //#region 发送邮件和延续密码短信
        ///// <summary>
        ///// 发送邮件和延续密码短信
        ///// </summary>
        ///// <param name="dealerId">经销商ID</param>
        ///// <param name="dealerMail">经销商邮件</param>
        ///// <param name="phone">经销商联系电话</param>
        ///// <param name="salesMail">销售邮件</param>
        //public static void SendMailSMS(int dealerId, string dealerMail, string phone, string salesMail, Guid appID, string systemType)
        //{
        //    DataTable dtAccount = new DasAccountDalc().GetDasAccountByDealerIDAndAppID(dealerId, appID);
        //    DealerInfo dealerInfo = new DealerInfoDalc().GetDealerInfo(dealerId);

        //    DealerInfoSendMaillogEntity infolog = new DealerInfoSendMaillogEntity();
        //    infolog.DealerID = dealerId;
        //    infolog.SendID = -1;
        //    infolog.CreateDate = DateTime.Now;

        //    try
        //    {
        //        string strContact = string.Empty;
        //        if (!string.IsNullOrEmpty(dealerMail))
        //        {
        //            if (systemType == "IMS")//只为IMS平台发送开通应用邮件
        //            {
        //                Console.WriteLine("正在为经销商：" + dealerId.ToString() + "发送邮件......");
        //                string appListHtml = GetAppListHtml(dealerId, systemType);

        //                strContact = SendMail(dealerMail, dealerInfo, appListHtml);//给经销商发送邮件
        //                if (!string.IsNullOrEmpty(salesMail))//给销售发送邮件
        //                {
        //                    SendMail(salesMail, dealerInfo, appListHtml);
        //                }
        //                else
        //                {
        //                    infolog.SendMailContect = "无销售负责人邮件地址";
        //                    infolog.State = 0;
        //                    infolog.SendType = 0;
        //                    sendMailDalc.Insert(infolog);
        //                }
        //                Console.WriteLine("为经销商：" + dealerId.ToString() + "发送邮件成功。");
        //            }

        //            Console.WriteLine("正在为经销商：" + dealerId.ToString() + "发送短信......");

        //            string dasAccountLoginName = dtAccount.Rows[0]["DasAccountLoginName"].ToString();
        //            string strMessage = "";
        //            if (systemType == "SAS")
        //            {
        //                strMessage = string.Format("您购买的{0}会员（{1}）账号信息已开通。用户名为：{2},延续以前密码，登录地址：{3}", SAS_System, dealerInfo.DealerShortName, dasAccountLoginName, SAS_SystemWebSite);
        //            }
        //            else
        //            {
        //                strMessage = string.Format("您购买的{0}会员（{1}）账号信息已开通。用户名为：{2},延续以前密码。", IMS_System, dealerInfo.DealerShortName, dasAccountLoginName);
        //            }
        //            int intCode = -1;
        //            if (!string.IsNullOrEmpty(phone))
        //            {
        //                intCode = SendMobile(strMessage, phone);//给经销商发送短信
        //            }
        //            Console.WriteLine("为经销商：" + dealerId.ToString() + "发送短信成功！");

        //            infolog.SendMailContect = strContact;
        //            infolog.SendMobileContect = strMessage + "经销商邮箱地址：" + dealerMail;
        //            infolog.MobileCode = intCode;
        //            infolog.State = 1;
        //            infolog.SendType = 3;
        //            sendMailDalc.Insert(infolog);
        //        }
        //        else
        //        {
        //            //排期
        //            infolog.SendMailContect = "没有找到经销商邮件地址";
        //            infolog.SendMobileContect = "";
        //            infolog.MobileCode = 0;
        //            infolog.State = 1;
        //            infolog.SendType = 2;
        //            sendMailDalc.Insert(infolog);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("为经销商：" + dealerId.ToString() + "发送邮件失败，原因：" + ex.ToString());
        //        infolog.SendMailContect = ex.ToString();
        //        infolog.SendMobileContect = "经销商邮箱地址：" + dealerMail;
        //        infolog.State = 0;
        //        infolog.SendType = 3;
        //        sendMailDalc.Insert(infolog);
        //    }
        //}
        //#endregion
    }
}
