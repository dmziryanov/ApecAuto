<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FloatingSearchForm.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.Store.FloatingSearchForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Поиск автозапчастей</title>
    <script type="text/javascript" language="javascript">
		function doSearch()
		{
			var pn = document.getElementById('<%=txtPartNumber.ClientID %>').value;
			var searchCrosses = document.getElementById('<%=cbSearchCrosses.ClientID %>').checked;
			window.opener.FloatingSearchForm_Callback( 
				{ partNumber : pn, searchCrosses : searchCrosses } 
				);
		}
    </script>
	<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_new.css")%>" type="text/css"></link>
</head>
<body class="body" style="padding: 0px 15px 0px 15px">
	<form name="frm" action="">
		<img id="img1" runat="server" src="~/images/search_img.jpg" alt="Поиск автозапчастей" width="219" height="49" border="0" /><div class="search" onkeypress="if(event.keyCode==13) { doSearch(); return false; }">
			<div id="partNumberError" class="error" style="display:none" runat="server"></div>
			По коду
			<input type="text" id="txtPartNumber" runat="server" class="code" /><br />
			<img id="img2" runat="server" src="~/images/1pix.gif"  width="1" height="5" border="0" /><br />
			<input type="checkbox" id="cbSearchCrosses" runat="server" checked />Search analogs<br /><br />
			<img id="img3" runat="server" src="~/images/search_btn.jpg" alt="" width="187" height="22" border="0" onclick="doSearch()" style="cursor:pointer;">
		</div>
	</form>
	
    <script type="text/javascript" language="javascript">
		document.getElementById('<%=txtPartNumber.ClientID %>').focus();
    </script>
</body>
</html>