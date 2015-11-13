using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using System.Web.SessionState;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Controls
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CheckOrderLineStatus : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!LightBO.IsLight()) return;

            int i;
            int StatusID;
            if (int.TryParse(context.Request.Params["OrderLineID"], out i) && int.TryParse(context.Request.Params["newStatus"], out StatusID))
            {
                using (var ctxStore = new DCFactory<StoreDataContext>())
                {
                    OrderLine CurOrderLine = ctxStore.DataContext.OrderLines.Where(x => x.OrderLineID == i).FirstOrDefault();
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(CurOrderLine.CanChangeLiteStatus(StatusID).ToString());
                }
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
