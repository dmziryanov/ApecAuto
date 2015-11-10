<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileManagerControl.ascx.cs" Inherits="RmsAuto.Store.Adm.Controls.FileManager.FileManagerControl" %>

<%@ Import Namespace="RmsAuto.Store.Cms.Entities" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="System.Linq" %>

<script type="text/javascript">
	function __refreshContent()
	{
		$('input[name$=_refreshButton]').click();
	}
	function __toggleUploadPanel()
	{
		$('#upload_panel').toggle();
		$('#folder_panel').toggle(false);

		$('input[name$=_fileNameBox]').focus();
	}
	function __toggleFolderPanel()
	{
		$('#folder_panel').toggle();
		$('#upload_panel').toggle(false);

		$('input[name$=_folderNameBox]').focus();
	}
	
	function __onFileClick( file )
	{
		if( file.type=='<%=SelectType.ToString().ToLower()%>' )
		{
			<%=OnClientFileClick%>
		}
	}
	
</script>

<link href="<%=ResolveUrl("~/Controls/FileManager/imagemanager.css")%>" rel="stylesheet" type="text/css" />


<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
<ContentTemplate>

	<table id="explorer_table">
	<tr>
		<td colspan="2">
			<div id="cur_folder_field">
				Текущая папка: 
				<asp:Label runat="server" ID="_currentFolderLabel" Font-Bold="true"></asp:Label>
				<asp:PlaceHolder runat="server" ID="_selectFolderSpan">
					<a href="" onclick="__onFileClick( {type:'folder',isFolder:true,folderID:<%=FolderID%>,folderName:'<%=GetFolderName(FolderID).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>'} )">Выбрать</a>
				</asp:PlaceHolder>
			</div>
		</td>
	</tr>
	<tr>
	<td>
		<div id="folders_panel">
			
			<asp:TreeView ID="_treeView" runat="server" 
				OnSelectedNodeChanged="_treeView_SelectedNodeChanged"
				ImageSet="Custom"  
				ShowLines="true"
				ShowExpandCollapse="true"
				PopulateNodesFromClient="false"
				SelectedNodeStyle-Font-Bold="true">
			</asp:TreeView>

		</div>
	</td>
	<td>
		<div id="files_panel">
			<a href="" onclick="__toggleUploadPanel();return false;" style="margin:5;<%=FolderID==0?"display:none":""%>">Загрузить файл</a>
			<a href="" onclick="__toggleFolderPanel();return false;" style="margin:5;">Создать подпапку</a>
			
			
				
			
			
			<div id="upload_panel" class="popup_panel" style="display:none;width:380;height:80;">
				<iframe id="Iframe1" src="<%=ResolveUrl("~/Controls/FileManager/Upload.aspx")%>?FolderID=<%=FolderID%>" frameborder=no width=360 height=80 scrolling="no">
				</iframe>
			</div>
			<div id="folder_panel" class="popup_panel" style="display:none;width:380;height:50;">
				<table>
				<tr>
				<td>Название папки:</td>
				<td><asp:TextBox runat="server" id="_folderNameBox" CssClass="file"></asp:TextBox></td>
				</tr>
				<tr>
				<td></td>
				<td><asp:Button runat="server" ID="_createFolderBox" Text="Создать" CssClass="submit_btn" OnClick="_createFolderBox_Click" /></td>
				</tr>					
				</table>
			</div>							
			<asp:Repeater Runat="server" ID="_filesRepeater" OnItemCommand="_filesRepeater_ItemCommand">
			<HeaderTemplate>
				<table class="files">
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td valign="center" align="center" style="width:64px">
						<asp:PlaceHolder  ID="Panel1" runat="server" Visible='<%#Eval("IsImage")%>'>
							<img id="img<%#Eval("FileID")%>" src="<%#GetThumbnailUrl((int)Eval("FileID"))%>" 
							border=0 />
						</asp:PlaceHolder >
						<asp:PlaceHolder ID="Panel2" runat="server" Visible='<%#!(bool)Eval("IsImage")%>'>
							<img id="img<%#Eval("FileID")%>" src="<%#ResolveUrl(GetFileTypeImageUrl((string)Eval("FileName")))%>" 
							border=0 
							width="32" height="32" />
						</asp:PlaceHolder >
					</td>
					<td valign="top">
						<%#Server.HtmlEncode(Convert.ToString(Eval("FileName")))%><br />
						<asp:Label ID="Label1" runat="server" Visible='<%#Convert.ToString(Eval("FileNote"))!=""%>'><%#Server.HtmlEncode( Convert.ToString( Eval( "FileNote" ) ) )%><br /></asp:Label>
						<%#Math.Round(Convert.ToInt32(Eval("FileSize"))/1024m,2)%> Кб
						<asp:Label ID="Label2" runat="server" Visible='<%#Eval("IsImage")%>'> ; <%#Eval("ImageWidth")%>x<%#Eval("ImageHeight")%></asp:Label>
						<br />
						
						<asp:PlaceHolder  ID="PlaceHolder3" runat="server" Visible='<%#(bool)Eval("IsImage")&&SelectType!=null&&SelectType!=FileManagerSelectType.Folder%>'>
							<a href="#" 
								onclick="__onFileClick( {type:'image',isFolder:false,isImage:true,fileID:<%#Eval("FileID")%>,fileUrl:'<%#GetFileUrl((int)Eval("FileID"))%>',fileName:'<%#Convert.ToString(Eval("FileName")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>',fileNote:'<%#Convert.ToString(Eval("FileNote")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>',imageWidth:<%#Eval("ImageWidth")%>,imageHeight:<%#Eval("ImageHeight")%>} )"
								>Выбрать</a>
						</asp:PlaceHolder >
						<asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%#!(bool)Eval("IsImage")&&SelectType==FileManagerSelectType.File%>'>
							<a href="#" 
								onclick="__onFileClick( {type:'file',isFolder:false,isImage:false,fileID:<%#Eval("FileID")%>,fileUrl:'<%#GetFileUrl((int)Eval("FileID"))%>',fileName:'<%#Convert.ToString(Eval("FileName")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>',fileNote:'<%#Convert.ToString(Eval("FileNote")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>'} )"
								>Выбрать</a>
						</asp:PlaceHolder >

						<a href="<%#GetFileUrl((int)Eval("FileID"))%>" target="_blank">Просмотреть</a>

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
			
			<asp:Button runat="server" ID="_refreshButton" CssClass="refresh_btn" />
			</div>
		</div>
	</td>
	</tr>
	</table>

</ContentTemplate>
</asp:UpdatePanel>
