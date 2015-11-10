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
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Cms.Entities;
using System.Collections.Generic;
using RmsAuto.Store.Cms.Routing;

namespace RmsAuto.Store.Web.Cms.Catalog
{
	public partial class MainMenu : System.Web.UI.UserControl
	{
		protected CatalogItem[] _breadCrumbs;
		protected int _currentCatalogItemId;

		protected void Page_Load( object sender, EventArgs e )
		{
            // deas 24.04.2011 task3996 убран вызов static
			_siteMapDataSource.Provider = new CatalogSiteMapProvider().Default;
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
           _leftMenuCtl.DataBind();
		}
	}
}