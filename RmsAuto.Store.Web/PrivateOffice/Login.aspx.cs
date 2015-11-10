using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
//using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Web
{
    public partial class Login1 : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if( User.Identity.IsAuthenticated )
			{
				Response.Redirect( UrlManager.GetPrivateOfficeUrl(), true );
			}

			CmsContext.Current.CatalogItem = UrlManager.CatalogItems.PrivateOfficeCatalogItem;

			if( !IsPostBack )
			{
				_logonPanel.Focus();
			}
        }
    }
}
