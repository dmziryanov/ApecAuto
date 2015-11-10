<%@ WebHandler Language="C#" Class="Debts" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Web.SessionState;
using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;

 public class Debts : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {


        var start = int.Parse(context.Request.Params["start"]);
        var PageSize = int.Parse(context.Request.Params["limit"]);
        string ClientName = (string)context.Session["DateReportClientName"] ?? "";
        if (ClientName == "null") { ClientName = ""; }

        PaymentsList pList = new PaymentsList();
        pList.items = LightBO.GetPaymentList(start, PageSize, DateTime.Parse((string)context.Session["dateEnd"] ?? "2011-11-11"), ClientName);
        pList.summaryData = LightBO.GetSummaryPaymentList(start, PageSize, DateTime.Parse((string)context.Session["dateEnd"] ?? "2011-11-11"));
        pList.summaryData = pList.summaryData.Where(x => pList.items.Count(y => y.ClientName == x.ClientName) > 0).ToList(); //Фильтруем итоговые строчки, чтобы остались только те которые в текущем запросе
        if (pList.items.Count >0) pList.totalCount = pList.items[0].cnt;
       // pList.summaryData = pList.items.GroupBy(x => x.ClientName).Select(y => new OrdersDTO { ClientName = y.Key, DebtSumm = y.Sum(z => z.PaymentSum) } ).ToList();
        //pList.items = pList.items.Skip(start).Take(PageSize).ToList();
        
        /*Дату время укажем минимальную, на случай если придется вносить взаиморасчеты задним числом, DateTime.Parse("2015-01-01"), start, PageSize);*/

        //TODO: возможно стоит вставить заголовки строк
        if (pList != null)
        {
            context.Response.ContentEncoding = Encoding.Default;

            //Необходимо присвоить внутреннюю кодировку
            context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;

            //OrdersDTO[] ord = ordlnsdto.Orders.Select(x => new OrdersDTO { OrderID = x.OrderID, ClientName = x.ClientName, DaysDelayed = 7, DocumentSum = x.Total, DateOfBegin = x.OrderDate.ToString("dd-MM-yyyy"), DebtSumm = x.Total, DelayedSumm = x.Total }).ToArray();

            context.Response.ContentType = "application/json";
            context.Response.Write(this.JsonSerializer( pList ));
        }
        else
            throw new HttpException("Данные устарели, повторите поиск строк заказа");

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}