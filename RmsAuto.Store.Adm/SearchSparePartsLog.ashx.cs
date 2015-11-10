using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Text;
using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Adm
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService( Namespace = "http://tempuri.org/" )]
	[WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
	public class SearchSparePartsLog : IHttpHandler
	{

		public void ProcessRequest( HttpContext context )
		{
			context.Response.ContentEncoding = Encoding.GetEncoding( 1251 );
			context.Response.ContentType = "text/csv";
			context.Response.AddHeader( "Content-Disposition", "attachment; filename=\"search_log.csv\"" );

			DateTime date1 = DateTime.Parse( context.Request[ "date1" ] );
			DateTime date2 = DateTime.Parse( context.Request[ "date2" ] );

			using( var dc = new LogDataContext() )
			{
				context.Response.Write( "SearchDate;tbPartNumber;tbManufacturer;ClientIP\r\n" );
				foreach( var log in SearchSparePartsLogDac.GetLog( dc, date1, date2 ) )
				{
					context.Response.Write( string.Format( "{0:yyyy-MM-dd HH:mm:ss};\"{1}\";{2};{3}\r\n",
						log.SearchDate,
						log.PartNumber.Replace( "\"", "\"\"" ),
						log.Manufacturer != null ? "\"" + log.Manufacturer.Replace( "\"", "\"\"" ) + "\"" : "",
						log.ClientIP ) );
				}
			}
		}

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
