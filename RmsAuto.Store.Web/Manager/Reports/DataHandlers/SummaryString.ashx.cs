using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Common.Misc;
using RmsAuto.Store.BL;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Manager.Reports.DataHandlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SummaryString : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {


            string ClientName = (string)context.Session["DateReportClientName"] ?? "";
            if (ClientName == "null") { ClientName = ""; }

            
            var EndDate = DateTime.Parse((string)context.Session["dateEnd"] ?? "2011-11-11");
            var res = LightBO.GetSummaryRow(ClientName, EndDate);

            
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
