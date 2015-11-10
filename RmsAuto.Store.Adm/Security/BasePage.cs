using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RmsAuto.Store.Adm.Security
{
	public class BasePage : System.Web.UI.Page
	{
		private string[] permittedUrls = new string[] { 
			"/SeoPartsCatalogItems/List.aspx",
			"/SeoPartsCatalogItems/Insert.aspx",
			"/SeoPartsCatalogItems/Edit.aspx",
			"/SeoShortLinks/List.aspx",
			"/SeoShortLinks/Insert.aspx",
			"/SeoShortLinks/Edit.aspx"
		};

		public bool IsSeo
		{
			get
			{
                return HttpContext.Current.User.Identity.Name.Contains( "seo" );
			}
		}

		protected override void OnInit(EventArgs e)
		{
			string url = Request.RawUrl;
			if (IsSeo && !IsPermittedUrl(url))
			{
				Response.Redirect(Request.ApplicationPath + "Default.aspx");
			}
			base.OnInit(e);
		}

		private bool IsPermittedUrl(string url)
		{
			foreach (var currentUrl in permittedUrls)
			{
				if (url.Contains(currentUrl))
				{
					return true;
				}
			}
			return false;
		}
	}
}
