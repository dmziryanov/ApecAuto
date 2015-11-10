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
//using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Web.PrivateOffice
{
	public partial class OrderLineTracking : PageWithUrlState
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			OrderLineTracking1.OrderLineId = Convert.ToInt32( Request[ "ID" ] );
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;

			_backLink.NavigateUrl = Request[ "back_url" ];
			_backLink.Visible = !string.IsNullOrEmpty( _backLink.NavigateUrl );

			UrlStateContainer.AddBaseParameterFromRequest( "ID" );
			UrlStateContainer.AddBaseParameterFromRequest( "back_url" );
		}
	}
}
