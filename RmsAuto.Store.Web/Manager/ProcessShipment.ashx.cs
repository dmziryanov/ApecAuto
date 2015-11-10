using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using RmsAuto.Store.Data;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ProcessShipment : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!LightBO.IsLight()) return;
            
            var OrderLinesIDs = context.Request.Params["OrderLinesIDs"];
            var CurrentClientId = context.Request.Params["CurrentUserId"];


            int[] ids = OrderLinesIDs.TrimEnd(';').Split(';').Select(s => int.Parse(s)).ToArray();


            try
            {
                LightBO.ProcessShipment(ids, CurrentClientId);
                context.Response.ContentType = "text/plain";
                context.Response.Write("Successfully dispatched");
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
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
