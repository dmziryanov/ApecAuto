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

using RmsAuto.Store.Cms.Routing;
using RmsAuto.Common.Web.UrlState;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web.PrivateOffice
{
	public partial class OrderList : PageWithUrlState
	{
		public enum Views
		{
			RequiresReactionView = 0,
			ActiveOrdersView = 1,
			ArchiveOrdersView = 2,
			WholesaleView = 3,
			AnalysisOrders = 4
		}

		protected void Page_Load( object sender, EventArgs e )
		{
            _pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;
			if( !IsPostBack )
			{
				if( !string.IsNullOrEmpty( Request[ "view" ] ) )
				{
					Views view = (Views)int.Parse( Request[ "view" ] );
					_multiView.ActiveViewIndex = (int)view;
				}
				else
				{
					//проверить наличие записей требующих подтверждения
					var reqReactionCount = OrderTracking.GetOrderLinesRequiresReactionCount( SiteContext.Current.CurrentClient.Profile.ClientId );
					if( reqReactionCount == 0 )
						_multiView.ActiveViewIndex = (int)Views.ActiveOrdersView;
				}
			}

			UrlStateContainer[ "view" ] = Convert.ToString( _multiView.ActiveViewIndex );
		}

        protected void Page_PreRender(object sender, EventArgs e)
        {
            _ftpPriceHintTextItem.Visible = SiteContext.Current.CurrentClient.Profile.IsLegalWholesale;
        }

		protected void _multiView_ActiveViewChanged( object sender, EventArgs e )
		{
			UrlStateContainer[ "view" ] = Convert.ToString( _multiView.ActiveViewIndex );
		}
	}
}
