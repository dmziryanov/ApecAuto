using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Store
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AddToCart : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            var qty = int.Parse(context.Request.Params["qty"]);
            var defaultOrderQty = int.Parse(context.Request.Params["DefaultOrderQty"]);
            var key = context.Request.Params["key"];

            string error = null;
            if (qty < defaultOrderQty)
                error = "Количество не должно быть меньше числа деталей в комплекте";
            else if (qty % defaultOrderQty != 0)
            {
                error = "Количество должно быть кратным числу деталей в комплекте (" +
                    defaultOrderQty
                    .Progression(defaultOrderQty, 5)
                    .Select(i => i.ToString())
                    .Aggregate((acc, s) => acc + "," + s) + " и т.д.)";
            }

            
            AddToCartResult res;
            
            if (error != null)
            {
                res = new AddToCartResult();
                res.ErrorText = error;
                res.isSuccessful = false;
            }
            else
            {
                var spKey = SparePartPriceKey.Parse(key);
                SiteContext.Current.CurrentClient.Cart.Add(spKey, qty, true);
                res = new AddToCartResult();
                var ShoppingCartTotals = SiteContext.Current.CurrentClient.Cart.GetTotals();
                res.TotalCount = ShoppingCartTotals.PartsCount;
                res.TotalSum = ShoppingCartTotals.Total;
                res.isSuccessful = true;
            }

            context.Response.AppendHeader("Access-Control-Allow-Origins", "*");
            context.Response.AppendHeader("Access-Control-Request-Headers", "*");
            context.Response.AppendHeader("Access-Control-Allow-Methods", "*");
            
            context.Response.ContentType = "application/json";
            context.Response.Write(this.JsonSerializerWithRoot(res));
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
