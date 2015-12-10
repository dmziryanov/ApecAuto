using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Controls
{
    public partial class QuickSearch : System.Web.UI.UserControl
    {
		protected string GetSearchManufacturersUrlPattern()
		{
			string url;
			if( !SiteContext.Current.IsAnonymous && SiteContext.Current.User.Role == SecurityRole.Manager )
			{
				url = ResolveUrl(RmsAuto.Store.Web.Manager.Store.SearchManufacturers.GetUrl() );
                //url = UrlManager.GetSearchManufacturersUrl();
			}
			else
			{
                url = UrlManager.GetSearchManufacturersUrl();
                //url = RmsAuto.Store.Web.SearchManufacturers.GetUrl();
			}
			return string.Format(
				"{0}?{1}={2}&{3}={4}",
				url,
				UrlKeys.StoreAndTecdoc.EnteredPartNumber, "{pn}",
				UrlKeys.StoreAndTecdoc.SearchCounterparts, "{sc}" );
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtPartNumber.Value = HttpUtility.UrlDecode(Request.QueryString["pn"]);
                cbSearchCrosses.Checked = true;// (Request.QueryString["st"] == "0"?false:true);
            }

//            _floatingSearchFormLink.OnClientSearch = 
//                string.Format( 
//                @"	$('#{0}').val(e.partNumber); 
//					$('#{1}').get().checked=e.searchCrosses;
//					window.focus();
//					doSearch();",
//                txtPartNumber.ClientID,
//                cbSearchCrosses.ClientID );
//            _floatingSearchFormLink.Visible = SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager;
            //_btnsearch.Visible= !(SiteContext.Current.User != null && SiteContext.Current.User.Role == SecurityRole.Manager);
        }
    }
}