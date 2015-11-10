<%@ Page Language="C#" MasterPageFile="~/PageTwoColumnsNEW.Master" AutoEventWireup="true" CodeBehind="ManufacturerDetails.aspx.cs" Inherits="RmsAuto.Store.Web.Cms.ManufacturerDetails" 
	Title="" %>

<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc1" %>
<%@ Register src="Manufacturers/ManufacturerDetails.ascx" tagname="ManufacturerDetails" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_leftContentPlaceHolder" runat="server">
    <uc1:LeftMenu ID="_leftMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="_mainContentPlaceHolder" runat="server">
	<uc2:ManufacturerDetails ID="_manufacturerDetails" runat="server" />
</asp:Content>
