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
using System.Net;
using System.Text;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Cms
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService( Namespace = "http://tempuri.org/" )]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1 )]
	public class FileHandler : IHttpHandler
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
			using( var dc = new CmsDataContext() )
			{
                try
                {
                    int fileID = Convert.ToInt32(CmsContext.Current.PageParameters["ID"]);
                    File file = dc.Files.SingleOrDefault(f => f.FileID == fileID);

                    if (file == null)
                    {
                        throw new HttpException((int)HttpStatusCode.NotFound, "Not Found");
                    }

                    string etag = file.Timestamp.ToString();

                    context.Response.ContentType = file.FileMimeType;
                    context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
                    context.Response.Cache.SetETag(etag);

                    if (!file.IsImage)
                    {
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + EncodeFileName(file.FileName) + "\"");
                    }

                    if (context.Request.Headers["If-None-Match"] != etag)
                    {
                        context.Response.BinaryWrite(file.FileBody.ToArray());
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                    }
                }
                
                finally 
                {
                    if (dc.Connection.State == System.Data.ConnectionState.Open) { dc.Connection.Close(); }
                }
			}
		}

		string EncodeFileName( string fname )
		{
			System.Text.StringBuilder sb = new StringBuilder();
			string a = "йцукенгшщзхъфывапролджэячсмитьбюё";
			string b = "jj;c;u;k;e;n;g;sh;shh;z;kh;'';f;y;v;a;p;r;o;l;d;zh;eh;ja;ch;s;m;i;t;';b;ju;jo";
			string[] bb = b.Split( ';' );
			for( int i = 0 ; i < fname.Length ; ++i )
			{
				int j = a.IndexOf( Char.ToLower( fname[ i ] ) );
				if( j >= 0 )
					sb.Append( Char.IsUpper( fname[ i ] ) ? bb[ j ].ToUpper() : bb[ j ] );
				else
					sb.Append( fname[ i ] );
			}
			return sb.ToString();
		}
	}
}
