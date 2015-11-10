using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web.Cms
{
	public partial class OffersList : LocalizablePage 
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;
		}
	}
}
