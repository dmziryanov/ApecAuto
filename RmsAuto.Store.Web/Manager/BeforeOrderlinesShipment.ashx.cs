using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.BL;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BeforeOrderlinesShipment : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            var OrderLinesIDs = context.Request.Params["OrderLinesIDs"];
            var OrderLineID = OrderLinesIDs.Split(';');
            var ConfirmText = LightBO.BeforeSendCheckOrderLine(OrderLineID);
            context.Response.ContentType = "text/plain";
            context.Response.Write(ConfirmText);
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
