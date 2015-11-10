using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web
{
	public partial class SearchDiscountManufacturers : LocalizablePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			CmsContext.Current.CatalogItem = UrlManager.CatalogItems.OffersCatalogItem;
		}
	}
}
