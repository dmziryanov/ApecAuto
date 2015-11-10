<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="SeoPartsCatalogPage.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.SeoPartsCatalogPage" Title="Untitled Page" %>

<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">


	<asp:Repeater runat="server" ID="_subItemsRepeater" EnableViewState="false">
	<HeaderTemplate>
		<table>
	</HeaderTemplate>
	<ItemTemplate>
		<tr><td><asp:HyperLink runat="server" ID="_hyperLink" NavigateUrl='<%# GetSubCatalogUrl(Container.DataItem) %>' Text='<%# Server.HtmlEncode( (string)Eval("Name") ) %>' /></td></tr>
	</ItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
	</asp:Repeater>

	<div>
		<asp:Literal runat="server"	ID="_bodyLiteral" />
	</div>
</asp:Content>
