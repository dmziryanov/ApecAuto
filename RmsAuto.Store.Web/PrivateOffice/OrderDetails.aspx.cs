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
	public partial class OrderDetails : PageWithUrlState
	{

		protected void Page_PreLoad(object sender, EventArgs e)
		{
			_pageTitleLiteral.Text = Resources.Texts.OrderedPartNo; //"Строки заказов";

			if(!IsPostBack)
			{
				var ids = Request[ "ID" ].Split( ',' ).Select( s => int.Parse( s ) ).ToArray();
				if( ids.Length == 0 )
					throw new ArgumentNullException( "ID" );

				_orderLinesWholesale.OrderIDs = ids;

				_backLink.NavigateUrl = Request[ "back_url" ];
				_backLink.Visible = !string.IsNullOrEmpty( Request[ "back_url" ]);
			}

			UrlStateContainer.AddBaseParameterFromRequest( "ID" );
			UrlStateContainer.AddBaseParameterFromRequest( "back_url" );
		}
	}
}
