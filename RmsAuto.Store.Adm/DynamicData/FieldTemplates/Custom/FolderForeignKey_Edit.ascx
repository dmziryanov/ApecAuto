<%@ Control Language="C#" CodeBehind="FolderForeignKey_Edit.ascx.cs" Inherits="RmsAuto.Store.Adm.FolderForeignKey_EditField" %>


<script type="text/jscript">

$(function(){
	var file = <%#GetFolderJS()%>;
	if( file!=null )
		<%#ClientID%>__save( file );
	else
		<%#ClientID%>__clear();
});

function <%#ClientID%>__save( file )
{
	$('#<%#FolderIDHidden.ClientID%>').val( file.folderID );
	var s = file.folderName;
	$('#<%#ImageDiv.ClientID%>').html(s);
}
function <%#ClientID%>__clear()
{
	$('#<%#FolderIDHidden.ClientID%>').val( '' );
	$('#<%#ImageDiv.ClientID%>').html('[не выбрано]');
}


</script>
<asp:HiddenField runat="server" ID="FolderIDHidden" />

<span runat="server" id="ImageDiv"></span>

<input type="button"  value="Выбрать" onclick="mso.imageManagerPopup.select('folder',<%=ClientID%>__save,null)" />
<input type="button"  value="Сброс" onclick="<%#ClientID%>__clear();" />
