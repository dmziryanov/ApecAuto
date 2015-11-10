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
using System.Collections.Generic;
using RmsAuto.Store.Cms.Entities;

namespace RmsAuto.Store.Cms.Routing
{
	public class CatalogDependentRoute : RouteBase
	{
		string _catalogItemPath;
		Route _subRoute;

		public CatalogDependentRoute(
			string catalogItemPath,
			string subUrl,
			RouteValueDictionary constraints,
			RouteValueDictionary dataTokens,
			IRouteHandler routeHandler )
		{
			_subRoute = new Route(
				subUrl,
				null,
				constraints,
				dataTokens,
				routeHandler );
			_catalogItemPath = catalogItemPath;
		}

		public override RouteData GetRouteData( HttpContextBase httpContext )
		{
			RouteData res = null;

			CatalogItem catalogItem = UrlManager.CatalogItems.GetCatalogItemByPath( _catalogItemPath );
			string catalogItemVirtualPath = UrlManager.CatalogItems.GetVirtualPathForCatalogItem( catalogItem.CatalogItemID );

			string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring( 2 ) + httpContext.Request.PathInfo;

			if( virtualPath.StartsWith( catalogItemVirtualPath + "/" ) )
			{
				string subPath = virtualPath.Substring( catalogItemVirtualPath.Length + 1 );

				res = _subRoute.GetRouteData(
					new CatalogDependentHttpContext( "~/" + subPath ) );
				if( res != null )
				{
					res.Values.Add( "CatalogItem", catalogItem );
					res.Route = this;
				}
			}

			return res;
		}

		public override VirtualPathData GetVirtualPath( RequestContext requestContext, RouteValueDictionary values )
		{
			VirtualPathData p = _subRoute.GetVirtualPath( requestContext, values );

			CatalogItem catalogItem = UrlManager.CatalogItems.GetCatalogItemByPath( _catalogItemPath );
			string catalogItemVirtualPath = UrlManager.CatalogItems.GetVirtualPathForCatalogItem( catalogItem.CatalogItemID );

			return new VirtualPathData( this, catalogItemVirtualPath + "/" + p.VirtualPath );
		}

		#region HttpContextWrapper

		class CatalogDependentHttpContext : HttpContextBase
		{
			public CatalogDependentHttpContext( string appRelativeCurrentExecutionFilePath )
			{
				_request = new CatalogDependentRequest( appRelativeCurrentExecutionFilePath );
			}

			public override HttpRequestBase Request
			{
				get
				{
					return _request;
				}
			}
			CatalogDependentRequest _request;
		}

		class CatalogDependentRequest : HttpRequestBase
		{
			public CatalogDependentRequest( string appRelativeCurrentExecutionFilePath )
			{
				_appRelativeCurrentExecutionFilePath = appRelativeCurrentExecutionFilePath;
			}

			public override string AppRelativeCurrentExecutionFilePath
			{
				get
				{
					return _appRelativeCurrentExecutionFilePath;
				}
			}
			string _appRelativeCurrentExecutionFilePath;

			public override string PathInfo
			{
				get
				{
					return "";
				}
			}
		}

		#endregion
	}

	/*class CatalogDependentRoute : RouteBase
	{
		string _catalogItemPath;
		IRouteHandler _routeHandler;
		string _subUrl;
		Regex _regex;
		RouteValueDictionary _parameters;
		RouteValueDictionary _dataTokens;

		public CatalogDependentRoute(
			string catalogItemPath,
			string subUrl,
			RouteValueDictionary parameters,
			RouteValueDictionary dataTokens,
			IRouteHandler routeHandler )
		{
			_catalogItemPath = catalogItemPath;
			_routeHandler = routeHandler;
			_subUrl = subUrl;
			_parameters = parameters;
			_dataTokens = dataTokens;
			string pattern = subUrl.Replace( ".", @"\." );
			foreach( KeyValuePair<string, object> parm in _parameters )
			{
				pattern = pattern.Replace( "{" + parm.Key + "}", "(?'" + parm.Key + "'" + parm.Value + ")" );
			}
			_regex = new Regex( @"^" + pattern + "$", RegexOptions.Singleline | RegexOptions.Compiled );
		}

		public override RouteData GetRouteData( HttpContextBase httpContext )
		{
			RouteData res = null;

			CatalogItem catalogItem = CatalogItemsCache.Current.GetCatalogItemByPath( _catalogItemPath );
			string catalogItemVirtualPath = CatalogItemsCache.Current.GetVirtualPathForCatalogItem( catalogItem.CatalogItemID );

			string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring( 2 ) + httpContext.Request.PathInfo;

			if( virtualPath.StartsWith( catalogItemVirtualPath + "/" ) )
			{
				string subPath = virtualPath.Substring( catalogItemVirtualPath.Length + 1 );

				Match m = _regex.Match( subPath );
				if( m.Success )
				{
					res = new RouteData( this, _routeHandler );
					foreach( KeyValuePair<string, object> parm in _parameters )
					{
						res.Values.Add( parm.Key, m.Groups[ parm.Key ].Value );
					}
					res.Values[ "CatalogItem" ] = catalogItem;
					if( _dataTokens != null )
					{
						foreach( KeyValuePair<string, object> pair in _dataTokens )
						{
							res.DataTokens.Add( pair.Key, pair.Value );
						}
					}
				}
			}

			return res;
		}

		public override VirtualPathData GetVirtualPath( RequestContext requestContext, RouteValueDictionary values )
		{
			CatalogItem catalogItem = CatalogItemsCache.Current.GetCatalogItemByPath( _catalogItemPath );
			string catalogItemVirtualPath = CatalogItemsCache.Current.GetVirtualPathForCatalogItem( catalogItem.CatalogItemID );

			string subUrl = _subUrl;

			foreach( string parameterName in _parameters.Keys )
			{
				subUrl = subUrl.Replace( "{" + parameterName + "}", Convert.ToString( values[ parameterName ] ) );
			}

			return new VirtualPathData( this, catalogItemVirtualPath + "/" + subUrl );
		}
	}*/
}
