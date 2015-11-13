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
using RmsAuto.Common.Web;
using RmsAuto.Store.Cms.Entities;
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Adm.scripts;
using System.Collections.Generic;
using System.Text;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.Controls.AddFile
{
	public partial class AddFileControl : System.Web.UI.UserControl
	{
        public int RenderType
        { get; set; }

	    private readonly String _folderName = "Баннеры";

		public enum AddFileSelectType
		{
			Image,
			File,
            Folder
		}

        public AddFileSelectType? SelectType { get; set; }
		public string OnClientFileClick { get; set; }

		protected int FolderID
		{
			get
			{
			    int? folderID;

                using( var dc = new DCFactory<CmsDataContext>() )
                {
                    folderID = FilesDac.GetFolderByName(dc.DataContext, _folderName).FolderID;
                }
                if (folderID != null)
			        return (int) folderID;
                else
                {
                    using (var dc = new DCFactory<CmsDataContext>())
                    {
                        Folder folder = new Folder();
                        folder.FolderName = _folderName;
                        folder.FolderCreationDate = DateTime.Now;
                        dc.DataContext.Folders.InsertOnSubmit(folder);
                        dc.DataContext.SubmitChanges();
                    }
                    
                    using (var dc = new DCFactory<CmsDataContext>())
                    {
                        folderID = FilesDac.GetFolderByName(dc.DataContext, _folderName).FolderID;
                    }
                    return (int) folderID;
                }
			}
		}

        public int BannerID
        {
            get
            {
                return (int)( !string.IsNullOrEmpty(Request[UrlKeys.Id]) ? (int?)Convert.ToInt32(Request[UrlKeys.Id]) : null);
            }
        }

        public int FileID
        {
            get { return Convert.ToInt32(Request["FileID"]); }
        }

		public static string GetFileUrl( int fileID )
		{
            using (var dc = new DCFactory<CmsDataContext>())
            {
                File f = FilesDac.GetFile(dc.DataContext, fileID);
                if (f.FileMimeType.Contains("flash"))
                    return string.Format("{0}/Files/{1}.ashx/{2}",
                    ConfigurationManager.AppSettings["WebSiteUrl"],
                    fileID, f.FileName);
                else
                    return string.Format("{0}/Files/{1}.ashx",
                    ConfigurationManager.AppSettings["WebSiteUrl"],
                    fileID);
            }
            
		}
		public static string GetThumbnailUrl( int fileID )
		{
			return string.Format( "{0}/Thumbnails/icon/{1}.ashx",
				ConfigurationManager.AppSettings[ "WebSiteUrl" ],
				fileID );
		}

		public static string GetFileTypeImageUrl( string fileName )
		{
			string ext = System.IO.Path.GetExtension( fileName ).TrimStart( '.' );
			string path = "~/images/Files/" + ext + ".png";
			if( !System.IO.File.Exists( HttpContext.Current.Server.MapPath( path ) ) )
				path = "~/images/Files/unknown.png";
			return path;
		}

		protected void Page_Load( object sender, EventArgs e )
		{
			Response.Cache.SetCacheability( HttpCacheability.NoCache );

			ScriptsManager.RegisterJQuery(Page);
		}

		protected string GetFolderName( int folderID )
		{
			return FilesDac.GetFolderPath( folderID ) ?? "";
		}
		protected void Page_PreRender( object sender, EventArgs e )
		{
			ReloadFiles();
            _noImagesLabel.Visible = FolderID != 0 && _filesRepeater.Items.Count == 0;
		}

		protected void _filesRepeater_ItemCommand( object source, RepeaterCommandEventArgs e )
		{
			if( e.CommandName == "Delete" )
			{
                using (var dc = new DCFactory<CmsDataContext>())
				{
					File file = dc.DataContext.Files.Where( f => f.FileID == Convert.ToInt32( e.CommandArgument ) ).First();
					if( file != null )
					{
						dc.DataContext.Files.DeleteOnSubmit( file );
					    Banners b = Banners.GetBannerByID(dc.DataContext, BannerID);
                        b.FileID = null;
                        
                        dc.DataContext.Files.DeleteOnSubmit(file);
                        dc.DataContext.SubmitChanges();
                        Response.Redirect(Request.RawUrl);
					}
				}
			}
		}

		void ReloadFiles()
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
                var bannerID = BannerID;
                // Режим создания нового баннера
                if (bannerID == 0)
                {
                }
                // Режим редактирования
                else if (bannerID != 0)
                {
                    int fileID = Banners.GetFileIDByBannerID(bannerID);
                    if (fileID != 0)
                    {
                        _filesRepeater.DataSource = dc.DataContext.Files.Where(f => f.FileID == fileID).OrderBy(f => f.FileName);
                        _filesRepeater.DataBind();
                    }
                }
			}
		    this.UploadFilePlaceHolder.Visible = UploadButton_Visible();
		}

        protected Boolean UploadButton_Visible()
        {
            if (_filesRepeater.Items.Count > 0)
                return false;
            else
                return true;
        }
	}
}