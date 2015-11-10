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
using RmsAuto.Store.Web.Manager.BasePages;


namespace RmsAuto.Store.Web.Manager.Store
{
	public partial class SearchSpareParts :RMMPage
    {
		public static string GetUrl( string mfr, string pn, bool searchCounterparts )
		{
			return string.Format( "{0}?{1}={2}&{3}={4}{5}",
								 "~/Manager/Store/SearchSpareParts.aspx",
								 UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode( mfr ),
								 UrlKeys.StoreAndTecdoc.EnteredPartNumber, HttpUtility.UrlEncode( pn ),
								 searchCounterparts ? string.Format( "&{0}=1", UrlKeys.StoreAndTecdoc.SearchCounterparts ) : string.Empty );
		}

		public static string GetUrl( string mfr, string pn, bool searchCounterparts, int excludeSupplierID )
		{
			return string.Format( "{0}?{1}={2}&{3}={4}&{5}={6}{7}",
								 "~/Manager/Store/SearchSpareParts.aspx",
								 UrlKeys.StoreAndTecdoc.ManufacturerName, HttpUtility.UrlEncode( mfr ),
								 UrlKeys.StoreAndTecdoc.EnteredPartNumber, HttpUtility.UrlEncode( pn ),
								 UrlKeys.StoreAndTecdoc.ExcludeSupplierID, excludeSupplierID,
								 searchCounterparts ? string.Format( "&{0}=1", UrlKeys.StoreAndTecdoc.SearchCounterparts ) : string.Empty );
		}

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!SiteContext.Current.CurrentClient.IsGuest)
                SiteContext.Current.CurrentClient.Cart.ArticleAdded +=
                    new EventHandler<ArticleAddedEventArgs>(Cart_ArticleAdded);
        }
          

        void Cart_ArticleAdded(object sender, ArticleAddedEventArgs e)
        {
            Response.Redirect(ClientCart.GetUrl(), true);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (!SiteContext.Current.CurrentClient.IsGuest)
                SiteContext.Current.CurrentClient.Cart.ArticleAdded -=
                    new EventHandler<ArticleAddedEventArgs>(Cart_ArticleAdded);
        }
    }
}
