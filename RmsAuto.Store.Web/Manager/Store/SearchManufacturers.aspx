<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchManufacturers.aspx.cs" Inherits="RmsAuto.Store.Web.Manager.Store.SearchManufacturers" MasterPageFile="~/Manager/ManagerNEW.Master" %>
<%@ Register src="~/Manager/Controls/HandyClientSetBar.ascx" tagname="HandyClientSetBar" tagprefix="uc1" %>
<%@ Register src="~/Controls/SparePartsManufacturers.ascx" tagname="SparePartsManufacturers" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_headerContentPlaceHolder" runat="server">
    <uc1:HandyClientSetBar ID="_handyClientSet" runat="server" />
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="_leftContentPlaceHolder">
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="_textContentPlaceHolder">
    <h1>Search results</h1>
    
    <uc1:SparePartsManufacturers ID="SparePartsManufacturers1" runat="server" />

</asp:Content>
