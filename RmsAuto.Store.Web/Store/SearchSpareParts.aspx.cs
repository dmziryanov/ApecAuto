using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Cms.Routing;
using System.Collections.Specialized;
using RmsAuto.Common.Web;


namespace RmsAuto.Store.Web
{
    public partial class SearchSpareParts : RmsAuto.Store.Web.BasePages.LocalizablePage
    {
		protected void Page_Load( object sender, EventArgs e )
		{
			CmsContext.Current.CatalogItem = UrlManager.CatalogItems.OnlineCatalogsCatalogItem;
		}
		
    }
}
