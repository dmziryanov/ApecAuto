using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using FreeImageAPI;
using RmsAuto.TechDoc.Entities;
using RmsAuto.TechDoc.Entities.Helpers;


namespace RmsAuto.TechDoc.Handlers
{
	public class ImageHandler : IHttpHandler
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

			if( string.IsNullOrEmpty( context.Request[ "id" ] ) )
			{
				context.Response.StatusCode = 404;
				context.Response.StatusDescription = "File not found";
				context.Response.End();
				return;
			}
			int photoId = int.Parse( context.Request[ "id" ] );

			ImageInfo img = Facade.GetImage( photoId );

			using( MemoryStream stm = new MemoryStream( img.Content ) )
			{
                if( img.Extension == "JP2" )
                {
                    // deas 09.03.2011 task3141
                    FreeImageBitmap fib = FreeImageBitmap.FromStream( stm );
                    //var fibitmap = FreeImage.LoadFromStream( stm, FREE_IMAGE_LOAD_FLAGS.JPEG_ACCURATE );
                    //try
                    //{
                        using( Bitmap b = fib.ToBitmap())//FreeImage.GetBitmap( fibitmap ) )
                        {
                            context.Response.Cache.SetMaxAge( TimeSpan.FromDays( 7 ) );
                            context.Response.ContentType = "image/jpeg";

                            ImageCodecInfo codecInfo = GetEncoder( ImageFormat.Jpeg );
                            EncoderParameters parameters = new EncoderParameters( 1 );
                            parameters.Param[ 0 ] = new EncoderParameter( Encoder.Quality, 95L );
                            b.Save( context.Response.OutputStream, codecInfo, parameters );
                        }
                    //}
                    //finally
                    //{
                    //    FreeImage.Unload( fibitmap );
                    //}
                }
                else
                {
					if( img.Extension == "PNG" ) context.Response.ContentType = "image/png";
					else if( img.Extension == "JPG" ) context.Response.ContentType = "image/jpeg";
                    else if ( img.Extension == "BMP" ) context.Response.ContentType = "image/bmp";
					else if( img.Extension == "GIF" ) context.Response.ContentType = "image/gif";
					else throw new Exception( "Unknown image format: " + img.Extension );

					context.Response.Cache.SetMaxAge( TimeSpan.FromDays( 7 ) );
					context.Response.BinaryWrite( img.Content );
                }
			}
			
			context.Response.End();
		}


		#endregion

		private ImageCodecInfo GetEncoder( ImageFormat format )
		{
			foreach( ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders() )
			{
				if( codec.FormatID == format.Guid )
				{
					return codec;
				}
			}
			return null;
		}
	}
}
