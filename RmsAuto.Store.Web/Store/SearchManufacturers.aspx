<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchManufacturers.aspx.cs" Inherits="RmsAuto.Store.Web.SearchManufacturers" MasterPageFile="~/PageTwoColumnsNEW.Master" %>
<%@ Register src="~/Controls/LeftMenu.ascx" tagname="LeftMenu" tagprefix="uc3" %>
<%@ Register src="~/Controls/SparePartsManufacturers.ascx" tagname="SparePartsManufacturers" tagprefix="uc1" %>

<asp:Content ID="Content2" runat="server" contentplaceholderid="_leftContentPlaceHolder">
	<uc3:LeftMenu ID="LeftMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="_mainContentPlaceHolder">
    <h1><%=global::Resources.Texts.SearchResults %></h1>
    
    <uc1:SparePartsManufacturers ID="SparePartsManufacturers1" runat="server" />
</asp:Content>
