<%@ Page ValidateRequest="false" Language="c#" %>
<%@ Import Namespace="RmsAuto.Store.Cms.Entities" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="System.Linq" %>

<script runat="server">

	int FolderID
	{
		get { return Convert.ToInt32( _treeView.SelectedValue ); }
	}
	
	string GetFileUrl( int fileID )
	{
		return string.Format( "{0}/Files/{1}.ashx",
			ConfigurationManager.AppSettings[ "WebSiteUrl" ],
			fileID );
	}

	string GetFileTypeImageUrl( string fileName )
	{
		string ext = System.IO.Path.GetExtension( fileName ).TrimStart( '.' );
		string path = "~/images/Files/" + ext + ".png";
		if( !System.IO.File.Exists( Server.MapPath( path ) ) )
			path = "~/images/Files/unknown.png";
		return ResolveUrl( path );
	}
	
	protected override void OnLoad( EventArgs e )
	{
		Response.Cache.SetCacheability( HttpCacheability.NoCache );
		base.OnLoad( e );

		if( !IsPostBack )
		{
			TreeNode root = new TreeNode( "Библиотека документов", "0" );
			root.SelectAction = TreeNodeSelectAction.SelectExpand;
			_treeView.Nodes.Add( root );

			using( CmsDataContext dc = new CmsDataContext() )
			{
				AddChildren( root, dc.Folders.Where( f => f.ParentID == null ).OrderBy( f => f.FolderName ).ToArray() );
			}
			
			_treeView.CollapseAll();
			root.Expand();
			root.Select();
		}	
	}

	protected override void OnPreRender( EventArgs e )
	{
		base.OnPreRender( e );
		ReloadFiles();
		_noImagesLabel.Visible = FolderID != 0 && _filesRepeater.Items.Count == 0;
		_deleteFolderButton.Visible = FolderID != 0 && _filesRepeater.Items.Count == 0 && _treeView.SelectedNode.ChildNodes.Count == 0;
	}

	void _createFolderBox_Click( object sender, EventArgs e )
	{
		using( CmsDataContext dc = new CmsDataContext() )
		{
			if( string.IsNullOrEmpty(_folderNameBox.Text.Trim()) )
				throw new Exception("");
			
			Folder folder = new Folder();
			folder.ParentID = FolderID != 0 ? (int?)FolderID : null;
			folder.FolderName = _folderNameBox.Text.Trim();
			folder.FolderCreationDate = DateTime.Now;
			dc.Folders.InsertOnSubmit( folder );
			dc.SubmitChanges();

			TreeNode node = new TreeNode( folder.FolderName, folder.FolderID.ToString() );
			node.SelectAction = TreeNodeSelectAction.SelectExpand;
			_treeView.SelectedNode.ChildNodes.Add( node );
		}
	}
	void _deleteFolderButton_Click( object sender, EventArgs e )
	{
		using( CmsDataContext dc = new CmsDataContext() )
		{
			Folder folder = dc.Folders.Where( f => f.FolderID == FolderID ).FirstOrDefault();
			if( folder != null )
			{
				if( folder.Folders.Count != 0 )
					throw new Exception( "1" );
				if( folder.Files.Count!=0 )
					throw new Exception( "2" );
				dc.Folders.DeleteOnSubmit( folder );
				dc.SubmitChanges();
				
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
			parentNode.ChildNodes.Add( node );

			AddChildren( node, folder.Folders.OrderBy( f => f.FolderName ).ToArray() );
		}
	}

	void _treeView_SelectedNodeChanged( object sender, EventArgs e )
	{
		//ReloadFiles();
	}
	void _filesRepeater_ItemCommand( object source, RepeaterCommandEventArgs e )
	{
		if( e.CommandName == "Delete" )
		{
			using( CmsDataContext dc = new CmsDataContext() )
			{
				File file = dc.Files.Where( f => f.FileID == Convert.ToInt32( e.CommandArgument ) ).FirstOrDefault();
				if( file != null )
				{
					dc.Files.DeleteOnSubmit( file );
					dc.SubmitChanges();
					//ReloadFiles();
				}
			}
		}
	}

	void ReloadFiles()
	{
		using( CmsDataContext dc = new CmsDataContext() )
		{
			_filesRepeater.DataSource = dc.Files.Where( f => f.FolderID == FolderID ).OrderBy( f => f.FileName );
			_filesRepeater.DataBind();
		}
	}


</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Библиотека документов</title>
	<script type="text/javascript" src="../../tiny_mce_popup.js"></script>
	<script type="text/javascript" src="../../utils/mctabs.js"></script>
	<script type="text/javascript" src="../../utils/form_utils.js"></script>
	<script type="text/javascript" src="../../utils/validate.js"></script>
	<script type="text/javascript" src="../../utils/editable_selects.js"></script>
	<script type="text/javascript" src="js/imagemanager.js"></script>
	<link href="css/imagemanager.css" rel="stylesheet" type="text/css" />
	<base target="_self" />
	<script type="text/javascript">
		function __refreshContent()
		{
			document.getElementById('<%=_refreshButton.UniqueID%>').click();
		}
		function __toggleUploadPanel(state)
		{
			var panel = document.getElementById('upload_panel');
			var prevState = panel.style.display=='block';
			if( state==null ) state = !prevState;
			if( state )
			{
				__toggleFolderPanel(false);
				panel.style.display='block';
			}
			else
			{
				panel.style.display='none';
			}
		}
		function __toggleFolderPanel(state)
		{
			var panel = document.getElementById('folder_panel');
			var prevState = panel.style.display=='block';
			if( state==null ) state = !prevState;
			if( state )
			{
				__toggleUploadPanel(false);
				panel.style.display='block';
				document.getElementById('<%=_folderNameBox.UniqueID%>').focus();
			}
			else
			{
				panel.style.display='none';
			}
		}
		
	</script>
</head>
<body id="imagemanager" style="display: none">
    <form runat="server" id="Form1" method="post"> 

	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
	<ContentTemplate>

		<table id="explorer_table">
		<tr>
		<td>
			<div id="folders_panel">
				
				<asp:TreeView ID="_treeView" runat="server" 
					OnSelectedNodeChanged="_treeView_SelectedNodeChanged"
					ImageSet="XPFileExplorer" 
					PopulateNodesFromClient="false"
					SelectedNodeStyle-Font-Bold="true">
				</asp:TreeView>
								
			</div>
		</td>
		<td>
			<div id="files_panel">
				<a href="" onclick="__toggleUploadPanel(null);return false;" style="margin:5;<%=FolderID==0?"display:none":""%>">Загрузить файл</a>
				<a href="" onclick="__toggleFolderPanel(null);return false;" style="margin:5;">Создать подпапку</a>
				
				<div id="upload_panel" style="display:none;width:360;height:80;border: solid 1px #808080; margin: 5">
					<iframe id="Iframe1" src="Upload.aspx?FolderID=<%=FolderID%>" frameborder=no width=360 height=80 scrolling="no">
					</iframe>
				</div>
				<div id="folder_panel" style="display:none;width:360;height:50;border: solid 1px #808080;background-color: #F0F0EE;font-family: Tahoma, Arial, Sans-Serif; font-size: 8pt; margin: 5">
					<table>
					<tr>
					<td>Название папки:</td>
					<td><asp:TextBox runat="server" id="_folderNameBox"></asp:TextBox></td>
					</tr>
					<tr>
					<td></td>
					<td><asp:Button runat="server" ID="_createFolderBox" Text="Создать" CssClass="submit_btn" OnClick="_createFolderBox_Click" /></td>
					</tr>					
					</table>
				</div>							
				<asp:Repeater Runat="server" ID="_filesRepeater" OnItemCommand="_filesRepeater_ItemCommand">
				<HeaderTemplate>
					<table style="margin: 5">
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td valign="center" align="center">
							<asp:Panel runat="server" Visible='<%#Eval("IsImage")%>'>
								<img id="img<%#Eval("FileID")%>" src="<%#GetFileUrl((int)Eval("FileID"))%>" 
								border=0 
								onclick="ImageManagerDialog.selectImage('<%#GetFileUrl((int)Eval("FileID"))%>','<%#Convert.ToString(Eval("FileNote")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>')" 
								style="cursor:hand;<%# Convert.ToDouble(Eval("ImageWidth"))/Convert.ToDouble(Eval("ImageHeight")) < 1 ? "height:64" : "width:64" %>">
							</asp:Panel>
							<asp:Panel ID="Panel1" runat="server" Visible='<%#!(bool)Eval("IsImage")%>'>
								<img id="img<%#Eval("FileID")%>" src="<%#GetFileTypeImageUrl((string)Eval("FileName"))%>" 
								border=0 
								width="32" height="32"
								onclick="ImageManagerDialog.selectImage('<%#GetFileUrl((int)Eval("FileID"))%>','<%#Convert.ToString(Eval("FileNote")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>')" 
								style="cursor:hand">
							</asp:Panel>
						</td>
						<td valign="top">
							<a href="#" onclick="document.getElementById('img<%#Eval("FileID")%>').onclick();return false;"><%#Server.HtmlEncode(Convert.ToString(Eval("FileName")))%></a><br />
							<asp:Label ID="Label1" runat="server" Visible='<%#Convert.ToString(Eval("FileNote"))!=""%>'><%#Server.HtmlEncode( Convert.ToString( Eval( "FileNote" ) ) )%><br /></asp:Label>
							<asp:Label ID="Label2" runat="server" Visible='<%#Eval("IsImage")%>'><%#Eval("ImageWidth")%>x<%#Eval("ImageHeight")%><br /></asp:Label>
							<%#Math.Round(Convert.ToInt32(Eval("FileSize"))/1024m,2)%> Кб<br />
							
							<asp:LinkButton runat="server" ID="LinkButton1" CommandName="Delete" CommandArgument='<%#Eval("FileID")%>' OnClientClick="return confirm('Удалить файл?')">Удалить</asp:LinkButton>
						</td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
				</FooterTemplate>
				</asp:Repeater>
			
				<div style="margin:5">
						
				<asp:Label runat="server" ID="_noImagesLabel">Нет файлов<br /></asp:Label>
				<asp:LinkButton runat="server" ID="_deleteFolderButton" OnClientClick="return confirm('Удалить папку');" OnClick="_deleteFolderButton_Click">Удалить папку</asp:LinkButton>
				
				<asp:Button runat="server" ID="_refreshButton" Width="0" Height="0" />
				</div>
			</div>
		</td>
		</tr>
		</table>


	</ContentTemplate>
	</asp:UpdatePanel>

    </form>
</body> 
</html> 
