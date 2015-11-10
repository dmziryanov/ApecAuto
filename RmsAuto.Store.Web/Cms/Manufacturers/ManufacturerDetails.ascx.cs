using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.Routing;
using System.Net;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Web.Cms.Manufacturers
{
	public partial class ManufacturerDetails : System.Web.UI.UserControl
	{
		public string ManufacturerUrlCode { get; set; }

		protected string GetFileUrl( int fileId )
		{
			return UrlManager.GetFileUrl( fileId );
		}

		protected void Page_PreRender( object sender, EventArgs e )
		{
			using( var dc = new DCWrappersFactory<CmsDataContext>() )
			{
				Manufacturer man = dc.DataContext.Manufacturers.Where( m => m.UrlCode == ManufacturerUrlCode && m.ShowInCatalog ).SingleOrDefault();

				if( man == null )
					throw new HttpException( (int)HttpStatusCode.NotFound, "Not found" );

				CmsContext.Current.BreadCrumbSuffix.Add( new BreadCrumbItem( man.Name, UrlManager.GetManufacturerDetailsUrl( man.UrlCode ) ) );
				CmsContext.Current.PageFields = new PageFields {
					Title = man.PageTitle ?? man.Name,
					Keywords = man.PageKeywords,
					Description = man.PageDescription,
					Footer = man.PageFooter
				};

				_manufacturerLabel.Text = Server.HtmlEncode( man.Name );
				_infoLabel.Text = man.Info;

				if( !string.IsNullOrEmpty( man.WebSiteUrl ) )
				{
					_siteRow.Visible = true;
					_siteLink.NavigateUrl = man.WebSiteUrl;
					_siteLink.Text = Server.HtmlEncode( man.WebSiteUrl );
				}
				else
				{
					_siteRow.Visible = false;
				}

				if( man.LogoFileID.HasValue )
				{
					_logoRow.Visible = true;
					_logoImage.ImageUrl = UrlManager.GetFileUrl( man.LogoFileID.Value );
				}
				else
				{
					_logoRow.Visible = false;
				}

				if( man.Folder != null )
				{
					_filesRow.Visible = true;
					_filesRepeater.DataSource = man.Folder.Files.OrderBy( f => f.FileName );
					_filesRepeater.DataBind();
				}
				else
				{
					_filesRow.Visible = false;
				}


			}
		}
	}
}