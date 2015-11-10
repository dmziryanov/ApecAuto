<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SparePart.aspx.cs" Inherits="RmsAuto.Store.Web.SparePartPage" %>

<%@ Register src="~/Controls/SparePartView.ascx" tagname="SparePartView" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>APEC</title>
	<link rel="StyleSheet" href="<%=ResolveUrl("~/css/style.css")%>" type="text/css"/>
    <!--[if lte IE 8]>
	<link rel="StyleSheet" href="<%= ResolveUrl("~/css/ie8.css")%>" type="text/css" media="screen" />
	<script src="<%= ResolveUrl("~/Scripts/html5support.js") %>" type="text/javascript"></script>
	<![endif]-->

	<script src="<%= ResolveUrl("~/Scripts/jquery-1.11.1.min.js") %>" type="text/javascript"></script>
	<script src="<%= ResolveUrl("~/Scripts/common.js") %>" type="text/javascript"></script>
</head>
<body class="body">
    <form id="form1" runat="server">
    <div class="win-bg">
    	<uc1:SparePartView ID="_sparePartView" runat="server" />
    	<br/>
        <div class="text-right"><a href="javascript:self.close()" class="button"><%=global::Resources.Texts.Close %></a></div>
    </div>
    </form>
</body>
</html>
