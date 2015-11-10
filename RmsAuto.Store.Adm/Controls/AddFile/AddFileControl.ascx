<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddFileControl.ascx.cs" Inherits="RmsAuto.Store.Adm.Controls.AddFile.AddFileControl" %>
<%@ Import Namespace="RmsAuto.Common.Web"%>

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
	function __onFileClick( file )
	{
		if( file.type=='<%=SelectType.ToString().ToLower()%>' )
		{
			<%=OnClientFileClick%>
		}
	}
	    
	function __uploadCompleted()
	{
	    window.parent.__refreshContent();
	}
</script>

<link href="<%=ResolveUrl("~/Controls/AddFile/imagemanager.css")%>" rel="stylesheet" type="text/css" />


<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
<ContentTemplate>

	<table id="explorer_table">
	<tr>
	<td>
		<div id="files_panel">
			<asp:PlaceHolder ID="UploadFilePlaceHolder" runat="server">
			    <a href="" onclick="__toggleUploadPanel();return false;" style="margin:5;<%=FolderID==0?"display:none":""%>">Загрузить файл</a>
			</asp:PlaceHolder>
			
			<div id="upload_panel" class="popup_panel" style="display:none;width:380;height:80;">
				<iframe id="Iframe1" src="<%=ResolveUrl("~/Controls/AddFile/Upload.aspx")%>?FolderID=<%=FolderID%>&id=<%=BannerID%>&RenderType=<%=this.RenderType%>" frameborder=no width=360 height=80 scrolling="no">
				</iframe>
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
						
						<asp:PlaceHolder  ID="PlaceHolder3" runat="server" Visible='<%#(bool)Eval("IsImage")&&SelectType!=null&&SelectType!=AddFileSelectType.Folder%>'>
							<a href="#" 
								onclick="__onFileClick( {type:'image',isFolder:false,isImage:true,fileID:<%#Eval("FileID")%>,fileUrl:'<%#GetFileUrl((int)Eval("FileID"))%>',fileName:'<%#Convert.ToString(Eval("FileName")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>',fileNote:'<%#Convert.ToString(Eval("FileNote")).Replace( @"\", @"\\" ).Replace( "\r", @"\r" ).Replace( "\n", @"\n" ).Replace( "\'", @"\'" )%>',imageWidth:<%#Eval("ImageWidth")%>,imageHeight:<%#Eval("ImageHeight")%>} )"
								>Выбрать</a>
						</asp:PlaceHolder >
						<asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%#!(bool)Eval("IsImage")&&SelectType==AddFileSelectType.File%>'>
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
			
			<asp:Button runat="server" ID="_refreshButton" CssClass="refresh_btn" />
			</div>
		</div>
	</td>
	</tr>
	</table>

</ContentTemplate>
</asp:UpdatePanel>