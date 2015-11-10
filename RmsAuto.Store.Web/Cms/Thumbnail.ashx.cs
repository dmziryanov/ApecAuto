using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Misc.Thumbnails;
using System.Net;

namespace RmsAuto.Store.Web.Cms
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService( Namespace = "http://tempuri.org/" )]
	[WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
	public class ThumbnailHandler : IHttpHandler
	{

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
		
		public void ProcessRequest( HttpContext context )
		{
			int fileID = Convert.ToInt32( CmsContext.Current.PageParameters[ "ID" ] );
			string thumbnailGeneratorKey = Convert.ToString( CmsContext.Current.PageParameters[ "Key" ] );


            ThumbnailInfo thumbnail = ThumbnailGenerator.GetThumbnail(fileID, thumbnailGeneratorKey, context.Request.Params["r"]);
			if( thumbnail == null )
			{
				throw new HttpException( (int)HttpStatusCode.NotFound, "Not Found" );
			}
			else
			{
				string etag = thumbnail.LastModifiedDate.GetHashCode().ToString( "x" );

				context.Response.ContentType = thumbnail.ContentType;
				context.Response.Cache.SetCacheability( HttpCacheability.ServerAndPrivate );
				context.Response.Cache.SetETag( etag );
				//context.Response.Cache.SetMaxAge( new TimeSpan( 0, 10, 0 ) );

				if( context.Request.Headers[ "If-None-Match" ] != etag )
				{
					context.Response.WriteFile( thumbnail.FilePath );
				}
				else
				{
					context.Response.StatusCode = (int)HttpStatusCode.NotModified;
				}
			}
		}

	}
}
