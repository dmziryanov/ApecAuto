using System;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Cms
{
    public partial class OnlineCatalogs : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
		protected void Page_PreRender( object sender, EventArgs e )
		{
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;
		}
	}
}
