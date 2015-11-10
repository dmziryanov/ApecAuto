using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using RmsAuto.Store.Cms.Routing;
using System.Text;
using System.Web.SessionState;
using RmsAuto.Store.BL;

namespace RmsAuto.Store.Web.Manager
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class OrderLinesPrintBootStrap : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            if (!LightBO.IsLight()) return;// проверка что вызывается в лайте, проверка прав осуществляется настройками конфига

            var TextPage = (byte[])context.Session["OrderLinesPrint"];
         
            RmsAuto.Store.Acctg.ClientProfile cl = RmsAuto.Store.Acctg.ClientProfile.Load((string)context.Session["ClientID"]);
            context.Response.ContentType = "application/text;" + Encoding.Default.EncodingName;
            context.Response.Write( Encoding.Default.GetString(TextPage) );
            context.Response.AddHeader("Content-Disposition", "filename='Накладная №" + cl.AcctgId + "_"+ DateTime.Now.ToString("hhmmss") + ".htm'");
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
