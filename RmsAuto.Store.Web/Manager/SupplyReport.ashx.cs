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
    public class SupplyReport : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            if (!LightBO.IsLight()) return;

            //List<ReportString> l = (List<ReportString>)context.Session["SupplyReportSearchResults"];
            {

                //var x = from n in l
                        //select string.Join(";", new string[] { n..ToString(), Convert.ToString(n.ClientName), Convert.ToString(n.OrderId), Convert.ToString(n.Brand), Convert.ToString(n.Articul), Convert.ToString(n.Name), Convert.ToString(n.Quantity), Convert.ToString(n.SupplierName), Convert.ToString(n.price), Convert.ToString(n.summ) });

                //context.Response.ContentEncoding = Encoding.Default;
                //context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;
                //context.Response.AppendHeader("content-disposition", "inline; filename=SupplyReport.csv");
                //context.Response.Write(HttpUtility.HtmlEncode("Дата;Клиент;Номер заказа;Производитель;Артикул;Наименование;Количество;Поставщик;Цена;Сумма") + Environment.NewLine);
                //context.Response.Write(string.Join(Environment.NewLine, x.ToArray()));
            }

            if (!LightBO.IsLight()) return;// проверка что вызывается в лайте, проверка прав осуществляется настройками конфига

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);

            var lowDate = DateTime.Parse(context.Request.Params["lowDate"], CultureInfo.CurrentCulture);
            var hiDate = DateTime.Parse(context.Request.Params["hiDate"], CultureInfo.CurrentCulture);
            var currentUserID = context.Request.Params["UserID"];
            var OrderId = context.Request.Params["OrderId"];


            var cnt = LightBO.GetStringCount(lowDate, hiDate, 0, 0, currentUserID, OrderId, LightBO.SupplyOrderLineStatus );

            List<ReportString> l = LightBO.GetSupplyStrings(lowDate, hiDate, cnt, 0, "", "", currentUserID, OrderId);  // (List<SaleReportString>)context.Session["SaleReportSearchResults"];
            {

                LightBO.FillSupplyPrice(l);
                l.FindAll(i => i != null).ForEach(n => n.Total = n.UnitPrice * n.Qty);

                var x = from n in l
                    select string.Join(";", new string[] { n.StatusChangeTime.ToString(), Convert.ToString(n.ClientName), Convert.ToString(n.OrderId), Convert.ToString(n.Manufacturer), Convert.ToString(n.PartNumber), Convert.ToString(n.PartName), Convert.ToString(n.Qty), Convert.ToString(n.SupplierName), Convert.ToString(n.SupplyPrice), Convert.ToString(n.SupplyTotal) });

                context.Response.ContentEncoding = Encoding.Default;
                context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;
                context.Response.AppendHeader("content-disposition", "inline; filename=SupplyReport.csv");
                context.Response.Write(HttpUtility.HtmlEncode("Date;Customer;Order number;Brand;Part number;Description;Q-ty;Supplier;Price;Total") + Environment.NewLine);
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
