using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Web.Manager.Controls;
using System.Text;
using System.Web.SessionState;
using RmsAuto.Store.BL;
using System.Globalization;
using System.Threading;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SaleReport : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!LightBO.IsLight()) return;// проверка что вызывается в лайте, проверка прав осуществляется настройками конфига в текущей веб-папке

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);


            var lowDate = DateTime.Parse(context.Request.Params["lowDate"], CultureInfo.CurrentCulture);
            var hiDate = DateTime.Parse(context.Request.Params["hiDate"], CultureInfo.CurrentCulture);
            
            var currentUserID = context.Request.Params["UserID"];
            var OrderId = context.Request.Params["OrderId"];


            var cnt = LightBO.GetStringCount(lowDate, hiDate, 0, 0, currentUserID, OrderId, LightBO.SaleOrderLineStatus);

            List<ReportString> l = LightBO.GetSaleStrings(lowDate, hiDate, cnt, 0, "" , "" , currentUserID, OrderId);  // (List<SaleReportString>)context.Session["SaleReportSearchResults"];
            {

                LightBO.FillSupplyPrice(l);
                l.FindAll(n => n != null).ForEach(n => n.ProfitSumm = (n.UnitPrice - n.SupplyPrice) * n.Qty);
                ////ordlnsdto.ForEach(n => n.ProfitPercent = Math.Round((n.ProfitSumm / n.SaleSumm ) * 100, 2));
                //// Рентабельность = Прибыль / Цена закупки (*100 - в процентах) !!! Со слов И.С. Беженуца рентабельность = колонка 12 / колонка 9 * кол-во
                l.FindAll(n => n != null).ForEach(n => n.ProfitPercent = Math.Round((n.ProfitSumm / (n.SupplyPrice * n.Qty)) * 100, 2));

                var x = from n in l
                        select string.Join(";", new string[] { n.StatusChangeTime.ToString(), Convert.ToString(n.ClientName), Convert.ToString(n.OrderId), Convert.ToString(n.Manufacturer), Convert.ToString(n.PartNumber), Convert.ToString(n.PartName), Convert.ToString(n.Qty), Convert.ToString(n.SupplierName), Convert.ToString(n.SupplyPrice), Convert.ToString(n.UnitPrice), Convert.ToString(n.Total), Convert.ToString(n.ProfitSumm), Convert.ToString(n.ProfitPercent) });

                context.Response.ContentEncoding = Encoding.Default;
                context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;
                context.Response.AppendHeader("content-disposition", "inline; filename=SaleReport.csv");
                context.Response.Write(HttpUtility.HtmlEncode("Date; Client; Order number;Brand;Part number;Description;Q-ty;Supplier;Purchase price;Sale price; Supply Price; Profit; (%)") + Environment.NewLine);
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
