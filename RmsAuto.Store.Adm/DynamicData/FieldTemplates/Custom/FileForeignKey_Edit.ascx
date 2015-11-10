<%@ Control Language="C#" CodeBehind="FileForeignKey_Edit.ascx.cs" Inherits="RmsAuto.Store.Adm.FileForeignKey_EditField" %>

<script type="text/jscript">

$(function(){
	var file = <%#GetFileJS()%>;
	if( file!=null )
		<%#ClientID%>__save( file );
	else
		<%#ClientID%>__clear();
});

function <%#ClientID%>__save( file )
{
	$('#<%#FileIDHidden.ClientID%>').val( file.fileID );
	var s = '';
	if( file.isImage )
	{
		s += '<img src="'+'<%#GetThumbnailUrl(12345)%>'.replace('12345',file.fileID)+'">';
	}
	else
	{
		s += '<a target="_blank" href="'+'<%#GetFileUrl(12345)%>'.replace('12345',file.fileID)+'">'+file.fileName+'</a>';
	}
	$('#<%#ImageDiv.ClientID%>').html(s);
}
function <%#ClientID%>__clear()
{
	$('#<%#FileIDHidden.ClientID%>').val( '' );
	$('#<%#ImageDiv.ClientID%>').html('[не выбран]');
}


</script>
<asp:HiddenField runat="server" ID="FileIDHidden" />

<span runat="server" id="ImageDiv"></span>

<input type="button"  value="Выбрать" onclick="mso.imageManagerPopup.select('image',<%=ClientID%>__save,null)" />
<input type="button"  value="Сброс" onclick="<%#ClientID%>__clear();" />
