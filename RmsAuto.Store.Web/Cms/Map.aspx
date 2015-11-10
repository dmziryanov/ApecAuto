<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="Map.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.Map" Title="Untitled Page" %>
<%--<%@ OutputCache Duration="600" VaryByParam="none" %>--%>

<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>

    <div class="sitemap">
	    <asp:TreeView ID="_siteMapTreeView" runat="server" EnableViewState="false">
	    </asp:TreeView>
    </div>
</asp:Content>
