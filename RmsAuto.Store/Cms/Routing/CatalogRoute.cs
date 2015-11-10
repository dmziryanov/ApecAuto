using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Routing;
using System.Text.RegularExpressions;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms;

namespace RmsAuto.Store.Cms.Routing
{

	class CatalogRoute : RouteBase
	{
		IRouteHandler _routeHandler = new CatalogRouteHandler();
		static Regex _regex = new Regex( @"^(?'path'\w+(?:/\w+)*)\.aspx$", RegexOptions.Singleline | RegexOptions.Compiled );

		class CatalogRouteHandler : IRouteHandler
		{
			public IHttpHandler GetHttpHandler( RequestContext requestContext )
			{
				int catalogItemID = (int)requestContext.RouteData.Values[ "ID" ];

				CatalogItem catalogItem = UrlManager.CatalogItems.GetCatalogItem( catalogItemID );

				var routingPage = (System.Web.UI.Page)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(
					(string)catalogItem.CatalogItemPath,
					typeof( System.Web.UI.Page ) );

				CmsContext.Current.CatalogItem = catalogItem;
				CmsContext.Current.PageParameters = HttpUtility.ParseQueryString( catalogItem.CatalogItemQueryString ?? "" );

				return routingPage;
			}
		}

		public override RouteData GetRouteData( HttpContextBase httpContext )
		{
			RouteData res = null;

			string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

			Match m = _regex.Match(virtualPath);
			if(m.Success)
			{
				//string[] path = m.Groups[ "path" ].Value.Split( '/' );

				CatalogItem catalogItem = UrlManager.CatalogItems.GetCatalogItemByVirtualPath(m.Groups["path"].Value);

				if( catalogItem != null )
				{
					res = new RouteData( this, _routeHandler );
					res.Values.Add( "ID", catalogItem.CatalogItemID );
				}
			}

			return res;
		}

		public override VirtualPathData GetVirtualPath( RequestContext requestContext, RouteValueDictionary values )
		{
			string path = UrlManager.CatalogItems.GetVirtualPathForCatalogItem( (int)values[ "ID" ] );

			string virtualPath = path/*string.Join( "/", path.ToArray() )*/ + ".aspx";

			return new VirtualPathData( this, virtualPath );
		}

	}

}
