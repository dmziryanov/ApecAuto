using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RmsAuto.Store.BL;

using RmsAuto.Store.Web.Manager.BasePages;

namespace RmsAuto.Store.Web.Manager
{
    public partial class Default : RMMPage
    {
		
        public static string GetUrl()
		{
			return "~/Manager/Default.aspx";
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			var siteContext = (ManagerSiteContext)SiteContext.Current;

            if (LightBO.IsLight() && SiteContext.Current is ManagerSiteContext)
            {
                var context = (ManagerSiteContext)SiteContext.Current;
                context.ClientSet.Clear();
            }

			if( siteContext.ClientSet.DefaultClient!=null )
			{
				Response.Redirect( ClientProfile.GetUrl(), true );
			}
        }
    }
}
