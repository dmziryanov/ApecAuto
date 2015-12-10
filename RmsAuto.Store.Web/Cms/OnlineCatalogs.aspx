<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="OnlineCatalogs.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.OnlineCatalogs" Title="Untitled Page" %>

<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="Manufacturers/ManufacturerList.ascx" tagname="ManufacturerList" tagprefix="uc2" %>
<%@ Register src="~/TecDoc/Controls/TecDocManufacturers.ascx" tagname="TecDocManufacturers" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">

	<h1><asp:Literal runat="server" ID="_pageTitleLiteral" /></h1>

	<uc3:TecDocManufacturers ID="_tecDocManufacturers" runat="server" />
	<br />
</asp:Content>


