<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="StockSuppliers.aspx.cs" Inherits="RmsAuto.Store.Web.Store.StockSuppliers" Title="Untitled Page" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:PageHeader ID="_pageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	
	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" OnInit="pageTitleLiteralInit" /></h1>
	
	<asp:Repeater ID="rptMfrs" runat="server" OnItemDataBound="rptMfrsOnItemDataBound">
	    <ItemTemplate>
	        <asp:Placeholder runat="server" Visible="<%# !this.CurChar.Equals((Container.DataItem as string)[0]) %>">
	        <b><%# (Container.DataItem as string)[0] %>...</b><br />
	        </asp:Placeholder>
	        <asp:HyperLink runat="server" NavigateUrl="<%# UrlManager.GetStockSupplierPartsPage(Container.DataItem as string) %>"><%# Server.HtmlEncode(Container.DataItem as string) %></asp:HyperLink><br />
	        </ItemTemplate>
	</asp:Repeater>

</asp:Content>
