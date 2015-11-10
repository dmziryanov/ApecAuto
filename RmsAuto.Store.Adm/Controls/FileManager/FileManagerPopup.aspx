<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileManagerPopup.aspx.cs" Inherits="RmsAuto.Store.Adm.Controls.FileManager.FileManagerPopup" %>
<%@ Register src="FileManagerControl.ascx" tagname="FileManagerControl" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Менеджер файлов</title>
    <link rel="Stylesheet" href="imagemanager.css" type="text/css" />
    <script language="javascript">
		var __popupWindow = window.parent.mso.imageManagerPopup.__popupWindow;
		
		function _saveFile( file )
		{
			__popupWindow.onSave( file );
		}
		function _closeButtonClick()
		{
			__popupWindow.onCancel();
		}
	
	</script>
</head>
<body style="margin:0 0 0 0">
	
	<table style="margin:0 0 0 0;width:100%;height:100%;border:outset 1px">
	<tr>
	<td><b>Менеджер файлов</b> - <asp:Literal runat="server" ID="_subTitleLiteral"></asp:Literal></td>
	<td align="right"><img id="_popupEditorCloseButton" alt="Закрыть" src="close.gif" style="cursor:hand" onclick="return _closeButtonClick();" /></td>
	</tr>
	<tr>
	<td colspan="2">

    <form id="form1" runat="server" style="margin:0 0 0 0">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		
    	<uc1:FileManagerControl ID="FileManagerControl1" runat="server" 
    	 OnClientFileClick="_saveFile(file)"
    	/>
    	
  
    </form>
	</td>
	</tr>
	
	</table>
</body>
</html>
