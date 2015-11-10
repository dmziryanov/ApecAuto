<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="SearchDiscountSpareParts.aspx.cs" Inherits="RmsAuto.Store.Web.SearchDiscountSpareParts" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="~/Controls/DiscountSpareParts.ascx" tagname="DiscountSpareParts" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:PageHeader ID="_pageHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="_leftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<h1>Результаты поиска</h1>
	
	<uc1:DiscountSpareParts ID="_discountSpareParts" runat="server" />
</asp:Content>
