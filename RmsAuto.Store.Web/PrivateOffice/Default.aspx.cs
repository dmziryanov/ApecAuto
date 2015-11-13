using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.PrivateOffice
{
    public partial class Default : RmsAuto.Store.Web.BasePages.LocalizablePage
	{
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

	    protected void Page_Load( object sender, EventArgs e )
		{
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;

			using( var dc = new DCFactory<CmsDataContext>() )
			{
				int parentId = UrlManager.CatalogItems.PrivateOfficeCatalogItem.CatalogItemID;
				/*_dataList.DataSource = dc.DataContext.CatalogItems
					.Where( c => c.CatalogItemVisible && c.ParentItemID == parentId )
					.OrderBy( c => c.CatalogItemPriority );*/
				_dataList.DataSource = UrlManager.CatalogItems._catalogItems
					.Where(c => c.CatalogItemVisible && c.ParentItemID == parentId)
					.OrderBy(c => c.CatalogItemPriority);
				_dataList.DataBind();
			}
		}
	}
}
