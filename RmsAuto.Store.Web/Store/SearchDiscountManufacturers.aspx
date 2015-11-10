<%@ Page Language="C#" MasterPageFile="~/PageTwoColumns.Master" AutoEventWireup="true" CodeBehind="SearchDiscountManufacturers.aspx.cs" Inherits="RmsAuto.Store.Web.SearchDiscountManufacturers" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="~/Controls/PageHeader.ascx" tagname="PageHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/DiscountManufacturers.ascx" tagname="DiscountManufacturers" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
	<uc1:PageHeader ID="_pageHeader1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
	<uc1:LeftMenu ID="_leftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="_textContentPlaceHolder" runat="server">
	<h1>Список производителей</h1>
	
	<uc1:DiscountManufacturers ID="_discountManufacturers" runat="server" />
</asp:Content>
