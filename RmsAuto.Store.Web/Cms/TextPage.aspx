<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="TextPage.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.TextPage" Title="Untitled Page" %>
<%--<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>--%>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<h1><asp:Literal runat="server" ID="_titleLiteral" /></h1>
	<asp:Literal runat="server" ID="_bodyLiteral" />
</asp:Content>
