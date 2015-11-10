using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RmsAuto.Store.Cms.Routing;
using System.Configuration;
using System.Net;
using RmsAuto.Store.Web.BasePages;

namespace RmsAuto.Store.Web
{
	public partial class Default : LocalizablePage
	{
		protected void Page_Load( object sender, EventArgs e )
		{
            // Devyatov 24.02.2011
            // Изменено последнее условие с Request.RawUrl.Contains( '?' )
            // IE при повторном обращении выдавал запрос с "?" в конце
            if ( Request.QueryString.Count != 0 || Request.RawUrl.EndsWith( "//" ) || Request.RawUrl.Remove( Request.RawUrl.Length - 1 ).Contains( '?' ) )
            {
                Response.Redirect( ConfigurationManager.AppSettings["WebSiteUrl"], false );
                Response.StatusCode = (int)HttpStatusCode.MovedPermanently;
                Response.End();
            }

			CmsContext.Current.CatalogItem = UrlManager.CatalogItems.RootCatalogItem;

			_pageBodyPlaceHolder.Visible = !string.IsNullOrEmpty( CmsContext.Current.CatalogItem.PageBody );
			_pageBodyLiteral.Text = CmsContext.Current.CatalogItem.PageBody;
		}
	}
}
