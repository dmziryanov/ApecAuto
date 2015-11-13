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
	[WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
	public class FlashHandler : IHttpHandler
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
            using (var dc = new DCFactory<CmsDataContext>())
			{
				int fileID = Convert.ToInt32(CmsContext.Current.PageParameters["ID"]);
                File item = dc.DataContext.Files.SingleOrDefault(f => f.FileID == fileID);
                
                if (item.FileBody.Length == 0)  
                 return;  

                context.Response.ContentType = "application/x-shockwave-flash";                
                context.Response.AddHeader("Accept-Ranges", "bytes");

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(item.FileBody.ToArray()))  
                {  
                    context.Response.BinaryWrite(ms.ToArray());  
                }  
                context.Response.End();  
            }
		}
	}
}
