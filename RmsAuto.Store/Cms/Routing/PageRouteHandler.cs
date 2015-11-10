using System;
using System.Web;
using System.Web.Routing;
using System.Collections.Generic;
using System.Collections.Specialized;
using RmsAuto.Store.Cms;
using RmsAuto.Store.Cms.Entities;
using System.Web.SessionState;

namespace RmsAuto.Store.Cms.Routing
{
    public class PageRouteHandler : IRouteHandler
	{
		public IHttpHandler GetHttpHandler( RequestContext requestContext )
		{
			CmsContext.Current.PageParameters = new NameValueCollection();
			foreach( KeyValuePair<string, object> pair in requestContext.RouteData.Values )
			{
				CmsContext.Current.PageParameters[ pair.Key ] = Convert.ToString( pair.Value );
			}

			CmsContext.Current.CatalogItem = (CatalogItem)requestContext.RouteData.Values[ "CatalogItem" ];

			var routingPage = (IHttpHandler)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(
				(string)requestContext.RouteData.DataTokens[ "Url" ],
				typeof( IHttpHandler ) );

			return routingPage;
		}
	}
}
