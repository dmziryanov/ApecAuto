using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Entities;
using System.Web.SessionState;
using System.Text;
using RmsAuto.Store.Data;
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
    public class OrderLinesBootStrap : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!LightBO.IsLight()) return;// проверка что вызывается менеджером

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteContext.CurrentCulture);
            
            using (var ctx = new DCFactory<StoreDataContext>())
            { 
            var IDs = context.Request.Params["IDs"];

            int[] k;
            RmsAuto.Store.Web.OrderTracking.OrderLineFilter f = null;
            OrderLine[] ordlns;

            if (IDs != null)
            {
                k = IDs.Split(';').Select(x => string.IsNullOrEmpty(x) ? 0 : int.Parse(x)).ToArray();
                f = new RmsAuto.Store.Web.OrderTracking.OrderLineFilter() { OrderIDs =k };
                ordlns = OrderTracking.GetOrderLinesForLiteRMM(f, RmsAuto.Store.Web.OrderTracking.OrderLineSortFields.OrderIDAsc, 0, int.MaxValue);
                
            }
            else
            {
                ordlns = (OrderLine[])HttpContext.Current.Cache["RmsAuto.Store.Web.ControlsOrderLinesWholesale.CurrentOrderLines"+SiteContext.Current.User.UserId];
            }

            var ordlnsdto = (from n in ordlns
                            join q in ctx.DataContext.Orders on n.OrderID equals q.OrderID
                            join m in ctx.DataContext.Users on q.ClientID equals m.AcctgID
                            select new { n.SupplierID, n.Manufacturer, n.PartNumber, n.Qty, n.AcctgOrderLineID, n.CurrentStatus, n.OrderID, n.CurrentStatusDate, n.LastStatusChange, n.EstSupplyDate, n.DeliveryDaysMin, n.DeliveryDaysMax, n.PartName, n.Total, n.UnitPrice, m.AcctgID, m.Clientname, q.OrderDate }).ToArray();

            //TODO: возможно стоит вставить заголовки строк
            if (ordlns != null)
            {
                context.Response.ContentEncoding = Encoding.Default;
                
                //Необходимо присвоить внутренню кодировку
                context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;
                context.Response.Write(HttpUtility.HtmlEncode("#;Supplier Number;Manufacturer;Part Number;Number;Status ID;Order ID;Order date;Est. supply date;Min supplyr term;Max supply term;Part;Q-ty;Price;Total summ;Client Number;Client's name") + Environment.NewLine);
                context.Response.Write(string.Join(Environment.NewLine, ordlnsdto.Select(x => (Array.IndexOf(ordlnsdto, x) + 1).ToString() + ";" + x.SupplierID + ";" + x.Manufacturer + ";" + x.PartNumber + ";" + x.AcctgOrderLineID + ";" + x.CurrentStatus + ";" + x.OrderID + ";" + x.OrderDate + ";" + x.EstSupplyDate + ";" + x.DeliveryDaysMin + ";" + x.DeliveryDaysMax + ";" + x.PartName + ";" + x.Qty + ";"  + x.UnitPrice+";" + x.Total + ";" + x.AcctgID + ";" + x.Clientname).ToArray()));
                context.Response.AppendHeader("content-disposition", "inline; filename=OrderLines.csv");
            }
            else
                throw new HttpException("Данные устарели, повторите поиск строк заказа");
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
