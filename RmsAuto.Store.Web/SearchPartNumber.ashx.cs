using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService( Namespace = "http://tempuri.org/" )]
	[WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
	public class SearchPartNumber : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			string pn = context.Request["pn"];
            
            if (context.Request.Cookies.Get("FranchName") != null)
            {
                string FranchName = context.Request.Cookies.Get("FranchName").Value;
                context.Response.Redirect(FranchName + UrlManager.GetSearchManufacturersUrl(pn, true), true);
            }
            else
            {
                context.Response.Redirect(UrlManager.GetSearchManufacturersUrl(pn, true), true);
            }
		}

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
