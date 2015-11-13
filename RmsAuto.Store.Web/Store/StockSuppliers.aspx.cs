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
using System.Collections.Generic;
using RmsAuto.Store.Configuration;
using RmsAuto.Store.Entities;
using RmsAuto.Store.BL;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Store
{
    public partial class StockSuppliers : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void pageTitleLiteralInit( object sender, EventArgs e )
		{
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );

			var ids = StocksConfiguration.Current.SupplierIds;
            using (var ctx = new DCFactory<StoreDataContext>())
			{
                throw new NotImplementedException();
                //var mfrs = ctx.DataContext.SpareParts
                //                .Where( sp => ids.Contains( sp.SupplierID ) )
                //                .Select( sp => sp.Manufacturer ).Distinct()
                //                .OrderBy( s => s );
                //rptMfrs.DataSource = mfrs;
                //rptMfrs.DataBind();
			}
		}

		protected char CurChar { get; private set; }

		protected void rptMfrsOnItemDataBound( object sender, RepeaterItemEventArgs e )
		{
			if( e.Item.ItemType == ListItemType.Item ||
				e.Item.ItemType == ListItemType.AlternatingItem )
			{
				if( !this.CurChar.Equals( ( e.Item.DataItem as string )[ 0 ] ) )
				{
					this.CurChar = ( e.Item.DataItem as string )[ 0 ];
				}
			}
		}
	}
}
