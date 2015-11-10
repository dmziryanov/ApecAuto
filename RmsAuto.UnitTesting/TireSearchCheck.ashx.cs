using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RmsAuto.Store.Web.Controls;

namespace RmsAuto.Store.Web
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TireSearchCheck : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            for (int i = 0; i < 100000; i++)
            {

                var s = new TireSearch();
                s.DoSearch("Continental", "все", "все", "все", "все");
                context.Response.Write(i.ToString());
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
