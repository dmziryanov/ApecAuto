<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractTermsFrame.aspx.cs" Inherits="RmsAuto.Store.Web.PrivateOffice.ContractTermsFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=global::Resources.Texts.ContractTerms %></title>
	<link rel="StyleSheet" href="<%=ResolveUrl("~/css/css_new.css")%>" type="text/css" />
</head>
<body style="margin: 10px 10px 10px 10px">
	<asp:Literal runat="server" ID="_contractTermsLiteral" />
</body>
</html>
