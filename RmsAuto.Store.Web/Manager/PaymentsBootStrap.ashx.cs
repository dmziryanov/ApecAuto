using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using RmsAuto.Store.BL;
using System.Web.SessionState;
using RmsAuto.Common.Misc;
using System.Globalization;
using System.Threading;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PaymentsBootStrap : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!LightBO.IsLight()) return;
            //секурность нстраиваиется в веб.конфиге
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);

            var UserId = int.Parse(context.Request.Params["UserID"]);
            var DateFrom = DateTime.Parse(context.Request.Params["DateFrom"], CultureInfo.CurrentCulture);
            var DateTo = DateTime.Parse(context.Request.Params["DateTo"], CultureInfo.CurrentCulture);

            var Payments = LightBO.GetUserLightPayments(UserId, DateFrom, DateTo.AddDays(1), 0, 10000);

                //public int ID { get; set; }
                //public decimal PaymentSum { get; set; }
                //public DateTime PaymentDate { get; set; }
                //public LightPaymentType PaymentType { get; set; }
                //public int UserID { get; set; }
                //public string ClientName { get; set; }
          

            var x = from n in Payments
                    select string.Join(";", new string[] { n.ClientName, n.PaymentDate.ToString(), (-1 * n.PaymentSum).ToString(), n.PaymentType.ToTextOrName(), n.PaymentMethod.ToTextOrName() });

            context.Response.ContentEncoding = Encoding.Default;
            context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;
            context.Response.AppendHeader("content-disposition", "inline; filename=Payments.csv");
            context.Response.Write(HttpUtility.HtmlEncode("Client;Date; Total; Payment's type; Payment's method") + Environment.NewLine);
            context.Response.Write(string.Join(Environment.NewLine, x.ToArray()));
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
