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
using RmsAuto.Store.Cms.BL;
using RmsAuto.Store.Adm.scripts;
using System.Collections.Generic;
using System.Text;
using RmsAuto.Store.Cms.Dac;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm.Controls.FileManager
{
	public partial class FileManagerControl : System.Web.UI.UserControl
	{
		public enum FileManagerSelectType
		{
			Image,
			File,
			Folder
		}

		public FileManagerSelectType? SelectType { get; set; }
		public string OnClientFileClick { get; set; }

		protected int FolderID
		{
			get { return Convert.ToInt32( _treeView.SelectedValue ); }
		}

		public static string GetFileUrl( int fileID )
		{
			return string.Format( "{0}/Files/{1}.ashx",
				ConfigurationManager.AppSettings[ "WebSiteUrl" ],
				fileID );
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

			if( !IsPostBack )
			{
				TreeNode root = new TreeNode( "Библиотека документов", "0" );
				root.SelectAction = TreeNodeSelectAction.SelectExpand;
				root.ImageUrl = "~/Controls/FileManager/root.gif";
				_treeView.Nodes.Add( root );

                using (var dc = new DCFactory<CmsDataContext>())
				{
					AddChildren( root, dc.DataContext.Folders.Where( f => f.ParentID == null ).OrderBy( f => f.FolderName ).ToArray() );
				}

				_treeView.CollapseAll();
				root.Expand();
				root.Select();
			}
		}

		protected string GetFolderName( int folderID )
		{
			return FilesDac.GetFolderPath( folderID ) ?? "";
		}
		protected void Page_PreRender( object sender, EventArgs e )
		{

			ReloadFiles();

			_selectFolderSpan.Visible = FolderID != 0 && SelectType == FileManagerSelectType.Folder;

			_noImagesLabel.Visible = FolderID != 0 && _filesRepeater.Items.Count == 0;
			_deleteFolderButton.Visible = FolderID != 0 && _filesRepeater.Items.Count == 0 && _treeView.SelectedNode.ChildNodes.Count == 0;
		}

		protected void _createFolderBox_Click( object sender, EventArgs e )
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				if( string.IsNullOrEmpty( _folderNameBox.Text.Trim() ) )
					throw new Exception( "" );

				Folder folder = new Folder();
				folder.ParentID = FolderID != 0 ? (int?)FolderID : null;
				folder.FolderName = _folderNameBox.Text.Trim();
				folder.FolderCreationDate = DateTime.Now;
				dc.DataContext.Folders.InsertOnSubmit( folder );
				dc.DataContext.SubmitChanges();

				TreeNode node = new TreeNode( folder.FolderName, folder.FolderID.ToString() );
				node.SelectAction = TreeNodeSelectAction.SelectExpand;
				node.ImageUrl = "~/Controls/FileManager/folder-closed.gif";
				_treeView.SelectedNode.ChildNodes.Add( node );
			}
		}
		protected void _deleteFolderButton_Click( object sender, EventArgs e )
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				Folder folder = dc.DataContext.Folders.Where( f => f.FolderID == FolderID ).FirstOrDefault();
				if( folder != null )
				{
					if( folder.Folders.Count != 0 )
						throw new Exception( "1" );
					if( folder.Files.Count != 0 )
						throw new Exception( "2" );
					dc.DataContext.Folders.DeleteOnSubmit( folder );
					dc.DataContext.SubmitChanges();

					TreeNode node = _treeView.SelectedNode;
					node.Parent.Select();
					node.Parent.ChildNodes.Remove( node );
				}
			}
		}
		void AddChildren( TreeNode parentNode, Folder[] folders )
		{
			foreach( Folder folder in folders )
			{
				TreeNode node = new TreeNode( folder.FolderName, folder.FolderID.ToString() );
				node.SelectAction = TreeNodeSelectAction.SelectExpand;
				node.ImageUrl = "~/Controls/FileManager/folder-closed.gif";
				parentNode.ChildNodes.Add( node );

				AddChildren( node, folder.Folders.OrderBy( f => f.FolderName ).ToArray() );
			}
		}

		protected void _treeView_SelectedNodeChanged( object sender, EventArgs e )
		{
			//ReloadFiles();
		}
		protected void _filesRepeater_ItemCommand( object source, RepeaterCommandEventArgs e )
		{
			if( e.CommandName == "Delete" )
			{
                using (var dc = new DCFactory<CmsDataContext>())
				{
					File file = dc.DataContext.Files.Where( f => f.FileID == Convert.ToInt32( e.CommandArgument ) ).FirstOrDefault();
					if( file != null )
					{
						dc.DataContext.Files.DeleteOnSubmit( file );
						dc.DataContext.SubmitChanges();
						//ReloadFiles();
					}
				}
			}
		}

		void ReloadFiles()
		{
            using (var dc = new DCFactory<CmsDataContext>())
			{
				_filesRepeater.DataSource = dc.DataContext.Files.Where( f => f.FolderID == FolderID ).OrderBy( f => f.FileName );
				_filesRepeater.DataBind();

				List<Folder> folders = new List<Folder>();
				for( Folder folder = dc.DataContext.Folders.SingleOrDefault( f => f.FolderID == FolderID ) ; folder != null ; folder = folder.Parent )
				{
					folders.Add( folder );
				}
				folders.Reverse();
				StringBuilder sb = new StringBuilder();
				foreach( Folder folder in folders )
				{
					if( sb.Length != 0 ) sb.Append( " / " );
					sb.Append( folder.FolderName );
				}
				_currentFolderLabel.Text = sb.Length!=0 ? sb.ToString() : "Библиотека документов";

			}
		}

	}


}