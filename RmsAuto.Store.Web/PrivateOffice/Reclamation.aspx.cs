using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web.PrivateOffice
{
	public partial class Reclamation : LocalizablePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			_pageTitleLiteral.Text = CmsContext.Current.CatalogItem.CatalogItemName;

			string mode = HttpContext.Current.Request.QueryString["mode"];
			if ( !string.IsNullOrEmpty( mode ) )
			{
				Reclamation1.Mode = mode;
			}
		}
	}
}
