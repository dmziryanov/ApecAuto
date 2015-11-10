using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Web;
using RmsAuto.Store.Cms.BL;
using System.Collections.Specialized;
using System.Net;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Cms.Routing
{
	class SeoPartsCatalogRoute : RouteBase
	{
		static IRouteHandler _routeHandler = new SeoCatalogRouteHandler { PageVirtualPath = "~/Cms/SeoPartsCatalogPage.aspx" };
		static Regex _regex = new Regex( @"^Parts(?'path'(?:/\w+)*)\.aspx$", RegexOptions.Singleline | RegexOptions.Compiled );

		class SeoCatalogRouteHandler : IRouteHandler
		{
			public string PageVirtualPath { get; set; }

			public IHttpHandler GetHttpHandler( RequestContext requestContext )
			{
				CmsContext.Current.CatalogItem = UrlManager.CatalogItems.RootCatalogItem;
				CmsContext.Current.PageParameters = new NameValueCollection();
				CmsContext.Current.CustomPageParameters[ "SeoPartsCatalogItems" ] = requestContext.RouteData.DataTokens[ "SeoPartsCatalogItems" ];

				var routingPage = (System.Web.UI.Page)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(
					PageVirtualPath,
					typeof( System.Web.UI.Page ) );

				return routingPage;
			}
		}

		public override RouteData GetRouteData( HttpContextBase httpContext )
		{
			RouteData res = null;

			string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring( 2 ) + httpContext.Request.PathInfo;

			Match m = _regex.Match( virtualPath );
			if( m.Success )
			{
				var path = m.Groups[ "path" ].Value.Split( '/' );
				var seoPartsCatalogItems = SeoPartsCatalogDac.GetCatalogItemsByVirtualPath( path.Skip( 1 ).ToArray() );

				if( seoPartsCatalogItems != null )
				{
					res = new RouteData( this, _routeHandler );
					res.DataTokens.Add( "SeoPartsCatalogItems", seoPartsCatalogItems );
					res.Values.Add( "ID", seoPartsCatalogItems.Last().ID );
				}
				else
				{
					throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );
				}
			}

			return res;
		}

		public override VirtualPathData GetVirtualPath( RequestContext requestContext, RouteValueDictionary values )
		{
			List<string> path = new List<string>();
			path.Add( "Parts" );

			if( values[ "ID" ] != null )
			{
				path.AddRange( SeoPartsCatalogBO.GetVirtualPathById( (int)values[ "ID" ] ) );
			}
			else if( values[ "SeoPartsCatalogItems" ] != null )
			{
				path.AddRange( SeoPartsCatalogBO.GetVirtualPathByItems( (SeoPartsCatalogItem[])values[ "SeoPartsCatalogItems" ] ) );
			}

			string virtualPath = string.Join( "/", path.ToArray() ) + ".aspx";

			return new VirtualPathData( this, virtualPath );
		}

	}
}
