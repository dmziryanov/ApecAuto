using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;

namespace RmsAuto.Store.Web.Manager.Reports.DataHandlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PutSession : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var pName = context.Request.Params["ParamName"];
            var pValue = context.Request.Params["ParamValue"];
            context.Session[pName] = pValue;
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
