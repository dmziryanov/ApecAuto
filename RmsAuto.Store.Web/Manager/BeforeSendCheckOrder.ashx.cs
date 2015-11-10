using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.BL;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BeforeSendCheckOrder : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            var OrderID = int.Parse(context.Request.Params["OrderId"]);
            var ConfirmText = LightBO.BeforeSendCheckOrder(OrderID);
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
