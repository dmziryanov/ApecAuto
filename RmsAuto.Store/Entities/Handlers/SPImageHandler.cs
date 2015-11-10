using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using RmsAuto.TechDoc.Entities;
using RmsAuto.TechDoc.Entities.Helpers;
using RmsAuto.Store.Dac;


namespace RmsAuto.Store.Entities.Handlers
{
	public class SPImageHandler : IHttpHandler
	{
		/// <summary>
		/// You will need to configure this handler in the web.config file of your 
		/// web and register it with IIS before being able to use it. For more information
		/// see the following link: http://go.microsoft.com/?linkid=8101007
		/// </summary>
		#region IHttpHandler Members

		public bool IsReusable
		{
			// Return false in case your Managed Handler cannot be reused for another request.
			// Usually this would be false in case you have some state information preserved per request.
			get { return true; }
		}

		public void ProcessRequest( HttpContext context )
		{
			if (string.IsNullOrEmpty(context.Request["spid"]))
			{
				context.Response.StatusCode = 404;
				context.Response.StatusDescription = "File not found";
				context.Response.End();
				return;
			}

			SparePartImage spi = SparePartImageDac.GetSparePartImage(context.Request["spid"]);
			byte[] imageBytes = spi.ImageBody.ToArray();

			context.Response.ContentType = "image/jpeg";
			context.Response.Cache.SetMaxAge(TimeSpan.FromDays(7));
			context.Response.BinaryWrite(imageBytes);

			context.Response.End();
		}


		#endregion
	}
}
