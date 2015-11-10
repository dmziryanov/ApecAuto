using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using RmsAuto.Store.Web.Manager.Controls;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;
using System.Text;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FinancialReport1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

              if (!LightBO.IsLight()) return;
           
              List<FinReportString> l = (List<FinReportString>)context.Session["FinSearchResults"];
                  {
                      
                    var x = from n in l
                            select string.Join(";", new string[] { n.Manager, n.ClientName, Convert.ToString((decimal)n.bClientSaldo), Convert.ToString((decimal)n.CashPayments), Convert.ToString((decimal)n.NonCashPayments), Convert.ToString((decimal)n.CashReturn), Convert.ToString((decimal)n.NonCashReturn), Convert.ToString((decimal)n.GoodsReturn), Convert.ToString((decimal)n.Supply), Convert.ToString((decimal)n.eClientSaldo) });

                    context.Response.ContentEncoding = Encoding.Default;
                    context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;
                    context.Response.AppendHeader("content-disposition", "inline; filename=FinancialReport.csv");
                    context.Response.Write(HttpUtility.HtmlEncode("Manager; Client; Opening balance;  Cash payments; Non-cash payments; Cash refund; Non-cash refund; Goods return; Dispatch;	Closing balance ") + Environment.NewLine);
                    context.Response.Write(string.Join(Environment.NewLine, x.ToArray()));
                  }
        }
        

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
