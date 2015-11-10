<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplierStatistic.aspx.cs" Inherits="RmsAuto.Store.Web.SupplierStatisticPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title><%=global::Resources.Texts.SupplierStat %></title>
	<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_new.css")%>" type="text/css"></link>
</head>
<body class="body" style="padding:15px;">
    <form id="form1" runat="server">
    <div>
    	<asp:Image ID="ChartImage" runat="server" />
    </div>
    <span class="right_block"><a href="javascript:self.close()"><%=global::Resources.Texts.Close %></a></span>
    </form>
</body>
</html>
