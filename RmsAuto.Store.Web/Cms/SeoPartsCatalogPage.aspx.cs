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
using RmsAuto.Store.Cms.Entities;
using System.Net;
using RmsAuto.Store.Cms.Routing;
using System.Collections.Generic;
using RmsAuto.Store.Cms.Dac;
using System.Data.Linq;

namespace RmsAuto.Store.Web.Cms
{
    public partial class SeoPartsCatalogPage : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected string GetSubCatalogUrl( object seoPartsCatalogItem )
		{
			SeoPartsCatalogItem[] seoPartsCatalogItems = (SeoPartsCatalogItem[])CmsContext.Current.CustomPageParameters[ "SeoPartsCatalogItems" ];
			List<SeoPartsCatalogItem> items = new List<SeoPartsCatalogItem>();
			items.AddRange( seoPartsCatalogItems );
			items.Add( (SeoPartsCatalogItem)seoPartsCatalogItem );
			return UrlManager.GetSeoPartsCatalogUrl( items.ToArray() );
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			SeoPartsCatalogItem[] seoPartsCatalogItems = (SeoPartsCatalogItem[])CmsContext.Current.CustomPageParameters[ "SeoPartsCatalogItems" ];

			if( seoPartsCatalogItems.Length == 0 || !seoPartsCatalogItems.All( c => c.Visible ) )
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );

			var item = SeoPartsCatalogDac.GetCatalogItem( seoPartsCatalogItems.Last().ID );
			if( item == null || !item.Visible )
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );

			_bodyLiteral.Text = item.Body;

			var childItems = SeoPartsCatalogDac.GetVisibleChildItems( item.ID );
			_subItemsRepeater.Visible = childItems.Length != 0;
			_subItemsRepeater.DataSource = childItems;
			_subItemsRepeater.DataBind();

			CmsContext.Current.PageFields = new PageFields
			{
				Title = item.PageTitle ?? item.Name,
				Keywords = item.PageKeywords,
				Description = item.PageDescription,
				Footer = item.PageFooter
			};

			for( int i = 0 ; i < seoPartsCatalogItems.Length ; ++i )
			{
				CmsContext.Current.BreadCrumbSuffix.Add(
					new BreadCrumbItem(
						seoPartsCatalogItems[ i ].Name,
						UrlManager.GetSeoPartsCatalogUrl( seoPartsCatalogItems.Take( i + 1 ).ToArray() ) ) );
			}
		}
	}
}
